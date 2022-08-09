#include <ArduinoJson.h>
#include <SPI.h>
#include <MFRC522.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#define RST_PIN         D0           // Configurable, see typical pin layout above
#define SS_PIN          D8         // Configurable, see typical pin layout above
#define Relay 27
const char* ssid     = "Sheesh";
const char* password = "Gtorrecillas1435";
char myBuffer;
String bufferString;
String serverName = "http://controlaccessapp.azurewebsites.net/api/check";
const char* mqttServer = "broker.hivemq.com"; 
const char* topic = "esp32/toggle";
const int mqttPort = 1883;
char cardNumber[0];
const char* statusPayload;
const char* messagePayload; 
const char* dataPayload;
HTTPClient http;
String payload;
String statusCode;
const int BUZZER = 4;
//const int BUZZER2 = 2;
StaticJsonDocument<300> doc2;
StaticJsonDocument<200> doc;
WiFiClient espClient;
PubSubClient client(espClient);

MFRC522 mfrc522(SS_PIN, RST_PIN);   // Create MFRC522 instance

JsonObject message = doc.to<JsonObject>();

void wifiInit() {
    Serial.print("Conectándose a ");
    Serial.println(ssid);

    WiFi.begin(ssid, password);

    while (WiFi.status() != WL_CONNECTED) {
      Serial.print(".");
        delay(500);  
    }
    Serial.println("");
    Serial.println("Conectado a WiFi");
    Serial.println(ssid);
}
  
void setup()
{
  pinMode(Relay,OUTPUT);
  Serial.begin(9600);
  SPI.begin();                                                  // Init SPI bus
  mfrc522.PCD_Init();                                              // Init MFRC522 
  wifiInit();
  http.begin(serverName);
  client.setServer(mqttServer, mqttPort);
  client.setCallback(callback);
 while (!client.connected()) {
     String client_id = "arduino client ";
     client_id += String(WiFi.macAddress());
    Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
     if (client.connect(client_id.c_str())) {
        Serial.println("Conectao");
        
     } else {
         Serial.print("failed with state ");
         Serial.print(client.state());
         delay(2000);
     }
 }
 client.subscribe(topic);
}

void callback(char *topic, byte *payloadMqtt, unsigned int length) {
 String response;
 Serial.print("Message arrived in topic: ");
 Serial.println(topic);
 Serial.print("Message:");
 for (int i = 0; i < length; i++) {
     response +=(char) payloadMqtt[i];
 }
 if (response == "true"){
  digitalWrite(Relay, LOW);
 } else if(response == "false"){
  digitalWrite(Relay, HIGH);
 }

}



void loop()
{
 client.loop();
http.addHeader("Content-Type", "application/json");
MFRC522::MIFARE_Key key;
  for (byte i = 0; i < 6; i++) key.keyByte[i] = 0xFF;

  //some variables we need
  byte block;
  byte len;
  MFRC522::StatusCode status;

  //-------------------------------------------

  // Reset the loop if no new card present on the sensor/reader. This saves the entire process when idle.

  if ( ! mfrc522.PICC_IsNewCardPresent()) {
    return;
  }

  // Select one of the cards
  if ( ! mfrc522.PICC_ReadCardSerial()) {
    return;
  }

  Serial.println(F("*Card Detected:*"));

  //-------------------------------------------

  mfrc522.PICC_DumpDetailsToSerial(&(mfrc522.uid)); //dump some details about the card

  //mfrc522.PICC_DumpToSerial(&(mfrc522.uid));      //uncomment this to see all blocks in hex

  //-------------------------------------------

  Serial.print(F("Card Number: "));

  byte buffer1[18];

  block = 4;
  len = 18;

  //------------------------------------------- GET CARD NUMBER
  status = mfrc522.PCD_Authenticate(MFRC522::PICC_CMD_MF_AUTH_KEY_A, 4, &key, &(mfrc522.uid)); //line 834 of MFRC522.cpp file
  if (status != MFRC522::STATUS_OK) {
    Serial.print(F("Authentication failed: "));
    Serial.println(mfrc522.GetStatusCodeName(status));
    return;
  }

  status = mfrc522.MIFARE_Read(block, buffer1, &len);
  if (status != MFRC522::STATUS_OK) {
    Serial.print(F("Reading failed: "));
    Serial.println(mfrc522.GetStatusCodeName(status));
  }

  //PRINT CARD NUMBER
  for (uint8_t i = 0; i < 16; i++)
  {
    if (buffer1[i] != 32)
    {
      
      myBuffer = (char)buffer1[i];
      bufferString += myBuffer;
      
    }
  }
 bufferString.toCharArray(cardNumber,11);
 Serial.println(cardNumber);
 message["serial"] = cardNumber;
 message["device"] = "Entrance A";
 char json[100];
 serializeJson(message,json);
 Serial.print("Posting to API: ");
 Serial.println(json);

 // Initialize http client

 int httpCode = http.POST(json);

 if (httpCode > 0 ){
  payload = http.getString();
  Serial.println("\nStatus code: " + String(httpCode));
  Serial.println(payload);
  deserializeJson(doc2, payload);
  statusPayload = doc2["status"]; // "ERROR"
  messagePayload = doc2["message"]; // "Exception on (ControlAccessDAL.SetCheck) - Details() - Error ...
  dataPayload = doc2["data"]; // "System.ArgumentException"
  Serial.println(statusPayload);
  Serial.println(messagePayload);
  Serial.println(payload);
  statusCode = String(statusPayload);

 }
 else {
  String payload = http.getString();
  Serial.print("Eror code: ");
  Serial.println(payload);
  deserializeJson(doc2, payload);
  statusPayload = doc2["status"]; // "ERROR"
  messagePayload = doc2["message"]; // "Exception on (ControlAccessDAL.SetCheck) - Details() - Error ...
  dataPayload = doc2["data"]; // "System.ArgumentException"
  Serial.println(statusPayload);
  Serial.println(messagePayload);
  Serial.println(dataPayload);
  statusCode = String(statusPayload);
 }

if(statusCode == "OK"){
  Serial.println("UWU OK");
  tone(BUZZER, 9,500);
  delay(200);
  digitalWrite(BUZZER, LOW);
}
else if (statusCode == "ERROR")
{
  Serial.println("UWU ERROR");
  tone(BUZZER, 9,500);
  delay(200);
  digitalWrite(BUZZER, LOW);
  tone(BUZZER, 9,500);
  delay(200);
  digitalWrite(BUZZER, LOW);
  tone(BUZZER, 9,500);
  delay(200);
  digitalWrite(BUZZER, LOW);
  tone(BUZZER, 9,500);
  delay(200);
  digitalWrite(BUZZER, LOW);
  
}
// Free resources

  mfrc522.PICC_HaltA();
  mfrc522.PCD_StopCrypto1();
  bufferString = "";
  http.end();
  
}

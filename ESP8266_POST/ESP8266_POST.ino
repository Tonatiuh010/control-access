#include <ArduinoJson.h>
#include <SPI.h>
#include <MFRC522.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#define RST_PIN         D0           // Configurable, see typical pin layout above
#define SS_PIN          D8          // Configurable, see typical pin layout above


const char* ssid     = "Sheesh";
const char* password = "Gtorrecillas1435";
char myBuffer;
String bufferString;
String serverName = "https://controlaccess20220725234915.azurewebsites.net/api/card";
char cardNumber[0];
HTTPClient http;

MFRC522 mfrc522(SS_PIN, RST_PIN);   // Create MFRC522 instance
StaticJsonDocument<200> doc;
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
  Serial.begin(9600);
  SPI.begin();                                                  // Init SPI bus
  mfrc522.PCD_Init();                                              // Init MFRC522 
  wifiInit();
  http.begin(serverName);
}

void loop()
{
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

  Serial.println(F("**Card Detected:**"));

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
    return;
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
 message["serial"] = cardNumber;
 char json[100];
 serializeJson(message,json);
 Serial.print("Posting to API: ");
 Serial.println(json);

 // Initialize http client




 int httpCode = http.POST(json);

 if (httpCode > 0 ){
  String payload = http.getString();
  Serial.println("\nStatus code: " + String(httpCode));
  Serial.println(payload);
 }
 else {
  String payload = http.getString();
  Serial.print("Eror code: ");
  Serial.println(payload);
 }
// Free resources
 

  mfrc522.PICC_HaltA();
  mfrc522.PCD_StopCrypto1();
  bufferString = "";
  http.end();
  
 
//  delay(1000);
 
}

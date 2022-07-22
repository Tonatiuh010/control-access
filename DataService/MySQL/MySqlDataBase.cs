using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DataService.MySQL
{    
    public class MySqlDataBase {
        /*
        Pending to add 
            Reconnect Feature, sets a timer to reconnect
            Safe Block - transaction implementation        
        */

        // Delegates
        public delegate void TransactionCallback(MySqlTransaction txn);
        public delegate void DataException(Exception e, string customMsg = "");

        // Properties
        public static DataException? OnException {get; set;}
        public MySqlConnection Connection { get;  set; } = new MySqlConnection();   
        
        // Constructor
        public MySqlDataBase(string connString) {
            try {
                OpenConnection(connString);             
            } catch (Exception ex) {                
                if(OnException != null) {
                    if (ex.GetType() == typeof(MySqlException) ) {
                    MySqlException e = (MySqlException)ex;                    
                    switch (e.Number) {
                        case 0:                            
                            OnException(ex, "Cannot connect to server.  Contact administrator");
                            break;
                        case 1045:
                            OnException(ex, "Invalid username/password, please try again");
                            break;
                    }
                } else {                    
                    OnException(ex, "Exception creating connection...");
                }
                }                
            }
        }

        public void OpenConnection(string connString) {
            Connection = new MySqlConnection(connString);                        
            Connection.StateChange += OnStateChange;
            Connection.Open();
        }

        public void CloseConnection() {
            if(Connection.State == ConnectionState.Open) {                
                Connection.Close();
                Connection.Dispose();
            }
        }

        public MySqlCommand CreateCommand(string cmdText, MySqlTransaction txn, CommandType type) 
        => CreateCommand(cmdText, Connection, txn, type);
        
        // Static Methods
        private static void OnStateChange(object obj, StateChangeEventArgs args) {
            if(args.CurrentState == ConnectionState.Closed){
                // This will be useful on the future
            }            
        }

        public static MySqlCommand CreateCommand(string cmdText, MySqlConnection conn, MySqlTransaction txn, CommandType type)  
        => new MySqlCommand(cmdText, conn, txn){
            CommandType = type
        };

        public static IDataParameter CreateParameter(string name, object value, MySqlDbType type, bool isNullable = true)  => new MySqlParameter(name, value) {
            Direction = ParameterDirection.Input,
            MySqlDbType = type,
            IsNullable = isNullable
        };
                
        public static IDataParameter CreateParameterOut(string name, MySqlDbType type) => new MySqlParameter() {
            ParameterName = name, 
            Direction = ParameterDirection.Output,
            MySqlDbType = type
        };

        public static void TransactionBlock(MySqlConnection conn, TransactionCallback action, DataException onException) {
            MySqlTransaction? txn = null;
            try {
                txn = conn.BeginTransaction();                
                action(txn);
                txn.Commit();
            } catch (Exception e) {
                if(txn != null)
                    txn.Rollback();
                onException(e);
            }
        }
    }
}
/*
=> TransactionBlock(
            Connection,
            txn => {
                Connection = new MySqlConnection();
                Connection.ConnectionString = connString;
                Connection.Open();
            },
            exception => {
                if (exception.GetType() == typeof(MySqlException) ) {
                    MySqlException e = (MySqlException)exception;
                    switch (e.Number) {
                        case 0:
                            Console.WriteLine("Cannot connect to server.  Contact administrator");
                            break;
                        case 1045:
                            Console.WriteLine("Invalid username/password, please try again");
                            break;
                    }
                }
                Console.WriteLine(exception.Message);
            } 
        );
*/
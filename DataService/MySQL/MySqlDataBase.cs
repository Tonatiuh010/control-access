using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DataService.MySQL
{
    public class MySqlDataBase //: IDisposable
    {
        
        // Delegates
        public delegate void TransactionCallback(/*MySqlTransaction txn*/);
        public delegate void DataException(Exception e, string customMsg = "");
        public delegate void ReaderAction(IDataReader reader);

        // Properties
        public static DataException? OnException { get; set; }
        public MySqlConnection Connection { get; set; } = new MySqlConnection();        
        private string? ConnectionString { get; set; }

        // Constructor
        public MySqlDataBase(string? connString)
        {
            try
            {
                ConnectionString = connString;
                CreateConnection();                    
            }
            catch (Exception ex)
            {
                if (OnException != null)
                {
                    if (ex.GetType() == typeof(MySqlException))
                    {
                        MySqlException e = (MySqlException)ex;
                        switch (e.Number)
                        {
                            case 0:
                                OnException(ex, "Cannot connect to server.  Contact administrator");
                                break;
                            case 1045:
                                OnException(ex, "Invalid username/password, please try again");
                                break;
                        }
                    }
                    else
                    {
                        OnException(ex, "Exception creating connection...");
                    }
                }
            }
        }

        public void OpenConnection() => OpenConnection(this);

        public void CreateConnection() {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new Exception("MySQL Connection string is empty");
            }

            Connection = new MySqlConnection(ConnectionString);
            Connection.StateChange += OnStateChange;
        }

        public void CloseConnection()
        {          
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }            
        }

        public MySqlCommand CreateCommand(string cmdText, CommandType type)
        => CreateCommand(cmdText, Connection, type);

        // Static Methods
        private void OnStateChange(object obj, StateChangeEventArgs args)
        {
            if (args.CurrentState == ConnectionState.Closed)
            {
                //OpenConnection();
            }
        }      

        public static MySqlCommand CreateCommand(string cmdText, MySqlConnection conn, CommandType type) 
        => new (cmdText, conn)
        {
            CommandType = type
        };

        public static IDataParameter CreateParameter(string name, object? value, MySqlDbType type, bool isNullable = true) => new MySqlParameter(name, value)
        {
            Direction = ParameterDirection.Input,
            MySqlDbType = type,
            IsNullable = isNullable
        };

        public static IDataParameter CreateParameterOut(string name, MySqlDbType type) => new MySqlParameter()
        {
            ParameterName = name,
            Direction = ParameterDirection.Output,
            MySqlDbType = type
        };

        private static void OpenConnection(MySqlDataBase db)
        {
            db.Connection.Open();
        }

        public static void ReaderBlock(MySqlCommand cmd, ReaderAction action)
        {
            using var reader = cmd.ExecuteReader();
            action(reader);
            reader.Close();
            //cmd.Dispose();
        }

        public static void NonQueryBlock(MySqlCommand cmd, Action action)
        {
            cmd.ExecuteNonQuery();
            action();           
        }

        public static void TransactionBlock(
            MySqlDataBase db,
            TransactionCallback action,
            DataException onException,
            Action? onProcess = null
        )
        {
            MySqlConnection conn = db.Connection;
            bool isTxnSuccess;
            
            if(db.Connection.State == ConnectionState.Closed)
            {
                db.OpenConnection();
            }

            try
            {                
                action();                
                isTxnSuccess = true;
            }
            catch (Exception e)
            {                                
                onException(e);
                isTxnSuccess = false;
            }

            if (isTxnSuccess)
                onProcess?.Invoke();

            db.Connection.Dispose();
            db.Connection.Close();
        }

        //void IDisposable.Dispose()
        //{
        //    CloseConnection();
        //}
    }
}
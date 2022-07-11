using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using DataService.MySQL;

namespace DataService
{
    class Program
    {
        static void Main(string[] args)
        {
            //MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            //string connectctionStr = "server=127.0.0.1;uid=root;pwd=losseisbastardos12;database=comfort_travel";            
            var builder = new MySqlConnectionStringBuilder() {
                UserID = "root",
                Password = "",
                Server = "127.0.0.1",
                Port = 3306,
                Database = "CTL_ACCESS"
            };

            var dal = new MySqlDataBase(builder.ToString());
            dal.OnException = (ex, msg) => {
                Console.WriteLine(ex.Message + " " + msg);
            };

            MySqlDataBase.TransactionBlock(dal.Connection, txn => {
                var cmd = dal.CreateCommand("SELECT * FROM SHIFT", txn, CommandType.Text);

                /*WHERE NAME = :IN_NAME*/
                // cmd.Parameters.Add(MySqlDataBase.CreateParameter(
                //     "IN_NAME", 
                //     "MATUTINE", 
                //     ParameterDirection.Input,
                //     MySqlDbType.VarChar
                // ));

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        string name = reader["NAME"].ToString();
                        string id = reader["SHIFT_ID"].ToString();                        
                        Console.WriteLine(name);
                    }
                }

            }, (ex, msg) => {

            });            


        }
    }    
}

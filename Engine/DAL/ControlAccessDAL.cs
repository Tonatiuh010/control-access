using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using DataService.MySQL;
using Engine.Constants;
using Engine.BO;

namespace Engine.DAL {
    public class ControlAccessDAL : MySqlDataBase {
        public static string? ConnString {get; set;}
        public static ControlAccessDAL Instance => new ControlAccessDAL();

        private ControlAccessDAL() : base(ConnString) {

        }

        public List<Check> GetChecks() {
            List<Check> model = new List<Check>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.WEIRD_QRY, txn, CommandType.Text);            

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        model.Add(new () {
                            Card = new(),
                            CheckDt = reader.GetDateTime("CHECK_DT"),                            
                        });

                        string name = reader["NAME"].ToString();
                        string id = reader["SHIFT_ID"].ToString();                        
                        Console.WriteLine(name);
                    }
                }

            }, (ex, msg) => {

            });            

            return model;
        }
    }
}
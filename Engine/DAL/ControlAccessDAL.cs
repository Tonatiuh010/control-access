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
        private static ControlAccessDAL _DAL {get; set;} = null;
        public static string? ConnString {get; set;}
        public static ControlAccessDAL Instance {
            get 
            {
                if(_DAL == null) {
                    _DAL = new ControlAccessDAL(); 
                }

                return _DAL;
            } 
        }
        
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

        public List<Employee> GetEmployees(int? employeeId) {
            List<Employee> model = new List<Employee>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.GET_EMPLOYEE_ACCESS_LEVEL, txn, CommandType.Text);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                /*EMPLOYEE_ID, FIRST_NAME, LAST_NAME, POSITION_ID, SHIFT_ID, EMPLOYEE_STATUS, POSITION_ID, POSITION_NAME, DEPARTAMENT_ID, JOB_ID, DEPARTAMENT, DEPTO_CODE, JOB, JOB_DETAIL, SHIFT_CODE, IN_SHIFT, OUT_SHIFT, LUNCH, SHIFT_INTERVAL, CARD_ID, CARD_NUMBER, CARD_STATUS*/

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        model.Add(new () {
                            Id = int.Parse(reader["EMPLOYEE_ID"].ToString()),
                            Name = reader["FIRST_NAME"].ToString(),
                            LastName = reader["LAST_NAME"].ToString(),
                            Job = new Position() {
                                // Id = int.Parse(reader["POSITION_ID"].ToString()),
                                // Alias = reader["POSITION_NAME"].ToString(),
                                // Departament = new Departament() {
                                //     Code = reader["DEPARTAMENT"]
                                // }
                            },
                            Shift = new Shift() {

                            },
                            AccessLevels = new List<AccessLevel>()                            

                        });
                    }
                }

            }, (ex, msg) => {
                // Put something here
            });
            
            return model;
        }

        public List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId) {
            List<EmployeeAccessLevel> model = new List<EmployeeAccessLevel>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.GET_EMPLOYEE_ACCESS_LEVEL, txn, CommandType.Text);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        model.Add(new () {
                            Id = int.Parse(reader["ACCESS_LEVEL_ID"].ToString()),
                            Name = reader["NAME"].ToString(),
                            Status = reader["STATUS"].ToString(),
                            EmployeeId = int.Parse(reader["EMPLOYEE_ID"].ToString())
                        });
                    }
                }

            }, (ex, msg) => {
                // Put something here
            });
            
            return model;
        }
        
        public List<AccessLevel> GetAccessLevels() {
            List<AccessLevel> model = new List<AccessLevel>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.GET_ACCESS_LEVEL, txn, CommandType.Text);                            

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        model.Add(new () {
                            Id = int.Parse(reader["ACCESS_LEVEL_ID"].ToString()),
                            Name = reader["NAME"].ToString(),
                            Status = reader["STATUS"].ToString()
                        });
                    }
                }

            }, (ex, msg) => {
                // Put something here
            });

            return model;
        }

        public Result SetEmployee(Employee employee, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_EMPLOYEE;

            MySqlDataBase.TransactionBlock(Connection, txn => {                
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employee.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", employee.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_LAST_NAME", employee.LastName, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_POSITION", employee.Job.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", employee.Shift.Id, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetEmployee", msg, ex, result));

            return result;
        }

        public Result SetDownEmployee(int employeeId, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_DOWN_EMPLOYEE;

            MySqlDataBase.TransactionBlock(Connection, txn => {                
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDownEmployee", msg, ex, result));

            return result;
        }


        public Result SetEmployeeAccessLevel(int employeeNumber, int accessLevel, string status) {
            Result result = new Result();
            string sSp = SQL.SET_EMPLOYEE_ACCESS;

            MySqlDataBase.TransactionBlock(Connection, txn  =>{
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeNumber, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_ACCESS", accessLevel, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_STATUS", status, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetAccessLevel", msg, ex, result));

            return result;                      
        }

        public Result SetCard(Card card, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_CARD;

            MySqlDataBase.TransactionBlock(Connection, txn => {

                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_CARD_ID", card.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NUMBER", card.Key, MySqlDbType.String));                
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", card.Employee.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser,  MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                 
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }                

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetCard", msg, ex, result));

            return result;
        }

        public Result SetDownCard(int cardId, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_DOWN_CARD;

            MySqlDataBase.TransactionBlock(Connection, txn => {

                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_CARD_ID", cardId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser,  MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                 
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }                

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDownCard", msg, ex, result));

            return result;
        }

        public static Result GetResult(IDataParameter pResult, string sSp) {  
            Result result = new Result();

            try {
                if(pResult.Value != null) {
                    var res = pResult.Value.ToString();

                    if(string.IsNullOrEmpty(res))  {
                        result.Status = C.ERROR;
                        result.Message = $"Procedure {sSp} did not return response value";
                    }

                    if (res != C.OK) {
                        result.Status = C.ERROR;
                        result.Message = res;
                    }

                    result.Status = res;
                    result.Message = C.COMPLETE;
                }
            } catch (Exception ex) {
                result.Status = C.ERROR;
                result.Message = ex.Message;
            }

            return result;
        }

        public static void SetExceptionResult(string actionName, string msg, Exception ex, Result result) {
            result.Status = C.ERROR;
            result.Message = $"Exception on ({actionName}) - Details({msg}) - {ex.Message}";
            result.Data = ex.GetType().ToString();            
        }

        public static void DataAccessBlock(Action action, ControlAccessDAL dal) {

        }

    }
}
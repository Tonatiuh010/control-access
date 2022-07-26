using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using DataService.MySQL;
using Engine.Constants;
using Engine.BO;
using Engine.BL.Delegates;

namespace Engine.DAL {
    public class ControlAccessDAL : MySqlDataBase {
        private static ControlAccessDAL? _DAL {get; set;} = null;        
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

        public Delegates.CallbackExceptionMsg? OnDALError {get; set;}
        private Validate Validate;
        private ControlAccessDAL() : base(ConnString){
            Validate = Validate.Instance;
            OnDALError = null;
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

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetChecks", msg, ex));

            return model;
        }

        public List<Departament> GetDepartaments(int? deptoId) {
            List<Departament> model = new List<Departament>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.GET_EMPLOYEE_DETAIL, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", deptoId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        model.Add(new Departament() {
                            Id = Validate.getDefaultIntIfDBNull(reader["DEPARTAMENT_ID"]),
                            Code = Validate.getDefaultStringIfDBNull(reader["CODE"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["NAME"])
                        });
                    }
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetDepartaments", msg, ex));
            
            return model;
        }

        public List<Employee> GetEmployees(int? employeeId) {
            List<Employee> model = new List<Employee>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.GET_EMPLOYEE_DETAIL, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        var position = new Position(){
                            Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                            Alias = Validate.getDefaultStringIfDBNull(reader["POSITION_NAME"]),
                            PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                            Description = Validate.getDefaultStringIfDBNull(reader["JOB_DETAIL"]),
                            Departament = new Departament() {
                                Id = Validate.getDefaultIntIfDBNull(reader["DEPARTAMENT_ID"]),
                                Code = Validate.getDefaultStringIfDBNull(reader["DEPTO_CODE"]),
                                Name = Validate.getDefaultStringIfDBNull(reader["DEPARTAMENT"])
                            }
                        };

                        var shift = new Shift(){
                            Id = Validate.getDefaultIntIfDBNull(reader["SHIFT_ID"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["SHIFT_CODE"]),
                            DayCount = Validate.getDefaultIntIfDBNull(reader["SHIFT_INTERVAL"]),
                            InTime = Validate.getDefaultTimeSpanIfDBNull(reader["IN_SHIFT"]),
                            OutTime = Validate.getDefaultTimeSpanIfDBNull(reader["OUT_SHIFT"]),
                            LunchTime = Validate.getDefaultTimeSpanIfDBNull(reader["LUNCH"])
                        };

                        model.Add(new () {
                            Id = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["FIRST_NAME"]),
                            LastName = Validate.getDefaultStringIfDBNull(reader["LAST_NAME"]),
                            Job =  position.IsValidPosition() ? position : null,
                            Shift = shift.IsValid() ? shift : null,
                            AccessLevels = new List<AccessLevel>()
                        });
                    }
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployees", msg, ex));
            
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
                            Id = Validate.getDefaultIntIfDBNull(reader["ACCESS_LEVEL_ID"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                            Status = Validate.getDefaultStringIfDBNull(reader["STATUS"]),
                            EmployeeId = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"])
                        });
                    }
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployeeAccessLevels", msg, ex));
            
            return model;
        }
        
        public List<AccessLevel> GetAccessLevels() {
            List<AccessLevel> model = new List<AccessLevel>();

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(SQL.GET_ACCESS_LEVEL, txn, CommandType.Text);                            

                using(var reader = cmd.ExecuteReader()) {
                    while(reader.Read()) {
                        model.Add(new () {
                            Id = Validate.getDefaultIntIfDBNull(reader["ACCESS_LEVEL_ID"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                            Status = Validate.getDefaultStringIfDBNull(reader["STATUS"].ToString())
                        });
                    }
                }
            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetAccessLevel", msg, ex));

            return model;
        }

        public Result SetCheck(string cardSerial, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_CARD_CHECK;

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_NUMBER", cardSerial, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetCheck", msg, ex, result));

            return result;
        }

        public Result SetDepartament(Departament departament, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_DEPARTAMENT;

            MySqlDataBase.TransactionBlock(Connection, txn => {                
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", departament.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", departament.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_CODE", departament.Code, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDepartament", msg, ex, result));

            return result;
        }

        public Result SetJob(Job job, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_JOB;

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", job.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", job.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DESCRIPTION", job.Description, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetJob", msg, ex, result));

            return result;
        }

        public Result SetAccessLevel(AccessLevel level, string txnUser){ 
            Result result = new Result();
            string sSp = SQL.SET_ACCESS_LEVEL;

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_ACCESS_ID", level.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", level.Name, MySqlDbType.String));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetAccessLevel", msg, ex, result));

            return result;
        }

        public Result SetShift(Shift shift, string txnUser) {
            Result result = new Result();
            string sSp = SQL.SET_SHIFT;

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", shift.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", shift.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_CLOCK_IN", shift.InTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_CLOCK_OUT", shift.OutTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_LUNCH", shift.LunchTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_DAY", shift.DayCount, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetShift", msg, ex, result));

            return result;
        }

        public Result SetPosition(Position position, string txnUser) {  
            Result result = new Result();
            string sSp = SQL.SET_POSITION;

            MySqlDataBase.TransactionBlock(Connection, txn => {
                var cmd = CreateCommand(sSp, txn, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_POSITION", position.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", position.Alias, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", position.Departament.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_JOB", position.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);
                
                using(var reader = cmd.ExecuteReader()) 
                {
                    result = GetResult(pResult, sSp);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetPosition", msg, ex, result));

            return result;
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
                    } else if (res != C.OK) {
                        result.Status = C.ERROR;
                        result.Message = res;
                    } else {
                        result.Status = res;
                        result.Message = C.COMPLETE;
                    }
                }
            } catch (Exception ex) {
                result.Status = C.ERROR;
                result.Message = ex.Message;
            }

            return result;
        }

        private void SetExceptionResult(string actionName, string msg, Exception ex) 
        {
            if(OnDALError != null) {
                OnDALError(ex, $"Error on ({actionName}) - {msg}");
            }
        }

        public static void SetExceptionResult(string actionName, string msg, Exception ex, Result result) 
        {
            result.Status = C.ERROR;
            result.Message = $"Exception on ({actionName}) - Details({msg}) - {ex.Message}";
            result.Data = ex.GetType().ToString();            
        }

        public static void SetOnConnectionException(DataException onException) => MySqlDataBase.OnException = onException;        

    }
}
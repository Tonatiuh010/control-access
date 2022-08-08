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
    public class ControlAccessDAL : MySqlDataBase
    {
        public delegate void DALCallback(ControlAccessDAL dal);
        public static string? ConnString { get; set; }
        public static ControlAccessDAL Instance { get {
                //if(_DAL == null)
                //{
                //    _DAL = new ControlAccessDAL();
                //} else if (_DAL.Connection.State == ConnectionState.Closed)
                //{
                //    _DAL.OpenConnection();
                //}
                //return _DAL;

                return new ControlAccessDAL();
            }
        }
        private static ControlAccessDAL _DAL { get; set; }


        private readonly Validate Validate;
        public Delegates.CallbackExceptionMsg? OnDALError { get; set; }
        private ControlAccessDAL() : base(ConnString) {
            Validate = Validate.Instance;
            OnDALError = null;
        }

        public List<Check> GetChecks(int? checkId, int? cardId, int? employeeId) {
            List<Check> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_CHECKS,  CommandType.StoredProcedure);
                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_CHECK", checkId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_CARD", cardId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["CHECK_ID"]),
                        Card = new CardEmployee(Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"]))
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["CARD_ID"]),
                            Key = Validate.getDefaultStringIfDBNull(reader["NUMBER"]),
                            Status = Validate.getDefaultStringIfDBNull(reader["CARD_STATUS"])
                        },
                        CheckDt = Validate.getDefaultDateIfDBNull(reader["CHECK_DT"]),
                        Type = Validate.getDefaultStringIfDBNull(reader["TYPE"]),
                        Device = Validate.getDefaultIntIfDBNull(reader["DEVICE_ID"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetChecks", msg, ex));

            return model;
        }

        public List<Departament> GetDepartaments(int? deptoId) {
            List<Departament> model = new List<Departament>();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_DEPARTAMENTS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", deptoId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Departament()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DEPARTAMENT_ID"]),
                        Code = Validate.getDefaultStringIfDBNull(reader["CODE"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetDepartaments", msg, ex));

            return model;
        }

        public List<Shift> GetShifts(int? shiftId) 
        {
            List<Shift> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_SHIFTS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", shiftId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Shift()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["SHIFT_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        InTime = Validate.getDefaultTimeSpanIfDBNull(reader["CLOCK_IN"]),
                        OutTime = Validate.getDefaultTimeSpanIfDBNull(reader["CLOCK_OUT"]),
                        LunchTime = Validate.getDefaultTimeSpanIfDBNull(reader["LUNCH_TIME"]),
                        DayCount = Validate.getDefaultIntIfDBNull(reader["DAY_COUNT"])

                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetShifts", msg, ex));

            return model;
        }

        public List<Job> GetJobs(int? jobId)
        {
            List<Job> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_JOBS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_JOB", jobId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Job()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["DESCRIPTION"])

                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetJobs", msg, ex));

            return model;
        }

        public List<CardEmployee> GetCards(int? cardId, bool? assigned)
        {
            var model = new List<CardEmployee>();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_CARDS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_CARD", cardId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_ASSIGNED", assigned, MySqlDbType.Int16));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new CardEmployee(Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"]))
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["CARD_ID"]),
                        Key = Validate.getDefaultStringIfDBNull(reader["NUMBER"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS"])
                    });
                }
                reader.Close();
            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetCards", msg, ex));

            return model;
        }

        public List<Position> GetPositions(int? positionId, int? jobId, int? deptoId)
        {
            List<Position> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_POSITIONS,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_POSITION", positionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_JOB", jobId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", deptoId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new Position()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["JOB"]),
                        Alias = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["JOB_DETAIL"]),
                        Departament = new Departament()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["DEPARTAMENT_ID"]),
                            Code = Validate.getDefaultStringIfDBNull(reader["DEPTO_CODE"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["DEPARTAMENT"])
                        }
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetPositions", msg, ex));

            return model;
        }

        public List<ControlAccess> GetEmployees(int? employeeId) {
            List<ControlAccess> model = new();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_EMPLOYEE_DETAIL,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var card = new Card()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["CARD_ID"]),
                        Key = Validate.getDefaultStringIfDBNull(reader["CARD_NUMBER"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["CARD_STATUS"]),
                    };

                    var position = new Position()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                        Alias = Validate.getDefaultStringIfDBNull(reader["POSITION_NAME"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["JOB"]),
                        PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                        Description = Validate.getDefaultStringIfDBNull(reader["JOB_DETAIL"]),
                        Departament = new Departament()
                        {
                            Id = Validate.getDefaultIntIfDBNull(reader["DEPARTAMENT_ID"]),
                            Code = Validate.getDefaultStringIfDBNull(reader["DEPTO_CODE"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["DEPARTAMENT"])
                        }
                    };

                    var shift = new Shift()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["SHIFT_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["SHIFT_CODE"]),
                        DayCount = Validate.getDefaultIntIfDBNull(reader["SHIFT_INTERVAL"]),
                        InTime = Validate.getDefaultTimeSpanIfDBNull(reader["IN_SHIFT"]),
                        OutTime = Validate.getDefaultTimeSpanIfDBNull(reader["OUT_SHIFT"]),
                        LunchTime = Validate.getDefaultTimeSpanIfDBNull(reader["LUNCH"])
                    };

                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["FIRST_NAME"]),
                        LastName = Validate.getDefaultStringIfDBNull(reader["LAST_NAME"]),
                        Image = new ImageData(Validate.getDefaultBytesIfDBNull(reader["IMAGE"])),
                        Status = Validate.getDefaultStringIfDBNull(reader["EMPLOYEE_STATUS"]),
                        Job = position.IsValidPosition() ? position : null,
                        Shift = shift.IsValid() ? shift : null,
                        Card = card.IsValid() ? card : null,
                        AccessLevels = new List<AccessLevel>()
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployees", msg, ex));
            
            return model;
        }

        public List<CheckDetails> GetCheckDetails(DateTime from, DateTime to)
        {
            List<CheckDetails> model = new ();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_CHECK_DETAILS, CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_FROM_DT", from, MySqlDbType.DateTime));
                cmd.Parameters.Add(CreateParameter("IN_TO_DT", to, MySqlDbType.DateTime));
                cmd.Parameters.Add(pResult);
                /* CARD_CHECK_ID, TIME_EXP, CHECK_DT, TYPE, POSITION_ID, POSITION, JOB_ID, JOB, DEPARTAMENT_ID, DEPARTAMENT, DEPARTAMENT_CODE */
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["CARD_CHECK_ID"]),
                        CheckDt = Validate.getDefaultDateIfDBNull(reader["CHECK_DT"]),
                        Type = Validate.getDefaultStringIfDBNull(reader["TYPE"]),
                        Position = new Position()
                        {
                            PositionId = Validate.getDefaultIntIfDBNull(reader["POSITION_ID"]),
                            Alias = Validate.getDefaultStringIfDBNull(reader["POSITION"]),
                            Departament = new Departament()
                            {
                                Id = Validate.getDefaultIntIfDBNull(reader["DEPARTAMENT_ID"]),
                                Code = Validate.getDefaultStringIfDBNull(reader["DEPARTAMENT_CODE"]),
                                Name = Validate.getDefaultStringIfDBNull(reader["DEPARTAMENT"])
                            },
                            Id = Validate.getDefaultIntIfDBNull(reader["JOB_ID"]),
                            Name = Validate.getDefaultStringIfDBNull(reader["JOB"])
                        }

                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployeeAccessLevels", msg, ex));

            return model;
        }

        public List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId) {
            List<EmployeeAccessLevel> model = new List<EmployeeAccessLevel>();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_EMPLOYEE_ACCESS_LEVEL,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["ACCESS_LEVEL_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS"]),
                        EmployeeId = Validate.getDefaultIntIfDBNull(reader["EMPLOYEE_ID"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetEmployeeAccessLevels", msg, ex));
            
            return model;
        }                

        public List<Device> GetDevices(int? deviceId)
        {
            List<Device> model = new();

            TransactionBlock(this, () =>
            {
                using var cmd = CreateCommand(SQL.GET_DEVICES,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_DEVICE", deviceId, MySqlDbType.Int32));
                cmd.Parameters.Add(pResult);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["DEVICE_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS"]),
                        Unsuccessful = Validate.getDefaultIntIfDBNull(reader["ERROR_COUNT"]),
                        Activations = Validate.getDefaultIntIfDBNull(reader["ACTIVATION_COUNT"]),
                        LastUpdate = Validate.getDefaultDateIfDBNull(reader["UPDATED_ON"])
                    });
                }
                reader.Close();

            },(ex, msg) => SetExceptionResult("ControlAccessDAL.GetDevices", msg, ex));

            return model;
        }

        public List<AccessLevel> GetAccessLevels() {
            List<AccessLevel> model = new List<AccessLevel>();

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(SQL.GET_ACCESS_LEVEL,  CommandType.Text);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    model.Add(new()
                    {
                        Id = Validate.getDefaultIntIfDBNull(reader["ACCESS_LEVEL_ID"]),
                        Name = Validate.getDefaultStringIfDBNull(reader["NAME"]),
                        Status = Validate.getDefaultStringIfDBNull(reader["STATUS"])
                    });
                }
                reader.Close();

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetAccessLevel", msg, ex));

            return model;
        }

        public ResultInsert SetCheck(string cardSerial, int? device, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_CARD_CHECK;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_NUMBER", cardSerial, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEVICE", device, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));                
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetCheck", msg, ex, result),
                () => SetResultInsert(result, new Check() {
                    Card = new CardEmployee() { Key = cardSerial }
                })
            );



            return result;
        }

        public ResultInsert SetDepartament(Departament departament, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_DEPARTAMENT;

            TransactionBlock(this, () => {                
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", departament.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", departament.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_CODE", departament.Code, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDepartament", msg, ex, result),
                () => SetResultInsert(result, departament)
            );

            return result;
        }

        public ResultInsert SetJob(Job job, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_JOB;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_JOB", job.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", job.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DESCRIPTION", job.Description, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetJob", msg, ex, result),
                () => SetResultInsert(result, job)
            );

            return result;
        }

        public ResultInsert SetAccessLevel(AccessLevel level, string txnUser){
            ResultInsert result = new();
            string sSp = SQL.SET_ACCESS_LEVEL;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_ACCESS_ID", level.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", level.Name, MySqlDbType.String));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetAccessLevel", msg, ex, result),
                () => SetResultInsert(result, level)
            );

            return result;
        }

        public ResultInsert SetShift(Shift shift, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_SHIFT;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", shift.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", shift.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_CLOCK_IN", shift.InTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_CLOCK_OUT", shift.OutTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_LUNCH", shift.LunchTime, MySqlDbType.Time));
                cmd.Parameters.Add(CreateParameter("IN_DAY", shift.DayCount, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetShift", msg, ex, result),
                () => SetResultInsert(result, shift)
            );

            return result;
        }

        public ResultInsert SetPosition(Position position, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_POSITION;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                
                cmd.Parameters.Add(CreateParameter("IN_POSITION", position.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", position.Alias, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_DEPTO", 
                    position.Departament?.Id,
                    MySqlDbType.Int32
                ));
                cmd.Parameters.Add(CreateParameter("IN_JOB", position.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetPosition", msg, ex, result),
                () => SetResultInsert(
                    result,
                    new BaseBO()
                    {
                        Id = position.PositionId
                    }
                )
            );

            return result;
        }

        public ResultInsert SetEmployee(ControlAccess employee, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_EMPLOYEE;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employee.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NAME", employee.Name, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_LAST_NAME", employee.LastName, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_POSITION", employee.Job?.PositionId, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_SHIFT", employee.Shift?.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_IMG", employee.Image?.Hex, MySqlDbType.LongText));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetEmployee", msg, ex, result), 
                () => SetResultInsert(result, employee)
            );
            
            return result;
        }

        public Result SetDownEmployee(int employeeId, string txnUser) {
            Result result = new();
            string sSp = SQL.SET_DOWN_EMPLOYEE;

            TransactionBlock(this, () => {                
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDownEmployee", msg, ex, result));

            return result;
        }


        public Result SetEmployeeAccessLevel(int employeeNumber, int accessLevel, string status, string user) {
            Result result = new();
            string sSp = SQL.SET_EMPLOYEE_ACCESS;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);

                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", employeeNumber, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_ACCESS", accessLevel, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_STATUS", status, MySqlDbType.String));
                cmd.Parameters.Add(CreateParameter("IN_USER", user, MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetAccessLevel", msg, ex, result));

            return result;                      
        }

        public ResultInsert SetCard(CardEmployee card, string txnUser) {
            ResultInsert result = new();
            string sSp = SQL.SET_CARD;

            TransactionBlock(this, () => {

                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_CARD_ID", card.Id, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_NUMBER", card.Key, MySqlDbType.String));                
                cmd.Parameters.Add(CreateParameter("IN_EMPLOYEE", card.Employee, MySqlDbType.Int32));
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser,  MySqlDbType.String));
                cmd.Parameters.Add(pResult);

                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, 
                (ex, msg) => SetExceptionResult("ControlAccessDAL.SetCard", msg, ex, result), 
                () =>  SetResultInsert(result, card)
            );

            return result;
        }

        public Result SetDownCard(int cardId, string txnUser) {
            Result result = new();
            string sSp = SQL.SET_DOWN_CARD;

            TransactionBlock(this, () => {
                using var cmd = CreateCommand(sSp,  CommandType.StoredProcedure);

                IDataParameter pResult = CreateParameterOut("OUT_RESULT", MySqlDbType.String);
                cmd.Parameters.Add(CreateParameter("IN_CARD_ID", cardId, MySqlDbType.Int32));                
                cmd.Parameters.Add(CreateParameter("IN_USER", txnUser,  MySqlDbType.String));
                cmd.Parameters.Add(pResult);


                NonQueryBlock(cmd, () => GetResult(pResult, sSp, result));

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.SetDownCard", msg, ex, result));

            return result;
        }

        public int? GetLastId()
        {
            int? id = null; 
            TransactionBlock(this, () => {

                using var cmd = CreateCommand("SELECT LAST_INSERT_ID()",  CommandType.Text);
                var result = cmd.ExecuteScalar().ToString();

                if(result != null)
                {
                    id = int.Parse(result);
                }

            }, (ex, msg) => SetExceptionResult("ControlAccessDAL.GetLastId()", msg, ex));

            return id;
        }

        public static void GetResult(IDataParameter pResult, string sSp, Result result) {

            try {
                if(pResult.Value != null) {
                    var res = pResult.Value.ToString();

                    if(string.IsNullOrEmpty(res))  {
                        result.Status = C.ERROR;
                        result.Message = $"Procedure {sSp} did not return response value";
                    } else if (res != C.OK) {
                        result.Status = C.ERROR;
                        result.Message = res;
                        throw new ArgumentException($"Error From Internal Procedure DB -> Says: {result.Message}");
                    } else {
                        result.Status = res;
                        result.Message = C.COMPLETE;
                    }
                }
            } catch (ArgumentException ex )
            {
                throw ex;
            } catch (Exception ex) {
                result.Status = C.ERROR;
                result.Message = ex.Message;
            }

        }

        public void SetResultInsert(ResultInsert result, BaseBO bo)
        { 
            result.InsertDetails = bo.IsValid() ? 
                new InsertStatus(bo) : 
                new InsertStatus() {
                    Id = GetLastId(),
                    FromObject = bo.GetType()
                };
        }

        private void SetExceptionResult(string actionName, string msg, Exception ex) => 
            OnDALError?.Invoke(ex, $"Error on ({actionName}) - {msg}");

        //public static void UsingDAL(DALCallback callback)
        //{
        //    using (var dal = Instance)
        //    {
        //        callback(dal);
        //        //dal.Dispose();
        //    }
        //}
        
        public static void SetExceptionResult(string actionName, string msg, Exception ex, Result result) 
        {
            result.Status = C.ERROR;
            result.Message = $"Exception on ({actionName}) - Details({msg}) - {ex.Message}";
            result.Data = ex.GetType().ToString();            
        }

        public static void SetOnConnectionException(DataException onException) => OnException = onException;

    }
}
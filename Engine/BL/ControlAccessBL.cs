using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BO;
using Engine.Constants;
using Engine.DAL;
using Engine.BL;
using D = Engine.BL.Delegates;

namespace Engine.BL {
    public class ControlAccessBL {
        public delegate object ActionDAL(ControlAccessDAL dal);
        public D.Delegates.CallbackExceptionMsg? OnError { get; set; } = null;
        private ControlAccessDAL DAL { get {
                var dal = ControlAccessDAL.Instance;
                if (OnError != null)
                {
                    dal.OnDALError = OnError;
                }

                return dal;
            } 
        }
    

        public string DomainUrl { get; set; }

        public ControlAccessBL(D.Delegates.CallbackExceptionMsg onError) { 
            OnError = onError;
            DomainUrl = string.Empty;
        }

        public List<CardEmployee> GetCards(int? cardId = null, bool? assigned = null)
        {            
            var cards = DAL.GetCards(cardId, assigned);

            foreach (var c in cards)
            {
                c.SetEmployeeFinder(GetEmployee);
            }

            return cards;            
        }

        public List<Shift> GetShifts(int? shiftId = null) => DAL.GetShifts(shiftId);

        public List<Job> GetJobs(int? jobId = null) => DAL.GetJobs(jobId);

        public List<Position> GetPositions(int? positionId = null, int? jobId = null, int? departmentId = null) =>
            DAL.GetPositions(positionId, jobId, departmentId);

        public List<ControlAccess> GetEmployees(int? employeeId = null) 
        {
            List<ControlAccess> employees = new();          
            employees = DAL.GetEmployees(employeeId);

            foreach (var employee in employees)
            {
                employee.AccessLevels = EmployeeAccessLevel.GetAccessLevels(
                    GetEmployeeAccessLevels(employee.Id)
                );

                if (employee.Image != null)
                    employee.Image.Url = $"{DomainUrl}/api/employee/image/{employee.Id}";
            }
            

            return employees;
        }

        public ResultInsert SetEmployee(ControlAccess employee, string txnUser) => DAL.SetEmployee(employee, txnUser);

        public ResultInsert SetCard(CardEmployee card, string txnUser) => DAL.SetCard(card, txnUser);

        public Result SetEmployeeAccessLevel(int employeeId, int accessLevelId, bool status, string txnUser) =>
            DAL.SetEmployeeAccessLevel(employeeId, accessLevelId, status ? C.ENABLED : C.DISABLED, txnUser);

        public Result SetDownEmployee(int employeeId, string txnUser) => DAL.SetDownEmployee(employeeId, txnUser);

        public Result SetDownCard(int cardId, string txnUser) => DAL.SetDownCard(cardId, txnUser);

        public List<AccessLevel> GetAccessLevels() => DAL.GetAccessLevels();

        public List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId) => DAL.GetEmployeeAccessLevels(employeeId);

        public ResultInsert SetCheck(string cardSerial,int? device, string txnUser) => DAL.SetCheck(cardSerial, device, txnUser);

        public List<Departament> GetDepartaments(int? deptoId = null) => DAL.GetDepartaments(deptoId);

        public List<Device> GetDevices(int? deviceId = null) => DAL.GetDevices(deviceId);

        public List<Check> GetChecks(int? checkId = null, int? cardId = null, int? employeeId = null) {
            var checks = DAL.GetChecks(checkId, cardId, employeeId);

            foreach(var ch in checks)
            {
                var card = ch.Card;
                var device = ch.Device;

                if(card?.GetType() == typeof(CardEmployee))
                {
                    ((CardEmployee)card).SetEmployeeFinder(GetEmployee);
                }

                if(device != null)
                {
                    ch.SetDeviceFinder(GetDevice);
                }
            }

            return checks;
        }            

        public ResultInsert SetDepartament(Departament departament, string txnUser) => DAL.SetDepartament(departament, txnUser);

        public ResultInsert SetJob(Job job, string txnUser) => DAL.SetJob(job, txnUser);

        public ResultInsert SetAccessLevel(AccessLevel level, string txnUser) => DAL.SetAccessLevel(level, txnUser);

        public ResultInsert SetShift(Shift shift, string txnUser) => DAL.SetShift(shift, txnUser);

        public ResultInsert SetPosition(Position position, string txnUser) => DAL.SetPosition(position, txnUser);      

        public AccessLevel? GetAccessLevel(string name) => GetAccessLevels().Find(x => x.Name == name);

        public List<CheckDetails> GetCheckDetails(DateTime from, DateTime to)
        {
            return DAL.GetCheckDetails(from, to);
        }

        public Device? GetDevice(string name) => GetDevices().Find(x => x.Name == name);

        private ControlAccess? GetEmployee(int? id )
        {
            var employees = GetEmployees(id);
            return employees != null && employees.Count > 0 ? employees[0] : null;
        }

        private Device? GetDevice(int? id)
        {
            var devices = GetDevices(id);
            return devices != null && devices.Count > 0 ? devices[0] : null;
        }
        
    }

}
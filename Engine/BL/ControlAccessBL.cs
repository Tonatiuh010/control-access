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
        public D.Delegates.CallbackExceptionMsg? OnError {get; set;} = null;
        private ControlAccessDAL DAL { get {
            var dal = ControlAccessDAL.Instance;

            if (OnError != null) {
                dal.OnDALError = OnError;
            }

            return dal;
        } }


        public ControlAccessBL(D.Delegates.CallbackExceptionMsg onError ) => OnError = onError;

        public List<CardEmployee> GetCards(int? cardId = null, bool? assigned = null)
        {
            var cards = DAL.GetCards(cardId, assigned);

            foreach(var c in cards)
            {
                c.SetEmployee(id =>
                {
                    var employees = GetEmployees(id);
                    return employees != null && employees.Count > 0 ? employees[0] : new Employee();
                });
            }

            return cards;
        }

        public List<Shift> GetShifts(int? shiftId = null) => DAL.GetShifts(shiftId);

        public List<Job> GetJobs(int? jobId = null) => DAL.GetJobs(jobId);

        public List<Position> GetPositions(int? positionId = null, int? jobId = null, int? departmentId = null) =>
            DAL.GetPositions(positionId, jobId, departmentId);

        public List<Employee> GetEmployees(int? employeeId = null) {
            var employees = DAL.GetEmployees(employeeId);

            foreach (var employee in employees)
                employee.AccessLevels = EmployeeAccessLevel.GetAccessLevels( 
                    GetEmployeeAccessLevels(employee.Id)
                );
          
            return employees;
        }
        
        public ResultInsert SetEmployee(Employee employee, string txnUser) => DAL.SetEmployee(employee, txnUser);
        
        public ResultInsert SetCard(CardEmployee card, string txnUser) => DAL.SetCard(card, txnUser);

        public Result SetEmployeeAccessLevel(int employeeId, int accessLevelId, bool status, string txnUser) => 
            DAL.SetEmployeeAccessLevel(employeeId, accessLevelId, status ? C.ENABLED : C.DISABLED, txnUser);

        public Result SetDownEmployee(int employeeId, string txnUser) => DAL.SetDownEmployee(employeeId, txnUser);        

        public Result SetDownCard(int cardId, string txnUser) => DAL.SetDownCard(cardId, txnUser);
        
        public List<AccessLevel> GetAccessLevels() => DAL.GetAccessLevels();                

        public List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId)=> DAL.GetEmployeeAccessLevels(employeeId);

        public Result SetCheck(string cardSerial, string txnUser) => DAL.SetCheck(cardSerial, txnUser);

        public List<Departament> GetDepartaments(int? deptoId = null) => DAL.GetDepartaments(deptoId);

        public ResultInsert SetDepartament(Departament departament, string txnUser) => DAL.SetDepartament(departament, txnUser);

        public ResultInsert SetJob(Job job, string txnUser) => DAL.SetJob(job, txnUser);

        public ResultInsert SetAccessLevel(AccessLevel level, string txnUser) => DAL.SetAccessLevel(level, txnUser);

        public ResultInsert SetShift(Shift shift, string txnUser) => DAL.SetShift(shift, txnUser);

        public ResultInsert SetPosition(Position position, string txnUser) => DAL.SetPosition(position, txnUser);

        public AccessLevel? GetAccessLevel(string name) => GetAccessLevels().Find(x => x.Name == name);
    
    }

}
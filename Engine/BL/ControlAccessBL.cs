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

        public List<Employee> GetEmployees(int? employeeId = null) {
            var employees = DAL.GetEmployees(employeeId);

            foreach (var employee in employees)
                employee.AccessLevels = EmployeeAccessLevel.GetAccessLevels( 
                    GetEmployeeAccessLevels(employee.Id) 
                );
          
            return employees;
        }
        
        public Result SetEmployee(Employee employee, string txnUser) => DAL.SetEmployee(employee, txnUser);
        
        public Result SetCard(CardEmployee card, string txnUser) => DAL.SetCard(card, txnUser);

        public Result SetEmployeeAccessLevel(int employeeId, int accessLevelId, bool status, string txnUser) => 
            DAL.SetEmployeeAccessLevel(employeeId, accessLevelId, status ? C.ENABLED : C.DISABLED, txnUser);

        public Result SetDownEmployee(int employeeId, string txnUser) => DAL.SetDownEmployee(employeeId, txnUser);        

        public Result SetDownCard(int cardId, string txnUser) => DAL.SetDownCard(cardId, txnUser);
        
        public List<AccessLevel> GetAccessLevels() => DAL.GetAccessLevels();                

        public  List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId)=> DAL.GetEmployeeAccessLevels(employeeId);

        public Result SetCheck(string cardSerial, string txnUser) => DAL.SetCheck(cardSerial, txnUser);

        public List<Departament> GetDepartaments(int? deptoId = null) => DAL.GetDepartaments(deptoId);

        public Result SetDepartament(Departament departament, string txnUser) => DAL.SetDepartament(departament, txnUser);

        public Result SetJob(Job job, string txnUser) => DAL.SetJob(job, txnUser);

        public Result SetAccessLevel(AccessLevel level, string txnUser) => DAL.SetAccessLevel(level, txnUser);

        public Result SetShift(Shift shift, string txnUser) => DAL.SetShift(shift, txnUser);

        public Result SetPosition(Position position, string txnUser) => DAL.SetPosition(position, txnUser);
    
    }

}
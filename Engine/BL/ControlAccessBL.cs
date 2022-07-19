using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BO;
using Engine.Constants;
using Engine.DAL;
using Engine.BL;

namespace Engine.BL {
    public class ControlAccessBL {
        private ControlAccessDAL DAL => ControlAccessDAL.Instance;

        public List<Employee> GetEmployees(int? employeeId = null) => DAL.GetEmployees(employeeId);
        
        public Result SetEmployee(Employee employee, string txnUser) => DAL.SetEmployee(employee, txnUser);
        
        public Result SetCard(Card card, string txnUser) => DAL.SetCard(card, txnUser);

        public Result SetEmployeeAccessLevel (int employeeId, int accessLevelId, string txnUser) => DAL.SetEmployeeAccessLevel(employeeId, accessLevelId, txnUser);

        public Result SetDownEmployee(int employeeId, string txnUser) => DAL.SetDownEmployee(employeeId, txnUser);        

        public Result SetDownCard(int cardId, string txnUser) => DAL.SetDownCard(cardId, txnUser);
        
        public List<AccessLevel> GetAccessLevels() => DAL.GetAccessLevels();                

        public  List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId)=> DAL.GetEmployeeAccessLevels(employeeId);

        public Result SetCheck(string cardSerial, string txnUser) => DAL.SetCheck(cardSerial, txnUser);

        public Result SetDepartament(Departament departament, string txnUser) => DAL.SetDepartament(departament, txnUser);

        public Result SetEmployee(Job job, string txnUser) => DAL.SetJob(job, txnUser);

        public Result SetAccessLevel(AccessLevel level, string txnUser) => DAL.SetAccessLevel(level, txnUser);

        public Result SetShift(Shift shift, string txnUser) => DAL.SetShift(shift, txnUser);

        public Result SetPosition(Position position, string txnUser) => DAL.SetPosition(position, txnUser);
    
    }

}
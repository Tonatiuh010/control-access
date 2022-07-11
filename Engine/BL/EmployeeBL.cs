using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BO;
using Engine.Constants;
using Engine.DAL;
using Engine.BL;

namespace Engine.BL {
    public class EmployeeBL {
        private ControlAccessDAL DAL => ControlAccessDAL.Instance;


        public List<Employee> GetEmployees(int? employeeId) {
            return DAL.GetEmployees(employeeId);
        }

        public Result SetEmployee(Employee employee, string txnUser){            
            return DAL.SetEmployee(employee, txnUser);
        }

        public Result SetCard(Card card, string txnUser) {
            return DAL.SetCard(card, txnUser);
        }

        public Result SetEmployeeAccessLevel (int employeeId, int accessLevelId, string txnUser) {
            return DAL.SetEmployeeAccessLevel(employeeId, accessLevelId, txnUser);
        }

        public Result SetDownEmployee(int employeeId, string txnUser) {
            return DAL.SetDownEmployee(employeeId, txnUser);
        }

        public Result SetDownCard(int cardId, string txnUser) {
            return DAL.SetDownCard(cardId, txnUser);
        }

        public List<AccessLevel> GetAccessLevels() {
            return DAL.GetAccessLevels();        
        }

        public  List<EmployeeAccessLevel> GetEmployeeAccessLevels(int? employeeId) {
            return DAL.GetEmployeeAccessLevels(employeeId);
        }

    }

}
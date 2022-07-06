using System;
using System.Linq;
using System.Collections.Generic;
using DataService.MySQL;

namespace Engine.DAL {
    public class ControlAccessDAL : MySqlDataBase {
        public static string? ConnString {get; set;}
        public static ControlAccessDAL Instance => new ControlAccessDAL();

        private ControlAccessDAL() : base(ConnString) {

        }
    }
}
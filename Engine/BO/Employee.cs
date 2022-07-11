using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.BO {
    public class Employee {
        public int Id {get; set;}
        public string Name {get; set;}
        public string LastName {get; set;}
        public Position Job {get; set;}
        public List<AccessLevel> AccessLevels {get; set;}
        public Shift Shift {get; set;}        
    }

    public class Job {        
        public int Id {get; set;}
        public string Name {get; set;}
        public string Specialist {get; set;}
        public string Description {get; set;}
        public Departament Departament {get; set;}
    }

    public class Position : Job {
        public int PositionId {get; set;}
        public string Code { get; set;}
        public string Alias {get; set;}

    }

    public class AccessLevel {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Status {get; set;}
    }

    public class EmployeeAccessLevel : AccessLevel {
        public int EmployeeId {get; set;}
    }

    public class Departament {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
    }

    public class Card {
        public int Id {get; set;}
        public string Key {get; set;}
        public Employee Employee {get; set;}
    }

    public class Check {
        public int Id {get; set;}
        public DateTime CheckDt {get; set;}
        public string Type {get; set;}
        public Card Card {get; set;}
    }

    public class Shift { 
        public int Id {get; set;}
        public string Name {get; set;}
        public DateTime InTime {get; set;}
        public DateTime OutTime {get; set;}
        public DateTime LunchTime {get; set;}
        public int DayCount {get; set;}

    }

}
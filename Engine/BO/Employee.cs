using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.BO {
    public class Employee {
        public int Id {get; set;}
        public string Name {get; set;}
        public string LastName {get; set;}

        public Job Job {get; set;}
        public AccessLevel Level {get; set;}
        
    }

    public class Job {        
        public int Id {get; set;}
        public string Name {get; set;}
        public string Specialist {get; set;}
        public string Description {get; set;}
        public Departament Departament {get; set;}
    }

    public class AccessLevel {
        public int Id {get; set;}
        public string Name {get; set;}        
    }

    public class Departament {
        public int Id {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
    }

    public class Card {
        public int Id {get; set;}
        public decimal Serial {get; set;}
        public Employee Employee {get; set;}
    }

    public class Check {
        public int Id {get; set;}
        public DateTime CheckDt {get; set;}
        public string Type {get; set;}
        public Card Card {get; set;}
    }

}
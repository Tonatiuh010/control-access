using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;
using CsvHelper.Configuration;


namespace DataSet.Map
{

    public class EmployeeMap : ClassMap<ControlAccess>
    {
        public EmployeeMap ()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("name");
            Map(m => m.LastName).Name("lastname");
            Map(m => m.Card.Id).Name("card");
            Map(m => m.Shift.Id).Name("shift");
            Map(m => m.Job.PositionId).Name("position");
            Map(m => m.Image.Url).Name("url");
        }
    }

    public class JobMap : ClassMap<Job>
    {
        public JobMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("name");
            Map(m => m.Description).Name("description");
        }
    }

    public class CardEmployeeMap : ClassMap<CardEmployee>
    {
        public CardEmployeeMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Key).Name("number");
            Map(m => m.Employee).Name("employee");
        }
    }

    public class AccessLevelMap : ClassMap<AccessLevel>
    {
        public AccessLevelMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("name");
        }
    }

    public class DepartamentMap : ClassMap<Departament>
    {
        public DepartamentMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("name");
            Map(m => m.Code).Name("code");
        }
    }

    public class PositionMap : ClassMap<Position>
    {
        public PositionMap ()
        {
            Map(m => m.PositionId).Name("id");
            Map(m => m.Alias).Name("name");
            Map(m => m.Departament.Id).Name("departament");
            Map(m => m.Id).Name("job");
        }
    }
}

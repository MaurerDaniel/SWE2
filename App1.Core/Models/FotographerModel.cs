using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Core.Models
{
    public class FotographerModel
    {
        public string ID { get;  set; }
        public string Name { get;  set; }
        public string Surname { get;  set; }
        public DateTime Birthday { get;  set; }
        public string Notes { get;  set; }

        //public FotographerModel(int id, string name, string surname, DateTime birthday, string notes)
        //{
        //    ID = id;
        //    Name = name;
        //    Surname = surname;
        //    Birthday = birthday;
        //    Notes = notes;
        //}
    }

}

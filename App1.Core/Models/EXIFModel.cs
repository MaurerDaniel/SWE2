using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Core.Models
{
    public class EXIFModel 
    {
        public string Breite { get; set; }
        public string Hoehe { get;  set; }
        public string HorizontAufloesung { get;  set; }
        public string VertAufloesung { get;  set; }
        public string BildTiefe { get;  set; }
        public string Aufloesungseinheit { get;  set; }

        //public EXIFModel(int breite, int höhe, int horizontAufl, int vertAufl, int bildTiefe, int auflösEinheit)
        //{
        //    Breite = breite;
        //    Höhe = höhe;
        //    HorizontAuflösung = horizontAufl;
        //    VertAuflösung = vertAufl;
        //    BildTiefe = bildTiefe;
        //    Auflösungseinheit = auflösEinheit;
        //}
    }

}


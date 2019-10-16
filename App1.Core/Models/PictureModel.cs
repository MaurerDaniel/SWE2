using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Core.Models
{
    [Table("Images)")]
    public class PictureModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        public FotographerModel Owner { get; set; }
        public IPTCModel IPTC { get;  set; }
        public string Path { get; set; }
        public EXIFModel EXIF { get;  set; }

        //public PictureModel(string name, FotographerModel _owner , IPTCModel _iptc, EXIFModel _exif)
        //{
        //    Name = name;
        //    Owner = _owner;
        //    IPTC = _iptc;
        //    EXIF = _exif;
        //}

    }
}

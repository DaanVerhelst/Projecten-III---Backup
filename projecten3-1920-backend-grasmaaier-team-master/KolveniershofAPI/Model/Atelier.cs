using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using KolveniershofAPI.Model.Model_EF;
using System;

namespace KolveniershofAPI.Model
{
    public class Atelier {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Naam { get; set; }

        public Foto Pictogram { get; set; }

        public ICollection<Atelier_Dag> AtelierDagen { get; set; }

        public Atelier(){
            AtelierDagen = new List<Atelier_Dag>();
        }

        public Atelier(string naam):this(){
            Naam = naam;
        }
    }
}

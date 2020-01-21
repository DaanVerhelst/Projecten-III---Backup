using KolveniershofAPI.Model.Model_EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model
{
    public class Bus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Naam { get; set; }

        public Foto Pictogram { get; set; }

        public ICollection<Bus_Dag> BusDagen { get; set; }

        public Bus()
        {
            BusDagen = new List<Bus_Dag>();
        }

        public Bus(string naam) : this()
        {
            Naam = naam;
        }
    }
}

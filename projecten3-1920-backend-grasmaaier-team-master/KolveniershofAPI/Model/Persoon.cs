using System.ComponentModel.DataAnnotations.Schema;
using KolveniershofAPI.Model.Model_EF;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofAPI.Model
{
    public abstract class Persoon { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID{ get; set; }

        [NotMapped]
        public string Username { get => $"{Voornaam}.{Familienaam}"; }

        public string Voornaam { get; set; }
        public string Familienaam { get; set; }

        public Foto  ProfielFoto { get; set; }
        public long? ProfielFotoID { get; set; }

        public IEnumerable<Dag_Persoon> Dag_Personen { get; set; }
        [NotMapped]
        public IEnumerable<Dag> Personen {
            get => Dag_Personen.Select(dp => dp.Dag);
        }

    }
}

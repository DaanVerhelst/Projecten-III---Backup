using System.ComponentModel.DataAnnotations.Schema;

namespace KolveniershofAPI.Model.Model_EF{
    public class Atelier_Dag_Persoon{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Persoon Persoon { get; set; }
        public Atelier_Dag AD { get; set; }
    }
}

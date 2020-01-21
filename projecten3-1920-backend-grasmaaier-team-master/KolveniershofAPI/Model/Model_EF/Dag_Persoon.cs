using System.ComponentModel.DataAnnotations.Schema;

namespace KolveniershofAPI.Model.Model_EF{
    public class Dag_Persoon{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Persoon Persoon { get; set; }
        public Dag Dag { get; set; }

        public bool IsZiek { get; set; }

		public string commentaar { get; set; }

        public string rol { get; set; }
    }
}

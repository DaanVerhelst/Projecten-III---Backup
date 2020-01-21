using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolveniershofAPI.Model{
    public class NotiteLijsten{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Persoon Persoon { get; set; }
		public string Categorie { get; set; }
		public string Comment { get; set; }

		public NotiteLijsten(string comment, NotieblokCategorie cat){
			Comment = comment;
            Categorie = Enum.GetName(typeof(NotieblokCategorie), cat);
		}

		public NotiteLijsten(){}
	}
}

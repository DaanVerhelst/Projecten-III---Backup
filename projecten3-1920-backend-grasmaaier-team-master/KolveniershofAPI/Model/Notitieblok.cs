using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model
{
    public class Notitieblok{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public ICollection<NotiteLijsten> LijstNoties { get; set; }

		public Notitieblok(){
			LijstNoties = new List<NotiteLijsten>();
		}


		public void VoegIndividueleOndersteuningNotitieToe(DateTime t, Persoon pers, string wat, string wie) {
			LijstNoties.Add(new IndividueleOndersteuningClienten(pers, t, wie, wat));
		}

		public void VoegBijzondereUurRegelingToe(DateTime binnen, DateTime toekomst, Persoon pers, string reden)
		{
			LijstNoties.Add(new BijzonderUurRegistratie(pers, binnen, toekomst, reden));
		}

        public void VoegCommentToe(string comment, NotieblokCategorie cat) {
            NotiteLijsten lijst = LijstNoties.FirstOrDefault(ln => ln.Categorie == Enum.GetName(typeof(NotieblokCategorie),cat));

            if (lijst == null){
                lijst = new NotiteLijsten(comment,cat);
                LijstNoties.Add(lijst);
            }

            lijst.Comment = comment;
        }


		internal ICollection<String> GetCategoriën(){
			return Enum.GetNames(typeof(NotieblokCategorie));
		}

		internal void removeCommentaar(NotieblokCategorie catEnum, int commentID){
			LijstNoties.Remove(LijstNoties.FirstOrDefault(e => e.ID == commentID));
		}
	}
}

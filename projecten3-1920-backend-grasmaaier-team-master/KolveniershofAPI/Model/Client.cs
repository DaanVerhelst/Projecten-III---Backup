using KolveniershofAPI.Model.Model_EF;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using KolveniershofAPI.Model.DTO;

namespace KolveniershofAPI.Model{
    public class Client:Persoon{
        public long? SfeerGroepID { get; set; }
        public SfeerGroep SfeerGroep{ get; set; }

		public Client()
		{

		}
		public Client(PersoonDTO dto){
			Voornaam = dto.Voornaam;
			Familienaam = dto.Familienaam;
		}

    }
}

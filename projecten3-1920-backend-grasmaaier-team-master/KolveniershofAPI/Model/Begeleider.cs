using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using KolveniershofAPI.Model.Model_EF;
using System.Linq;

namespace KolveniershofAPI.Model { 
    public class Begeleider:Persoon{
        public bool IsStagair { get; set; }
        public bool IsAdmin { get; set; }

        public Begeleider()
        {

        }
    }
}

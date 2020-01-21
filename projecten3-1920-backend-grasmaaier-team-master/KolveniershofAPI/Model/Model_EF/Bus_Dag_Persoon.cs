using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Model_EF
{
    public class Bus_Dag_Persoon { 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public Persoon Persoon { get; set; }
    public Bus_Dag BD { get; set; }
}
}

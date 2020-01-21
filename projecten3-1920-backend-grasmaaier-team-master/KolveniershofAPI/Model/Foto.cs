using System.ComponentModel.DataAnnotations.Schema;

namespace KolveniershofAPI.Model
{
    public class Foto {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public byte[] FotoData{ get; set;}
        public string Extension{ get; set; }
        public string FileName { get; set; }
    }
}

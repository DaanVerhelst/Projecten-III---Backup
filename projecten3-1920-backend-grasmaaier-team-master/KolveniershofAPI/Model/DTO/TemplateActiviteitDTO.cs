using KolveniershofAPI.Model.Model_EF;
using System.Linq;

namespace KolveniershofAPI.Model.DTO
{
    public class TemplateActiviteitDTO
    {
        public AtelierDagDTO AtelierInfo { get; set; }
        public long[] Clients { get; set; }
        public long[] Begeleiders { get; set; }

        public TemplateActiviteitDTO(Atelier_Dag ad)
        {
            AtelierInfo = new AtelierDagDTO(ad);
            Clients = ad.Clienten.Select(c => c.ID).ToArray();
            Begeleiders = ad.Begeleiders.Select(c => c.ID).ToArray();
        }

        public TemplateActiviteitDTO() { }
    }
}
namespace KolveniershofAPI.Model.DTO
{
    public class BusDTO
    {
        public long BusID { get; set; }
        public string Naam { get; set; }

        public BusDTO(Bus b)
        {
            BusID = b.ID;
            Naam = b.Naam;
        }
    }
}

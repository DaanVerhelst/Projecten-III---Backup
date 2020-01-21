namespace KolveniershofAPI.Model.DTO{
    public class AtelierDTO{
        public long AtelierID{ get; set; }
        public string Naam { get; set; }

        public AtelierDTO(Atelier at){
            AtelierID = at.ID;
            Naam = at.Naam;
        }
    }
}

using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Seeding
{
    public class DataInit{
        private readonly ApplicationDBContext _context;

        private readonly string _jsonName = "/Data/Seeding/data.json";
        private static Random random = new Random();

        private readonly int aantalBegeleiders = 5;
        private string[] _firstNames;
        private string[] _lastNames;
        //private Foto _placeHolderUser;

        private ICollection<SfeerGroep> _sfeerGroepen;
        private ICollection<Begeleider> _begeleiders;
        private ICollection<Client> _clienten;
        private ICollection<Bus> _bussen;
        private List<DagMenu> _menus;


        private string[] _activities;
        private string[] _bussoorten;
        private Foto _placeHolderPicto;

        private ICollection<Dag> Dagen;
        private ICollection<Atelier> _ateliers;
        private readonly UserManager<IdentityUser> _um;


        private ICollection<DagTemplate> _templates;

        public DataInit(ApplicationDBContext context,UserManager<IdentityUser> userManager){
            _context = context;
            _um = userManager;
        }

        public void SeedData() {
            MakePlaceHolder();

            ReadJson();

            _begeleiders = MakeBegeleiders();
            _clienten = MakeClients();

            MakeClientAccounts().Wait();
            MakeBegeleidersAccount().Wait();

            _ateliers = MakeAteliers();
            _templates = MakeDagTempaltes();
            FillAteliers();
            AddAteliersToDays();


            _bussen = MakeBussen();
            //FillBussen();

            AddBussenToDays();

            MakeMenu();


            AddDataToDB();
        }

        private void AddAteliersToDays(){
            //Add every atelier to a random day
            Array.ForEach(_ateliers.ToArray(), (Atelier a) => {
                Dagen.ElementAt(random.Next(Dagen.Count)).AtelierToevoegen(a);
            });
        }


        private void AddBussenToDays()
        {
            //Add every atelier to a random day
            Array.ForEach(_bussen.ToArray(), (Bus b) => {
                Dagen.ElementAt(random.Next(Dagen.Count)).BusToevoegen(b);
            });
        }


        private void MakePlaceHolder()
        {
  /*          _placeHolderUser = new Foto()
            {
                Extension = ".jpg",
                FileName = "Test Foto2",
                FotoData = Encoding.ASCII.GetBytes("Nog meer Test Data")
            };*/
            _placeHolderPicto = new Foto()
            {
                Extension = ".png",
                FileName = "TestPicto",
                FotoData = Encoding.ASCII.GetBytes("Test data van een pictogram")
            };

            Dag Today = new Dag() { Date = DateTime.Now.AddDays(0) };
            AddCommentsToDay(Today);

            Dagen = new List<Dag>();
            Dagen.Add(new Dag() { Date = DateTime.Now.AddDays(-2) });
            Dagen.Add(new Dag() { Date = DateTime.Now.AddDays(-1) });
            Dagen.Add(Today);
           
        }

        private void AddCommentsToDay(Dag dag) {
            dag.voegNotitieToe("Beige bus rijd vandaag niet omwille van problemen met eendere rid.","Vervoer");
            dag.voegNotitieToe("Sofie is vandaag geopereerd, lut is heel de week afwezig", "Begeleiding");
            dag.voegNotitieToe("Nick afscheid Jochem", "Varia");
            dag.voegNotitieToe("Heel druk in het verkeer waarschijnlijk vertragingen", "Logistiek");
        }

        private void AddDataToDB() {
            using (_context) {
                _context.Bussen.AddRange(_bussen);
                _context.Clienten.AddRange(_clienten);
                _context.Begeleiders.AddRange(_begeleiders);
                _context.Ateliers.AddRange(_ateliers);
                _context.Dagen.AddRange(Dagen);
                _context.Templates.AddRange(_templates);
                _context.DagMenus.AddRange(_menus);
                _context.SaveChanges();
            }
        }

        private void ReadJson() {
            JsonTextReader reader = new JsonTextReader(
             new StreamReader($"{Directory.GetCurrentDirectory()}{_jsonName}"));
            using (reader){
                JObject obj = (JObject)JToken.ReadFrom(reader);

                JArray ja = ((JArray)obj.GetValue("firstNames"));
                _firstNames = ja.Select(val => (string)val).ToArray();

                ja = ((JArray)obj.GetValue("lastNames"));
                _lastNames = ja.Select(val => (string)val).ToArray();

                ja = ((JArray)obj.GetValue("sfeerGroups"));
                _sfeerGroepen = ja.Select(val=> {
                    return new SfeerGroep() { Naam = (string)val };
                }).ToList();

                ja = ((JArray)obj.GetValue("Activities"));
                _activities = ja.Select(val => (string)val).ToArray();

                ja = ((JArray)obj.GetValue("Bussoorten"));
                _bussoorten = ja.Select(val => (string)val).ToArray();

                if (_lastNames == null || _firstNames == null || _sfeerGroepen==null)
                    throw new ArgumentException("Er ging iets fout in de json parsing");
            }
        }

        private ICollection<Client> MakeClients() {
            return this._firstNames.Reverse().TakeWhile((val, indx) => indx < _firstNames.Length-aantalBegeleiders)
                .Select((val,indx)=> {
                    return new Client()
                    {
                        //ProfielFoto = _placeHolderUser,
                        Voornaam = val,
                        Familienaam = _lastNames.Count() > indx ? _lastNames.Reverse().ToArray()[indx]
                                       : MakeRandomString(10),
                        SfeerGroep = _sfeerGroepen.ElementAt(random.Next(_sfeerGroepen.Count))
                    };
                }).ToList();
        }

        private ICollection<Bus> MakeBussen()
        {
            return _bussoorten.Select(val => {

                string fotoNaam = val.ToUpper().Replace(' ', '-');
                byte[] bytes = File.ReadAllBytes($"Data/Seeding/Pictos/{fotoNaam}.jpg");
                Console.WriteLine(bytes);

                Foto foto = new Foto() { Extension = "jpg", FileName = fotoNaam, FotoData = bytes };

                return new Bus()
                {
                    Naam = val,
                    Pictogram = foto
                };
            }).ToList();
        }

        private ICollection<Atelier> MakeAteliers() {
            return _activities.Select(val =>{

                string fotoNaam = val.ToUpper().Replace(' ', '-');
                byte[] bytes = File.ReadAllBytes($"Data/Seeding/Pictos/{fotoNaam}.jpg");
                Console.WriteLine(bytes);

                Foto foto = new Foto() { Extension = "jpg", FileName = fotoNaam,FotoData=bytes };

                return new Atelier() {
                    Naam = val,
                    Pictogram = foto
                };
            }).ToList();
        }

        private void FillAteliers() {
            /*Needs rrewrite
            foreach (Atelier at in _ateliers) {
                ((DagTemplate)at).VoegBegeleiderToeAanDag(_begeleiders.ElementAt(random.Next(_begeleiders.Count)), Dagen.ElementAt(random.Next(Dagen.Count)));
                //Make sure there's at least one client
                Client randomCli = _clienten.ElementAt(random.Next(_clienten.Count));
                at.VoegClientToeAanDag(randomCli, Dagen.ElementAt(random.Next(Dagen.Count)));

                foreach (Client cl in _clienten) {
                    if (cl != randomCli && random.Next(100)>45) {
                        at.VoegClientToeAanDag(cl, Dagen.ElementAt(random.Next(Dagen.Count)));
                        // at.VoegClientToe(cl);
                    }
                }
            }
            */
        }


        public ICollection<DagTemplate> MakeDagTempaltes()
        {
            ICollection<DagTemplate> templates = new List<DagTemplate>();
            for (int i = 0; i <= 20; i++)
            {
                DagTemplate temp = new DagTemplate();
                List<Atelier> ateliersOpDag = new List<Atelier>();
                for (int j = 0; j <= random.Next(6); j++)
                {

                    Atelier[] ateliers = _ateliers.Except(ateliersOpDag).ToArray();
                    Atelier at = ateliers[random.Next(ateliers.Count())];

                    List<Begeleider> begeleiders = new List<Begeleider>();
                    for (int x = 0; x <= random.Next(begeleiders.Count); x++)
                    {
                        Begeleider[] beg = _begeleiders.Except(begeleiders).ToArray();
                        begeleiders.Add(beg[random.Next(beg.Count())]);
                    }

                    List<Client> clients = new List<Client>();
                    for (int y = 0; y <= random.Next(_clienten.Count); y++)
                    {
                        Client[] clienten = _clienten.Except(clients).ToArray();
                        Client cl = clienten[random.Next(clienten.Count())];
                        clients.Add(cl);
                    }


                    int vm = random.Next(2);
                    temp.AtelierToevoegenOpTijdstip(at, vm == 0 ? new TimeSpan(7, 0, 0) : new TimeSpan(14, 0, 0), 
                        vm == 0 ? new TimeSpan(11, 0, 0): new TimeSpan(17, 0, 0));
                    temp.VoegClientenToeAanAtelier(at, clients);
                    temp.VoegBegeleidersToeAanAtelier(at, begeleiders);
                }
                temp.WeekNR = (int)Math.Ceiling((((decimal)i) / 5));
                temp.DagNR = (i % 5) + 1;
                templates.Add(temp);
            }

            return templates;

        }

        private ICollection<Begeleider> MakeBegeleiders(){
            if (_firstNames.Length < aantalBegeleiders || _lastNames.Length < aantalBegeleiders)
                throw new ArgumentException($"Je hebt minstens {aantalBegeleiders} voornamen en familienamen " +
                    $"nodig in de json file");

                return _firstNames.TakeWhile((val, indx) => indx < aantalBegeleiders)
                       .Select((val, indx) =>{
                           return new Begeleider(){
                               //ProfielFoto = _placeHolderUser, 
                               IsAdmin = false,
                               IsStagair = false,
                               Voornaam = val,
                               Familienaam = _lastNames[indx]
                           };
                       }).ToList();
        }

        private async Task MakeClientAccounts() {
            foreach (Client cl in _clienten) {
                IdentityUser iu = MakeIdentityUser(cl);

                var res = await _um.CreateAsync(iu, "test123");
                if (res.Succeeded) {
                    await _um.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Client"));
                }
            }
        }

        private async Task MakeBegeleidersAccount() {
            foreach (Begeleider beg in _begeleiders) {
                IdentityUser iu = MakeIdentityUser(beg);

                var res = await _um.CreateAsync(iu, "admin123");
                if (res.Succeeded){
                    await _um.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Begeleider"));
                    await _um.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Admin"));
                }
            }
        }

        private IdentityUser MakeIdentityUser(Persoon per) { 
            return new IdentityUser(){
                Email = $"{per.Username}@mail.be",
                UserName = per.Username,
                EmailConfirmed = true
            };
        }

        private string MakeRandomString(int length) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void MakeMenu()
        {
            _menus = new List<DagMenu>();

            DagMenu m1 = new DagMenu(1, "Maandag", "Groentesoep", "Prei", "Steak", "Kroket");
            DagMenu m2 = new DagMenu(2, "Dinsdag", "Vissoep","Wortelen","Worst","Frietjes");
            DagMenu m3 = new DagMenu(3, "Woensdag", "Kippensoep","Rode kool","Vleesvervanger","Aardappelen");
            DagMenu m4 = new DagMenu(4, "Donderdag", "Tomatensoep","Bloemkool","Kippenfilet","Aardappelen");
            DagMenu m5 = new DagMenu(5, "Vrijdag", "Spruitensoep","Appelmoes","Worst","Aardappelen");

            _menus.AddRange(new DagMenu[]{ m1,m2,m3,m4,m5});
        }
    }
}
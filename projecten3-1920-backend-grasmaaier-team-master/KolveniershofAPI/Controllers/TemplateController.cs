using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Policy = "Begeleider")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IAtelierRepository _atelierRepository;
        private readonly IBegeleiderRepository _begeleiderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAtelierDagRepository _atDagRepository;

        public TemplateController(ITemplateRepository tempRepo, IAtelierRepository atRepo,
            IBegeleiderRepository begRepo, IClientRepository clientRepo, IAtelierDagRepository atdaRepo)
        {
            _templateRepository = tempRepo;
            _atelierRepository = atRepo;
            _begeleiderRepository = begRepo;
            _clientRepository = clientRepo;
            _atDagRepository = atdaRepo;
        }

        [Route("week")]
        [HttpGet]

        public ActionResult<int[]> GetWeken()
        {
            return _templateRepository.GeefWeken();
        }


        [Route("dag/{dag}/week/{week}/create/")]
        [HttpPost]
        public IActionResult VoegActiviteitToeAanTemplate(int dag, int week, [FromBody] TemplateActiviteitDTO[] DTO)
        {
            try
            {
                DagTemplate ad = _templateRepository.GeefTemplate(dag, week);

                if (ad != null)
                    _templateRepository.Verwijder(ad);

                ad = new DagTemplate() { DagNR = dag, WeekNR = week };

                foreach (TemplateActiviteitDTO dto in DTO)
                {
                    Atelier at = _atelierRepository.GetById(dto.AtelierInfo.AtelierID);

                    if (at == null)
                        return BadRequest("We couldn't find the atelier you specified");

                    if (dto.AtelierInfo.Start != null && dto.AtelierInfo.Eind != null)
                        ad.AtelierToevoegenOpTijdstip(at, dto.AtelierInfo.Start, dto.AtelierInfo.Eind);
                    else
                        ad.AtelierToevoegen(at);
                    Atelier_Dag a_d = ad.GetAtelierDagVanAtelier(at);

                    if (dto.Clients.Count() != 0)
                    {
                        ICollection<Persoon> clients = new List<Persoon>();

                        foreach (long id in dto.Clients)
                        {
                            Client cl = _clientRepository.GetById(id);
                            if (cl == null)
                                return BadRequest("We couldn't find the client you specified");

                            clients.Add(cl);
                        }

                        a_d.UpdatePersonen(clients);
                    }

                    if (dto.Begeleiders.Count() != 0)
                    {
                        ICollection<Persoon> begeleiders = new List<Persoon>();

                        foreach (long id in dto.Begeleiders)
                        {
                            Begeleider beg = _begeleiderRepository.GetBegeleiderByID(id);
                            if (beg == null)
                                return BadRequest("We couldn't find the client you specified");

                            begeleiders.Add(beg);
                        }

                        a_d.UpdatePersonen(begeleiders);
                    }
                }

                _templateRepository.AddTemplate(ad);
                _atelierRepository.saveChanges();
                return Created($"api/Template/dag/{dag}/week/{week}", new TemplateDTO(ad));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}")]
        [HttpGet]
        public ActionResult<TemplateDTO> GetDagTemplate(int dag, int week)
        {
            try
            {
                DagTemplate dt = GetDagTemplateRepo(dag, week);
                return new TemplateDTO(dt);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("dag/{dag}/week/{week}/activiteit/{ActiviteitID}/clienten")]
        [HttpGet]
        public ActionResult<ICollection<PersoonDTO>> GeefClientenInActiviteit(int dag, int week, int ActiviteitID)
        {
            try
            {
                Atelier_Dag ad = getAtelierDag(dag, week, ActiviteitID);
                return ad.Clienten.Select(c => new PersoonDTO(c)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activiteit/{ActiviteitID}/clienten")]
        [HttpDelete]
        public ActionResult<ICollection<PersoonDTO>> VerwijderClientenUitActiviteitTemplate(int dag, int week, int ActiviteitID, [FromBody] int[] clientenId)
        {
            try
            {
                Atelier_Dag ad = getAtelierDag(dag, week, ActiviteitID);
                ICollection<Persoon> personen = new List<Persoon>();

                Array.ForEach(clientenId, cl => {
                    Client c = _clientRepository.GetById(cl);

                    if (c == null)
                        throw new ArgumentException("The client you specified doesn't exits");

                    personen.Add(c);
                });

                ad.VerwijderPersonen(personen);
                _atelierRepository.saveChanges();
                return personen.Select(p => new PersoonDTO(p)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activiteit/{ActiviteitID}/begeleider")]
        [HttpDelete]
        public ActionResult<ICollection<PersoonDTO>> VerwijderBegeleiderUitActiviteitTemplate(int dag, int week, int ActiviteitID, [FromBody] int[] begeleiderId)
        {
            try
            {
                Atelier_Dag ad = getAtelierDag(dag, week, ActiviteitID);
                ICollection<Persoon> personen = new List<Persoon>();

                Array.ForEach(begeleiderId, beg => {
                    Begeleider b = _begeleiderRepository.GetBegeleiderByID(beg);

                    if (b == null)
                        throw new ArgumentException("The client you specified doesn't exits");

                    personen.Add(b);
                });

                ad.VerwijderPersonen(personen);
                _atelierRepository.saveChanges();
                return personen.Select(p => new PersoonDTO(p)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [Route("dag/{dag}/week/{week}/activiteit/{ActiviteitID}/begeleiders")]
        [HttpGet]
        public ActionResult<ICollection<PersoonDTO>> GeefBegeleidersInActiviteit(int dag, int week, int ActiviteitID)
        {
            try
            {
                Atelier_Dag ad = getAtelierDag(dag, week, ActiviteitID);
                return ad.Begeleiders.Select(b => new PersoonDTO(b)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("week/{week}")]
        [HttpGet]
        public ICollection<AtelierDTO[]> GeefWeekTemplate(int week)
        {
            DagTemplate[] dt = _templateRepository.GeefWeekTemplate(week);
            List<AtelierDTO[]> adto = new List<AtelierDTO[]> { null, null, null, null, null };

            foreach (DagTemplate temp in dt)
            {
                AtelierDTO[] adt = temp.Atelier_Dag.Select(ad => new AtelierDTO(ad.Atelier)).ToArray();
                adto[temp.DagNR - 1] = adt;
            }

            return adto;
        }

        [Route("busweek/{week}")]
        [HttpGet]
        public ICollection<BusDTO[]> GeefBusWeekTemplate(int week)
        {
            DagTemplate[] dt = _templateRepository.GeefWeekTemplate(week);
            List<BusDTO[]> adto = new List<BusDTO[]> { null, null, null, null, null };

            foreach (DagTemplate temp in dt)
            {
                BusDTO[] adt = temp.Bus_Dag.Select(ad => new BusDTO(ad.Bus)).ToArray();
                adto[temp.DagNR - 1] = adt;
            }

            return adto;
        }
        [Route("dag/{dag}/week/{week}/delete")]
        [HttpDelete]
        public ActionResult<TemplateDTO> VerwijderTemplate(int dag, int week)
        {
            try
            {
                DagTemplate dt = GetDagTemplateRepo(dag, week);
                _templateRepository.Verwijder(dt);
                return new TemplateDTO(dt);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activiteit/{ActiviteitID}/begeleiders/")]
        [HttpPut]
        public ActionResult<ActiviteitPersonenDTO> VoegBegeleidersToeAanTemplateActiviteit(int dag, int week,
            long ActiviteitID, [FromBody] long[] begeleiderIDs)
        {

            try
            {
                DagTemplate dt = GetDagTemplateRepo(dag, week);
                Atelier at = GetAtelierFromRepo(ActiviteitID);

                ICollection<Persoon> begeleiders = new List<Persoon>();

                Array.ForEach(begeleiderIDs, (long id) => {
                    Begeleider beg = _begeleiderRepository.GetBegeleiderByID(id);

                    if (beg == null)
                        throw new ArgumentException("We couldn't find the begeleider you specified");

                    begeleiders.Add(beg);
                });

                dt.GetAtelierDagVanAtelier(at).UpdatePersonen(begeleiders);
                _atelierRepository.saveChanges();
                return new ActiviteitPersonenDTO(at, begeleiders.ToArray());

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activiteiten")]
        [HttpGet]
        public ActionResult<IEnumerable<TemplateActiviteitDTO>> GetActiviteitenInTemplate(int dag, int week)
        {
            try
            {
                DagTemplate dt = GetDagTemplateRepo(dag, week);
                return dt.Atelier_Dag.Select(ad => new TemplateActiviteitDTO(ad)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activiteit/{ActiviteitID}/clienten/")]
        [HttpPut]
        public ActionResult<ActiviteitPersonenDTO> VoegClientenToeAanTemplateActiviteit(int dag, int week,
            int ActiviteitID, [FromBody]long[] clientenID)
        {
            try
            {
                DagTemplate dt = GetDagTemplateRepo(dag, week);
                Atelier at = GetAtelierFromRepo(ActiviteitID);

                ICollection<Persoon> clients = new List<Persoon>();

                Array.ForEach(clientenID, (long id) => {
                    Client cl = _clientRepository.GetById(id);

                    if (cl == null)
                        throw new ArgumentException("We couldn't find the client you specified");

                    clients.Add(cl);
                });

                dt.GetAtelierDagVanAtelier(at).UpdatePersonen(clients);
                _atelierRepository.saveChanges();
                return new ActiviteitPersonenDTO(at, clients.ToArray());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activitieit/{ActiviteitID}/delete")]
        [HttpDelete]
        public ActionResult<TemplateActiviteitDTO> VerwijderActiviteitUitTemplate(int dag, int week, int ActiviteitID)
        {
            try
            {
                Atelier_Dag ad = getAtelierDag(dag, week, ActiviteitID);

                if (ad == null)
                    throw new ArgumentNullException("The activity isn't associated to this template");

                _atDagRepository.Delete(ad);
                return new TemplateActiviteitDTO(ad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("dag/{dag}/week/{week}/activiteit/")]
        [HttpPut]
        public ActionResult<TemplateActiviteitDTO> VeranderDuurVanActiviteit(int dag, int week, [FromBody]AtelierDagDTO DTO)
        {
            try
            {
                Atelier_Dag ad = getAtelierDag(dag, week, DTO.AtelierID);
                ad.Start = DTO.Start == null ? new TimeSpan() : DTO.Start;
                ad.End = DTO.Eind == null ? new TimeSpan() : DTO.Eind;

                _atelierRepository.saveChanges();
                return new TemplateActiviteitDTO(ad);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private DagTemplate GetDagTemplateRepo(int dag, int week)
        {
            DagTemplate dagTemp = _templateRepository.GeefTemplate(dag, week);

            if (dagTemp == null)
                throw new ArgumentException("We couldn't find the specified template");

            return dagTemp;
        }

        private Atelier GetAtelierFromRepo(long atelierID)
        {
            Atelier at = _atelierRepository.GetById(atelierID);

            if (at == null)
                throw new ArgumentException("The specified atelier doesn't exist");

            return at;
        }

        private Atelier_Dag getAtelierDag(int dag, int week, long ActiviteitID)
        {
            DagTemplate dt = GetDagTemplateRepo(dag, week);
            Atelier at = _atelierRepository.GetById(ActiviteitID);
            return dt.GetAtelierDagVanAtelier(at);
        }

    }
}
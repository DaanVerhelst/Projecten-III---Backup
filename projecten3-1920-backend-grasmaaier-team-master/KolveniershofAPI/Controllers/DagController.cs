using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Extensions;
using KolveniershofAPI.Model.Interface;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class DagController : Controller
    {
		private readonly ITemplateRepository _tRepo;
		private readonly IDagRepository _dagRepo;
		private readonly IClientRepository _cRepo;
		private readonly IAtelierRepository _atRepo;
		private readonly IBegeleiderRepository _beRepo;

		public DagController(IBegeleiderRepository beRepo, IClientRepository cRepo,IAtelierRepository atRepo, ITemplateRepository templateRepo, IDagRepository dagRepo)
		{
			_tRepo = templateRepo;
			_dagRepo = dagRepo;
			_cRepo = cRepo;
			_atRepo = atRepo;
			_beRepo = beRepo;
		}


        #region Make Template
        [HttpPost("concrete/{dt}/{weekNr}")]
        public ActionResult<DagDTO[]> CopyTemplate(DateTime dt, int weekNr)
        {
            DateTime[] week = dt.GeefWeek();

            Dag[] dagenDb = week.Select(d => _dagRepo.GetDagByDay(d)).ToArray();
            foreach (Dag d in dagenDb)
            {
                if (d != null)
                    _dagRepo.RemoveDag(d);
            }

            DagTemplate[] dtemp = _tRepo.GeefWeekTemplate(weekNr);
            List<Dag> dagen = new List<Dag>();

            foreach (DagTemplate dagTemp in dtemp)
            {

                int dagNr = dagTemp.DagNR;
                DateTime d = week[dagNr-1];

                Dag dag = new Dag() { Date = d };
                dag.Atelier_Dag = dagTemp.Atelier_Dag.Select(ad =>
                    new Atelier_Dag() { Start = ad.Start, Template = dag, End = ad.End, Atelier = ad.Atelier, ADP = ad.ADP }
                ).ToList();

                dagen.Add(dag);
                _dagRepo.AddDag(dag);
            }

            _dagRepo.SaveChanges();
            return dagen.Select(d=>new DagDTO(d)).ToArray();
        }

        [HttpPost("concrete/{dt}")]
        public ActionResult<DagDTO> UpdateDag(DateTime dt, [FromBody] TemplateActiviteitDTO[] DTO) {
            Dag d = _dagRepo.GetDagByDay(dt);
            _dagRepo.RemoveDag(d);
            _dagRepo.SaveChanges();

            d = new Dag() { Date = dt };

            foreach (var x in DTO) {
                Atelier at = _atRepo.GetById(x.AtelierInfo.AtelierID);

                if (x.AtelierInfo.Eind != null && x.AtelierInfo.Start != null){
                    d.AtelierToevoegenOpTijdstip(at, x.AtelierInfo.Start, x.AtelierInfo.Eind);
                }else{
                    d.AtelierToevoegen(at);
                }

                List<Begeleider> begeleiders = new List<Begeleider>();
                foreach (var begId in x.Begeleiders) {
                    Begeleider b = _beRepo.GetBegeleiderByID(begId);
                    begeleiders.Add(b);
                }
                d.VoegBegeleidersToeAanAtelier(at, begeleiders);


                List<Client> clients = new List<Client>();
                foreach (var clId in x.Clients){
                    Client b = _cRepo.GetById(clId);
                    clients.Add(b);
                }
                d.VoegClientenToeAanAtelier(at, clients);
            }

            _dagRepo.AddDag(d);
            _dagRepo.SaveChanges();
            return new DagDTO(d);
        }

        [HttpGet("HasTemplate/{dt}")]
        public ActionResult<bool> HasTemplate(DateTime dt)
        {
            DateTime[] week = dt.GeefWeek();

            foreach (DateTime dateTime in week)
            {
                bool hasTemp = _dagRepo.HeeftTemplate(dateTime);

                if (hasTemp)
                    return hasTemp;
            }

            return false;
        } 
        #endregion

        #region Week

        // GET: api/Dag/week/11022019T10:00:00
        /// <summary>
        /// Get all activities during an entire week
        /// </summary>
        /// <returns>array of activities</returns>
        /// 
        [Route("week/{start}")]
		[HttpGet]
		public ActionResult<DagDTO[]> GetWeek(DateTime start){
            return _dagRepo.Getweek(start).Select(d => {
                //if (d != null)
                    return new DagDTO(d);

                //return null;
            }).OrderBy(o=>o.Date).ToArray();
		}

        // GET: api/Dag/week/2019-12-06/client/2
        /// <summary>
        /// Get all activities during an entire week for a specific client
        /// </summary>
        /// <returns>array of activities</returns>
        /// 
        [Route("week/{start}/client/{id}")]
        [HttpGet]
        public ActionResult<DagDTO[]> GetWeekForClient(DateTime start, long id) {


            return _dagRepo.GetweekForClient(start, id).Select(e => new DagDTO(e)).ToArray();
        }

		#endregion

		#region Dag

		#region Get
		// GET: api/Dag/day/11022019T10:00:00
		/// <summary>
		/// Get all activities during an entire day
		/// </summary>
		/// <returns>array of activities</returns>
		/// 
		[Route("day/{start}")]
		[HttpGet]
		public ActionResult<DagDTO> GetDag(DateTime start)
		{
			try{
                Dag d = _dagRepo.GetDagByDay(start);
                return new DagDTO(d);
            } catch (Exception e){
				return BadRequest(e.Message);
			}
			
		}

		// GET: api/Dag/day/client/2/11022019T10:00:00
		/// <summary>
		/// Get all clients in an activity
		/// </summary>
		/// <returns>array of clients</returns>
		/// 
		[Route("day/clients/{activiteitID}/{start}")]
		[HttpGet]
		public ActionResult<IEnumerable<Client>> GeefClientenInActiviteit(DateTime dag, int activiteitID)
		{
			try
			{
				return _dagRepo.GetClientsInActivityByDay(dag, activiteitID).ToList();
			} catch(Exception e) {
				return BadRequest(e.Message);
			}
			
		}

		// GET: api/Dag/day/begeleider/2/11022019T10:00:00
		/// <summary>
		/// Get all begeleiders in an activity
		/// </summary>
		/// <returns>array of begeleiders</returns>
		/// 
		[Route("day/begeleider/{activiteitID}/{start}")]
		[HttpGet]
		public ActionResult<IEnumerable<Begeleider>> GeefBegeleidersVanActiviteit(DateTime dag, int activiteitID)
		{
			try
			{
				return _dagRepo.GetBegeleidersInActivity(dag, activiteitID).ToList();

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		#endregion

		#region Post

		// POST: api/Dag/weekend/somecomment/11022019T10:00:00
		/// <summary>
		/// Add commentaar to weekend
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("weekend/notes/{dag}/{clientID}/{comment}")]
		[HttpPost]
		public ActionResult<Dag> VoegCommentaarToeAanWeekend(DateTime dag, int clientID, string comment)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				Persoon p = _cRepo.GetById(clientID);
                if (d == null)
                {
                    d = new Dag();
                    d.Date = dag;
                    _dagRepo.AddDag(d);
                }
                d.voegCommentaarToe(comment, p);
				_dagRepo.SaveChanges();
				return Created(nameof(GetDag), d.Dag_Personen.FirstOrDefault(e => e.Persoon == p).commentaar);
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
        
       

        // POST: api/Dag/day/notes/11022019T10:00:00/somecomment/somesection
        /// <summary>
        /// Add commentaar to Dag
        /// </summary>
        /// <returns>n/a</returns>
        /// 
        [Route("day/notes/{dag}/{catEnum}/{commentaar}")] //catEnum = sectie voor commentaar
		[HttpPost]
		public ActionResult<Dag> VoegCommentaarToeAanDag(DateTime dag, String catEnum, String commentaar)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);

                if (d == null) {
                    d = new Dag() { Date = dag};
                    _dagRepo.AddDag(d);
                    _dagRepo.SaveChanges();
                }

				d.voegNotitieToe(commentaar, catEnum);
				_dagRepo.SaveChanges();
				return Created(nameof(GetDag), d.Notities);
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: api/Dag/day/absent/11022019T10:00:00/2
		/// <summary>
		/// Set certain client to absent.
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/absent/{dag}/{id}")]
		[HttpPost]
		public IActionResult PlaatsClientOpAfwezigAanDag(DateTime dag, int clientID)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				Persoon p = _cRepo.GetById(clientID);

				d.zetPersoonOpAfwezig(p);
				_dagRepo.SaveChanges();

				return NoContent();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: api/Dag/day/specialHour/11022019T10:00:00/2
		/// <summary>
		/// Add a special Hour arrangement to a day, voor een client
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/specialHour/{dag}/{clientID}")]
		[HttpPost]
		public ActionResult<Dag> VoegRijToeBijzondereUurRegeling(DateTime dag, DateTime buiten, DateTime toekomst,string  reden, int clientID) {
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				Persoon p = _cRepo.GetById(clientID);
				d.VoegBijzondereUurRegeling(buiten, toekomst, p, reden);
				_dagRepo.SaveChanges();
				return Created(nameof(GetDag), d.Notities);
			} catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: api/Dag/day/individual/11022019T10:00:00/2
		/// <summary>
		/// Add individual measures to a day, voor een client
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/individual/{dag}/{clientID}")]
		[HttpPost]
		public IActionResult VoegRijToeIndividueleOndersteuning(DateTime dag, DateTime tijd, string wat, string wie, int clientID)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				Persoon p = _cRepo.GetById(clientID);
				d.VoegIndividueleOndersteuningToe(tijd, p, wat, wie);
				_dagRepo.SaveChanges();
				return Created(nameof(GetDag), d.Notities);
			}catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: api/Dag/day/activity/2/11022019T10:00:00
		/// <summary>
		/// Add an activity to a day.
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/activity/{activityID}/{dag}")]
		[HttpPost]
		public ActionResult<Dag> VoegActiviteitToeAanDag(DateTime dag, AtelierDagDTO dto)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);

				if (d == null)
				{
					return BadRequest("This day does not exist");
				}
				else
				{
					Atelier at = _atRepo.GetById(dto.AtelierID);
					d.AtelierToevoegenOpTijdstip(at, dto.Start, dto.Eind);
					_dagRepo.SaveChanges();
					return Created(nameof(GetDag), d.Atelier_Dag);
				}
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			
		}

		// POST: api/Dag/day/begeleider/2/11022019T10:00:00
		/// <summary>
		/// Add begeleiders to an activity on a day
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/begeleider/{activityID}/{dag}/{begeleiderIDs}")]
		[HttpPost]
		public ActionResult<Dag> VoegBegeleidersToeAanActiviteitDag(DateTime dag, int activiteitID, int[] begeleiderIDs)
		{
			try
			{

				Dag d = _dagRepo.GetDagByDay(dag);
				if (d == null)
				{
					return BadRequest("This day does not exist");
				}
				else
				{

					ICollection<Begeleider> begeleiders = new List<Begeleider>();
					Array.ForEach(begeleiderIDs, e =>
					{
						Begeleider b = _beRepo.GetBegeleiderByID(e);
						begeleiders.Add(b);
					});
					Atelier at = _atRepo.GetById(activiteitID);
					d.VoegBegeleidersToeAanAtelier(at, begeleiders);
					_dagRepo.SaveChanges();
					return Created(nameof(GetDag), d.Atelier_Dag);
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: api/Dag/day/clients/2/11022019T10:00:00
		/// <summary>
		/// Add clients to an activity on a day
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/client/{activityID}/{dag}/{clientIDs}")]
		[HttpPost]
		public ActionResult<Dag> VoegClientToeAanActiviteitDag(DateTime dag, int activiteitID, int[] clientIDs)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				if (dag == null)
				{
					return BadRequest("This day doesn't exist");
				}
				else
				{
					ICollection<Client> clients = new List<Client>();
					Array.ForEach(clientIDs, e =>
					{
						Client c = _cRepo.GetById(e);
						clients.Add(c);
					});
					Atelier at = _atRepo.GetById(activiteitID);
					d.VoegClientenToeAanAtelier(at, clients);
					_dagRepo.SaveChanges();
					return Created(nameof(GetDag), d.Atelier_Dag);
				}
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			
		}
		#endregion

		#region Put

		// PUT: api/Dag/day/hourChange/2/11022019T10:00:00
		/// <summary>
		/// Changes length of an activity
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/hourChange/{activityID}/{dag}/")]
		[HttpPut]
		public ActionResult<Dag> VeranderDuurVanActiviteit(DateTime dag, AtelierDagDTO dto)
		{
			try
			{

				Dag d = _dagRepo.GetDagByDay(dag);
				Atelier at = _atRepo.GetById(dto.AtelierID);
				d.pasDuurAtelierDagAan(at, dto.Eind);
				_dagRepo.SaveChanges();
				return d;
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		#endregion

		#region Delete

		// DELETE: api/Dag/day/11022019T10:00:00
		/// <summary>
		/// Remove activities from a day
		/// </summary>
		/// <returns>list of Atelier</returns>
		/// 
		[Route("day/{dag}/{activiteiten}")]
		[HttpDelete]
		public ActionResult<ICollection<Atelier>> VerwijderActiviteitenUitDag(int[] activiteiten, DateTime dag)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				ICollection<Atelier> adList = new List<Atelier>();
				Array.ForEach(activiteiten, e =>
				{
					adList.Add(_atRepo.GetById(e));
				});
				if (adList != null)
				{
					Array.ForEach(adList.ToArray(), e =>
					{
						d.verwijderAtelierUitDag(e);
					});
				}
				_dagRepo.SaveChanges();
				return adList.ToList();
			} catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// DELETE: api/Dag/day/begeleiders/11022019T10:00:00/2/
		/// <summary>
		/// Remove begeleiders from activity
		/// </summary>
		/// <returns>List of Begeleiders</returns>
		/// 
		[Route("day/begeleider/{dag}/{activiteitID}/{begeleiderIDs}")]
		[HttpDelete]
		public ActionResult<ICollection<Begeleider>> VerwijderBegeleidersUitDagActiviteit(int[] begeleiderIDs, int activiteitId, DateTime dag)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				Atelier at = _atRepo.GetById(activiteitId);
				ICollection<Begeleider> begeleiders = new List<Begeleider>();
				Array.ForEach(begeleiderIDs, e =>
				{
					begeleiders.Add(_beRepo.GetBegeleiderByID(e));
				});

				Array.ForEach(begeleiders.ToArray(), e =>
				{
					d.verwijderBegeleiderUitActiviteit(at, e);
				});

				_dagRepo.SaveChanges();
				return begeleiders.ToList();
			} catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// DELETE: api/Dag/day/clients/11022019T10:00:00/2/
		/// <summary>
		/// Remove clients from activity
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/clients/{dag}/{activiteitID}/{begeleiderIDs}")]
		[HttpDelete]
		public ActionResult<ICollection<Client>> VerwijderClientenUitDagActiviteit(int[] clientIDs, int activiteitId, DateTime dag)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				Atelier at = _atRepo.GetById(activiteitId);
				ICollection<Client> clients = new List<Client>();
				Array.ForEach(clientIDs, e =>
				{
					clients.Add(_cRepo.GetById(e));
				});
				Array.ForEach(clients.ToArray(), e =>
				{
					d.verwijderClientUitActiviteit(at, e);
				});

				_dagRepo.SaveChanges();
				return clients.ToList();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// DELETE: api/Dag/day/commentary/11022019T10:00:00/2/someSectie
		/// <summary>
		/// Remove commentary from day.
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("day/commentary/{dag}/{commentID}/{catEnum}")]
		[HttpDelete]
		public ActionResult<Dag> VerwijderCommentaarVanDag(DateTime dag, String catEnum, int commentID)
		{
			try
			{
				Dag d = _dagRepo.GetDagByDay(dag);
				if (d != null)
				{
					d.removeCommentaar(catEnum, commentID);
				} else
				{
					return BadRequest("This day does not exist.");
				}
				return d;
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		#endregion

		#endregion

		#region Notities

		// GET: api/Dag/notes/categories
		/// <summary>
		/// Give all Note Categories
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("notes/categories")]
		[HttpGet]
		public ActionResult<ICollection<String>> GeefNotitieCategorieën()
		{
			try {


				ICollection<String> categoriën = new List<String>();
				Dag d = new Dag();
				categoriën = d.GetNotitieCategoriën();
				return categoriën.ToList();
			}catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// GET: api/Dag/11022019T10:00:00/notes
		/// <summary>
		/// give all notes on a day
		/// </summary>
		/// <returns>n/a</returns>
		/// 
		[Route("{dag}/notes")]
		public ActionResult<Notitieblok> GeefNotitiesOpDag(DateTime dag)
		{
			try
			{

				Notitieblok n = _dagRepo.getNotitiesByDay(dag);

				if (n == null)
				{
					return BadRequest("Did not find any notities for the given day");
				}
				else
				{
					return n;
				}
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			
		}

        [Route("week/Test")]
        [HttpGet]
        public ActionResult<IEnumerable<Dag>> GetAlles()
        {
            return _dagRepo.GetAll().ToList();
        }



        #endregion

    }

    
}
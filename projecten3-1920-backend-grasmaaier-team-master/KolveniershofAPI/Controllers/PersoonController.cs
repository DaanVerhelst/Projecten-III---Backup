using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofAPI.Controllers
{
    //[ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class PersoonController : ControllerBase
    {
		
		private readonly IClientRepository _clientRepo;
		private readonly ISfeergroepRepository _sfeergroepRepo;
        private readonly IBegeleiderRepository _begeleiderRepo;
        private readonly IDagRepository _dagRepo;

		public static string[] ImageExt = { "jpg", "jpeg", "png", "tiff", "bmp", "gif" };

        public PersoonController(IClientRepository client, ISfeergroepRepository sfeergroep,
                IBegeleiderRepository begRepo)
		{
			_clientRepo = client;
			_sfeergroepRepo = sfeergroep;
            _begeleiderRepo = begRepo;
		}

		#region Client
		// GET: api/Client
		/// <summary>
		/// Get all clients ordered by id
		/// </summary>
		/// <returns>array of clients</returns>
		/// 
        [Route("clienten/")]
		[HttpGet]
		public ActionResult<PersoonDTO[]> GeefAlleClients()
		{
			try
			{
				return _clientRepo.GetAll().Select(c => new PersoonDTO(c)).ToArray();
			} catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        [Route("begeleiders/")]
        [HttpGet]
        public ActionResult<PersoonDTO[]> GeefBegeleiders() {
			try
			{
				return _begeleiderRepo.GetAllBegeleiders()
					   .Select(b => new PersoonDTO(b)).ToArray();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			} 
        }

		// POST: api/Persoon/
		/// <summary>
		/// Create a client
		/// </summary>
		/// <returns>Client that was created</returns>
		/// 
		[Route("")]
		[HttpPost]
		public IActionResult CreateClient(CreatePersoonDTO pers) //PersoonDTO mee geven als param
		{
			try
			{
				Client c = new Client {Voornaam = pers.Voornaam, Familienaam = pers.Familienaam };
				_clientRepo.AddClient(c);
				_clientRepo.saveChanges();
				//if(String.IsNullOrEmpty())
				return Created(nameof(GetClient), c.ID);
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        // POST: api/Persoon/begeleider
        /// <summary>
        /// Create a begeleider
        /// </summary>
        /// <returns>Begeleider that was created</returns>
        /// 
        [Route("begeleider")]
        [HttpPost]
        public IActionResult CreateBegeleider(CreateBegeleiderDTO pers) //PersoonDTO mee geven als param
        {
            try
            {
                Begeleider c = new Begeleider { Voornaam = pers.Voornaam, Familienaam = pers.Familienaam, IsAdmin = pers.IsAdmin };
                _begeleiderRepo.AddBegeleider(c);
                _begeleiderRepo.saveChanges();
                //if(String.IsNullOrEmpty())
                return Created(nameof(GetBegeleider), c.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // PUT: api/Persoon/update/3
        /// <summary>
        /// Update a certain client
        /// </summary>
        /// <returns>n/A</returns>
        /// 
        [Route("{id}")]
		[HttpPut]
		public IActionResult UpdateClient(long id, PersoonDTO client)
		{
			try
			{
				Client c = _clientRepo.GetById(id);
				if (c == null)
				{
					return NotFound();
				}

				if (client.Familienaam != "") { c.Familienaam = client.Familienaam; };
				if (client.Voornaam != "") { c.Voornaam = client.Voornaam; };

				_clientRepo.updateClient(c);
				_clientRepo.saveChanges();

				return NoContent();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// GET: api/Persoon/begeleider/2
		/// <summary>
		/// Get a begeleider by ID
		/// </summary>
		/// <returns>the begeleider you requested</returns>
		/// 
		[Route("begeleider/{id}")]
		[HttpGet]
		public ActionResult<Begeleider> GetBegeleider(long id)
		{
			try
			{
				Begeleider c = _begeleiderRepo.GetBegeleiderByID(id);
				if (c == null)
				{
					return NotFound();
				}

				return c;
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        // GET: api/Persoon/2
        /// <summary>
        /// Get a client by ID
        /// </summary>
        /// <returns>the client you requested</returns>
        /// 
        [Route("{id}")]
        [HttpGet]
        public ActionResult<Client> GetClient(long id)
        {
            try
            {
                Client c = _clientRepo.GetById(id);
                if (c == null)
                {
                    return NotFound();
                }

                return c;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Persoon/delete/2
        /// <summary>
        /// Delete a client by ID
        /// </summary>
        /// <returns>the deleted client</returns>
        /// 
        [Route("{id}")]
		[HttpDelete]
		public ActionResult<Client> DeleteClient(int id)
		{
			try
			{
				Client c = _clientRepo
					.GetById(id);

				if (c == null)
				{
					return NotFound();
				}

				_clientRepo.RemoveClient(c);
				_clientRepo.saveChanges();

				return c;
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        // GET: api/Persoon/begeleider/2
        /// <summary>
        /// Delete a client by ID
        /// </summary>
        /// <returns>the deleted client</returns>
        /// 
        [Route("begeleider/{id}")]
        [HttpDelete]
        public ActionResult<Begeleider> DeleteBegeleider(int id)
        {
            try
            {
                Begeleider c = _begeleiderRepo.GetBegeleiderByID(id);

                if (c == null)
                {
                    return NotFound();
                }

                _begeleiderRepo.RemoveBegeleider(c);
                _begeleiderRepo.saveChanges();

                return c;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region sfeergroep

        // POST: api/Persoon/sfeergroep/
        /// <summary>
        /// Create a sfeergroep
        /// </summary>
        /// <returns>The created sfeergroep</returns>
        /// 
        [Route("sfeergroep/{naam}")]
		[HttpPost]
		public IActionResult CreateSfeerGroep(string naam)
		{ //SfeergroepDTO meegeven
			try
			{
				if (String.IsNullOrEmpty(naam))
					return BadRequest("Please provide a name");

				SfeerGroep sg = new SfeerGroep { Naam = naam };
				_sfeergroepRepo.Create(sg);
				_sfeergroepRepo.SaveChanges();
				return Created(nameof(GetSfeerGroep), sg.ID);
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// PUT: api/Persoon/sfeergroep/2
		/// <summary>
		/// Update a sfeergroep
		/// </summary>
		/// <returns>Client that was created</returns>
		/// 
		[Route("sfeergroep/{id}")]
		[HttpPut]
		public IActionResult UpdateSfeerGroep(int id, SfeerGroep sfeerGroep)
		{ //SfeergroepDTO meegeven als param
			try
			{
				SfeerGroep s = _sfeergroepRepo.GetById(id);
				if (s == null)
				{
					return NotFound();
				}
				if (sfeerGroep.Naam != "") { s.Naam = sfeerGroep.Naam; }

				_sfeergroepRepo.Update(s);
				_sfeergroepRepo.SaveChanges();

				return NoContent();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// DELETE: api/Persoon/sfeergroep/2
		/// <summary>
		/// Delete a sfeergroep
		/// </summary>
		/// <returns>Sfeergroep that was deleted</returns>
		/// 
		[Route("sfeergroep/{id}")]
		[HttpDelete]
		public ActionResult<SfeerGroep> DeleteSfeerGroep(int id)
		{ //Sfeergroep ID mee geven als param
			try
			{
				SfeerGroep sg = _sfeergroepRepo.GetById(id);
				_sfeergroepRepo.Delete(sg);
				_sfeergroepRepo.SaveChanges();

				return sg;
			} catch( Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// GET: api/Persoon/sfeergroep/2
		/// <summary>
		/// Gets a sfeergroep
		/// </summary>
		/// <returns>Sfeergroep that was requested</returns>
		/// 
		[Route("sfeergroep/{id}")]
		[HttpGet]
		public ActionResult<SfeerGroep> GetSfeerGroep(int id)
		{ //Sfeergroep ID mee gegeven
			try
			{
				SfeerGroep sg = _sfeergroepRepo.GetById(id);
				if (sg == null)
				{
					return NotFound();
				}
				return sg;
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			
		}

		// GET: api/Persoon/sfeergroep
		/// <summary>
		/// Gets all sfeergroepen
		/// </summary>
		/// <returns>An array of sfeergroepen</returns>
		/// 
		[Route("sfeergroep")]
		[HttpGet]
		public ActionResult<IEnumerable<SfeerGroep>> GetAlleSfeergroepen()
		{
			try
			{
				IEnumerable<SfeerGroep> sgList = _sfeergroepRepo.GetAll();

				return sgList.ToList();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// PUT: api/Persoon/sfeergroep/setclient/2/2
		/// <summary>
		/// Connects a client to a sfeergroep
		/// </summary>
		/// <returns>Client that was updated</returns>
		/// 
		[Route("sfeergroep/setclient/{persoonID}/{sfeergroepID}")]
		[HttpPost]
		public IActionResult SetSfeerGroepVoorPersoon(int persoonID, int sfeerGroepID) //Geef de id van een persoon en sfeergroep
		{
			try
			{
				Client c = _clientRepo.GetById(persoonID);
				SfeerGroep s = _sfeergroepRepo.GetById(sfeerGroepID);
				c.SfeerGroep = s;
				_clientRepo.updateClient(c);
				_clientRepo.saveChanges();
				// Zorg ervoor dat je een persoon kan wijzigen van sfeergroep of een nieuwe kan toewijzen. 
				// Een persoon zit maar max in 1 sfeergroep.
				return Created(nameof(GetClient), c.ID);
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		#endregion

		#region profielfoto

		// Get: api/Persoon/profilepic/2
		/// <summary>
		/// Get the profile picture you requested.
		/// </summary>
		/// <returns>The requested profile picture</returns>
		/// 
		[Route("profilepic/{id}")]
		[HttpGet]
		public IActionResult GetProfielFoto(int id)
		{
			try
			{
				Client c = _clientRepo.GetById(id);
				var image = c.ProfielFoto;
				if (image == null)
				{
					return NoContent();
				}

				return File(image.FotoData, $"image/{image.Extension.ToLower()}", $"{image.FileName}.{image.Extension}");
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        // Get: api/Persoon/profilepic/2
        /// <summary>
        /// Get the profile picture you requested.
        /// </summary>
        /// <returns>The requested profile picture</returns>
        /// 
        [Route("begeleider/profilepic/{id}")]
        [HttpGet]
        public IActionResult GetProfielFotoB(int id)
        {
            try
            {
                Begeleider c = _begeleiderRepo.GetBegeleiderByID(id);
                var image = c.ProfielFoto;
                if (image == null)
                {
                    return NoContent();
                }

                return File(image.FotoData, $"image/{image.Extension.ToLower()}", $"{image.FileName}.{image.Extension}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Persoon/profilepic/update/{id}
        /// <summary>
        /// Update a picture for a certain client
        /// </summary>
        /// <returns>the updated picture</returns>
        /// 
        [Route("profilepic/{id}")]
		[HttpPut]
		public IActionResult UpdateProfielFoto(int id, [FromForm]IFormFile file)
		{
			try
			{
				if (file.Length > 20000000)
				{
					return BadRequest("Your file is too big");
				}

				var fileNameSplit = file.FileName.Split(".");
				string extension = fileNameSplit[fileNameSplit.Length - 1];
				if (!ImageExt.Contains("Your file is not an image.")) ;

				Client c = _clientRepo.GetById(id);

				if (c == null)
				{
					return NotFound();
				}

				string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
				using (var ms = new MemoryStream())
				{
					file.CopyTo(ms);
					var fileBytes = ms.ToArray();
					c.ProfielFoto = new Foto { FotoData = fileBytes, FileName = name, Extension = extension };
				}

				_clientRepo.updateClient(c);
				_clientRepo.saveChanges();

				return NoContent();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: api/Persoon/profilepic/create/2
		/// <summary>
		/// Add a picture for a certain client
		/// </summary>
		/// <returns>The created picture</returns>
		/// 
		[Route("{id}/profilepic")]
		[HttpPost]
		public IActionResult PostProfielFoto([FromForm(Name = "file")]IFormFile file, int id)
		{
			try
			{
				if (file.Length > 20000000)
				{
					return BadRequest("Your file is too big!");
				}
				var fileNameSplit = file.FileName.Split(".");
				string extension = fileNameSplit[fileNameSplit.Length - 1];
				if (!ImageExt.Contains(extension.ToLower()))
				{
					return BadRequest("Your file is not an image");
				}

				Client c = _clientRepo.GetById(id);

				string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
				using (var ms = new MemoryStream())
				{
					file.CopyTo(ms);
					var fileBytes = ms.ToArray();
					c.ProfielFoto = new Foto { FotoData = fileBytes, FileName = name, Extension = extension };
				}

				_clientRepo.saveChanges();
				//c.ProfielFoto
				return Created(nameof(GetProfielFoto), c.ID);
            } catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        // POST: api/Persoon/begeleider/2/profilepic
        /// <summary>
        /// Add a picture for a certain client
        /// </summary>
        /// <returns>The created picture</returns>
        /// 
        [Route("begeleider/{id}/profilepic")]
        [HttpPost]
        public IActionResult PostProfielFotoB([FromForm(Name = "file")]IFormFile file, int id)
        {
            try
            {
                if (file.Length > 20000000)
                {
                    return BadRequest("Your file is too big!");
                }
                var fileNameSplit = file.FileName.Split(".");
                string extension = fileNameSplit[fileNameSplit.Length - 1];
                if (!ImageExt.Contains(extension.ToLower()))
                {
                    return BadRequest("Your file is not an image");
                }

                Begeleider c = _begeleiderRepo.GetBegeleiderByID(id);

                string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    c.ProfielFoto = new Foto { FotoData = fileBytes, FileName = name, Extension = extension };
                }

                _begeleiderRepo.saveChanges();
                //c.ProfielFoto
                return Created(nameof(GetProfielFoto), c.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Busregeling

        #endregion


    }
}
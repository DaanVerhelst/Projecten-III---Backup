using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofAPI.Controllers {

	[Route("api/[controller]")]
    [ApiController]
    public class AtelierController : ControllerBase{
        private readonly IAtelierRepository _atRepo;
        public static string[] ImageExt = { "jpg", "jpeg", "png", "tiff", "bmp", "gif" };
        public AtelierController(IAtelierRepository atelierRepo)
        {
            _atRepo = atelierRepo;

        }
		#region Atelier
		// GET: api/Ateliers
		/// <summary>
		/// Get all ateliers
		/// </summary>
		/// <returns>all ateliers</returns>
		///
		[HttpGet]
        public ActionResult<IEnumerable<AtelierDTO>> GetAlleAteliers(){
			try
			{
				IEnumerable<Atelier> a = _atRepo.GetAll().OrderBy(prop => prop.ID);
				return a.Select(ad => new AtelierDTO(ad)).ToList();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
        }

        // GET: api/Atelier/2
        /// <summary>
        /// GET an atelier
        /// </summary>
        /// <returns>an atelier</returns>
        ///
        [Route("{id}")]
        [HttpGet]
        public ActionResult<AtelierDTO> GetAtelier(int id){
			try
			{
				Atelier a = _atRepo.GetById(id);
				return a == null ? null : new AtelierDTO(a);
			} catch(Exception e)
			{
				return BadRequest(e.Message);
			}
        }

        // POST: api/Atelier
        /// <summary>
        /// Create an atelier
        /// </summary>
        /// <returns>result of creation</returns>
        [Authorize(Policy = "Begeleider")]
        [Route("{naam}")]
		[HttpPost]
        public ActionResult CreateAtelier(string naam) {
			try
			{
				if (String.IsNullOrEmpty(naam))
					return BadRequest("Please provide a name");

				Atelier at = new Atelier(naam);
				_atRepo.Add(at);
				_atRepo.saveChanges();

				return Created($"api/Atlier/{at.ID}", new AtelierDTO(at));
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
        }

		// PUT: api/Atelier/2
		/// <summary>
		/// update an atelier
		/// </summary>
		/// <returns>result of update</returns>
		///
		[Route("{id}/{naam}")]
		[HttpPut]
		public IActionResult UpdateAtelier(int id, string naam)
		{ //Geef AterlierDTO en controleer de HTTPPut
			Atelier a = _atRepo.GetById(id);
			if (a == null)
			{
				return NotFound();
			}
			if (!String.IsNullOrEmpty(naam)) { a.Naam = naam; }

			_atRepo.Update(a);
			_atRepo.saveChanges();

			return Created(nameof(GetAtelier), a.ID);

		}

		//TODO: DB probleem met deleten atelier!

		// DELETE: api/Atelier/2
		/// <summary>
		/// Delete an atelier
		/// </summary>
		/// <returns>result of deletion</returns>
		///
		[Route("{id}")]
		[HttpDelete]
		public ActionResult<Atelier> DeleteAtelier(int id)
		{ //Geef Atelier ID mee als param
			try
			{
				Atelier a = _atRepo.GetById(id);
				if (a == null)
				{
					return NotFound();
				}

				_atRepo.Remove(a);
				_atRepo.saveChanges();
				return a;
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		#endregion

		#region Pictos
		// GET: api/Atelier/2/Picto
		/// <summary>
		/// GET a Pictogram of an atelier
		/// </summary>
		/// <returns>a Picto</returns>
		///
		[Route("{id}/Picto")]
		[HttpGet]
		public IActionResult GetAtelierPictogram(int id)
		{
			try
			{
				Foto image = _atRepo.GetAtelierPicto(id);

				if (image == null)
					return BadRequest("We couldn't retrieve this pictogram");

				return File(image.FotoData, $"image/{image.Extension.ToLower()}", $"{image.FileName}.{image.Extension}");
			} catch (Exception e) {
				return BadRequest(e.Message); 
			}
		}

		// POST: api/Atelier/2/Picto
		/// <summary>
		/// Upload picto for Atelier
		/// </summary>
		/// <returns>result of creation</returns>
		[Route("{id}/picto")]
        [HttpPost]
        public IActionResult UploadAtelierPictogram(int id, [FromForm(Name = "file")] IFormFile file) { //Geef File als parameter op
			try
			{
				if (file.Length > 20000000)
				{
					return BadRequest("Your file is too big");
				}

				var fileNameSplit = file.FileName.Split(".");
				string extension = fileNameSplit[fileNameSplit.Length - 1];
				if (!ImageExt.Contains(extension.ToLower()))
				{
					return BadRequest("Your file is not an image.");
				}

				Atelier a = _atRepo.GetById(id);

				string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
				using (var ms = new MemoryStream())
				{
					file.CopyTo(ms);
					var fileBytes = ms.ToArray();
					a.Pictogram = new Foto { FotoData = fileBytes, FileName = name, Extension = extension };
				}

				_atRepo.saveChanges();
				return NoContent();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
        }

		// Put: api/Atelier/2/Picto
		/// <summary>
		/// Update picto for Atelier
		/// </summary>
		/// <returns>Nothing</returns>
		[Route("{id}/picto")]
		[HttpPut]
		public IActionResult UpdateAtelierPictogram(int id, [FromForm(Name = "file")] IFormFile file)
		{
			try
			{
				if (file.Length > 20000000)
				{
					return BadRequest("Your file is too big");
				}

				var fileNameSplit = file.FileName.Split(".");
				string extension = fileNameSplit[fileNameSplit.Length - 1];
				if (!ImageExt.Contains("Your file is not an image."));

				Atelier a = _atRepo.GetById(id);

				if (a == null)
				{
					return NotFound();
				}

				string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
				using (var ms = new MemoryStream())
				{
					file.CopyTo(ms);
					var fileBytes = ms.ToArray();
					a.Pictogram = new Foto { FotoData = fileBytes, FileName = name, Extension = extension };
				}

				_atRepo.Update(a);
				_atRepo.saveChanges();
				return NoContent();
			} catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		#endregion


	}
}
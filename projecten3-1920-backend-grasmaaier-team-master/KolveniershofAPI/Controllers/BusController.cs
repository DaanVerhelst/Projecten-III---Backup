using System;
using System.Collections.Generic;
using System.IO;
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
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly IBusRepository _busRepo;
        public static string[] ImageExt = { "jpg", "jpeg", "png", "tiff", "bmp", "gif" };

        public BusController(IBusRepository busRepo)
        {
            this._busRepo = busRepo;
        }

        #region Bus
        // GET: api/Bus
        /// <summary>
        /// Get all Bussen
        /// </summary>
        /// <returns>all Bussen</returns>
        ///
        [HttpGet]
        public ActionResult<IEnumerable<BusDTO>> GetAlleBussen()
        {
            try
            {
                IEnumerable<Bus> a = _busRepo.GetAll().OrderBy(prop => prop.ID);
                return a.Select(ad => new BusDTO(ad)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Bus/2
        /// <summary>
        /// GET an atelier
        /// </summary>
        /// <returns>an atelier</returns>
        ///
        [Route("{id}")]
        [HttpGet]
        public ActionResult<BusDTO> GetAtelier(int id)
        {
            try
            {
                Bus a = _busRepo.GetById(id);
                return a == null ? null : new BusDTO(a);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Bus
        /// <summary>
        /// Create an Bus
        /// </summary>
        /// <returns>result of creation</returns>
        [Authorize(Policy = "Begeleider")]
        [Route("{naam}")]
        [HttpPost]
        public ActionResult CreateAtelier(string naam)
        {
            try
            {
                if (String.IsNullOrEmpty(naam))
                    return BadRequest("Please provide a name");

                Bus at = new Bus(naam);
                _busRepo.Add(at);
                _busRepo.saveChanges();

                return Created($"api/Bus/{at.ID}", new BusDTO(at));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Bus/2
        /// <summary>
        /// update an Bus
        /// </summary>
        /// <returns>result of update</returns>
        ///
        [Route("{id}/{naam}")]
        [HttpPut]
        public IActionResult UpdateBus(int id, string naam)
        { //Geef AterlierDTO en controleer de HTTPPut
            Bus a = _busRepo.GetById(id);
            if (a == null)
            {
                return NotFound();
            }
            if (!String.IsNullOrEmpty(naam)) { a.Naam = naam; }

            _busRepo.Update(a);
            _busRepo.saveChanges();

            return Created(nameof(GetAtelier), a.ID);

        }

        //TODO: DB probleem met deleten atelier!

        // DELETE: api/Bus/2
        /// <summary>
        /// Delete an Bus
        /// </summary>
        /// <returns>result of deletion</returns>
        ///
        [Route("{id}")]
        [HttpDelete]
        public ActionResult<Bus> DeleteBus(int id)
        { //Geef Atelier ID mee als param
            try
            {
                Bus a = _busRepo.GetById(id);
                if (a == null)
                {
                    return NotFound();
                }

                _busRepo.Remove(a);
                _busRepo.saveChanges();
                return a;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region Pictos
        // GET: api/Bus/2/Picto
        /// <summary>
        /// GET a Pictogram of an Bus
        /// </summary>
        /// <returns>a Picto</returns>
        ///
        [Route("{id}/Picto")]
        [HttpGet]
        public IActionResult GetBusPictogram(int id)
        {
            try
            {
                Foto image = _busRepo.GetBusPicto(id);

                if (image == null)
                    return BadRequest("We couldn't retrieve this pictogram");

                return File(image.FotoData, $"image/{image.Extension.ToLower()}", $"{image.FileName}.{image.Extension}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Bus/2/Picto
        /// <summary>
        /// Upload picto for Bus
        /// </summary>
        /// <returns>result of creation</returns>
        [Route("{id}/picto")]
        [HttpPost]
        public IActionResult UploadBusPictogram(int id, [FromForm(Name = "file")] IFormFile file)
        { //Geef File als parameter op
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

                Bus a = _busRepo.GetById(id);

                string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    a.Pictogram = new Foto { FotoData = fileBytes, FileName = name, Extension = extension };
                }

                _busRepo.saveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Put: api/Bus/2/Picto
        /// <summary>
        /// Update picto for Bus
        /// </summary>
        /// <returns>Nothing</returns>
        [Route("{id}/picto")]
        [HttpPut]
        public IActionResult UpdateBusPictogram(int id, [FromForm(Name = "file")] IFormFile file)
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

                Bus a = _busRepo.GetById(id);

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

                _busRepo.Update(a);
                _busRepo.saveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion


    }
}
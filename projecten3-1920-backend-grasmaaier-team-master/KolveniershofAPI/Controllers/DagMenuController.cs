using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DagMenuController : ControllerBase
    {
        private readonly IDagMenuRepository _dagMenuRepo;
        private readonly ITemplateRepository _templateRepo;

        public DagMenuController(IDagMenuRepository dagMenuRepo, ITemplateRepository templateRepo)
        {
            this._dagMenuRepo = dagMenuRepo;
            this._templateRepo = templateRepo;
        }

        [HttpGet]
        public IEnumerable<DagMenu> GetAlleMenus()
        {
            return _dagMenuRepo.GetAll().OrderBy(menu => menu.ID);
        }

        [Route("{id}")]
        [HttpGet]
        public DagMenu GetMenu(int id)
        {
            DagMenu dagMenu = _dagMenuRepo.getBy(id);
            return dagMenu == null ? null : dagMenu;
        }

        [Authorize(Policy = "Begeleider")]
        [Route("{weekNummer}/{dagNummer}")]
        [HttpPost]
        public IActionResult CreateMenu(int weekNummer, int dagNummer, [FromBody] MenuDTO menuDTO)
        {
            try
            {
                DagTemplate dag = _templateRepo.GeefTemplate(dagNummer, weekNummer);
                if (dag == null)
                {
                    throw new ArgumentException("DagTemplate could not be found");
                }
              //  if (dag.Menu != null)
                {
                    throw new ArgumentException("Menu already exists");
                }

                DagMenu menu = new DagMenu(menuDTO);
              //  dag.Menu = menu;

                _dagMenuRepo.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateMenu(long id, [FromBody] MenuDTO menuDTO)
        {
            if (id != menuDTO.ID)
            {
                return BadRequest("ID's did not match");
            }

            var menu = _dagMenuRepo.getBy(id);
            menu.Soep = menuDTO.Soep;
            menu.Groente = menuDTO.Groente;
            menu.Vlees = menuDTO.Vlees;
            menu.Supplement = menuDTO.Supplement;
            _dagMenuRepo.SaveChanges();

            return Ok();

        }
    }
}
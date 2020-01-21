using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KolveniershofAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase{
        private readonly IUserRepository _userRepo;
        private readonly IBegeleiderRepository _begRepo;
        private readonly IClientRepository _clientRepo;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public GebruikerController(IUserRepository userRepo, SignInManager<IdentityUser> sim,
            UserManager<IdentityUser> um,IConfiguration configuration,IClientRepository clientRepo,
            IBegeleiderRepository begRepo){
            _userRepo = userRepo;
            _signInManager = sim;
            _userManager = um;
            _begRepo = begRepo;
            _config = configuration;
            _clientRepo = clientRepo;
        }

        private String GetToken(IdentityUser user){ 
            // Create the token
            var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName) 
            };

           Array.ForEach(_userManager.GetClaimsAsync(user).Result.ToArray(),(Claim cl)=>claims.Add(cl));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                null, null, claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<String>> CreateToken(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    string token = GetToken(user);
                    return Created("", token); //returns only the token
                }
            }

            return BadRequest();
        }

        [Authorize(Policy = "Admin")]
        [Route("begeleider/registreer/{isAdmin}")]
        [HttpPost]
        public async Task<ActionResult<String>> RegisterBegeleider(RegisterDTO register,bool isAdmin){
            if (!_userRepo.EmailUnique(register.Email))
                return BadRequest("This email isn't unique");

            Begeleider beg = new Begeleider(){
                Familienaam = register.Familienaam,
                Voornaam = register.Voornaam
            };
            
            IdentityUser iu = MakeIdentityUser(beg, register);
            var result = await _userManager.CreateAsync(iu, register.Password);

            if (result.Succeeded) {
                result = await _userManager.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Begeleider"));

                if(isAdmin)
                    result = await _userManager.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Admin"));

                if (result.Succeeded) {
                    _begRepo.AddBegeleider(beg);
                    string token = GetToken(iu);
                    return Created("", token);
                }
            }
            return BadRequest("Something went wrong");
        }
        
        [Authorize(Policy="Admin")]
        [Route("client/registreer")]
        [HttpPost]
        public async Task<ActionResult<String>> RegisterClient(RegisterDTO register) {
            if (!_userRepo.EmailUnique(register.Email))
                return BadRequest("This email isn't unique");


            Client cli = new Client(){
                Familienaam = register.Familienaam,
                Voornaam = register.Voornaam
            };

            IdentityUser iu = MakeIdentityUser(cli,register);
            var result = await _userManager.CreateAsync(iu, register.Password);

            if (result.Succeeded) {
                result = await _userManager.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Client"));
                if (result.Succeeded) {
                    _clientRepo.AddClient(cli);
                    string token = GetToken(iu);
                    return Created("", token);
                }
            }

            return BadRequest("Something went wrong");
        }

        [Authorize(Policy = "Admin")]
        [Route("checkToken")]
        [HttpGet]
        public IActionResult CheckToken() {
            return Ok();
        }

        private IdentityUser MakeIdentityUser(Persoon pers, RegisterDTO dto) {
            IdentityUser iu = new IdentityUser(){
                Email = dto.Email,
                EmailConfirmed = true,
                UserName = pers.Username
            };

            return iu;
        }
    }
}
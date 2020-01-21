using KolveniershofAPI.Model.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories{
    public class UserRepository : IUserRepository{
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> um){
            _userManager = um;
        }

        public bool EmailUnique(string email){
            return (_userManager.FindByEmailAsync(email).Result)==null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface{
    public interface IUserRepository{
        bool EmailUnique(string email);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface{
    public interface IClientRepository{
        IEnumerable<Client> GetAll();
        void AddClient(Client client);
        Client GetById(long id);
        void RemoveClient(Client client);
		void updateClient(Client client);
		void saveChanges();
    }
}

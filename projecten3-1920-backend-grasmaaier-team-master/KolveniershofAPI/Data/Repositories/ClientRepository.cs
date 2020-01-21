using System.Collections.Generic;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KolveniershofAPI.Data.Repositories{
    public class ClientRepository : IClientRepository{
        private readonly DbSet<Client> _clients;
        private readonly ApplicationDBContext _context;
        
        public ClientRepository(ApplicationDBContext context){
            _clients = context.Clienten;
            _context = context;
        }

        public void AddClient(Client client){
            _clients.Add(client);

        }

        public IEnumerable<Client> GetAll() {
            return _clients.ToList();
        }

        public Client GetById(long id){
            return _clients
                .Include(e => e.Dag_Personen)
                .Include(f => f.ProfielFoto)
                .FirstOrDefault(c=>c.ID==id);
        }

        public void RemoveClient(Client client){
            _clients.Remove(client);

        }

		public void saveChanges()
		{
			_context.SaveChanges();
		}

		public void updateClient(Client client)
		{
			_clients.Update(client);
		}
	}
}

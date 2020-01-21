using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Model_EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface
{
	public interface IDagRepository{
		IEnumerable<Dag> Getweek(DateTime start);
        IEnumerable<Dag> GetweekForClient(DateTime start, long id);
        IEnumerable<Atelier_Dag> GetDay(DateTime dag);
        IEnumerable<Dag> GetAll();
        bool HeeftTemplate(DateTime dt);

        void AddDag(Dag dag);
        void RemoveDag(Dag dag);

        IEnumerable<Client> GetClientsInActivityByDay(DateTime dag, int activiteitID);
		IEnumerable<Begeleider> GetBegeleidersInActivity(DateTime dag, int activiteitID);

		Dag GetDagByDay(DateTime dag);
		void SaveChanges();
		Notitieblok getNotitiesByDay(DateTime dag);
	}
}

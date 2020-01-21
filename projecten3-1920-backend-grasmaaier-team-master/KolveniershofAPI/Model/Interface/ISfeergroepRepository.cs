using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface{
    public interface ISfeergroepRepository{
        IEnumerable<SfeerGroep> GetAll();
        SfeerGroep GetById(int id);

		void Delete(SfeerGroep sg);
		void Update(SfeerGroep sg);

		void Create(SfeerGroep sg);

		void SaveChanges();
    }
}

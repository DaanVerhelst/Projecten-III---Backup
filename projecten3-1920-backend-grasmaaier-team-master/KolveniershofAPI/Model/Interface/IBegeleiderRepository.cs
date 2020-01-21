using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface
{
    public interface IBegeleiderRepository{
        Begeleider GetBegeleiderByID(long id);
        ICollection<Begeleider> GetAllBegeleiders();
        void AddBegeleider(Begeleider begeleider);
        void RemoveBegeleider(Begeleider begeleider);
        void UpdateBegeleider(Begeleider begeleider);
        void saveChanges();
    }
}

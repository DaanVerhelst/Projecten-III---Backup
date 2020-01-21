using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface{
    public interface ITemplateRepository{
        void Verwijder(DagTemplate dt);
        int[] GeefWeken();
        DagTemplate GeefTemplate(int dag, int week);
        DagTemplate[] GeefWeekTemplate(int week);
        void AddTemplate(DagTemplate temp);
        DagTemplate GetByMenu(int id);
    }
}

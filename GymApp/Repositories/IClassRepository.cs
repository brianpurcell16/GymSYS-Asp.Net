using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApp.Models;

namespace GymApp.Repositories
{
    public interface IClassRepository
    {
        void Add(GymClass gymClass);
        GymClass GetById(int classId);
        List<GymClass> GetActiveClasses();
        void Cancel(int classId);
        bool DecreaseSpaces(int classId);
        void IncreaseSpaces(int classId);
    }
}

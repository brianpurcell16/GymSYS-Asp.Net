using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;

namespace GymApp.Services
{
    public interface IClassService
    {
        void ScheduleClass(GymClass gymClass);

        GymClass GetClassById(int id);
        List<GymClass> GetActiveClasses();
        void CancelClass(int classId);

    }
}
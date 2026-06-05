using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public void ScheduleClass(GymClass gymClass)
        {
            gymClass.Status = "A"; 
            gymClass.AvailableSpaces = gymClass.Capacity;
            _classRepository.Add(gymClass);
        }

        public GymClass GetClassById(int classId)
        {
            return _classRepository.GetById(classId);
        }

        public List<GymClass> GetActiveClasses()
        {
            return _classRepository.GetActiveClasses();
        }

        public void CancelClass(int classId)
        {
            _classRepository.Cancel(classId);
        }
    }
}
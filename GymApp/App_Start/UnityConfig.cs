using GymApp.Repositories;
using GymApp.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace GymApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IMemberRepository, MemberRepository>();
            container.RegisterType<IClassRepository, ClassRepository>();
            container.RegisterType<IBookingRepository, BookingRepository>();

            container.RegisterType<IMemberService, MemberService>();
            container.RegisterType<IClassService, ClassService>();
            container.RegisterType<IBookingService, BookingService>();
        }
    }

}
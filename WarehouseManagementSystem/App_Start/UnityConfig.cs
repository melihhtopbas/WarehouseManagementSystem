using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using WarehouseManagementSystem.Areas.Admin.Controllers;

namespace WarehouseManagementSystem
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

          //  container.RegisterType<LoginController>(new InjectionConstructor());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
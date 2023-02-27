
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using Warehouse.Data;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Web;
using Warehouse.Service.Admin;
using Warehouse.Service.WebSite;
using WarehouseManagementSystem.Areas.Admin.Controllers;
using WarehouseManagementSystem.Controllers;

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
            container.RegisterType<ICacheService, InMemoryCache>();

            container.RegisterType<BaseController>(new InjectionConstructor());
            container.RegisterType<AdminBaseController>(new InjectionConstructor()); 


            //container.RegisterType<ContactService>(new InjectionConstructor());
            //container.RegisterType<FaqService>(new InjectionConstructor());
            //container.RegisterType<PropertyService>(new InjectionConstructor());
            //container.RegisterType<Warehouse.Service.WebSite.SettingService>(new InjectionConstructor());
            //container.RegisterType<SliderService>(new InjectionConstructor());


            //container.RegisterType<CountryService>(new InjectionConstructor());
            //container.RegisterType<Warehouse.Service.Admin.SettingService>(new InjectionConstructor());
            //container.RegisterType<LanguageService>(new InjectionConstructor());
            //container.RegisterType<OrderService>(new InjectionConstructor());






            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
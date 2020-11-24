using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Autofac.Core;
using WPFContacts.Services;

namespace WPFContacts
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs args)
        {
            var builder = new ContainerBuilder();
            ////var assemblies = new[] { Assembly.GetExecutingAssembly() };

            ////builder.RegisterAssemblyTypes(assemblies)
            ////    .Where(t => typeof(IWPFContactService).IsAssignableFrom(t))
            ////    .SingleInstance()
            ////    .AsImplementedInterfaces();

            builder.RegisterType<WPFContactService>()
                .As<IWPFContactService>();
            
        }
    }
}

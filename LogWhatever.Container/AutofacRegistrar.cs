using Autofac;
using LogWhatever.Common.Adapters.Bundling;
using LogWhatever.Handlers.Commands;
using LogWhatever.Repositories;
using LogWhatever.Service.Email;

namespace LogWhatever.Container
{
	public class AutofacRegistrar
	{
		#region Public Methods
		public void Register(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof (BaseRepository).Assembly).AsImplementedInterfaces().PropertiesAutowired();
			builder.RegisterAssemblyTypes(typeof (SendGridEmailer).Assembly).AsImplementedInterfaces().PropertiesAutowired();
			builder.RegisterAssemblyTypes(typeof (IBundleFactory).Assembly).AsImplementedInterfaces().PropertiesAutowired();
			builder.RegisterAssemblyTypes(typeof (BaseCommandHandler).Assembly).AsImplementedInterfaces().PropertiesAutowired();
		}
		#endregion
	}
}
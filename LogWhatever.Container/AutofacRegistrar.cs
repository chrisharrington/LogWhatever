using Autofac;
using LogWhatever.Common.Adapters.Bundling;

namespace LogWhatever.Container
{
	public class AutofacRegistrar
	{
		#region Public Methods
		public void Register(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof (IBundleFactory).Assembly).AsImplementedInterfaces().PropertiesAutowired();
		}
		#endregion
	}
}
namespace Pintle.Diagnostics.Sitecore.Configuration
{
	using System.Linq;
	using Microsoft.Extensions.DependencyInjection;
	using global::Sitecore.Abstractions;
	using global::Sitecore.DependencyInjection;

	public class ServicesConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			var descriptor = serviceCollection.FirstOrDefault(d => d.ServiceType == typeof(BaseLog));

			var newLogger = new LoggingService(new SitecoreLogProvider(ConfigurationReader.GetSitecoreLogProviderLevel()));

			var newDescriptor = new ServiceDescriptor(
				typeof(BaseLog),
				newLogger);

			var index = serviceCollection.IndexOf(descriptor);
			serviceCollection.Insert(index, newDescriptor);
			serviceCollection.Remove(descriptor);
			serviceCollection.AddSingleton<LoggingService>(p => newLogger);
		}
	}
}

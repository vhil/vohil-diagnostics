namespace Pintle.Diagnostics.Sitecore.Pipelines
{
	using global::Microsoft.Extensions.DependencyInjection;
	using global::Sitecore.Abstractions;
	using global::Sitecore.DependencyInjection;
	using global::Sitecore.Pipelines;

	public class InitializeLoggingService
	{
		public void Process(PipelineArgs args)
		{
			var logger = ServiceLocator.ServiceProvider.GetService<BaseLog>();

			if (logger is LoggingService loggingService)
			{
				foreach (var logProvider in ConfigurationReader.GetDefaultLogProviders())
				{
					loggingService.AddLogProvider(logProvider);
				}
			}
		}
	}
}

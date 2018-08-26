# Pintle.Diagnostics
Pintle.Diagnostics is a Sitecore CMS module which provides extendable configuration-driven logging and diagnostics features.

Functionality and features
 * supports Sitecore 8.x and 9.x 
 * allows to easily configure separate log files for required features
 * uses default Sitecore configuration factory for logger instances
 * implements defautl Sitecore.Abstractions.BaseLog abstraction
 * enables to write to multiple log files with differen severity level
 * provides implementation for default Sitecore logs and NLog logger 

The module is a set of NuGet packages that can be used in your solution:
 * [Pintle.Diagnostics.Core](https://www.nuget.org/packages/Pintle.Diagnostics.Core "Pintle.Diagnostics.Core")
 * [Pintle.Diagnostics](https://www.nuget.org/packages/Pintle.Diagnostics "Pintle.Diagnostics")
 * [Pintle.Diagnostics.Nlog.Core](https://www.nuget.org/packages/Pintle.Diagnostics.Nlog.Core "Pintle.Diagnostics.Nlog.Core")
 * [Pintle.Diagnostics.Nlog](https://www.nuget.org/packages/Pintle.Diagnostics.Nlog "Pintle.Diagnostics.Nlog")
 
 Within helix modular architecture:
- Install [Pintle.Diagnostics.Nlog](https://www.nuget.org/packages/Pintle.Diagnostics.Nlog "Pintle.Diagnostics.Nlog") nuget package to your project layer module (will include config files for default logger both Sitecore and nlog providers)
- Install [Pintle.Diagnostics.Nlog.Core](https://www.nuget.org/packages/Pintle.Diagnostics.Nlog.Core "Pintle.Diagnostics.Nlog.Core") nuget package to your feature or foundation layer module

# Getting started

## Default logging service

The module by default installs a configuration for a logging service instance which writes to 4 files simultaniously:
 * default sitecore log file
 * $(dataFolder)/logs/log.error.${date:format=yyyyMMdd}.txt (writes only error log messages)
 * $(dataFolder)/logs/log.warn.${date:format=yyyyMMdd}.txt (writes error and warning log messages)
 * $(dataFolder)/logs/log.debug.${date:format=yyyyMMdd}.txt (writes all messages including debug log entries)
 
This can be easily re-configured to match your project needs by modifying configuration files. The default configuration looks like next:
```xml
﻿<?xml version="1.0" encoding="utf-8" ?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
	<sitecore>
		<pintle>
			<logging>
				<defaultLogger type="Pintle.Diagnostics.LoggingService, Pintle.Diagnostics" singleInstance="true" >
					<param name="sitecoreProvider" ref="pintle/logging/providers/defaultSitecoreLogger"/>
					<param name="errorLogger" ref="pintle/logging/providers/errorLogger"/>
					<param name="warnLogger" ref="pintle/logging/providers/warnLogger"/>
					<param name="debugLogger" ref="pintle/logging/providers/debugLogger"/>
				</defaultLogger>
				<providers>
				    <defaultSitecoreLogger type="Pintle.Diagnostics.Sitecore.SitecoreLogProvider, Pintle.Diagnostics.Sitecore" singleInstance="true" >
						<param name="logLevel">Debug</param>
					</defaultSitecoreLogger>
					<errorLogger type="Pintle.Diagnostics.NLog.NLogLogProvider, Pintle.Diagnostics.NLog" 
											 singleInstance="true" 
											 logFilePath="$(dataFolder)/logs/log.error.${date:format=yyyyMMdd}.txt">
						<param name="filePath">$(logFilePath)</param>
						<param name="logLevel">Error</param>
					</errorLogger>
					<warnLogger type="Pintle.Diagnostics.NLog.NLogLogProvider, Pintle.Diagnostics.NLog"
					            singleInstance="true"
					            logFilePath="$(dataFolder)/logs/log.warn.${date:format=yyyyMMdd}.txt">
						<param name="filePath">$(logFilePath)</param>
						<param name="logLevel">Warn</param>
					</warnLogger>
					<debugLogger type="Pintle.Diagnostics.NLog.NLogLogProvider, Pintle.Diagnostics.NLog"
					            singleInstance="true"
					            logFilePath="$(dataFolder)/logs/log.debug.${date:format=yyyyMMdd}.txt">
						<param name="filePath">$(logFilePath)</param>
						<param name="logLevel">Debug</param>
					</debugLogger>
				</providers>
			</logging>
		</pintle>
	</sitecore>
</configuration>
``` 

## Use a dedicated log file for your feature

You can create your own logging service by configuring it in sitecore configs:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
	<sitecore>
		<myFeature>
			<logging>
				<myFeatureLogger type="Pintle.Diagnostics.LoggingService, Pintle.Diagnostics" singleInstance="true" >
					<param name="warnLogger" ref="myFeature/logging/providers/myFeatureLogFile"/>
				</myFeatureLogger>
				<providers>
					<myFeatureLogFile type="Pintle.Diagnostics.NLog.NLogLogProvider, Pintle.Diagnostics.NLog"
					             singleInstance="true"
					             logFilePath="$(dataFolder)/logs/log.myFeature.${date:format=yyyyMMdd}.txt">
						<param name="filePath">$(logFilePath)</param>
						<param name="logLevel">Info</param>
					</myFeatureLogFile>
				</providers>
			</logging>
		</myFeature>
	</sitecore>
</configuration>
```

Then in your feature code you still inject the Sitecore.Abstraction.BaseLog dependency:

```cs
using Sitecore.Abstractions;

public class MyFeatureService
{
	protected readonly BaseLog logger;

	public MyFeatureService(BaseLog logger)
	{
		this.logger = logger;
	}
}
```

And in your dependency injection services configurator you can register your feature to use your new dedicated logger:
```cs
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Abstractions;
using Sitecore.Configuration;

public class ServicesConfigurator : IServicesConfigurator
{
	public void Configure(IServiceCollection serviceCollection)
	{
		serviceCollection.AddTransient<MyFeatureService>(provider => 
			new MyFeatureService(Factory.CreateObject("myFeature/logging/myFeatureLogger", true) as BaseLog));
	}
}
```

You are welcome to add more log files with different severity levels to the logging service configuration.


## Contributing

We love it if you would contribute!

Help us! Keep the quality of feature requests and bug reports high

We strive to make it possible for everyone and anybody to contribute to this project. Please help us by making issues easier to resolve by providing sufficient information. Understanding the reasons behind issues can take a lot of time if information is left out.

Thank you, and happy contributing!

namespace Pintle.Diagnostics
{
	using System;
	using System.Collections.Generic;
	using Sitecore.Caching;
	using Sitecore.Configuration;
	using Sitecore.Abstractions;

	public class LoggingService : BaseLog
    {
	    public static BaseLog ConfiguredInstance => Factory.CreateObject("pintle/logging/defaultLogger", true) as LoggingService;

        private readonly List<ILogProvider> logProviders;
        private ICache singles;

        public LoggingService(ILogProvider provider1)
			:this(new [] {provider1})
        {
        }

		public LoggingService(
			ILogProvider provider1,
			ILogProvider provider2)
			:this(new [] {provider1, provider2})
        {
        }

		public LoggingService(
			ILogProvider provider1,
			ILogProvider provider2,
			ILogProvider provider3)
			:this(new [] {provider1, provider2, provider3})
        {
        }

	    public LoggingService(
			ILogProvider provider1, 
			ILogProvider provider2, 
			ILogProvider provider3, 
			ILogProvider provider4)
		    : this(new[] { provider1, provider2, provider3, provider4})
	    {
	    }

	    public LoggingService(
			ILogProvider provider1,
			ILogProvider provider2, 
			ILogProvider provider3, 
			ILogProvider provider4, 
			ILogProvider provider5)
		    : this(new[] { provider1, provider2, provider3, provider4, provider5 })
	    {
	    }

		public LoggingService(params ILogProvider[] providers)
	    {
			this.logProviders = new List<ILogProvider>();
			this.logProviders.AddRange(providers);
	    }

	    public override bool Enabled => true;

        public override bool IsDebugEnabled => true;

	    public override ICache Singles
        {
            get
            {
                if (this.singles == null && this.Enabled)
                {
                    this.singles = CacheManager.GetNamedInstance("Pintle log singles", Settings.Caching.SmallCacheSize, true);
                }
                    
                return this.singles;
            }
        }

        public override void Audit(string message, Type ownerType)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Audit, message, (object)ownerType);
            }
        }

        public override void Audit(string message, string loggerName)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Audit, message, loggerName);
            }
        }

        public override void Audit(string message, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Audit, message, owner);
            }
        }

        public override void Audit(object owner, string format, params string[] parameters)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Audit, string.Format(format, parameters), owner);
            }
        }

        public override void Audit(Type ownerType, string format, params string[] parameters)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Audit, string.Format(format, parameters), (object)ownerType);
            }
        }

        public override void Debug(string message)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Debug, message, typeof(LoggingService));
            }
        }

        public override void Debug(string message, string loggerName)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Debug, message, loggerName);
            }
        }

        public override void Debug(string message, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Debug, message, owner);
            }
        }

        public override void Error(string message, Type ownerType)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Error, message, (object)ownerType);
            }
        }

        public override void Error(string message, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Error, message, owner);
            }
        }

        public override void Error(string message, Exception exception, string loggerName)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Error, message, loggerName, exception);
            }
        }

        public override void Error(string message, Exception exception, Type ownerType)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Error, message, (object)ownerType, exception);
            }
        }

        public override void Error(string message, Exception exception, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Error, message, owner, exception);
            }
        }

        public override void Fatal(string message, string loggerName)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Fatal, message, loggerName);
            }
        }

        public override void Fatal(string message, Type ownerType)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Fatal, message, (object)ownerType);
            }
        }

        public override void Fatal(string message, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Fatal, message, owner);
            }
        }

        public override void Fatal(string message, Exception exception, Type ownerType)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Fatal, message, (object)ownerType, exception);
            }
        }

        public override void Fatal(string message, Exception exception, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Fatal, message, owner, exception);
            }
        }

        public override void Info(string message, string loggerName)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Info, message, loggerName);
            }
        }

        public override void Info(string message, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Info, message, owner);
            }
        }

        public override void SingleError(string message, object owner)
        {
            if (!this.Enabled || this.Singles == null || this.Singles.ContainsKey(message))
            {
                return;
            }
                
            this.Error(message, owner);
            this.Singles.Add(message, (object)string.Empty);
        }

        public override void SingleFatal(string message, Exception exception, Type ownerType)
        {
            if (!this.Enabled || this.Singles == null || this.Singles.ContainsKey(message))
            {
                return;
            }
               
            this.Fatal($"SINGLE MSG: {message as object}", exception, ownerType);
            this.Singles.Add(message, string.Empty);
        }

        public override void SingleFatal(string message, Exception exception, object owner)
        {
            this.SingleFatal(message, exception, owner.GetType());
        }

        public override void SingleWarn(string message, object owner)
        {
            if (!this.Enabled || this.Singles == null || this.Singles.ContainsKey(message))
            {
                return;
            }
                
            this.Warn(message, owner);
            this.Singles.Add(message, string.Empty);
        }

        public override void Warn(string message, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Warn, message, owner);
            }
        }

        public override void Warn(string message, Exception exception, string loggerName)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Warn, message, loggerName, exception);
            }
        }

        public override void Warn(string message, Exception exception, object owner)
        {
            foreach (var provider in this.logProviders)
            {
                provider.Log(SeverityLevel.Warn, message, owner, exception);
            }
        }
    }
}
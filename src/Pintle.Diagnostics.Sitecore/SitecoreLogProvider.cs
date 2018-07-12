namespace Pintle.Diagnostics.Sitecore
{
	using System;
	using global::Sitecore.Diagnostics;

	public class SitecoreLogProvider : LogProvider
	{
		private readonly DefaultLog logger;

		public SitecoreLogProvider(string logLevel) : base(logLevel)
		{
			this.logger = new DefaultLog();
		}

		public override void Audit(string message, object owner)
		{
			this.logger.Audit(message, owner);
		}

		public override void Debug(string message, object owner)
		{
			this.logger.Debug(message, owner);
		}

		public override void Info(string message, object owner)
		{
			this.logger.Info(message, owner);
		}

		public override void Warn(string message, object owner, Exception exception = null)
		{
			if (exception == null)
			{
				this.logger.Warn(message, owner);
			}
			else
			{
				this.logger.Warn(message, exception, owner);
			}
		}

		public override void Error(string message, object owner, Exception exception = null)
		{
			if (exception == null)
			{
				this.logger.Error(message, owner);
			}
			else
			{
				this.logger.Error(message, exception, owner);
			}
		}

		public override void Fatal(string message, object owner, Exception exception = null)
		{
			if (exception == null)
			{
				this.logger.Fatal(message, owner);
			}
			else
			{
				this.logger.Fatal(message, exception, owner);
			}
		}
	}
}
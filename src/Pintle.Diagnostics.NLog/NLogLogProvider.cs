using System.IO;
using System.Web;

namespace Pintle.Diagnostics.NLog
{
	using System;
	using global::NLog;
	using NLogConfig = global::NLog.Config;
	using NLogTargets = global::NLog.Targets;

	public class NLogLogProvider : LogProvider
	{
		private static readonly object SyncRoot = new object();
		private readonly Logger logger;

		public NLogLogProvider(string filePath, string logLevel)
			:base(logLevel)
        {
			lock (SyncRoot)
			{
				filePath = filePath.Replace("/", "\\");

				if (filePath.StartsWith("\\"))
				{
					filePath = Path.Combine(HttpRuntime.AppDomainAppPath, filePath);
				}

				var loggerName = GenerateLoggerName(filePath);
				var targetName = "Target_" + loggerName;

				if (LogManager.Configuration == null)
				{
					LogManager.Configuration = new NLogConfig.LoggingConfiguration();
				}

				var config = LogManager.Configuration;
				var fileTarget = new NLogTargets.FileTarget
				{
					KeepFileOpen = false,
					Layout = @"${longdate} ${threadid:uppercase=true:padding=4} ${level:uppercase=true:padding=6} ${message}",
					FileName = filePath,
					Name = targetName
				};

				config.AddTarget(targetName, fileTarget);
				config.LoggingRules.Add(new NLogConfig.LoggingRule(loggerName, global::NLog.LogLevel.Debug, fileTarget));

				LogManager.Configuration = config;
				this.logger = LogManager.GetLogger(loggerName);
			}
		}

		private static string GenerateLoggerName(string filePath)
	    {
		    var loggerName = "NLogger_" + filePath
			    .Replace("\\", "_")
			    .Replace("{", "")
			    .Replace("}", "")
			    .Replace("=", "")
			    .Replace("$", "")
			    .Replace("*", "")
			    .Replace(".", "_")
			    .Replace(":", "");

		    return loggerName;
	    }

		protected override void Audit(string message, object owner)
		{
			this.Info(message, owner);
		}

		protected override void Debug(string message, object owner)
		{
			this.logger.Debug(message);
		}

		protected override void Info(string message, object owner)
		{
			this.logger.Info(message);
		}

		protected override void Warn(string message, object owner, Exception exception = null)
		{
			if (exception == null)
			{
				this.logger.Warn(message);
			}
			else
			{
				this.logger.Warn(exception, message);
			}
		}

		protected override void Error(string message, object owner, Exception exception = null)
		{
			if (exception == null)
			{
				this.logger.Error(message);
			}
			else
			{
				this.logger.Error(exception, message);
			}
		}

		protected override void Fatal(string message, object owner, Exception exception = null)
		{
			if (exception == null)
			{
				this.logger.Fatal(message);
			}
			else
			{
				this.logger.Fatal(exception, message);
			}
		}
	}
}
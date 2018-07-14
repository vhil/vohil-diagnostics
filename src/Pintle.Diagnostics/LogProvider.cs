namespace Pintle.Diagnostics
{
	using System;

	public abstract class LogProvider : ILogProvider
	{
		protected LogProvider(string logLevel)
		{
			SeverityLevel level;
			if (Enum.TryParse(logLevel, true, out level))
			{
				this.LogLevel = level;
			}
			else
			{
				throw new NotSupportedException($"LogLevel value '{logLevel}' is not supported.");
			}
		}

		public SeverityLevel LogLevel { get; }

		public void Log(SeverityLevel level, string message, object owner, Exception exception = null)
		{
			var msg = this.FormatMessage(message, owner);

			if ((int)this.LogLevel <= (int)level)
			{
				switch (level)
				{
					case SeverityLevel.Debug:
						this.Debug(msg, owner);
						break;
					case SeverityLevel.Info:
						this.Info(msg, owner);
						break;
					case SeverityLevel.Audit:
						this.Audit($"Audit: {msg}", owner);
						break;
					case SeverityLevel.Warn:
						this.Warn(msg, owner, exception);
						break;
					case SeverityLevel.Error:
						this.Error(msg, owner, exception);
						break;
					case SeverityLevel.Fatal:
						this.Fatal(msg, owner, exception);
						break;
					default:
						throw new NotSupportedException($"Log level '{level.ToString()}' is not supported.");
				}
			}
		}

		protected virtual string FormatMessage(string message, object owner)
		{
			string ownerName;

			var type = owner as Type;
			if (type != null)
			{
				ownerName = type.Name;
			}
			else if (owner is string)
			{
				ownerName = owner.ToString();
			}
			else
			{
				ownerName = owner?.GetType().Name ?? "NULL";
			}
			
			return $"[{ownerName}]: {message}";
		}

		protected abstract void Audit(string message, object owner);

		protected abstract void Debug(string message, object owner);

		protected abstract void Info(string message, object owner);

		protected abstract void Warn(string message, object owner, Exception exception = null);

		protected abstract void Error(string message, object owner, Exception exception = null);

		protected abstract void Fatal(string message, object owner, Exception exception = null);
	}
}
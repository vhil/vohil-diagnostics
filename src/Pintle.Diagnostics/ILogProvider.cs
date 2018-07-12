namespace Pintle.Diagnostics
{
	using System;

    public interface ILogProvider
    {
        SeverityLevel LogLevel { get; }
        void Log(SeverityLevel level, string message, object owner, Exception exception = null);
    }
}
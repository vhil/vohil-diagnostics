using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Xml;

namespace Pintle.Diagnostics.Sitecore
{
	public static class ConfigurationReader
	{
		private static readonly XmlDocument configuration = ConfigReader.GetConfiguration();

		private const string DefaultLoggerNodePath = "/sitecore/pintle/logging/defaultLogger";
		private const string DefaultSitecoreLogNodePath = "/sitecore/pintle/logging/providers/defaultSitecoreLogger";

		public static IEnumerable<ILogProvider> GetDefaultLogProviders()
		{
			var source = configuration.SelectNodes($"{DefaultLoggerNodePath}/param");

			if (source == null || source.Count <= 0)
				return Enumerable.Empty<ILogProvider>();

			var ret = new List<ILogProvider>();

			foreach (XmlNode param in source)
			{
				var refAttr = XmlUtil.GetAttribute("ref", param);
				var logProvider = Factory.CreateObject(refAttr, false) as ILogProvider;
				if (logProvider != null && !(logProvider is SitecoreLogProvider))
				{
					ret.Add(logProvider);
				}
			}

			return ret;
		}

		public static string GetSitecoreLogProviderLevel()
		{
			var source = configuration.SelectNodes($"{DefaultSitecoreLogNodePath}/param");
			if (source == null || source.Count <= 0) return "Debug";

			foreach (XmlNode param in source)
			{
				var value = XmlUtil.GetValue(param);
				return value;
			}

			return "Debug";
		}
	}
}

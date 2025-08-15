namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    using System;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;

    public static class XmlErrorHelper
    {
        public static string GetErrorMessage<T>(string xmlErrorMessage, ILogger<T> logger)
        {
            if (!string.IsNullOrEmpty(xmlErrorMessage) && xmlErrorMessage.StartsWith('<'))
            {
                try
                {
                    var doc = XDocument.Parse(xmlErrorMessage);
                    return doc.Root.Attribute("Msg").Value;
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Invalid xml content: " + xmlErrorMessage);
                }
            }

            return xmlErrorMessage;
        }
    }
}

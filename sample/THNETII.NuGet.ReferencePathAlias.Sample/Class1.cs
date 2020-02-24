extern alias AWSSDK_Extensions_NETCore_Setup;
#if NETFRAMEWORK
extern alias NetFrameworkSystemXml;
#endif

using System;

using AwsConfigurationExtensions = AWSSDK_Extensions_NETCore_Setup::Microsoft.Extensions.Configuration.ConfigurationExtensions;

#if NETFRAMEWORK
using NetFrameworkXmlElement = NetFrameworkSystemXml::System.Xml.XmlElement;
#endif

namespace THNETII.NuGet.ReferencePathAlias.Sample
{
    public static class Class1
    {
        public static Type AwsConfigurationExtensionsType { get; } =
            typeof(AwsConfigurationExtensions);

#if NETFRAMEWORK
        public static Type XmlElementType { get; } =
            typeof(NetFrameworkXmlElement);
#endif
    }
}

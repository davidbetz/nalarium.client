#region Copyright
//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2007-2009
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
#endregion
using System;
using System.Configuration;
//+
namespace Nalarium.Client.Configuration
{
    /// <summary>
    /// Provides access to the configuration section.
    /// </summary>
    public class ClientSection : Nalarium.Configuration.AppConfig.ConfigurationSection
    {
        //<client>
        //    <frameworks basePath="~/Content/Lib">
        //        <add name="jQuery">
        //            <resources tracing="true">
        //                <add path="jquery-1.4.js" minPath="jquery-1.4.min.js" />
        //            </resources>
        //        </add>
        //    </frameworks>
        //</client>

        //- @Frameworks -//
        [ConfigurationProperty("frameworks")]
        [ConfigurationCollection(typeof(FrameworkElement), AddItemName = "add")]
        public FrameworkCollection Frameworks
        {
            get
            {
                return (FrameworkCollection)this["frameworks"];
            }
            set
            {
                this["frameworks"] = value;
            }
        }

        //- @ResourceHandlerPath -//
        [ConfigurationProperty("resourceHandlerPath", DefaultValue = "/ClientResourceHandler.axd")]
        public String ResourceHandlerPath
        {
            get
            {
                return (String)this["resourceHandlerPath"];
            }
        }

        //+
        //- @GetConfigSection -//
        /// <summary>
        /// Gets the config section.
        /// </summary>
        /// <returns>Configuration section</returns>
        public static ClientSection GetConfigSection()
        {
            return GetConfigSection<ClientSection>("nalarium/client");
        }
    }
}
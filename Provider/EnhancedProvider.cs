#region Copyright
//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
#endregion
using System;
using System.Collections.Generic;
using System.Web.UI;
//+
//using Nalarium.Client.Configuration;
//+
namespace Nalarium.Client.Provider
{
    public class EnhancedProvider : Nalarium.Client.Provider.ProviderBase
    {
        private static Type _type = typeof(EnhancedProvider);

        //+
        //- @Framework -//
        public override String Framework
        {
            get
            {
                return Nalarium.Client.Framework.Enhanced;
            }
        }

        //- @DomReadyTemplate -//
        public override String DomLoadTemplate
        {
            get
            {
                return @"throw 'DOM Ready support required either jQuery or Prototype';";
            }
        }

        //- @PageLoadTemplate -//
        public override String PageLoadTemplate
        {
            get
            {
                return @"
Event.add(window, 'load', function( ) {
{Code}
});
";
            }
        }

        //- #AddCommonScriptData -//
        protected override void AddBaseScriptData(ScriptCatalog scriptCatalog)
        {
            base.AddBaseScriptData(scriptCatalog);
            //+
            scriptCatalog.ScriptResourceList.Add(
                    ScriptResource.Create()
                                  .ScriptType(_type)
                                  .ResourceName("Nalarium.Client._Resource._Manual.TinyFX.js")
                                  .ScriptPriority(ScriptPriority.Core)
            );
            //+
            scriptCatalog.ScriptResourceList.Add(
                    ScriptResource.Create()
                                  .ScriptType(_type)
                                  .ResourceName("Nalarium.Client._Resource._Manual.Base64.js")
                                  .ScriptPriority(ScriptPriority.Required)
            );
            scriptCatalog.ScriptResourceList.Add(
                    ScriptResource.Create()
                                  .ScriptType(_type)
                                  .ResourceName("Nalarium.Client._Resource._Manual.Integrity.js")
                                  .ScriptPriority(ScriptPriority.Required)
            );
        }
    }
}
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
//+
//+
namespace Nalarium.Client.Provider
{
    public abstract class ProviderBase
    {
        private Type _type = typeof(ProviderBase);

        //+
        //- #Controller -//
        public ClientController Controller { get; set; }

        //- @Framework -//
        public abstract String Framework { get; }

        //- @DomReadyTemplate -//
        public abstract String DomLoadTemplate { get; }

        //- @PageLoadTemplate -//
        public abstract String PageLoadTemplate { get; }

        //- @ImportInto -//
        public void ImportInto(ScriptCatalog scriptCatalog)
        {
            AddBaseScriptData(scriptCatalog);
            AddAbstract(scriptCatalog);
            AddBinderData(scriptCatalog);
            //AddAspNetWebFormScriptData(scriptCatalog);
        }

        //- #AddBaseScriptData -//
        protected virtual void AddBaseScriptData(ScriptCatalog scriptCatalog)
        {
        }

        //- #AddAbstract -//
        protected virtual void AddAbstract(ScriptCatalog scriptCatalog)
        {
            Type type = GetType();
            scriptCatalog.ScriptResourceList.Add(
                ScriptResource.Create()
                              .ScriptType(type)
                              .ResourceName("Nalarium.Client._Resource._Abstract.WCF.js")
                              .ScriptPriority(ScriptPriority.Required)
            );
            scriptCatalog.ScriptResourceList.Add(
                ScriptResource.Create()
                              .ScriptType(type)
                              .ResourceName("Nalarium.Client._Resource._Abstract.AjaxLoader.js")
                              .ScriptPriority(ScriptPriority.Required)
            );
        }

        //- #AddBinderData -//
        protected virtual void AddBinderData(ScriptCatalog scriptCatalog)
        {
        }
    }
}
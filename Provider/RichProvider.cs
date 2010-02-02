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
namespace Nalarium.Client.Provider
{
    public class RichProvider : Nalarium.Client.Provider.BasicProvider
    {
        private static Type _type = typeof(RichProvider);

        //+
        //- #AddBaseScriptData -//
        protected override void AddBaseScriptData(ScriptCatalog scriptCatalog)
        {
            foreach (Nalarium.Client.Configuration.ResourceElement element in Controller.FrameworkElement.Resources)
            {
                scriptCatalog.FrameworkCoreIncludeList.Add(ScriptInclude
                                                    .Create()
                                                    .Path(Nalarium.Client.Configuration.ClientSection.GetConfigSection().Frameworks.BasePath + "/" + Nalarium.Web.UrlCleaner.CleanWebPathHead(element.Path))
                                                    .MinimizedPath(element.MinimizedPath));
            }
            //+
            base.AddBaseScriptData(scriptCatalog);
            //+
            scriptCatalog.ScriptResourceList.Add(
                    ScriptResource.Create()
                                  .ScriptType(_type)
                                  .ResourceName("Nalarium.Client._Resource._Manual.Integrity.js")
                                  .ScriptPriority(ScriptPriority.Required)
            );
        }
    }
}
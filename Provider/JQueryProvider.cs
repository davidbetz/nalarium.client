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
    public class JQueryProvider : Nalarium.Client.Provider.RichProvider
    {
        private static Type _type = typeof(JQueryProvider);

        //+
        //- @Framework -//
        public override String Framework
        {
            get
            {
                return Nalarium.Client.Framework.JQuery;
            }
        }

        //- @DomReadyTemplate -//
        public override String DomLoadTemplate
        {
            get
            {
                return @"
$(function( ) {
{Code}
});";
            }
        }

        //- @PageLoadTemplate -//
        public override String PageLoadTemplate
        {
            get
            {
                return @"
$(window).bind('load', function( ) {
{Code}
});";
            }
        }

        //- ~CreateClientBinder -//
        protected override void AddBinderData(ScriptCatalog scriptCatalog)
        {
            scriptCatalog.ScriptResourceList.Add(
                    ScriptResource.Create()
                                  .ScriptType(_type)
                                  .ResourceName("Nalarium.Client._Resource.JQuery.Binder.js")
                                  .ScriptPriority(ScriptPriority.Required)
            );
        }
    }
}
﻿#region Copyright
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
    public class NoneProvider : Nalarium.Client.Provider.ProviderBase
    {
        private static Type _type = typeof(NoneProvider);

        //+
        //- @Framework -//
        public override String Framework
        {
            get
            {
                return Nalarium.Client.Framework.None;
            }
        }

        //- @DomReadyTemplate -//
        public override String DomLoadTemplate
        {
            get
            {
                return @"throw 'DOM Ready support requires either jQuery or Prototype';";
            }
        }

        //- @PageLoadTemplate -//
        public override String PageLoadTemplate
        {
            get
            {
                return @"
window.__load = function( ) {
{Code}
};
if(window.addEventListener) {
    window.addEventListener('load', window.__load, false);
}
else if(window.attachEvent) {
    window.attachEvent('onload', window.__load);
}";
            }
        }
    }
}
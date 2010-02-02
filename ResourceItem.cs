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
namespace Nalarium.Client
{
    public class ResourceItem
    {
        public String ResourceName { get; set; }

        public String AssemblyName { get; set; }

        public Boolean InPage { get; set; }

        public String[] ParameterArray { get; set; }
    }
}
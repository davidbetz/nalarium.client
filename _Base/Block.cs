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
    public abstract class Block
    {
        private Boolean isEnabled = true;

        //+
        //- @IsEnabled -//
        public Boolean IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        //- @Controller -//
        public ClientController Controller { get; internal set; }

        //+
        //- ~Register -//
        internal void Register()
        {
            if (Controller.Framework().Equals("prototype", StringComparison.InvariantCultureIgnoreCase))
            {
                RegisterPrototype();
            }
            else if (Controller.Framework().Equals("jquery", StringComparison.InvariantCultureIgnoreCase))
            {
                RegisterJQuery();
            }
            else if (Controller.Framework().Equals("enhanced", StringComparison.InvariantCultureIgnoreCase))
            {
                RegisterEnhanced();
            }
            else if (Controller.Framework().Equals("basic", StringComparison.InvariantCultureIgnoreCase))
            {
                RegisterBasic();
            }
        }

        //- #RegisterBasic -//
        protected virtual void RegisterBasic()
        {
        }

        //- #RegisterEnhanced -//
        protected virtual void RegisterEnhanced()
        {
        }

        //- #RegisterPrototype -//
        protected virtual void RegisterPrototype()
        {
        }

        //- #RegisterJQuery -//
        protected virtual void RegisterJQuery()
        {
        }
    }
}
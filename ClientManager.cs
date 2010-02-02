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
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
//+
using Nalarium.Web;
//+
namespace Nalarium.Client
{
    public class ClientManager : System.Web.UI.Control
    {
        //- ~Info -//
        internal class Info
        {
            public const String Scope = "ClientResource";
            public const String Page = "Page";
        }

        private static Type _type = typeof(ClientManager);
        private ClientController _controller;

        //+
        //- @DebugMode -//
        public Boolean DebugMode
        {
            get
            {
                return _controller.DebugMode;
            }
            set
            {
                _controller.DebugMode = value;
            }
        }

        //- @IncludeTracing -//
        public Boolean IncludeTracing
        {
            get
            {
                return _controller.IncludeTracing;
            }
            set
            {
                _controller.IncludeTracing = value;
            }
        }

        //- @Framework -//
        public String Framework
        {
            get
            {
                return _controller.Framework();
            }
            set
            {
                _controller.Framework(value);
            }
        }

        //- @OutputMode -//
        public OutputMode OutputMode
        {
            get
            {
                return _controller.OutputMode();
            }
            set
            {
                _controller.OutputMode(value);
            }
        }

        //- @Combine -//
        public Boolean Combine
        {
            get
            {
                return _controller.Combine();
            }
            set
            {
                _controller.Combine(value);
            }
        }

        //- @Compress -//
        public Boolean Compress
        {
            get
            {
                return _controller.Compress();
            }
            set
            {
                _controller.Compress(value);
            }
        }

        //- @Minimize -//
        public Boolean Minimize
        {
            get
            {
                return _controller.Minimize();
            }
            set
            {
                _controller.Minimize(value);
            }
        }

        //+
        //- @Ctor -//
        public ClientManager(ClientController controller)
        {
            if (_controller == null)
            {
                throw new InvalidOperationException("Controller may not be null");
            }
            CheckAndSave();
            _controller = controller;
            _controller.Framework(Nalarium.Client.Framework.None);
            IncludeTracing = true;
        }
        public ClientManager()
        {
            CheckAndSave();
            _controller = new ClientController();
            _controller.Framework(Nalarium.Client.Framework.None);
            IncludeTracing = true;
        }

        //+
        //- @CheckAndSave -//
        private void CheckAndSave()
        {
            if (Http.Page != null && Http.Page.Items != null)
            {
                ClientManager cm = Http.Page.Items[_type] as ClientManager;
                if (cm != null)
                {
                    throw new InvalidOperationException("Only one ClientManager is allowed.");
                }
                else
                {
                    Http.Page.Items[_type] = this;
                }
            }
        }

        //- @GetCurrent -//
        /// <summary>
        /// Obtains the instance of the ClientManager for the current page.
        /// </summary>
        /// <returns>The ClientManager object.</returns>
        public static ClientManager GetCurrent()
        {
            return GetCurrent(Http.Page);
        }
        /// <summary>
        /// Obtains the current instance of the ClientManager.
        /// </summary>
        /// <param name="page">The Page object in which to look for the ClientManager.</param>
        /// <returns>The ClientManager object.</returns>
        public static ClientManager GetCurrent(System.Web.UI.Page page)
        {
            ClientManager cm = page.Items[_type] as ClientManager;
            if (cm == null)
            {
                cm = new ClientManager();
                page.Items[_type] = cm;
            }
            //+
            return cm;
        }

        public static void RegisterToPage(System.Web.UI.Page page, ClientController controller)
        {
            RegisterToPage(page, new ClientManager(controller));
        }
        public static void RegisterToPage(System.Web.UI.Page page, ClientManager manager)
        {
            page.Items[_type] = manager;
        }

        //- @Controller -//
        /// <summary>
        /// Represents the controller used to add scripts and styles to an entity.
        /// </summary>
        public static ClientController Controller
        {
            get
            {
                ClientManager clientManager = GetCurrent();
                if (clientManager == null)
                {
                    return null;
                }
                //+
                return clientManager._controller;
            }
        }

        //- #OnInit -//
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            System.Web.UI.Page page = this.Page;
            if (GetCurrent(this.Page) != null)
            {
                return;
                //throw new InvalidOperationException(Resource.ClientManager_OnlyOne);
            }
            page.Items[_type] = this;
        }

        //- #OnPreRender -//
        protected override void OnPreRender(EventArgs e)
        {
            _controller.Render();
            //+
            base.OnPreRender(e);
        }
    }
}

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
    public class ClientHelper
    {
        private ClientController _controller;

        //+
        //- @Ctor -//
        public ClientHelper(ClientController controller)
        {
            controller.OutputMode(OutputMode.Manual);
            _controller = controller;
        }

        //+
        //- @OutputScriptData -//
        public System.Web.HtmlString OutputScriptData()
        {
            return new System.Web.HtmlString(_controller.GenerateOutput());
        }

        //- @Compress -//
        public Boolean Compress()
        {
            return _controller.Compress();
        }
        public ClientHelper Compress(Boolean compress)
        {
            _controller.Compress(compress);
            //+
            return this;
        }

        //- @Minimize -//
        public Boolean Minimize()
        {
            return _controller.Minimize();
        }
        public ClientHelper Minimize(Boolean minimize)
        {
            _controller.Minimize(minimize);
            //+
            return this;
        }

        //- @Combine -//
        public Boolean Combine()
        {
            return _controller.Combine();
        }
        public ClientHelper Framework(Boolean combine)
        {
            _controller.Combine(combine);
            //+
            return this;
        }

        //- @Framework -//
        public String Framework()
        {
            return _controller.Framework();
        }
        public ClientHelper Framework(String framework)
        {
            _controller.Framework(framework);
            //+
            return this;
        }

        //- SCRIPT -//
        #region SCRIPT

        //- @RegisterOnDomLoadData -//
        public ClientHelper OnDomLoadData(String scriptData)
        {
            OnDomLoadData(Script.Create().Content(scriptData));
            //+
            return this;
        }
        public ClientHelper OnDomLoadData(String scriptData, Boolean isVolatile)
        {
            OnDomLoadData(Script.Create().Content(scriptData).IsVolatile(isVolatile));
            //+
            return this;
        }
        public ClientHelper OnDomLoadData(Script script)
        {
            _controller.OnDomLoadData(script);
            //+
            return this;
        }

        //- @RegisterOnLoadData -//
        public ClientHelper OnLoadData(String scriptData)
        {
            OnLoadData(Script.Create().Content(scriptData));
            //+
            return this;
        }
        public ClientHelper OnLoadData(String scriptData, Boolean isVolatile)
        {
            OnLoadData(Script.Create().Content(scriptData).IsVolatile(isVolatile));
            //+
            return this;
        }
        public ClientHelper OnLoadData(Script script)
        {
            _controller.OnLoadData(script);
            //+
            return this;
        }

        //- @RegisterAdHocScriptData -//
        public ClientHelper AdHocScriptData(String scriptData)
        {
            AdHocScriptData(Script.Create().Content(scriptData));
            //+
            return this;
        }
        public ClientHelper AdHocScriptData(String scriptData, Boolean isVolatile)
        {
            AdHocScriptData(Script.Create().Content(scriptData).IsVolatile(isVolatile));
            //+
            return this;
        }
        public ClientHelper AdHocScriptData(Script script)
        {
            _controller.AdHocScriptData(script);
            //+
            return this;
        }

        //- @RegisterScriptInclude -//
        public ClientHelper ScriptInclude(String path)
        {
            ScriptInclude(Nalarium.Client.ScriptInclude.Create().Path(path));
            //+
            return this;
        }
        public ClientHelper ScriptInclude(ScriptInclude script)
        {
            _controller.ScriptInclude(script);
            //+
            return this;
        }

        //- @RegisterInlineScriptData -//
        public ClientHelper InlineScriptData(String script)
        {
            _controller.InlineScriptData(script);
            //+
            return this;
        }
        public ClientHelper InlineScriptData(String key, String script)
        {
            _controller.InlineScriptData(key, script);
            //+
            return this;
        }
        public ClientHelper InlineScriptData(Type type, String key, String script)
        {
            _controller.InlineScriptData(type, key, script);
            //+
            return this;
        }
        public ClientHelper InlineScriptData(Type type, String key, String script, Boolean addScriptTags)
        {
            _controller.InlineScriptData(type, key, script, addScriptTags);
            //+
            return this;
        }

        //- @RegisterScriptResource -//
        public ClientHelper ScriptResource(Type type, String resourceName)
        {
            ScriptResource(Nalarium.Client.ScriptResource.Create()
                                                 .ScriptType(type)
                                                 .ResourceName(resourceName)
                                                 .Framework(Nalarium.Client.Framework.None)
            );
            //+
            return this;
        }
        public ClientHelper ScriptResource(ScriptResource resource)
        {
            _controller.ScriptResource(resource);
            //+
            return this;
        }

        #endregion
    }
}
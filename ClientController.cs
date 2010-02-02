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
    public class ClientController
    {
        //- ~Info -//
        internal class Info
        {
            public const String ContentType = "text/javascript";
        }

        //+
        internal static Type _type = typeof(ClientController);

        //+ field
        private ScriptCatalog _scriptCatalog = new ScriptCatalog();
        private Map<String, Block> _blockList = new Map<String, Block>();
        private List<Style> _styleList = new List<Style>();
        private List<StyleResource> _styleResourceList = new List<StyleResource>();
        private String _framework = String.Empty;
        //private Boolean _frameworkIsSet = false;

        //+
        //- @DebugMode -//
        public Boolean DebugMode { get; set; }

        //- @IncludeTracing -//
        public Boolean IncludeTracing { get; set; }

        //- @FrameworkElement -//
        public Nalarium.Client.Configuration.FrameworkElement FrameworkElement { get; private set; }

        internal String ScriptBasePath
        {
            get
            {
                return Nalarium.Client.Configuration.ClientSection.GetConfigSection().Frameworks.BasePath;
            }
        }

        //- @Framework -//
        public String Framework()
        {
            return _framework;
        }
        public ClientController Framework(String framework)
        {
            //if (framework.Equals(Nalarium.Client.Framework.NotSet))
            //{
            //    return this;
            //}
            if (!String.IsNullOrEmpty(_framework))
            {
                throw new InvalidOperationException(Resource.ClientManager_FrameworkAlreadySet);
            }
            _framework = framework;
            //+
            FrameworkElement = Nalarium.Client.Configuration.ClientSection.GetConfigSection().Frameworks.SingleOrDefault(p => p.Name == _framework);
            if (!_framework.Equals(Nalarium.Client.Framework.None) &&
                !_framework.Equals(Nalarium.Client.Framework.Basic) &&
                !_framework.Equals(Nalarium.Client.Framework.Enhanced) &&
                FrameworkElement == null)
            {
                throw new InvalidOperationException("Framework configuration is required.");
            }
            //+
            return this;
        }

        //- @OutputMode -//
        private OutputMode _outputMode { get; set; }
        private Boolean _combine { get; set; }
        private Boolean _compress { get; set; }
        private Boolean _minimize { get; set; }

        //- @OutputMode -//
        public OutputMode OutputMode()
        {
            return _outputMode;
        }
        public ClientController OutputMode(OutputMode mode)
        {
            _outputMode = mode;
            //+
            return this;
        }

        //- @Combine -//
        public Boolean Combine()
        {
            return _combine;
        }
        public ClientController Combine(Boolean combine)
        {
            _combine = combine;
            //+
            return this;
        }

        //- @Compress -//
        public Boolean Compress()
        {
            return _compress;
        }
        public ClientController Compress(Boolean compress)
        {
            _compress = compress;
            //+
            return this;
        }

        //- @Minimize -//
        public Boolean Minimize()
        {
            return _minimize;
        }
        public ClientController Minimize(Boolean minimize)
        {
            _minimize = minimize;
            //+
            return this;
        }

        //+
        //- Ctor -//
        public ClientController()
        {
        }

        //- @RegisterBlock -//
        public ClientController Block(Block block)
        {
            if (block != null)
            {
                block.Controller = this;
                String key = block.GetType().FullName;
                if (!_blockList.ContainsKey(key))
                {
                    _blockList.Add(key, block);
                }
            }
            //+
            return this;
        }

        //- SCRIPT -//
        #region SCRIPT

        //- @RegisterOnDomLoadData -//
        public ClientController OnDomLoadData(String scriptData)
        {
            OnDomLoadData(Script.Create().Content(scriptData));
            //+
            return this;
        }
        public ClientController OnDomLoadData(String scriptData, Boolean isVolatile)
        {
            OnDomLoadData(Script.Create().Content(scriptData).IsVolatile(isVolatile));
            //+
            return this;
        }
        public ClientController OnDomLoadData(Script script)
        {
            if (script.IsVolatile())
            {
                _scriptCatalog.DomLoadDataListVolatile.Add(script);
            }
            else
            {
                _scriptCatalog.DomLoadDataList.Add(script);
            }
            //+
            return this;
        }

        //- @RegisterOnLoadData -//
        public ClientController OnLoadData(String scriptData)
        {
            OnLoadData(Script.Create().Content(scriptData));
            //+
            return this;
        }
        public ClientController OnLoadData(String scriptData, Boolean isVolatile)
        {
            OnLoadData(Script.Create().Content(scriptData).IsVolatile(isVolatile));
            //+
            return this;
        }
        public ClientController OnLoadData(Script script)
        {
            if (script.IsVolatile())
            {
                _scriptCatalog.LoadDataListVolatile.Add(script);
            }
            else
            {
                _scriptCatalog.LoadDataList.Add(script);
            }
            //+
            return this;
        }

        //- @RegisterAdHocScriptData -//
        public ClientController AdHocScriptData(String scriptData)
        {
            AdHocScriptData(Script.Create().Content(scriptData));
            //+
            return this;
        }
        public ClientController AdHocScriptData(String scriptData, Boolean isVolatile)
        {
            AdHocScriptData(Script.Create().Content(scriptData).IsVolatile(isVolatile));
            //+
            return this;
        }
        public ClientController AdHocScriptData(Script script)
        {
            if (script.IsVolatile())
            {
                _scriptCatalog.AdHocDataListVolatile.Add(script);
            }
            else
            {
                _scriptCatalog.AdHocDataList.Add(script);
            }
            //+
            return this;
        }

        //- @RegisterScriptInclude -//
        public ClientController ScriptInclude(String path)
        {
            ScriptInclude(Nalarium.Client.ScriptInclude.Create().Path(path));
            //+
            return this;
        }
        public ClientController ScriptInclude(ScriptInclude script)
        {
            if (!_scriptCatalog.ScriptIncludeList.Any(p => p.Path().Equals(script.Path())))
            {
                _scriptCatalog.ScriptIncludeList.Add(script);
            }
            //+
            return this;
        }

        //- @RegisterInlineScriptData -//
        public ClientController InlineScriptData(String script)
        {
            if (OutputMode() == Nalarium.Client.OutputMode.Classic)
            {
                Http.Page.ClientScript.RegisterClientScriptBlock(_type, GuidCreator.GetNewGuid(), script, true);
            }
            else
            {
                Http.Response.Write(script);
            }
            //+
            return this;
        }
        public ClientController InlineScriptData(String key, String script)
        {
            if (OutputMode() == Nalarium.Client.OutputMode.Classic)
            {
                Http.Page.ClientScript.RegisterClientScriptBlock(_type, key, script, true);
            }
            else
            {
                Http.Response.Write(script);
            }
            //+
            return this;
        }
        public ClientController InlineScriptData(Type type, String key, String script)
        {
            if (OutputMode() == Nalarium.Client.OutputMode.Classic)
            {
                Http.Page.ClientScript.RegisterClientScriptBlock(type, key, script, true);
            }
            else
            {
                Http.Response.Write(script);
            }
            //+
            return this;
        }
        public ClientController InlineScriptData(Type type, String key, String script, Boolean addScriptTags)
        {
            if (OutputMode() == Nalarium.Client.OutputMode.Classic)
            {
                Http.Page.ClientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
            }
            else
            {
                Http.Response.Write(script);
            }
            //+
            return this;
        }

        //- @RegisterScriptResource -//
        public ClientController ScriptResource(Type type, String resourceName)
        {
            ScriptResource(Nalarium.Client.ScriptResource.Create()
                                                 .ScriptType(type)
                                                 .ResourceName(resourceName)
                                                 .Framework(Nalarium.Client.Framework.None)
            );
            //+
            return this;
        }
        public ClientController ScriptResource(ScriptResource resource)
        {
            _scriptCatalog.ScriptResourceList.Add(resource);
            //+
            return this;
        }

        #endregion

        //- STYLE -//
        #region STYLE

        //- @RegisterStyleInclude -//
        public ClientController StyleInclude(String path)
        {
            if (!_styleList.Any(p => p.Path == path))
            {
                _styleList.Add(new Style
                {
                    Path = path
                });
            }
            //+
            return this;
        }

        //- @RegisterStyleResource -//
        public ClientController StyleResource(Type type, String resourceName)
        {
            if (!_styleResourceList.Any(p => p.Type == type && p.ResourceName == resourceName))
            {
                _styleResourceList.Add(new StyleResource
                {
                    Type = type,
                    ResourceName = resourceName
                });
            }
            //+
            return this;
        }

        //- $AddStyleSheet -//
        private void AddStyleSheet(String path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                HtmlGenericControl css = new HtmlGenericControl("link");
                css.Attributes.Add("href", path);
                css.Attributes.Add("rel", "stylesheet");
                css.Attributes.Add("type", "text/css");
                if (Http.Page.Header == null)
                {
                    throw new Exception("Page <head/> element must have runat=\"server\"");
                }
                Http.Page.Header.Controls.Add(css);
            }
        }

        //- $AddStyleResource -//
        private void AddStyleResource(StyleResource sr)
        {
            if (sr != null)
            {
                AddStyleSheet(Http.Page.ClientScript.GetWebResourceUrl(sr.Type, sr.ResourceName));
            }
        }

        #endregion

        public String GenerateOutput()
        {
            return Render();
        }

        //- ~Render -//
        internal String Render()
        {
            _blockList.GetValueList().ForEach(p => p.Register());
            //+
            String leftover = _scriptCatalog.Write(this);
            if (!String.IsNullOrEmpty(leftover))
            {
                if (OutputMode() == Nalarium.Client.OutputMode.Manual)
                {
                    return leftover;
                }
                else
                {
                    InlineScriptData(_type, "__$Leftover", leftover, true);
                }
            }
            //+ style
            _styleList.ForEach(p => AddStyleSheet(p.Path));
            _styleResourceList.ForEach(sr => AddStyleResource(sr));
            //+
            return String.Empty;
        }
    }
}
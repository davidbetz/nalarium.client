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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
//+
using Nalarium.Client.Provider;
using Nalarium.Web;
//+
namespace Nalarium.Client
{
    [DataContract]
    public class ScriptCatalog
    {
        private static Type _type = typeof(ScriptCatalog);
        private static Map _scriptCache = new Map();

        //+
        //- ~FrameworkCoreIncludeList -//
        [DataMember]
        internal List<ScriptInclude> FrameworkCoreIncludeList { get; set; }

        [DataMember]
        internal List<ScriptInclude> ScriptIncludeList { get; set; }

        //- ~ScriptResourceList -//
        [DataMember]
        internal List<ScriptResource> ScriptResourceList { get; set; }

        //- ~DomLoadDataList -//
        [DataMember]
        internal List<Script> DomLoadDataList { get; set; }

        //- ~DomLoadDataListVolatile -//
        [DataMember]
        internal List<Script> DomLoadDataListVolatile { get; set; }

        //- ~LoadDataList -//
        [DataMember]
        internal List<Script> LoadDataList { get; set; }

        //- ~LoadDataListVolatile -//
        [DataMember]
        internal List<Script> LoadDataListVolatile { get; set; }

        //- ~AdHocDataList -//
        [DataMember]
        internal List<Script> AdHocDataList { get; set; }

        //- ~AdHocDataListVolatile -//
        [DataMember]
        internal List<Script> AdHocDataListVolatile { get; set; }

        //- ~Combine -//
        [DataMember]
        internal Boolean Combine { get; set; }

        //- ~Compress -//
        [DataMember]
        internal Boolean Compress { get; set; }

        //- ~Minimize -//
        [DataMember]
        internal Boolean Minimize { get; set; }

        //- @DomReadyTemplate -//
        [DataMember]
        internal String DomLoadTemplate { get; set; }

        //- @PageLoadTemplate -//
        [DataMember]
        internal String PageLoadTemplate { get; set; }

        //- @Framework -//
        [DataMember]
        internal String Framework { get; set; }

        //- ~IncludeTracing -//
        internal Boolean IncludeTracing { get; set; }

        //- ~Provider -//
        internal ProviderBase Provider { get; set; }

        //- ~ApplicationPath -//
        [DataMember]
        internal String ApplicationPath { get; set; }

        //+
        //- @Ctor -//
        public ScriptCatalog()
        {
            ApplicationPath = Nalarium.Web.Http.Request.ApplicationPath;
            FrameworkCoreIncludeList = new List<ScriptInclude>();
            ScriptIncludeList = new List<ScriptInclude>();
            ScriptResourceList = new List<ScriptResource>();
            DomLoadDataList = new List<Script>();
            DomLoadDataListVolatile = new List<Script>();
            LoadDataList = new List<Script>();
            LoadDataListVolatile = new List<Script>();
            AdHocDataList = new List<Script>();
            AdHocDataListVolatile = new List<Script>();
            //+
            IncludeTracing = true;
        }

        //+
        //- ~Write- //
        internal String Write(ClientController controller)
        {
            String provider = controller.FrameworkElement.Provider;
            if (!String.IsNullOrEmpty(provider))
            {
                Provider = Nalarium.Activation.ObjectCreator.CreateAs<ProviderBase>(provider);
                if (Provider == null)
                {
                    throw new InvalidOperationException("Framework provider for " + controller.Framework() + " not found (" + provider + ").");
                }
            }
            if (Provider == null)
            {
                switch (controller.Framework().ToLower())
                {
                    case Nalarium.Client.Framework.None:
                        Provider = new NoneProvider();
                        break;
                    case Nalarium.Client.Framework.Basic:
                        Provider = new BasicProvider();
                        break;
                    case Nalarium.Client.Framework.Enhanced:
                        Provider = new EnhancedProvider();
                        break;
                    case Nalarium.Client.Framework.Prototype:
                        Provider = new PrototypeProvider();
                        break;
                    case Nalarium.Client.Framework.JQuery:
                        Provider = new JQueryProvider();
                        break;
                    default:
                        if (String.IsNullOrEmpty(provider))
                        {
                            throw new InvalidOperationException("Framework provider for " + controller.Framework() + " not set.");
                        }
                        break;
                }
            }
            //+
            Provider.Controller = controller;
            this.Provider.ImportInto(this);
            ScriptResourceList = ScriptResourceList.OrderBy(s => s.ScriptPriority()).ToList();
            //++ copy for serialization
            Combine = controller.Combine();
            Minimize = controller.Minimize();
            Compress = controller.Compress();
            IncludeTracing = controller.IncludeTracing;
            DomLoadTemplate = Provider.DomLoadTemplate;
            PageLoadTemplate = Provider.PageLoadTemplate;
            Framework = Provider.Framework;
            //+ flatten data
            String leftover = String.Empty;
            String volatileData = GetVolatileData(this);
            //+
            String scriptBasePath = controller.ScriptBasePath;
            //+
            StringBuilder output = new StringBuilder();
            if (this.Combine)
            {
                String data = ScriptCatalog.Serialize(this);
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                String hash = UTF8Encoding.UTF8.GetString(sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(data)));
                String base64Id = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(hash));
                lock (this)
                {
                    if (_scriptCache.ContainsKey(base64Id))
                    {
                        _scriptCache.Remove(base64Id);
                    }
                    _scriptCache.Add(base64Id, data);
                }
                List<ScriptInclude> doNotCombineIncludeList = ScriptIncludeList.Where(p => p.DoNotCombine()).ToList();
                List<ScriptInclude> remoteIncludeList = ScriptIncludeList.Where(p => !p.DoNotCombine() && p.Path().StartsWith("http://") || p.Path().StartsWith("https://")).ToList();
                //+
                remoteIncludeList.ForEach(p => Http.Page.ClientScript.RegisterClientScriptInclude(p.Path(), p.Path()));
                doNotCombineIncludeList.ForEach(p => Http.Page.ClientScript.RegisterClientScriptInclude(p.Path(), p.Path()));
                String path = Nalarium.Client.Configuration.ClientSection.GetConfigSection().ResourceHandlerPath + "?c=" + base64Id;
                if (controller.OutputMode() == OutputMode.Manual)
                {
                    Http.Page.ClientScript.RegisterClientScriptInclude("__$ClientResource", path);
                }
                else
                {
                    Http.Response.Write(CreateScriptElement(path));
                }
                //+
                leftover = volatileData;
            }
            else
            {
                if (controller.OutputMode() == OutputMode.Classic)
                {
                    FrameworkCoreIncludeList.ForEach(p => Http.Page.ClientScript.RegisterClientScriptInclude(p.Path(), p.Path()));
                    ScriptResourceList.ForEach(p => Http.Page.ClientScript.RegisterClientScriptResource(p.ScriptType(), p.ResourceName()));
                    ScriptIncludeList.ForEach(p => Http.Page.ClientScript.RegisterClientScriptInclude(p.Path(), p.Path()));
                    leftover = GetNonVolatileData(this) + "\n" + volatileData;
                    Http.Page.ClientScript.RegisterClientScriptBlock(_type, "AdHoc", CreateScriptWrapElement(leftover));
                }
                else
                {
                    FrameworkCoreIncludeList.ForEach(p => output.AppendLine(CreateScriptElement(p.Path())));
                    ScriptResourceList.ForEach(p =>
                    {
                        //String contentType;
                        //output.AppendLine(ResourceReader.ReadResource(p.ScriptType.Assembly.FullName, p.ResourceName, out contentType));
                        output.AppendLine(CreateScriptElement(CreateExternalResourceUrl(new ResourceItem
                        {
                            AssemblyName = p.ScriptType().Assembly.FullName,
                            ResourceName = p.ResourceName()
                        })));
                    });
                    ScriptIncludeList.ForEach(p => output.AppendLine(CreateScriptElement(p.Path())));
                    output.AppendLine(CreateScriptWrapElement(GetNonVolatileData(this) + "\n" + volatileData));
                }
                //+
                //+
                return output.ToString();
            }
            //+
            return leftover;
        }

        private String CreateScriptWrapElement(String data)
        {
            return @"<script type=""text/javascript"">
" + data + @"
</script>";
        }
        private String CreateScriptElement(String path)
        {
            return "<script type=\"text/javascript\" src=\"" + path + "\"></script>";
        }

        internal static string CreateExternalResourceUrl(ResourceItem resourceItem)
        {
            string code;
            if (resourceItem.AssemblyName.StartsWith("System.Web"))
            {
                code = "s";
            }
            else if (resourceItem.AssemblyName.Contains(","))
            {
                code = "f";
            }
            else
            {
                code = "p";
            }
            string parameterData = string.Empty;
            //if (resourceItem.ParameterArray != null)
            //{
            //    parameterData = VariableDataProcessor.PackageVariableData(resourceItem.ParameterArray);
            //}
            string data = code + resourceItem.AssemblyName + "\x04" + resourceItem.ResourceName;
            if (!String.IsNullOrEmpty(parameterData))
            {
                data += "\x04" + parameterData;
            }
            string urlBase = UrlCleaner.CleanWebPathTail(System.Web.HttpRuntime.AppDomainAppVirtualPath) + "/" + UrlCleaner.CleanWebPathHead(Nalarium.Client.Configuration.ClientSection.GetConfigSection().ResourceHandlerPath) + "?d=";

            return urlBase + MachineKeyCryptographer.EncryptCode(data);
        }

        //- ~GetNonVolatileData -//
        internal static String GetNonVolatileData(ScriptCatalog scriptCatalog)
        {
            Func<Script, Boolean> frameworkIsCompatible = p => IsFrameworkCompatible(scriptCatalog.Framework, p.Framework());
            String domLoadData;
            String loadData;
            String adHoc;
            //+ domLoadData
            StringBuilder builder = new StringBuilder();
            scriptCatalog.DomLoadDataList.Where(frameworkIsCompatible).ToList().ForEach(p => builder.AppendLine(p.Content()));
            domLoadData = builder.ToString();
            //+ loadData
            builder = new StringBuilder();
            scriptCatalog.LoadDataList.Where(frameworkIsCompatible).ToList().ForEach(p => builder.AppendLine(p.Content()));
            loadData = builder.ToString();
            //+ adHoc
            builder = new StringBuilder();
            scriptCatalog.AdHocDataList.Where(frameworkIsCompatible).ToList().ForEach(p => builder.AppendLine(p.Content()));
            adHoc = builder.ToString();
            //+
            builder = new StringBuilder();
            builder.AppendLine(Template.Interpolate(scriptCatalog.DomLoadTemplate, MapEntry.Create("Code", domLoadData)));
            builder.AppendLine(Template.Interpolate(scriptCatalog.PageLoadTemplate, MapEntry.Create("Code", loadData)));
            builder.AppendLine(adHoc);
            //+
            return builder.ToString();
        }

        //- ~GetVolatileData -//
        internal static String GetVolatileData(ScriptCatalog scriptCatalog)
        {
            Func<Script, Boolean> frameworkIsCompatible = p => IsFrameworkCompatible(scriptCatalog.Framework, p.Framework());
            String domLoadDataVolatile;
            String loadDataVolatile;
            String adHocVolatile;
            //+ domLoadDataVolatile
            StringBuilder builder = new StringBuilder();
            scriptCatalog.DomLoadDataListVolatile.Where(frameworkIsCompatible).ToList().ForEach(p => builder.AppendLine(p.Content()));
            domLoadDataVolatile = builder.ToString();
            //+ loadDataVolatile
            builder = new StringBuilder();
            scriptCatalog.LoadDataListVolatile.Where(frameworkIsCompatible).ToList().ForEach(p => builder.AppendLine(p.Content()));
            loadDataVolatile = builder.ToString();
            //+ adHocVolatile
            builder = new StringBuilder();
            scriptCatalog.AdHocDataListVolatile.Where(frameworkIsCompatible).ToList().ForEach(p => builder.AppendLine(p.Content()));
            adHocVolatile = builder.ToString(); ;
            //+
            builder = new StringBuilder();
            builder.AppendLine(Template.Interpolate(scriptCatalog.DomLoadTemplate, MapEntry.Create("Code", domLoadDataVolatile)));
            builder.AppendLine(Template.Interpolate(scriptCatalog.PageLoadTemplate, MapEntry.Create("Code", loadDataVolatile)));
            builder.AppendLine(adHocVolatile);
            //+
            return builder.ToString();
        }

        //- ~IsFrameworkCompatible -//
        internal static Boolean IsFrameworkCompatible(String activeFramework, String requiredFramework)
        {
            if (requiredFramework.Equals(Nalarium.Client.Framework.None, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            //+
            return activeFramework.Equals(requiredFramework, StringComparison.InvariantCultureIgnoreCase);
        }

        //- ~Deserialize -//
        internal static ScriptCatalog Deserialize(String data)
        {
            DataContractSerializer serializer = new DataContractSerializer(ScriptCatalog._type);
            try
            {
                XmlReader xmlReader = XmlReader.Create(new StringReader(data));
                //+
                return (ScriptCatalog)serializer.ReadObject(xmlReader);
            }
            catch
            {
                return null;
            }
        }

        //- ~Serialize -//
        internal static String Serialize(ScriptCatalog scriptCatalog)
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(_type);
            serializer.WriteObject(stream, scriptCatalog);
            //+
            return UTF8Encoding.UTF8.GetString(stream.ToArray());
        }

        //- ~TryGetValue -//
        internal static void TryGetValue(String base64Id, out String data)
        {
            _scriptCache.TryGetValue(base64Id, out data);
        }
    }
}
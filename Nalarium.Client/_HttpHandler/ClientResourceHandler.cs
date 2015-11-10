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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
//+
using Jazmin;
//+
using Nalarium.Net;
using Nalarium.Web;
//+
namespace Nalarium.Client
{
    public class ClientResourceHandler : Nalarium.Web.ReusableHttpHandler
    {
        //- @Process -//
        public override void Process()
        {
#if !DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Http.SetSlidingCache(365);
                System.Web.HttpCachePolicy cache = Response.Cache;
                cache.SetValidUntilExpires(true);
                cache.VaryByParams["d"] = true;
                cache.SetOmitVaryStar(true);
                cache.SetLastModified(DateTime.Now);
            }
#endif
            //+
            String d = Nalarium.Web.HttpData.GetQueryItem("d");
            String c = Nalarium.Web.HttpData.GetQueryItem("c");
            if (!String.IsNullOrEmpty(d))
            {
                ProcessorForSingle(d);
            }
            else if (!String.IsNullOrEmpty(c))
            {
                ProcessorForCombining(c);
            }
            else
            {
                Response.Write("/** " + Resource.ResourceHandler_InvalidScript + " **/");
                return;
            }
        }

        private void ProcessorForSingle(String d)
        {
            ContentType = "text/plain";
            //+
            //+ decrypt
            String decryptedCode = MachineKeyCryptographer.DecryptCode(d);
            if (!decryptedCode.Contains("\x04"))
            {
                Response.Write("Invalid code.");
                return;
            }
            //+
            String[] partArray = decryptedCode.Split('\x04');
            char code = partArray[0][0];
            String fullname = partArray[0].Substring(1, partArray[0].Length - 1);
            String resourceName = partArray[1];
            String parameterData = string.Empty;
            if (partArray.Length > 2)
            {
                parameterData = partArray[2];
            }
            //String[] dataArray = null;
            //if (!String.IsNullOrEmpty(parameterData))
            //{
            //    dataArray = VariableDataProcessor.UnpackageVariableData(parameterData);
            //}
            //+
            String resourceData = String.Empty;
            String contentType = String.Empty;
            switch (code)
            {
                case 'f':
                case 'p':
                    resourceData = ResourceReader.ReadResource(fullname, resourceName, out contentType);
                    break;
                case 's':
                    resourceData = ResourceReader.ReadResource(fullname, "System.Web", out contentType);
                    break;
            }
            //+
            if (!String.IsNullOrEmpty(contentType))
            {
                ContentType = contentType;
            }
            //+
            //HttpCachePolicy cache = response.Cache;
            //cache.SetCacheability(HttpCacheability.Public);
            //cache.SetExpires(DateTime.Now + TimeSpan.FromDays(365));
            //cache.VaryByParams["d"] = true;
            //+
            //if (dataArray != null)
            //{
            //    resourceData = VariableDataProcessor.ProcessVariableData(resourceData, dataArray);
            //}
            Response.Write(resourceData);
        }

        private void ProcessorForCombining(String d)
        {
            ContentType = "text/javascript";
            //+
            String base64Id = d.Replace(" ", "+");
            String data;
            ScriptCatalog.TryGetValue(base64Id, out data);
            ScriptCatalog scriptCatalog = ScriptCatalog.Deserialize(data);
            if (scriptCatalog == null)
            {
                Response.Write("/** " + Resource.ResourceHandler_InvalidScript + " **/");
                return;
            }
            String content;
            foreach (ScriptResource sr in scriptCatalog.ScriptResourceList)
            {
                Assembly assembly = Assembly.Load(sr.FullAssemblyName());
                UnmanagedMemoryStream resourceStream = (UnmanagedMemoryStream)assembly.GetManifestResourceStream(sr.ResourceName());
                if (resourceStream != null)
                {
                    StreamReader reader = new StreamReader(resourceStream, true);
                    content = reader.ReadToEnd();
                    //+ don't minimize things that are already minimized
                    if (scriptCatalog.Minimize && !sr.ResourceName().EndsWith("-minimized.js"))
                    {
                        try
                        {
                            content = JavaScriptCompressor.Compress(content);
                        }
                        catch
                        {
                        }
                    }
                    if (!String.IsNullOrEmpty(content))
                    {
                        Output.AppendLine(String.Format("/** " + Resource.ResourceHandler_CombinedScript + " **/", sr.ResourceName()));
                        Output.AppendLine(content);
                        Output.AppendLine();
                    }
                }
            }
            foreach (ScriptInclude script in scriptCatalog.FrameworkCoreIncludeList)
            {
                content = File.ReadAllText(Server.MapPath(Path.Combine(scriptCatalog.ApplicationPath, script.Path())));
                if (scriptCatalog.Compress)
                {
                    try
                    {
                        content = JavaScriptCompressor.Compress(content);
                    }
                    catch
                    {
                    }
                }
                if (!String.IsNullOrEmpty(content))
                {
                    Output.AppendLine(String.Format("/** " + Resource.ResourceHandler_CombinedScript + " **/", script.Path()));
                    Output.AppendLine(content);
                    Output.AppendLine();
                }
            }
            List<ScriptInclude> localIncludeList = scriptCatalog.ScriptIncludeList.Where(p => !p.Path().StartsWith("http://") && !p.Path().StartsWith("https://")).ToList();
            foreach (ScriptInclude script in localIncludeList)
            {
                if (script.IsDynamic())
                {
                    content = HttpAbstractor.GetWebText(new Uri(script.Path()));
                }
                else
                {
                    content = File.ReadAllText(Server.MapPath(Path.Combine(scriptCatalog.ApplicationPath, script.Path())));
                }
                if (scriptCatalog.Compress)
                {
                    try
                    {
                        content = JavaScriptCompressor.Compress(content);
                    }
                    catch
                    {
                    }
                }
                if (!String.IsNullOrEmpty(content))
                {
                    Output.AppendLine(String.Format("/** " + Resource.ResourceHandler_CombinedScript + " **/", script.Path()));
                    Output.AppendLine(content);
                    Output.AppendLine();
                }
            }
            content = ScriptCatalog.GetNonVolatileData(scriptCatalog);
            if (scriptCatalog.Minimize)
            {
                try
                {
                    content = JavaScriptCompressor.Compress(content);
                }
                catch
                {
                }
            }
            if (!String.IsNullOrEmpty(content))
            {
                Output.AppendLine(String.Format("/** " + Resource.ResourceHandler_CombinedScriptData + " **/"));
                Output.AppendLine(content);
                Output.AppendLine();
            }
            if (scriptCatalog.Compress)
            {
                String encodings = Request.Headers.Get("Accept-Encoding");
                if (!String.IsNullOrEmpty(encodings))
                {
                    if (encodings.ToLower().Contains("gzip"))
                    {
                        Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
                        Response.AppendHeader("Content-Encoding", "gzip");
                    }
                    else if (encodings.Contains("deflate"))
                    {
                        Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress);
                        Response.AppendHeader("Content-Encoding", "deflate");
                    }
                }
            }
        }
    }
}
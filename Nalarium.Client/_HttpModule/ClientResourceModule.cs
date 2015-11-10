//#region Copyright
////+ Nalarium Pro 3.0 - Client Module
////+ Copyright © Jampad Technology, Inc. 2008-2010
////+
////+ This file is a part of the Nalarium Suite.
////+ The use and distribution terms for this software are covered by the
////+ Microsoft Permissive License (Ms-PL) which can be found at
////+ http://www.microsoft.com/opensource/licenses.mspx.
//#endregion
//using System;
//using System.Linq;
////+
//using Nalarium.Web;
////+
//using Nalarium.Client.Configuration;
////+
//namespace Nalarium.Client
//{
//    public class ClientResourceModule : Nalarium.Web.HttpModule
//    {
//        //- #StartUp -//
//        protected override void StartUp()
//        {
//            EnableBeginRequest = true;
//        }

//        //- @BeginRequest -//
//        protected override void BeginRequest()
//        {
//            if (!Http.AbsoluteUrl.Contains("/webresource.axd?d="))
//            {
//                return;
//            }
//            String d = HttpData.GetQueryItem("d");
//            String t = HttpData.GetQueryItem("t");
//            if (String.IsNullOrEmpty(t) || String.IsNullOrEmpty(d))
//            {
//                return;
//            }
//            String[] partArray = Nalarium.Web.MachineKeyCryptographer.DecryptCodeAsStringArray(d);
//            String[] allowedScriptArray = new String[] { "WebForms.js", "WebUIValidation.js" };
//            String currentFile;
//            if (partArray[0] == "s")
//            {
//                currentFile = partArray[2];
//            }
//            else
//            {
//                currentFile = partArray[1];
//            }
//            if (!allowedScriptArray.Contains(currentFile))
//            {
//                return;
//            }
//            foreach (NativeScriptExtractionElement element in ClientSection.GetConfigSection().NativeScriptExtractions)
//            {
//                if (currentFile == element.Name)
//                {
//                    Http.Response.SuppressContent = true;
//                    Http.Response.End();
//                    return;
//                }
//            }
//        }
//    }
//}
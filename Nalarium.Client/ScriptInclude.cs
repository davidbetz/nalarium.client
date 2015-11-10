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
using System.Runtime.Serialization;
//+
namespace Nalarium.Client
{
    [System.Diagnostics.DebuggerDisplay("{Path}, {ContentType}, {IsDynamic}")]
    [DataContract]
    public class ScriptInclude
    {
        [DataMember(Name = "Path")]
        private String _path;
        [DataMember(Name = "MinimizedPath")]
        private String _minimizedPath;
        [DataMember(Name = "IsDynamic")]
        private Boolean _isDynamic;
        [DataMember(Name = "ContentType")]
        private String _contentType;
        [DataMember(Name = "DoNotCombine")]
        private Boolean _doNotCombine;
        [DataMember(Name = "Framework")]
        private String _framework;

        private ScriptInclude()
        {
        }

        public static ScriptInclude Create()
        {
            return new ScriptInclude();
        }

        public String Path()
        {
            return _path;
        }
        public ScriptInclude Path(String path)
        {
            _path = path;
            //+
            return this;
        }

        public String MinimizedPath()
        {
            return _minimizedPath;
        }
        public ScriptInclude MinimizedPath(String minimizedPath)
        {
            _minimizedPath = minimizedPath;
            //+
            return this;
        }

        public String ContentType()
        {
            return _contentType;
        }
        public ScriptInclude ContentType(String contentType)
        {
            _contentType = contentType;
            //+
            return this;
        }

        public Boolean IsDynamic()
        {
            return _isDynamic;
        }
        public ScriptInclude IsDynamic(Boolean isDynamic)
        {
            _isDynamic = isDynamic;
            //+
            return this;
        }

        public String Framework()
        {
            return _framework;
        }
        public ScriptInclude Framework(String framework)
        {
            _framework = framework;
            //+
            return this;
        }

        public Boolean DoNotCombine()
        {
            return _doNotCombine;
        }
        public ScriptInclude DoNotCombine(Boolean doNotCombine)
        {
            _doNotCombine = doNotCombine;
            //+
            return this;
        }
    }
}
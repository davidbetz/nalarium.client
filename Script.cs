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
    [System.Diagnostics.DebuggerDisplay("{Framework}, {Path}")]
    [DataContract]
    public class Script
    {
        [DataMember(Name = "Path")]
        private String _path;
        [DataMember(Name = "ContentType")]
        private String _contentType;
        [DataMember(Name = "Framework")]
        private String _framework = String.Empty;
        [DataMember(Name = "Content")]
        private String _content;
        [DataMember(Name = "IsVolatile")]
        private Boolean _isVolatile;

        private Script()
        {
        }

        public static Script Create()
        {
            return new Script();
        }

        public String Path()
        {
            return _path;
        }
        public Script Path(String path)
        {
            _path = path;
            //+
            return this;
        }

        public String ContentType()
        {
            return _contentType;
        }
        public Script ContentType(String contentType)
        {
            _contentType = contentType;
            //+
            return this;
        }

        public String Content()
        {
            return _content;
        }
        public Script Content(String content)
        {
            _content = content;
            //+
            return this;
        }

        public String Framework()
        {
            return _framework;
        }
        public Script Framework(String framework)
        {
            _framework = framework;
            //+
            return this;
        }

        public Boolean IsVolatile()
        {
            return _isVolatile;
        }
        public Script IsVolatile(Boolean isVolatile)
        {
            _isVolatile = isVolatile;
            //+
            return this;
        }
    }
}
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
    [System.Diagnostics.DebuggerDisplay("{ResourceName}, {FullTypeName}")]
    [DataContract]
    public class ScriptResource
    {
        [DataMember(Name = "FullAssemblyName")]
        private String _fullAssemblyName;
        [DataMember(Name = "FullTypeName")]
        private String _fullTypeName;
        [DataMember(Name = "ScriptPriority")]
        private ScriptPriority _scriptPriority;
        [DataMember(Name = "ResourceName")]
        private String _resourceName;
        [DataMember(Name = "Framework")]
        private String _framework;
        private Type _scriptType;

        private ScriptResource()
        {
            _scriptPriority = Nalarium.Client.ScriptPriority.Normal;
        }

        public static ScriptResource Create()
        {
            return new ScriptResource();
        }

        public String FullAssemblyName()
        {
            return _fullAssemblyName;
        }
        public ScriptResource FullAssemblyName(String fullAssemblyName)
        {
            _fullAssemblyName = fullAssemblyName;
            //+
            return this;
        }

        public String FullTypeName()
        {
            return _fullTypeName;
        }
        public ScriptResource FullTypeName(String fullTypeName)
        {
            _fullTypeName = fullTypeName;
            //+
            return this;
        }

        public ScriptPriority ScriptPriority()
        {
            return _scriptPriority;
        }
        public ScriptResource ScriptPriority(ScriptPriority scriptPriority)
        {
            _scriptPriority = scriptPriority;
            //+
            return this;
        }

        public String ResourceName()
        {
            return _resourceName;
        }
        public ScriptResource ResourceName(String resourceName)
        {
            _resourceName = resourceName;
            //+
            return this;
        }

        public String Framework()
        {
            return _framework;
        }
        public ScriptResource Framework(String framework)
        {
            _framework = framework;
            //+
            return this;
        }

        //- @ScriptType -//
        public Type ScriptType()
        {
            return _scriptType;
        }
        public ScriptResource ScriptType(Type type)
        {
            _scriptType = type;
            FullAssemblyName(_scriptType.Assembly.FullName);
            //+
            return this;
        }
    }
}

using System;
using System.Configuration;
//+
namespace Nalarium.Client.Configuration
{
    public class ResourceCollection : Nalarium.Configuration.CommentableCollection<ResourceElement>
    {
        //- @Tracing -//
        [ConfigurationProperty("tracing", IsRequired = true)]
        public String Tracing
        {
            get
            {
                return (String)this["tracing"];
            }
            set
            {
                this["tracing"] = value;
            }
        }

        //- #GetElementKey -//
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ResourceElement)element).Path;
        }
    }
}
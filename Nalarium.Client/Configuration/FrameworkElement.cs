using Nalarium.Configuration.AppConfig;
using System;
using System.Configuration;
//+
namespace Nalarium.Client.Configuration
{
    public class FrameworkElement : CommentableElement
    {
        //- @Resources -//
        [ConfigurationProperty("resources")]
        [ConfigurationCollection(typeof(ResourceElement), AddItemName = "add")]
        public ResourceCollection Resources
        {
            get
            {
                return (ResourceCollection)this["resources"];
            }
            set
            {
                this["resources"] = value;
            }
        }

        //- @Provider -//
        [ConfigurationProperty("provider")]
        public String Provider
        {
            get
            {
                return (String)this["provider"];
            }
            set
            {
                this["provider"] = value;
            }
        }

        //- @Name -//
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}

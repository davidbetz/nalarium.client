using System;
using System.Configuration;
//+
namespace Nalarium.Client.Configuration
{
    public class FrameworkCollection : Nalarium.Configuration.AppConfig.CommentableCollection<FrameworkElement>
    {
        //- @BasePath -//
        [ConfigurationProperty("basePath", IsRequired = true)]
        public String BasePath
        {
            get
            {
                return (String)this["basePath"];
            }
            set
            {
                this["basePath"] = value;
            }
        }

        //- #GetElementKey -//
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FrameworkElement)element).Name;
        }
    }
}

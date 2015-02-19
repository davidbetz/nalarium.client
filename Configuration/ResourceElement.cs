using System;
using System.Configuration;
//+
namespace Nalarium.Client.Configuration
{
    public class ResourceElement : Nalarium.Configuration.AppConfig.CommentableElement
    {
        //- @Path -//
        [ConfigurationProperty("path", IsRequired = true)]
        public String Path
        {
            get
            {
                return (String)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }

        //- @MinimizedPath -//
        [ConfigurationProperty("minPath")]
        public String MinimizedPath
        {
            get
            {
                return (String)this["minPath"];
            }
            set
            {
                this["minPath"] = value;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FileServer
{
    public class UserManageSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty s_property
       = new ConfigurationProperty(string.Empty, typeof(UserManageSectionCollection), null,
                                       ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public UserManageSectionCollection KeyVaules
        {
            get
            {
                return (UserManageSectionCollection)base[s_property];
            }
        }
    }

    [ConfigurationCollection(typeof(UserManageSectionElement))]
    public class UserManageSectionCollection : ConfigurationElementCollection
    {

        public UserManageSectionCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {

        }

        new public UserManageSectionElement this[string name]
        {
            get
            {
                return (UserManageSectionElement)base.BaseGet(name);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new UserManageSectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserManageSectionElement)element).User;
        }

    }

    public class UserManageSectionElement : ConfigurationElement
    {
        [ConfigurationProperty("user", IsRequired = true)]
        public string User
        {
            get { return (string)base["user"]; }
            set { base["user"] = value; }
        }


    }
}
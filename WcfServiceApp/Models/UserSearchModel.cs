using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WcfServiceApp.Extentions;
using WcfServiceApp.Models.Attributes;

namespace WcfServiceApp.Models
{
    public class UserSearchModel
    {
        [SearchCriteria]
        public string UserId { get; set; }
        [SearchCriteria]
        public string FirstName { get; set; }
        [SearchCriteria]
        public string Surname { get; set; }
        [SearchCriteria]
        public string Gender { get; set; }
        [SearchCriteria]
        public string Mobile { get; set; }
        [SearchCriteria]
        public string Email { get; set; }

        public Boolean HasCriteria()
        {
            //get the properties of this object
            var properties = this.GetType().GetProperties(BindingFlags.Public |
                                                          BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var searchProperties = properties.Where(p => p.CustomAttributes.Select
                (a => a.AttributeType).Contains(typeof(SearchCriteriaAttribute)));

            return searchProperties.Any(sp => sp.GetValue(this).ToStringInstance().HasValue());
        }

        public string QueryString()
        {
            var result = string.Empty;

            var queryList = new List<string>();

            if (UserId.HasValue())
            {
                queryList.Add("UserId =" + "'" + UserId + "'");
            }
            if (FirstName.HasValue())
            {
                queryList.Add("FirstName =" + "'" + FirstName + "'");
            }
            if (Surname.HasValue())
            {
                queryList.Add("Surname =" + "'" + Surname + "'");
            }
            if (Gender.HasValue())
            {
                queryList.Add("Gender =" + "'" + Gender + "'");
            }
            if (Mobile.HasValue())
            {
                queryList.Add("Mobile =" + "'" + Mobile + "'");
            }
            if (Email.HasValue())
            {
                queryList.Add("Email =" + "'" + Email + "'");
            }

            if (queryList.Any())
                result = "Where " + string.Join(" AND ", queryList);

            return result;
        }
    }
}
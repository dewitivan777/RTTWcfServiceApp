using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WcfServiceApp.Extentions;
using WcfServiceApp.Models.Attributes;

namespace WcfServiceApp.Models
{
    public class AddressSearchModel
    {
        [SearchCriteria]
        public string UserId { get; set; }
        [SearchCriteria]
        public string Address { get; set; }
        [SearchCriteria]
        public string AddressType { get; set; }
        [SearchCriteria]
        public string City { get; set; }
        [SearchCriteria]
        public string Province { get; set; }
        [SearchCriteria]
        public int ZipCode_PostalCode { get; set; }

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
            if (Address.HasValue())
            {
                queryList.Add("Address =" + "'" + Address + "'");
            }
            if (AddressType.HasValue())
            {
                queryList.Add("AddressType =" + "'" + AddressType + "'");
            }
            if (City.HasValue())
            {
                queryList.Add("City =" + "'" + City + "'");
            }
            if (Province.HasValue())
            {
                queryList.Add("Province =" + "'" + Province + "'");
            }
            if (ZipCode_PostalCode.ToString().HasValue() && ZipCode_PostalCode != 0)
            {
                queryList.Add("Zipcode_PostalCode =" + ZipCode_PostalCode);
            }

            if (queryList.Any())
                result = "Where " + string.Join(" AND ", queryList);

            return result;
        }
    }
}
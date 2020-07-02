using System;
namespace WcfServiceApp.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class SearchCriteriaAttribute : System.Attribute
        {
        }
}
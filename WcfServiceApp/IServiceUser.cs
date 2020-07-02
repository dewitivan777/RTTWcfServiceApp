using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using DevTrends.WCFDataAnnotations;
using WcfServiceApp.Models;

namespace WcfServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceUser
    {
        //Select statements
        [OperationContract]
        Task<UserData> Get(UserDetails userInfo);
        [OperationContract]
        Task<ServerResponseUser> GetAll(UserSearchQuery query);
        [OperationContract]
        Task<UserAddress> GetAddress(UserAddressDetails addressInfo);
        [OperationContract]
        Task<ServerResponseAddress> GetAllAddress(AddressSearchQuery addressInfo);
        [OperationContract]
        //Export
        Task<ExportData> GetAllExportDetails();

        //Insert statements
        [OperationContract]
        Task<ServerResponse> InsertUserDetails(UserDetails userInfo);
        [OperationContract]
        Task<ServerResponse> InsertUserAddress(UserAddressDetails addressInfo);

        //update statements
        [OperationContract]
        Task<ServerResponse> Update(UserDetails userInfo);
        [OperationContract]
        Task<ServerResponse> UpdateAddress(UserAddressDetails addressInfo);

        //Delete statements
        [OperationContract]
        Task<ServerResponse> Delete(string Id);
        [OperationContract]
        Task<ServerResponse> DeleteAddress(string Id);
    }

    [DataContract]
    public class ExportData
    {
        public ExportData()
        {
            this.ExportTable = new DataTable("ExportTable");
        }

        [DataMember]
        public DataTable ExportTable { get; set; }
    }

    [DataContract]
    public class UserData
    {
        public UserData()
        {
            this.UsersTable = new DataTable("UserTable");
        }

        [DataMember]
        public DataTable UsersTable { get; set; }
    }

    [DataContract]
    public class UserAddress
    {
        public UserAddress()
        {
            this.AddressTable = new DataTable("UserAddressTable");
        }

        [DataMember]
        public DataTable AddressTable { get; set; }
    }

    [DataContract]
    public class ServerResponse
    {
        bool success = false;
        string error_message = string.Empty;

        [DataMember]
        public bool Success
        {
            get { return success; }
            set { success = value; }

        }

        [DataMember]
        public string ErrorMessage
        {
            get { return error_message; }
            set { error_message = value; }
        }
    }

    [DataContract]
    public class ServerResponseUser
    {
        bool success = false;
        string error_message = string.Empty;
        int total = 0;

        IEnumerable<UserDetails> users;

        [DataMember]
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return error_message; }
            set { error_message = value; }
        }

        [DataMember]
        public int Total
        {
            get { return total; }
            set { total = value; }
        }

        [DataMember]
        public IEnumerable<UserDetails> Users
        {
            get { return users; }
            set { users = value; }
        }
    }

    [DataContract]
    public class ServerResponseAddress
    {
        bool success = false;
        string error_message = string.Empty;
        int total = 0;

        IEnumerable<UserAddressDetails> addresses;

        [DataMember]
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return error_message; }
            set { error_message = value; }
        }

        [DataMember]
        public int Total
        {
            get { return total; }
            set { total = value; }
        }

        [DataMember]
        public IEnumerable<UserAddressDetails> Addresses
        {
            get { return addresses; }
            set { addresses = value; }
        }
    }

    [DataContract]
    public class UserSearchQuery
    {
        int limit = 20;
        int offset = 0;

        UserSearchModel userquery;

        [DataMember]
        public int Limit
        {
            get { return limit; }
            set { limit = value; }

        }

        [DataMember]
        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        [DataMember]
        public UserSearchModel UserQuery
        {
            get { return userquery; }
            set { userquery = value; }

        }
    }

    [DataContract]
    public class AddressSearchQuery
    {
        int limit = 20;
        int offset = 0;

        AddressSearchModel addressquery;

        [DataMember]
        public int Limit
        {
            get { return limit; }
            set { limit = value; }

        }

        [DataMember]
        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        [DataMember]
        public AddressSearchModel AddressQuery
        {
            get { return addressquery; }
            set { addressquery = value; }

        }
    }

    public class UserAddressDetails
    {
        string id = string.Empty;
        string userid = string.Empty;
        string address = string.Empty;
        string address_type = string.Empty;
        string city = string.Empty;
        string province = string.Empty;
        int zipcode_postalcode = 0;
        DateTime datecreated = DateTime.MinValue;
        DateTime dateupdated = DateTime.MinValue;

        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        [DataMember]
        [Required]
        public string UserId
        {
            get { return userid; }
            set { userid = value; }
        }


        [DataMember]
        [Required]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }


        [DataMember]
        [Required]
        public string AddressType
        {
            get { return address_type; }
            set { address_type = value; }
        }


        [DataMember]
        [Required]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        [DataMember]
        [Required]
        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        [DataMember]
        [Required]
        public int ZipCode_PostalCode
        {
            get { return zipcode_postalcode; }
            set { zipcode_postalcode = value; }
        }

        [DataMember]
        public DateTime DateCreated
        {
            get { return datecreated; }
            set { datecreated = value; }
        }

        [DataMember]
        public DateTime DateUpdated
        {
            get { return dateupdated; }
            set { dateupdated = value; }
        }
    }

    [DataContract]
    public class UserDetails
    {
        string userid = string.Empty;
        string firstname = string.Empty;
        string surname = string.Empty;
        DateTime dob = DateTime.MinValue;
        string gender = string.Empty;
        string mobile = string.Empty;
        string email = string.Empty;
        string workmobile = string.Empty;
        DateTime datecreated = DateTime.MinValue;
        DateTime dateupdated = DateTime.MinValue;

        [DataMember]
        public string UserId
        {
            get { return userid; }
            set { userid = value; }
        }

        [DataMember]
        [Required]
        public string FirstName
        {
            get { return firstname; }
            set { firstname = value; }
        }

        [DataMember]
        [Required]
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        [DataMember]
        [Required]
        public DateTime DOB
        {
            get { return dob; }
            set { dob = value; }
        }

        [DataMember]
        [Required]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [DataMember]
        [Required]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Invalid Mobile. Must have a length of 10")]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }


        [DataMember]
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [DataMember]
        [Required]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Invalid WorkMobile. Must have a length of 10")]
        public string WorkMobile
        {
            get { return workmobile; }
            set { workmobile = value; }
        }

        [DataMember]
        public DateTime DateCreated
        {
            get { return datecreated; }
            set { datecreated = value; }
        }

        [DataMember]
        public DateTime DateUpdated
        {
            get { return dateupdated; }
            set { dateupdated = value; }
        }
    }
}

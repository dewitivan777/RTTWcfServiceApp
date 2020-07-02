using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
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
        [DataMember]
        public bool Success { get; set; } = false;

        [DataMember]
        public string ErrorMessage { get; set; } = string.Empty;
    }

    [DataContract]
    public class ServerResponseUser
    {
        [DataMember]
        public bool Success { get; set; } = false;

        [DataMember]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember]
        public int Total { get; set; } = 0;

        [DataMember]
        public IEnumerable<UserDetails> Users { get; set; }
    }

    [DataContract]
    public class ServerResponseAddress
    {
        [DataMember]
        public bool Success { get; set; } = false;

        [DataMember]
        public string ErrorMessage { get; set; } = string.Empty;

        [DataMember]
        public int Total { get; set; } = 0;

        [DataMember]
        public IEnumerable<UserAddressDetails> Addresses { get; set; }
    }

    [DataContract]
    public class UserSearchQuery
    {
        [DataMember]
        public int Limit { get; set; } = 20;

        [DataMember]
        public int Offset { get; set; } = 0;

        [DataMember]
        public UserSearchModel UserQuery { get; set; }
    }

    [DataContract]
    public class AddressSearchQuery
    {
        [DataMember]
        public int Limit { get; set; } = 20;

        [DataMember]
        public int Offset { get; set; } = 0;

        [DataMember]
        public AddressSearchModel AddressQuery { get; set; }
    }

    public class UserAddressDetails
    {
        [DataMember]
        public string Id { get; set; } = string.Empty;


        [DataMember]
        [Required]
        public string UserId { get; set; } = string.Empty;


        [DataMember]
        [Required]
        public string Address { get; set; } = string.Empty;


        [DataMember]
        [Required]
        public string AddressType { get; set; } = string.Empty;


        [DataMember]
        [Required]
        public string City { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public string Province { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public int ZipCode_PostalCode { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime DateUpdated { get; set; } = DateTime.MinValue;
    }

    [DataContract]
    public class UserDetails
    {
        [DataMember]
        public string UserId { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public string Surname { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public DateTime DOB { get; set; } = DateTime.MinValue;

        [DataMember]
        [Required]
        public string Gender { get; set; } = string.Empty;

        [DataMember]
        [Required]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Invalid Mobile. Must have a length of 10")]
        public string Mobile { get; set; } = string.Empty;


        [DataMember]
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        [Required]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Invalid WorkMobile. Must have a length of 10")]
        public string WorkMobile { get; set; } = string.Empty;

        [DataMember]
        public DateTime DateCreated { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime DateUpdated { get; set; } = DateTime.MinValue;
    }
}

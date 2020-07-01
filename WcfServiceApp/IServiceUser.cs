using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceUser
    {
        //Select statements
        [OperationContract]
        UserData Get(UserDetails userInfo);

        [OperationContract]
        IEnumerable<UserDetails> GetAll(UserDetails userInfo);

        [OperationContract]
        UserAddress GetAddress(UserAddressDetails addressInfo);

        [OperationContract]
        IEnumerable<UserAddressDetails> GetUserAddress(UserAddressDetails addressInfo);

        [OperationContract]
        ExportData GetAllExportDetails();

        //Insert statements
        [OperationContract]
        bool InsertUserDetails(UserDetails userInfo);
        [OperationContract]
        bool InsertUserAddress(UserAddressDetails addressInfo);

        //update statements
        [OperationContract]
        bool Update(UserDetails userInfo);
        bool UpdateAddress(UserAddressDetails addressInfo);

        //Delete statements
        [OperationContract]
        bool Delete(string Id);
        bool DeleteAddress(string Id);
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
            this.AddressTable = new DataTable("UserAddress");
        }

        [DataMember]
        public DataTable AddressTable { get; set; }
    }

    public class UserAddressDetails
    {
        string id = string.Empty;
        string userid = string.Empty;
        string address = string.Empty;
        string address_type = string.Empty;
        string city = string.Empty;
        string province = string.Empty;
        string zipcode_postalcode = string.Empty;

        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return userid; }
            set { userid = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string AddressType
        {
            get { return address_type; }
            set { address_type = value; }
        }

        [DataMember]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        [DataMember]
        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        [DataMember]
        public string Zipcode_PostalCode
        {
            get { return zipcode_postalcode; }
            set { zipcode_postalcode = value; }
        }
    }

    public class UserDetails
    {
        string userid = string.Empty;
        string firstname = string.Empty;
        string surname = string.Empty;
        string dob = string.Empty;
        string gender = string.Empty;
        string mobile = string.Empty;
        string email = string.Empty;
        string workmobile = string.Empty;

        [DataMember]
        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }

        [DataMember]
        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        [DataMember]
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        [DataMember]
        public string DOB
        {
            get { return dob; }
            set { dob = value; }
        }

        [DataMember]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [DataMember]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }


        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [DataMember]
        public string Workmobile
        {
            get { return workmobile; }
            set { workmobile = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace WcfServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        SqlConnection con =
            new SqlConnection(WebConfigurationManager.ConnectionStrings["DBConstring"].ConnectionString);

        public bool InsertUserDetails(UserDetails userInfo)
        {
            bool success = false;

            con.Open();
            //Check if email is already in use
            SqlCommand validate = new SqlCommand("select count(*) from UserTable where Email = @email", con);
            validate.Parameters.AddWithValue("@email", userInfo.Email);
            int count = Convert.ToInt32(validate.ExecuteScalar());

            if (count == 0)
            {
                SqlCommand cmd =
                    new SqlCommand(
                        "insert into UserTable(UserId,Firstname,Surname,DateOfBirth,Gender,Mobile,Email,WorkMobile) values(@userid,@firstname,@surname,@dob,@gender,@mobile,@email,@work_mobile)",
                        con);

                cmd.Parameters.AddWithValue("@userid",  Guid.NewGuid().ToString("N"));
                cmd.Parameters.AddWithValue("@firstname", userInfo.Firstname);
                cmd.Parameters.AddWithValue("@surname", userInfo.Surname);
                cmd.Parameters.AddWithValue("@dob", userInfo.DOB);
                cmd.Parameters.AddWithValue("@gender", userInfo.Gender);
                cmd.Parameters.AddWithValue("@mobile", userInfo.Mobile);
                cmd.Parameters.AddWithValue("@email", userInfo.Email);
                cmd.Parameters.AddWithValue("@work_mobile", userInfo.Workmobile);

                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    success = true;
                }
            }
            else
            {
                con.Close();
                return success;
            }

            con.Close();
            return success;
        }

        public bool InsertUserAddress(UserAddressDetails userInfo)
        {
            bool success=false;

            con.Open();

            SqlCommand cmd =
                new SqlCommand(
                    "insert into AddressTable(User_ID,Address_type,Address,City,Province,[Zip/Postal_Code]) values(@userid,@address_type,@address,@city,@province,@zip)",
                    con);

            cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString("N"));
            cmd.Parameters.AddWithValue("@userid", userInfo.UserId);
            cmd.Parameters.AddWithValue("@address_type", userInfo.AddressType);
            cmd.Parameters.AddWithValue("@address", userInfo.Address);
            cmd.Parameters.AddWithValue("@city", userInfo.City);
            cmd.Parameters.AddWithValue("@province", userInfo.Province);
            cmd.Parameters.AddWithValue("@zip", userInfo.Zipcode_PostalCode);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
            {
                success = true;
            }
            
            con.Close();

            return success;
        }

        //Get All Users
        public IEnumerable<UserDetails> GetAll(UserDetails userInfo)
        {
            var users = new List<UserDetails>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("Select * from UserTable", con))
                {
                    cmd.Parameters.AddWithValue("@userId", userInfo.Userid);
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var user = new UserDetails();

                                user.Userid = reader.IsDBNull(0) ? null : reader.GetString(0);
                                user.Firstname = reader.IsDBNull(0) ? null : reader.GetString(1);
                                user.Surname = reader.IsDBNull(0) ? null : reader.GetString(2);
                                user.DOB = reader.IsDBNull(0) ? null : reader.GetString(3);
                                user.Gender = reader.IsDBNull(0) ? null : reader.GetString(4);
                                user.Mobile = reader.IsDBNull(0) ? null : reader.GetString(5);
                                user.Email = reader.IsDBNull(0) ? null : reader.GetString(6);
                                user.Workmobile = reader.IsDBNull(0) ? null : reader.GetString(7);

                                users.Add(user);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to read line: " + reader.ToString() + " Error: " + ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }

            return users;
        }

        //Get User
        public UserData Get(UserDetails userInfo)
        {
            using (SqlCommand cmd = new SqlCommand("Select * from UserTable where UserId = @userId", con))
            {
                cmd.Parameters.AddWithValue("@userId", userInfo.Userid);
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        UserData users = new UserData();
                        sda.Fill(users.UsersTable);
                        return users;
                    }
                }
            }
        }

        //Get Address
        public UserAddress GetAddress(UserAddressDetails userInfo)
        {

            using (SqlCommand cmd =
                new SqlCommand(
                    "Select Address_type,Address,City,[Zip/Postal_Code],Province from UserAddressTable where Id = @id",
                    con))
            {
                cmd.Parameters.AddWithValue("@id", userInfo.Id);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        var addresses = new UserAddress();
                        sda.Fill(addresses.AddressTable);

                        return addresses;
                    }
                }
            }
        }

        //Get All User's Addresses
        public IEnumerable<UserAddressDetails> GetUserAddress(UserAddressDetails userInfo)
        {
            var addresses = new List<UserAddressDetails>();

            try
            {
                using (SqlCommand cmd =
                    new SqlCommand(
                        "Select Address_type,Address,City,[Zip/Postal_Code],Province from UserAddressTable where User_ID = @userid",
                        con))
                {
                    cmd.Parameters.AddWithValue("@userid", userInfo.UserId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var address = new UserAddressDetails();
                                address.Id = reader.IsDBNull(0) ? null : reader.GetString(0);
                                address.UserId = reader.IsDBNull(0) ? null : reader.GetString(1);
                                address.Address = reader.IsDBNull(0) ? null : reader.GetString(2);
                                address.AddressType = reader.IsDBNull(0) ? null : reader.GetString(3);
                                address.City = reader.IsDBNull(0) ? null : reader.GetString(4);
                                address.Province = reader.IsDBNull(0) ? null : reader.GetString(5);
                                address.Zipcode_PostalCode = reader.IsDBNull(0) ? null : reader.GetString(6);
                                addresses.Add(address);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to read line: " + reader.ToString() + " Error: " + ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }

            return addresses;
        }

        //Update User
        public bool Update(UserDetails userInfo)
        {
            bool success = false;
            using (SqlCommand cmd = new SqlCommand("UPDATE UserTable SET Firstname = @name, Surname = @surname,Date_of_birth = @dob,Gender = @gender,Mobile = @mobile,Email = @email,Work_mobile =@work WHERE id = @userid", con))
            {
                cmd.Parameters.AddWithValue("@name", userInfo.Firstname);
                cmd.Parameters.AddWithValue("@surname", userInfo.Surname);
                cmd.Parameters.AddWithValue("@dob", userInfo.DOB);
                cmd.Parameters.AddWithValue("@gender", userInfo.Gender);
                cmd.Parameters.AddWithValue("@mobile", userInfo.Mobile);
                cmd.Parameters.AddWithValue("@email", userInfo.Email);
                cmd.Parameters.AddWithValue("@work", userInfo.Workmobile);
                cmd.Parameters.AddWithValue("@userid", userInfo.Userid);

                cmd.Connection = con;
                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    success = true;
                }

                con.Close();
                return success;
            }
        }

        //Update Address 
        public bool UpdateAddress(UserAddressDetails userInfo)
        {
            bool success = false;
            using (SqlCommand cmd = new SqlCommand("UPDATE UserAddressTable SET Firstname = @name, Surname = @surname,Date_of_birth = @dob,Gender = @gender,Mobile = @mobile,Email = @email,Work_mobile =@work WHERE id = @userid", con))
            {
                cmd.Parameters.AddWithValue("@userid", userInfo.UserId);

                cmd.Connection = con;
                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    success = true;
                }

                con.Close();
                return success;
            }
        }

        public bool Delete(string Id)
        {
            bool success = false;
            using (SqlCommand cmd = new SqlCommand("Delete From UserAddressTable Where UserId = @userid", con))
            {
                cmd.Parameters.AddWithValue("@userid", Id);

                cmd.Connection = con;
                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    success = true;
                }

                con.Close();
                return success;
            }
        }

        public bool DeleteAddress(string Id)
        {
            bool success = false;
            using (SqlCommand cmd = new SqlCommand("Delete From UserAddressTable Where Id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", Id);

                cmd.Connection = con;
                con.Open();
                int result = cmd.ExecuteNonQuery();
                
                if (result == 1)
                {
                    success = true;
                }

                con.Close();
                return success;
            }
        }

        public ExportData GetAllExportDetails()
        {

            using (SqlCommand cmd = new SqlCommand("select usrT.Firstname, usrT.Surname,usrT.Gender,usrT.Mobile,usrT.Work_mobile,usrT.Email ,addrT.Address_type,addrT.Address,addrT.City,addrT.Province,addrT.[Zip/Postal_Code] " +
                "from UserTable usrT left join AddressTable addrT on addrT.User_ID = usrT.id; ", con))
            {

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        ExportData exportDt = new ExportData();
                        sda.Fill(exportDt.ExportTable);
                        con.Close();
                        return exportDt;
                    }
                }
            }
        }
    }
}




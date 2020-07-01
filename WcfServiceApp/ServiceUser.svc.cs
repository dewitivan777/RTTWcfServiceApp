using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Web.Configuration;
using DevTrends.WCFDataAnnotations;

namespace WcfServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    [ValidateDataAnnotationsBehavior]
    public class ServiceUser : IServiceUser
    {
        SqlConnection con =
            new SqlConnection(WebConfigurationManager.ConnectionStrings["DBConstring"].ConnectionString);

        public async Task<bool> InsertUserDetails(UserDetails userInfo)
        {
            bool success = false;

            try
            {
                con.Open();
                //Check if email is already in use
                SqlCommand validate = new SqlCommand("select count(*) from UserTable where Email = @email", con);
                validate.Parameters.AddWithValue("@email", userInfo.Email);
                int count = Convert.ToInt32(validate.ExecuteScalar());

                if (count == 0)
                {
                    SqlCommand cmd =
                        new SqlCommand(
                            "insert into UserTable(UserId,Firstname,Surname,DOB,Gender,Mobile,Email,WorkMobile,DateCreated,DateUpdated) values (@userid,@firstname,@surname,@dob,@gender,@mobile,@email,@work_mobile,@datecreated,@dateupdated)",
                            con);

                    cmd.Parameters.AddWithValue("@userid", Guid.NewGuid().ToString("N"));
                    cmd.Parameters.AddWithValue("@firstname", userInfo.Firstname);
                    cmd.Parameters.AddWithValue("@surname", userInfo.Surname);
                    cmd.Parameters.AddWithValue("@dob", userInfo.DOB);
                    cmd.Parameters.AddWithValue("@gender", userInfo.Gender);
                    cmd.Parameters.AddWithValue("@mobile", userInfo.Mobile);
                    cmd.Parameters.AddWithValue("@email", userInfo.Email);
                    cmd.Parameters.AddWithValue("@work_mobile", userInfo.WorkMobile);
                    cmd.Parameters.AddWithValue("@datecreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                    int result = await cmd.ExecuteNonQueryAsync();

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        public async Task<bool> InsertUserAddress(UserAddressDetails userInfo)
        {
            bool success = false;

            try
            {
                con.Open();
                SqlCommand cmd =
                    new SqlCommand(
                        "insert into AddressTable(Id,UserID,AddressType,Address,City,Province,[ZipCode_PostalCode],DateCreated) values(@Id,@userid,@address_type,@address,@city,@province,@zip,@datecreated)",
                        con);

                cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString("N"));
                cmd.Parameters.AddWithValue("@userid", userInfo.UserId);
                cmd.Parameters.AddWithValue("@address_type", userInfo.AddressType);
                cmd.Parameters.AddWithValue("@address", userInfo.Address);
                cmd.Parameters.AddWithValue("@city", userInfo.City);
                cmd.Parameters.AddWithValue("@province", userInfo.Province);
                cmd.Parameters.AddWithValue("@zip", userInfo.Zipcode_PostalCode);
                cmd.Parameters.AddWithValue("@datecreated", DateTime.Now);
                cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                int result = await cmd.ExecuteNonQueryAsync();

                if (result == 1)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        //Get All Users
        public async Task<IEnumerable<UserDetails>> GetAll(UserSearchQuery query)
        {
            var users = new List<UserDetails>();
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from UserTable Order By DateUpdated desc OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY", con))
                {
                    cmd.Parameters.AddWithValue("@offset", query.Offset);
                    cmd.Parameters.AddWithValue("@limit", query.Limit);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
                                user.DOB = reader.IsDBNull(0) ? DateTime.MinValue : reader.GetDateTime(3);
                                user.Gender = reader.IsDBNull(0) ? null : reader.GetString(4);
                                user.Mobile = reader.IsDBNull(0) ? null : reader.GetString(5);
                                user.Email = reader.IsDBNull(0) ? null : reader.GetString(6);
                                user.WorkMobile = reader.IsDBNull(0) ? null : reader.GetString(7);
                                user.DateCreated = reader.IsDBNull(0) ? DateTime.MinValue : reader.GetDateTime(8);
                                user.DateUpdated = reader.IsDBNull(0) ? DateTime.MinValue : reader.GetDateTime(9);

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
            finally
            {
                con.Close();
            }

            return users;
        }

        //Get User
        public async Task<UserData> Get(UserDetails userInfo)
        {
            var users = new UserData();
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from UserTable where UserId = @userId", con))
                {
                    cmd.Parameters.AddWithValue("@userId", userInfo.Userid);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(users.UsersTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return users;
        }

        //Get Address
        public async Task<UserAddress> GetAddress(UserAddressDetails userInfo)
        {
            var addresses = new UserAddress();
            try
            {
                con.Open();
                using (SqlCommand cmd =
                    new SqlCommand(
                        "Select AddressType,Address,City,[ZipCode_Postal_Code],Province from UserAddressTable where Id = @id",
                        con))
                {
                    cmd.Parameters.AddWithValue("@id", userInfo.Id);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {

                            sda.Fill(addresses.AddressTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return addresses;
        }

        //Get All User's Addresses
        public async Task<IEnumerable<UserAddressDetails>> GetUserAddress(UserAddressDetails userInfo)
        {
            var addresses = new List<UserAddressDetails>();

            try
            {
                using (SqlCommand cmd =
                    new SqlCommand(
                        "Select AddressType,Address,City,[ZipCode_Postal_Code],Province from UserAddressTable where User_ID = @userid",
                        con))
                {
                    cmd.Parameters.AddWithValue("@userid", userInfo.UserId);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
                                address.Zipcode_PostalCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(6);
                                address.DateCreated = reader.IsDBNull(0) ? DateTime.MinValue : reader.GetDateTime(7);
                                address.DateUpdated = reader.IsDBNull(0) ? DateTime.MinValue : reader.GetDateTime(8);

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
            finally
            {
                con.Close();
            }

            return addresses;
        }

        //Update User
        public async Task<bool> Update(UserDetails userInfo)
        {
            bool success = false;

            try
            {
                con.Open();
                using (SqlCommand cmd =
                    new SqlCommand(
                        "UPDATE UserTable SET Firstname = @name, Surname = @surname, DOB = @dob, Gender = @gender, Mobile = @mobile, Email = @email, WorkMobile = @work, DateUpdated = @dateupdated WHERE UserId = @userid",
                        con))
                {
                    cmd.Parameters.AddWithValue("@name", userInfo.Firstname);
                    cmd.Parameters.AddWithValue("@surname", userInfo.Surname);
                    cmd.Parameters.AddWithValue("@dob", userInfo.DOB);
                    cmd.Parameters.AddWithValue("@gender", userInfo.Gender);
                    cmd.Parameters.AddWithValue("@mobile", userInfo.Mobile);
                    cmd.Parameters.AddWithValue("@email", userInfo.Email);
                    cmd.Parameters.AddWithValue("@work", userInfo.WorkMobile);
                    cmd.Parameters.AddWithValue("@userid", userInfo.Userid);
                    cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                    cmd.Connection = con;
                    con.Open();
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        //Update Address 
        public async Task<bool> UpdateAddress(UserAddressDetails userInfo)
        {
            bool success = false;

            try
            {
                con.Open();
                using (SqlCommand cmd =
                    new SqlCommand(
                        "UPDATE UserAddressTable SET Address = @address, AddressType = @addresstype, City = @city, Province = @province, ZipCode_PostalCode = @zip, DateUpdated = @dateupdated WHERE Id = @id",
                        con))
                {
                    cmd.Parameters.AddWithValue("@id", userInfo.Id);
                    cmd.Parameters.AddWithValue("@address", userInfo.Address);
                    cmd.Parameters.AddWithValue("@addresstype", userInfo.AddressType);
                    cmd.Parameters.AddWithValue("@city", userInfo.Id);
                    cmd.Parameters.AddWithValue("@province", userInfo.Id);
                    cmd.Parameters.AddWithValue("@zip", userInfo.Id);
                    cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                    cmd.Connection = con;
                    con.Open();
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        public async Task<bool> Delete(string Id)
        {
            bool success = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand("Delete From UserAddressTable Where UserId = @userid", con))
                {
                    cmd.Parameters.AddWithValue("@userid", Id);

                    cmd.Connection = con;
                    con.Open();
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        public async Task<bool> DeleteAddress(string Id)
        {
            bool success = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand("Delete From UserAddressTable Where Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", Id);

                    cmd.Connection = con;
                    con.Open();
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        success = true;
                    }

                    con.Close();
                    return success;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to make connection: " + ex);
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        public async Task<ExportData> GetAllExportDetails()
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




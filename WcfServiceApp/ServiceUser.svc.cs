using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Web.Configuration;
using DevTrends.WCFDataAnnotations;
using WcfServiceApp.Extentions;

namespace WcfServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    [ValidateDataAnnotationsBehavior]
    public class ServiceUser : IServiceUser
    {
        readonly SqlConnection con =
            new SqlConnection(WebConfigurationManager.ConnectionStrings["DBConstring"].ConnectionString);

        //Insert User
        public async Task<ServerResponse> InsertUserDetails(UserDetails userInfo)
        {
            var response = new ServerResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                //Check if email is already in use
                SqlCommand validate = new SqlCommand("select count(*) from UserTable where Email = @email", con);
                validate.Parameters.AddWithValue("@email", userInfo.Email);
                int count = Convert.ToInt32(validate.ExecuteScalar());

                if (count == 0)
                {
                    using (var cmd =
                        new SqlCommand(
                            "insert into UserTable(UserId,Firstname,Surname,DOB,Gender,Mobile,Email,WorkMobile,DateCreated,DateUpdated) values (@userid,@firstname,@surname,@dob,@gender,@mobile,@email,@work_mobile,@datecreated,@dateupdated)",
                            con))
                    {
                        cmd.Parameters.AddWithValue("@userid", Guid.NewGuid().ToString("N"));
                        cmd.Parameters.AddWithValue("@firstname", userInfo.FirstName);
                        cmd.Parameters.AddWithValue("@surname", userInfo.Surname);
                        cmd.Parameters.AddWithValue("@dob", SQLExtentions.IsValidSqlDateTime(userInfo.DOB) ? userInfo.DOB : (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue);
                        cmd.Parameters.AddWithValue("@gender", userInfo.Gender);
                        cmd.Parameters.AddWithValue("@mobile", userInfo.Mobile);
                        cmd.Parameters.AddWithValue("@email", userInfo.Email);
                        cmd.Parameters.AddWithValue("@work_mobile", userInfo.WorkMobile);
                        cmd.Parameters.AddWithValue("@datecreated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                        int result = await cmd.ExecuteNonQueryAsync();

                        if (result == 1)
                        {
                            response.Success = true;
                        }
                    }
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "Email already in Use";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Insert Address
        public async Task<ServerResponse> InsertUserAddress(UserAddressDetails userInfo)
        {
            var response = new ServerResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (var cmd = new SqlCommand(
                    "insert into UserAddressTable(Id,UserID,AddressType,Address,City,Province,ZipCode_PostalCode,DateCreated,DateUpdated) values(@Id,@userid,@address_type,@address,@city,@province,@zip,@datecreated,@dateupdated)",
                    con))
                {
                    cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString("N"));
                    cmd.Parameters.AddWithValue("@userid", userInfo.UserId);
                    cmd.Parameters.AddWithValue("@address_type", userInfo.AddressType);
                    cmd.Parameters.AddWithValue("@address", userInfo.Address);
                    cmd.Parameters.AddWithValue("@city", userInfo.City);
                    cmd.Parameters.AddWithValue("@province", userInfo.Province);
                    cmd.Parameters.AddWithValue("@zip", userInfo.ZipCode_PostalCode);
                    cmd.Parameters.AddWithValue("@datecreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Get All Users
        public async Task<ServerResponseUser> GetAll(UserSearchQuery query)
        {
            var response = new ServerResponseUser();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string cmdText = query.UserQuery != null ? $"Select * from UserTable {(query.UserQuery.HasCriteria() ? query.UserQuery.QueryString() : "")}" : $"Select * from UserTable";

                //Get total
                SqlCommand validate = new SqlCommand(cmdText.Replace("*", "count(*)"), con);
                int total = Convert.ToInt32(validate.ExecuteScalar());

                cmdText += " Order By DateUpdated desc OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

                using (var cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@offset", query.Offset);
                    cmd.Parameters.AddWithValue("@limit", query.Limit);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        var users = new List<UserDetails>();
                        while (reader.Read())
                        {
                            try
                            {
                                var user = new UserDetails();

                                user.UserId = reader.IsDBNull(0) ? null : reader.GetString(0);
                                user.FirstName = reader.IsDBNull(1) ? null : reader.GetString(1);
                                user.Surname = reader.IsDBNull(2) ? null : reader.GetString(2);
                                user.DOB = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                                user.Gender = reader.IsDBNull(4) ? null : reader.GetString(4);
                                user.Mobile = reader.IsDBNull(5) ? null : reader.GetString(5);
                                user.Email = reader.IsDBNull(6) ? null : reader.GetString(6);
                                user.WorkMobile = reader.IsDBNull(7) ? null : reader.GetString(7);
                                user.DateCreated = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);
                                user.DateUpdated = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9);

                                users.Add(user);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to read line: " + reader.ToString() + " Error: " + ex);
                            }
                        }

                        response.Success = true;
                        response.Total = total;
                        response.Users = users;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Get User
        public async Task<UserData> Get(UserDetails userInfo)
        {
            var users = new UserData();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (var cmd = new SqlCommand("Select * from UserTable where UserId = @userId", con))
                {
                    cmd.Parameters.AddWithValue("@userId", userInfo.UserId);
                    using (var sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (var dt = new DataTable())
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
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (var cmd =
                    new SqlCommand(
                        "Select AddressType,Address,City,[ZipCode_Postal_Code],Province from UserAddressTable where Id = @id",
                        con))
                {
                    cmd.Parameters.AddWithValue("@id", userInfo.Id);

                    using (var sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (var dt = new DataTable())
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
        public async Task<ServerResponseAddress> GetAllAddress(AddressSearchQuery query)
        {
            var response = new ServerResponseAddress();
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string cmdText = query.AddressQuery != null ? $"Select * from UserAddressTable {(query.AddressQuery.HasCriteria() ? query.AddressQuery.QueryString() : "")}" : $"Select * from UserAddressTable";

                //Get total
                SqlCommand validate = new SqlCommand(cmdText.Replace("*", "count(*)"), con);
                int total = Convert.ToInt32(validate.ExecuteScalar());

                cmdText += " Order By DateUpdated desc OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

                using (SqlCommand cmd =
                    new SqlCommand(
                        cmdText,
                        con))
                {
                    cmd.Parameters.AddWithValue("@offset", query.Offset);
                    cmd.Parameters.AddWithValue("@limit", query.Limit);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        var addresses = new List<UserAddressDetails>();
                        while (reader.Read())
                        {
                            try
                            {
                                var address = new UserAddressDetails();
                                address.Id = reader.IsDBNull(0) ? null : reader.GetString(0);
                                address.UserId = reader.IsDBNull(1) ? null : reader.GetString(1);
                                address.Address = reader.IsDBNull(2) ? null : reader.GetString(2);
                                address.AddressType = reader.IsDBNull(3) ? null : reader.GetString(3);
                                address.City = reader.IsDBNull(4) ? null : reader.GetString(4);
                                address.Province = reader.IsDBNull(5) ? null : reader.GetString(5);
                                address.ZipCode_PostalCode = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                                address.DateCreated = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);
                                address.DateUpdated = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);

                                addresses.Add(address);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Failed to read line: " + reader.ToString() + " Error: " + ex);
                            }
                        }
                        response.Success = true;
                        response.Total = total;
                        response.Addresses = addresses;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Update User
        public async Task<ServerResponse> Update(UserDetails userInfo)
        {
            var response = new ServerResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (var cmd =
                    new SqlCommand(
                        "UPDATE UserTable SET Firstname = @name, Surname = @surname, DOB = @dob, Gender = @gender, Mobile = @mobile, Email = @email, WorkMobile = @work, DateUpdated = @dateupdated WHERE UserId = @userid",
                        con))
                {
                    cmd.Parameters.AddWithValue("@name", userInfo.FirstName);
                    cmd.Parameters.AddWithValue("@surname", userInfo.Surname);
                    cmd.Parameters.AddWithValue("@dob", SQLExtentions.IsValidSqlDateTime(userInfo.DOB) ? userInfo.DOB : (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue);
                    cmd.Parameters.AddWithValue("@gender", userInfo.Gender);
                    cmd.Parameters.AddWithValue("@mobile", userInfo.Mobile);
                    cmd.Parameters.AddWithValue("@email", userInfo.Email);
                    cmd.Parameters.AddWithValue("@work", userInfo.WorkMobile);
                    cmd.Parameters.AddWithValue("@userid", userInfo.UserId);
                    cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                    cmd.Connection = con;
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Update Address 
        public async Task<ServerResponse> UpdateAddress(UserAddressDetails addressInfo)
        {
            var response = new ServerResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (var cmd =
                    new SqlCommand(
                        "UPDATE UserAddressTable SET Address = @address, AddressType = @addresstype, City = @city, Province = @province, ZipCode_PostalCode = @zip, DateUpdated = @dateupdated WHERE Id = @id",
                        con))
                {
                    cmd.Parameters.AddWithValue("@id", addressInfo.Id);
                    cmd.Parameters.AddWithValue("@address", addressInfo.Address);
                    cmd.Parameters.AddWithValue("@addresstype", addressInfo.AddressType);
                    cmd.Parameters.AddWithValue("@city", addressInfo.City);
                    cmd.Parameters.AddWithValue("@province", addressInfo.Province);
                    cmd.Parameters.AddWithValue("@zip", addressInfo.ZipCode_PostalCode);
                    cmd.Parameters.AddWithValue("@dateupdated", DateTime.Now);

                    cmd.Connection = con;
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Delete User
        public async Task<ServerResponse> Delete(string Id)
        {
            var response = new ServerResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (var cmd = new SqlCommand("Delete From UserTable Where UserId = @userid", con))
                {
                    cmd.Parameters.AddWithValue("@userid", Id);

                    cmd.Connection = con;
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        //Delete Addresses' Belonging to User As well
                        using (var cmdAddress = new SqlCommand("Delete From UserAddressTable Where UserId = @userid", con))
                        {
                            cmdAddress.Parameters.AddWithValue("@userid", Id);

                            cmdAddress.Connection = con;
                            int resultAddress = await cmdAddress.ExecuteNonQueryAsync();

                            if (resultAddress == 1)
                            {
                                response.Success = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Delete Address
        public async Task<ServerResponse> DeleteAddress(string Id)
        {
            var response = new ServerResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                using (var cmd = new SqlCommand("Delete From UserAddressTable Where Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", Id);

                    cmd.Connection = con;
                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result == 1)
                    {
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Failed to make connection: " + ex;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        //Export User Data
        public async Task<ExportData> GetAllExportDetails()
        {
            ExportData exportDt = new ExportData();
            try
            {
                using (var cmd = new SqlCommand("select usrT.UserId, usrT.Firstname, usrT.Surname,usrT.Gender,usrT.Email ,addrT.AddressType,addrT.Address,addrT.City,addrT.Province,addrT.ZipCode_PostalCode " + "from UserTable usrT left join UserAddressTable addrT on addrT.UserId = usrT.UserId Order By usrT.UserId;", con))
                {
                    using (var sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (var dt = new DataTable())
                        {
                            sda.Fill(exportDt.ExportTable);
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return exportDt;
        }
    }
}

using System;
using System.Data.SqlClient;
using System.Dynamic;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class UserRepository
    {
        private readonly SqlConnection _sqlConnection;
        public UserRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        
        public string FindUserEmailByUserName(string userName)
        {
            try
            {
                string query = "SELECT email " +
                               "FROM [user] " +
                               "WHERE user_name = @user_name" ;

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@user_name", userName);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }

            return null;
        }
        
        
        public int FindIdByEmail(string email)
        {
            try
            {
                string query = "SELECT id " +
                               "FROM [user] " +
                               "WHERE email = @email" ;

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("email", email);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            finally
            {
                _sqlConnection.Close();
            }

            return -1;
        }
        
        
        public bool UpdatePasswordById(int id,string password)
        {
            try
            {
                string query = "UPDATE [user] " +
                               "SET password = @password " +
                               "WHERE id = @id" ;

                byte[] hashvalue = Hashing.CalculateSHA256(password);
                string hashpass = "";
                foreach (byte item in hashvalue)
                {
                    hashpass += item;
                }
                
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@password", hashpass);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }

        }


        public User GetUserById(int id)
        {
            try
            {
                Console.WriteLine(id);
                User user = new User();
                string query = "SELECT * " +
                               "FROM [user] " +
                               "WHERE id = @id" ;

                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.Add("@id", id);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.Id = (int)reader["id"];
                            user.FullName = (string)reader["full_name"];
                            user.DateOfBirth = (DateTime)reader["date_of_birth"];
                            user.DateOfBirth = user.DateOfBirth.Date;
                            user.Avatar = reader["avatar"] != DBNull.Value ? Util.BitmapConverter.ToImage((byte[])reader["avatar"]) : null;
                            user.Email = (string)reader["email"];
                            user.PhoneNumber = (string)reader["phone_number"];
                            return user;
                        }
                        return null;

                    }
                }
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void UpdateAvatar(int id, BitmapImage bitmap)
        {
            try
            {
                string query = "UPDATE [user] " +
                               "SET avatar = @avatar " +
                               "WHERE id = @id" ;
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    Byte[] binary =  Util.BitmapConverter.ConvertToByteFromBitmapImage(bitmap);
                    command.Parameters.AddWithValue("@avatar", binary);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _sqlConnection.Close();
            }
            
        }

        public void UpdateUserInfo(int id, string fullname,
            string email, string s, DateTime dateofbirth)
        {
            try
            {
                string query = "UPDATE [user] " +
                               "SET full_name = @full_name, " +
                               "email = @email, "+
                               "phone_number = @phone_number, "+
                               "date_of_birth = @date_of_birth "+
                               "WHERE id = @id" ;
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@full_name", fullname);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone_number", s);
                    command.Parameters.AddWithValue("@date_of_birth", dateofbirth);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}
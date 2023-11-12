using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories
{
    public class DbConnection
    {
        private readonly SqlConnection _sqlConnection;

        public DbConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "LAPTOP-JRRAO9EA\\MSSQLSERVERR";
            builder.UserID = "sa";
            builder.Password = "123456";
            builder.InitialCatalog = "APP_HOC_AV";
            _sqlConnection = new SqlConnection(builder.ConnectionString);
        }

        public int Authentication(string user_name,string password)
        {
            try
            {
                string passwordResult;
                int userId = 0;
                var hashbytes = Hashing.CalculateSHA256(password);
                string hashpass = "";
                foreach (byte item in hashbytes)
                {
                    hashpass += item;
                }

                string query = "select id, user_name, password " +
                               "from [User] " +
                               "where user_name = @un";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@un", user_name);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            passwordResult = reader.GetString(2);
                            if (password == passwordResult)
                            {
                                userId = reader.GetInt32(0);
                                return userId;
                            }
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public int NewUser(User user)
        {
            try
            {
                string query = "INSERT INTO [User] (full_name," +
                               " date_of_birth, email," +
                               " phone_number, gender," +
                               " user_name, password," +
                               " user_level, status, " +
                               "last_login, created_at, updated_at)" +
                               " output inserted.id " +
                               "VALUES " +
                               "(@full_name,@date_of_birth,@email," +
                               " @phone_number,@gender,@user_name,@pwd, " +
                               "1,0,null, GETDATE(), GETDATE())";

                byte[] hashvalue = Hashing.CalculateSHA256(user.Password);
                string hashpass = "";
                foreach (byte item in hashvalue)
                {
                    hashpass += item;
                }
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@full_name", user.FullName);
                    command.Parameters.AddWithValue("@date_of_birth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@phone_number", user.PhoneNumber);
                    command.Parameters.AddWithValue("@gender", user.Gender);
                    command.Parameters.AddWithValue("@user_name", user.UserName);
                    command.Parameters.AddWithValue("@pwd", hashpass);
                    _sqlConnection.Open();
                    int modified = (int)command.ExecuteScalar();
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                        _sqlConnection.Close();
                    return modified;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}
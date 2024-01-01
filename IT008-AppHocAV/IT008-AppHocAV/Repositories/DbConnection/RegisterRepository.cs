using System;
using System.Data.SqlClient;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class RegisterRepository
    {
        private readonly SqlConnection _sqlConnection;

        public RegisterRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        
        
         /// <summary>
        /// Creates an new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
using System;
using System.Data.SqlClient;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class UserQ
    {
        private readonly SqlConnection _sqlConnection;
        public UserQ(SqlConnection sqlConnection)
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
    }
}
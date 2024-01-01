using System;
using System.Data.SqlClient;
using System.Windows;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class Authentication
    {
        private readonly SqlConnection _sqlConnection;

        public Authentication(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        /// <summary>
        /// Authentication an user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Login(string userName, string password)
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
                    command.Parameters.AddWithValue("@un", userName);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            passwordResult = reader.GetString(2);
                            if (hashpass == passwordResult)
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
    }
}
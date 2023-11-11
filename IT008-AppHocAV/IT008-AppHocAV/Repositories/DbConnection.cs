using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;
using IT008_AppHocAV.View.MainWindow;

namespace IT008_AppHocAV.Repositories
{
    public class DbConnection
    {
        private readonly SqlConnection _sqlConnection;

        public DbConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-38JM1H0";
            builder.UserID = "sa";
            builder.Password = "123456";
            builder.InitialCatalog = "APP_HOC_AV";
            _sqlConnection = new SqlConnection(builder.ConnectionString);
        }

        public int Authentication(string userName, string password)
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


        public int CreateEssay(Essay essay)
        {
            try
            {
                string query =
                    " INSERT INTO Essay (user_id, title, topic,image, description, content, updated_at, created_at) " +
                    " output inserted.id " +
                    " VALUES " +
                    " (@user_id, @title, @topic , @image , @description, @content, GETDATE(), GETDATE()) ";

                byte[] data = essay.Image == null ? null : ConvertToByteFromBitmapImage(essay.Image);


                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@user_id", essay.UserId);
                    command.Parameters.AddWithValue("@title", essay.Title);
                    command.Parameters.AddWithValue("@topic", essay.Topic);

                    if (data != null)
                        command.Parameters.AddWithValue("@image", data);
                    else
                        command.Parameters.AddWithValue("@image", SqlBinary.Null);


                    command.Parameters.AddWithValue("@description", essay.Description);
                    command.Parameters.AddWithValue("@content", essay.Content);
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

        public void UpdateEssayContent(int id, string content)
        {
            try
            {
                string query = "UPDATE essay " +
                               "SET content = @content, updated_at = GETDATE() " +
                               "WHERE id = @id ";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@content", content);
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


        public List<Essay> SelectListEssayByUserId(int userId)
        {
            List<Essay> result = new List<Essay>();
            try
            {
                string query = " SELECT id, title, topic, updated_at, created_at" +
                               " FROM [essay] " +
                               " WHERE user_id = @UserId";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Essay essay = new Essay(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                reader.GetString(reader.GetOrdinal("topic")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            result.Add(essay);
                        }
                    }

                }
                return result;
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

        
        
        public Essay SelectEssayById(int id)
        {
            try
            {

                string query = " SELECT *" +
                               " FROM [essay] " +
                               " WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    _sqlConnection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            BitmapImage image = null;

                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                long len = reader.GetBytes(reader.GetOrdinal("image"), 0, null, 0, 0);

                                Byte[] buffer = new byte[len];


                                reader.GetBytes(reader.GetOrdinal("image"), 0, buffer, 0, (int)len);

                                image = ToImage(buffer);

                            }

                            Essay essay = new Essay(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                reader.GetString(reader.GetOrdinal("topic")),
                                reader.GetString(reader.GetOrdinal("description")),
                                image,
                                reader.GetString(reader.GetOrdinal("content")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );

                            return essay;
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

        public bool DeleteEssayById(int id)
        {
            try
            {
                string query = "DELETE [essay] WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                    return true;
                }
                
            }
            catch (Exception e )
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }

        }
        
        private Byte[] ConvertToByteFromBitmapImage(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using(MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }
        
        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class EssayQ
    {
        private readonly SqlConnection _sqlConnection;

        public EssayQ(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        
        
        
         /// <summary>
        /// Creates a new Essay
        /// </summary>
        /// <param name="essay"></param>
        /// <returns></returns>
        public int CreateEssay(Models.Essay essay)
        {
            try
            {
                string query =
                    " INSERT INTO Essay (user_id, title, topic,image, content, updated_at, created_at) " +
                    " output inserted.id " +
                    " VALUES " +
                    " (@user_id, @title, @topic , @image, @content, GETDATE(), GETDATE()) ";

                byte[] data = essay.Image == null ? null : BitmapConverter.ConvertToByteFromBitmapImage(essay.Image);

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@user_id", essay.UserId);
                    command.Parameters.AddWithValue("@title", essay.Title);
                    command.Parameters.AddWithValue("@topic", essay.Topic);

                    if (data != null)
                        command.Parameters.AddWithValue("@image", data);
                    else
                        command.Parameters.AddWithValue("@image", SqlBinary.Null);


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

        /// <summary>
        /// Updates essay content
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wordcount"></param>
        /// <param name="content"></param>
        public void UpdateEssayContent(int id,int wordcount ,string content )
        {
            try
            {
                string query = "UPDATE essay " +
                               "SET content = @content, words = @wordcount, updated_at = GETDATE() " +
                               "WHERE id = @id ";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@content", content);
                    command.Parameters.AddWithValue("@wordcount", wordcount);
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


        /// <summary>
        /// Gets all Essays of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Models.Essay> SelectListEssayByUserId(int userId)
        {
            List<Models.Essay> result = new List<Models.Essay>();
            try
            {
                string query = " SELECT id, title, topic, updated_at, created_at, words" +
                               " FROM [essay] " +
                               " WHERE user_id = @UserId ORDER BY updated_at desc";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.Essay essay = new Models.Essay(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                reader.GetString(reader.GetOrdinal("topic")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                reader.GetInt32(reader.GetOrdinal("words"))
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

        /// <summary>
        /// Gets all data of a essay
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

                                image =  BitmapConverter.ToImage(buffer);
                            }

                            Models.Essay essay = new Models.Essay(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                reader.GetString(reader.GetOrdinal("topic")),
                                image,
                                reader.GetString(reader.GetOrdinal("content")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                reader.GetInt32(reader.GetOrdinal("words"))
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

        /// <summary>
        /// delete a essay by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        
        /// <summary>
        /// Update title and topic by essay id 
        /// </summary>
        /// <param name="essay"></param>
        public void UpdateTitleAndTopic(Essay essay)
        {
            try
            {
                string query = "UPDATE ESSAY " +
                               "SET title = @title, topic = @topic " +
                               "WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", essay.Id);
                    command.Parameters.AddWithValue("@title", essay.Title);
                    command.Parameters.AddWithValue("@topic", essay.Topic);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                    Console.WriteLine(essay.Topic);
                }
                
            }
            catch (Exception e )
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
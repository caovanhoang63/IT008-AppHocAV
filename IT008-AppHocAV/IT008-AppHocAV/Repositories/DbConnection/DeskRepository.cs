using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;
using PexelsDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class DeskRepository
    {

        private readonly SqlConnection _sqlConnection;

        public DeskRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public int CreateDesk(Models.ListFlashCard desk)
        {
            try
            {
                string query =
                    " INSERT INTO desk (user_id ,title, description, quantity, updated_at, created_at ) " +
                    " output inserted.id " +
                    " VALUES " +
                    " (@user_id, @title, @description , @quantity , GETDATE(), GETDATE()) ";
            
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@user_id", desk.UserId);
                    command.Parameters.AddWithValue("@title", desk.Title);
                    command.Parameters.AddWithValue("@description", desk.Description);
                    command.Parameters.AddWithValue("@quantity", desk.Quantity);

                     _sqlConnection.Open();
                    int modified = (int)command.ExecuteScalar();
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                        _sqlConnection.Close();
                    return modified;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e);
                return 0;
            }

            finally
            {
                _sqlConnection.Close();
            }
        }


        public void InsertCards(Models.FlashCard card)
        {
            try
            {
           
                string query = "INSERT INTO [card] (desk_id, question,answer, image, created_at ,updated_at ) " +
                    " VALUES "+
                    " (@desk_id, @question, @answer, @image, GETDATE(), GETDATE())";
             
                byte[] data = card.Image == null ? null : BitmapConverter.ConvertToByteFromBitmapImage(card.Image);
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@desk_id", card.Id);
                    command.Parameters.AddWithValue("@question", card.Question);
                    command.Parameters.AddWithValue("@answer", card.Answer);
                    if (data != null)
                        command.Parameters.AddWithValue("@image", data);
                    else
                        command.Parameters.AddWithValue("@image", SqlBinary.Null);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                        _sqlConnection.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void UpdateDeskContent(int id ,int quantity, string title, string description)
        {
            try
            {
                string query = "UPDATE desk"+
                                " SET title = @title , description = @description , quantity = @quantity , updated_at= GETDATE()"+
                                "WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@description",description);
                    command.Parameters.AddWithValue("@quantity",quantity);
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
        public ListFlashCard SelectDeskById ( int id)
        {
            try
            {
                string query = " SELECT *" +
                               " FROM [desk] " +
                               " WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    _sqlConnection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            Models.ListFlashCard desk = new Models.ListFlashCard(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetInt32(reader.GetOrdinal("quantity")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at")));
                            return desk;
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
        public List<Models.ListFlashCard> SelectListDeskByUserID( int userID)
        {
            
            List<Models.ListFlashCard> result = new List<Models.ListFlashCard>();
            try
            {
                string query = " SELECT  id , title , description, quantity, updated_at, created_at" +
                               " FROM [desk] " +
                               " WHERE user_id = @UserId ORDER BY updated_at desc";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.ListFlashCard desk = new Models.ListFlashCard(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("title")),
                                  reader.GetString(reader.GetOrdinal("description")),
                                  reader.GetInt32(reader.GetOrdinal("quantity")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at")),
                               reader.GetDateTime(reader.GetOrdinal("created_at"))

                            );
                            result.Add(desk);
                        }
                    }

                }
                if ( result.Count > 0)
                {
                    
                }    
                return result;
            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public bool DeleteDeskById(int id)
        {
            try
            {
                string query = "DELETE [desk] WHERE id = @id";
                string querycard = "DELETE [card] WHERE desk_id = @id";
                using (SqlCommand command = new SqlCommand(querycard, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                    _sqlConnection.Close();
                }
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    _sqlConnection.Open();
                    command.ExecuteScalar();
                     return true;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}

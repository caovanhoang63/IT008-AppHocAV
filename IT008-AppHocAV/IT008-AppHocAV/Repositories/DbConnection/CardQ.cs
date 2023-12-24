using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class CardQ
    {
        private readonly SqlConnection _sqlConnection;

        public CardQ(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public int SelectTopId( )
        {
            int result = 0; 
            try
            {
                string query = "SELECT TOP 1 id  From[card]  order by id DESC ";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {

                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            result = reader.GetInt32(0);
                        }    
                        

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                _sqlConnection.Close();
            }

        }
        public bool CreateCard(Models.FlashCard card)
        {
            try
            {
                string query = "INSERT INTO [card] ( desk_id , question, answer, image, created_at, updated_at ) " +
                                "OUTPUT inserted.id " +
                                "VALUES "+
                                "(@desk_id, @question, @answer, @image, GETDATE(), GETDATE())";
 

                byte[] data = card.Image ==null ? null : BitmapConverter.ConvertToByteFromBitmapImage(card.Image);
                
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                   
                    command.Parameters.AddWithValue("@desk_id", card.DeskId);
                   
                    command.Parameters.AddWithValue("@question", card.Question);

                    command.Parameters.AddWithValue("@answer", card.Answer);
                    if (data != null)
                        command.Parameters.AddWithValue("@image", data);
                    else
                        command.Parameters.AddWithValue("@image", SqlBinary.Null);
                  _sqlConnection.Open();
                   
                    int modified = (int)command.ExecuteScalar();
 
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                        _sqlConnection.Close();
                    return true;

                  
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void UpdateCardContent(int id, string term, string define,BitmapImage image)
        {
            try
            {
                 
                string query = " UPDATE card "+
                                " SET question = @term , answer = @define ,image =@image,updated_at = GETDATE()"+
                                "WHERE id = @id";
                byte[] data = image == null ? null : BitmapConverter.ConvertToByteFromBitmapImage(image);
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@term",term);
                    command.Parameters.AddWithValue("@define",define);
                    if (data != null)
                        command.Parameters.AddWithValue("@image", data);
                    else
                        command.Parameters.AddWithValue("@image", SqlBinary.Null);

                    _sqlConnection.Open();
                    command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public List<Models.FlashCard> SelectCardByID(int id)
        {
            List<Models.FlashCard> result = new List<Models.FlashCard>();
            try
            {
                string query = "SELECT *"+
                                "FROM [card]"+
                                "WHERE desk_id = @id";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            BitmapImage image = null;

                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                long len = reader.GetBytes(reader.GetOrdinal("image"), 0, null, 0, 0);

                                Byte[] buffer = new byte[len];


                                reader.GetBytes(reader.GetOrdinal("image"), 0, buffer, 0, (int)len);

                                image =  BitmapConverter.ToImage(buffer);
                            }
                         
                             Models.FlashCard card = new Models.FlashCard(
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("desk_id")),
                                    reader.GetString(reader.GetOrdinal("question")),
                                    reader.GetString(reader.GetOrdinal("answer")),

                                    image,

                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at")));

                                result.Add(card);
                          
                          
                         

                            
                        }
                      
                        return result;
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

        public bool DeleteCardById(int id)
        {
            try
            {
                string query = "DELETE [card] WHERE id = @id";

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

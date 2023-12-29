using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Policy;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class RecallRepository
    {
        private SqlConnection _sqlConnection;
        public RecallRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public int AddNewWord(int userID, string word)
        {
            try
            {
                string query = "INSERT INTO VocabularyRecallLog(User_Id, Word, Created_At, Updated_At) " +
                               "output inserted.id " +
                               "VALUES(@UserId, @Word, GETDATE(), GETDATE())";

                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.Add("@UserId", userID);
                    command.Parameters.Add("@Word", word);
                    _sqlConnection.Open();
                    int id = (int)command.ExecuteScalar();
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                        _sqlConnection.Close();
                    return id;
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
        }
        
        public int  AddRecallLog(VocabularyRecallLog vocabularyRecallLog)
        {
            try
            {
                string query = "INSERT INTO VocabularyRecallLog(User_Id, Word, Meaning, Created_At, Updated_At) " +
                               "output inserted.id " +
                               "VALUES(@UserId, @Word, @Meaning, GETDATE(), GETDATE())";

                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.Add("@UserId", vocabularyRecallLog.UserId);
                    command.Parameters.Add("@Word", vocabularyRecallLog.Word);
                    command.Parameters.Add("@Meaning", vocabularyRecallLog.Meaning);
                    _sqlConnection.Open();
                    int id = (int)command.ExecuteScalar();
                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                        _sqlConnection.Close();
                    return id;
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
        }

        public List<VocabularyRecallLog> GetAllRecallLogsByDateAndUserId(int UserID, DateTime dateTime)
        {
            try
            {
                string query = "SELECT * " +
                               "FROM VocabularyRecallLog " +
                               "WHERE User_Id = @UserId AND Year(Created_At) = Year(@Created_At) " +
                               "And Month(Created_At) = Month(@Created_At) And Day(Created_At) = Day(@Created_At)";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@UserId", UserID);
                    command.Parameters.AddWithValue("@Created_At", dateTime);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<VocabularyRecallLog> vocabularyRecallLogs = new List<VocabularyRecallLog>();
                        while (reader.Read())
                        {
                            VocabularyRecallLog vocabularyRecallLog = new VocabularyRecallLog();
                            vocabularyRecallLog.Id = reader.GetInt32(0);
                            vocabularyRecallLog.UserId = reader.GetInt32(1);
                            vocabularyRecallLog.Word = reader.GetString(2);
                            vocabularyRecallLog.Meaning = reader.GetString(3);
                            vocabularyRecallLog.IsSuccessful = reader.GetBoolean(reader.GetOrdinal("Is_Successful"));
                            vocabularyRecallLog.Example = reader.GetString(5);
                            vocabularyRecallLog.CreatedAt = reader.GetDateTime(6);
                            vocabularyRecallLog.UpdatedAt = reader.GetDateTime(7);
                            vocabularyRecallLogs.Add(vocabularyRecallLog);
                        }

                        return vocabularyRecallLogs;
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

        public int IsRecalled(int userId, string word)
        {
            try
            {
                string query = "SELECT id  " +
                               "FROM VocabularyRecallLog " +
                               "WHERE User_Id = @UserId AND Word = @Word";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@UserId", userId);
                    command.Parameters.Add("@Word", word);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        {
                            return -1;
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
        }

        public bool DeleteById(int id)
        {
            try
            {
                string query = "DELETE FROM VocabularyRecallLog " +
                               "WHERE id = @Id";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@Id", id);
                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                return false;
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
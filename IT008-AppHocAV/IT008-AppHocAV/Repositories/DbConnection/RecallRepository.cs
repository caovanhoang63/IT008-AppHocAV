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


        public List<int?> FindDefinitionIdByWordAndUserId(string word, int userId)
        {
            try
            {
                List<int?> result = new List<int?>();
                string query = "SELECT definition_id FROM VocabularyRecallLog " +
                               "WHERE word = @Word AND user_id = @UserId";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@Word", word);
                    command.Parameters.Add("@UserId", userId);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? id = reader["definition_id"] == DBNull.Value
                                ? null
                                : (int?)reader["definition_id"];
                            result.Add(id);
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
        }

        public int AddNewRecallLog(VocabularyRecallLog log)
        {
            try
            {
                string query = "INSERT INTO VocabularyRecallLog(User_Id, Word, Definition_Id, Created_At, Updated_At) " +
                               "output inserted.id " +
                               "VALUES(@UserId, @Word, @Definition_Id, GETDATE(), GETDATE())";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@UserId", log.UserId);
                    command.Parameters.Add("@Word", log.Word);
                    command.Parameters.Add("@Definition_Id", log.DefinitionId);
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
        
        public int AddNewWord(int userID, string word)
        {
            try
            {
                string query = "INSERT INTO VocabularyRecallLog(User_Id, Word, Created_At, Updated_At) " +
                               "output inserted.id " +
                               "VALUES(@UserId, @Word, GETDATE(), GETDATE())";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
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
                            vocabularyRecallLog.Id = (int)reader["id"];
                            vocabularyRecallLog.UserId = (int)reader["user_id"];
                            vocabularyRecallLog.Word = reader["word"].ToString();
                            vocabularyRecallLog.Meaning = reader["meaning"].ToString();
                            vocabularyRecallLog.IsSuccessful = reader.GetBoolean(reader.GetOrdinal("Is_Successful"));
                            vocabularyRecallLog.Example = reader["example"].ToString();

                            vocabularyRecallLog.DefinitionId = reader["definition_id"] == DBNull.Value
                                ? null
                                : (int?)reader["definition_id"];
                            vocabularyRecallLog.CreatedAt = (DateTime)reader["created_at"];
                            vocabularyRecallLog.UpdatedAt = (DateTime)reader["updated_at"];
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


        public void AddDefinitionId(int id, int definitionId)
        {
            try
            {
                string query = "UPDATE VocabularyRecallLog " +
                               "SET Definition_Id = @Definition_Id , updated_at = GETDATE() " +
                               "WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@Id", id);
                    command.Parameters.Add("@Definition_Id", definitionId);
                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
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

        public List<VocabularyRecallLog> FindVocabularyRecallLogsByWord(string word)
        {
            try
            {
                List<VocabularyRecallLog> vocabularyRecallLogs = new List<VocabularyRecallLog>();
                string query  = "SELECT * FROM VocabularyRecallLog WHERE word = @word";
                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.Add("@word", word);
                    _sqlConnection.Open();
                    using (SqlDataReader reader =command.ExecuteReader())
                    {
                        VocabularyRecallLog vocabularyRecallLog = new VocabularyRecallLog();
                        while (reader.Read())
                        {
                            vocabularyRecallLog.Id = (int) reader["id"];
                            vocabularyRecallLog.UserId = (int) reader["user_id"];
                            vocabularyRecallLog.Word = reader["word"].ToString();
                            vocabularyRecallLog.Meaning = reader["meaning"].ToString();
                            vocabularyRecallLog.IsSuccessful = reader.GetBoolean(reader.GetOrdinal("Is_Successful"));
                            vocabularyRecallLog.Example = reader["example"].ToString();
                            vocabularyRecallLog.DefinitionId = reader["definition_id"] == DBNull.Value
                                ? null
                                : (int?) reader["definition_id"];
                            vocabularyRecallLog.CreatedAt = (DateTime) reader["created_at"];
                            vocabularyRecallLog.UpdatedAt = (DateTime) reader["updated_at"];
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

        public void UpdateVocabularyRecallLog(VocabularyRecallLog log)
        {
            try
            {
                string query = "UPDATE VocabularyRecallLog " +
                               "SET definition_id = @definition_id, Updated_At = GETDATE() " +
                               "WHERE id = @Id";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@Id", log.Id);
                    command.Parameters.Add("@definition_id", log.DefinitionId);
                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
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


        public bool DeleteByDefinitionIdAndWord(int definitonId, string word)
        {
            try
            {

                string query = "DELETE FROM VocabularyRecallLog " +
                               "WHERE definition_id = @definition_id AND word = @word";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@definition_id", definitonId);
                    command.Parameters.Add("@word", word);
                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
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
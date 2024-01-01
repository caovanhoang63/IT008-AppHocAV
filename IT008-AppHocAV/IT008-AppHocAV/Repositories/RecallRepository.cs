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

        public List<VocabularyRecallLog> FindAllRecallLogsByDateAndUserId(int UserID, DateTime? dateTime = null )
        {
            try
            {
                string query = "";
                if (dateTime != null)
                {
                    query = "SELECT v.id as id, v.user_id as user_id, v.word as word, v.definition_id as definition_id, " +
                            "d.definitionText as definitionText, v.meaning as meaning, v.is_successful as is_successful, " +
                            "v.example as example, v.created_at as created_at, v.updated_at as updated_at " +
                            "FROM " +
                            "VocabularyRecallLog V " +
                            "LEFT JOIN " +
                            "Definition D " +
                            "On V.Definition_id = D.id " +
                            "WHERE " +
                            " User_Id = @UserId AND Year(Created_At) = Year(@Created_At) " +
                            "And Month(Created_At) = Month(@Created_At) And Day(Created_At) = Day(@Created_At)";
                }
                else
                {
                    query =
                        "SELECT v.id as id, v.user_id as user_id, v.word as word, v.definition_id as definition_id, " +
                        "d.definitionText as definitionText, v.meaning as meaning, v.is_successful as is_successful, " +
                        "v.example as example, v.created_at as created_at, v.updated_at as updated_at " +
                        "FROM " +
                        "VocabularyRecallLog V " +
                        "LEFT JOIN " +
                        "Definition D " +
                        "On V.Definition_id = D.id ";
                }
                

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@UserId", UserID);
                    if (dateTime != null)
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
                            vocabularyRecallLog.DefinitionId = reader["definition_id"] == DBNull.Value
                                ? null
                                : (int?)reader["definition_id"];
                            if (vocabularyRecallLog.DefinitionId != null)
                            {
                                vocabularyRecallLog.DefinitionText = reader["definitionText"].ToString();
                            }
                            vocabularyRecallLog.Meaning = reader["meaning"].ToString();
                            if (vocabularyRecallLog.Meaning == "" && vocabularyRecallLog.DefinitionId != null)
                            {
                                vocabularyRecallLog.Meaning = vocabularyRecallLog.DefinitionText;
                            }
                            vocabularyRecallLog.IsSuccessful = reader.GetBoolean(reader.GetOrdinal("Is_Successful"));
                            vocabularyRecallLog.Example = reader["example"].ToString();
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

        public void UpdateRecallLog(VocabularyRecallLog rowData)
        {
            try
            {
                string query = "UPDATE VocabularyRecallLog " +
                               "SET Word = @Word, Meaning = @Meaning, " +
                               "Is_Successful = @Is_Successful, Example = @Example, Updated_At = GETDATE() " +
                               "WHERE id = @Id";

                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.Add("@Id", rowData.Id);
                    command.Parameters.Add("@Word", rowData.Word);
                    command.Parameters.Add("@Meaning", rowData.Meaning);
                    command.Parameters.Add("@Is_Successful", rowData.IsSuccessful);
                    command.Parameters.Add("@Example", rowData.Example);
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
    }
}
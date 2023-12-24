using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;
using PexelsDotNetSDK.Models;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class ExamQ
    {
        private readonly SqlConnection _sqlConnection;

        public ExamQ(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public List<Question> GetRandomQuestion()
        {   
            List<Question> result = new List<Question>();
            Random random = new Random();
            HashSet<int> questionIds = new HashSet<int>();
            while (true) 
            {
                questionIds.Add(random.Next(1,81));
                if (questionIds.Count == 20)
                    break;
            }

            try
            {
                string query = "Select * from question where id=@i";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.Question question = new Models.Question(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("content")),
                                reader.GetString(reader.GetOrdinal("answer_a")),
                                reader.GetString(reader.GetOrdinal("answer_b")),
                                reader.GetString(reader.GetOrdinal("answer_c")),
                                reader.GetString(reader.GetOrdinal("answer_d")),
                                reader.GetString(reader.GetOrdinal("correct_answer")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            result.Add(question);
                        }
                    }

                }
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;

            }
            finally
            {
                _sqlConnection.Close();
            }

        }
    }
}
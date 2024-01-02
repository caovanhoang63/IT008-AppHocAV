using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;
using PexelsDotNetSDK.Models;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class ExamRepository
    {
        private readonly SqlConnection _sqlConnection;
        private List<Question> Questions;


        public ExamRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public List<Question> GetRandomQuestion()
        {
            Questions = new List<Question>();
            Random random = new Random();
            HashSet<int> questionIds = new HashSet<int>();
            string queslist = "(";
            int a = random.Next(1, 81);
            questionIds.Add(a);
            queslist += a.ToString();
            while (questionIds.Count < 20) 
            {
                int newId = random.Next(1, 81);
                if (!questionIds.Contains(newId)) 
                {
                    questionIds.Add(newId); 
                    queslist += "," + newId;
                }
            }
            queslist += ")";
            try
            {
                string query = "Select * from question where id in"+queslist;
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
                            Questions.Add(question);
                        }
                    }

                }
                questionIds.Clear();
                return Questions;

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
        public List<Models.Exam> SelectListExamByUserId(int userId)
        {
            List<Models.Exam> result = new List<Models.Exam>();


            try
            {
                string query = " SELECT id,user_id,level,category,score,created_at,time_taken" +
                               " FROM [test] " +
                               " WHERE user_id = " + userId + " ORDER BY created_at desc";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.Exam Exam = new Models.Exam(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                reader.GetByte(reader.GetOrdinal("level")),
                                reader.GetString(reader.GetOrdinal("category")),
                                reader.GetByte(reader.GetOrdinal("score")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetInt32(reader.GetOrdinal("time_taken")));
                            result.Add(Exam);
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

        public void SaveResult(Models.Exam exam)
        {
            try
            {
                string query = "INSERT INTO [test] (user_id,level,category,score,created_at,time_taken) " +
                    "VALUES (@user_id,@level,@cate,@score,GETDATE(),@time_taken)";
                using (SqlCommand command = new SqlCommand(query, _sqlConnection))
                {
                    command.Parameters.AddWithValue("@user_id", exam.Userid);
                    command.Parameters.AddWithValue("@level", Int32.Parse(exam.Level));
                    command.Parameters.AddWithValue("@cate", exam.Category);
                    command.Parameters.AddWithValue("@score", exam.Score);
                    command.Parameters.AddWithValue("@time_taken", Int32.Parse(exam.TimeTaken));
                    _sqlConnection.Open();
                    command.ExecuteNonQuery();
                    CTMessageBox.Show("Message", "Saved successfully ", MessageBoxType.Information);
                    return;


                }
            }
            catch (Exception ex)
            {
                CTMessageBox.Show("Message", "Saved fail ", MessageBoxType.Information);
                Console.WriteLine(ex);
                return;

            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void SaveDetail()
        {

        }
        public bool DeleteExamById(int id)
        {
            try
            {
                string query = "DELETE [test] WHERE id = @id";

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
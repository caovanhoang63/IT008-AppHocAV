using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class ExamQ
    {
        private readonly SqlConnection _sqlConnection;

        public ExamQ(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        private void GetRandomQuestion()
        {   
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
            }
            catch (Exception ex)
            {
                    Console.WriteLine(ex.ToString());
            }
            finally
            {
                _sqlConnection.Close();
            }

        }
    }
}
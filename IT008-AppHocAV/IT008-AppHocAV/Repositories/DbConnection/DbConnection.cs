﻿using System.Data.SqlClient;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Properties;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class DbConnection
    {
        #region Declare Fields
            private readonly SqlConnection _sqlConnection;
            private readonly Authentication _authentication;
            private readonly EssayQ _essayQ;
            private readonly CardQ _cardQ;
            private readonly DeskQ _deskQ;
            private readonly Register _register;
            private readonly ExamQ _examQ;
            private readonly UserQ _userQ;
            private readonly DictionaryRepository _dictionaryRepository;
            private readonly RecallRepository _recallRepository;
        #endregion

        #region Declare Properties

        public Authentication Authentication => _authentication;
        public DeskQ DeskQ => _deskQ;
        public EssayQ EssayQ => _essayQ;
        public CardQ CardQ => _cardQ;

        public Register Register => _register;

        public ExamQ ExamQ => _examQ;

        public UserQ UserQ => _userQ;
        public DictionaryRepository DictionaryRepository => _dictionaryRepository;
        
        public RecallRepository RecallRepository => _recallRepository;
        #endregion

        #region Declare Constructors
        public DbConnection()
        {   
            _sqlConnection = new SqlConnection(Settings.Default.DbConnectionString);
            _authentication = new Authentication(_sqlConnection);
            _essayQ = new EssayQ(_sqlConnection);
            _deskQ = new DeskQ(_sqlConnection);
            _cardQ = new CardQ(_sqlConnection);
            _userQ = new UserQ(_sqlConnection);
            _register = new Register(_sqlConnection);
            _dictionaryRepository = new DictionaryRepository(_sqlConnection);
            _recallRepository = new RecallRepository(_sqlConnection);
        }
        #endregion
        
    }
}
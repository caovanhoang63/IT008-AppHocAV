using System.Data.SqlClient;
using System.Net.NetworkInformation;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Properties;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class DbConnection
    {
        #region Declare Fields
            private readonly SqlConnection _sqlConnection;
            private readonly Authentication _authentication;
            private readonly EssayRepository _essayRepository;
            private readonly CardRepository _cardRepository;
            private readonly DeskRepository _deskRepository;
            private readonly RegisterRepository _registerRepository;
            private readonly ExamRepository _examRepository;
            private readonly UserRepository _userRepository;
            private readonly DictionaryRepository _dictionaryRepository;
            private readonly RecallRepository _recallRepository;
        #endregion

        #region Declare Properties

        public Authentication Authentication => _authentication;
        public DeskRepository DeskRepository => _deskRepository;
        public EssayRepository EssayRepository => _essayRepository;
        public CardRepository CardRepository => _cardRepository;
        public RegisterRepository RegisterRepository => _registerRepository;
        public ExamRepository ExamRepository => _examRepository;
        public UserRepository UserRepository => _userRepository;
        public DictionaryRepository DictionaryRepository => _dictionaryRepository;
        public RecallRepository RecallRepository => _recallRepository;
        #endregion

        #region Declare Constructors
        public DbConnection()
        {   
            _sqlConnection = new SqlConnection(Settings.Default.DbConnectionString);
            _authentication = new Authentication(_sqlConnection);
            _essayRepository = new EssayRepository(_sqlConnection);
            _examRepository = new ExamRepository(_sqlConnection);
            _deskRepository = new DeskRepository(_sqlConnection);
            _cardRepository = new CardRepository(_sqlConnection);
            _userRepository = new UserRepository(_sqlConnection);
            _registerRepository = new RegisterRepository(_sqlConnection);
            _dictionaryRepository = new DictionaryRepository(_sqlConnection);
            _recallRepository = new RecallRepository(_sqlConnection);
        }
        #endregion
        
    }
}
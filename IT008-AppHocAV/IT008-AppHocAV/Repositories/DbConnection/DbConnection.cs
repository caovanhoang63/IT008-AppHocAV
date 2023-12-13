using System.Data.SqlClient;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class DbConnection
    {
        #region Declare Fields
            private readonly SqlConnection _sqlConnection;
            private readonly Authentication _authentication;
            private readonly EssayQ _essayQ;
            private readonly CardQ _cardQ;
            private readonly Register _register;
        #endregion

        #region Declare Properties

        public Authentication Authentication => _authentication;

        public EssayQ EssayQ => _essayQ;
        public CardQ CardQ => _cardQ;

        public Register Register => _register;
            

        #endregion

        #region Declare Constructors
        public DbConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "LAPTOP-JRRAO9EA\\MSSQLSERVERR";
            builder.UserID = "sa";
            builder.Password = "123456";
            builder.InitialCatalog = "APP_HOC_AV";
            _sqlConnection = new SqlConnection(builder.ConnectionString);
            _authentication = new Authentication(_sqlConnection);
            _essayQ = new EssayQ(_sqlConnection);
            _cardQ = new CardQ(_sqlConnection);
            _register = new Register(_sqlConnection);

        }
        #endregion
        
    }
}
using System.Data.Common;
using System.Windows;
using DbConnection = IT008_AppHocAV.Repositories.DbConnection.DbConnection;

namespace IT008_AppHocAV.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DbConnection DbConnection { get; set; } = new DbConnection();
    }
}

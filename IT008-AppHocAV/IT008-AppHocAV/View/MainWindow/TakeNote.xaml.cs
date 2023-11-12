using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IT008_AppHocAV;
namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for TakeNote.xaml
    /// </summary>
    public partial class TakeNote : Page
    {
        public TakeNote()
        {
            InitializeComponent();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            path += "/TakeNote";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filename = path;
            filename += "/";
            filename +=TittleBox.Text;
            string noteContent = TakeNoteBox.Text;
            filename += ".txt";
            File.WriteAllText(filename, noteContent);
            NavigationService.GoBack();
        }

    }
}

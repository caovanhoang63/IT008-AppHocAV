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
using System.Windows.Shapes;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for TakeNoteWindow.xaml
    /// </summary>
    public partial class TakeNoteWindow : Window
    {
        public TakeNoteWindow()
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
            filename += TittleBox.Text;
            string noteContent = TakeNoteBox.Text;
            filename += ".txt";
            File.WriteAllText(filename, noteContent);
            this.Close();
        }
        private void TittleBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TakeNoteBox.Focus();
                e.Handled = true;
            }
            else e.Handled = false;
        }
    }
}

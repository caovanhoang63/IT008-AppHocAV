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
using IT008_AppHocAV.View.MainWindow;

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
            string noteContent = TakeNoteBox.Text;
            DateTime current = DateTime.Now;
            filename += current.ToString("dd_MM_yyyy_HH_mm_ss");
            filename += ".txt";
            File.WriteAllText(filename, noteContent);
            this.Close();
            
        }
        private void TakeNoteBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int caretIndex = TakeNoteBox.CaretIndex;
                TakeNoteBox.Text = TakeNoteBox.Text.Insert(caretIndex, Environment.NewLine);
                TakeNoteBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                e.Handled = true;
            }
            else e.Handled = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for Exam.xaml
    /// </summary>
    public partial class DoExamPage : Page
    {
        private List<Question> _question;
        public DoExamPage()
        {
            InitializeComponent();
        }

       
    }
}

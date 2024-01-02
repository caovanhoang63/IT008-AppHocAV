using System.Windows;

namespace IT008_AppHocAV.View
{
    public partial class SettingWindow : Window
    {
        
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ChatGptApiKey = ChatGptApiKeyTextBox.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Save successfully!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    
    }
}
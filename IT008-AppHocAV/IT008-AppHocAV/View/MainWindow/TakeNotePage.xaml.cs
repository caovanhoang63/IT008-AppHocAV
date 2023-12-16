﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    /// Interaction logic for TakeNotePage.xaml
    /// </summary>
    public partial class TakeNotePage : Page
    {
        public TakeNotePage()
        {
            InitializeComponent();
        }

        private void Minimal_Button_Click(object sender, RoutedEventArgs e)
        {
            Note.Visibility = Visibility.Hidden;
        }
        private void Popup_Button_Click(object sender, RoutedEventArgs e)
        {
            TakeNoteWindow takeNoteWindow = new TakeNoteWindow();
            takeNoteWindow.Show();
            string notebox = NoteTextBox.Text;
            takeNoteWindow.TakeNoteBox.Text = notebox;
            Note.Visibility = Visibility.Collapsed;
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
            DateTime current = DateTime.Now;
            filename += "/";
            string noteContent = NoteTextBox.Text;
            filename += current.ToString("dd_MM_yyyy_HH_mm_ss");
            filename += ".txt";
            File.WriteAllText(filename, noteContent);
            Note.Visibility = Visibility.Collapsed;
        }
        private void NoteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                int caretIndex = NoteTextBox.CaretIndex;
                NoteTextBox.Text = NoteTextBox.Text.Insert(caretIndex, Environment.NewLine);
                NoteTextBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                e.Handled = true;
            }
        }

    }
}

using System.Data.SqlClient;
using System;
using System.Windows.Controls;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;

namespace ReadBook.Pages.ResetPassword
{
    public partial class InputPhone : Page
    {
        public InputPhone()
        {
            InitializeComponent();
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (NumberTextBox.Text[0] == '8')
                {
                    NumberTextBox.Text = "+7";
                    NumberTextBox.Select(NumberTextBox.Text.Length, 0);
                }
            }
            catch (Exception ex)
            {
                var text = ex.ToString();
                Error window = new Error(text);
                window.Show();
            }
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0) || e.Text[0] == '-' || e.Text[0] == '(' || e.Text[0] == ')' || e.Text[0] == '+')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}

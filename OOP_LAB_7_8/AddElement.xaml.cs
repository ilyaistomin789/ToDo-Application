using System;
using System.Collections.Generic;
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
using OOP_LAB_7_8.Models;
using OOP_LAB_7_8.Resources.Languages;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для AddElement.xaml
    /// </summary>
    public partial class AddElement : Window
    {
        public AddElement()
        {
            InitializeComponent();
        }

        public AddElement(string currentLanguage)
        {
            InitializeComponent();
            if (currentLanguage == LanguageString.language[1])
            {
                nameTextBox.Margin = new Thickness(10, 0, 0, 0);
                priorityTextBox.Margin = new Thickness(1, 0, 0, 0);
                categoryTextBox.Margin = new Thickness(6, 0, 0, 0);
                statusTextBox.Margin = new Thickness(31, 0, 0, 0);
                descriptionTextBox.Margin = new Thickness(7, 0, 0, 0);
            }

            if (currentLanguage == LanguageString.language[2])
            {
                nameTextBox.Margin = new Thickness(34, 0, 0, 0);
                priorityTextBox.Margin = new Thickness(2, 0, 0, 0);
                categoryTextBox.Margin = new Thickness(7, 0, 0, 0);
                statusTextBox.Margin = new Thickness(32, 0, 0, 0);
                descriptionTextBox.Margin = new Thickness(15, 0, 0, 0);
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ToDoCollection.toDoModelsList.Add(new ToDoModel(nameTextBox.Text, Calendar.SelectedDate, priorityTextBox.Text, categoryTextBox.Text, statusTextBox.Text, descriptionTextBox.Text));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

    }
}

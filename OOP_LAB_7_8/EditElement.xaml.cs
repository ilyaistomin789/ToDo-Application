using System;
using System.Windows;
using OOP_LAB_7_8.Models;
using OOP_LAB_7_8.Resources.Languages;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для EditElement.xaml
    /// </summary>
    public partial class EditElement : Window
    {
        private int _id;
        private string _currentLanguage;
        public EditElement()
        {
            InitializeComponent();
        }


        public EditElement(int id, string name, string endingDate, string priority, string category, string status, string description, string currentLanguage)
        {
            InitializeComponent();
            this._id = id;
            ed_nameTextBox.Text = name;
            ed_Calendar.SelectedDate = DateTime.Parse(endingDate);
            ed_priorityTextBox.Text = priority;
            ed_categoryTextBox.Text = category;
            ed_statusTextBox.Text = status;
            ed_descriptionTextBox.Text = description;
            InitializeComponent();
            if (currentLanguage == LanguageString.language[1])
            {
                ed_nameTextBox.Margin = new Thickness(10, 0, 0, 0);
                ed_priorityTextBox.Margin = new Thickness(1, 0, 0, 0);
                ed_categoryTextBox.Margin = new Thickness(6, 0, 0, 0);
                ed_statusTextBox.Margin = new Thickness(31, 0, 0, 0);
                ed_descriptionTextBox.Margin = new Thickness(7, 0, 0, 0);
            }

            if (currentLanguage == LanguageString.language[2])
            {
                ed_nameTextBox.Margin = new Thickness(34, 0, 0, 0);
                ed_priorityTextBox.Margin = new Thickness(2, 0, 0, 0);
                ed_categoryTextBox.Margin = new Thickness(7, 0, 0, 0);
                ed_statusTextBox.Margin = new Thickness(32, 0, 0, 0);
                ed_descriptionTextBox.Margin = new Thickness(15, 0, 0, 0);
            }
            this._currentLanguage = currentLanguage;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ToDoCollection.toDoModelsList[_id].Name = ed_nameTextBox.Text;
                ToDoCollection.toDoModelsList[_id].EndingDate = ed_Calendar.SelectedDate.Value.ToString("D");
                ToDoCollection.toDoModelsList[_id].Priority = ed_priorityTextBox.Text;
                ToDoCollection.toDoModelsList[_id].Category = ed_categoryTextBox.Text;
                ToDoCollection.toDoModelsList[_id].Status = ed_statusTextBox.Text;
                ToDoCollection.toDoModelsList[_id].Description = ed_descriptionTextBox.Text;
                if (MessageBox.Show(_currentLanguage == LanguageString.language[1] ? "Элемент изменен" : "Element changed") == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


            

        }
    }
}

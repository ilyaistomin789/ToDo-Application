using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OOP_LAB_7_8.Models;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для SelectElement.xaml
    /// </summary>
    public partial class SelectElement : Window
    {
        private IEnumerable<ToDoModel> resEnumerable;
        public SelectElement()
        {
            InitializeComponent();
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (slct_currentTextBlock.Text == "Приоритет" || slct_currentTextBlock.Text == "Priority")
            {
                resEnumerable = from field in ToDoCollection.toDoModelsList where field.Priority == slctComboBox.Text select field;
            }

            if (slct_currentTextBlock.Text == "Категория" || slct_currentTextBlock.Text == "Category")
            {
                resEnumerable = from field in ToDoCollection.toDoModelsList where field.Category == slctComboBox.Text select field;
            }

            if (slct_currentTextBlock.Text == "Статус" || slct_currentTextBlock.Text == "Status")
            {
                resEnumerable = from field in ToDoCollection.toDoModelsList where field.Status == slctComboBox.Text select field;
            }

            slct_dg_todo.ItemsSource = resEnumerable;
        }
    }
}

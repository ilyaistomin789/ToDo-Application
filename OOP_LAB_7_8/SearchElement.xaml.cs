using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OOP_LAB_7_8.Models;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для SearchElement.xaml
    /// </summary>
    public partial class SearchElement : Window
    {
        public SearchElement()
        {
            InitializeComponent();
        }


        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<ToDoModel> srchRes;
                if (srchFieldStackPanel.Visibility == Visibility.Visible)
                {
                    if (srch_currentTextBlock.Text == "Название" || srch_currentTextBlock.Text == "Name")
                    {
                        srchRes = from t in ToDoCollection.toDoModelsList
                                where t.Name == srch_currentTextBox.Text
                                select t;
                            srch_dg_todo.ItemsSource = srchRes;
                    }

                    if (srch_currentTextBlock.Text == "Приоритет" || srch_currentTextBlock.Text == "Priority")
                    {
                        srchRes = from t in ToDoCollection.toDoModelsList
                            where t.Priority == srch_currentTextBox.Text
                            select t;
                        srch_dg_todo.ItemsSource = srchRes;
                    }

                    if (srch_currentTextBlock.Text == "Категория" || srch_currentTextBlock.Text == "Category")
                    {
                        srchRes = from t in ToDoCollection.toDoModelsList
                            where t.Category == srch_currentTextBox.Text
                            select t;
                        srch_dg_todo.ItemsSource = srchRes;
                    }

                    if (srch_currentTextBlock.Text == "Статус" || srch_currentTextBlock.Text == "Status")
                    {
                        srchRes = from t in ToDoCollection.toDoModelsList
                            where t.Status == srch_currentTextBox.Text
                            select t;
                        srch_dg_todo.ItemsSource = srchRes;
                    }

                    if (srch_currentTextBlock.Text == "Описание" || srch_currentTextBlock.Text == "Description")
                    {
                        srchRes = from t in ToDoCollection.toDoModelsList
                            where t.Description == srch_currentTextBox.Text
                            select t;
                        srch_dg_todo.ItemsSource = srchRes;
                    }

                }
                else
                {
                    srchRes = from t in ToDoCollection.toDoModelsList
                        where srch_Calendar.SelectedDate != null && t.EndingDate == srch_Calendar.SelectedDate.Value.ToString("D")
                        select t;
                    srch_dg_todo.ItemsSource = srchRes;
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}

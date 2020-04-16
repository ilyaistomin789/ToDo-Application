using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using OOP_LAB_7_8.Models;
using OOP_LAB_7_8.FileService;
using OOP_LAB_7_8.Resources.Languages;
using Timer = System.Timers.Timer;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _win;
        private FileIOService _fileService;
        private IEnumerable<string> distinctRes;
        private bool boolLang;
        public CultureInfo currLang = App.Languages[0];

        public MainWindow()
        {
            InitializeComponent();
            App.LanguageChanged += LanguageChanged;
            engButton.Tag = App.Languages[0];
            rusButton.Tag = App.Languages[1];
            rusButton.Click += ChangeLanguageClick;
            engButton.Click += ChangeLanguageClick;

        }
        private void LanguageChanged(Object sender, EventArgs e)
        {
            currLang = App.Language;
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            Button mi = sender as Button;
            if (mi?.Tag is CultureInfo lang)
            {
                App.Language = lang;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileService = new FileIOService(@"D:\Visual Studio 2019\OOP(C#)\2SEM\OOP_LAB_7_8\OOP_LAB_7_8\FileService\files\toDoDataList.json");
            try
            {
                ToDoCollection.toDoModelsList = _fileService.LoadData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this.Close();
            }
            dg_todo.ItemsSource = ToDoCollection.toDoModelsList;
            ToDoCollection.toDoModelsList.ListChanged += ToDoModelsListOnListChanged;
        }
        private void ToDoModelsListOnListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemMoved)
            {
                try
                {
                    _fileService.SaveData((BindingList<ToDoModel>) sender);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    this.Close();
                }
            }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            _win = new AddElement(currLang.Name);
            if (currLang.Name == LanguageString.language[1])
            {
                if (_win is AddElement separator) separator.add_separator.Width = 200;
            }
            _win.Show();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (dg_todo.SelectedIndex != -1)
            {
                var currentIndex = dg_todo.SelectedIndex;
                try
                {
                    string name = ToDoCollection.toDoModelsList[currentIndex].Name;
                    ToDoCollection.toDoModelsList.RemoveAt(currentIndex);
                    MessageBox.Show(currLang.Name == LanguageString.language[1]
                        ? $"Удален элемент с именем : {name}"
                        : $"Element with the name '{name}' was deleted");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show(currLang.Name == LanguageString.language[1] ? "Выберите элемент" : "Selected element");
            }
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            dg_todo.ItemsSource = ToDoCollection.toDoModelsList;
        }

        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(currLang.Name == LanguageString.language[1]
                ? "Создатель : Ilya Istomin \nВерсия : 2.0 \n2020"
                : "Creator : Ilya Istomin \nVersion : 2.0 \n2020");
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (dg_todo.SelectedIndex != -1)
            {
                int currentIndex = dg_todo.SelectedIndex;
                try
                {
                    _win = new EditElement(currentIndex,ToDoCollection.toDoModelsList[currentIndex].Name,ToDoCollection.toDoModelsList[currentIndex].EndingDate,ToDoCollection.toDoModelsList[currentIndex].Priority, ToDoCollection.toDoModelsList[currentIndex].Category, ToDoCollection.toDoModelsList[currentIndex].Status, ToDoCollection.toDoModelsList[currentIndex].Description, currLang.Name);
                    if (currLang.Name == LanguageString.language[1])
                    {
                        if (_win is EditElement separator) separator.ed_separator.Width = 200;
                    }
                    _win.Show();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show(currLang.Name == LanguageString.language[1] ? "Выберите элемент" : "Selected element");
            }
        }

        private void LangButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (boolLang == false)
            {
                rusButton.Visibility = Visibility.Visible;
                engButton.Visibility = Visibility.Visible;
                boolLang = true;
            }
            else
            {
                rusButton.Visibility = Visibility.Collapsed;
                engButton.Visibility = Visibility.Collapsed;
                boolLang = false;
            }
        }

        private void OpenSearchWindow(string nameTextBlock = null)
        {
            _win = new SearchElement();
            _win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (nameTextBlock != null)
            {
                if (_win is SearchElement style) style.srch_currentTextBlock.Text = nameTextBlock;
            }
            if (currLang.Name == LanguageString.language[1])
            {
                if (_win is SearchElement separator) separator.srch_separator.Width = 168;
            }
            _win.Show();
        }
        private void OpenSelectWindow(IEnumerable<string> elementsComboBox, string nameTextBlock = null)
        {
            _win = new SelectElement();
            _win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (nameTextBlock != null)
            {
                if (_win is SelectElement style)
                {
                    style.slct_currentTextBlock.Text = nameTextBlock;
                    style.slctComboBox.ItemsSource = elementsComboBox;
                }
            }
            if (currLang.Name == LanguageString.language[1])
            {
                if (_win is SelectElement separator) separator.slct_separator.Width = 187;
            }

            _win.Show();
        }

        private void NameMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(null);
        }

        private void EndDateMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            _win = new SearchElement();
            _win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _win.Height = 500;
            if (_win is SearchElement style)
            {
                style.srch_calendarStackPanel.Visibility = Visibility.Visible;
                style.srchFieldStackPanel.Visibility = Visibility.Collapsed;
            }
            _win.Show();

        }

        private void PriorityMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currLang.Name == LanguageString.language[1] ? "Приоритет" : "Priority");
        }

        private void CategoryMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currLang.Name == LanguageString.language[1] ? "Категория" : "Category");
        }

        private void StatusMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currLang.Name == LanguageString.language[1] ? "Статус" : "Status");
        }

        private void DescriptionMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currLang.Name == LanguageString.language[1] ? "Описание" : "Desciption");
        }

        private void Slct_priorityMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string[] priorities = new string[ToDoCollection.toDoModelsList.Count];
            
            for (int i = 0; i < priorities.Length; i++)
            {
                priorities[i] = ToDoCollection.toDoModelsList[i].Priority;
            }

            distinctRes = priorities.Distinct();
            OpenSelectWindow(distinctRes, currLang.Name == LanguageString.language[1] ? "Приоритет" : "Priority");
        }

        private void Slct_categoryMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string[] categories = new string[ToDoCollection.toDoModelsList.Count];
            for (int i = 0; i < categories.Length; i++)
            {
                categories[i] = ToDoCollection.toDoModelsList[i].Category;
            }

            distinctRes = categories.Distinct();
            OpenSelectWindow(distinctRes, currLang.Name == LanguageString.language[1] ? "Категория" : "Category");
        }

        private void Slct_statusMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string[] statuses = new string[ToDoCollection.toDoModelsList.Count];
            for (int i = 0; i < statuses.Length; i++)
            {
                statuses[i] = ToDoCollection.toDoModelsList[i].Status;
            }

            distinctRes = statuses.Distinct();
            OpenSelectWindow(distinctRes, currLang.Name == LanguageString.language[1] ? "Статус" : "Status");
        }
    }
}

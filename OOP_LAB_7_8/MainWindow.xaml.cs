using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OOP_LAB_7_8.Models;
using OOP_LAB_7_8.FileService;
using OOP_LAB_7_8.Resources.Languages;

namespace OOP_LAB_7_8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _win;
        private string _path = Environment.CurrentDirectory;
        private FileIOService _fileService;
        private IEnumerable<string> _distinctRes;
        private bool _boolLang;
        private bool _boolAdd;
        private bool _boolRemove;
        private int _countRemoving;
        private int _countAdding;
        public CultureInfo currentLanguage = App.Languages[0];
        private readonly Stack<ToDoModel> _undoStack = new Stack<ToDoModel>();
        private readonly Stack<ToDoModel> _redoStack = new Stack<ToDoModel>();
        public MainWindow()
        {
            InitializeComponent();
            App.LanguageChanged += LanguageChanged;
            engButton.Tag = App.Languages[0];
            rusButton.Tag = App.Languages[1];
            belButton.Tag = App.Languages[2];
            rusButton.Click += ChangeLanguageClick;
            engButton.Click += ChangeLanguageClick;
            belButton.Click += ChangeLanguageClick;
            themesComboBox.Items.Add("Red");
            themesComboBox.Items.Add("Light");
            themesComboBox.Items.Add("Green");
            themesComboBox.SelectedIndex = 1;
            SetUpCommands();
            
        }

        private void SetUpCommands()
        {
            CommandBinding undoCommandBinding = new CommandBinding(ApplicationCommands.Undo, UndoCommand_Executed,UndoCommand_CanExecute);
            CommandBinding redoCommandBinding = new CommandBinding(ApplicationCommands.Redo, RedoCommand_Executed,RedoCommand_CanExecute);
            this.CommandBindings.Add(undoCommandBinding);
            this.CommandBindings.Add(redoCommandBinding);
        }

        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _redoStack.Count > 0;
            if (e.CanExecute == false)
            {
                MessageBox.Show(currentLanguage.Name == LanguageString.language[1] ? "Операция Redo невозможна" : "Operation Redo is not possible");
            }
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_boolAdd == true)
            {
                var element = ToDoCollection.toDoModelsList.First(t => t.Equals(_redoStack.Peek()));
                _redoStack.Pop();
                ToDoCollection.toDoModelsList.Remove(element);
                _countAdding--;
                if (_countAdding == 0)
                {
                    _boolAdd = false;
                    _boolRemove = true;
                }
            }
            else
            {
                _undoStack.Push(_redoStack.Peek());
                ToDoCollection.toDoModelsList.Add(_redoStack.Pop());
                _boolAdd = true;
                _boolRemove = false;
                _countAdding++;
            }
        }

        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _undoStack.Count > 0;
            if (e.CanExecute == false)
            {
                MessageBox.Show(currentLanguage.Name == LanguageString.language[1] ? "Операция Undo невозможна" : "Operation Undo is not possible");
            }
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_boolRemove == true)
            {
                _redoStack.Push(_undoStack.Peek());
                ToDoCollection.toDoModelsList.Add(_undoStack.Pop());
                _countRemoving--;
                if (_countRemoving == 0)
                {
                    _boolAdd = true;
                    _boolRemove = false;
                }

            }
            else
            {
                var element = ToDoCollection.toDoModelsList.First(t => t.Equals(_undoStack.Peek()));
                _undoStack.Pop();
                ToDoCollection.toDoModelsList.Remove(element);
                _boolAdd = false;
                _boolRemove = true;
            }
        }


        private void ThemeChange(object sender, RoutedEventArgs e)
        {
            string style = themesComboBox.SelectedItem as string;
            var uri = new Uri($"Resources/Themes/{style}" + "Theme.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            currentLanguage = App.Language;
        }

        private void ChangeLanguageClick(object sender, EventArgs e)
        {
            Button mi = sender as Button;
            if (mi?.Tag is CultureInfo lang)
            {
                App.Language = lang;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileService = new FileIOService(Directory.GetParent(_path).Parent?.FullName + @"\FileService\files\toDoDataList.json");
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
            _win = new AddElement(currentLanguage.Name);
            if (currentLanguage.Name == LanguageString.language[1])
            {
                if (_win is AddElement separator) separator.add_separator.Width = 200;
            }

            if (currentLanguage.Name == LanguageString.language[2])
            {
                if (_win is AddElement separator) separator.add_separator.Width = 180;
            }
            _win.Show();
            if (_boolRemove == true)
            {
                _boolRemove = false;
            }
            _boolAdd = true;
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (dg_todo.SelectedIndex != -1)
            {
                var currentIndex = dg_todo.SelectedIndex;
                try
                {
                    string name = ToDoCollection.toDoModelsList[currentIndex].Name;
                    _undoStack.Push(ToDoCollection.toDoModelsList[currentIndex]);
                    ToDoCollection.toDoModelsList.RemoveAt(currentIndex);
                    MessageBox.Show(currentLanguage.Name == LanguageString.language[1]
                        ? $"Удален элемент с именем : {name}"
                        : $"Element with the name '{name}' was deleted");
                    if (_boolAdd == true)
                    {
                        _boolAdd = false;
                    }
                    _boolRemove = true;
                    _countRemoving++;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show(currentLanguage.Name == LanguageString.language[1] ? "Выберите элемент" : "Selected element");
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
            MessageBox.Show(currentLanguage.Name == LanguageString.language[1]
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
                    _win = new EditElement(currentIndex,ToDoCollection.toDoModelsList[currentIndex].Name,ToDoCollection.toDoModelsList[currentIndex].EndingDate,ToDoCollection.toDoModelsList[currentIndex].Priority, ToDoCollection.toDoModelsList[currentIndex].Category, ToDoCollection.toDoModelsList[currentIndex].Status, ToDoCollection.toDoModelsList[currentIndex].Description, currentLanguage.Name);
                    if (currentLanguage.Name == LanguageString.language[1])
                    {
                        if (_win is EditElement separator) separator.ed_separator.Width = 200;
                    }
                    if (currentLanguage.Name == LanguageString.language[2])
                    {
                        if (_win is EditElement separator) separator.ed_separator.Width = 180;
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
                MessageBox.Show(currentLanguage.Name == LanguageString.language[1] ? "Выберите элемент" : "Selected element");
            }
        }

        private void LangButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (_boolLang == false)
            {
                rusButton.Visibility = Visibility.Visible;
                engButton.Visibility = Visibility.Visible;
                belButton.Visibility = Visibility.Visible;
                _boolLang = true;
            }
            else
            {
                rusButton.Visibility = Visibility.Collapsed;
                engButton.Visibility = Visibility.Collapsed;
                belButton.Visibility = Visibility.Collapsed;
                _boolLang = false;
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
            if (currentLanguage.Name == LanguageString.language[1])
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
            if (currentLanguage.Name == LanguageString.language[1])
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
            OpenSearchWindow(currentLanguage.Name == LanguageString.language[1] ? "Приоритет" : "Priority");
        }

        private void CategoryMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currentLanguage.Name == LanguageString.language[1] ? "Категория" : "Category");
        }

        private void StatusMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currentLanguage.Name == LanguageString.language[1] ? "Статус" : "Status");
        }

        private void DescriptionMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow(currentLanguage.Name == LanguageString.language[1] ? "Описание" : "Desciption");
        }

        private void Slct_priorityMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string[] priorities = new string[ToDoCollection.toDoModelsList.Count];
            
            for (int i = 0; i < priorities.Length; i++)
            {
                priorities[i] = ToDoCollection.toDoModelsList[i].Priority;
            }

            _distinctRes = priorities.Distinct();
            OpenSelectWindow(_distinctRes, currentLanguage.Name == LanguageString.language[1] ? "Приоритет" : "Priority");
        }

        private void Slct_categoryMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string[] categories = new string[ToDoCollection.toDoModelsList.Count];
            for (int i = 0; i < categories.Length; i++)
            {
                categories[i] = ToDoCollection.toDoModelsList[i].Category;
            }

            _distinctRes = categories.Distinct();
            OpenSelectWindow(_distinctRes, currentLanguage.Name == LanguageString.language[1] ? "Категория" : "Category");
        }

        private void Slct_statusMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            string[] statuses = new string[ToDoCollection.toDoModelsList.Count];
            for (int i = 0; i < statuses.Length; i++)
            {
                statuses[i] = ToDoCollection.toDoModelsList[i].Status;
            }

            _distinctRes = statuses.Distinct();
            OpenSelectWindow(_distinctRes, currentLanguage.Name == LanguageString.language[1] ? "Статус" : "Status");
        }

        private void EllipseButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ELLIPSE BUTTON");
        }
    }
}

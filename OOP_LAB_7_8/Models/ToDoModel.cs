using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OOP_LAB_7_8.Annotations;

namespace OOP_LAB_7_8.Models
{
    public class ToDoModel : INotifyPropertyChanged
    {
        private string name;
        private string creationDate;
        private string endingDate;
        private string description;
        private string status;
        private string priority;
        private string category;
        public string Name
        {
            get => name;
            set
            {
                if (value != null && name == value)
                    return;
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string CreationDate
        {
            get => creationDate;
            set
            {
                if (value != null && creationDate == value)
                    return;
                creationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }

        public string EndingDate
        {
            get => endingDate;
            set
            {
                if (value != null && endingDate == value)
                    return;
                endingDate = value;
                OnPropertyChanged(nameof(EndingDate));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (value != null && description == value)
                    return;
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Status
        {
            get => status;
            set
            {
                if (value != null && status == value)
                    return;
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string Priority
        {
            get => priority;
            set
            {
                if (value != null && priority == value)
                    return;
                priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public string Category
        {
            get => category;
            set
            {
                if (value != null && category == value)
                    return;
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }


        public ToDoModel()
        {
            CreationDate = DateTime.UtcNow.ToString("F");
        }
        public ToDoModel(string name, DateTime? endingDate, string priority, string category, string status, string description)
        {
            CreationDate = DateTime.Now.ToString("F");
            Name = name;
            EndingDate = endingDate.Value.ToString("D");
            Priority = priority;
            Category = category;
            Status = status;
            Description = description;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

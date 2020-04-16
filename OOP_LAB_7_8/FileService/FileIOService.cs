using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OOP_LAB_7_8.Models;

namespace OOP_LAB_7_8.FileService
{
    public class FileIOService
    {
        private readonly string _path;

        public FileIOService(string path)
        {
            _path = path;
        }
        public BindingList<ToDoModel> LoadData()
        {
            if (!File.Exists(_path))
            {
                File.CreateText(_path).Dispose();
                return new BindingList<ToDoModel>();
            }
            using (var reader = File.OpenText(_path))
            {
                string fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<ToDoModel>>(fileText);
            }
        }

        public void SaveData(BindingList<ToDoModel> toDoBindingList)
        {
            using (StreamWriter sw = new StreamWriter(_path))
            {
                string output = JsonConvert.SerializeObject(toDoBindingList);
                sw.Write(output);
            }
        }
    }
}

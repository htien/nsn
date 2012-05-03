using System;
using System.Collections.Generic;
using System.IO;

namespace SaberLily.Utils
{
    public class PropertiesConfiguration
    {
        private Dictionary<string, object> list;
        private string filename;

        public PropertiesConfiguration() { }

        public PropertiesConfiguration(string file)
        {
            Reload(file);
        }

        public object Get(string field, string defValue)
        {
            return Get(field) == null ? defValue : Get(field);
        }

        public object Get(string field)
        {
            return list.ContainsKey(field) ? list[field] : null;
        }

        public void Set(string field, object value)
        {
            if (value == typeof(IDictionary<string, string>))
            {
                IDictionary<string, string> dic = (IDictionary<string, string>)value;
                foreach (string key in dic.Keys)
                {
                }
            }
            if (!list.ContainsKey(field))
                list.Add(field, value.ToString());
            else
                list[field] = value.ToString();
        }

        public void Save()
        {
            Save(this.filename);
        }

        public void Save(string filename)
        {
            this.filename = filename;

            if (!File.Exists(filename))
                File.Create(filename);

            StreamWriter file = new StreamWriter(filename);

            foreach (string prop in list.Keys)
                if (!String.IsNullOrWhiteSpace(list[prop]))
                    file.WriteLine(prop + "=" + list[prop]);

            file.Close();
        }

        public void Reload()
        {
            Reload(this.filename);
        }

        public void Reload(string filename)
        {
            this.filename = filename;
            list = new Dictionary<string, string>();

            if (File.Exists(filename))
                LoadFromFile(filename);
        }

        private void LoadFromFile(string filename)
        {
            foreach (string line in File.ReadAllLines(filename))
            {
                if (!String.IsNullOrEmpty(line) &&
                    !line.StartsWith(";") &&
                    !line.StartsWith("#") &&
                    !line.StartsWith("'") &&
                    line.Contains("="))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore dublicates
                        list.Add(key, value);
                    }
                    catch { }
                }
            }
        }
    }
}

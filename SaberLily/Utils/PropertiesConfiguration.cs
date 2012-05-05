using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public string this[string key]
        {
            get { return GetString(key); }
            set { Add(key, value); }
        }

        public object Get(string key, string defValue)
        {
            return Get(key) == null ? defValue : Get(key);
        }

        public object Get(string key)
        {
            return list.ContainsKey(key) ? list[key] : null;
        }

        public string GetString(string key)
        {
            object val = Get(key);
            return val != null ? val.ToString() : null;
        }

        public int GetInt(string key)
        {
            object val = Get(key);
            return val != null ? Convert.ToInt32(val) : 0;
        }

        public bool GetBoolean(string key)
        {
            object val = Get(key);
            return val != null ? Convert.ToBoolean(val) : false;
        }

        public void Add(string key, object value)
        {
            if (list == null)
                list = new Dictionary<string, object>();

            if (value.GetType() == typeof(IDictionary<string, string>))
                list[key] = value;
            else
                list[key] = value.ToString();
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

            foreach (string key in list.Keys)
            {
                if (list[key].GetType() == typeof(string))
                {
                    if (!String.IsNullOrWhiteSpace(list[key] as string))
                        file.WriteLine(key + "=" + list[key]);
                }
            }
            file.Close();
        }

        public virtual void Reload()
        {
            Reload(this.filename);
        }

        public virtual void Reload(string filename)
        {
            Reload(filename, "UTF-8");
        }

        public virtual void Reload(string filename, string encoding)
        {
            this.filename = filename;
            list = new Dictionary<string, object>();

            if (File.Exists(filename))
                LoadFromFile(filename, encoding);
        }

        public void LoadFromFile(string filename, string encoding)
        {
            if (list == null)
            {
                Reload(filename);
            }

            foreach (string line in File.ReadAllLines(filename, Encoding.GetEncoding(encoding)))
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

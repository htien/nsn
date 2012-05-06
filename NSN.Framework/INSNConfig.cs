namespace NSN.Framework
{
    public interface INSNConfig
    {
        string this[string key] { get; set; }
        string ApplicationPath { get; }
        string ApplicationMapPath { get; }
        object Get(string key);
        object Get(string key, string defValue);
        string GetString(string key);
        int GetInt(string key);
        bool GetBoolean(string key);
        void Add(string key, object value);
    }
}

namespace Authentication.Local.Models
{
    using System.Collections.Generic;
    using System.Data;
    using Syrx.Settings.Databases;

    public class AppSettings
    {
        public IEnumerable<Namespaces> Namespaces { get; set; }
        public IEnumerable<Connections> Connections { get; set; }
    }

    public class Namespaces
    {
        public string Namespace { get; set; }
        public IEnumerable<Types> Types { get; set; }
    }

    public class Types
    {
        public string Name { get; set; }
        public Dictionary<string, CommandSetting> Commands { get; set; }
    }

    public class CommandSetting
    {
        public string Split { get; set; }
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public CommandFlagSetting Flags { get; set; }
        public IsolationLevel IsolationLevel { get; set; }
        public string ConnectionAlias { get; set; }
    }

    public class Connections
    {
        public string Alias { get; set; }
        public string ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }
}
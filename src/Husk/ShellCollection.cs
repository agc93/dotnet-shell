using System.Collections.Generic;

namespace Husk
{
    public class ShellCollection : Dictionary<string, string>
    {
        public ShellCollection() { }
        // private ShellCollection(IDictionary<string, string> dict) {
        //     this = dict;
        // }
        public ShellCollection AddShell(string name, string path) {
            this[name] = path;
            return this;
        }

        public ShellCollection AddShell(KeyValuePair<string, string> shell) {
            this[shell.Key] = shell.Value;
            return this;
        }

        public ShellCollection AddShells(string[] extraShells) {
            foreach (var shell in extraShells)
            {
                this.AddShell(shell);
            }
            return this;
        }

        public ShellCollection AddShell(string path) {
            var key = System.IO.Path.GetFileNameWithoutExtension(path);
            this[key] = path;
            return this;
        }

        // public static implicit operator ShellCollection(Dictionary<string, string> d) {
        //     var s = new ShellCollection();
        //     s = (ShellCollection)d;
        //     return s;
        // }
    }

    public static class ShellCollectionExtension {
        public static ShellCollection ToShellCollection(this IEnumerable<KeyValuePair<string, string>> collection) {
            var shells = new ShellCollection();
            foreach (var item in collection)
            {
                shells.AddShell(item);
            }
            return shells;
        }
    }
}
using YamlDotNet.Serialization;

namespace dotnet_shell
{
    public static class YamlExtensions
    {
        public static string ToYaml(this ShellCollection shells) {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(shells);
            return yaml;
        }

        public static ShellCollection ToShellCollection(this string yaml) {
            var deserializer = new DeserializerBuilder().Build();
            var shells = deserializer.Deserialize<ShellCollection>(yaml);
            return shells;
        }
    }
}
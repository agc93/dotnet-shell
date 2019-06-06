namespace Husk.Services
{
    public interface IConfigService
    {
         bool WriteConfig(ShellCollection shells);
         ShellCollection ReadConfig(string filePath = null);
    }
}
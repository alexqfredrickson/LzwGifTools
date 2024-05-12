using System.Configuration;

namespace LzwGifTools.Tests
{
    public static class Config
    {
        public static string WorkingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
    }
}

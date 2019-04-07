using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LzwGifTools.Tests
{
    public static class Config
    {
        public static string WorkingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
    }
}

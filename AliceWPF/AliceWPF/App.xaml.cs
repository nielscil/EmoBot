using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Alice;
using System.IO;

namespace AliceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ChatBot.Init(this);

            string codeBasePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string path = new Uri(codeBasePath).LocalPath + "\\Resources\\test.json";
            ChatBot.LoadFacts(path);
        }
    }
}

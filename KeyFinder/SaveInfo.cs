using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FCTool
{
    class SaveInfo
    {

        public static string[] GetInfo()
        {
            string Path = Environment.CurrentDirectory + "\\FCToolSaves.txt";
            var logFile = File.ReadAllLines(Path);
            var logList = new List<string>(logFile);

            return logList.ToArray();
        }

        public static void Save(List<string> list)
        {
            System.IO.File.WriteAllText(@"FCToolSaves.txt", string.Empty);
            TextWriter tw = new StreamWriter("FCToolSaves.txt");

            foreach (string s in list)
                tw.WriteLine(s);

            tw.Close();
        } 
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace FCTool
{
    class LicenseSet
    {
        public static void ReplaceLicensingSettingsFile(string NewPath)
        {
            string Path = Environment.CurrentDirectory;
            string name = "\\LicensingSettings.xml";
            
            File.SetAttributes(Path + name, FileAttributes.Normal);
            File.SetAttributes(NewPath + name, FileAttributes.Normal);
            File.Copy(Path + name, NewPath + name, true);
            //File.SetAttributes(NewPath + name, FileAttributes.Normal);
        }

        public static void ChangeAdressLicensingSettingsFile(string Path, string NewAdress)
        {
            string name = "\\LicensingSettings.xml";
            XDocument doc = XDocument.Load(Path + name);
           
            var address = doc.Root.Element("LicensingServers").Element("MainNetworkLicenseServer");

           
            if (address != null)
            {
                
                address.Attribute("ServerAddress").Value = NewAdress;
            }
           
            doc.Save(Path + name);
        }

        public static string  LicensingSettingsFileToString(string Path)
        {
            string name = "\\LicensingSettings.xml";
            XDocument doc = XDocument.Load(Path + name);

            return doc.ToString();
        }
    }
}

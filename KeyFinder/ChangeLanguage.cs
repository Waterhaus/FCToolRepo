using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Forms;

namespace FCTool
{
    class ChangeLanguage
    {
        public static List<int> InitListOfLang()
        {
            List<int> L = new List<int>();
            L.Add(0);
            L.Add(5);
            L.Add(63);
            L.Add(2);
            L.Add(15);
            L.Add(1);
            L.Add(3);
            L.Add(26);
            L.Add(27);
            L.Add(64);

            return L;

        }

        public static void Change(int lang, List<int> check)
        {
            // The name of the key must include a valid root.
            const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\ABBYY\\FlexiCapture\\12.0\\Fine Objects";
            const string subkey = "InterfaceLanguage";
            const string keyName = userRoot + "\\" + subkey;

            RegistryKey myKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\ABBYY\\FlexiCapture\\12.0\\Fine Objects", true);
            if (myKey != null && check.Contains(1))
            {
                myKey.SetValue("InterfaceLanguage", lang.ToString(), RegistryValueKind.String);
                myKey.Close();
            }

            myKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\ABBYY\\FlexiCapture\\12.0\\LicenseManager\\Fine Objects", true);
            if (myKey != null && check.Contains(2))
            {
                myKey.SetValue("InterfaceLanguage", lang.ToString(), RegistryValueKind.String);
                myKey.Close();
            }

            myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ABBYY\\FlexiCapture\\12.0\\Fine Objects", true);
            if (myKey != null && check.Contains(3))
            {
                myKey.SetValue("InterfaceLanguage", lang.ToString(), RegistryValueKind.String);
                myKey.Close();
            }

            myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ABBYY\\ScanStationFC\\4.0\\Fine Objects", true);
            if (myKey != null && check.Contains(4))
            {
                myKey.SetValue("InterfaceLanguage", lang.ToString(), RegistryValueKind.String);
                myKey.Close();
            }

            
        }
    
    }
}

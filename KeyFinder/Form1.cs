using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FCTool
{
    public partial class Form1 : Form
    {
        List<string> blackList;
        List<string> zeroList;
        List<int> language;
        List<int> langList;
        List<KeyValuePair<string,List<int>>> Data;
        string ServerPath;
        string StationsPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            
        }



        public static string ExtractFilename(string filepath)
        {
            // If path ends with a "\", it's a path only so return String.Empty.
            if (filepath.Trim().EndsWith(@"\"))
                return String.Empty;

            // Determine where last backslash is.
            int position = filepath.LastIndexOf('\\');
            // If there is no backslash, assume that this is a filename.
            if (position == -1)
            {
                // Determine whether file exists in the current directory.
                if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + filepath))
                    return filepath;
                else
                    return String.Empty;
            }
            else
            {
                // Determine whether file exists using filepath.
                if (File.Exists(filepath))
                    // Return filename without file path.
                    return filepath.Substring(position + 1);
                else
                    return String.Empty;
            }
        }

        public static string ExtractPath(string filepath)
        {
            // If path ends with a "\", it's a path only so return String.Empty.
            if (filepath.Trim().EndsWith(@"\"))
                return String.Empty;

            // Determine where last backslash is.
            int position = filepath.LastIndexOf('\\');
            // If there is no backslash, assume that this is a filename.
            if (position == -1)
            {
                // Determine whether file exists in the current directory.
                if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + filepath))
                    return filepath;
                else
                    return String.Empty;
            }
            else
            {
                // Determine whether file exists using filepath.
                if (File.Exists(filepath))
                    // Return filename without file path.
                    return filepath.Substring(0,position);
                else
                    return String.Empty;
            }
        }

        bool isValid(string filepath)
        {
            int position = filepath.LastIndexOf('.');

            return filepath.Substring(position + 1).Equals("dll", StringComparison.Ordinal) || filepath.Substring(position + 1).Equals("chm", StringComparison.Ordinal);
        }

        void GetList(string sDir, string searchTemplate, ref List<string> files)
        {
            
            foreach (string f in Directory.GetFiles(sDir, searchTemplate))
            {
                if(isValid(f)) files.Add(f);
            }

            GetListOfFiles(sDir, searchTemplate, ref files);

        }

        void GetListOfFiles(string sDir, string searchTemplate, ref List<string> files)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d, searchTemplate))
                    {
                        if(isValid(f))   files.Add(f);
                    }
                    GetListOfFiles(d, searchTemplate, ref files);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        void CreateListOfLanguages(string str)
        {
            string l;
            language = new List<int>();
            while (str != null)
            {
                int pos = str.LastIndexOf(',');
                if (pos >= 0)
                {
                    l = str.Substring(pos + 1);
                    language.Add(int.Parse(l));
                    str = str.Substring(0,pos);
                }else
                {
                    language.Add(int.Parse(str));
                    str = null;
                }
            }

            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            UpdatePath();
            string[] quicklang = System.IO.File.ReadAllLines(@"Langs.txt");
            textBox1.Text = StationsPath;
            textBox2.Text  = quicklang[0];
            string[] banned = System.IO.File.ReadAllLines(@"BlackList.txt");
            blackList = new List<string>(banned);

            comboBox1.SelectedIndex = 0;

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i,true);
            }

            

        }

        private void InitializeDataGridView(List<string> files)
        {
            // Create an unbound DataGridView by declaring a column count.
            dataGridView1.ColumnCount = 2;
            dataGridView1.ColumnHeadersVisible = true;

            dataGridView1.Columns[0].Width = 300;
            dataGridView1.Columns[1].Width = 500;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Set the column header names.
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Path";


            // Populate the rows.
            string[] col1 = new string[files.Count];
            string[] col2 = new string[files.Count];

            for(int i = 0; i < col1.Length; i++)
            {
                col1[i] = ExtractFilename(files[i]);
                col2[i] = ExtractPath(files[i]);
            }

           

            for(int i = 0; i < col1.Length; i++)
            {
                string[] rowArray = new string[2];
                rowArray[0] = col1[i];
                rowArray[1] = col2[i];
                dataGridView1.Rows.Add(rowArray);
                //dataGridView1.Columns.Add(rowArray);
            }
        }


        private List<string> DeleteBlackListFiles( List<string> files)
        {
            string[] f = new string[files.Count];
          
            
            List<string> anser = new List<string>();


            for (int i = 0; i < f.Length; i++)
            {
                f[i] = ExtractFilename(files[i]);
               
                bool isBanned = inBlackList(f[i]);

                if (!isBanned)
                {
                    anser.Add(files[i]);

                }


            }

            return anser;
        }

        private bool inBlackList(string str)
        {
            
            return blackList.Contains(str);
        }

        private List<string> DeleteTrashFiles(ref List<string> files)
        {
            string[] f = new string[files.Count];
            int position = 0;
            string name;
            int n;
            List<string> anser = new List<string>();
            

            for (int i = 0; i < f.Length; i++)
            {
                f[i] = ExtractFilename(files[i]);
                position = f[i].LastIndexOf('.');
                name = f[i].Substring(0,position);
                bool isNumeric = int.TryParse(name.Substring(name.Length - 1,1), out n);

                if (isNumeric)
                {
                    anser.Add(files[i]);
                    
                } 


            }

            anser.Sort();
            


            return anser;

            }


        private List<string> FindZeroFiles(List<string> files)
        {
            List<string> zero_files = new List<string>();
            string[] f = new string[files.Count];
            int position = 0;
            string name;
            int n;
          


            for (int i = 0; i < f.Length; i++)
            {
                f[i] = ExtractFilename(files[i]);
                position = f[i].LastIndexOf('.');
                name = f[i].Substring(0, position);
                bool isNumeric = int.TryParse(name.Substring(name.Length - 1, 1), out n);

                if (isNumeric && n == 0)
                {
                    zero_files.Add(name.Substring(0, name.Length - 1));

                }


            }
            return zero_files;

        }


        private string ClearEndNumber(string str)
        {
            int n = 0;

            bool isNumeric = true;

            while (isNumeric)
            {
                isNumeric = int.TryParse(str.Substring(str.Length - 1, 1), out n);
                if (isNumeric) str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        private List<string> DeleteNotZero(List<string> files)
        {
            string[] f = new string[files.Count];
            int position = 0;
            string name;
            int n;
            List<string> anser = new List<string>();


            for (int i = 0; i < f.Length; i++)
            {
                f[i] = ExtractFilename(files[i]);
                position = f[i].LastIndexOf('.');
                name = f[i].Substring(0, position);
                name = ClearEndNumber(name);
                bool inZeroList = zeroList.Contains(name);

                if (inZeroList) anser.Add(files[i]);


            }
            return anser;
        }


        private string GetClearName(string str)
        {
            return ClearEndNumber(DeleteExtention(str));
        }
        private int GetClearNumber(string str)
        {
            string fullname = DeleteExtention(str);
            string name = ClearEndNumber(DeleteExtention(str));

            string c = fullname.Substring(name.Length);
            return int.Parse(c);


        }


        private int FindIndex(string str)
        {
           return zeroList.IndexOf(GetClearName(str));
        }

        private void ConvertToData(List<string> files)
        {
            Data = new List<KeyValuePair<string, List<int>>>();
           

            List<string> anser = new List<string>();


            for (int i = 0; i < zeroList.Count; i++)
            {
                Data.Add(new KeyValuePair<string, List<int>>(zeroList[i], new List<int>()));
               

            }

            int k = 0;
            for (int i = 0; i < files.Count; i++)
            {
                k = FindIndex(files[i]);
                Data[k].Value.Add(GetClearNumber(files[i]));
            }
            int a = 0;

       }


        private string DeleteExtention(string str)
        {
            str = ExtractFilename(str);
            int position = str.LastIndexOf('.');
            return str.Substring(0, position);
        }

      
        private void FindMissing()
        {

            // Create an unbound DataGridView by declaring a column count.
            dataGridView2.ColumnCount = 2;
            dataGridView2.ColumnHeadersVisible = true;

            dataGridView2.Columns[0].Width = 200;
            dataGridView2.Columns[1].Width = 200;

            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            // Set the column header names.
            dataGridView2.Columns[0].Name = "File Name";
            dataGridView2.Columns[1].Name = "Missed";

            for (int i = 0; i < zeroList.Count; i++)
            {

                var firstNotSecond = Data[i].Value.Except(language).ToList();
                var secondNotFirst = language.Except(Data[i].Value).ToList();

              
                if (secondNotFirst.Count != 0)
                {
                    string[] row = new string[2];
                    row[0] =  Data[i].Key ;
                    for (int j = 0; j < secondNotFirst.Count; j++)
                        row[1] += ", " + secondNotFirst[j];

                    dataGridView2.Rows.Add(row);
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            

            string lang = textBox2.Text;
            CreateListOfLanguages(lang);


            dataGridView1.Rows.Clear();
            string path = textBox1.Text;
            //string[] files = Directory.GetFiles(@path, );
            List<string> files = new List<string>();
            List<string> chm_files = new List<string>();
            GetList(@path, "*.dll", ref files);
            GetList(@path, "*.chm", ref chm_files);

            files.AddRange(chm_files);
            //files.Sort();
            List<string> anser = DeleteBlackListFiles(DeleteTrashFiles(ref files));
            zeroList = FindZeroFiles(anser);
            List<string> final = DeleteNotZero(anser);
            ConvertToData(final);
            FindMissing();

            InitializeDataGridView(final);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            langList = ChangeLanguage.InitListOfLang();
            int LangIndex = comboBox1.SelectedIndex;
            string name = comboBox1.Text;
            int LangNumber = langList[LangIndex];
            List<int> check = new List<int>();
            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                check.Add(indexChecked);
            }

            ChangeLanguage.Change(LangNumber, check);

            MessageBox.Show(name + " язык был установлен!");
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex == 0)
            {
                for (int i = 1; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, checkedListBox1.GetItemChecked(0));
                }
            }
            else
            {
                if (!checkedListBox1.GetItemChecked(checkedListBox1.SelectedIndex))
                {
                    checkedListBox1.SetItemChecked(0, false);
                }
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox_adressServ.Enabled = true;
            }
            else
            {
                textBox_adressServ.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox_view.Enabled = true;
            }
            else
            {
                textBox_view.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            if (radioButton1.Checked)
            {
                try
                {
                    LicenseSet.ReplaceLicensingSettingsFile(StationsPath);
                    LicenseSet.ReplaceLicensingSettingsFile(ServerPath);
                    MessageBox.Show("Замена прошла успешно!");
                }
                catch (Exception)
                {

                    throw;
                }
           
            }

            if (radioButton2.Checked)
            {
                LicenseSet.ChangeAdressLicensingSettingsFile(StationsPath, textBox_adressServ.Text);
                LicenseSet.ChangeAdressLicensingSettingsFile(ServerPath, textBox_adressServ.Text);
                MessageBox.Show("Замена прошла успешно!");
            }

            if (radioButton3.Checked)
            {
                
                MessageBox.Show("Ха! Кно");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void сменитьПутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathForm form = new PathForm();
            form.main = this;
            form.Show();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void UpdatePath()
        {
            string[] list = SaveInfo.GetInfo();
            ServerPath = list[1];
            StationsPath = list[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recursive_File_Copier
{
    public partial class Form1 : Form
    {
        FileInfo[] g_Files;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                textBox1.Text = folderBrowserDialog1.SelectedPath;

                string loc = textBox1.Text;
                DirectoryInfo d = new DirectoryInfo(loc);
                g_Files = d.GetFiles("*.*", SearchOption.AllDirectories);

                if (g_Files.Length < 1) return;

                foreach(FileInfo f in g_Files)
                {
                    listBox1.Items.Add(f.FullName);
                }
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 1 || textBox2.Text.Length < 1) return;

            int barDivision = 100 / g_Files.Length;
            progressBar1.Step = barDivision + 1;

            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if(!Directory.Exists(textBox2.Text)) Directory.CreateDirectory(textBox2.Text);

            FileInfo[] fi = dir.GetFiles();
            foreach(FileInfo file in fi)
            {
                file.CopyTo(Path.Combine(textBox2.Text, file.Name), true);
            }

            ScanSubdirs(textBox1.Text, textBox2.Text);
        }

        private void ScanSubdirs(string curPath, string destPath)
        {
            DirectoryInfo dir = new DirectoryInfo(curPath);
            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dirs)
            {
                string newPath = Path.Combine(destPath, subdir.Name);
                string nextPath = Path.Combine(curPath, subdir.Name);
                if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
                foreach (FileInfo f in subdir.GetFiles("*.*"))
                {
                    try
                    {
                        f.CopyTo(Path.Combine(newPath, f.Name), true);
                    }
                    catch(Exception ex)
                    { }
                    progressBar1.PerformStep();
                }
                ScanSubdirs(nextPath, newPath);
            }
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
        }
    }
}

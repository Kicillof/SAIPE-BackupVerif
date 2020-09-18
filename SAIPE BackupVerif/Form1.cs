using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace SAIPE_BackupVerif
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rootPath = @"G:\GitHub Repos\FILESYSTEM TEST"; //Folder de backups
            string[] dirs = Directory.GetDirectories(rootPath,"*",SearchOption.AllDirectories); //Lista de folders y subfolders
            string[] files; //Lista de archivos
            bool checkRoot = true;
            var files_list = new List<Archivos>();

            foreach (string dir in dirs)
            {
                //Archivos en el root
                if (checkRoot) //Verifica el root solo una vez (checkRoot)
                {
                    checkRoot = false;
                    files = Directory.GetFiles(rootPath, "*", SearchOption.TopDirectoryOnly); //Lista de archivos en el root
                    DirectoryInfo dirRoot = new DirectoryInfo(rootPath);
                    Console.WriteLine(dirRoot.Name);

                    foreach (string file in files)
                    {
                        Console.WriteLine("     " + Path.GetFileName(file));
                    }
                }

                files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly); //Lista de archivos en la carpeta actual

                DirectoryInfo dirInf = new DirectoryInfo(dir);
                Console.WriteLine(dirInf.Name);

                foreach (string file in files)
                {
                    var info = new FileInfo(file);
                    double size_kb = info.Length / 1024.0; //el .0 es necesario para mostrar los valores de MB con decimales
                    double size_mb = info.Length / 1024.0 / 1024.0;

                    Console.WriteLine("     " 
                        + Path.GetFileName(file) 
                        + " - " + String.Format("{0:0.00}", size_kb) + " KB" 
                        + " - " + String.Format("{0:0.00}", size_mb) + " MB"
                        + " - " + info.LastWriteTime);

                    var file_new = new Archivos(Path.GetFileName(file), dirInf.Name, size_kb, size_mb, info.LastWriteTime);
                    files_list.Add(file_new);
                }
            }

            dataGridView1.DataSource = files_list;

            this.dataGridView1.Columns[2].DefaultCellStyle.Format = "0.00#";
            this.dataGridView1.Columns[3].DefaultCellStyle.Format = "0.00#";

            //dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);

            dataGridView1.Refresh();
            //dataGridView1.Update();


            Console.ReadLine();
        }

    }
}

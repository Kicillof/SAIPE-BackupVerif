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

        public void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(label_directorio.Text)) //Verifica que se haya seleccionado un directorio
            {
                MessageBox.Show("Seleccionar un directorio para analizar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string rootPath = label_directorio.Text.ToString();
                //string rootPath = @"G:\GitHub Repos\FILESYSTEM TEST"; //Folder de backups C:\Users\soportetecnico\Desktop\FILESYSTEM TEST
                string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories); //Lista de folders y subfolders
                bool checkRoot = true;
                var files_list = new List<Archivos>();

                //Relevo de archivos en root
                RelevoArchivos(checkRoot, rootPath, dirs, files_list);

                //Relevo de archivos en cada carpeta
                checkRoot = false;
                RelevoArchivos(checkRoot, rootPath, dirs, files_list);

                files_list = files_list.OrderByDescending(x => x.LastModified).ToList(); //Ordena

                dataGridView1.DataSource = files_list;

                //Establece el formato de los tamaños en KB Y mb
                this.dataGridView1.Columns[2].DefaultCellStyle.Format = "0.00#";
                this.dataGridView1.Columns[3].DefaultCellStyle.Format = "0.00#";

                dataGridView1.Refresh();

                Console.ReadLine();
            }
        }

        private List<Archivos> RelevoArchivos(bool checkRoot, string rootPath, string[] dirs, List<Archivos> files_list)
        {
            string[] files; //Lista de archivos
            DirectoryInfo dirInf = new DirectoryInfo(rootPath);

            if (checkRoot)
            {
                checkRoot = false;
                files = Directory.GetFiles(rootPath, "*", SearchOption.TopDirectoryOnly); //Lista de archivos en el root
                DirectoryInfo dirRoot = new DirectoryInfo(rootPath);
                //Console.WriteLine(dirRoot.Name);

                foreach (string file in files)
                {
                    var info = new FileInfo(file);
                    double size_kb = info.Length / 1024.0; //el .0 es necesario para mostrar los valores de MB con decimales
                    double size_mb = info.Length / 1024.0 / 1024.0;

                    //Console.WriteLine("     "
                    //    + Path.GetFileName(file)
                    //    + " - " + String.Format("{0:0.00}", size_kb) + " KB"
                    //    + " - " + String.Format("{0:0.00}", size_mb) + " MB"
                    //    + " - " + info.LastWriteTime);

                    var file_new = new Archivos(Path.GetFileName(file), dirRoot.Name, size_kb, size_mb, info.LastWriteTime);
                    files_list.Add(file_new);
                }
            }
            else
            {
                foreach (string dir in dirs)
                {
                    files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly); //Lista de archivos en la carpeta actual

                    dirInf = new DirectoryInfo(dir);
                    //Console.WriteLine(dirInf.Name);

                    foreach (string file in files)
                    {
                        var info = new FileInfo(file);
                        double size_kb = info.Length / 1024.0; //el .0 es necesario para mostrar los valores de MB con decimales
                        double size_mb = info.Length / 1024.0 / 1024.0;

                        //Console.WriteLine("     "
                        //    + Path.GetFileName(file)
                        //    + " - " + String.Format("{0:0.00}", size_kb) + " KB"
                        //    + " - " + String.Format("{0:0.00}", size_mb) + " MB"
                        //    + " - " + info.LastWriteTime);

                        var file_new = new Archivos(Path.GetFileName(file), dirInf.Name, size_kb, size_mb, info.LastWriteTime);
                        files_list.Add(file_new);
                    }
                }
            }
            return files_list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderb = new FolderBrowserDialog();
            //DialogResult result = folderb.ShowDialog();
            //string txtPath;

            if (folderb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label_directorio.Text = folderb.SelectedPath;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("La segunda fecha no puede ser mayor a la primera", "", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

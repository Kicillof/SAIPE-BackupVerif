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
                }
                
                //Console.WriteLine(dir);
                //Debug.WriteLine(dir);
            }

            //string[] files = Directory.GetFiles(rootPath,"*", SearchOption.AllDirectories); //Lista de archivos

            //foreach (string file in files)
            //{

            //    Console.WriteLine(Path.GetFileName(file));
            //    //Console.WriteLine(file);
            //    //Debug.WriteLine(file);

            //    //System.IO.FileInfo fi = new System.IO.FileInfo(file);
            //    //Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
            //}

            Console.ReadLine();
        }
    }
}

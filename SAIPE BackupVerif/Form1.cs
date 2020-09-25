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
            if (string.IsNullOrEmpty(textBox_directorio.Text)) //Verifica que se haya seleccionado un directorio
            {
                MessageBox.Show("Seleccionar un directorio para analizar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else //Procede normalmente
            {
                string rootPath = textBox_directorio.Text.ToString();
                //string rootPath = @"G:\GitHub Repos\FILESYSTEM TEST"; //Folder de backups C:\Users\soportetecnico\Desktop\FILESYSTEM TEST
                string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories); //Lista de folders y subfolders
                bool checkRoot = true;
                var files_list = new List<Archivos>();

                //Relevo de archivos en root
                RelevoArchivos(checkRoot, rootPath, dirs, files_list);

                //Relevo de archivos en cada carpeta
                checkRoot = false;
                RelevoArchivos(checkRoot, rootPath, dirs, files_list);

                //Identifica el checkbox en true
                byte id_checkbox = 0;
                if (checkBox1.Checked == true) { id_checkbox = 1; };
                if (checkBox2.Checked == true) { id_checkbox = 2; };
                if (checkBox3.Checked == true) { id_checkbox = 3; };
                if (checkBox4.Checked == true) { id_checkbox = 4; }; 

                //Ordenar
                switch (id_checkbox)
                {
                    case 1:
                        if (checkBox5.Checked) //si es ascendente
                            files_list = files_list.OrderBy(x => x.Folder).ToList();
                        else //si es descendente
                            files_list = files_list.OrderByDescending(x => x.Folder).ToList();
                        break;
                    case 2:
                        if (checkBox5.Checked) //si es ascendente
                            files_list = files_list.OrderBy(x => x.Name).ToList();
                        else //si es descendente
                            files_list = files_list.OrderByDescending(x => x.Name).ToList();
                        break; 
                    case 3:
                        if (checkBox5.Checked) //si es ascendente
                            files_list = files_list.OrderBy(x => x.SizeMB).ToList();
                        else //si es descendente
                            files_list = files_list.OrderByDescending(x => x.SizeMB).ToList();
                        break;
                    case 4:
                        if (checkBox5.Checked) //si es ascendente
                            files_list = files_list.OrderBy(x => x.LastModified).ToList();
                        else //si es descendente
                            files_list = files_list.OrderByDescending(x => x.LastModified).ToList();
                        break;
                    default:
                        break;
                }

                //Asigna la lista para armar el grid
                dataGridView1.DataSource = files_list;

                //Establece el formato de los tamaños en KB Y mb
                this.dataGridView1.Columns[2].DefaultCellStyle.Format = "0.00#";
                this.dataGridView1.Columns[3].DefaultCellStyle.Format = "0.00#";

                ConstatarFechasGrid(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date);

                dataGridView1.Refresh();
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

                    var file_new = new Archivos(Path.GetFileName(file), dirRoot.Name, size_kb, size_mb, info.LastWriteTime, dirRoot.FullName);
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

                        var file_new = new Archivos(Path.GetFileName(file), dirInf.Name, size_kb, size_mb, info.LastWriteTime, dirInf.FullName);
                        files_list.Add(file_new);
                    }
                }
            }
            return files_list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderb = new FolderBrowserDialog();

            if (folderb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_directorio.Text = folderb.SelectedPath; //Coloca en el label el directorio seleccionado
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = new DateTime(2010, 01, 01); //Establece las fechas predeterminadas
            dateTimePicker2.Value = DateTime.Today;
            checkBox5.Checked = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtFecha1 = dateTimePicker1.Value.Date; //Obtiene el componente fecha del DateTime
            DateTime dtFecha2 = dateTimePicker2.Value.Date;

            if (dtFecha2 < dtFecha1) //Verifica que las fechas sean correctas
            {
                MessageBox.Show("La segunda fecha no puede ser menor a la primera", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker2.Value = DateTime.Now;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtFecha1 = dateTimePicker1.Value.Date; //Obtiene el componente fecha del DateTime
            DateTime dtFecha2 = dateTimePicker2.Value.Date;

            if (dtFecha2 < dtFecha1) //Verifica que las fechas sean correctas
            {
                MessageBox.Show("La segunda fecha no puede ser menor a la primera", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Value = DateTime.Now;
            }
        }

        private void ConstatarFechasGrid(DateTime dtFecha1, DateTime dtFecha2)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToDateTime(row.Cells[4].Value) < dtFecha1 || Convert.ToDateTime(row.Cells[4].Value) > dtFecha2)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red; //Asigna rojo a las filas que no hacen match con las fechas
                }
            }
        }

        private void ConstatarCheckboxes1() //Verifica los checkbox usados para ordenar
        {
            byte cantidad_checkbox = 0;
            if (checkBox1.Checked == true) { cantidad_checkbox++; };
            if (checkBox2.Checked == true) { cantidad_checkbox++; };
            if (checkBox3.Checked == true) { cantidad_checkbox++; };
            if (checkBox4.Checked == true) { cantidad_checkbox++; }; //Controla la cantidad de checkboxes en true
            if (cantidad_checkbox > 1)
            {
                MessageBox.Show("Seleccionar un solo parametro para ordenar la grilla.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ConstatarCheckboxes1();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ConstatarCheckboxes1();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ConstatarCheckboxes1();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ConstatarCheckboxes1();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox6.Checked = !checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            checkBox5.Checked = !checkBox6.Checked;
        }
    }
}

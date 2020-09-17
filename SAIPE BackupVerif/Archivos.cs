using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAIPE_BackupVerif
{
    class Archivos
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public double SizeKB { get; set; }
        public double SizeMB { get; set; }
        public DateTime LastModified { get; set; }

        public Archivos(string name, string folder, double sizeKB, double sizeMB, DateTime lastModified)
        {
            Name = name;
            Folder = folder;
            SizeKB = sizeKB;
            SizeMB = sizeMB;
            LastModified = lastModified;
        }

    }

    
}

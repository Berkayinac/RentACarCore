using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Serilog.ConfigurationModels;

// dosya olduğunda ona ait yol lazım
// elastic olduğunda veri tabanı olduğunda bulunduğu yol lazım connection string lazım
public class FileLogConfiguration
{
    public string FolderPath { get; set; }

    public FileLogConfiguration()
    {
        FolderPath = string.Empty;
    }

    public FileLogConfiguration(string folderPath)
    {
        FolderPath = folderPath;
    }
}

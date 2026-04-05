

using System;
using System.IO;
using System.Linq;

namespace TeamNut.Repositories
{
    internal class DbConfig
    {
        public static string ConnectionString
        {
            get
            {
                
                string directory = AppDomain.CurrentDomain.BaseDirectory;

                
                while (directory != null && !Directory.GetFiles(directory, "*.csproj").Any())
                {
                    directory = Directory.GetParent(directory)?.FullName;
                }

                
                string dbPath = Path.Combine(directory ?? "", "NutData.db");
                return $"Data Source={dbPath}";
            }
        }
    }
}



/*using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNut.Repositories
{
    internal class DbConfig

    {
        public static string ConnectionString
        {
            get
            {
                // Start where the EXE is
                string directory = AppDomain.CurrentDomain.BaseDirectory;

                // Look upwards for the folder containing the .csproj or .sln
                while (directory != null && !Directory.GetFiles(directory, "*.csproj").Any())
                {
                    directory = Directory.GetParent(directory)?.FullName;
                }

                // Combine that "Home" folder with your database name
                string dbPath = Path.Combine(directory ?? "", "NutData.db");
                return $"Data Source={dbPath}";
            }
        }
    }
        //public static string ConnectionString => "Data Source=NutData.db";
        //public static string ConnectionString => $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\NutData.db")}";
        //public static string ConnectionString => $"Data Source=C:\\Users\\trprp\\source\\repos\\UBB-SE-2026-NUT\\TeamNut\\NutData.db";
        //public static string ConnectionString => @"Server=(localdb)\TeamNutInstance;Database=NUTdb;Trusted_Connection=True;TrustServerCertificate=True;";

    }
}
*/
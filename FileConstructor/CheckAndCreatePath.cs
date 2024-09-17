using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeakInterface.FileConstructor
{
    public class CheckAndCreatePath
    {
        public void CheckAndCreatePaths(List<string> filePaths)
        {
            try
            {
                foreach (var filePath in filePaths)
                {
                    string directoryPath = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileName(filePath);

                    Console.WriteLine("Directorio: " + directoryPath);
                    Console.WriteLine("Archivo: " + fileName);

                    if (!Directory.Exists(directoryPath))
                    {
                        Console.WriteLine("El directorio no existe, creándolo...");
                        Directory.CreateDirectory(directoryPath);
                    }
                    else
                    {
                        Console.WriteLine("El directorio ya existe.");
                    }

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("El archivo no existe, creándolo...");
                        using (File.Create(filePath)) { }
                    }
                    else
                    {
                        Console.WriteLine("El archivo ya existe.");
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                // Considera mostrar un mensaje de error en la interfaz de usuario también
            }

        }
    }
}

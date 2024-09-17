using LeakInterface.Global;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeakInterface.CSVRead
{
    public static class GlobalData
    {
        public static Dictionary<string, Dictionary<string, string>> Data { get; private set; }

        static GlobalData()
        {
            Data = new Dictionary<string, Dictionary<string, string>>();
            LoadDataFromCsv(GlobalL.DBSequence);
        }

        private static void LoadDataFromCsv(string path)
        {
            try
            {
                using (var parser = new TextFieldParser(path))
                {
                    parser.SetDelimiters(new string[] { "," });
                    parser.HasFieldsEnclosedInQuotes = true;

                    string[] headers = parser.ReadFields();

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        string zCode = fields[4].Trim();

                        var record = new Dictionary<string, string>();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            if (i != 4)
                            {
                                record[headers[i].Trim()] = fields[i].Trim();
                            }
                        }

                        Data[zCode] = record;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

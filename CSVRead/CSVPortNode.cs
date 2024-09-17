using LeakInterface.Global;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace LeakInterface.CSVRead
{
    public static class CsvDataReader
    {
        public static List<string> ColumnNames { get; private set; }
        public static List<List<string>> DataRows { get; private set; }

        public static void ReadCsvFile(string filePath)
        {
            ColumnNames = new List<string>();
            DataRows = new List<List<string>>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                if (!parser.EndOfData)
                {
                    ColumnNames.AddRange(parser.ReadFields());
                }

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    DataRows.Add(new List<string>(fields));
                }
            }
        }
    }
}

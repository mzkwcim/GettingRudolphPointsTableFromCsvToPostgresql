using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVProject
{
    internal class VariableBuildingSystem
    {
        public static void GettingTextFromCsvToVariables()
        {
            List<string> collumnNames = new List<string>();
            List<string> tableValues = new List<string>();
            string name = "RudolphTableopengirls";
            int adder = 0;
            using (var reader = new StreamReader(Project.filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(",");
                    if (adder == 0)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            collumnNames.Add(values[i].Replace("\"", ""));
                            Console.WriteLine(values[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            tableValues.Add(values[i].Replace("\"", ""));
                        }
                    }

                    adder++;
                }
            }
            DataBaseConnectionSystem.Connection(name, collumnNames, tableValues);
        }
    }
}

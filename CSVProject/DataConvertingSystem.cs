using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVProject
{
    internal class DataConvertionSystem
    {
        public static List<double> ConvertStringToDouble(List<string> tableValues)
        {
            List<double> converted = new List<double>();
            for (int i = 0; i < tableValues.Count; i++)
            {
                string[] list = tableValues[i].Split(":");
                converted.Add((i % 18 == 0) ? Convert.ToDouble(tableValues[i]) : (list[0] != "00") ? (Math.Round(Convert.ToDouble(list[0].Replace("\'", "")), 2) * 60 + Math.Round(Convert.ToDouble(list[1].Replace("\'", "")), 2)) : Math.Round(Convert.ToDouble(list[1].Replace("\'", "")), 2));
            }
            return converted;
        }
    }
}

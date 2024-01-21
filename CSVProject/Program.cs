using CsvHelper;
using Npgsql;
using System.Globalization;
using System.IO;
using CSVProject;

class Project
{
    public static string filePath = "C:\\Users\\mzkwcim\\Desktop\\RudolphTable\\RudolphTable_Girls_Open.csv";
    public static void Main(string[] args)
    {
        VariableBuildingSystem.GettingTextFromCsvToVariables();
    }
}

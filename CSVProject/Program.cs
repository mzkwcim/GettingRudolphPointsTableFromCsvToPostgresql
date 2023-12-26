using CsvHelper;
using Npgsql;
using System.Globalization;
using System.IO;


class Project
{
    static void Main()
    {
        string filePath = "C:\\Users\\mzkwcim\\Desktop\\RudolphTable\\RudolphTable_Girls_Open.csv";
        List<string> listOfFiles = new List<string>();
        List<string> collumnNames = new List<string>();
        List<string> tableValues = new List<string>();
        string name = "RudolphTableopengirls";
        int adder = 0;
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(",");
                if (adder == 0)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        collumnNames.Add(values[i].Replace("\"",""));
                        Console.WriteLine(values[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        tableValues.Add(values[i].Replace("\"",""));
                    }
                }

                adder++;
            }
        }

        Connection(name, collumnNames, tableValues);
    }
    static List<double> ConvertStringToDouble(List<string> tableValues)
    {
        List<double> converted = new List<double>();
        for (int i = 0; i < tableValues.Count; i++)
        {
            string[] list = tableValues[i].Split(":");
            converted.Add((i % 18 == 0) ? Convert.ToDouble(tableValues[i]) : (list[0] != "00") ? (Math.Round(Convert.ToDouble(list[0].Replace("\'", "")), 2) * 60 + Math.Round(Convert.ToDouble(list[1].Replace("\'", "")), 2)) : Math.Round(Convert.ToDouble(list[1].Replace("\'", "")), 2));
        }
        return converted;
    }
    static string CreateTable(string name, List<string> collumnNames)
    {
        string createTableQueryTest = $"CREATE TABLE {name} (" +
                          "ID SERIAL PRIMARY KEY,";
        for (int i = 0; i < collumnNames.Count; i++)
        {
            createTableQueryTest += (i != collumnNames.Count -1) ? $"{collumnNames[i].ToLower()} DOUBLE PRECISION, " : $"{collumnNames[i].ToLower()} DOUBLE PRECISION );";
        }
        Console.WriteLine(createTableQueryTest);
        return createTableQueryTest;
    }
    static string AddValuesToQuery(string name, List<string> collumnNames, List<string> tableValues)
    {
        Console.WriteLine("Jeszcze nie");
        List<double> doubles = ConvertStringToDouble(tableValues);
        Console.WriteLine("tutaj?");
        string AddValues = $"INSERT INTO {name} (";
        for (int j = 0; j < collumnNames.Count; j++)
        {
            AddValues += (j < collumnNames.Count - 1) ? $" {collumnNames[j]}, " : $" {collumnNames[j]} ) VALUES ";
        }
        for (int i = 0; i < 20; i++)
        {
            AddValues += $"( {20-i}, ";
            for (int j = 1; j < 18; j++)
            {
                AddValues += (j < 17) ? $"{Math.Round(doubles[(i * 18) + j],2)}, " : $"{Math.Round(doubles[(i * 18) + j],2)} ";
                Console.WriteLine(doubles[(i * 18) + j]);
            }
            AddValues += (i < 19) ? " ), " : " ); ";
        }
        return AddValues;
    }
    static void Connection(string name, List<string> collumnNames, List<string> tableValues)
    {
        string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=RudolphTable";

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                Console.WriteLine("haha");
                connection.Open();
                if (TableExists(connection, name))
                {
                    using (NpgsqlCommand command2 = new NpgsqlCommand(AddValuesToQuery(name, collumnNames, tableValues), connection))
                    {
                        command2.ExecuteNonQuery();
                        Console.WriteLine("Powodzenie");
                    }
                }
                else
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(CreateTable(name, collumnNames), connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Baza Utworzona");
                    }
                    using (NpgsqlCommand command2 = new NpgsqlCommand(AddValuesToQuery(name, collumnNames, tableValues), connection))
                    {
                        command2.ExecuteNonQuery();
                        Console.WriteLine("Powodzenie");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    static bool TableExists(NpgsqlConnection connection, string tableName)
    {
        using (NpgsqlCommand command = new NpgsqlCommand())
        {
            command.Connection = connection;
            command.CommandText = $"SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = @tableName)";
            command.Parameters.AddWithValue("@tableName", tableName.ToLower());
            object result = command.ExecuteScalar();
            return result != null && (bool)result;
        }
    }
}

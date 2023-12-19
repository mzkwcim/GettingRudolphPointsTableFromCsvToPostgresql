using CsvHelper;
using Npgsql;
using System.Globalization;
using System.IO;


class Project
{
    static void Main()
    {
        string filePath = "C:\\Users\\Laptop\\Desktop\\RudolphTable2023_10YearOldMen.csv";
        List<string> collumnNames = new List<string>();
        List<string> tableValues = new List<string>();
        string name = "RudolphTable10YearsOldBoys";
        int adder = 0;
        using (var reader = new StreamReader(filePath))
        {
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(";");
                if (adder == 0)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        collumnNames.Add(values[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        tableValues.Add(values[i]);
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
            string [] list = tableValues[i].Split(":");
            converted.Add((list.Length != 1) ? (Math.Round(Convert.ToDouble(list[0]), 2) * 60 + Math.Round(Convert.ToDouble(list[1]), 2)) : Math.Round(Convert.ToDouble(list[0]), 2));
        }
        return converted;
    }
    static string CreateTable(string name, List<string> collumnNames)
    {
        string createTableQueryTest = $"CREATE TABLE {name} (" +
                          "ID SERIAL PRIMARY KEY," +
                          " pkt INTEGER, ";                
        for (int  i = 0; i < 17; i++)
        {
            createTableQueryTest += (i != 16) ? ($"{collumnNames[i + 1].ToLower()} DOUBLE PRECISION, ") : ($"\"{collumnNames[i + 1].ToLower()}\" DOUBLE PRECISION ");
        }
        createTableQueryTest += ");";
        Console.WriteLine(createTableQueryTest);
        return createTableQueryTest;
    }
    static string AddValuesToQuery(string name, List<string> collumnNames, List<string> tableValues)
    {
        List<double> doubles = ConvertStringToDouble(tableValues);
        Console.WriteLine("tutaj?");
        string AddValues = $"INSERT INTO {name} (";
        for (int j = 0; j < collumnNames.Count; j++)
        {
            AddValues += (j < collumnNames.Count - 1) ? $"{collumnNames[j]}, " : $"{collumnNames[j]} ) VALUES ";
        }

        for (int i = 0; i < 20; i++)
        {
            AddValues += "(";
            for (int j = 0; j < 18; j++)
            {
                AddValues += (j < 17) ? $"{doubles[(i * 18) + j]}, " : $"{doubles[(i * 18) + j]} ";
                Console.WriteLine(doubles[(i*18)+j]);
            }
            AddValues += ((i  < 19)) ? " ), " : " ); ";
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
            catch(Exception ex)
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


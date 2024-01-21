using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVProject
{
    internal class PostgreSqlQueryBuildingSystem
    {
        public static string CreateTable(string name, List<string> collumnNames)
        {
            string createTableQueryTest = $"CREATE TABLE {name} (" +
                              "ID SERIAL PRIMARY KEY,";
            for (int i = 0; i < collumnNames.Count; i++)
            {
                createTableQueryTest += (i != collumnNames.Count - 1) ? $"{collumnNames[i].ToLower()} DOUBLE PRECISION, " : $"{collumnNames[i].ToLower()} DOUBLE PRECISION );";
            }
            return createTableQueryTest;
        }
        public static string AddValuesToQuery(string name, List<string> collumnNames, List<string> tableValues)
        {
            List<double> doubles = DataConvertionSystem.ConvertStringToDouble(tableValues);
            string AddValues = $"INSERT INTO {name} (";
            for (int j = 0; j < collumnNames.Count; j++)
            {
                AddValues += (j < collumnNames.Count - 1) ? $" {collumnNames[j]}, " : $" {collumnNames[j]} ) VALUES ";
            }
            for (int i = 0; i < 20; i++)
            {
                AddValues += $"( {20 - i}, ";
                for (int j = 1; j < 18; j++)
                {
                    AddValues += (j < 17) ? $"{Math.Round(doubles[(i * 18) + j], 2)}, " : $"{Math.Round(doubles[(i * 18) + j], 2)} ";
                }
                AddValues += (i < 19) ? " ), " : " ); ";
            }
            return AddValues;
        }
        public static bool TableExists(NpgsqlConnection connection, string tableName)
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
}

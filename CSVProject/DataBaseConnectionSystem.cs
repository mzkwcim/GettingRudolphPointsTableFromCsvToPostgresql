using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVProject
{
    internal class DataBaseConnectionSystem
    {
        public static void Connection(string name, List<string> collumnNames, List<string> tableValues)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=RudolphTable";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (PostgreSqlQueryBuildingSystem.TableExists(connection, name))
                    {
                        using (NpgsqlCommand command2 = new NpgsqlCommand(PostgreSqlQueryBuildingSystem.AddValuesToQuery(name, collumnNames, tableValues), connection))
                        {
                            command2.ExecuteNonQuery();
                            Console.WriteLine("Dane Zostaly dodane do bazy");
                        }
                    }
                    else
                    {
                        using (NpgsqlCommand command = new NpgsqlCommand(PostgreSqlQueryBuildingSystem.CreateTable(name, collumnNames), connection))
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine("Baza Utworzona");
                        }
                        using (NpgsqlCommand command2 = new NpgsqlCommand(PostgreSqlQueryBuildingSystem.AddValuesToQuery(name, collumnNames, tableValues), connection))
                        {
                            command2.ExecuteNonQuery();
                            Console.WriteLine("Dane Zostaly dodane do bazy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}

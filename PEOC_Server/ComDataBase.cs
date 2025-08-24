using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PEOC_Server
{
    public class ComDataBase
    {
        string connect_string;
        NpgsqlConnection connect;
        NpgsqlCommand exe_command;

        public ComDataBase(string _con_str)
        {
            this.connect_string = _con_str;

            this.connect = new NpgsqlConnection(connect_string);
            connect.Open();

            exe_command = new NpgsqlCommand();
            exe_command.Connection = connect;
        }

        public static string FormationConnectionString()
        {
            return "Server = localhost; Database = PeocDataBase; Trusted_Connection = True";
        }

        public async Task<IEnumerable<string>> GetMenu()
        {
            exe_command.CommandText = $"SELECT * FROM menu_and_crash WHERE availab='True'";
            NpgsqlDataReader reader = await exe_command.ExecuteReaderAsync();

            var result = new List<string>();

            while (await reader.ReadAsync())
            {
                result.Add(new Teacher(
                    id: (int)reader["id"],
                    first_name: reader[1] as string, // column index can be used
                    last_name: reader.GetString(2), // another syntax option
                    subject: reader["subject"] as string,
                    salary: (int)reader["salary"]));

            }
            return result;
        }
    }
}

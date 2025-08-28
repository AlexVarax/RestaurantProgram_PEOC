using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // методы обеспечивающие работу пользовательского функционала
        public async Task<decimal> CalculatingIncomeForPeriod(DateTime beginning, DateTime end) 
        {
            exe_command.CommandText = $"SELECT * FROM menu_and_crash WHERE availab='True'";
            await exe_command.ExecuteNonQueryAsync();

            return 1;
        }

        public async Task<IEnumerable<string>> GetMenu()
        {
            exe_command.CommandText = $"SELECT SUM(quantity) AS \"Total Quantity\" FROM inventory WHERE product_type = 'Hardware'";
            NpgsqlDataReader reader = await exe_command.ExecuteReaderAsync();

            var result = new List<string>();

            while (await reader.ReadAsync())
            {
                result.Add($"{reader["name"]} -- {reader["price"]} рублей");
            }

            return result;
        }

        //public async Task TableBooking(string guest_name, int table, )
    }
}

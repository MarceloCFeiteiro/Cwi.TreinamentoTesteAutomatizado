using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Cwi.TreinamentoTesteAutomatizado.Controlles
{
    public class PostgreDataBaseController
    {
        private readonly NpgsqlConnection _npgsqlConnection;

        public PostgreDataBaseController(NpgsqlConnection npgsqlConnection)
        {
            try
            {
                _npgsqlConnection = npgsqlConnection;

                if (_npgsqlConnection.State == ConnectionState.Closed)
                {
                    _npgsqlConnection.Open();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Não foi possivel abrir a connecção com o banco de dados.", ex);
            }

        }

        public async Task ClearDataBase(string schema = "public")
        {
            var query = $@"DO
                           $$
                           DECLARE
                               l_stmt VARCHAR;
                               databaseschema VARCHAR:= '{schema}';
                           BEGIN
                               SELECT 'truncate ' || string_agg(format('%I.%I', schemaname, tablename), ',')
                               INTO l_stmt
                               FROM pg_tables
                               WHERE schemaname = databaseschema;
                           
                               EXECUTE l_stmt || ' RESTART IDENTITY';
                               END;
                               $$";
            await _npgsqlConnection.ExecuteAsync(query);
        }

        public async Task<IEnumerable<object>> SelectFrom(string tableName, Table table)
        {
            var selectColumns = string.Join(",", GetColumnsForSelect(table));
            var filterConditions = string.Join(" OR ", GetFilterCondictions(table));

            var query = $"SELECT {selectColumns} FROM {tableName} WHERE {filterConditions}";

            return await _npgsqlConnection.QueryAsync(query);
        }

        public async Task InsertFrom(string tableName, Table table)
        {
            var columns = string.Join(", ", table.Header.Select(x => x).ToArray());
            var values = GetValuesForTable(table);
            string query = string.Empty;

            foreach (var value in values)
            {
                query = $"INSERT INTO {tableName} ({columns}) VALUES {value}";
                await _npgsqlConnection.ExecuteAsync(query);
            }

        }

        private List<string> GetValuesForTable(Table table)
        {
            List<string> values = new();

            for (int row = 0; row < table.Rows.Count; row++)
            {
                var newRow = new List<string>();

                for (int header = 0; header < table.Header.Count; header++)
                {
                    string value = table.Rows[row][header];
                    newRow.Add($"\'{value}\'");
                }
                values.Add($"({string.Join(",", newRow)})");
            }

            return values;
        }
        private string[] GetFilterCondictions(Table table)
        {
            List<string> filters = new();

            for (int row = 0; row < table.Rows.Count; row++)
            {
                var rowCondictions = new List<string>();

                for (int header = 0; header < table.Header.Count; header++)
                {
                    string column = table.Header.ElementAt(header);
                    string value = table.Rows[row][header];

                    rowCondictions.Add($"{column} = {value}");
                }

                filters.Add($"({string.Join(" AND ", rowCondictions)})");
            }

            return filters.ToArray();
        }

        private string[] GetColumnsForSelect(Table table)
        {
            return table.Header.Select(x => $"{x} AS \"{x}\"").ToArray();
        }
    }
}

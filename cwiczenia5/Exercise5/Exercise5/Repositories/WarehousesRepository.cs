using System.Data.SqlClient;

namespace Exercise5.Repositories
{
    public interface IWarehousesRepository
    {
        Task<bool> WarehouseIsExist(int index);
    }
    public class WarehousesRepository : IWarehousesRepository
    {

        private readonly IConfiguration _configuration;
        public WarehousesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> WarehouseIsExist(int index)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"SELECT * FROM Warehouse WHERE idWarehouse = {index}";
                sqlCommand.Parameters.AddWithValue("@1", index);
                await sqlConnection.OpenAsync();
                if (await sqlCommand.ExecuteScalarAsync() is not null)
                {
                    return true;
                }
                return false;
            }
        }
        }
    }


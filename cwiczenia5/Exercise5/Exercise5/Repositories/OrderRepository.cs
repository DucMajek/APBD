using Exercise5.Models;
using System.Data.SqlClient;

namespace Exercise5.Repositories
{
    public interface IOrdersRepository
    {
        Task<bool> checkAmountInOrder(int index);
        Task<bool> checkProductInOrder(int index);
        Task<bool> Fulfill(int index, int amount);
        Task updateFulFilled(int index);

    }
    public class OrderRepository : IOrdersRepository
    {
        private readonly IConfiguration _configuration;
        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> checkAmountInOrder(int index)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"SELECT amount FROM [Order] WHERE IdProduct = {index}";
                sqlCommand.Parameters.AddWithValue("IdProduct", index);
                await sqlConnection.OpenAsync();
                if (await sqlCommand.ExecuteScalarAsync() is not null)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> checkProductInOrder(int index) 
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"SELECT IdOrder FROM [Order] WHERE IdOrder = {index} ";
                sqlCommand.Parameters.AddWithValue("IdOrder", index);
                await sqlConnection.OpenAsync();
                if (await sqlCommand.ExecuteScalarAsync() is not null)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> Fulfill(int index, int amount) 
        {
            using (var Sqlconnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var Sqlcommand = Sqlconnection.CreateCommand();
                Sqlcommand.CommandText = $"Select FulfilledAt from [Order] where IdOrder={index} and amount={amount}";
                await Sqlconnection.OpenAsync();
                if (await Sqlcommand.ExecuteScalarAsync() is null)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task updateFulFilled(int index) 
        {
            using (var Sqlconnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var Sqlcommand = Sqlconnection.CreateCommand();
                Sqlcommand.CommandText = $"Update [Order] set fulFilledAt = GETDATE() where IdOrder = {index}";
                await Sqlconnection.OpenAsync();
                await Sqlcommand.ExecuteScalarAsync();
            }
        }

    }
}

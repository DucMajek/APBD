using Exercise5.Models.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using SqlCommand = System.Data.SqlClient.SqlCommand;
using SqlConnection = System.Data.SqlClient.SqlConnection;

namespace Exercise5.Repositories
{
    public interface IProductWarehousesRepository
    {
        Task <int> AddProducts(RegisterNewProductDTO dto, int index, int IdProduct, int amount);
        Task<int> LastAdded(RegisterNewProductDTO dto, int index, int IdProduct, int amount);
        public void CreateWithProcedure(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt);
    }
    public class ProductWarehousesRepository : IProductWarehousesRepository
    {
        private readonly IConfiguration _configuration;
        public ProductWarehousesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddProducts(RegisterNewProductDTO dto, int index, int IdProduct, int amount)
        {
            using (var Sqlconnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = Sqlconnection.CreateCommand();
                sqlCommand.CommandText = $"SET IDENTITY_INSERT Product_Warehouse ON; INSERT INTO Product_Warehouse (IdProductWarehouse, IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (((SELECT ISNULL(Max(IdProductWarehouse),0) FROM Product_Warehouse) + 1), @IdWarehouse, @IdProduct, (SELECT IdOrder FROM [Order] WHERE IdProduct = {IdProduct} and Amount = {amount}), @Amount, ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount), GETDATE()); SET IDENTITY_INSERT Product_Warehouse OFF";

                sqlCommand.Parameters.AddWithValue("@IdWarehouse", index);
                sqlCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
                sqlCommand.Parameters.AddWithValue("@Amount", amount);
                Sqlconnection.OpenAsync();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = $"SELECT IdProductWarehouse from Product_Warehouse Where IdProduct = {IdProduct} and IdWarehouse = {index} and Amount = {amount}";
                var id = (int)await sqlCommand.ExecuteScalarAsync();
                return id;

            }
        }

        public async void CreateWithProcedure(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt)
        {
            using (var Sqlconnectio = new SqlConnection(_configuration.GetConnectionString("Default")))
            { 
                try { 
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var sqlcommand = new SqlCommand("AddProductToWarehouse", Sqlconnectio);
                        sqlcommand.CommandType = CommandType.StoredProcedure;
                        sqlcommand.Parameters.AddWithValue("IdProduct", IdProduct);
                        sqlcommand.Parameters.AddWithValue("Amount", Amount);
                        sqlcommand.Parameters.AddWithValue("IdWarehouse", IdWarehouse);
                        sqlcommand.Parameters.AddWithValue("CreatedAt", CreatedAt);
                        Sqlconnectio.Open();
                        var result = sqlcommand.ExecuteNonQuery();
                        scope.Complete();
                    }          
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


               
             }
        }

        public async Task<int> LastAdded(RegisterNewProductDTO dto, int index, int IdProduct, int amount)
        {
            using (var Sqlconnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = Sqlconnection.CreateCommand();
                sqlCommand.Parameters.AddWithValue("@IdWarehouse", index);
                sqlCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
                sqlCommand.Parameters.AddWithValue("@Amount", amount);
                Sqlconnection.OpenAsync();
                sqlCommand.CommandText = $"SELECT IdProductWarehouse from Product_Warehouse Where IdProduct = {IdProduct} and IdWarehouse = {index} and Amount = {amount}";      
                var id = (int)await sqlCommand.ExecuteScalarAsync();
                return id;
            }
        }
    }
}

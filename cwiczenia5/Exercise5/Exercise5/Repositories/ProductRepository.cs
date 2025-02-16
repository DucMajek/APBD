using Exercise5.Models;
using Exercise5.Models.DTOs;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Exercise5.Repositories
{
    public interface IProductRepository
    {
        Task<bool> ProductIsExist(int index);
    }
    public class ProductRepository : IProductRepository
    {

        private readonly IConfiguration _configuration;
        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> ProductIsExist(int index)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"select * from Product where IdProduct = {index}";
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


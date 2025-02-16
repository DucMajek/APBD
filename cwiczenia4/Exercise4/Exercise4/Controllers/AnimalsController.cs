using Exercise4.Models;
using Exercise4.Models.DTOs;
using Exercise4.Repositories;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace Exercise4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IAnimalRepository _animalRepository;
        public AnimalsController(IConfiguration configuration, IAnimalRepository animalRepository)
        {
            _configuration = configuration;
            _animalRepository = animalRepository;
        }

        [HttpGet]//api/animals/1?orderBy
        public async Task<IActionResult> GetAnimals(string? orderBy)
        {
            orderBy ??= "name";
            switch (orderBy)
            {
                case "name": break;
                case "description": break;
                case "category": break;
                case "area": break;
                default: orderBy = "name"; break;
            }


            var animals = new List<Animal>();
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"select * from animal order by {orderBy}";
                await sqlConnection.OpenAsync();
                var reader = await sqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    animals.Add(new Animal
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Category = reader.GetString(3),
                        Area = reader.GetString(4)
                    });
                }
            }
            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal(AddAnimal newAnimal)
        {
            if (await _animalRepository.Exists(newAnimal.ID))
            {
                return Conflict();
            }

            await _animalRepository.Create(new Animal
            {
                ID = newAnimal.ID,
                Name = newAnimal.Name,
                Description = newAnimal.Description,
                Category = newAnimal.Category,
                Area = newAnimal.Area,
            });

            return Created($"/api/animals/{newAnimal.ID}", newAnimal);
        }


        [HttpPut("{index}")]
        public async Task<IActionResult> Put(int index, UpdateAnimal newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Incomplete data{ModelState}");
            }

            if (await _animalRepository.Exists(index))
            {

                await _animalRepository.Put(index, newData);
            }
            else
            {
                return NotFound();

            }
            return Ok(newData);
        }


        [HttpDelete("{index}")]
        public async Task<IActionResult> Delete(int index)
        {
           
                if (await _animalRepository.Exists(index))
                {
                    await _animalRepository.Delete(index);
                    
                }
                else
                {
                    return NotFound();
                }

            return Ok($"Delted animal of index: {index}");

        }
    }
}

using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
	                                        c.Id AS CoffeeId,
	                                        c.Title AS CoffeeTitle,
	                                        c.BeanVarietyId,
	                                        b.Id AS BeanId,
	                                        b.Name AS BeanName,
	                                        b.Region AS BeanRegion,
	                                        b.Notes AS BeanNotes
                                        FROM Coffee c
	                                        LEFT JOIN BeanVariety b ON c.BeanVarietyId = b.Id";
                    var reader = cmd.ExecuteReader();
                    var coffees = new List<Coffee>();
                    while (reader.Read())
                    {
                        var coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("CoffeeTitle")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanId")),
                                Name = reader.GetString(reader.GetOrdinal("BeanName")),
                                Region = reader.GetString(reader.GetOrdinal("BeanRegion")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("BeanNotes")) ? null : reader.GetString(reader.GetOrdinal("BeanNotes"))
                            }
                        };
                        coffees.Add(coffee);
                    }
                    reader.Close();
                    return coffees;
                }
            }
        }

        public Coffee Get(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
	                                        c.Id AS CoffeeId,
	                                        c.Title AS CoffeeTitle,
	                                        c.BeanVarietyId,
	                                        b.Id AS BeanId,
	                                        b.Name AS BeanName,
	                                        b.Region AS BeanRegion,
	                                        b.Notes AS BeanNotes
                                        FROM Coffee c
	                                        LEFT JOIN BeanVariety b ON c.BeanVarietyId = b.Id
                                        WHERE c.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    Coffee coffee = null;
                    if (reader.Read())
                    {
                        coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("CoffeeTitle")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanId")),
                                Name = reader.GetString(reader.GetOrdinal("BeanName")),
                                Region = reader.GetString(reader.GetOrdinal("BeanRegion")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("BeanNotes")) ? null : reader.GetString(reader.GetOrdinal("BeanNotes"))
                            }
                        };
                    }
                    reader.Close();
                    return coffee;
                }
            }
        }

        public void Add(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Coffee (Title, BeanVarietyId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@title, @beanVarietyId)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);
                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Coffee
                                        SET Title = @title,
	                                        BeanVarietyId = @beanVarietyId
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);
                    cmd.Parameters.AddWithValue("@id", coffee.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Coffee
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

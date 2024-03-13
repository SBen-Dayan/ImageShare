using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ImageShare.Data
{
    public class ImageRepository
    {
        private readonly string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Image GetImage(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();

            if(!reader.Read())
            {
                return null;
            }

            return new()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Password = (string)reader["Password"],
                Views = (int)reader["Views"]
            };
        }

        public int Insert(Image image)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Images VALUES (@name, @password, 0) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new ("@name", image.Name),
                new("@password", image.Password)
            });
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }

        public void IncrementImageViews(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Images SET Views = Views + 1 WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;

namespace Grocery.Core.Data.Repositories
{
    public class ProductRepository : DatabaseConnection, IProductRepository
    {
        private readonly List<Product> products = [];
        public ProductRepository()
        {
            CreateTable(@"CREATE TABLE IF NOT EXISTS Product (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [Name] NVARCHAR(50) UNIQUE NOT NULL,
                            [Stock] INTEGER NOT NULL,
                            [Shelflife] DATE NOT NULL,
                            [Price] DECIMAL NOT NULL)");
            List<string> insertQueries = [@"INSERT OR IGNORE INTO Product(Name, Stock, Shelflife, Price) VALUES('Melk', 300, '2025-9-25', 0.95)",
                                          @"INSERT OR IGNORE INTO Product(Name, Stock, Shelflife, Price) VALUES('Kaas', 100, '2025-9-30', 7.98)",
                                          @"INSERT OR IGNORE INTO Product(Name, Stock, Shelflife, Price) VALUES('Brood', 400, '2025-9-12', 2.19)",
                                          @"INSERT OR IGNORE INTO Product(Name, Stock, Shelflife, Price) VALUES('Cornflakes', 0, '2025-12-31', 1.48)"];
            InsertMultipleWithTransaction(insertQueries);
        }
        public List<Product> GetAll()
        {
            products.Clear();
            string selectQuery = "SELECT Id, Name, Stock, Shelflife, Price FROM Product";
            OpenConnection();
            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int stock = reader.GetInt32(2);
                    DateOnly shelflife = DateOnly.FromDateTime(reader.GetDateTime(3));
                    Decimal price = reader.GetDecimal(4);
                    products.Add(new(id, name, stock, shelflife, price));
                }
            }
            CloseConnection();
            return products;
        }

        public Product? Get(int id)
        {
            string selectQuery = $"SELECT Id, Name, Stock, Shelflife, Price FROM Product WHERE Id = {id}";
            Product? product = null;
            OpenConnection();
            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string name = reader.GetString(1);
                    int stock = reader.GetInt32(2);
                    DateOnly shelflife = DateOnly.FromDateTime(reader.GetDateTime(3));
                    Decimal price = reader.GetDecimal(4);
                    product = (new(id, name, stock, shelflife, price));
                }
            }
            CloseConnection();
            return product;
        }

        public Product Add(Product item)
        {
            throw new NotImplementedException();
        }

        public Product? Delete(Product item)
        {
            throw new NotImplementedException();
        }

        public Product? Update(Product item)
        {
            int recordsAffected;
            string updateQuery = $"UPDATE Product SET Name = @Name, Stock = @Stock, Shelflife = @Shelflife, Price = @Price WHERE Id = {item.Id};";
            OpenConnection();
            using (SqliteCommand command = new(updateQuery, Connection))
            {
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("Stock", item.Stock);
                command.Parameters.AddWithValue("ShelfLife", item.ShelfLife);
                command.Parameters.AddWithValue("Price", item.Price);

                recordsAffected = command.ExecuteNonQuery();
            }
            CloseConnection();
            return item;
        }
    }
}

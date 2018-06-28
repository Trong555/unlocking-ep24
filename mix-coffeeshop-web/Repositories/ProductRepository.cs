using System;
using System.Collections.Generic;
using System.Security.Authentication;
using mix_coffeeshop_web.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace mix_coffeeshop_web.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        void UpdateProduct(Product data);
        void CreateNewProduct(Product data);
    }

    public class ProductRepository : IProductRepository
    {
        IMongoCollection<Product> Collection { get; set; }

        public ProductRepository(DatabaseConfigurations config)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.MongoDBConnection));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var mongoClient = new MongoClient(settings);
            var database = mongoClient.GetDatabase(config.DatabaseName);
            Collection = database.GetCollection<Product>("products");
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = Collection.Find(it => true).ToList();
            return products;
        }

        public void UpdateProduct(Product data)
        {
            Collection.ReplaceOne(it => it.Id == data.Id, data);
        }

        public void CreateNewProduct(Product data)
        {
            Collection.InsertOne(data);
        }
    }
}
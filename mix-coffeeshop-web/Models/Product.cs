using System;
using MongoDB.Bson.Serialization.Attributes;

namespace mix_coffeeshop_web.Models
{
    public class Product
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public string ThumbURL { get; set; }
        public bool HasStock { get; set; }
        public int Stock { get; set; }
    }
}
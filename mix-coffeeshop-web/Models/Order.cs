using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace mix_coffeeshop_web.Models
{
    public class Order
    {
        [BsonId]
        public string Id { get; set; }
        public IEnumerable<Product> OrderedProducts { get; set; }
        public string Username { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string ReferenceCode { get; set; }
    }
}
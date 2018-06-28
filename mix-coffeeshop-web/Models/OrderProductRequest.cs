using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace mix_coffeeshop_web.Models
{
    public class OrderProductRequest
    {
        public IEnumerable<KeyValuePair<int, int>> OrderedProducts { get; set; }
        public string Username { get; set; }
    }
}
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace mix_coffeeshop_web.Models
{
    public class OrderProductResponse
    {
        public string ReferenceCode { get; set; }
        public string Message { get; set; }
    }
}
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model
{
    public class Book
    {
        public Book()
        {
            Id=ObjectId.GenerateNewId();
        }

        [BsonId]
        [BsonRepresentation((BsonType.ObjectId))]
        public ObjectId Id { get; set; }


        public string Title { get; set; }
        public string ISBN { get; set; }
        public int Price { get; set; }
        public List<Author> Authors { get; set; }
        public Language Language { get; set; }
        public int Year { get; set; }
        public DateTime LastStock { get; set; }
    }
}

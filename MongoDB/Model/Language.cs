using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Model
{
    public class Language
    {
        public Language()
        {
            Id=ObjectId.GenerateNewId();
        }
        [BsonId]
        [BsonRepresentation((BsonType.ObjectId))]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}
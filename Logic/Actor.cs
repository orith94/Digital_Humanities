using MongoDB.Bson.Serialization.Attributes;

namespace WebAppMovies.Logic

{
    public class Actor
    {
        [BsonElement("שם")]
        public string name { get; set; }

        [BsonElement("דמות")]
        public string character { get; set; }
    }
}
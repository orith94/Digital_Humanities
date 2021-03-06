using MongoDB.Bson.Serialization.Attributes;

namespace WebAppMovies.Logic
{
    public class MovieObject
    {
        [BsonElement("שם הסרט")]
        public string name { get; set; }

        [BsonElement("שם")]
        public string name2 { get; set; }

        [BsonElement("שנה")]
        public string year { get; set; }

        [BsonElement("דמות")]
        public string character { get; set; }

        public MovieObject(string name, string year, string character)
        {
            this.name = name;
            this.year = year;
            this.character = character;
            this.name2 = null;
        }
    }
}
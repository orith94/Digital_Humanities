using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppMovies.Logic
{
    public class movie
    {
        [BsonId] //id
        public ObjectId Id { get; set; }

        [BsonElement("שם הסרט")] //id
        public string MovieName { get; set; }

        public dynamic COFID { get; set; }
        public dynamic imdbId { get; set; }

        [BsonElement("בימוי")]
        public dynamic Directed { get; set; }

        [BsonElement("הפקה")]
        public dynamic production { get; set; }

        [BsonElement("הקלטת קול")]
        public dynamic VoiceRecord { get; set; }

        [BsonElement("חברת הפקה")]
        public dynamic ProductionCompany { get; set; }

        public dynamic עריכה { get; set; }

        public dynamic מוזיקה { get; set; }

        [BsonElement("משחק")]
        public Actor[] actors { get; set; }

        [BsonElement("פרסים/פסטיבלים ישראל")]
        public dynamic[] Festivals { get; set; }

        [BsonElement(@"פרסים/פסטיבלים חו" + "\"" + "ל")]
        public dynamic[] Festivalsoutside { get; set; }

        public dynamic צילום { get; set; }

        [BsonElement("שנת יציאה")]
        public double Year { get; set; }

        public dynamic תסריט { get; set; }
        public dynamic תקציר { get; set; }

        public dynamic איפור { get; set; }
        public dynamic בום { get; set; }
        public dynamic ליהוק { get; set; }

        [BsonElement("מספר צופים בישראל")]
        public string WatchNum { get; set; }

        [BsonElement("ניהול תסריט")]
        public dynamic ScriptManeger { get; set; }

        public dynamic סטילס { get; set; }

        public dynamic ע { get; set; }

        [BsonElement("שם אחר/לועזי")]
        public dynamic DiferentName { get; set; }

        public dynamic תקציב { get; set; }

        public string actor_and_character { get; set; }
    }
}
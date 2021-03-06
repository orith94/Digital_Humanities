using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppMovies.Logic
{
    public class Person
    {
        [BsonId] //id
        public ObjectId Id { get; set; }

        [BsonElement("שם")]
        public string Name { get; set; }

        [BsonElement("אזרחות")]
        public string civilian { get; set; }

        [BsonElement("בום")]
        public dynamic boom { get; set; }

        [BsonElement("מין")]
        public string Sex { get; set; }

        [BsonElement("מקום לידה")]
        public string PlaceOfBirth { get; set; }

        [BsonElement("מוזיקה")]
        public dynamic Music { get; set; }

        [BsonElement("עריכה")]
        public dynamic Editing { get; set; }

        [BsonElement("איפור")]
        public dynamic Makeup { get; set; }

        [BsonElement("סטילס")]
        public dynamic Stills { get; set; }

        [BsonElement("צילום")]
        public dynamic Filming { get; set; }

        [BsonElement("הקלטת קול")]
        public dynamic VoiceRecording { get; set; }

        [BsonElement("ליהוק")]
        public dynamic Casting { get; set; }

        [BsonElement("חברת הפקה")]
        public dynamic ProductionAgency { get; set; }

        public dynamic COFID { get; set; }

        [BsonElement("ביוגראפיה")]
        public string Biography { get; set; }

        [BsonElement("בימוי")]
        public string[] Directed { get; set; }

        [BsonElement("הפקה")]
        public string[] Production { get; set; }

        [BsonElement("משחק")]
        public MovieObject[] Acting { get; set; }

        [BsonElement("תאריך לידה")]
        public string DateOfBirth { get; set; }

        public string[] תסריט { get; set; }

        [BsonElement("תפקידים נוספים - תסריט")]
        public dynamic ExtraParts { get; set; }

        [BsonElement("תפקידים נוספים - מוזיקה מקורית")]
        public dynamic OGMusic { get; set; }

        [BsonElement("ע")]
        public dynamic Assitant { get; set; }

        [BsonElement("תפקידים נוספים - עריכה")]
        public dynamic ExtraEdit { get; set; }

        [BsonElement("תפקידים נוספים - בימוי")]
        public dynamic ExtraDirected { get; set; }

        [BsonElement("ניהול תסריט")]
        public dynamic ScriptManagement { get; set; }

        [BsonElement("תאריך מוות")]
        public dynamic Death { get; set; }

        public string imdbId { get; set; }

        public string wdId { get; set; }

        [BsonElement("פרסים/פסטיבלים ישראל")]
        public dynamic festivals { get; set; }

        [BsonElement("תקציר")]
        public dynamic prolog { get; set; }

        [BsonElement(@"פרסים/פסטיבלים חו" + "\"" + "ל")]
        public dynamic[] Festivalsoutside { get; set; }

        [BsonElement("שם אחר/לועזי")]
        public dynamic DiferentName { get; set; }

        public string[] Directed2 { get; set; }

        public string WikiDataUrl { get; set; }
    }
}
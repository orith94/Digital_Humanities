using CsvHelper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace WebAppMovies.Logic
{
    public class Converts
    {
        public static void ConvertDataToJson(List<movie> data, string path)
        {
            string output = JsonConvert.SerializeObject(data);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }

        public static void ConvertDataToJson(List<Person> data, string path)
        {
            string output = JsonConvert.SerializeObject(data);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }

        public static void ConvertDataToJson(string data, string path)
        {
            System.IO.File.WriteAllText(path, data);
        }

        public static void ConvertDataToCsv(List<movie> data, string path)
        {
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteField("MovieName");
                csvWriter.WriteField("Year");
                csvWriter.WriteField("תקציר");
                csvWriter.WriteField("שחקנים");

                csvWriter.NextRecord();
                if (data == null)
                {
                    return;
                }
                foreach (var project in data)
                {
                    csvWriter.WriteField(project.MovieName);
                    csvWriter.WriteField(project.Year);
                    csvWriter.WriteField(project.תקציר);
                    csvWriter.WriteField(project.actor_and_character);
                    csvWriter.NextRecord();
                }

                writer.Flush();
                var result = Encoding.UTF8.GetString(mem.ToArray());
            }
        }

        public static void ConvertDataToCsv(List<Person> data, string path)
        {
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteField("Name");
                //csvWriter.WriteField("Sex");
                //csvWriter.WriteField("Acting");
                csvWriter.WriteField("Biography");
                csvWriter.WriteField("DateOfBirth");
                csvWriter.WriteField("WikiDataUrl");
                csvWriter.WriteField("WikiDataId");
                csvWriter.NextRecord();

                foreach (var project in data)
                {
                    csvWriter.WriteField(project.Name);
                    //csvWriter.WriteField(project.Sex);
                    //csvWriter.WriteField(project.Acting);
                    csvWriter.WriteField("ויקיפדיה - " + project.Biography);
                    csvWriter.WriteField(project.DateOfBirth);
                    csvWriter.WriteField(project.WikiDataUrl);
                    csvWriter.WriteField(project.wdId);
                    csvWriter.NextRecord();
                }

                writer.Flush();
                var result = Encoding.UTF8.GetString(mem.ToArray());
            }
        }
    }
}
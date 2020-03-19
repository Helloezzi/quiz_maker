using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;

namespace ProblemEditor
{
    public class JasonManager
    {
        public static string FilePath = Environment.CurrentDirectory + "";

        public static void Export(ObservableCollection<BaseItem> collecion)
        {
            string json = JsonConvert.SerializeObject(collecion);
            string path = FilePath + $"\\{DateTime.Now.ToString("MMddyyyy_HHmmss")}.json";
            File.WriteAllText(path, json);
        }

        public static ObservableCollection<Chapter> Import(string filePath)
        {
            string json = File.ReadAllText(filePath);

            //JArray jArray = JArray.Parse(json) as JArray;
            //foreach(JObject obj in jArray)
            //{
            //    string children = obj.Value<string>("Children");
                
            //    //Chapter temp = obj.ToObject<Chapter>();

            //    //if (temp.Children.Count > 0)
            //    {
            //        Console.WriteLine("111");
            //    }
                

            //    //if (obj is Chapter chapter)
            //    {
            //        Console.WriteLine("111");
            //    }

            //    //if (obj is Problem problem)
            //    {
            //        Console.WriteLine("111");
            //    }
            //}

            return JsonConvert.DeserializeObject<ObservableCollection<Chapter>>(json);
        }
    }
}

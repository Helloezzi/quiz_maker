using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace QuizMaker
{
    public class JasonManager
    {
        public static string FilePath = Environment.CurrentDirectory + "";

        public static void Export(string filePath, ObservableCollection<BaseItem> collecion)
        {
            List<BaseItem> listTemp = new List<BaseItem>();

            foreach(BaseItem item in collecion)
            {
                listTemp.Add(item);

                if(item.Children.Count > 0)
                {
                    foreach(BaseItem child in item.Children)
                    {
                        listTemp.Add(child);
                    }
                }
            }

            string json = JsonConvert.SerializeObject(listTemp);
            //string path = FilePath + $"\\{DateTime.Now.ToString("MMddyyyy_HHmmss")}.json";
            File.WriteAllText(filePath, json);
        }

        public static ObservableCollection<BaseItem> Import(string filePath)
        {
            string json = File.ReadAllText(filePath);

            ObservableCollection<BaseItem> result = new ObservableCollection<BaseItem>();

            JArray jArray = JArray.Parse(json) as JArray;
            foreach(JObject obj in jArray)
            {
                int type = obj.Value<int>("Type");                
                if (type == (int)ElementType.Chapter)
                {
                    Chapter temp = obj.ToObject<Chapter>();
                    result.Add(temp);
                }
            }

            foreach (JObject obj in jArray)
            {
                int type = obj.Value<int>("Type");
                if (type == (int)ElementType.Quiz)
                {
                    Quiz child = obj.ToObject<Quiz>();

                    foreach(BaseItem parent in result)
                    {
                        if (parent.Id == child.ParentID)
                        {
                            parent.Children.Add(child);
                        }
                    }
                }
            }
            return result;
        }
    }
}

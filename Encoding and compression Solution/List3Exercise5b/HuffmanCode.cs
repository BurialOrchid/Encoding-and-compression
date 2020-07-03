using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace List3Exercise5b
{
    public class HuffmanCode
    {
        public string BinaryCodesName { get; set; }

        public string[] BinaryCodes { get; set; }

        public void CreateHuffmanCodes(string path)
        {
            BinaryCodes = HuffmanCoderMethods.CreateHuffmanCodes(path);
        }

        public bool ImportHuffmanCodes(string path)
        {
            try
            {
                HuffmanCode item = DeserializeItem(path);
                this.BinaryCodes = item.BinaryCodes;
                this.BinaryCodesName = item.BinaryCodesName;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool ExportHuffmanCodes(string path)
        {
            try
            {
                SerializeItem(path);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private void SerializeItem(string fileName)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            StreamWriter sw = new StreamWriter(fileName) { AutoFlush = true };
            JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, this);
        }

        private HuffmanCode DeserializeItem(string fileName)
        {
            StreamReader sw = new StreamReader(fileName);
            string swr = sw.ReadToEnd();
            HuffmanCode huffmanCode = JsonConvert.DeserializeObject<HuffmanCode>(swr);
            return huffmanCode;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixTravelHistorySaver
    {
        string fileName = "MatrixHistory";

        public void Save(List<MatrixEdgeData> history)
        {
            string jsonObject = SerializeWithDataContractJsonSerializer(history);

            string path=Path.Combine(Application.persistentDataPath, fileName + Application.version+".json");

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(jsonObject);
            }
        }
        public List<MatrixEdgeData> Load()
        {
            string path = Path.Combine(Application.persistentDataPath, fileName + Application.version + ".json");

            if (!File.Exists(path))
                return null;

            string jsonObject;
            using (StreamReader reader = new StreamReader(path))
            {
                jsonObject = reader.ReadLine() + "";
            }
            List<MatrixEdgeData> history = DeserializeWithDataContractJsonSerializer<List<MatrixEdgeData>>(jsonObject);
            return history;
        }
        public bool SaveDataExists()
        {
            string path = Path.Combine(Application.persistentDataPath, fileName + Application.version + ".json");
            return File.Exists(path);
        }

        string SerializeWithDataContractJsonSerializer<T>(T obj)
        {
            string serializedObject;
            var jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                jsonSerializer.WriteObject(memoryStream, obj);
                serializedObject = System.Text.Encoding.ASCII.GetString(memoryStream.ToArray());
            }
            return serializedObject;
        }
        T DeserializeWithDataContractJsonSerializer<T>(string jsonObject)
        {
            T DeserializedObject;
            if (jsonObject.Length == 0)
                return default(T);
            var jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonObject)))
            {
                DeserializedObject = (T)jsonSerializer.ReadObject(memoryStream);
            }
            return DeserializedObject;
        }

    }
}

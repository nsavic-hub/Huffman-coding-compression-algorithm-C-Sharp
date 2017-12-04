using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures_Algorithms.Project1
{
    public class DataSerializer<T> where T : IConvertible
    {
        public static void Serialise(string path, Vector<T> vector)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, vector);
            }
        }
        public void deserialise(string path, ref Vector<T> vector)
        {

            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                vector = (Vector<T>)bin.Deserialize(stream);

            }
        }

        /* 
        Loads the file
        Reads all the bytes and then converts it tobase64 because streamreader and binaryreader alone can't read ascii values
        Alternatively you could use "new StreamReader(path, Encoding.GetEncoding("iso-8859-1"), true))" 
        */
        public static void LoadFile(string path, ref Vector<T> vector)
        {
            vector = new Vector<T>();
            var bytes = File.ReadAllBytes(path);
            string output = Convert.ToBase64String(bytes);

            foreach (var value in output)
            {
                vector.Add((T)Convert.ChangeType(value, typeof(T)));
            }
        }

        /*
        Uses a foreach loop to read every value in Vector
        Adds them to a string and writes the string to the file
        Alternatively you could use 'string.Join("", vector);' in place of a foreach loop
        */
        public static void SaveFile(string path, Vector<T> vector)
        {
            string line = "";
            foreach (var vect in vector)
            {
                if(vect.ToString() != "\0")
                    line += vect;
            }

            byte[] output = Convert.FromBase64String(line);
            File.WriteAllBytes(path, output);
        }

        /*
        Reads every line in the encodedinput text file.
        */
        public static void LoadVectorFromTextFile(string path, ref Vector<T> vector)
        {
            vector = new Vector<T>();
            string line = "";

            using (StreamReader sr = new StreamReader(File.Open(path, FileMode.Open)))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == " ")
                    {
                        vector.Add((T)Convert.ChangeType('\0', typeof(T)));
                        continue;
                    }
                    vector.Add((T)Convert.ChangeType(line, typeof(T)));
                }
            }
        }

        /*
        Saves to decoded_input.txt
        */
        public static void SaveVectorToTextFile(string path, Vector<T> vector)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII, 0x10000))
            {
                foreach (var vect in vector)
                {
                    sw.WriteLine(vect);
                }
            }
        }

    }
}

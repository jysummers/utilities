using System;
using System.IO;

namespace Base64DecoderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var contentBytes = Read();
            Write(contentBytes);
        }



        static byte[] Read()
        {
            Console.WriteLine($"Reading file test.txt...");
            using var stream = new FileStream(path: $"_data/test.txt", mode: FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);
            var value = reader.ReadToEnd();

            Console.WriteLine($"Done reading.");
            return Convert.FromBase64String(value);
        }



        static void Write(byte[] contentBytes)
        {
            Console.WriteLine($"Writing file test.xlsx...");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "_output");
            Directory.CreateDirectory(path);
            using var stream = new FileStream(path: $"{path}/test.xlsx", mode: FileMode.Create, FileAccess.Write);
            foreach(var contentByte in contentBytes)
                stream.WriteByte(contentByte);

            Console.WriteLine($"Done writing.");
        }
    }
}

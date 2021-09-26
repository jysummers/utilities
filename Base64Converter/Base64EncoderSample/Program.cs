using System;
using System.IO;

namespace Base64DecoderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] contentBytes;

            contentBytes = Read("_data/valid.xlsx");
            Write("valid.txt", contentBytes);

            contentBytes = Read("_data/invalid.xlsx");
            Write("invalid.txt", contentBytes);
        }



        static byte[] Read(string filename)
        {
            Console.WriteLine($"Reading file {filename}...");
            using var stream = new FileStream(path: filename, mode: FileMode.Open, FileAccess.Read);
            var bytes = ReadToEnd(stream);

            Console.WriteLine($"Done reading.");
            return bytes;
        }



        static void Write(string filename, byte[] contentBytes)
        {
            Console.WriteLine($"Writing file {filename}...");

            var base64 = Convert.ToBase64String(contentBytes);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "_output");
            Directory.CreateDirectory(path);
            using var stream = new FileStream(path: $"{path}/{filename}", mode: FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(stream);

            writer.Write(base64);

            Console.WriteLine($"Done writing.");
        }




        static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}

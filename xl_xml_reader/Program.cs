using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xl_xml_reader
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Starting...");
            Console.WriteLine("Please enter xml path?");

            var path = @"D:\xl_xml_reader\xl_xml_reader\xl_xml_reader\bin\Debug\Comments.xml";//Console.ReadLine();
            xmlProcessor(path);


            Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static async void xmlProcessor(string path)
        {
            int bufferSize = 100 * 1024 * 1024; // 100MB
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                while ((bytesRead = fs.Read(buffer, 0, bufferSize)) > 0)
                {                    
                    Stream st = new MemoryStream(buffer);
                    await xmlReader(st);
                }
            }
        }

        private static async Task xmlReader(Stream stream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //Console.WriteLine("Start Element {0}", reader.Name);
                            if (reader.Name.ToLower() == "row") {
                                //read attribute and insert to db
                                //Console.WriteLine("Id", reader["Id"]);
                                //Console.WriteLine("PostId", reader["PostId"]);
                                //Console.WriteLine("Score", reader["Score"]);
                                Console.WriteLine("Text", reader[3]);
                                //Console.WriteLine("CreateDate", reader["CreateDate"]);
                                //Console.WriteLine("UserDisplayName", reader["UserDisplayName"]);
                                //Console.WriteLine("UserId", reader["UserId"]);
                            }
                            break;
                       
                        default:
                            break;
                    }
                }
            }
        }
    }
}

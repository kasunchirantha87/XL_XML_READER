using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=DESKTOP-RRA8J2R\\SQLEXPRESS;Database=MSC;User Id=sa;Password=sql2012!;Trusted_Connection=true";
                // using the code here...
                conn.Open();
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    while (await reader.ReadAsync())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                //Console.WriteLine("Start Element {0}", reader.Name);
                                if (reader.Name.ToLower() == "row")
                                {
                                    int Id = Convert.ToInt32(reader["Id"]);
                                    int PostId = Convert.ToInt32(reader["PostId"]);
                                    int Score = Convert.ToInt32(reader["Score"]);
                                    string Text = reader["Text"];
                                    string CreationDate = reader["CreationDate"];
                                    string UserDisplayName = reader["UserDisplayName"];
                                    int UserId = Convert.ToInt32(reader["UserId"]);

                                    SqlCommand insertCommand = new SqlCommand("INSERT INTO Comment (Id,PostId,Score,Text,CreationDate,UserDisplayName,UserId)" +
                                        " VALUES (@Id,@PostId,@Score,@Text,@CreationDate,@UserDisplayName,@UserId)", conn);
                                    //read attribute and insert to db
                                    Console.WriteLine(Text);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}

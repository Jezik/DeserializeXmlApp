using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeserializeXmlApp
{
    
    class Program
    {       
        
        static void Main(string[] args)
        {
            string[] filesArray = GetXMLFiles(args[0]);
                        
            FolderMain xData = DeserializeData(filesArray[0]);
            List<Folder> folderList = xData.Folder;

            foreach (var value in folderList)
            {
                Console.WriteLine(value.StrSecond.Operand);
            }
            
            Console.ReadKey();
        }

        // Get files from a directory and add them to a string array
        private static string[] GetXMLFiles(string directoryPath)
        {
            string[] filesArray = null;

            try
            {
                filesArray = Directory.GetFiles(directoryPath, "*.xml");
            }
            catch (IOException e)
            {
                Console.WriteLine("Path is a filename");
                Console.WriteLine(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Access denied");
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Wrong path");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return filesArray;
        }

        // Deserialize data from .xml file
        private static FolderMain DeserializeData(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FolderMain));

            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            FolderMain xmlData = (FolderMain)obj;
            reader.Close();

            return xmlData;
        }
    }
}

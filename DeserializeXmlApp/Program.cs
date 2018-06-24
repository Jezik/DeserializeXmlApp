using System;
using System.IO;
using System.Xml;
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
            string[] filesArray = null;
           
            filesArray = GetXMLFiles(args[0], filesArray);

            foreach (string filename in filesArray)
            {
                Console.WriteLine(filename);
            }

            ReadXMLFile2(filesArray[0]);
            Console.ReadKey();
        }

        // Get files from a directory and add them to a string array
        private static string[] GetXMLFiles(string directoryPath, string[] filesArray)
        {
            
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

        // Read xml file and construct something useful
        private static void ReadXMLFile(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlElement root = xmlDoc.DocumentElement;
            foreach (XmlNode node in root)
            {
                foreach (XmlNode childnode in node)
                {
                    if (childnode.Name == "str" && childnode.Attributes.GetNamedItem("name").Value.Equals("operand"))
                    {
                        Console.WriteLine("Operand: {0}", childnode.Attributes.GetNamedItem("value").Value);
                    }
                    if (childnode.Name == "int")
                    {
                        Console.WriteLine("Mod: {0}", childnode.Attributes.GetNamedItem("value").Value);
                    }
                }
            }
        }

        // Read xml and construct something useful II
        private static void ReadXMLFile2(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList childOperands = root.SelectNodes("//folder/str[@name='operand']");
            foreach (XmlNode operandnode in childOperands)
            {
                Console.WriteLine("Operand: {0}", operandnode.Attributes.GetNamedItem("value").Value);
            }

            XmlNodeList childMods = root.SelectNodes("//folder/int");
            foreach (XmlNode modnode in childMods)
            {
                Console.WriteLine("Mod: {0}", modnode.Attributes.GetNamedItem("value").Value);
            }
        }       
    }
}

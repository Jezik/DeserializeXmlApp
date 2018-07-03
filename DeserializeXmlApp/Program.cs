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
            double result = 0.0;
                        
            FolderMain xData = DeserializeData(filesArray[0]);
            List<Folder> folderList = xData.Folder;
            int[] modArray = new int[folderList.Count];

            for (int i = 0; i < folderList.Count; i++)
            {
                modArray[i] = Int32.Parse(folderList[i].Mod.Value);
            }

            foreach (var value in folderList)
            {
                Operands op = value.StrSecond.Operand;
                int mod = Int32.Parse(value.Mod.Value);
                result = CalculateFileResult(result, mod, op);
            }

            Console.WriteLine("Result is: {0}", result);

            /*for (int i = 0; i < modArray.Length; i++)
            {
                result = CalculateFileResult(modArray[i], );
            }*/
            
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

        // Calculate result for the current file
        private static double CalculateFileResult (double x, int y, Operands op)
        {
            double res;

            switch (op)
            {
                case Operands.add:
                    return res = x + y;                    
                case Operands.divide:
                    return res = x / y;
                case Operands.multiply:
                    return res = x * y;
                case Operands.subtract:
                    return res = x - y;
                default:
                    Console.WriteLine("Wrong data!!!");
                    return 0.0;
            }
        }

        
    }
}

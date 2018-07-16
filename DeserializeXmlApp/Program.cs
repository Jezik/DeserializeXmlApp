using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DeserializeXmlApp
{
    
    class Program
    {       
        
        static void Main(string[] args)
        {
            int calculationNumFinal = 0;
            string fileName = null;
            Stopwatch stopwatch = new Stopwatch();
            string[] filesArray = null;
            Thread[] threadArray = null;
            List<Tuple<string, int>> tupleList = new List<Tuple<string, int>>();
            Tuple<string, int> fileResultTuple = null;

            stopwatch.Start();

            if (args.Length > 0)
            {
                filesArray = GetXMLFiles(args[0]);

                // Read files from array and make calculations
                if (filesArray != null)
                {
                    threadArray = new Thread[filesArray.Length];
                    for (int i=0; i < threadArray.Length; i++)
                    {
                        int copyOfI = i;
                        threadArray[copyOfI] = new Thread(() => { tupleList.Add(fileResultTuple = ExecuteFileOperations(filesArray[copyOfI])); });
                        threadArray[copyOfI].Start();
                    }
                }
                else
                {
                    Console.WriteLine("There is no .xml files in the provided directory.");
                }
            }
            else
            {
                Console.WriteLine("Usage: specify directory as a parameter.");
            }            

            // Waiting for all the threads to finish their work and return result tuples
            if (threadArray != null)
            {
                for (int j=0; j < threadArray.Length; j++ )
                {
                    threadArray[j].Join();
                }
            }

            // Looking for the best file ever
            foreach (var tuple in tupleList)
            {
                if (tuple.Item2 > calculationNumFinal)
                {
                    calculationNumFinal = tuple.Item2;
                    fileName = tuple.Item1;
                }
            }
                      
            // Print to console final result if any
            if (fileName != null)
            {
                Console.WriteLine("====RESULT====");
                Console.WriteLine("File with the biggest number of deserialized <calculation> tags: {0}", fileName);
            }

            // Print to console elapsed time for application
            stopwatch.Stop();
            Console.WriteLine("Elapsed time is: {0} seconds", (stopwatch.ElapsedMilliseconds/1000.0).ToString());

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


        // Executing operations against each file in a fileArray
        private static Tuple<string, int> ExecuteFileOperations(string filePath)
        {            
            FolderMain xData = ValidateDeserializeData(filePath);
            if (xData != null)
            {
                List<Folder> folderList = xData.Folder;
                int calculationNum = 0;
                double result = 0.0;

                foreach (var value in folderList)
                {
                    try
                    {
                        Operands op = value.StrSecond.Operand;
                        int mod = Int32.Parse(value.Mod.Value);
                        result = CalculateFileResult(result, mod, op);
                        calculationNum++;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        continue;
                    }
                }

                Console.WriteLine("File: {0}", filePath);
                Console.WriteLine("Result is: {0}", result);
                Console.WriteLine("Number of calculations: {0}", calculationNum);
                Console.WriteLine();

                var fileOutTuple = new Tuple<string, int>(filePath, calculationNum);

                return fileOutTuple;
            }
            else 
                return null;
        }
        

        // Validation and deseralization
        private static FolderMain ValidateDeserializeData(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FolderMain));
            FolderMain xmlData = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, "XmlSchema.xsd");
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback); 

            XmlReader reader = XmlReader.Create(filePath, settings);
            try
            {
                xmlData = (FolderMain)deserializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine("Debug: " + e.Message);
                Console.WriteLine("Wrong operation type is provided for file {0}. \n" +
                    "Check operand attributes. Only \"add\", \"divide\", \"multiply\" and \"subtract\" operations are possible. \n", filePath);
            }

            void ValidationCallback(object sender, ValidationEventArgs e)
            {
                Console.Write(e.Severity + ": ");
                Console.WriteLine(e.Message);
                Console.WriteLine("File: {0}, Number of line: {1}, Number of position: {2}", filePath, e.Exception.LineNumber, e.Exception.LinePosition);
                Console.WriteLine();
            }

            return xmlData;
        }


        // Calculate result for the current file
        private static double CalculateFileResult(double x, int y, Operands op)
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

using System;
using System.IO;
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

            // Reading directory from arguments
            try
            {                
                filesArray = Directory.GetFiles(args[0], "*.xml");
            }
            catch(IOException e)
            {
                Console.WriteLine("Path is a filename");
                Console.WriteLine(e.StackTrace);
            }
            catch(UnauthorizedAccessException e)
            {
                Console.WriteLine("Access denied");
                Console.WriteLine(e.StackTrace);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Wrong path");
                Console.WriteLine(e.StackTrace);
            }            
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            Console.WriteLine(filesArray.Length);
            Console.ReadKey();
        }
    }
}

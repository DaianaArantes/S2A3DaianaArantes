using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


//Daiana Arantes
//Writen 2018
namespace Assigment3
{
    class ToolFile
    {
        private StreamWriter sw;
        private StreamReader sr;
        private string fileName;

        public void OpenFileToWrite(string path)
        {
            //The reader must be closed before openning the writer
            if(sr != null)
            {
                sr.Close();
                sr = null;
            }


            sw = new StreamWriter(path, append: true);
            sw.AutoFlush = true;
            fileName = path;
        }

        public void OpenFileToRead(string path)
        {
            //The writer must be closed before openning the reader
            if (sw != null)
            {
                sw.Close();
                sw = null;
            }

            sr = new StreamReader(path);
            fileName = path;
        }

        public void WriteFile(Tool tools)
        {
            if(sw == null)
            {
                OpenFileToWrite(fileName);
            }

            sw.WriteLine(
                tools.Transact + ";" +
                tools.PurshaseDate + ";" +
                tools.SerialNumber + ";" +
                tools.ToolPurchased + ";" +
                tools.Price + ";" +
                tools.Quantity + ";" +
                tools.Amount
                );
        }

        public string DisplayData()
        {
            if(sr == null)
            {
                OpenFileToRead(fileName);
            }

            //Header of the data
            string fLine = "#"+ new string(' ', 11) + "Purchase-Date" +
                new string(' ', 8) + "Serial#" + new string(' ', 11) +
                "Manufacturing Tools" + new string(' ', 14) + "Price" +
                new string(' ', 17) + "Qty" + new string(' ', 14) +
                "Amount" + Environment.NewLine;
            
            fLine += new string('-', 145) + Environment.NewLine;
            decimal price = 0;
            int qty = 0;
            decimal amount = 0;
            

            //while not the end of the file, print lines
            while (!sr.EndOfStream)
            {
                string[] toolsArr = sr.ReadLine().Split(';');
                price = Convert.ToDecimal(toolsArr[4]);
                qty = Convert.ToInt32(toolsArr[5]);
                amount = Convert.ToDecimal(toolsArr[6]);

                //format to print file in order
                fLine += String.Format("{0,-4}", toolsArr[0]);
                fLine += "\t" + " " + String.Format("{0,-12}", toolsArr[1]);
                fLine += "\t" + "\t" + " " + String.Format("{0,-5}", toolsArr[2]);
                fLine += "\t" + "\t" + String.Format("{0,-30}", toolsArr[3]);
                fLine += "\t" + "\t" + String.Format("{0:c}", price);
                fLine += "\t" + "\t" + String.Format("{0,6}", qty);
                fLine += "\t" + "\t" + String.Format("{0:c}", amount);

               
                
                fLine += Environment.NewLine;
            }
            fLine += new string('-', 145) + Environment.NewLine;

            sr.BaseStream.Position = 0;

            return fLine;
        }

        public string DeleteFile(string path)
        {
            CloseFile();

            if(!File.Exists(path))
            {
                return "The file "+ path + " does not exist";
            }
            else
            {
                File.Delete(path);

                if(File.Exists(path))
                {
                    return "File was not deleted!";
                }
                else
                {
                    return "File deleted!";
                }
            }
        }

        public void CloseFile()
        {
            if(sw != null)
            {
                sw.Close();
                sw = null;
            }

            if(sr != null)
            {
                sr.Close();
                sr = null;
            }
             
        }

        public string DeleteRecord(int transact)
        {
            if(sr == null)
            {
                OpenFileToRead(fileName);
            }

            Tool tool;
            List<Tool> tools = new List<Tool>();

            string line;
            string[] lineArr;
            while ((line = sr.ReadLine()) != null)
            {
                lineArr = line.Split(';');

                tool = new Tool();
                tool.Transact = Convert.ToInt16(lineArr[0]);
                tool.PurshaseDate = lineArr[1];
                tool.SerialNumber = Convert.ToInt16(lineArr[2]);
                tool.ToolPurchased = lineArr[3];
                tool.Price = Convert.ToDouble(lineArr[4]);
                tool.Quantity = Convert.ToInt16(lineArr[5]);
                tool.Amount = Convert.ToDouble(lineArr[6]);

                if(transact != tool.Transact)
                {
                    tools.Add(tool);
                }
            }

            CloseFile();

            DeleteFile(fileName);

            OpenFileToWrite(fileName);

            foreach (Tool item in tools)
            {
                WriteFile(item);
            }
            return "Record deleted";
        }

        //Chech if transact num is not being used
        public bool IsTransactNumUsed(int transact)
        {
            if (sr == null)
            {
                OpenFileToRead(fileName);
            }

            while (!sr.EndOfStream)
            {
                string[] toolsArr = sr.ReadLine().Split(';');

                if (Convert.ToInt16(toolsArr[0]) == transact)
                {
                    return true;
                }
                else
                {
                    return false;
                }  
            }          
            sr.BaseStream.Position = 0;
            return false;
        } 
    }
}

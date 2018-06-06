using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Assigment3
{
    public partial class Form1 : Form
    {
        int transactNum;
        string date;
        int serialNum;
        string toolPurchased;
        double price;
        int quantity;
        double amount;
        string fileName;

        private ToolFile toolsFile;

        public Form1()
        {
            InitializeComponent();

            toolsFile = new ToolFile();
        }

        private void buttonCreateOpen_Click(object sender, EventArgs e)
        {
            labelMessage.Text = "";
            fileName = textBoxFileName.Text;
            
                if (radioButtonNew.Checked)
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        toolsFile.OpenFileToWrite(fileName);
                        labelMessage.Text = "File " + fileName + " created";

                        buttonWriteRec.Enabled = true;
                        buttonDeleteFile.Enabled = true;
                        buttonDeleteRec.Enabled = true;
                        buttonCloseFile.Enabled = true;
                        buttonDisplay.Enabled = true;
                    }
                    else
                    {
                        labelMessage.Text += "Please insert a file name!";
                    }
                }
                else if (radioButtonOpen.Checked)
                {
                    DialogResult result = openFileDialog1.ShowDialog();

                    if (openFileDialog1.FileName != "")
                    {
                        fileName = openFileDialog1.FileName;
                        textBoxFileName.Text = fileName;

                        toolsFile.OpenFileToRead(fileName);
                        DisplayData();

                        buttonWriteRec.Enabled = true;
                        buttonDeleteFile.Enabled = true;
                        buttonDeleteRec.Enabled = true;
                        buttonCloseFile.Enabled = true;
                        buttonDisplay.Enabled = true;
                    }
                }
        }

        private void buttonWriteRec_Click(object sender, EventArgs e)
        {
            labelMessage.Text = "";
            bool informationValidate = true;
            try
            {
                transactNum = Convert.ToInt16(textBoxTransact.Text);              
                if (transactNum < 1)
                {
                    labelMessage.Text += "Transact Number not Valid \n";
                    informationValidate = false;
                }
                else if(toolsFile.IsTransactNumUsed(transactNum))
                {
                    labelMessage.Text += "Transact number is alread used!";
                    informationValidate = false;
                }                    
            }
            catch (FormatException) {
                labelMessage.Text += "Transact Number not Valid\n";
                informationValidate = false;
            };

            date = textBoxDate.Text;
            Regex rgx = new Regex("^\\d{2}[/]\\d{2}[/]\\d{4}$");
            if(String.IsNullOrEmpty(date))
            {
                labelMessage.Text += "Date cannot be empty!\n";
                informationValidate = false;
            }
            else if (!rgx.IsMatch(date))
            {
                labelMessage.Text += "Date format is dd/mm/yyyy, please insert a proper date!\n";
                informationValidate = false;
            }            
            try
            {
                serialNum = Convert.ToInt16(textBoxSerialNum.Text);
                if (serialNum < 1)
                {
                    labelMessage.Text += "Serial Number must be positive number\n";
                    informationValidate = false;
                }
            }
            catch (FormatException)
            {
                labelMessage.Text += "Serial Number not Valid\n";
                informationValidate = false;
            }
            toolPurchased = textBoxTool.Text;

            if(String.IsNullOrEmpty(toolPurchased))
            {
                labelMessage.Text += "Please insert a Tool Purchased\n";
                informationValidate = false;
            }
            try
            {
                price = Convert.ToDouble(textBoxPrice.Text);

                if (price <= 0)
                {
                    labelMessage.Text += "Price must be positive number\n";
                    informationValidate = false;
                }
            }
            catch (FormatException)
            {
                labelMessage.Text += "Price not Valid\n";
                informationValidate = false;
            }
            try
            {
                quantity = Convert.ToInt16(textBoxQty.Text);

                if (quantity < 1)
                {
                    labelMessage.Text += "Quantity must be positive number\n";
                    informationValidate = false;
                }
            }
            catch (FormatException)
            {
                labelMessage.Text += "Quantity not Valid\n";
                informationValidate = false;
            }
            try
            {
                amount = Convert.ToDouble(textBoxAmount.Text);

                if (amount <= 0)
                {
                    labelMessage.Text += "Amount must be positive number\n";
                    informationValidate = false;
                }
            }
            catch (FormatException)
            {
                labelMessage.Text += "Amount not Valid\n";
                informationValidate = false;
            }

            if (informationValidate)
            {
                Tool tool = new Tool();
                tool.Transact = transactNum;
                tool.PurshaseDate = date;
                tool.SerialNumber = serialNum;
                tool.ToolPurchased = toolPurchased;
                tool.Price = price;
                tool.Quantity = quantity;
                tool.Amount = amount;
                
                toolsFile.WriteFile(tool);

                labelMessage.Text += "1 record written!";
                textBoxAmount.Text = "";
                textBoxDate.Text = "";
                textBoxPrice.Text = "";
                textBoxQty.Text = "";
                textBoxSerialNum.Text = "";
                textBoxTransact.Text = "";
                textBoxTool.Text = "";
            }
        }

        private void buttonDeleteRec_Click(object sender, EventArgs e)
        {
            labelMessage.Text = "";
            try
            {
                transactNum = Convert.ToInt16(textBoxTransact2.Text);

                if (transactNum < 1)
                {
                    labelMessage.Text += "Transact Number not Valid \n";
                    
                }
                else
                {
                    toolsFile.DeleteRecord(transactNum);
                    labelMessage.Text += "Record deleted!";
                    
                }

            }
            catch (FormatException)
            {
                labelMessage.Text += "Transact Number not Valid\n";
                
            };

        }

        //Button to call display method
        private void buttonDisplay_Click(object sender, EventArgs e)
        {
            DisplayData();

        }

        //Button to close a file opened
        private void buttonCloseFile_Click(object sender, EventArgs e)
        {
            toolsFile.CloseFile();
        }

        //Button to delete a file opened
        private void buttonDeleteFile_Click(object sender, EventArgs e)
        {
            fileName = textBoxFileName.Text;

            if (String.IsNullOrEmpty(fileName))
            {
                labelMessage.Text += "Please insert a File Name\n";
                
            }
            else
            {
                toolsFile.DeleteFile(fileName);

                buttonWriteRec.Enabled = false;
                buttonDeleteFile.Enabled = false;
                buttonDeleteRec.Enabled = false;
                buttonCloseFile.Enabled = false;
                buttonDisplay.Enabled = false;
                labelMessage.Text = "";
                richTextBoxDisplay.Text = "";
            }

            
        }

        //Auto fill button, for test purpose
        private void buttonAutoFill_Click(object sender, EventArgs e)
        {
            textBoxTransact.Text = "1";
            textBoxDate.Text = "02/02/2002";
            textBoxSerialNum.Text = "1";
            textBoxTool.Text = "Nail";
            textBoxPrice.Text = "1.11";
            textBoxQty.Text = "2";
            textBoxAmount.Text = "2.22";
            textBoxFileName.Text = "example.txt";
        }

        //method to display data from file into richTextBox
        private void DisplayData()
        {
            labelMessage.Text = "";
            richTextBoxDisplay.Text = "";

            string dataDisplay = toolsFile.DisplayData();
            //Check if there is data to show
            if (dataDisplay == "")
            {
                labelMessage.Text += "There is no data to show!!\n";
                richTextBoxDisplay.Text = "";
            }
            else
            {
                //If there is data, show in the richTextBox
                richTextBoxDisplay.Text += dataDisplay;
                labelMessage.Text += "Itens displayed " + (dataDisplay.Length -
                    dataDisplay.Replace("\n","").Length-3).ToString();
            }
        }
    }
}

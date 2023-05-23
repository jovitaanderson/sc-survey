﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingWinForms
{
    public partial class AdminForm : Form
    {
        private string csvAdminQuestionsFilePath = "admin_questions.csv"; // Path to the CSV file
        private string csvAdminTableFilePath = "admin_table.csv";
        private string csvAdminDownloadFilePath = "admin_download.csv";
        private string csvAdminAdvanceFilePath = "admin_advance.csv";

        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // Customize the appearance of the TabControl for vertical tabs
            tabControl.Alignment = TabAlignment.Left;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(40, 120);
            tabControl.Appearance = TabAppearance.FlatButtons;

            // Form_Load event or constructor
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl1_DrawItem;

            // Load data from the CSV file
            LoadDataFromCSV();
        }

        // DrawItem event handler
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            Graphics g = e.Graphics;
            Rectangle bounds = tabControl.GetTabRect(e.Index);

            /*if (e.Index == tabControl.SelectedIndex)
            {
                using (Brush selectedTabBrush = new SolidBrush(Color.LightBlue))
                {
                    g.FillRectangle(selectedTabBrush, bounds);
                }
            }*/

            using (Brush tabTextBrush = new SolidBrush(Color.Black))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(tabControl.TabPages[e.Index].Text, tabControl.Font, tabTextBrush, bounds, stringFormat);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1 firstForm = new Form1();
            firstForm.Show();
            this.Close();
        }


        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pictureBox.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void ClearCSVFiles()
        {
            File.WriteAllText(csvAdminQuestionsFilePath, string.Empty);
            File.WriteAllText(csvAdminTableFilePath, string.Empty);
            File.WriteAllText(csvAdminDownloadFilePath, string.Empty);
            File.WriteAllText(csvAdminAdvanceFilePath, string.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Clear the contents of the CSV files
            ClearCSVFiles();

            // Get the data from the TextBox
            //Table
            string title = textBoxTitle.Text;
            string x_axis = textBoxXAxis.Text;
            string y_axis = textBoxYAxis.Text;

            // Concatenate the data into a comma-separated string
            string titleDate = string.Format("{0},{1},{2}", title, x_axis, y_axis);

            // Append the data to the CSV file
            using (StreamWriter sw = new StreamWriter(csvAdminTableFilePath, true))
            {
                sw.WriteLine(titleDate);
            }

            //Q1
            string question1 = textBoxQ1.Text;
            string q1a1 = textBox1A1.Text;
            string q1a2 = textBox1A2.Text;
            string q1a3 = textBox1A3.Text;
            string q1a4 = textBox1A4.Text;
            string q1a5 = textBox1A5.Text;

            //Q2
            string question2 = textBoxQ2.Text;
            string q2a1 = textBox2A1.Text;
            string q2a2 = textBox2A2.Text;
            string q2a3 = textBox2A3.Text;
            string q2a4 = textBox2A4.Text;
            string q2a5 = textBox2A5.Text;

            //Q3
            string question3 = textBoxQ3.Text;
            string q3a1 = textBox3A1.Text;
            string q3a2 = textBox3A2.Text;
            string q3a3 = textBox3A3.Text;
            string q3a4 = textBox3A4.Text;
            string q3a5 = textBox3A5.Text;

            // Concatenate the data into comma-separated rows
            string row1 = string.Format("{0},{1},{2},{3},{4},{5}", question1, q1a1, q1a2, q1a3, q1a4, q1a5);
            string row2 = string.Format("{0},{1},{2},{3},{4},{5}", question2, q2a1, q2a2, q2a3, q2a4, q2a5);
            string row3 = string.Format("{0},{1},{2},{3},{4},{5}", question3, q3a1, q3a2, q3a3, q3a4, q3a5);

            // Append the rows to the CSV file
            using (StreamWriter sw = new StreamWriter(csvAdminQuestionsFilePath, true))
            {
                sw.WriteLine(row1);
                sw.WriteLine(row2);
                sw.WriteLine(row3);
            }

            //Download
            DateTime selectedStartDate = dateTimePickerStartDate.Value;
            string startDate = selectedStartDate.ToShortDateString();
            DateTime selectedEndDate = dateTimePickerEndDate.Value;
            string endDate = selectedEndDate.ToShortDateString();

            string downloadData = string.Format("{0},{1}", startDate, endDate);

            // Append the data to the CSV file
            using (StreamWriter sw = new StreamWriter(csvAdminDownloadFilePath, true))
            {
                sw.WriteLine(downloadData);
            }

            //Advance
            string timeOut = textBoxTimeOut.Text;
            string randomQuestions = comboBoxRandomQns.Text;
            string endSurveyText = textBoxEndMessage.Text;
            // Get the image from the PictureBox
            Image image = pictureBox.Image;

            //save as root directory
            if (image != null)
            {
                // Get the current root path of the application
                string rootPath = Directory.GetCurrentDirectory();

                // Specify the directory within the root path to save the image
                string directoryPath = Path.Combine(rootPath, "Images");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Generate a unique file name for the image
                string fileName = "background" + ".png"; 

                // Save the image to the specified directory
                string imagePath = Path.Combine(directoryPath, fileName);
                image.Save(imagePath);

                // Optionally, you can store the imagePath for future reference or use it as needed
            }

            //save as csv
            /*string base64Image = null;
            if (image != null)
            {
                // Convert the image to Base64 string
                base64Image = ConvertImageToBase64(image);
            }*/

            // Concatenate the data into a comma-separated string
            string data = string.Format("{0},{1},{2}", timeOut, randomQuestions, endSurveyText); //, base64Image);

            // Append the data to the CSV file
            using (StreamWriter sw = new StreamWriter(csvAdminAdvanceFilePath, true))
            {
                sw.WriteLine(data);
            }

            MessageBox.Show("Data saved to CSV file.");
        }

        private void LoadDataFromCSV()
        {
            // Load data for the Table tab
            LoadTableData();

            // Load data for the Question1 tab
            LoadQuestionData(tabQuestion1, csvAdminQuestionsFilePath, 0);

            // Load data for the Question2 tab
            LoadQuestionData(tabQuestion2, csvAdminQuestionsFilePath, 1);

            // Load data for the Question3 tab
            LoadQuestionData(tabQuestion3, csvAdminQuestionsFilePath, 2);

            // Load data for the Download tab
            LoadDownloadData();

            // Load data for the Advance tab
            LoadAdvanceData();
        }

        private void LoadTableData()
        {
            if (File.Exists(csvAdminTableFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminTableFilePath);
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');

                    if (values.Length == 3)
                    {
                        textBoxTitle.Text = values[0];
                        textBoxXAxis.Text = values[1];
                        textBoxYAxis.Text = values[2];
                    }
                }
            }
        }

        private void LoadQuestionData(TabPage tabPage, string csvFilePath, int questionIndex)
        {
            if (File.Exists(csvFilePath))
            {
                string[] lines = File.ReadAllLines(csvFilePath);
                if (lines.Length > questionIndex)
                {
                    string[] values = lines[questionIndex].Split(',');

                    TextBox questionTextBox = tabPage.Controls.OfType<TextBox>().FirstOrDefault(c => c.Name.StartsWith("textBoxQ"));
                    TextBox[] answerTextBoxes = tabPage.Controls.OfType<TextBox>().Where(c => c.Name.StartsWith("textBox" + (questionIndex + 1) + "A")).ToArray();

                    if (questionTextBox != null && answerTextBoxes.Length == 5)
                    {
                        questionTextBox.Text = values[0];
                        for (int i = values.Length - 2; i >= 0; i--)
                        {
                            answerTextBoxes[values.Length - 2 - i].Text = values[i + 1];
                        }
                    }
                }
            }
        }


        private void LoadDownloadData()
        {
            if (File.Exists(csvAdminDownloadFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminDownloadFilePath);
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');

                    if (values.Length == 2)
                    {
                        DateTime startDate;
                        DateTime endDate;
                        if (DateTime.TryParse(values[0], out startDate) && DateTime.TryParse(values[1], out endDate))
                        {
                            dateTimePickerStartDate.Value = startDate;
                            dateTimePickerEndDate.Value = endDate;
                        }
                    }
                }
            }
        }

        private void LoadAdvanceData()
        {
            if (File.Exists(csvAdminAdvanceFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminAdvanceFilePath);
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');

                    if (values.Length == 3)
                    {
                        textBoxTimeOut.Text = values[0];
                        comboBoxRandomQns.Text = values[1];
                        textBoxEndMessage.Text = values[2];

                        // Load the image if it exists
                        string rootPath = Directory.GetCurrentDirectory();
                        string directoryPath = Path.Combine(rootPath, "Images");
                        string imagePath = Path.Combine(directoryPath, "background.png");

                        if (File.Exists(imagePath))
                        {
                            Image image = Image.FromFile(imagePath);
                            pictureBox.Image = image;
                        }
                    }
                }
            }
        }
    }
}

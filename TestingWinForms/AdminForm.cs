using System;
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
        private string csvFilePath = "player_answers.csv";

        private string csvPlayerFilePath = "player_answers.csv";

        private int questionsNumber = 3;

        public AdminForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadNumQuestions();
            for (int i = 0; i < questionsNumber - 3; i++)
            {
                addTab();
            }

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

            // Set the desired font size
            Font tabFont = new Font(tabControl.Font.FontFamily, 16, tabControl.Font.Style);


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

                g.DrawString(tabControl.TabPages[e.Index].Text, tabFont, tabTextBrush, bounds, stringFormat);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            TableForm firstForm = new TableForm();
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
            try
            {
                File.WriteAllText(csvAdminQuestionsFilePath, string.Empty);
                File.WriteAllText(csvAdminTableFilePath, string.Empty);
                File.WriteAllText(csvAdminDownloadFilePath, string.Empty);
                File.WriteAllText(csvAdminAdvanceFilePath, string.Empty);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                ClearCSVFiles();
            }
        }

        // Helper method to retrieve the question TextBox based on the question index
        private TextBox GetQuestionTextBox(int questionIndex)
        {
            // Modify this method based on your control naming convention
            // For example, if your TextBoxes are named textBoxQ1, textBoxQ2, etc.
            // you can use the following code:
            string textBoxName = "textBoxQ" + (questionIndex + 1);
            return Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
        }

        // Helper method to retrieve the answer TextBox based on the question and answer indices
        private TextBox GetAnswerTextBox(int questionIndex, int answerIndex)
        {
            // Modify this method based on your control naming convention
            // For example, if your TextBoxes are named textBoxA11, textBoxA12, etc.
            // you can use the following code:
            string textBoxName = "textBoxA" + (questionIndex + 1) + (answerIndex + 1);
            return Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
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
            while (true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(csvAdminTableFilePath, true))
                    {
                        sw.WriteLine(titleDate);
                    }
                    break;
                }
                catch (IOException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

            /*//Q1
            string question1 = textBoxQ1.Text;
            string q1a1 = textBoxA11.Text;
            string q1a2 = textBoxA12.Text;
            string q1a3 = textBoxA13.Text;
            string q1a4 = textBoxA14.Text;
            string q1a5 = textBoxA15.Text;

            //Q2
            string question2 = textBoxQ2.Text;
            string q2a1 = textBoxA21.Text;
            string q2a2 = textBoxA22.Text;
            string q2a3 = textBoxA23.Text;
            string q2a4 = textBoxA24.Text;
            string q2a5 = textBoxA25.Text;

            //Q3
            string question3 = textBoxQ3.Text;
            string q3a1 = textBoxA31.Text;
            string q3a2 = textBoxA32.Text;
            string q3a3 = textBoxA33.Text;
            string q3a4 = textBoxA34.Text;
            string q3a5 = textBoxA35.Text;

            // Concatenate the data into comma-separated rows
            string row1 = string.Format("{0},{1},{2},{3},{4},{5}", question1, q1a1, q1a2, q1a3, q1a4, q1a5);
            string row2 = string.Format("{0},{1},{2},{3},{4},{5}", question2, q2a1, q2a2, q2a3, q2a4, q2a5);
            string row3 = string.Format("{0},{1},{2},{3},{4},{5}", question3, q3a1, q3a2, q3a3, q3a4, q3a5);

            // Append the rows to the CSV file
            while (true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(csvAdminQuestionsFilePath, true))
                    {
                        sw.WriteLine(row1);
                        sw.WriteLine(row2);
                        sw.WriteLine(row3);
                    }
                    break;
                }
                catch (IOException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }*/

            // Store the question and answer data in lists
            List<string> questions = new List<string>();
            List<List<string>> answers = new List<List<string>>();

            // Loop through the questions
            for (int i = 0; i < questionsNumber; i++)
            {
                // Get the question text
                TextBox textBoxQuestion = GetQuestionTextBox(i);
                string question = "";
                if (textBoxQuestion.Text != null)
                    question = textBoxQuestion.Text;
                questions.Add(question);

                // Get the answer texts
                List<string> answerOptions = new List<string>();
                for (int j = 0; j < 5; j++)
                {
                    TextBox textBoxAnswer = GetAnswerTextBox(i, j);
                    string answer = textBoxAnswer.Text;
                    answerOptions.Add(answer);
                }
                answers.Add(answerOptions);
            }

            // Save the questions and answers to the CSV file
            for (int i = 0; i < questions.Count; i++)
            {
                string question = questions[i];
                List<string> answerOptions = answers[i];

                // Concatenate the data into comma-separated rows
                string rowData = string.Format("{0},{1}", question, string.Join(",", answerOptions));

                // Append the rows to the CSV file
                while (true)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(csvAdminQuestionsFilePath, true))
                        {
                            sw.WriteLine(rowData);
                        }
                        break;
                    }
                    catch (IOException ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }


            //string columnHeader = "date,point_x,point_y," + question1 + "," + question2 + "," + question3;
            //UpdatePlayerCSVHeader(columnHeader);

            //Download
            DateTime selectedStartDate = dateTimePickerStartDate.Value;
            string startDate = selectedStartDate.ToShortDateString();
            DateTime selectedEndDate = dateTimePickerEndDate.Value;
            string endDate = selectedEndDate.ToShortDateString();

            string downloadData = string.Format("{0},{1}", startDate, endDate);

            // Append the data to the CSV file
            while (true) {
                try {
                    using (StreamWriter sw = new StreamWriter(csvAdminDownloadFilePath, true))
                    {
                        sw.WriteLine(downloadData);
                    }
                    break;
                }
                catch (IOException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

            //Advance
            string timeOut = textBoxTimeOut.Text;
            if (string.IsNullOrEmpty(timeOut))
            {
                timeOut = "10"; // Assigning default value of 10
            }
            string randomQuestions = comboBoxRandomQns.Text;
            string endSurveyText = textBoxEndMessage.Text;
            // Get the image from the PictureBox
            Image image = pictureBox.Image;
            string imagePath = null;

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
                string fileName = Guid.NewGuid().ToString() + ".png";

                // Save the image to the specified directory
                imagePath = Path.Combine(directoryPath, fileName);
                image.Save(imagePath);
            }

            // Concatenate the data into a comma-separated string
            string data = string.Format("{0},{1},{2},{3}", timeOut, randomQuestions, endSurveyText, imagePath); //, base64Image);

            // Append the data to the CSV file
            while (true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(csvAdminAdvanceFilePath, true))
                    {
                        sw.WriteLine(data);
                    }
                    break;
                }
                catch (IOException ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

            MessageBox.Show("Data saved to CSV file.");
        }

        void UpdatePlayerCSVHeader(String columnHeader) {

            // Read all lines from the CSV file
            string[] lines;
            while (true)
            {
                try
                {
                    lines = File.ReadAllLines(csvPlayerFilePath);
                    break;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            // Replace the first line (header) with the new column names
            lines[0] = columnHeader;

            // Write the updated lines back to the CSV file
            File.WriteAllLines(csvPlayerFilePath, lines);

        }

        private void LoadDataFromCSV()
        {
            // Load data for the Table tab
            LoadTableData();

            /*// Load data for the Question1 tab
            LoadQuestionData(tabQuestion1, csvAdminQuestionsFilePath, 0);

            // Load data for the Question2 tab
            LoadQuestionData(tabQuestion2, csvAdminQuestionsFilePath, 1);

            // Load data for the Question3 tab
            LoadQuestionData(tabQuestion3, csvAdminQuestionsFilePath, 2);
            */

            // Load data for the Download tab
            LoadDownloadData();

            // Load data for the Advance tab
            LoadAdvanceData();

            for (int i = 0; i < questionsNumber; i++)
            {
                TabPage tabPage = tabControl.TabPages[i + 3];
                LoadQuestionData(tabPage, csvAdminQuestionsFilePath, i);
            }
        }

        private void LoadTableData()
        {
                if (File.Exists(csvAdminTableFilePath))
                {
                    string[] lines;
                    while (true)
                    {
                        try
                        {
                            lines = File.ReadAllLines(csvAdminTableFilePath);
                            break;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    } 


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

        private void LoadNumQuestions()
        {
            if (File.Exists(csvAdminQuestionsFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminQuestionsFilePath);
                questionsNumber = lines.Length;
            }
        }

        private void LoadQuestionData(TabPage tabPage, string csvFilePath, int questionIndex)
        {
                if (File.Exists(csvFilePath))
                {
                    string[] lines = File.ReadAllLines(csvFilePath);
                    //questionsNumber = lines.Length;
                    while (true)
                    {
                        try
                        {
                            lines = File.ReadAllLines(csvFilePath);
                            break;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    if (lines.Length > questionIndex)
                    {
                        string[] values = lines[questionIndex].Split(',');

                        TextBox questionTextBox = tabPage.Controls.OfType<TextBox>().FirstOrDefault(c => c.Name.StartsWith("textBoxQ"));
                        TextBox[] answerTextBoxes = tabPage.Controls.OfType<TextBox>().Where(c => c.Name.StartsWith("textBox" + "A" + (questionIndex + 1))).ToArray();

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
                    string[] lines;
                    while (true)
                    {
                        try
                        {
                            lines = File.ReadAllLines(csvAdminDownloadFilePath);
                            break;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

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
                    string[] lines;
                    while (true)
                    {
                        try
                        {
                            lines = File.ReadAllLines(csvAdminAdvanceFilePath);
                            break;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if (lines.Length > 0)
                    {
                        string[] values = lines[lines.Length - 1].Split(',');

                        if (values.Length == 4)
                        {
                            textBoxTimeOut.Text = values[0];
                            comboBoxRandomQns.Text = values[1];
                            textBoxEndMessage.Text = values[2];

                            // Load the image if it exists
                            //string rootPath = Directory.GetCurrentDirectory();
                            //string directoryPath = Path.Combine(rootPath, "Images");
                            string imagePath = Path.Combine(values[3]);

                            if (File.Exists(imagePath))
                            {
                                Image image = Image.FromFile(imagePath);
                                pictureBox.Image = image;
                            }
                        }
                    }
                }

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void tabTable_Click(object sender, EventArgs e)
        {

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            
            // Filter the player answers based on the selected date range
            List<string> filteredPlayerAnswers = FilterPlayerAnswersByDateRange(dateTimePickerStartDate.Value, dateTimePickerEndDate.Value);

            // Check if there are any matching answers
            if (filteredPlayerAnswers.Count > 0)
            {
                // Open a SaveFileDialog to specify the download location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FileName = "playeranswers.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Write the filtered player answers to the selected file
                    File.WriteAllLines(savePath, filteredPlayerAnswers);

                    MessageBox.Show("Player answers downloaded successfully.");
                }
            }
            else
            {
                MessageBox.Show("No player answers found for the selected date range.");
            }
        }

        private List<string> FilterPlayerAnswersByDateRange(DateTime startDate, DateTime endDate)
        {
            List<string> filteredAnswers = new List<string>();

            if (File.Exists(csvFilePath))
            {
                string[] allAnswers;

                while (true) {
                    try
                    {
                        allAnswers = File.ReadAllLines(csvFilePath);
                        break;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                foreach (string answer in allAnswers)
                {
                    // Assuming the date is the first column in the player answers CSV file
                    string[] values = answer.Split(',');

                    if (values.Length > 0 && DateTime.TryParse(values[0], out DateTime answerDate))
                    {
                        if (answerDate >= startDate && answerDate <= endDate)
                        {
                            filteredAnswers.Add(answer);
                        }
                    }
                }
            }

            return filteredAnswers;
        }

        private void btnEndApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addTab()
        {
            string labelText = "";
            if (tabControl.TabPages.Count > 0)
            {
                TabPage lastTabPage = tabControl.TabPages[tabControl.TabPages.Count - 1];
                if (lastTabPage != null)
                {
                    labelText = lastTabPage.Text;
                }
            }

            int newQuestionNumber = 0;
            // Extract the current question number from the previous tab text
            string numberText = labelText.Substring("Question ".Length);
            if (int.TryParse(numberText, out newQuestionNumber))
            {
                // Increment the question number
                newQuestionNumber++;
            }

            TabPage newTabPage = new TabPage();
            newTabPage.Text = "Question " + newQuestionNumber;
            // Copy the controls from the previous tab to the new tab
            if (tabControl.TabPages.Count > 0)
            {
                TabPage previousTabPage = tabControl.TabPages[tabControl.TabPages.Count - 1];
                foreach (Control control in previousTabPage.Controls)
                {
                    Control newControl = (Control)Activator.CreateInstance(control.GetType());
                    newControl.Location = control.Location;
                    newControl.Size = control.Size;

                    if (newControl is TextBox textBox && (control.Name.StartsWith("textBoxQ")))
                    {
                        string previousControlName = control.Name;
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        textBox.Name = newTabName; // Update the Label with the new tab name

                        textBox.Text = ""; // Set the TextBox to blank
                    }
                    else if (newControl is TextBox textBoxA)
                    {
                        string previousControlName = control.Name;
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        textBoxA.Name = newTabName;

                        textBoxA.Text = "";
                    }
                    //This is correct
                    else if (newControl is Label label && (control.Name.StartsWith("labelQ")))
                    {
                        string previousControlName = control.Name;
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        label.Name = newTabName; // Update the Label with the new tab name

                        string previousControlText = control.Text;
                        string newTabText = previousControlText.Substring(0, previousControlText.Length - 1) + (lastDigitValue + 1);
                        label.Text = newTabName;// newTabText;
                    }
                    //this is controls for answers
                    else if (newControl is Label labelA)
                    {
                        string previousControlName = control.Name;
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        labelA.Name = newTabName; // Update the Label with the new tab name

                        labelA.Text = control.Text;
                    }

                    // Copy any other desired properties or event handlers
                    newTabPage.Controls.Add(newControl);
                }
                
            }
            tabControl.TabPages.Add(newTabPage);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addTab();
            questionsNumber++;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           if (tabControl.TabPages.Count > 6)
            {
                TabPage lastTabPage = tabControl.TabPages[tabControl.TabPages.Count - 1];
                tabControl.TabPages.Remove(lastTabPage);
                questionsNumber--;
            }
        }

        private void labelQ2_Click(object sender, EventArgs e)
        {

        }
    }
}

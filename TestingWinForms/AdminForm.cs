using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        private string csvAdminAdvanceFilePath = "admin_advance.csv";
        private string csvFilePath = "player_answers.csv";

        private string csvPlayerFilePath = "player_answers.csv";

        private string dateFormat = "dd/MM/yyyy HH:mm:ss";

        private int[] xAxisIntervalCounts = new int[21];  // Array to store the count of values from 0 to 10 (inclusive) with intervals of 0.5 for roundedFirstValue
        private int[] yAxisIntervalCounts = new int[21]; // Array to store the count of values from 0 to 10 (inclusive) with intervals of 0.5 for roundedSecondValue

        private int questionsNumber = 3;
        int[] responsesCount;
        double[] percentageCount;
        private static int optionsNumber = 8;

        public AdminForm()
        {
            InitializeComponent();
            DoubleBuffered = true;


            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParams = base.CreateParams;
                handleParams.ExStyle = 0x02000000;
                return handleParams;
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            LoadNumQuestions();
            for (int i = 0; i < questionsNumber - 3; i++)
            {
                addTab(); 
            }

            // Customize the appearance of the TabControl for vertical tabs
            tabControl.Alignment = TabAlignment.Left;
            tabControl.SizeMode = TabSizeMode.Normal;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(40, 120);
            tabControl.Appearance = TabAppearance.Normal;

            // Form_Load event or constructor
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl1_DrawItem; 

            // Load data from the CSV file
            LoadDataFromCSV();

            // Make text box wrapped
            EnableTextBoxTextWrapping(tabControl);

            // Make modifications to the form or controls
            this.ResumeLayout();

        }

        // DrawItem event handler
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            Graphics g = e.Graphics;
            Rectangle bounds = tabControl.GetTabRect(e.Index);

            // Set the desired font size
            Font tabFont = new Font(tabControl.Font.FontFamily, 16, tabControl.Font.Style);

            using (Brush tabTextBrush = new SolidBrush(Color.Black))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                if (isSelected)
                {
                    // Customize the selected tab's appearance
                    using (Brush selectedTabBrush = new SolidBrush(Color.LightGray))
                    {
                        g.FillRectangle(selectedTabBrush, bounds);
                        g.DrawString(tabControl.TabPages[e.Index].Text, tabFont, tabTextBrush, bounds, stringFormat);
                    }
                }
                else
                {
                    // Keep the rest of the tabs uncolored
                    g.DrawString(tabControl.TabPages[e.Index].Text, tabFont, tabTextBrush, bounds, stringFormat);
                }
            }
        }

        private void EnableTextBoxTextWrapping(TabControl tabControl)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Multiline = true;
                        textBox.WordWrap = true;
                        textBox.ScrollBars = ScrollBars.Vertical; // Enable vertical scrolling

                        textBox.TextChanged += (sender, e) => TextBox_TextChanged(sender, e, tabPage); // Attach event handler
                        textBox.KeyPress += TextBox_KeyPress;
                    }
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Cancel the Enter key press event
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e, TabPage tabPage)
        {
            TextBox textBox = (TextBox)sender;
            // Adjust the height of the text box based on the preferred height and the fixed height
            textBox.Height = TextRenderer.MeasureText("A", textBox.Font).Height * 2; 
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
            string textBoxName = "textBoxQ" + (questionIndex + 1);
            return Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
        }

        private ComboBox GetQuestionComboBox(int questionIndex)
        {
            string comboBoxName = "comboBox" + (questionIndex + 1);
            return Controls.Find(comboBoxName, true).FirstOrDefault() as ComboBox;
        }

        // Helper method to retrieve the answer TextBox based on the question and answer indices
        private TextBox GetAnswerTextBox(int questionIndex, int answerIndex)
        {
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
            // Convert the Color to a string representation
            string existingColour = ColorTranslator.ToHtml(btnExisColour.BackColor);
            string newColour = ColorTranslator.ToHtml(btnSelPointColour.BackColor);

            // Concatenate the data into a comma-separated string
            string titleDate = string.Format("{0},{1},{2},{3},{4}", title, x_axis, y_axis, existingColour, newColour);

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

            // Store the question and answer data in lists
            List<string> questions = new List<string>();
            List<List<string>> answers = new List<List<string>>();
            List<string> types = new List<string>();

            // Loop through the questions
            for (int i = 0; i < questionsNumber; i++)
            {
                // Get the question text
                TextBox textBoxQuestion = GetQuestionTextBox(i);
                ComboBox comboBox = GetQuestionComboBox(i);
                
                if (!string.IsNullOrWhiteSpace(textBoxQuestion.Text))
                {
                    questions.Add(textBoxQuestion.Text);

                    // Get the answer texts
                    List<string> answerOptions = new List<string>();
                    for (int j = 0; j < optionsNumber; j++)
                    {
                        TextBox textBoxAnswer = GetAnswerTextBox(i, j);
                        string answer = textBoxAnswer.Text;
                        answerOptions.Add(answer);
                    }
                    answers.Add(answerOptions);

                    //Add type of questions
                    if (comboBox.Text == "")
                    {
                        // No item selected, set default value
                        types.Add("MRQ");
                    }
                    else
                    {
                        types.Add(comboBox.Text);
                    }
                }
                    
            }

            // Save the questions and answers to the CSV file
            for (int i = 0; i < questions.Count; i++)
            {
                string question = questions[i];
                string type = types[i];
                List<string> answerOptions = answers[i];

                // Concatenate the data into comma-separated rows
                string rowData = string.Format("{0},{1},{2}", question, type, string.Join(",", answerOptions));

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

            UpdatePlayerCSVHeader();


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

        void UpdatePlayerCSVHeader() {

            string columnHeader = "date,point_x,point_y,";

            for (int i = 1; i <= questionsNumber; i++)
            {
                string questionX = "question" + i;
                columnHeader += questionX + ",";
            }

            columnHeader = columnHeader.TrimEnd(',');

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

                        if (values.Length == 5)
                        {
                            textBoxTitle.Text = values[0];
                            textBoxXAxis.Text = values[1];
                            textBoxYAxis.Text = values[2];
                            btnExisColour.BackColor = ColorTranslator.FromHtml(values[3]);
                            btnSelPointColour.BackColor = ColorTranslator.FromHtml(values[4]);
                        }
                    }
                }

        }

        private void LoadNumQuestions()
        {
            if (File.Exists(csvAdminQuestionsFilePath))
            {

                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(csvAdminQuestionsFilePath);
                        break;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                questionsNumber = lines.Length;
            }
        }

        private void LoadQuestionData(TabPage tabPage, string csvFilePath, int questionIndex)
        {
                if (File.Exists(csvFilePath))
                {
                    string[] lines = File.ReadAllLines(csvFilePath);
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
                        ComboBox questionTypeComboBox = tabPage.Controls.OfType<ComboBox>().FirstOrDefault(c => c.Name.StartsWith("comboBox"));

                    if (questionTextBox != null && answerTextBoxes.Length == optionsNumber && questionTypeComboBox != null)
                        {
                            questionTextBox.Text = values[0];
                            for (int i = values.Length - 3; i >= 0; i--)
                            {
                                answerTextBoxes[values.Length - 3 - i].Text = values[i + 2];
                            }
                            questionTypeComboBox.Text = values[1];
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


        private void btnDownload_Click(object sender, EventArgs e)
        {

            DateTime selectedDate = dateTimePickerStartDate.Value.Date; // Get the selected date without the time portion
            DateTime startDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0); // Set the time as 00:00:00 (midnight)
            DateTime endDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 23, 59, 59); // Set the time as 23:59:59 (end of the day)


            // Filter the player answers based on the selected date range
            List<string> filteredPlayerAnswers = FilterPlayerAnswersByDateRange(startDateTime, endDateTime);
            var combinedFilteredPlayerAnswers = separateData(filteredPlayerAnswers);
            List<string> pointsFilterePlayerAnswers = combinedFilteredPlayerAnswers.Item1;
            List<string> reponsesFilterePlayerAnswers = combinedFilteredPlayerAnswers.Item2;

            // Calculate total reponse by time
            List<int> dateColumn = getDateColumn(filteredPlayerAnswers);
            List<string> hourCount = getHourCount(dateColumn);

            //Calculation for conslidated points data
            List<string> roundedFilterePlayerAnswers = roundDataPoints(pointsFilterePlayerAnswers);
            List<string> consolidatedPointAnswers = countAndAnalyzePointsInterval(roundedFilterePlayerAnswers); //Vertical
                                                                                                      //List<string> consolidatedPointAnswersF2 = consolidatedPointsFormat(filteredPlayerAnswers.Count); //Horizontal

            // Calculation for conslidated reponses data
            responsesCount = new int[optionsNumber];
            List<string[]> responseSplitByQuestion = splitResponseByQuestion(reponsesFilterePlayerAnswers);
            List<string> consolidatedResponse = countAndAnalyzeResponse(responseSplitByQuestion);

            //Combine the csv file into one
            List<string> combinedConslidatedData = combinedData(startDateTime, endDateTime, consolidatedPointAnswers, consolidatedResponse, hourCount);

            // Check if there are any matching answerss
            if (filteredPlayerAnswers.Count > 0)
            {
                // Open a SaveFileDialog to specify the download location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Format the date and time as strings
                string dateString = currentDate.ToString("dd-MM-yyyy");
                string timeString = currentDate.ToString("HHmmss");

                saveFileDialog.FileName = $"{dateString}_{timeString}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Write the filtered player answers to the selected file
                    File.WriteAllLines(savePath, combinedConslidatedData);

                    MessageBox.Show("Player answers downloaded successfully.");
                }
            }
            else
            {
                MessageBox.Show("No player answers found for the selected date range.");
            }
        }

        private List<string> getHourCount(List<int> hours)
        {
            List<string> hourCountCSV = new List<string>();
            hourCountCSV.Add($"Hour Interval,Frequency");

            Dictionary<int, int> hourFrequency = new Dictionary<int, int>();

            // Initialize the hourFrequency dictionary with all hours from 0 to 24 and set their count as 0
            for (int i = 0; i <= 23; i++)
            {
                hourFrequency[i] = 0;
            }

            // Count the frequency of each hour in the hours list
            foreach (int hour in hours)
            {
                hourFrequency[hour]++;
            }

            int totalCount = 0;
            // Print the hour and its corresponding frequency
            foreach (KeyValuePair<int, int> pair in hourFrequency)
            {
                if (pair.Key < 9)
                {
                    hourCountCSV.Add($"0{pair.Key}:00-0{pair.Key + 1}:59,{pair.Value}");
                }
                else if (pair.Key == 9) {
                    hourCountCSV.Add($"0{pair.Key}:00-{pair.Key + 1}:59,{pair.Value}");
                }
                else
                {
                    hourCountCSV.Add($"{pair.Key}:00-{pair.Key + 1}:59,{pair.Value}");
                }
                totalCount += pair.Value;
                
            }
            hourCountCSV.Add($"Total,{totalCount}");
            return hourCountCSV;

        }

        private List<int> getDateColumn(List<string> filteredPlayerAnswers)
        {
            List<int> dateColumn = new List<int>();

            foreach (string item in filteredPlayerAnswers)
            {
                string[] values = item.Split(',');
                values[0] = values[0].TrimStart('\'');

                if (values.Length > 0 && DateTime.TryParseExact(values[0], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)) {
                    dateColumn.Add(date.Hour);

                }

            }

            return dateColumn;
        }

        private List<string> combinedData(DateTime startDateTime, DateTime endDateTime, List<string> consolidatedPointAnswers, List<string> consolidatedResponse, List<string> hourCount)
        {
            List<string> combinedConslidatedData = new List<string>();
            combinedConslidatedData.Add($"Date selected: {startDateTime} to {endDateTime}");
            combinedConslidatedData.Add("");
            combinedConslidatedData.AddRange(consolidatedPointAnswers);
            combinedConslidatedData.Add("");
            combinedConslidatedData.AddRange(consolidatedResponse);
            combinedConslidatedData.Add("");
            combinedConslidatedData.AddRange(hourCount);
            return combinedConslidatedData;

        }

        private List<string> countAndAnalyzeResponse(List<string[]> responseList) {

            int numOfResponses;

            if (responseList.Count >= 1)
            {
                numOfResponses = responseList[0].Length;
            }
            else {
                numOfResponses = 0;
            }
           

            List<string> conslidatedResponse = new List<string>();
            conslidatedResponse.Add("Response count for each question option");

            List<string> conslidatedPercentageResponse = new List<string>();
            conslidatedPercentageResponse.Add("Percentage of each response per question over total responses");

            string csvColumnHeader = "Question,optionA,optionB,optionC,optionD,optionE,optionF,optionG,optionH";
            conslidatedResponse.Add($"{ csvColumnHeader},TotalAnswers");
            conslidatedPercentageResponse.Add(csvColumnHeader);
            int qnNumberCSV = 1;
            foreach (string[] response in responseList)
            {

                string[] values;
                int a = response.Length;

                responsesCount = new int[optionsNumber];
                percentageCount = new double[optionsNumber];

                for (int i = 0; i < response.Length; i++)
                {
                    if (response[i] != null)
                    {
                        //string wee = response[i];
                        values = response[i].Split(';');

                        for (int j = 0; j < optionsNumber; j++)
                        {
                            if (!string.IsNullOrEmpty(values[j]) && values[j] != "" && values[j] != " ")
                            {
                                responsesCount[j]++;
                            }
                        }
                    }
                }

                int totalAnswers = 0;
                for (int i = 0; i < responsesCount.Length; i++) { 
                    totalAnswers += responsesCount[i];
                }

                string finalCountPerQuestion = string.Join(",", responsesCount);
                conslidatedResponse.Add($"Question{qnNumberCSV},{finalCountPerQuestion},{totalAnswers}");

                for (int i = 0; i < responsesCount.Length; i++)
                {
                    percentageCount[i] = ((double)responsesCount[i] / numOfResponses) * 100;
                }

                string percentagePerQuestion = string.Join("%,", percentageCount) + "%";
                conslidatedPercentageResponse.Add($"Question{qnNumberCSV},{percentagePerQuestion}");
                
                qnNumberCSV++;
            }
            qnNumberCSV = 1;

            conslidatedResponse.Add("");
            conslidatedResponse.AddRange(conslidatedPercentageResponse);

            return conslidatedResponse;
        }

        private (List<string>, List<string>) separateData(List<string> filteredPlayerAnswers) {
            // Assuming you have a List<string> called dataList containing the comma-separated values

            List<string> points = new List<string>();
            List<string> responses = new List<string>();

            foreach (string item in filteredPlayerAnswers)
            {
                string[] values = item.Split(',');

                if (values.Length >= 3)
                {
                    string point = values[1].Trim() + "," + values[2].Trim();
                    points.Add(point);
                }

                if (values.Length >= 4)
                {
                    string response = "";

                    for (int i = 3; i < values.Length; i++)
                    {
                        response += values[i].Trim() + ", ";

                    }

                    //remove traiing ", "
                    response = response.TrimEnd(' ');
                    response = response.TrimEnd(',');

                    responses.Add(response);
                }

            }

            return (points, responses);
        }

        private List<string> roundDataPoints(List<string> pointsFilterePlayerAnswers)
        {
            List<string> roundedPoints = new List<string>();

            foreach (string item in pointsFilterePlayerAnswers)
            {
                string[] values = item.Split(',');
                if (values.Length >= 2)
                {
                    if (float.TryParse(values[0], out float firstValue) && float.TryParse(values[1], out float secondValue))
                    {
                        float roundedFirstValue = RoundToNearestHalf(firstValue);
                        float roundedSecondValue = RoundToNearestHalf(secondValue);

                        string roundedItem = roundedFirstValue.ToString() + ", " + roundedSecondValue.ToString();
                        roundedPoints.Add(roundedItem);
                    }
                }
            }
            return roundedPoints;
        }

        private List<string> countAndAnalyzePointsInterval(List<string> roundedPoints) {

            int numOfResponses = roundedPoints.Count;
            double[] percentageCountX = new double[21]; 
            double[] percentageCountY = new double[21];
            float totalX = 0;
            float totalY = 0;

            //List<string> pointsConslidatedFormat = new List<string>();

            //pointsConslidatedFormat.Add("Response count for each interval");
            string csvColumnHeader = "Answer,x-axis,y-axis";
            //pointsConslidatedFormat.Add(csvColumnHeader);

            //List<string> conslidatedPercentagePoint = new List<string>();
            //conslidatedPercentagePoint.Add(csvColumnHeader);
            //conslidatedPercentagePoint.Add("Percentage of each response per interval");


            List<string> combinedData = new List<string>(); //seprataed by two columns
            combinedData.Add("Response count for each interval,,,,,Percentage of each response per interval");
            combinedData.Add($"{csvColumnHeader},,,{csvColumnHeader}");

            foreach (string item in roundedPoints)
            {
                string[] values = item.Split(',');
                if (values.Length >= 2)
                {
                    if (float.TryParse(values[0], out float roundedFirstValue) && float.TryParse(values[1], out float roundedSecondValue))
                    {
                        int x_axis = (int)(roundedFirstValue * 2);
                        int y_axis = (int)(roundedSecondValue * 2);
                        totalX += roundedFirstValue;
                        totalY += roundedSecondValue;
                        xAxisIntervalCounts[x_axis]++;
                        yAxisIntervalCounts[y_axis]++;
                    }
                }
            }

            //Calculate % of responses
            for (int i = 0; i < 21; i++)
            {
                percentageCountX[i] = ((double)xAxisIntervalCounts[i] / numOfResponses) * 100;
                percentageCountY[i] = ((double)yAxisIntervalCounts[i] / numOfResponses) * 100;
            }

            // Output the counts
            for (int i = 0; i < 21; i++)
            {
                float value = i / 2f;
                //pointsConslidatedFormat.Add($"{value},{xAxisIntervalCounts[i]},{yAxisIntervalCounts[i]}");
                //conslidatedPercentagePoint.Add($"{value},{percentageCountX[i]},{percentageCountY[i]}%");
                combinedData.Add($"{value},{xAxisIntervalCounts[i]},{yAxisIntervalCounts[i]},,,{value},{percentageCountX[i]}%,{percentageCountY[i]}%");

            }
            float avgX = totalX / numOfResponses;
            float avgY = totalY / numOfResponses;
            //pointsConslidatedFormat.Add($"Total,{roundedPoints.Count},{roundedPoints.Count}");
            //conslidatedPercentagePoint.Add($"Total,{roundedPoints.Count},{roundedPoints.Count}");
            combinedData.Add($"Average,{avgX},{avgY}");
            combinedData.Add($"Total,{roundedPoints.Count},{roundedPoints.Count},,,Total,{roundedPoints.Count},{roundedPoints.Count}");

            //pointsConslidatedFormat.Add("");
            //pointsConslidatedFormat.AddRange(conslidatedPercentagePoint);

            return combinedData;

        }


        // Helper method to round to the nearest 0.5 decimal place
        private float RoundToNearestHalf(float value)
        {
            return (float)Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
        }

        private List<string[]> splitResponseByQuestion(List<string> responseFilterePlayerAnswers)
        {
            List<string[]> separatedArrays = new List<string[]>();
            string[] values;
            int finalNumQuestion = 0;

            // Split the responses and group the values by position
            for (int i = responseFilterePlayerAnswers.Count - 1; i >= 0; i--)
            {// Check if it's the first response, initialize the arrays accordingly
                values = responseFilterePlayerAnswers[i].Split(',');
                if (i == (responseFilterePlayerAnswers.Count - 1))
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        separatedArrays.Add(new string[responseFilterePlayerAnswers.Count]);
                    }
                    finalNumQuestion = values.Length;
                }

                // Store the values in their respective arrays
                for (int j = 0; j < finalNumQuestion; j++)
                {
                    if (j < values.Length)
                    {
                        separatedArrays[j][i] = values[j];
                    }
                    else
                    {
                        separatedArrays[j][i] = ";;;;;;;;";
                    }
                }
            }
            
            return separatedArrays;
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

                    values[0] = values[0].TrimStart('\'');

                    if (values.Length > 0 && DateTime.TryParseExact(values[0], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime answerDate))
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

        private List<string> AllPlayerAnswers(DateTime startDate, DateTime endDate)
        {
            List<string> AllAnswers = new List<string>();

            if (File.Exists(csvFilePath))
            {
                string[] allAnswers;

                while (true)
                {
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
                    AllAnswers.Add(answer);
                }
            }

            AllAnswers.RemoveAt(0);
            return AllAnswers;
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

                    string previousControlName = control.Name;

                    // Get the last tab index from the previous tab
                    int tabIndexOffset = previousTabPage.TabIndex + 1;

                    //textBox
                    if (newControl is TextBox textBox && (control.Name.StartsWith("textBoxQ")))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        textBox.Name = newTabName; // Update the Label with the new tab name

                        textBox.Text = ""; // Set the TextBox to blank
                    }
                    else if (newControl is TextBox textBoxA)
                    {
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        textBoxA.Name = newTabName;

                        textBoxA.Text = "";
                    }
                    //label
                    else if (newControl is Label label && (control.Name.StartsWith("labelQ")))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        label.Name = newTabName; // Update the Label with the new tab name

                        string previousControlText = control.Text;
                        string newTabText = previousControlText.Substring(0, previousControlText.Length - 1) + (lastDigitValue + 1);
                        label.Text = newTabText;
                    }
                    //this is controls for answers
                    else if (newControl is Label labelA && control.Name.StartsWith("labelA"))
                    {
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        labelA.Name = newTabName; // Update the Label with the new tab name

                        labelA.Text = control.Text;
                    }
                    else if (newControl is Label labelT && control.Name.StartsWith("label"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newLabelTypeName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        labelT.Name = newLabelTypeName; // Update the Label with the new tab name

                        labelT.Text = control.Text;
                    }

                    else if (newControl is Button button && control.Name.StartsWith("btnClear"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newButtonName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        button.Name = newButtonName;
                        button.Text = control.Text;
                        button.Click += ClearButton_Click;
                    }
                    else if (newControl is ComboBox comboBox && control.Name.StartsWith("comboBox"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newButtonName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        comboBox.Name = newButtonName;
                        comboBox.Text = "MRQ"; //Default is pick MRQ

                        // Copy items from the previous ComboBox
                        if (control is ComboBox previousComboBox)
                        {
                            foreach (var item in previousComboBox.Items)
                            {
                                comboBox.Items.Add(item);
                            }
                        }
                    }

                    newControl.TabIndex = control.TabIndex + tabIndexOffset;
                    // Copy any other desired properties or event handlers
                    newTabPage.Controls.Add(newControl);
                }
                
            }
            // Add auto-scrolling to the TabControl
            newTabPage.AutoScroll = true;

            tabControl.TabPages.Add(newTabPage);


            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.Font = new Font(tabPage.Font.FontFamily, 16, FontStyle.Regular);
                foreach (Control control in tabPage.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    }
                    if (control is ComboBox comboBox)
                    {
                        comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    }
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TabPage tabPage = (TabPage)button.Parent; // Get the parent tab page

            // Clear the text of all textboxes within the tab page
            foreach (Control control in tabPage.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = "";
                }
                if (control is ComboBox comboBox)
                {
                    comboBox.Text = "";
                }
            }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnClear3_Click(object sender, EventArgs e)
        {
            ClearButton_Click(sender, e);
        }

        private void btnClear2_Click(object sender, EventArgs e)
        {
            ClearButton_Click(sender, e);
        }

        private void btnClear1_Click(object sender, EventArgs e)
        {
            ClearButton_Click(sender, e);
        }

        private void btnColourPicker_Click(object sender, EventArgs e)
        {
            // Create a new instance of the ColorDialog
            ColorDialog colorDialog = new ColorDialog();

            // Show the ColorDialog and capture the result
            DialogResult result = colorDialog.ShowDialog();

            // Check if the user clicked the OK button in the ColorDialog
            if (result == DialogResult.OK)
            {
                // Retrieve the selected color
                Color selectedColor = colorDialog.Color;

                // Do something with the selected color
                // For example, set the background color of a control
                btnExisColour.BackColor = selectedColor;
            }
        }

        private void btnSelPointColour_Click(object sender, EventArgs e)
        {
            // Create a new instance of the ColorDialog
            ColorDialog colorDialog = new ColorDialog();

            // Show the ColorDialog and capture the result
            DialogResult result = colorDialog.ShowDialog();

            // Check if the user clicked the OK button in the ColorDialog
            if (result == DialogResult.OK)
            {
                // Retrieve the selected color
                Color selectedColor = colorDialog.Color;

                // Do something with the selected color
                // For example, set the background color of a control
                btnSelPointColour.BackColor = selectedColor;
            }
        }

        private void labelA28_Click(object sender, EventArgs e)
        {

        }

        private void textBoxA28_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDownloadAll_Click(object sender, EventArgs e)
        {

            DateTime selectedDate = dateTimePickerStartDate.Value.Date; // Get the selected date without the time portion
            DateTime startDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0); // Set the time as 00:00:00 (midnight)
            DateTime endDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 23, 59, 59); // Set the time as 23:59:59 (end of the day)


            // Filter the player answers based on the selected date range
            List<string> filteredPlayerAnswers = AllPlayerAnswers(startDateTime, endDateTime);
            var combinedFilteredPlayerAnswers = separateData(filteredPlayerAnswers);
            List<string> pointsFilterePlayerAnswers = combinedFilteredPlayerAnswers.Item1;
            List<string> reponsesFilterePlayerAnswers = combinedFilteredPlayerAnswers.Item2;

            // Calculate total reponse by time
            List<int> dateColumn = getDateColumn(filteredPlayerAnswers);
            List<string> hourCount = getHourCount(dateColumn);

            //Calculation for conslidated points data
            List<string> roundedFilterePlayerAnswers = roundDataPoints(pointsFilterePlayerAnswers);
            List<string> consolidatedPointAnswers = countAndAnalyzePointsInterval(roundedFilterePlayerAnswers); //Vertical
                                                                                                                //List<string> consolidatedPointAnswersF2 = consolidatedPointsFormat(filteredPlayerAnswers.Count); //Horizontal

            // Calculation for conslidated reponses data
            responsesCount = new int[optionsNumber];
            List<string[]> responseSplitByQuestion = splitResponseByQuestion(reponsesFilterePlayerAnswers);
            List<string> consolidatedResponse = countAndAnalyzeResponse(responseSplitByQuestion);

            //Combine the csv file into one
            List<string> combinedConslidatedData = combinedData(startDateTime, endDateTime, consolidatedPointAnswers, consolidatedResponse, hourCount);

            // Check if there are any matching answerss
            if (filteredPlayerAnswers.Count > 0)
            {
                // Open a SaveFileDialog to specify the download location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Format the date and time as strings
                string dateString = currentDate.ToString("dd-MM-yyyy");
                string timeString = currentDate.ToString("HHmmss");

                saveFileDialog.FileName = $"{dateString}_{timeString}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Write the filtered player answers to the selected file
                    File.WriteAllLines(savePath, combinedConslidatedData);

                    MessageBox.Show("Player answers downloaded successfully.");
                }
            }
            else
            {
                MessageBox.Show("No player answers found for the selected date range.");
            }
        }
    }
}

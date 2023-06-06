using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace TestingWinForms
{
    public partial class AdminForm : Form 
    {
        private string dateFormat = "dd/MM/yyyy HH:mm:ss";

        private int[] xAxisIntervalCounts = new int[41];  // Array to store the count of values from 0 to 10 (inclusive) with intervals of 0.5 for roundedFirstValue
        private int[] yAxisIntervalCounts = new int[41]; // Array to store the count of values from 0 to 10 (inclusive) with intervals of 0.5 for roundedSecondValue

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

            //Set the default colours when first opened
            btnExisColour.BackColor = Color.Green;
            btnSelPointColour.BackColor = Color.Red;
        }

        // Helper method to prevent flickering
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


            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 1;

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

        // Helper method to wrap text
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


        // Dont allow user to press enter as CSV will automatically save as new line
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

        private void ClearCSVFiles()
        {
            try
            {
                File.WriteAllText(GlobalVariables.csvAdminQuestionsFilePath, string.Empty);
                File.WriteAllText(GlobalVariables.csvAdminFontFilePath, string.Empty);
                File.WriteAllText(GlobalVariables.csvAdminTableFilePath, string.Empty);
                File.WriteAllText(GlobalVariables.csvAdminAdvanceFilePath, string.Empty);
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

        private PictureBox GetPictureBox(int pictureBoxIndex)
        {
            string pictureBoxName = "pictureBoxQ" + (pictureBoxIndex + 1);
            return Controls.Find(pictureBoxName, true).FirstOrDefault() as PictureBox;
        }

        private Label GetQuestionFontLabel(int labelIndex)
        {
            string labelName = "sampleLabelQ" + (labelIndex + 1);
            return Controls.Find(labelName, true).FirstOrDefault() as Label;
        }

        private Label GetAnswerFontLabel(int questionIndex, int answerIndex)
        {
            string labelName = "sampleLabelA" + (questionIndex + 1) + (answerIndex + 1);
            return Controls.Find(labelName, true).FirstOrDefault() as Label;
        }

        // Helper method to retrieve the answer TextBox based on the question and answer indices
        private TextBox GetAnswerTextBox(int questionIndex, int answerIndex)
        {
            string textBoxName = "textBoxA" + (questionIndex + 1) + (answerIndex + 1);
            return Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
        }

        private string FontToBinaryString(Font font)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, font);

                byte[] binaryData = stream.ToArray();
                string base64String = Convert.ToBase64String(binaryData);

                return base64String;
            }
        }

        private void saveTable()
        {
            // Get the data from the TextBox
            //Table
            string title = textBoxTitle.Text;
            string x_axis_top = textBoxXAxisTop.Text;
            string x_axis_bottom = textBoxXAxisBottom.Text;
            string y_axis_left = textBoxYAxisLeft.Text;
            string y_axis_right = textBoxYAxisRight.Text;

            // Convert the Color to a string representation
            string existingColour = ColorTranslator.ToHtml(btnExisColour.BackColor);
            string newColour = ColorTranslator.ToHtml(btnSelPointColour.BackColor);


            ContentAlignment textAlignment = sampleLabelTitle.TextAlign;
            bool textWrap = sampleLabelTitle.AutoSize;

            // Serialize the font object to a binary string
            string fontTitle = $"{FontToBinaryString(sampleLabelTitle.Font)};{sampleLabelTitle.TextAlign};{sampleLabelTitle.AutoSize}";
            string fontXTopAxis = $"{FontToBinaryString(sampleLabelXTopAxis.Font)};{sampleLabelXTopAxis.TextAlign};{sampleLabelXTopAxis.AutoSize}";
            string fontXBotAxis = $"{FontToBinaryString(sampleLabelXBotAxis.Font)};{sampleLabelXBotAxis.TextAlign};{sampleLabelXBotAxis.AutoSize}";
            string fontYLeftAxis = $"{FontToBinaryString(sampleLabelYLeftAxis.Font)};{sampleLabelYLeftAxis.TextAlign};{sampleLabelYLeftAxis.AutoSize}";
            string fontYRightAxis = $"{FontToBinaryString(sampleLabelYRightAxis.Font)};{sampleLabelYRightAxis.TextAlign};{sampleLabelYRightAxis.AutoSize}";

            // Concatenate the data into a comma-separated string
            string titleDate = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", 
                title, x_axis_top, x_axis_bottom, y_axis_left, y_axis_right, existingColour, newColour,
                fontTitle, fontXTopAxis, fontXBotAxis, fontYLeftAxis, fontYRightAxis);

            // Append the data to the CSV file
            while (true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(GlobalVariables.csvAdminTableFilePath, true))
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
        }

        private void saveQuestions()
        {
            // Store the question and answer data in lists
            List<string> questions = new List<string>();
            List<List<string>> answers = new List<List<string>>();
            List<string> types = new List<string>();
            List<string> imagePaths = new List<string>();

            List<string> fontQuestions = new List<string>();
            List<List<string>> fontAnswers = new List<List<string>>();

            // Loop through the questions
            for (int i = 0; i < questionsNumber; i++)
            {
                // Get the question text
                TextBox textBoxQuestion = GetQuestionTextBox(i);
                Label sampleLabel = GetQuestionFontLabel(i);
                ComboBox comboBox = GetQuestionComboBox(i);

                // Get the image from the PictureBox
                Image image;
                if (GetPictureBox(i).Image != null)
                {
                    image = GetPictureBox(i).Image;

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
                        imagePaths.Add(imagePath);
                    }
                } else
                {
                    imagePaths.Add("");
                }

                

                if (!string.IsNullOrWhiteSpace(textBoxQuestion.Text))
                {
                    string questionText = textBoxQuestion.Text;
                    if (questionText.Contains(","))
                    {
                        questionText = questionText.Replace(",", "\0");
                    }
                    questions.Add(questionText);

                    string fontQuestionToAdd = $"{FontToBinaryString(sampleLabel.Font)};{sampleLabel.TextAlign};{sampleLabel.AutoSize}";
                    fontQuestions.Add(fontQuestionToAdd);

                    // Get the answer fonts
                    List<string> answerFonts = new List<string>();
                    // Get the answer texts
                    List<string> answerOptions = new List<string>();
                    for (int j = 0; j < optionsNumber; j++)
                    {
                        TextBox textBoxAnswer = GetAnswerTextBox(i, j);
                        string answer = textBoxAnswer.Text;
                        if (answer.Contains(","))
                        {
                            answer = answer.Replace(",", "\0");
                        }
                        answerOptions.Add(answer);


                        Label answerLabel = GetAnswerFontLabel(i, j);
                        answerFonts.Add($"{FontToBinaryString(answerLabel.Font)};{answerLabel.TextAlign};{answerLabel.AutoSize}");
                    }
                    answers.Add(answerOptions);
                    fontAnswers.Add(answerFonts);

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
                string imagePath = imagePaths[i];
                string fontQuestion = fontQuestions[i];
                List<string> fontAnswer = fontAnswers[i];

                // Concatenate the data into comma-separated rows
                string rowData = string.Format("{0},{1},{2},{3}", imagePath, question, type, string.Join(",", answerOptions));

               
                // Append the rows to the CSV file
                while (true)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(GlobalVariables.csvAdminQuestionsFilePath, true))
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

                //Concat the font data into FONT csv
                string rowFontData = string.Format("{0},{1}", fontQuestion, string.Join(",", fontAnswer));

                // Append the rows to the CSV file
                while (true)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(GlobalVariables.csvAdminFontFilePath, true))
                        {
                            sw.WriteLine(rowFontData);
                        }
                        break;
                    }
                    catch (IOException ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void saveAdvance()
        {
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

            // Serialize the font object to a binary string
            string fontEndText = $"{FontToBinaryString(sampleLabelEndText.Font)};{sampleLabelEndText.TextAlign};{sampleLabelEndText.AutoSize}";

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

            // Get the image from the PictureBox
            Image image3 = pictureBox2.Image;
            string imagePath3 = null;

            //save as root directory
            if (image3 != null)
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
                imagePath3 = Path.Combine(directoryPath, fileName);
                image3.Save(imagePath3);
            }

            // Concatenate the data into a comma-separated string
            string data = string.Format("{0},{1},{2},{3},{4},{5}", timeOut, randomQuestions, endSurveyText, imagePath, imagePath3, fontEndText); 

            // Append the data to the CSV file
            while (true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(GlobalVariables.csvAdminAdvanceFilePath, true))
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
        }

        void UpdatePlayerCSVHeader()
        {

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
                    lines = File.ReadAllLines(GlobalVariables.csvRawDataFilePath);
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
            File.WriteAllLines(GlobalVariables.csvRawDataFilePath, lines);

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Clear the contents of the CSV files
            ClearCSVFiles();

            saveTable();

            saveQuestions();

            UpdatePlayerCSVHeader();

            saveAdvance();

            MessageBox.Show("Data saved to CSV file.");
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
                LoadQuestionData(tabPage, GlobalVariables.csvAdminQuestionsFilePath, i);
                LoadFontData(tabPage, GlobalVariables.csvAdminFontFilePath, i);
            }
        }

        private Font FontFromBinaryString(string fontData)
        {
            byte[] binaryData = Convert.FromBase64String(fontData);

            using (MemoryStream stream = new MemoryStream(binaryData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Font)formatter.Deserialize(stream);
            }
        }

        private void LoadTableData()
        {
            if (File.Exists(GlobalVariables.csvAdminTableFilePath))
            {
                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminTableFilePath);
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

                    if (values.Length == 12)
                    {
                        textBoxTitle.Text = values[0];
                        textBoxXAxisTop.Text = values[1];
                        textBoxXAxisBottom.Text = values[2];
                        textBoxYAxisLeft.Text = values[3];
                        textBoxYAxisRight.Text = values[4];

                        btnExisColour.BackColor = ColorTranslator.FromHtml(values[5]);
                        btnSelPointColour.BackColor = ColorTranslator.FromHtml(values[6]);

                        // Load the font data from the CSV
                        loadContentToComponent(values, 7, sampleLabelTitle);
                        loadContentToComponent(values, 8, sampleLabelXTopAxis);
                        loadContentToComponent(values, 9, sampleLabelXBotAxis);
                        loadContentToComponent(values, 10, sampleLabelYLeftAxis);
                        loadContentToComponent(values, 11, sampleLabelYRightAxis);

                    }
                }
            }

        }

        void loadContentToComponent(string[] values, int ValueIndex, Label label) {
            string[] textProperties = values[ValueIndex].Split(';'); // Assuming text font and alignment data is at index 5
            string fontTitle = textProperties[0]; // First ; is text font

            Font loadedFontTitle = FontFromBinaryString(fontTitle);
            label.Font = loadedFontTitle;
            label.Height = (int)Math.Ceiling(FontFromBinaryString(fontTitle).GetHeight()) + Padding.Vertical; ;

            if (textProperties.Length > 2)
            {
                String textAlign = textProperties[1]; // Second ; is text align property
                String textWrap = textProperties[2]; // Third ; is text wrap property
                if (Enum.TryParse(textAlign, out ContentAlignment alignment))
                {
                    label.TextAlign = alignment;
                }
                else
                {
                    label.TextAlign = ContentAlignment.TopLeft; //default ContentAlignment
                }
                label.AutoSize = textWrap.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            label.Height = (int)Math.Ceiling(loadedFontTitle.GetHeight()) + Padding.Vertical;
        }

        private void LoadNumQuestions()
        {
            if (File.Exists(GlobalVariables.csvAdminQuestionsFilePath))
            {

                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminQuestionsFilePath);
                        questionsNumber = lines.Length;
                        break;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

               
            }
        }

        private void LoadQuestionData(TabPage tabPage, string csvFilePath, int questionIndex)
        {
            if (File.Exists(csvFilePath)) 
            {
                string[] lines;
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
                    PictureBox questionPictureBox = tabPage.Controls.OfType<PictureBox>().FirstOrDefault(c => c.Name.StartsWith("pictureBoxQ"));

                    if (questionTextBox != null && answerTextBoxes.Length == optionsNumber && questionTypeComboBox != null)
                    {
                        //if got comma, change back to comma
                        questionTextBox.Text = values[1].Replace("\0", ",");
                        for (int i = values.Length - 4; i >= 0; i--)
                        {
                            answerTextBoxes[values.Length - 4 - i].Text = values[i + 3].Replace("\0", ",");
                        }
                        questionTypeComboBox.Text = values[2];

                        string imagePath = Path.Combine(values[0]);
                        if (File.Exists(imagePath))
                        {
                            Image image = Image.FromFile(imagePath);
                            questionPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            questionPictureBox.Image = image;
                        }

                    }
                }
            }
        }

        private void LoadFontData(TabPage tabPage, string csvFilePath, int questionIndex)
        {
            if (File.Exists(csvFilePath))
            {
                string[] lines;
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

                    Label sampleQuestionLabels = tabPage.Controls.OfType<Label>().FirstOrDefault(c => c.Name.StartsWith("sampleLabelQ"));
                    Label[] sampleAnswerLabels = tabPage.Controls.OfType<Label>().Where(c => c.Name.StartsWith("sampleLabelA" + (questionIndex + 1))).ToArray();
                   
                    if (sampleQuestionLabels != null && sampleAnswerLabels.Length == optionsNumber)
                    {
                        string[] textProperties = values[0].Split(';'); // Assuming text font and alignment data is at index 5
                        string fontTitle = textProperties[0]; // First ; is text font
                        if (textProperties.Length > 2)
                        {
                            String textAlign = textProperties[1]; // Second ; is text align properties
                            String textWrap = textProperties[2];
                            // Load text alignment
                            if (Enum.TryParse(textAlign, out ContentAlignment alignment))
                            {
                                // Conversion succeeded, and the alignment variable now contains the corresponding enum value
                                // You can use the alignment variable as needed
                                sampleQuestionLabels.TextAlign = alignment;
                            }
                            else
                            {
                                // Conversion failed, handle the error or set a default value
                                sampleQuestionLabels.TextAlign = ContentAlignment.MiddleCenter;
                            }
                            sampleQuestionLabels.AutoSize = textWrap.Equals("true", StringComparison.OrdinalIgnoreCase);

                        }

                        sampleQuestionLabels.Font = FontFromBinaryString(fontTitle);

                        for (int i = values.Length - 1; i >= 1; i--)
                        {
                            string[] answerTextProperties = values[i].Split(';');
                            String font = answerTextProperties[0];
                            if (answerTextProperties.Length > 2)
                            {
                                String textAlign = answerTextProperties[1];
                                String textWrap = answerTextProperties[2];

                                if (Enum.TryParse(textAlign, out ContentAlignment alignment))
                                {
                                    // Conversion succeeded, and the alignment variable now contains the corresponding enum value
                                    // You can use the alignment variable as needed
                                    sampleAnswerLabels[values.Length - 1 - i].TextAlign = alignment;
                                }
                                else
                                {
                                    // Conversion failed, handle the error or set a default value
                                    sampleAnswerLabels[values.Length - 1 - i].TextAlign = ContentAlignment.TopLeft;
                                }
                                sampleAnswerLabels[values.Length - 1 - i].AutoSize = textWrap.Equals("true", StringComparison.OrdinalIgnoreCase);
                            }

                            sampleAnswerLabels[values.Length - 1 - i].Font = FontFromBinaryString(font);
                            
                        }
                    }
                }
            }
        }


        private void LoadAdvanceData()
        {
            if (File.Exists(GlobalVariables.csvAdminAdvanceFilePath))
            {
                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminAdvanceFilePath);
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

                    if (values.Length == 6)
                    {
                        textBoxTimeOut.Text = values[0];
                        comboBoxRandomQns.Text = values[1];
                        textBoxEndMessage.Text = values[2];

                        string imagePath = Path.Combine(values[3]);

                        if (File.Exists(imagePath))
                        {
                            Image image = Image.FromFile(imagePath);
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox.Image = image;
                        }

                        string imagePath3 = Path.Combine(values[4]);

                        if (File.Exists(imagePath3))
                        {
                            Image image3 = Image.FromFile(imagePath3);
                            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox2.Image = image3;
                        }

                        // Load the font data from the CSV
                        //string fontEndSurvey = values[5]; // Assuming font data is at index 5
                        loadContentToComponent(values, 5, sampleLabelEndText);
                        // Deserialize the font from the font data
                        //Font loadedFontEndSurvey = FontFromBinaryString(fontEndSurvey);
                        // Apply the font to the label or control of your choice
                        //sampleLabelEndText.Font = loadedFontEndSurvey;
                    }
                }
            }

        }


        private void btnDownload_Click(object sender, EventArgs e)
        {

            DateTime selectedStartDate = dateTimePickerStartDate.Value.Date; // Get the selected date without the time portion
            DateTime selectedEndDate = dateTimePickerEndDate.Value.Date;
            DateTime startDateTime = new DateTime(selectedStartDate.Year, selectedStartDate.Month, selectedStartDate.Day, 0, 0, 0); // Set the time as 00:00:00 (midnight)
            DateTime endDateTime = new DateTime(selectedEndDate.Year, selectedEndDate.Month, selectedEndDate.Day, 23, 59, 59); // Set the time as 23:59:59 (end of the day)

            // Compare the date and time from start to end
            int result = DateTime.Compare(selectedEndDate, selectedStartDate);


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
            List<string> combinedConslidatedData = new List<string>();
            combinedConslidatedData.Add($"Date selected: {startDateTime} to {endDateTime}");
            combinedConslidatedData.Add("");
            List<string> conData = combinedData(consolidatedPointAnswers, consolidatedResponse, hourCount);
            combinedConslidatedData.AddRange(conData);

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

                saveFileDialog.FileName = $"CON_{dateString}_{timeString}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Write the filtered player answers to the selected file
                    File.WriteAllLines(savePath, combinedConslidatedData);

                    MessageBox.Show("Player answers downloaded successfully.");
                }
            }
            else if (result < 0)
            {
                // End date is before the start date
                // Display an error message or take appropriate action
                MessageBox.Show("End date cannot be before the start date.");
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

        private List<string> combinedData(List<string> consolidatedPointAnswers, List<string> consolidatedResponse, List<string> hourCount)
        {
            List<string> combinedConslidatedData = new List<string>();
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
                            if (j < values.Length)
                            {
                                if (!string.IsNullOrEmpty(values[j]) && values[j] != "" && values[j] != " ")
                                {
                                    responsesCount[j]++;
                                }
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
            double[] percentageCountX = new double[41]; 
            double[] percentageCountY = new double[41];
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
                        xAxisIntervalCounts[x_axis+20]++;
                        yAxisIntervalCounts[y_axis+20]++;
                    }
                }
            }

            //Calculate % of responses
            for (int i = 0; i < 41; i++)
            {
                percentageCountX[i] = ((double)xAxisIntervalCounts[i] / numOfResponses) * 100;
                percentageCountY[i] = ((double)yAxisIntervalCounts[i] / numOfResponses) * 100;
            }

            // Output the counts
            for (int i = 0; i < 41; i++)
            {
                float value = (i / 2f) - 10;
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

            if (File.Exists(GlobalVariables.csvRawDataFilePath))
            {
                string[] allAnswers;

                while (true) {
                    try
                    {
                        allAnswers = File.ReadAllLines(GlobalVariables.csvRawDataFilePath);
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

            if (File.Exists(GlobalVariables.csvRawDataFilePath))
            {
                string[] allAnswers;

                while (true)
                {
                    try
                    {
                        allAnswers = File.ReadAllLines(GlobalVariables.csvRawDataFilePath);
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
                previousTabPage.VerticalScroll.Value = 0;

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

                        labelT.Font = control.Font;
                    }
                    else if (newControl is Label labelSampleQuestion && control.Name.StartsWith("sampleLabelQ"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newLabelTypeName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        labelSampleQuestion.Name = newLabelTypeName; // Update the Label with the new tab name

                        labelSampleQuestion.Text = control.Text;

                        // Set the default font style and size
                        labelSampleQuestion.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular);

                    }
                    else if (newControl is Label labelSampleAnswer && control.Name.StartsWith("sampleLabelA"))
                    {
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newTabName = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        labelSampleAnswer.Name = newTabName; // Update the Label with the new tab name

                        labelSampleAnswer.Text = control.Text;

                        // Set the default font style and size 
                        labelSampleAnswer.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular);

                    }
                    else if (newControl is Button fontButtonQ && control.Name.StartsWith("btnChangeQ"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newFontButtonName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        fontButtonQ.Name = newFontButtonName; // Update the Label with the new tab name
                        fontButtonQ.Text = control.Text;
                        fontButtonQ.Click += btnChangeFont_Click;
                        fontButtonQ.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular);
                    }
                    else if (newControl is Button textChangeButtonQ && control.Name.StartsWith("btnTextChangeQ"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newFontButtonName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        textChangeButtonQ.Name = newFontButtonName; // Update the Label with the new tab name
                        textChangeButtonQ.Text = control.Text;
                        textChangeButtonQ.Click += btnTextChange_Click;
                        textChangeButtonQ.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular);
                    }
                    else if (newControl is Button fontButtonA && control.Name.StartsWith("btnChangeA"))
                    {
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newFontButtonA = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        fontButtonA.Name = newFontButtonA; // Update the Label with the new tab name
                        fontButtonA.Text = control.Text;
                        fontButtonA.Click += btnChangeFont_Click;
                        fontButtonA.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular);
                    }
                    else if (newControl is Button textChangeButtonA && control.Name.StartsWith("btnTextChangeA"))
                    {
                        string secondLastDigit = previousControlName.Substring(previousControlName.Length - 2, 1);
                        int secondLastDigitValue = int.Parse(secondLastDigit);
                        string newFontButtonA = previousControlName.Substring(0, previousControlName.Length - 2) + (secondLastDigitValue + 1) + previousControlName.Substring(previousControlName.Length - 1);
                        textChangeButtonA.Name = newFontButtonA; // Update the Label with the new tab name
                        textChangeButtonA.Text = control.Text;
                        textChangeButtonA.Click += btnTextChange_Click;
                        textChangeButtonA.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular);
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
                    else if (newControl is PictureBox pictureBox && control.Name.StartsWith("pictureBoxQ"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newPictureBoxName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        pictureBox.Name = newPictureBoxName;

                    }
                    else if (newControl is Button buttonBackground && control.Name.StartsWith("btnBackground"))
                    {
                        string lastDigit = previousControlName.Substring(previousControlName.Length - 1);
                        int lastDigitValue = int.Parse(lastDigit);
                        string newButtonName = previousControlName.Substring(0, previousControlName.Length - 1) + (lastDigitValue + 1);
                        buttonBackground.Name = newButtonName;
                        buttonBackground.Text = control.Text;
                        buttonBackground.Click += UploadBackground_Click;
                    }

                    newControl.TabIndex = control.TabIndex + tabIndexOffset;
                    // Copy any other desired properties or event handlers
                    newTabPage.Controls.Add(newControl);
                }
                
            }
            // Add auto-scrolling to the TabControl
            newTabPage.AutoScroll = true;

            tabControl.TabPages.Add(newTabPage);

            // Set the selected tab to the newly added tab
            tabControl.SelectedTab = newTabPage;

            // Enable text wrapping for textboxes in the newly added tab
            EnableTextBoxTextWrapping(tabControl);

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.Font = new Font(tabPage.Font.FontFamily, 16, FontStyle.Regular);
                foreach (Control control in tabPage.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                        int contentWidth = textBox.Width;

                        // Check if the vertical scrollbar is visible
                        if (textBox.ScrollBars == ScrollBars.Vertical && textBox.Lines.Length > textBox.Height / textBox.Font.Height)
                        {
                            int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;
                            contentWidth -= scrollbarWidth;
                        }

                        textBox.Size = new Size(contentWidth, textBox.Height);
                    }
                    if (control is ComboBox comboBox)
                    {
                        comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    }
                    if (control is Button changeButton && control.Name.StartsWith("btnChange"))
                    {
                        changeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        changeButton.AutoSize = true;
                    }
                    if (control is Button textChangeButton && control.Name.StartsWith("btnTextChange"))
                    {
                        textChangeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        textChangeButton.AutoSize = true;
                    }
                    else if (control is Button normalButton)
                    {
                        normalButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    }
                }
            }
        }

        // helper method to change font dynamically
        private void btnChangeFont_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string buttonName = clickedButton.Name;

            if (buttonName.StartsWith("btnChangeQ"))
            {
                // Button name starts with "btnChangeQ"
                // Handle logic for "Q" buttons
                string lastDigit = buttonName.Substring(buttonName.Length - 1);
                string labelName = "sampleLabelQ" + lastDigit;

                FontDialog fontDialog = new FontDialog();
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    Control[] labels = this.Controls.Find(labelName, true);
                    if (labels.Length > 0 && labels[0] is Label label)
                    {
                        label.Font = fontDialog.Font;
                    }
                }
            }
            else if (buttonName.StartsWith("btnChangeA"))
            {
                // Button name starts with "btnChangeA"
                // Handle logic for "A" buttons
                string lastTwoDigits = buttonName.Substring(buttonName.Length - 2);
                string labelName = "sampleLabelA" + lastTwoDigits;

                FontDialog fontDialog = new FontDialog();
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    Control[] labels = this.Controls.Find(labelName, true);
                    if (labels.Length > 0 && labels[0] is Label label)
                    {
                        label.Font = fontDialog.Font;
                    }
                }
            }
        }

        // helper method to change font dynamically
        private void btnTextChange_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string buttonName = clickedButton.Name;

            if (buttonName.StartsWith("btnTextChangeQ"))
            {
                string lastDigit = buttonName.Substring(buttonName.Length - 1);
                string labelName = "sampleLabelQ" + lastDigit;

                Control[] labels = this.Controls.Find(labelName, true);
                if (labels.Length > 0 && labels[0] is Label label)
                {
                    using (CustomText dialog = new CustomText(label.TextAlign, label.AutoSize))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            label.TextAlign = dialog.SelectedAlignment;
                            label.AutoSize = dialog.SelectedWrap;
                        }
                    }
                }
            }
            else if (buttonName.StartsWith("btnTextChangeA"))
            {
                string lastTwoDigits = buttonName.Substring(buttonName.Length - 2);
                string labelName = "sampleLabelA" + lastTwoDigits;

                Control[] labels = this.Controls.Find(labelName, true);
                if (labels.Length > 0 && labels[0] is Label label)
                {
                    using (CustomText dialog = new CustomText(label.TextAlign, label.AutoSize))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            label.TextAlign = dialog.SelectedAlignment;
                            label.AutoSize = dialog.SelectedWrap;
                        }
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

        private void UploadBackground_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int buttonIndex = int.Parse(clickedButton.Name.Substring(clickedButton.Name.Length - 1));

            // Find the corresponding PictureBox based on the button index
            string pictureBoxName = "pictureBoxQ" + (buttonIndex);
            Control[] pictureBoxes = this.Controls.Find(pictureBoxName, true);
            if (pictureBoxes.Length > 0 && pictureBoxes[0] is PictureBox pictureBox)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Select Image";
                    openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedImagePath = openFileDialog.FileName;

                        // Load the selected image into the PictureBox
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.Image = Image.FromFile(selectedImagePath);
                    }
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


        private void btnDownloadAll_Click(object sender, EventArgs e)
        {

            DateTime selectedDate = dateTimePickerStartDate.Value.Date; // Get the selected date without the time portion
            DateTime startDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0); // Set the time as 00:00:00 (midnight)
            DateTime endDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 23, 59, 59); // Set the time as 23:59:59 (end of the day)


            // Filter the player answers based on the selected date range
            List<string> allPlayerAnswers = AllPlayerAnswers(startDateTime, endDateTime);

            var combinedFilteredPlayerAnswers = separateData(allPlayerAnswers);
            List<string> pointsFilterePlayerAnswers = combinedFilteredPlayerAnswers.Item1;
            List<string> reponsesFilterePlayerAnswers = combinedFilteredPlayerAnswers.Item2;

            // Calculate total reponse by time
            List<int> dateColumn = getDateColumn(allPlayerAnswers);
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
            List<string> combinedConslidatedData = GetFirstLastDate(allPlayerAnswers);
            List<string> consData = combinedData(consolidatedPointAnswers, consolidatedResponse, hourCount);
            combinedConslidatedData.AddRange(consData);

            // Check if there are any matching answerss
            if (allPlayerAnswers.Count > 0)
            {
                // Open a SaveFileDialog to specify the download location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Format the date and time as strings
                string dateString = currentDate.ToString("dd-MM-yyyy");
                string timeString = currentDate.ToString("HHmmss");

                saveFileDialog.FileName = $"ALL_{dateString}_{timeString}.csv";

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

        private List<string> GetFirstLastDate(List<string> filteredPlayerAnswers)
        {
            List<string> conData = new List<string>();
            string firstResponse = filteredPlayerAnswers[0];
            string lastResponse = filteredPlayerAnswers[filteredPlayerAnswers.Count - 1];

            string[] valuesFirst = firstResponse.Split(',');
            string[] valuesLast = lastResponse.Split(',');
            valuesFirst[0] = valuesFirst[0].TrimStart('\'');
            valuesLast[0] = valuesLast[0].TrimStart('\'');

            if ((DateTime.TryParseExact(valuesFirst[0], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate)) && (DateTime.TryParseExact(valuesLast[0], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate)))
            {
                conData.Add($"Date range: {startDate} to {endDate}");
            }
            return conData;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pictureBox2.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnBackground1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pictureBoxQ1.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnBackground2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pictureBoxQ2.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnBackground3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pictureBoxQ3.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnDownloadRawData_Click(object sender, EventArgs e)
        {
            // Specify the path of the existing CSV file
            string sourceFilePath = GlobalVariables.csvRawDataFilePath;

            // Create a SaveFileDialog to prompt the user to choose a save location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv";

            // Get the current date and time
            DateTime currentDate = DateTime.Now;

            // Format the date and time as strings
            string dateString = currentDate.ToString("dd-MM-yyyy");
            string timeString = currentDate.ToString("HHmmss");

            saveFileDialog.FileName = $"RAW_{dateString}_{timeString}.csv";

            // Show the SaveFileDialog and check if the user clicked the OK button
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                string destinationFilePath = saveFileDialog.FileName;

                try
                {
                    // Copy the existing CSV file to the selected location
                    File.Copy(sourceFilePath, destinationFilePath);

                    // Display a success message
                    MessageBox.Show("File downloaded successfully!");
                }
                catch (Exception ex)
                {
                    // Display an error message if the file copy fails
                    MessageBox.Show("An error occurred while downloading the file: " + ex.Message);
                }
            }

        }

        private void btnChangeEndSurveyFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelYRightAxis.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelEndText.Font = fontDialog.Font;
            }
        }

        private void btnTextChangeEndSurveyFont_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelEndText.TextAlign,
            sampleLabelEndText.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelEndText.TextAlign = dialog.SelectedAlignment;
                    sampleLabelEndText.AutoSize = dialog.SelectedWrap;
                }
            }
        }


        private void btnChangeQ1_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelQ1.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelQ1.Font = fontDialog.Font;
            }
        }

        private void btnChangeA17_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA17.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA17.Font = fontDialog.Font;
            }
        }

        private void btnChangeA16_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA16.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA16.Font = fontDialog.Font;
            }
        }

        private void btnChangeA15_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA15.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA15.Font = fontDialog.Font;
            }
        }

        private void btnChangeA14_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA14.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA14.Font = fontDialog.Font;
            }
        }

        private void btnChangeA13_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA13.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA13.Font = fontDialog.Font;
            }
        }

        private void btnChangeA12_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA12.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA12.Font = fontDialog.Font;
            }
        }

        private void btnChangeA11_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA11.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA11.Font = fontDialog.Font;
            }
        }

        private void btnChangeA18_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA18.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA18.Font = fontDialog.Font;
            }
        }

        private void btnChangeA28_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA28.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA28.Font = fontDialog.Font;
            }
        }

        private void btnChangeA21_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA21.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA21.Font = fontDialog.Font;
            }
        }

        private void btnChangeA22_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA22.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA22.Font = fontDialog.Font;
            }
        }

        private void btnChangeA23_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA23.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA23.Font = fontDialog.Font;
            }
        }

        private void btnChangeA24_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA24.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA24.Font = fontDialog.Font;
            }
        }

        private void btnChangeA25_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA25.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA25.Font = fontDialog.Font;
            }
        }

        private void btnChangeA26_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA26.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA26.Font = fontDialog.Font;
            }
        }

        private void btnChangeA27_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA27.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA27.Font = fontDialog.Font;
            }
        }

        private void btnChangeQ2_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelQ2.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelQ2.Font = fontDialog.Font;
            }
        }

        private void btnChangeA38_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA38.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA38.Font = fontDialog.Font;
            }
        }

        private void btnChangeA31_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA31.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA31.Font = fontDialog.Font;
            }
        }

        private void btnChangeA32_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA32.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA32.Font = fontDialog.Font;
            }
        }

        private void btnChangeA33_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA33.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA33.Font = fontDialog.Font;
            }
        }

        private void btnChangeA34_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA34.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA34.Font = fontDialog.Font;
            }
        }

        private void btnChangeA35_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA35.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA35.Font = fontDialog.Font;
            }
        }

        private void btnChangeA36_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA36.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA36.Font = fontDialog.Font;
            }
        }

        private void btnChangeA37_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelA37.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelA37.Font = fontDialog.Font;
            }
        }

        private void btnChangeQ3_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelQ3.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelQ3.Font = fontDialog.Font;
            }
        }

        private void btnChangeTitle_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelTitle.Font; // Set the initial font
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelTitle.Font = fontDialog.Font;
            }
        }

        private void btnTextChangeTitle_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelTitle.TextAlign,
            sampleLabelTitle.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelTitle.TextAlign = dialog.SelectedAlignment;
                    sampleLabelTitle.AutoSize = dialog.SelectedWrap;
                }
            }
        }

        private void btnChangeXTopAxis_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelXTopAxis.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelXTopAxis.Font = fontDialog.Font;
            }
        }


        private void btnTextChangeXTopAxis_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelXTopAxis.TextAlign,
            sampleLabelXTopAxis.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelXTopAxis.TextAlign = dialog.SelectedAlignment;
                    sampleLabelXTopAxis.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnChangeXBotAxis_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelTitle.Font; // Set the initial font
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelXBotAxis.Font = fontDialog.Font;
            }
        }
        private void btnTextChangeXBotAxis_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelXBotAxis.TextAlign,
            sampleLabelXBotAxis.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelXBotAxis.TextAlign = dialog.SelectedAlignment;
                    sampleLabelXBotAxis.AutoSize = dialog.SelectedWrap;

                }
            }

        }

        private void btnChangeYLeftAxis_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelYLeftAxis.Font; // Set the initial font
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelYLeftAxis.Font = fontDialog.Font;
            }
        }

        private void btnTextChangeYLeftAxis_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelYLeftAxis.TextAlign,
            sampleLabelYLeftAxis.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelYLeftAxis.TextAlign = dialog.SelectedAlignment;
                    sampleLabelYLeftAxis.AutoSize = dialog.SelectedWrap;
                }
            }
        }

        private void btnChangeYRightAxis_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabelYRightAxis.Font; // Set the initial font
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabelYRightAxis.Font = fontDialog.Font;
            }
        }

        private void btnTextChangeYRightAxis_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelYRightAxis.TextAlign,
            sampleLabelYRightAxis.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelYRightAxis.TextAlign = dialog.SelectedAlignment;
                    sampleLabelYRightAxis.AutoSize = dialog.SelectedWrap;
                }
            }
        }

        private void btnTextChangeQ1_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelQ1.TextAlign,
            sampleLabelQ1.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelQ1.TextAlign = dialog.SelectedAlignment;
                    sampleLabelQ1.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeQ2_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelQ2.TextAlign,
            sampleLabelQ2.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelQ2.TextAlign = dialog.SelectedAlignment;
                    sampleLabelQ2.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeQ3_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelQ3.TextAlign,
            sampleLabelQ3.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelQ3.TextAlign = dialog.SelectedAlignment;
                    sampleLabelQ3.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA11_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA11.TextAlign,
            sampleLabelA11.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA11.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA11.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA12_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA12.TextAlign,
            sampleLabelA12.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA12.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA12.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA13_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA13.TextAlign,
            sampleLabelA13.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA13.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA13.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA14_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA14.TextAlign,
            sampleLabelA14.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA14.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA14.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA15_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA15.TextAlign,
            sampleLabelA15.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA15.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA15.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA16_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA16.TextAlign,
            sampleLabelA16.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA16.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA16.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA17_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA17.TextAlign,
            sampleLabelA17.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA17.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA17.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA18_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA18.TextAlign,
            sampleLabelA18.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA18.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA18.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA28_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA28.TextAlign,
            sampleLabelA28.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA28.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA28.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA22_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA22.TextAlign,
            sampleLabelA22.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA22.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA22.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA23_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA23.TextAlign,
            sampleLabelA23.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA23.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA23.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA24_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA24.TextAlign,
            sampleLabelA24.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA24.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA24.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA25_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA25.TextAlign,
            sampleLabelA25.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA25.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA25.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA26_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA26.TextAlign,
            sampleLabelA26.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA26.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA26.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA27_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA27.TextAlign,
            sampleLabelA27.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA27.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA27.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA21_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA21.TextAlign,
            sampleLabelA21.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA21.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA21.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA38_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA38.TextAlign,
            sampleLabelA38.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA38.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA38.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA32_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA32.TextAlign,
            sampleLabelA32.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA32.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA32.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA33_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA33.TextAlign,
            sampleLabelA33.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA33.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA33.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA34_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA34.TextAlign,
            sampleLabelA34.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA34.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA34.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA35_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA35.TextAlign,
            sampleLabelA35.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA35.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA35.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA36_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA36.TextAlign,
            sampleLabelA36.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA36.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA36.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA37_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA37.TextAlign,
            sampleLabelA37.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA37.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA37.AutoSize = dialog.SelectedWrap;

                }
            }
        }

        private void btnTextChangeA31_Click(object sender, EventArgs e)
        {
            using (CustomText dialog = new CustomText(sampleLabelA31.TextAlign,
            sampleLabelA31.AutoSize))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sampleLabelA31.TextAlign = dialog.SelectedAlignment;
                    sampleLabelA31.AutoSize = dialog.SelectedWrap;

                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingWinForms
{
    public partial class QuestionForm : Form
    {
        private List<Question> questions;
        private List<Question> defaultQuestions;
        private int currentQuestionIndex;
        private Timer timer;
        //private const int TimerInterval = 5000; // 3 seconds in milliseconds
        private int timerInterval = 0;
        private int rowNumber;

        private string randomQuestionsText = null;

        private int optionsNumber = 8;

        String[] savedAnswers; //Stores answers to each question

        Dictionary<string, string> checkboxValues = new Dictionary<string, string>(); // Stores answers on checkbox change


        public QuestionForm(int rowNumber)
        {

            // Initialize the list of questions
            defaultQuestions = new List<Question>()
            {
                new Question(0, "MCQ", "Question 1", new List<string> { "Option A", "Option B", "Option C", "Option D", "Option E", "Option C", "Option D", "Option E",}, "",
                "", new List<string>{ "", "", "", "", "", "", "", ""}, 
                ContentAlignment.TopLeft, new List<ContentAlignment>{ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft },
                true, new List<bool>{true, true, true, true, true, true, true, true },
                Color.Black, new List<Color>{ Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black,  Color.Black }),
                new Question(1, "MCQ", "Question 2", new List<string> { "Option D", "Option E", "Option F", "Option D", "Option E", "Option C", "Option D", "Option E"}, "",
                "", new List<string>{ "", "", "", "", "", "", "", ""},
                ContentAlignment.TopLeft, new List<ContentAlignment>{ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft },
                true, new List<bool>{true, true, true, true, true, true, true, true },
                Color.Black, new List<Color>{ Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black,  Color.Black }),
                new Question(2, "MRQ", "Question 3", new List<string> { "Option G", "Option H", "Option I", "Option D", "Option E", "Option C", "Option D", "Option E" }, "",
                "", new List<string>{ "", "", "", "", "", "", "", ""},
                ContentAlignment.TopLeft, new List<ContentAlignment>{ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft, ContentAlignment.TopLeft },
                true, new List<bool>{true, true, true, true, true, true, true, true },
                Color.Black, new List<Color>{ Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black,  Color.Black }),
            };

            InitializeComponent();
            DoubleBuffered = true;
            BackgroundImageLayout = ImageLayout.None;


            //tableLayoutPanel1.SuspendLayout();

            // Check if admin wants questions to be in random
            LoadRandomQuestionsAndTimerIntervalData();

            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

            this.rowNumber = rowNumber;

            currentQuestionIndex = 0;

            // Set up the timer
            timer = new Timer();
            if (timerInterval > 0) // Handle timerInterval set to smaller than 0 error
            {
                timer.Interval = timerInterval;
            }
            timer.Tick += Timer_Tick;
            timer.Start();

            questions = LoadQuestionsFromCSV();
            savedAnswers = new String[questions.Count];

            string semicolonString = " " + string.Join("; ", new string[optionsNumber]);

            for (int i = 0; i < savedAnswers.Length; i++)
            {
                savedAnswers[i] = semicolonString;
            }

        }

        private void QuestionForm_Load(object sender, EventArgs e)
        {
            // Create an array or list of radio buttons
            RadioButton[] radioButtons = new RadioButton[]
            {
                optionARadioButton,
                optionBRadioButton,
                optionCRadioButton,
                optionDRadioButton,
                optionERadioButton,
                optionFRadioButton,
                optionGRadioButton,
                optionHRadioButton
            };

            CheckBox[] checkboxes = new CheckBox[]
           {
                    optionACheckBox,
                    optionBCheckBox,
                    optionCCheckBox,
                    optionDCheckBox,
                    optionECheckBox,
                    optionFCheckBox,
                    optionGCheckBox,
                    optionHCheckBox
           };

            // Subscribe to the CheckedChanged event for each radio button
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.CheckedChanged += RadioButton_CheckedChanged;
            }

            // Subscribe to the CheckedChanged event for each radio button
            foreach (CheckBox checkBox in checkboxes)
            {
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
            }
            DisplayQuestion(); //Display first question
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

        

        // Helper method to centralise the question label
        private void CenterQuestionLabel()
        {
            // Check if the label's width exceeds 3/4 of the screen width
            int maxWidth = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.75);
            if (questionLabel.Width > maxWidth)
            {
                // Enable text wrapping
                questionLabel.AutoSize = true;
                questionLabel.MaximumSize = new Size(maxWidth, 0);
            }

            // Calculate the center position of the form
            int centerX = Width / 2;
            // Calculate the center position of the question label
            int labelX = centerX - (questionLabel.Width / 2);
            // Set the position of the question label
            questionLabel.Location = new Point(labelX, questionLabel.Location.Y);


            labelMCQorMRQ.TextAlign = ContentAlignment.MiddleCenter;
            // Calculate the center position of the form
            int centerMCQX = Width / 2;
            // Calculate the center position of the question label
            int labelMCQX = centerMCQX - (labelMCQorMRQ.Width / 2);
            // Set the position of the question label
            labelMCQorMRQ.Location = new Point(labelMCQX, labelMCQorMRQ.Location.Y);

        }


        // Returns a List<Question> consisting of questions saved in admin_question.csv file
        private List<Question> LoadQuestionsFromCSV()
        {
            List<Question> questions = new List<Question>();

            int currQuestionIndex = 0;

            if (File.Exists(GlobalVariables.csvAdminQuestionsFilePath))
            {
                using (var reader = new StreamReader(GlobalVariables.csvAdminQuestionsFilePath))
                {
                    if (File.Exists(GlobalVariables.csvAdminFontFilePath))
                    {
                        using (var fontReader = new StreamReader(GlobalVariables.csvAdminFontFilePath))
                        {
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(',');

                                var fontLine = fontReader.ReadLine();
                                var fontValues = fontLine.Split(',');

                                if (values.Length >= 2 && fontValues.Length != 0) // Assuming each line in the CSV has at least 4 values: question, option1, option2, option3
                                {
                                    string imagePath = Path.Combine(values[0]);
                                    string questionText = values[1].Replace("\0", ",");
                                    string type = values[2];

                                    List<string> options = new List<string> { values[3].Replace("\0", ","), values[4].Replace("\0", ","),
                                    values[5].Replace("\0", ","), values[6].Replace("\0", ","), values[7].Replace("\0", ","), 
                                        values[8].Replace("\0", ","), values[9].Replace("\0", ","), values[10].Replace("\0", ",")  };


                                    //fonts
                                    //string fontQuestion = fontValues[0];
                                    ContentAlignment textAlignQuestion = ContentAlignment.TopLeft;
                                    bool textWrapQuestion = true;
                                    Color textColor = Color.Black;
                                    string[] fontQuestionProperties = fontValues[0].Split(';'); // Assuming text font and alignment data is at index 5
                                    string fontQuestion = fontQuestionProperties[0]; // First ; is text font
                                    if (fontQuestionProperties.Length > 3)
                                    { 

                                        // Parse the textAlignValue as ContentAlignment
                                        if (Enum.TryParse(fontQuestionProperties[1], out ContentAlignment alignment))
                                        {
                                            textAlignQuestion = alignment;
                                        }
                                        else
                                        {
                                            textAlignQuestion = ContentAlignment.TopLeft;
                                        }

                                        // Parse the textWrapValue as boolean
                                        if (bool.TryParse(fontQuestionProperties[2], out bool wrapValue))
                                        {
                                            textWrapQuestion = wrapValue;
                                        }
                                        else
                                        {
                                            textWrapQuestion = true;
                                        }

                                        // Retrieve the forecolor value and set it to the label's ForeColor property
                                        if (!string.IsNullOrEmpty(fontQuestionProperties[3]))
                                        {
                                            if (int.TryParse(fontQuestionProperties[3].Replace("\0", ","), out int foreColorArgb))
                                            {
                                                textColor = Color.FromArgb(foreColorArgb);
                                            }
                                            else
                                            {
                                                textColor = Color.Black; // Default forecolor if parsing fails
                                            }
                                        }
                                    }

                                    //string fontQuestion = fontValues[0];
                                    //List<string> fontOptions = new List<string> { fontValues[1], fontValues[2],
                                    //fontValues[3], fontValues[4], fontValues[5], fontValues[6], fontValues[7], fontValues[8] };

                                    List<string> fontOptions = new List<string>();
                                    List<ContentAlignment> textAlignOptions = new List<ContentAlignment>();
                                    List<bool> textWrapOptions = new List<bool>();
                                    List<Color> textColorOptions = new List<Color>();

                                    for (int i = 1; i < 9; i++)
                                    {
                                        string[] answerData = fontValues[i].Split(';');

                                        if (answerData.Length >= 4)
                                        {
                                            string answerValue1 = answerData[0];
                                            string answerValue2 = answerData[1];
                                            string answerValue3 = answerData[2];
                                            string answerValue4 = answerData[3];

                                            // Add the three data values to the answer list
                                            fontOptions.Add(answerValue1);

                                            // Parse the second value as ContentAlignment
                                            if (Enum.TryParse(answerValue2, out ContentAlignment alignment))
                                            {
                                                textAlignOptions.Add(alignment);
                                            }
                                            else
                                            {
                                                textAlignOptions.Add(ContentAlignment.TopLeft);
                                            }

                                            // Parse the third value as boolean
                                            if (bool.TryParse(answerValue3, out bool wrapValue))
                                            {
                                                textWrapOptions.Add(wrapValue);
                                            }
                                            else
                                            {
                                                textWrapOptions.Add(true);
                                            }

                                            // Retrieve the forecolor value and set it to the label's ForeColor property
                                            if (!string.IsNullOrEmpty(answerValue4))
                                            {
                                                if (int.TryParse(answerValue4.Replace("\0", ","), out int foreColorArgb))
                                                {
                                                    textColorOptions.Add(Color.FromArgb(foreColorArgb));
                                                }
                                                else
                                                {
                                                    textColorOptions.Add(Color.Black); // Default forecolor if parsing fails
                                                }
                                            }
                                        }
                                    }

                                    questions.Add(new Question(currQuestionIndex, questionText, type, options, imagePath, fontQuestion, fontOptions,
                                        textAlignQuestion, textAlignOptions, textWrapQuestion, textWrapOptions, textColor, textColorOptions));
                                    currQuestionIndex++;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid line format in CSV: " + line);
                                }
                            }

                            
                        }
                    } 
                    else
                    {

                    }
                    
                }

                currQuestionIndex = 0;
            }
            else
            {
                Console.WriteLine("CSV file does not exist: " + GlobalVariables.csvRawDataFilePath);
                questions = defaultQuestions;
            }

            if (randomQuestionsText == "Yes")
            {
                // Shuffle the questions randomly
                Random random = new Random();
                questions = questions.OrderBy(q => random.Next()).ToList();
            }

            return questions;
        }

        private void LoadRandomQuestionsAndTimerIntervalData()
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

                    if (values[1] != null)
                    {
                        randomQuestionsText = values[1];
                    }
                    if (values[0] != null && int.TryParse(values[0], out int interval))
                    {
                        timerInterval = interval * 1000;
                    } else
                    {
                        timerInterval = 10 * 1000; // Default timer interval is 10s
                    }
                    if (values[8] != null)
                        loadContentToComponent(values, submitButton);
                }
                else
                {
                    timerInterval = 10 * 1000; // Default timer interval is 10s
                }
            }
            else
            {
                timerInterval = 10 * 1000; // Default timer interval is 10s
            }
        }

        void loadContentToComponent(string[] values, Button button)
        {
            string[] textProperties = values[8].Split(';'); // Assuming text font and alignment data is at index 5
            string backgroundColor = textProperties[0]; // First ; is text font
            string foreColor = textProperties[1];

            // Retrieve the forecolor value and set it to the label's ForeColor property
            if (!string.IsNullOrEmpty(backgroundColor))
            {
                if (int.TryParse(backgroundColor.Replace("\0", ","), out int foreColorArgb))
                {
                    Color nextBackground = Color.FromArgb(foreColorArgb);
                    button.BackColor = nextBackground;
                }
                else
                {
                    button.BackColor = Color.Transparent; // Default forecolor if parsing fails
                }
            }
            if (!string.IsNullOrEmpty(foreColor))
            {
                if (int.TryParse(foreColor.Replace("\0", ","), out int foreColorArgb))
                {
                    Color nextForeground = Color.FromArgb(foreColorArgb);
                    button.ForeColor = nextForeground;
                }
                else
                {
                    button.ForeColor = Color.Black; // Default forecolor if parsing fails
                }
            }
        }

        //Helper method to get font from csv
        private Font FontFromBinaryString(string fontData)
        {
            byte[] binaryData = Convert.FromBase64String(fontData);

            using (MemoryStream stream = new MemoryStream(binaryData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Font)formatter.Deserialize(stream);
            }
        }

        private void DisplayQuestion()
        {
            // Declare an array or list to hold the radio buttons
            RadioButton[] radioButtons = new RadioButton[]
            {
                    optionARadioButton,
                    optionBRadioButton,
                    optionCRadioButton,
                    optionDRadioButton,
                    optionERadioButton,
                    optionFRadioButton,
                    optionGRadioButton,
                    optionHRadioButton
            };

            // Declare an array or list to hold the checkboxes
            CheckBox[] checkboxes = new CheckBox[]
            {
                    optionACheckBox,
                    optionBCheckBox,
                    optionCCheckBox,
                    optionDCheckBox,
                    optionECheckBox,
                    optionFCheckBox,
                    optionGCheckBox,
                    optionHCheckBox
            };

            // Hide the default radio buttons and checkboxes
            optionARadioButton.Visible = false;
            optionBRadioButton.Visible = false;
            optionCRadioButton.Visible = false;
            optionDRadioButton.Visible = false;
            optionERadioButton.Visible = false;
            optionFRadioButton.Visible = false;
            optionGRadioButton.Visible = false;
            optionHRadioButton.Visible = false;

            optionACheckBox.Visible = false;
            optionBCheckBox.Visible = false;
            optionCCheckBox.Visible = false;
            optionDCheckBox.Visible = false;
            optionECheckBox.Visible = false;
            optionFCheckBox.Visible = false;
            optionGCheckBox.Visible = false;
            optionHCheckBox.Visible = false;
           

            if (currentQuestionIndex < questions.Count)
            {
                // Reset the timer
                ResetTimer();

                Question currentQuestion = questions[currentQuestionIndex];

                //Update the background
                // Display Background
                if (!string.IsNullOrEmpty(currentQuestion.Image) && File.Exists(currentQuestion.Image))
                {
                    // Create a temporary Bitmap object from the image path
                    Bitmap backgroundImage = new Bitmap(currentQuestion.Image);

                    // Dispose of the previous background image, if one exists
                    if (this.BackgroundImage != null)
                    {
                        this.BackgroundImage.Dispose();
                    }

                    // Set the temporary Bitmap as the new background image
                    this.BackgroundImage = backgroundImage;

                    // Adjust the background image display settings
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }    

                // Update the question label
                questionLabel.Text = currentQuestion.Text;
                Font questionFont = FontFromBinaryString(currentQuestion.FontQuestion);
                questionLabel.Font = questionFont;
                questionLabel.TextAlign = currentQuestion.QuestionAlignment;
                questionLabel.AutoSize = currentQuestion.QuestionAutoSize;
                int maxWidthAxis = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.40);
                questionLabel.MaximumSize = new Size(maxWidthAxis, 0);
                questionLabel.MinimumSize = new Size(0, 0);
                questionLabel.Height = (int)Math.Ceiling(questionFont.GetHeight()) + Padding.Vertical;
                questionLabel.ForeColor = currentQuestion.QuestionColor;

                int numOptions = 8;
                int numAns = 0;

                //TODO: change to dynamic (loop)
                if (currentQuestion.Type == "MCQ")
                {
                    labelMCQorMRQ.Text = "Select one option only.";

                    // Update the options visibility and text
                    for (int i = 0; i < numOptions; i++)
                    {
                        RadioButton radioButton = radioButtons[i];
                        string optionText = currentQuestion.Options[i];

                        if (!string.IsNullOrEmpty(optionText))
                            numAns++;

                        //Remove all existing labels in controls
                        if (radioButton.Controls.Count > 0)
                        {
                            radioButton.Controls.RemoveAt(0);
                        }

                        // Create a new label to hold the text parts
                        Label label = new Label();
                        Font characterFont = new Font("Times New Roman", 36, FontStyle.Bold);
                        label.Text = GetCharacterFromIndex(i);
                        label.Font = characterFont;
                        label.AutoSize = true;
                        label.BackColor = Color.Transparent;
                        radioButton.Controls.Add(label);

                        radioButton.Visible = !string.IsNullOrEmpty(optionText); 
                        radioButton.Text = optionText;
                        radioButton.Checked = false;
                        radioButton.Font = FontFromBinaryString(currentQuestion.FontValues[i]);
                        radioButton.TextAlign = currentQuestion.TextAligns[i];
                        radioButton.AutoSize = currentQuestion.AutoSizes[i];
                        radioButton.ForeColor = currentQuestion.AnswerColors[i];

                        radioButton.AutoSize = false;

                        // Measure the required size for the text
                        SizeF textSize = TextRenderer.MeasureText(optionText, radioButton.Font);
                        int availableWidth = tableLayoutPanelRadioButton.Parent.Width;
                        int lineCount = (int)Math.Ceiling(textSize.Width / availableWidth);
                        int minHeight = (int)(textSize.Height * 2);
                        int height = (int)(textSize.Height * (lineCount + 1)) + 40; // + 20 from padding below
                        radioButton.Height = Math.Max(minHeight, height);

                        radioButton.FlatStyle = FlatStyle.Flat;
                        radioButton.Appearance = Appearance.Button;
                        radioButton.FlatAppearance.BorderSize = 0;
                        
                        // Adjust the position and size of the labels
                        int labelMarginTop = (radioButton.Height - label.Height) / 2; // Align vertically to the middle
                        label.Location = new Point(20, labelMarginTop);
                        radioButton.Padding = new Padding(label.Width + 20, 10, 20, 10);

                        SetRoundedShape(radioButton);
                    }

                    // Hide checkboxes
                    foreach (CheckBox checkbox in checkboxes)
                    {
                        checkbox.Visible = false;
                        checkbox.Text = string.Empty;
                        checkbox.Checked = false;
                    }

                    tableLayoutPanelRadioButton.Visible = true;
                    tableLayoutPanelCheckBox.Visible = false;

                    AdjustRadioTableLayoutPanelScroll(numAns);

                } else
                {
                    labelMCQorMRQ.Text = "Select multiple options.";

                    // Update the options visibility and text
                    for (int i = 0; i < numOptions; i++)
                    {
                        CheckBox checkbox = checkboxes[i];
                        string optionText = currentQuestion.Options[i];

                        if (!string.IsNullOrEmpty(optionText))
                            numAns++;

                        //Remove all existing labels in controls
                        if (checkbox.Controls.Count > 0)
                        {
                            checkbox.Controls.RemoveAt(0);
                        }

                        // Create a new label to hold the text parts
                        Label label = new Label();
                        Font characterFont = new Font("Times New Roman", 36, FontStyle.Bold);
                        label.Text = GetCharacterFromIndex(i);
                        label.Font = characterFont;
                        label.AutoSize = true;
                        label.BackColor = Color.Transparent;
                        checkbox.Controls.Add(label);

                        checkbox.Visible = !string.IsNullOrEmpty(optionText);
                        checkbox.Text = optionText;
                        checkbox.Checked = false;
                        Font textFont = FontFromBinaryString(currentQuestion.FontValues[i]);
                        checkbox.Font = textFont;
                        checkbox.TextAlign = currentQuestion.TextAligns[i];
                        checkbox.AutoSize = currentQuestion.AutoSizes[i];
                        checkbox.ForeColor = currentQuestion.AnswerColors[i];

                        checkbox.AutoSize = false; 

                        // Measure the required size for the text
                        SizeF textSize = TextRenderer.MeasureText(optionText, checkbox.Font);
                        int availableWidth = tableLayoutPanelCheckBox.Parent.Width;
                        int lineCount = (int)Math.Ceiling(textSize.Width / availableWidth);
                        int minHeight = (int)(textSize.Height * 2);
                        int height = (int)(textSize.Height * (lineCount + 1)) + 40; // + 20 from padding below
                        checkbox.Height = Math.Max(minHeight, height);

                        checkbox.FlatStyle = FlatStyle.Flat;
                        checkbox.Appearance = Appearance.Button;
                        checkbox.FlatAppearance.BorderSize = 0;

                        // Adjust the position and size of the labels
                        int labelMarginTop = (checkbox.Height - label.Height) / 2; // Align vertically to the middle
                        label.Location = new Point(20, labelMarginTop);
                        checkbox.Padding = new Padding(label.Width + 20, 10, 20, 10);

                        SetRoundedShape(checkbox);
                    }
                    // Hide radio buttons
                    foreach (RadioButton radioButton in radioButtons)
                    {
                        radioButton.Visible = false;
                        radioButton.Text = string.Empty;
                        radioButton.Checked = false;
                    }

                    tableLayoutPanelRadioButton.Visible = false;
                    tableLayoutPanelCheckBox.Visible = true;

                    AdjustCheckboxTableLayoutPanelScroll(numAns);

                }
                // Enable the submit button
                submitButton.Enabled = true;
                CenterQuestionLabel();
            }
            else
            {
                foreach (CheckBox checkbox in checkboxes)
                {
                    checkbox.Enabled = false;
                }

                foreach (RadioButton radioButton in radioButtons)
                {
                    radioButton.Enabled = false;
                }

                submitButton.Enabled = false;

                AppendDataToSpecificRow(GlobalVariables.csvRawDataFilePath, rowNumber, string.Join(",", savedAnswers));
                
                timer.Stop();
                ThankYouScreen thankYouForm = new ThankYouScreen();
                thankYouForm.Show();
                this.Close();
            }



        }

        // Helper method to draw arc
        private GraphicsPath SetRoundedShape(Control control)
        {
            int borderRadius = 40; // Adjust the radius as needed
            GraphicsPath path = new GraphicsPath();
            // Top-left corner
            path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
            // Top-right corner
            path.AddArc(control.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
            // Bottom-right corner
            path.AddArc(control.Width - borderRadius, control.Height - borderRadius, borderRadius, borderRadius, 0, 90);
            // Bottom-left corner
            path.AddArc(0, control.Height - borderRadius, borderRadius, borderRadius, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);

            // Store the previous path in the control's tag
            if (control.Tag is GraphicsPath previousPath)
                previousPath.Dispose();
            control.Tag = path;

            SubscribePaintEvent(control);

            return path;
        }

        private void SubscribePaintEvent(Control control)
        {
            control.Paint -= RoundedShapePaint;
            control.Paint += RoundedShapePaint;
        }

        private void RoundedShapePaint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            GraphicsPath path = control.Tag as GraphicsPath;

            if (path != null)
            {
                using (var pen = new Pen(control.ForeColor, 5))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Draw path
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        //Helper method to get the letter for displaying answer
        private string GetCharacterFromIndex(int index)
        {
            string character;
            switch (index)
            {
                case 0:
                    character = "A";
                    break;
                case 1:
                    character = "B";
                    break;
                case 2:
                    character = "C";
                    break;
                case 3:
                    character = "D";
                    break;
                case 4:
                    character = "E";
                    break;
                case 5:
                    character = "F";
                    break;
                case 6:
                    character = "G";
                    break;
                case 7:
                    character = "H";
                    break;
                default:
                    character = ""; // Handle other index values if needed
                    break;
            }
            return character;
        }

        private void AdjustRadioTableLayoutPanelScroll(int numAns)
        {
            int totalHeight = 0;

            // Iterate through each row in the TableLayoutPanel
            for (int row = 0; row < numAns; row++)
            {
                int rowHeight = GetRadioButtonRowHeight(tableLayoutPanelRadioButton, row);
                totalHeight += rowHeight;
                
            }

            int availableHeight = tableLayoutPanelRadioButton.Parent.Height;

            if (totalHeight > availableHeight)
            {
                tableLayoutPanelRadioButton.VerticalScroll.Value = 0;
                // To reset the scroll bar
                tableLayoutPanelRadioButton.AutoScroll = false;
                tableLayoutPanelRadioButton.AutoScroll = true;
            }
            else
            {
                tableLayoutPanelRadioButton.AutoScroll = false;
            }
        }

        private int GetRadioButtonRowHeight(TableLayoutPanel tableLayoutPanel, int row)
        {
            int radioButtonHeight = 0;

            // Check if the row contains a RadioButton control
            foreach (Control control in tableLayoutPanel.Controls)
            {
                if (tableLayoutPanel.GetRow(control) == row && control is RadioButton radioButton)
                {
                    radioButtonHeight = radioButton.Height;
                    break;
                }
            }

            return radioButtonHeight;
        }


        private void AdjustCheckboxTableLayoutPanelScroll(int numAns)
        {
            int totalHeight = 0;

            // Iterate through each row in the TableLayoutPanel
            for (int row = 0; row < numAns; row++)
            {
                int rowHeight = GetCheckBoxRowHeight(tableLayoutPanelCheckBox, row);
                totalHeight += rowHeight;
            }

            int availableHeight = tableLayoutPanelCheckBox.Parent.Height;

            if (totalHeight > availableHeight)
            {
                tableLayoutPanelCheckBox.VerticalScroll.Value = 0;
                // To reset the scroll bar
                tableLayoutPanelCheckBox.AutoScroll = false;
                tableLayoutPanelCheckBox.AutoScroll = true;
                tableLayoutPanelCheckBox.HorizontalScroll.Visible = false;
            }
            else
            {
                tableLayoutPanelCheckBox.AutoScroll = false;
            }
        }

        private int GetCheckBoxRowHeight(TableLayoutPanel tableLayoutPanel, int row)
        {
            int checkBoxHeight = 0;

            // Check if the row contains a RadioButton control
            foreach (Control control in tableLayoutPanel.Controls)
            {
                if (tableLayoutPanel.GetRow(control) == row && control is CheckBox checkBox)
                {
                    checkBoxHeight = checkBox.Height;
                    break; 
                }
            }

            return checkBoxHeight;
        }


        //for checkboxes
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Loop through the radio buttons on the form and its child controls
            LoopThroughControls(this);

            void LoopThroughControls(Control parentControl)
            {
                foreach (Control control in parentControl.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Name.StartsWith("option"))
                    {
                        // Extract the radio button name (e.g., optionA, optionB, etc.)
                        string checkboxName = checkBox.Name.Replace("CheckBox", "");
                        SetRoundedShape(checkBox);

                        if (checkBox.Checked)
                        {
                            checkBox.Paint -= CheckBox_Paint; // Unsubscribe from the previous Paint event handler
                            checkBox.Paint += CheckBox_Paint; // Subscribe to the new Paint event handler
                            checkboxValues[checkboxName] = checkBox.Text;
                        }
                        else
                        {
                            checkBox.Paint -= CheckBox_Paint; // Unsubscribe from the Paint event handler
                            checkBox.BackColor = Color.White;
                            checkboxValues[checkboxName] = " ";
                        }
                        
                    }
                    // Recursively loop through child controls if the control is a container control
                    if (control.Controls.Count > 0)
                    {
                        LoopThroughControls(control);
                    }
                }
            }

        }

        private void CheckBox_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            // Set the background color with transparency
            Color backgroundColor = Color.FromArgb(100, Color.Green);

            
            // Create a brush with the translucent background color
            Brush brush = new SolidBrush(backgroundColor);

            // Draw the background
            e.Graphics.FillRectangle(brush, checkBox.ClientRectangle);

            // Clean up resources
            brush.Dispose();

        }


        //for radio buttons
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {

            // Loop through the radio buttons on the form and its child controls
            LoopThroughControls(this);

            void LoopThroughControls(Control parentControl)
            {
                foreach (Control control in parentControl.Controls)
                {
                    if (control is RadioButton radioButton && radioButton.Name.StartsWith("option"))
                    {
                        // Extract the radio button name (e.g., optionA, optionB, etc.)
                        string radioButtonName = radioButton.Name.Replace("RadioButton", "");
                        SetRoundedShape(radioButton);

                        if (radioButton.Checked)
                        {
                            radioButton.Paint -= RadioButton_Paint; // Unsubscribe from the previous Paint event handler
                            radioButton.Paint += RadioButton_Paint; // Subscribe to the new Paint event handler
                            radioButton.BackColor = Color.Transparent;
                            checkboxValues[radioButtonName] = radioButton.Text;
                        }
                        else
                        {
                            radioButton.Paint -= RadioButton_Paint; // Unsubscribe from the Paint event handler
                            radioButton.BackColor = Color.White;
                            checkboxValues[radioButtonName] = " ";
                        }
                    }

                    // Recursively loop through child controls if the control is a container control
                    if (control.Controls.Count > 0)
                    {
                        LoopThroughControls(control);
                    }
                }
            }
        }

        private void RadioButton_Paint(object sender, PaintEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            // Set the background color with transparency
            Color backgroundColor = Color.FromArgb(100, Color.Green);

            // Create a brush with the translucent background color
            Brush brush = new SolidBrush(backgroundColor);

            // Draw the background
            e.Graphics.FillRectangle(brush, radioButton.ClientRectangle);

            // Clean up resources
            brush.Dispose();
        }


        private void saveAnswersToArray() {

            if (checkboxValues == null || checkboxValues.Count == 0)
            {
                for (int i = 0; i < optionsNumber; i++)
                {
                    string key = "option" + (char)('A' + i);
                    string value = " ";
                    checkboxValues.Add(key, value);
                }
            }

            // Convert the dictionary to a list of key-value pairs
            List<KeyValuePair<string, string>> sortedList = checkboxValues.ToList();

            // Sort the list by the first string value
            sortedList.Sort((x, y) => string.Compare(x.Key, y.Key));

            savedAnswers[questions[currentQuestionIndex].Index] = string.Join(";", sortedList.Select(kv => kv.Value));

            checkboxValues.Clear();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            saveAnswersToArray();

            // Reset the timer
            ResetTimer();

            // Move to the next question
            currentQuestionIndex++;

            // Display the next question
            DisplayQuestion();
        }

        // Appends player's answer to same csv row as points saved in TableForm
        private void AppendDataToSpecificRow(string csvFilePath, int rowNumber, string rowData)
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

                if (rowNumber >= 0 && rowNumber < lines.Length)
                {
                    lines[rowNumber] = lines[rowNumber] + "," + rowData; // Replace the row with the new data
                    File.WriteAllLines(csvFilePath, lines); // Rewrite the entire file with the updated row
                }
                else
                {
                    // Row number is out of range
                    Console.WriteLine("Invalid row number.");
                }
            }
            else
            {
                // File doesn't exist
                Console.WriteLine("CSV file does not exist.");
            }
        }

        // Timer for question
        private void Timer_Tick(object sender, EventArgs e)
        {
            saveAnswersToArray();
            // Timer elapsed, redirect to Form1
            AppendDataToSpecificRow(GlobalVariables.csvRawDataFilePath, rowNumber, string.Join(",", savedAnswers));
            
            timer.Stop();
            TableForm form1 = new TableForm();
            form1.Show();
            this.Close();
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

       
    }

    public class Question
    {
        public int Index { get; set; }
        public string Text { get; set; }

        public string Type { get; set; }

        public string Image { get; set; }

        public List<string> Options { get; set; }

        //Fonts
        public string FontQuestion { get; set; }

        public List<string> FontValues { get; set; }

        public List<ContentAlignment> TextAligns { get; set; }

        public ContentAlignment QuestionAlignment { get; set; }

        public bool QuestionAutoSize { get; set; }

        public List<bool> AutoSizes { get; set; }

        public Color QuestionColor { get; set; }

        public List<Color> AnswerColors { get; set; }

        public Question(int index, string text, string type, List<string> options, string imagePath, 
            string fontQuestion, List<string> fontValues, ContentAlignment questionAlignment, List<ContentAlignment> textAligns, 
            bool questionAutoSize, List<bool> autoSizes, Color questionColor, List<Color> answerColors)
        {
            Index = index;
            Text = text;
            Type = type;
            Options = options;
            Image = imagePath;
            FontQuestion = fontQuestion;
            FontValues = fontValues;
            QuestionAlignment = questionAlignment;
            TextAligns = textAligns;
            QuestionAutoSize = questionAutoSize;
            AutoSizes = autoSizes;
            QuestionColor = questionColor;
            AnswerColors = answerColors;
        }
    }
}

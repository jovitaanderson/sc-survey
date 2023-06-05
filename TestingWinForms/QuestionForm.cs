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
                new Question(0, "MCQ", "Question 1", new List<string> { "Option A", "Option B", "Option C", "Option D", "Option E", "Option C", "Option D", "Option E",}, ""),
                new Question(1, "MCQ", "Question 2", new List<string> { "Option D", "Option E", "Option F", "Option D", "Option E", "Option C", "Option D", "Option E"}, ""),
                new Question(2, "MRQ", "Question 3", new List<string> { "Option G", "Option H", "Option I", "Option D", "Option E", "Option C", "Option D", "Option E" }, ""),
            };

            InitializeComponent();
            DoubleBuffered = true;
            BackgroundImageLayout = ImageLayout.None;


            tableLayoutPanel1.SuspendLayout();

            // Check if admin wants questions to be in random
            LoadRandomQuestionsAndTimerIntervalData();

            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

            this.rowNumber = rowNumber;

            // Subscribe the same event handler method to the CheckedChanged event of each checkbox
            optionACheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionBCheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionCCheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionDCheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionECheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionFCheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionGCheckBox.CheckedChanged += CheckBox_CheckedChanged;
            optionHCheckBox.CheckedChanged += CheckBox_CheckedChanged;


            //subscribe for radio Buttons
            optionARadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionBRadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionCRadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionDRadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionERadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionFRadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionGRadioButton.CheckedChanged += RadioButton_CheckedChanged;
            optionHRadioButton.CheckedChanged += RadioButton_CheckedChanged;

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
            DisplayQuestion(); //Display first question
            //DisplayBackground();
            tableLayoutPanel1.ResumeLayout();
            
            
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
                questionLabel.MaximumSize = new Size(maxWidth, 0);
            }

            questionLabel.TextAlign = ContentAlignment.MiddleCenter;
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

        /*private void DisplayBackground() 
        {
            // Suspend layout updates
            this.SuspendLayout();

            // Enable double buffering to reduce flickering
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            
            // Display background image
            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                // Create a temporary Bitmap object from the image path
                Bitmap backgroundImage = new Bitmap(imagePath);

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

            // Resume layout updates
            this.ResumeLayout(true);
        }*/

        /*private string LoadBackgroundImageFromCSV()
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

                    if (values[4] != null)
                    {
                        string imagePath = Path.Combine(values[4]);
                        return imagePath;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }*/


        // Returns a List<Question> consisting of questions saved in admin_question.csv file
        private List<Question> LoadQuestionsFromCSV()
        {
            List<Question> questions = new List<Question>();

            int currQuestionIndex = 0;

            if (File.Exists(GlobalVariables.csvAdminQuestionsFilePath))
            {
                using (var reader = new StreamReader(GlobalVariables.csvAdminQuestionsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (values.Length >= 2) // Assuming each line in the CSV has at least 4 values: question, option1, option2, option3
                        {
                            string imagePath = Path.Combine(values[0]);
                            string questionText = values[1];
                            string type = values[2];
                            
                            List<string> options = new List<string> { values[3], values[4], 
                                values[5], values[6], values[7], values[8], values[9], values[10]  };

                            questions.Add(new Question(currQuestionIndex, questionText, type, options, imagePath));
                            currQuestionIndex++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format in CSV: " + line);
                        }
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
                int numOptions = 8;

                //TODO: change to dynamic (loop)
                if (currentQuestion.Type == "MCQ")
                {
                    labelMCQorMRQ.Text = "Select one option only.";

                    // Update the options visibility and text
                    for (int i = 0; i < numOptions; i++)
                    {
                        RadioButton radioButton = radioButtons[i];
                        string optionText = currentQuestion.Options[i];

                        radioButton.Visible = !string.IsNullOrEmpty(optionText);
                        radioButton.Text = optionText;
                        radioButton.Checked = false;
                    }

                    // Clear the selection for any remaining radio buttons
                    for (int i = numOptions; i < radioButtons.Length; i++)
                    {
                        RadioButton radioButton = radioButtons[i];
                        radioButton.Visible = false;
                        radioButton.Text = string.Empty;
                        radioButton.Checked = false;
                    }

                    // Hide checkboxes
                    foreach (CheckBox checkbox in checkboxes)
                    {
                        checkbox.Visible = false;
                        checkbox.Text = string.Empty;
                        checkbox.Checked = false;
                    }

                } else
                {
                    labelMCQorMRQ.Text = "Select multiple options.";

                    // Update the options visibility and text
                    for (int i = 0; i < numOptions; i++)
                    {
                        CheckBox checkbox = checkboxes[i];
                        string optionText = currentQuestion.Options[i];

                        checkbox.Visible = !string.IsNullOrEmpty(optionText);
                        checkbox.Text = optionText;
                        checkbox.Checked = false;
                    }

                    // Clear the selection for any remaining checkboxes
                    for (int i = numOptions; i < checkboxes.Length; i++)
                    {
                        CheckBox checkbox = checkboxes[i];
                        checkbox.Visible = false;
                        checkbox.Text = string.Empty;
                        checkbox.Checked = false;
                    }

                    // Hide radio buttons
                    foreach (RadioButton radioButton in radioButtons)
                    {
                        radioButton.Visible = false;
                        radioButton.Text = string.Empty;
                        radioButton.Checked = false;
                    }
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

                        if (checkBox.Checked)
                        {
                            checkBox.Paint -= CheckBox_Paint; // Unsubscribe from the previous Paint event handler
                            checkBox.Paint += CheckBox_Paint; // Subscribe to the new Paint event handler
                            //checkBox.BackColor = Color.Green;
                            checkboxValues[checkboxName] = checkBox.Text;
                        }
                        else
                        {
                            checkBox.Paint -= CheckBox_Paint; // Unsubscribe from the Paint event handler
                            //checkBox.BackColor = Color.Transparent;
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
                            radioButton.BackColor = Color.Transparent;
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
        public Question(int index, string text, string type, List<string> options, string imagePath)
        {
            Index = index;
            Text = text;
            Type = type;
            Options = options;
            Image = imagePath;
        }
    }
}

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
        private string csvFilePath = "player_answers.csv";
        private int rowNumber;

        private string csvAdminQuestionsFilePath = "admin_questions.csv"; // Path to the CSV file
        private string csvAdminAdvanceFilePath = "admin_advance.csv";
        private List<string> autoSelectedOptions = new List<string>(); // Stores the selected checkboxes
        private string randomQuestionsText = null;

        String[] savedAnswers; //Stores answers to each question

        Dictionary<string, string> checkboxValues = new Dictionary<string, string>(); // Stores answers on checkbox change
        int totalOptions = 8;

        bool[] isQuestionMRQ = new bool[] { false, true, true, true, false, true, true, true };

        public QuestionForm(int rowNumber)
        {
            // Initialize the list of questions
            defaultQuestions = new List<Question>()
            {
                new Question(0, "MCQ", "Question 1", new List<string> { "Option A", "Option B", "Option C", "Option D", "Option E", "Option C", "Option D", "Option E" }),
                new Question(1, "MCQ", "Question 2", new List<string> { "Option D", "Option E", "Option F", "Option D", "Option E", "Option C", "Option D", "Option E"}),
                new Question(2, "MRQ", "Question 3", new List<string> { "Option G", "Option H", "Option I", "Option D", "Option E", "Option C", "Option D", "Option E" })
            };

            InitializeComponent();
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

            string semicolonString = " " + string.Join("; ", new string[totalOptions]);

            for (int i = 0; i < savedAnswers.Length; i++)
            {
                savedAnswers[i] = semicolonString;
            }


            DisplayBackground();
            DisplayQuestion(); //Display first question
        }

        private void DisplayBackground() {

            //Display background image
            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                // Set the background image of the Windows Forms application
                this.BackgroundImage = Image.FromFile(imagePath);

                // Adjust the background image display settings
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Center the label and checkboxes on the form
            questionLabel.TextAlign = ContentAlignment.MiddleCenter;
            optionACheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionBCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionCCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionDCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionECheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionFCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionGCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            optionHCheckBox.TextAlign = ContentAlignment.MiddleCenter;

        }


        private string LoadBackgroundImageFromCSV()
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

                    if (values[3] != null)
                    {
                        string imagePath = Path.Combine(values[3]);
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
        }


        // Returns a List<Question> consisting of questions saved in admin_question.csv file
        private List<Question> LoadQuestionsFromCSV()
        {
            List<Question> questions = new List<Question>();

            int currQuestionIndex = 0;

            if (File.Exists(csvAdminQuestionsFilePath))
            {
                using (var reader = new StreamReader(csvAdminQuestionsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (values.Length >= 2) // Assuming each line in the CSV has at least 4 values: question, option1, option2, option3
                        {
                            string questionText = values[0];
                            List<string> options = new List<string> { values[1], values[2], values[3], values[4], 
                                values[5], values[6], values[7], values[8] };

                            questions.Add(new Question(currQuestionIndex, questionText, "MCQ", options));
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
                Console.WriteLine("CSV file does not exist: " + csvFilePath);
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


        private void CheckBox_MCQ(object sender, EventArgs e)
        {
            CheckBox clickedCheckBox = (CheckBox)sender;

            // Uncheck all other checkboxes except the clicked checkbox
            foreach (Control control in Controls)
            {
                if (control is CheckBox checkBox && checkBox != clickedCheckBox)
                {
                    checkBox.Checked = false;
                }
            }
        }
        private void DisplayQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                // Reset the timer
                ResetTimer();

                Question currentQuestion = questions[currentQuestionIndex];

                if (currentQuestion.Type == "MCQ")
                {
                    optionACheckBox.CheckedChanged += CheckBox_MCQ;
                    optionBCheckBox.CheckedChanged += CheckBox_MCQ;
                    optionCCheckBox.CheckedChanged += CheckBox_MCQ;
                    optionDCheckBox.CheckedChanged += CheckBox_MCQ;
                    optionECheckBox.CheckedChanged += CheckBox_MCQ;
                }
                else {
                    optionACheckBox.CheckedChanged -= CheckBox_MCQ;
                    optionBCheckBox.CheckedChanged -= CheckBox_MCQ;
                    optionCCheckBox.CheckedChanged -= CheckBox_MCQ;
                    optionDCheckBox.CheckedChanged -= CheckBox_MCQ;
                    optionECheckBox.CheckedChanged -= CheckBox_MCQ;
                }

                // Update the question label
                questionLabel.Text = currentQuestion.Text;

                //TODO: change to dynamic (loop)
                // Update the options
                optionACheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[0]);
                optionBCheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[1]);
                optionCCheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[2]);
                optionDCheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[3]);
                optionECheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[4]);
                optionFCheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[5]);
                optionGCheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[6]);
                optionHCheckBox.Visible = !string.IsNullOrEmpty(currentQuestion.Options[7]);

                // Set the checkbox text
                optionACheckBox.Text = currentQuestion.Options[0];
                optionBCheckBox.Text = currentQuestion.Options[1];
                optionCCheckBox.Text = currentQuestion.Options[2];
                optionDCheckBox.Text = currentQuestion.Options[3];
                optionECheckBox.Text = currentQuestion.Options[4];
                optionFCheckBox.Text = currentQuestion.Options[5];
                optionGCheckBox.Text = currentQuestion.Options[6];
                optionHCheckBox.Text = currentQuestion.Options[7];

                // Clear the check box selection
                optionACheckBox.Checked = false;
                optionBCheckBox.Checked = false;
                optionCCheckBox.Checked = false;
                optionDCheckBox.Checked = false;
                optionECheckBox.Checked = false;
                optionFCheckBox.Checked = false;
                optionGCheckBox.Checked = false;
                optionHCheckBox.Checked = false;

                // Enable the submit button
                submitButton.Enabled = true;
            }
            else
            {
                // No more questions, display a completion message
                questionLabel.Text = "Quiz completed!";

                // Disable the check boxes and submit button
                optionACheckBox.Enabled = false;
                optionBCheckBox.Enabled = false;
                optionCCheckBox.Enabled = false;
                optionDCheckBox.Enabled = false;
                optionECheckBox.Enabled = false;
                optionFCheckBox.Enabled = false;
                optionGCheckBox.Enabled = false;
                optionHCheckBox.Enabled = false;
                submitButton.Enabled = false;

                AppendDataToSpecificRow(csvFilePath, rowNumber, string.Join(",", savedAnswers));
                
                timer.Stop();
                ThankYouScreen thankYouForm = new ThankYouScreen();
                thankYouForm.Show();
                this.Close();
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Loop through the checkboxes on the form
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox checkbox && checkbox.Name.StartsWith("option"))
                {
                    // Extract the checkbox name (e.g., optionA, optionB, etc.)
                    string checkboxName = checkbox.Name.Replace("CheckBox", "");

                    if (checkbox.Checked)
                    {
                        checkboxValues[checkboxName] = checkbox.Text;
                    }
                    else
                    {
                        checkboxValues[checkboxName] = " ";
                    }
                }
            }

        }

        private void saveAnswersToArray() {

            if (checkboxValues == null || checkboxValues.Count == 0)
            {
                for (int i = 0; i < totalOptions; i++)
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
            AppendDataToSpecificRow(csvFilePath, rowNumber, string.Join(",", savedAnswers));
            
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

        public List<string> Options { get; set; }
        public Question(int index, string text, string type, List<string> options)
        {
            Index = index;
            Text = text;
            Type = type;
            Options = options;
        }
    }
}

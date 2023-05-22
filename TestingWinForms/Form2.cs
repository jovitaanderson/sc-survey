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
    public partial class Form2 : Form
    {
        private List<Question> questions;
        private int currentQuestionIndex;
        private Timer timer;
        private const int TimerInterval = 5000; // 3 seconds in milliseconds

        public Form2()
        {
            InitializeComponent();

            // Initialize the list of questions
            questions = new List<Question>()
            {
                new Question("Question 1", new List<string> { "Option A", "Option B", "Option C" }),
                new Question("Question 2", new List<string> { "Option D", "Option E", "Option F" }),
                new Question("Question 3", new List<string> { "Option G", "Option H", "Option I" })
            };

            currentQuestionIndex = 0;

            // Set up the timer
            timer = new Timer();
            timer.Interval = TimerInterval;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Display the first question
            DisplayQuestion();

        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                Console.WriteLine(questions.Count);
                // Reset the timer
                ResetTimer();

                Question currentQuestion = questions[currentQuestionIndex];

                // Update the question label
                questionLabel.Text = currentQuestion.Text;

                // Update the options
                optionACheckBox.Text = currentQuestion.Options[0];
                optionBCheckBox.Text = currentQuestion.Options[1];
                optionCCheckBox.Text = currentQuestion.Options[2];

                // Clear the check box selection
                optionACheckBox.Checked = false;
                optionBCheckBox.Checked = false;
                optionCCheckBox.Checked = false;

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
                submitButton.Enabled = false;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {

            // Reset the timer
            ResetTimer();

            // Get the selected options
            List<string> selectedOptions = new List<string>();

            if (optionACheckBox.Checked)
                selectedOptions.Add(optionACheckBox.Text);
            if (optionBCheckBox.Checked)
                selectedOptions.Add(optionBCheckBox.Text);
            if (optionCCheckBox.Checked)
                selectedOptions.Add(optionCCheckBox.Text);

            // Save the selected options (you can modify this to save the data to a file or a database)
            SaveSelectedOptions(selectedOptions);

            // Move to the next question
            currentQuestionIndex++;

            // Display the next question
            DisplayQuestion();
        }

        private void SaveSelectedOptions(List<string> selectedOptions)
        {
            // Create a string representing the current selected options
            string selectedOptionsString = string.Join(";", selectedOptions);

            // Create the CSV header if the file doesn't exist
            if (!File.Exists("selected_options.csv"))
            {
                string header = "q1,q2,q3" + Environment.NewLine;
                File.WriteAllText("selected_options.csv", header);
            }

            // Read the existing content of the CSV file
            string existingContent = File.ReadAllText("selected_options.csv");

            // Split the existing content by lines
            string[] lines = existingContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Determine the row index to update
            int rowIndex = lines.Length;

            // Split each line by commas
            string[] columns = lines[rowIndex - 1].Split(',');

            // Update the current selected options for each column
            for (int i = 0; i < selectedOptions.Count; i++)
            {
                // Append the selected option to the existing value in the corresponding column
                columns[i] = columns[i].Trim() + ";" + selectedOptions[i];
            }

            // Join the columns back into a single line
            lines[rowIndex - 1] = string.Join(",", columns);

            // Concatenate all lines back into the updated content
            string newContent = string.Join(Environment.NewLine, lines);

            // Rewrite the entire file with the updated content
            File.WriteAllText("selected_options.csv", newContent);

        }

        // Timer for question
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Timer elapsed, redirect to Form1
            timer.Stop();
           // MessageBox.Show("Time's up! Redirecting to Form1...");
            //timer.Stop();
            //ResetTimer();
            //this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
            //form1.ShowDialog();
            this.Close();
        }

        private void ResetTimer()
        {
            // Reset the timer
            timer.Stop();
            timer.Start();
        }
    }

    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }

        public Question(string text, List<string> options)
        {
            Text = text;
            Options = options;
        }
    }
}

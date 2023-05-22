using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            // Display the first question
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
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
            // Implement your own code to save the selected options
            Console.WriteLine("Selected Options: " + string.Join(", ", selectedOptions));
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

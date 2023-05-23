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
    public partial class Form2 : Form
    {
        private List<Question> questions;
        private int currentQuestionIndex;
        private Timer timer;
        private const int TimerInterval = 5000; // 3 seconds in milliseconds
        private string csvFilePath = "player_answers.csv";
        private int rowNumber; 

        public Form2(int rowNumber)
        {
            InitializeComponent();
            this.rowNumber = rowNumber;

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

                timer.Stop();
                //timer = new System.Threading.Timer(OnTimerElapsed, null, 2000, Timeout.Infinite); // Start the timer for 3 seconds
                Form1 form1 = new Form1();
                form1.Show();
                this.Close();
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

            string joinedOptions = string.Join(";", selectedOptions);

            // Save the selected options (you can modify this to save the data to a file or a database)
            //SaveSelectedOptions(selectedOptions);
            AppendDataToSpecificRow(csvFilePath, rowNumber, joinedOptions);

            // Move to the next question
            currentQuestionIndex++;

            // Display the next question
            DisplayQuestion();
        }

        private void AppendDataToSpecificRow(string csvFilePath, int rowNumber, string rowData)
        {
            if (File.Exists(csvFilePath))
            {
                string[] lines = File.ReadAllLines(csvFilePath);
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
            // Timer elapsed, redirect to Form1
            timer.Stop();
            Form1 form1 = new Form1();
            form1.Show();
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

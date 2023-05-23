﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace TestingWinForms
{
    public partial class Form1 : Form
    {
        private List<Point> existingClickedPositions; // Stores the previous saved clicked positions
        private Point clickedPosition; // Stores the current clicked positions
        private Rectangle drawingArea = new Rectangle(100, 50, 200, 200); // Defines the drawing area 
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds
        private string csvFilePath = "player_answers.csv"; // Path to the CSV file
        private const string columnNames = "point_x,point_y,question1,question2,question3";
        private int timerToQuestionPage = 1000;
        private int lastRowNumber;

        public Form1()
        {
            InitializeComponent();
            existingClickedPositions = new List<Point>();

            //Create file with column header if file does not exits
            if (!File.Exists(csvFilePath))
            {
                string csvHeader = $"{columnNames}" + Environment.NewLine;
                File.WriteAllText(csvFilePath, csvHeader, Encoding.UTF8);
            }

            this.MouseClick += Form1_MouseClick; // Wire up the event handler
            LoadPointsFromCSV(); // Load points from CSV file
        }

        private void LoadPointsFromCSV()
        {
            // Load all existing points to screen
            if (File.Exists(csvFilePath))
            {
                string[] lines = File.ReadAllLines(csvFilePath);
                lastRowNumber = lines.Length;

                // Get the index of the "point_x" and "point_y" columns
                string[] headers = lines[0].Split(',');
                int pointXIndex = Array.IndexOf(headers, "point_x");
                int pointYIndex = Array.IndexOf(headers, "point_y");

                for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header row
                {
                    //Console.WriteLine(lines[i]);
                    string[] parts = lines[i].Split(',');
                    if (parts.Length > pointXIndex && parts.Length > pointYIndex &&
                        int.TryParse(parts[pointXIndex], out int x) && int.TryParse(parts[pointYIndex], out int y))
                    {
                        existingClickedPositions.Add(new Point(x, y));
                    }
                }

                Refresh(); // Redraw the form to display the loaded points
            }
        }

        private void SavePointsToCSV()
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, true))
            {
                writer.WriteLine($"{clickedPosition.X},{clickedPosition.Y}");
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (drawingArea.Contains(e.Location))
            {
                existingClickedPositions.Add(e.Location);
                clickedPosition = e.Location; 

                Refresh(); // Redraw the form to display the dots
                SavePointsToCSV();

                //For debugging
                UpdatePositionLabel(e.Location); // Update the position label

                timer = new System.Threading.Timer(OnTimerElapsed, null, timerToQuestionPage, Timeout.Infinite); // Start the timer for x seconds
            }
        }

        private void OnTimerElapsed(object state)
        {
            // Invoke the navigation to a new page on the UI thread
            Invoke(new Action(() =>
            {
                timer.Dispose(); // Dispose the timer
                timer = null; // Set the timer reference to null

                int currentRowNumber = lastRowNumber; // Get the last row number and increment by 1


                Form2 newForm = new Form2(currentRowNumber); // Navigate to a new page
                newForm.Show();
                this.Hide();
            }));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, drawingArea, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid);

            // Paint existing dots
            foreach (Point position in existingClickedPositions)
            {
                if (drawingArea.Contains(position))
                {
                    int dotX = position.X - dotSize / 2;
                    int dotY = position.Y - dotSize / 2;

                    e.Graphics.FillEllipse(Brushes.Blue, dotX, dotY, dotSize, dotSize);
                }
            }

            //Paint current dot
            if (drawingArea.Contains(clickedPosition))
            {
                int dotX = clickedPosition.X - dotSize / 2;
                int dotY = clickedPosition.Y - dotSize / 2;

                e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
            }
        }

        //For debugging
        private void UpdatePositionLabel(Point position)
        {
            lbl_clickMessage.Text = $"X: {position.X}, Y: {position.Y}";
        }
    }
}
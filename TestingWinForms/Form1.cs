using System;
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
        private Rectangle drawingArea; // Defines the drawing area 
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private System.Threading.Timer timer; // Timer to wait for 3 seconds
        private string csvFilePath = "player_answers.csv"; // Path to the CSV file
        private const string columnNames = "point_x,point_y,question1,question2,question3";

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

            drawingArea = new Rectangle(100, 50, 200, 200); // Define the drawing area
            this.MouseClick += Form1_MouseClick; // Wire up the event handler
            LoadPointsFromCSV(); // Load points from CSV file
        }

        private void LoadPointsFromCSV()
        {
            // Load all existing points to screen
            if (File.Exists(csvFilePath))
            {
                string[] lines = File.ReadAllLines(csvFilePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
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

                //foreach (Point position in clickedPositions)
                //{
                    writer.WriteLine($"{clickedPosition.X},{clickedPosition.Y}");
                //}
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (drawingArea.Contains(e.Location))
            {
                existingClickedPositions.Add(e.Location);
                clickedPosition = e.Location; 

                Refresh(); // Redraw the form to display the dots
                UpdatePositionLabel(e.Location); // Update the position label
                timer = new System.Threading.Timer(OnTimerElapsed, null, 2000, Timeout.Infinite); // Start the timer for 3 seconds
                Refresh(); // Redraw the form to display the dots
                SavePointsToCSV();
            }
        }

        private void OnTimerElapsed(object state)
        {
            // Invoke the navigation to a new page on the UI thread
            Invoke(new Action(() =>
            {
                timer.Dispose(); // Dispose the timer
                timer = null; // Set the timer reference to null

                // Navigate to a new page
                Form2 newForm = new Form2();
                newForm.Show();
                this.Hide();
            }));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, drawingArea, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid, Color.Black, drawingAreaBorderWidth, ButtonBorderStyle.Solid);

            foreach (Point position in existingClickedPositions)
            {
                if (drawingArea.Contains(position))
                {
                    int dotSize = 10;
                    int dotX = position.X - dotSize / 2;
                    int dotY = position.Y - dotSize / 2;

                    e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
                }
            }

            if (drawingArea.Contains(clickedPosition))
            {
                int dotSize = 10;
                int dotX = clickedPosition.X - dotSize / 2;
                int dotY = clickedPosition.Y - dotSize / 2;

                e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
            }


        }

        private void UpdatePositionLabel(Point position)
        {
            lbl_clickMessage.Text = $"X: {position.X}, Y: {position.Y}";
        }
    }
}

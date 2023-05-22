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

        private List<Point> clickedPositions; // Stores the clicked positions
        private Rectangle drawingArea; // Defines the drawing area
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private System.Threading.Timer timer; // Timer to wait for 3 seconds
        private string csvFilePath = "points.csv"; // Path to the CSV file

        public Form1()
        {
            InitializeComponent();
            clickedPositions = new List<Point>();
            drawingArea = new Rectangle(100, 50, 200, 200); // Define the drawing area
            this.MouseClick += Form1_MouseClick; // Wire up the event handler
            LoadPointsFromCSV(); // Load points from CSV file
        }

        private void LoadPointsFromCSV()
        {
            if (File.Exists(csvFilePath))
            {
                string[] lines = File.ReadAllLines(csvFilePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                    {
                        clickedPositions.Add(new Point(x, y));
                    }
                }

                Refresh(); // Redraw the form to display the loaded points
            }
        }
        private void SavePointsToCSV()
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                foreach (Point position in clickedPositions)
                {
                    writer.WriteLine($"{position.X},{position.Y}");
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (drawingArea.Contains(e.Location))
            {
                clickedPositions.Add(e.Location);
                Refresh(); // Redraw the form to display the dots
                UpdatePositionLabel(e.Location); // Update the position label
                timer = new System.Threading.Timer(OnTimerElapsed, null, 2000, Timeout.Infinite); // Start the timer for 3 seconds
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

            foreach (Point position in clickedPositions)
            {
                if (drawingArea.Contains(position))
                {
                    int dotSize = 10;
                    int dotX = position.X - dotSize / 2;
                    int dotY = position.Y - dotSize / 2;

                    e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
                }
            }
        }

        private void UpdatePositionLabel(Point position)
        {
            lbl_clickMessage.Text = $"X: {position.X}, Y: {position.Y}";
        }
    }
}

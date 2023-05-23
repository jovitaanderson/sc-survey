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
        private Rectangle drawingArea = new Rectangle(100, 50, 200, 200); // Defines the drawing area 
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds
        private string csvFilePath = "points.csv"; // Path to the CSV file

        private string csvAdminQuestionsFilePath = "admin_questions.csv"; // Path to the CSV file
        private string csvAdminTableFilePath = "admin_table.csv";
        private string csvAdminDownloadFilePath = "admin_download.csv";
        private string csvAdminAdvanceFilePath = "admin_advance.csv";

        private string csvFilePath = "player_answers.csv"; // Path to the CSV file
        private const string columnNames = "date,point_x,point_y,question1,question2,question3";
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
            LoadTableFromCSV();

            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(imagePath))
            {
                // Set the background image of the Windows Forms application
                this.BackgroundImage = Image.FromFile(imagePath);

                // Adjust the background image display settings
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            
        }

        private void LoadTableFromCSV()
        {
            if (File.Exists(csvAdminTableFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminTableFilePath);
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');

                    if (values.Length == 3)
                    {
                        labelTitle.Text = values[0];
                        labelXAxis.Text = values[1];
                        labelYAxis.Text = values[2];
                    }
                }
            }
        }

        private string LoadBackgroundImageFromCSV()
        {
            if (File.Exists(csvAdminAdvanceFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminAdvanceFilePath);
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
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");

                writer.WriteLine($"{currentDate},{clickedPosition.X},{clickedPosition.Y}");
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

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            // Perform the password check here
            bool isPasswordClickedProperly = CheckPasswordClickedProperly();

            if (isPasswordClickedProperly)
            {
                AdminForm newForm = new AdminForm();
                newForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect password click!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private bool CheckPasswordClickedProperly()
        {
            // Implement your password click check logic here
            // Return true if the password is clicked properly, otherwise return false
            // Example:
            string enteredPassword = textBox1.Text;
            string correctPassword = "123456";
            return enteredPassword == correctPassword;
        }
    }
}

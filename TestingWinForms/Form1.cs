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
        //private Rectangle drawingArea = new Rectangle(100, 50, 200, 200); // Defines the drawing area (x, y, height, width)
        private Rectangle drawingArea;
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds

        private string csvAdminTableFilePath = "admin_table.csv";
        private string csvAdminAdvanceFilePath = "admin_advance.csv";

        private string csvFilePath = "player_answers.csv"; // Path to the CSV file
        private const string columnNames = "date,point_x,point_y,question1,question2,question3";
        private int timerToQuestionPage = 1000;
        private int lastRowNumber;

        public Form1()
        {

            InitializeComponent();

            // Wire up the Resize event handler
            this.Resize += Form1_Resize;

            // Initial calculation of the drawing area
            CalculateDrawingArea();

            existingClickedPositions = new List<Point>();

            //Create file with column header if file does not exits
            if (!File.Exists(csvFilePath))
            {
                string csvHeader = $"{columnNames}" + Environment.NewLine;
                File.WriteAllText(csvFilePath, csvHeader, Encoding.UTF8);
            }

            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

            this.MouseClick += Form1_MouseClick; // Wire up the event handler
            LoadPointsFromCSV(); // Load points from CSV file
            LoadTableFromCSV();

            //Display Background Image
            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                // Set the background image of the Windows Forms application
                this.BackgroundImage = Image.FromFile(imagePath);

                // Adjust the background image display settings
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            // Set the Anchor property for labels and buttons
            //labelTitle.Anchor = AnchorStyles.Left;
            //labelXAxis.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            //labelYAxis.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Set the properties for labelXAxis and labelYAxis
            //labelXAxis.AutoSize = false;
            //labelXAxis.MaximumSize = new Size(200, 0); // Adjust the desired width

            //labelYAxis.AutoSize = false;
            //labelYAxis.MaximumSize = new Size(200, 0); // Adjust the desired width

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Recalculate the drawing area when the form is resized
            CalculateDrawingArea();
            Refresh();
        }

        private void CalculateDrawingArea()
        {
            int margin = 100; // Minimum margin size

            // Calculate the available width and height for the square
            int availableWidth = this.ClientSize.Width - 2 * margin;
            int availableHeight = this.ClientSize.Height - 2 * margin;

            // Determine the size of the square based on the smaller dimension
            int squareSize = Math.Min(availableWidth, availableHeight);

            // Calculate the coordinates of the square to center it within the available space
            int x = margin + (availableWidth - squareSize) / 2;
            int y = margin + (availableHeight - squareSize) / 2;

            // Update the drawing area rectangle
            drawingArea = new Rectangle(x, y, squareSize, squareSize);
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

                Form2 newForm = new Form2(lastRowNumber); // Navigate to a new page
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

            // Draw X and Y axis labels
            int xSteps = 5; // Specify the number of steps on the X axis
            int ySteps = 5; // Specify the number of steps on the Y axis
            int stepSizeX = drawingArea.Width / xSteps;
            int stepSizeY = drawingArea.Height / ySteps;

            using (Font font = new Font("Arial", 8))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    // Draw X axis labels
                    for (int i = 1; i <= xSteps; i++)
                    {
                        int labelX = drawingArea.Left + (i * stepSizeX);
                        int labelY = drawingArea.Bottom + 5;

                        string label = (i * 10).ToString(); // Adjust the label based on your requirements
                        e.Graphics.DrawString(label, font, Brushes.Black, labelX, labelY, format);
                    }

                    // Draw Y axis labels
                    for (int i = 1; i <= ySteps; i++)
                    {
                        int labelX = drawingArea.Left - 25;
                        int labelY = drawingArea.Bottom - (i * stepSizeY);

                        string label = (i * 10).ToString(); // Adjust the label based on your requirements
                        e.Graphics.DrawString(label, font, Brushes.Black, labelX, labelY, format);
                    }
                }
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

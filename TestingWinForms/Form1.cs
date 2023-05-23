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
using Microsoft.VisualBasic.FileIO;

namespace TestingWinForms
{
    public partial class Form1 : Form
    {
        private List<PointF> existingClickedPositions; // Stores the previous saved clicked positions
        private Point clickedPosition; // Stores the current clicked positions
        private Rectangle drawingArea = new Rectangle(100, 50, 200, 200); // Defines the drawing area (x, y, height, width)
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds

        private string csvAdminTableFilePath = "admin_table.csv";
        private string csvAdminAdvanceFilePath = "admin_advance.csv";

        private string csvFilePath = "player_answers.csv"; // Path to the CSV file
        private string columnNames;
        private int timerToQuestionPage = 1000;
        private int lastRowNumber;

        String loadColumnNames()
        {
            string csvFilePath = "admin_questions.csv";

            List<string> firstColumnValues = new List<string>();

            if (File.Exists(csvFilePath))
            {
                using (TextFieldParser parser = new TextFieldParser(csvFilePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Length > 0)
                        {
                            string value = fields[0]; // Read the value from the first column
                            firstColumnValues.Add(value);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("CSV file does not exist.");
                return "date,point_x,point_y,question1,question2,question3";
            }
            return "date,point_x,point_y," + string.Join(",", firstColumnValues);
        }

        public Form1()
        {

            InitializeComponent();
            existingClickedPositions = new List<PointF>();
            columnNames = loadColumnNames();

            //Create file with column header if file does not exits
            if (!File.Exists(csvFilePath))
            {
                string csvHeader = $"{columnNames}" + Environment.NewLine;
                File.WriteAllText(csvFilePath, csvHeader, Encoding.UTF8);
            }

            //FormBorderStyle = FormBorderStyle.None; // Remove the border
           // WindowState = FormWindowState.Maximized; // Maximize the window

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
                // Calculate the inverse scaling factors
                float inverseScaleX = drawingArea.Width / 10f;
                float inverseScaleY = drawingArea.Height / 10f;

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
                        float.TryParse(parts[pointXIndex], out float x) && float.TryParse(parts[pointYIndex], out float y))
                    {
                        //to fix 

                        // Scale back the coordinates to the original dimensions
                        float originalX = (x * inverseScaleX) + drawingArea.X;
                         float originalY = (y * inverseScaleY) + drawingArea.Y;

                        existingClickedPositions.Add(new PointF(originalX, originalY));
                    }
                }

                Refresh(); // Redraw the form to display the loaded points
            }
        }

        private void SavePointToCSV(PointF currPoint)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath, true))
            {
                string currentDate = DateTime.Now.ToString("dd/MM/yyyy");

                writer.WriteLine($"{currentDate},{currPoint.X},{currPoint.Y}");
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (drawingArea.Contains(e.Location))
            {
                // Calculate the scaled coordinates within the rectangle
                float scaleX = 10f / drawingArea.Width;
                float scaleY = 10f / drawingArea.Height;

                float scaledX = (e.Location.X - drawingArea.X) * scaleX;
                float scaledY = (e.Location.Y - drawingArea.Y) * scaleY;
                //int scaledXInt = (int)Math.Round(scaledX);
                //int scaledYInt = (int)Math.Round(scaledY);

                PointF point = new PointF(scaledX, scaledY); // Create a PointF instance with the float values

                existingClickedPositions.Add(point);
                clickedPosition = e.Location; 

                Refresh(); // Redraw the form to display the dots
                SavePointToCSV(point);

                //For debugging
                UpdatePositionLabel(point); // Update the position label

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
            foreach (PointF position in existingClickedPositions)
            {
                Point roundedPosition = Point.Round(position); // Convert PointF to Point
                if (drawingArea.Contains(roundedPosition))
                {
                    float dotX = position.X - dotSize / 2;
                    float dotY = position.Y - dotSize / 2;

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
        private void UpdatePositionLabel(PointF position)
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

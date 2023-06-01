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
using System.Runtime.Serialization.Formatters.Binary;

namespace TestingWinForms
{
    public partial class TableForm : Form
    {
        private List<PointF> existingClickedPositions; // Stores the previous saved clicked positions
        private Point clickedPosition; // Stores the current clicked positions
        //private Rectangle drawingArea = new Rectangle(100, 50, 200, 200); // Defines the drawing area (x, y, height, width)
        private Rectangle drawingArea;
        private int drawingAreaBorderWidth = 2; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds

        private string columnNames;
        private int timerToQuestionPage = 1000;
        private int lastRowNumber;
        private bool hasClicked = false;

        private Color existingColour;
        private Color selectedColour;


        private void TableForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                AdminForm newForm = new AdminForm();
                newForm.Show();
                this.Hide();
            }

        }

        private void initializeDirectory() {
            // Get the current root path of the application
            string rootPath = Directory.GetCurrentDirectory();

            // Specify the directory within the root path to save the image
            string adminPath = Path.Combine(rootPath, "admin");
            string downloadPath = Path.Combine(rootPath, "downloads");
            string developerPath = Path.Combine(rootPath, "ForDevelopersOnly");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(adminPath))
            {
                Directory.CreateDirectory(adminPath);
            }
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
            if (!Directory.Exists(developerPath))
            {
                Directory.CreateDirectory(developerPath);
            }
        }

        String loadColumnNames()
        {

            List<string> firstColumnValues = new List<string>();

            if (File.Exists(GlobalVariables.csvAdminQuestionsFilePath))
            {
                using (TextFieldParser parser = new TextFieldParser(GlobalVariables.csvAdminQuestionsFilePath))
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

        public TableForm()
        {
            
            existingClickedPositions = new List<PointF>();

            // Wire up the Resize event handler
            this.Resize += Form1_Resize;

            // Initial calculation of the drawing area
            //CalculateDrawingArea();

            InitializeComponent();
            initializeDirectory();
            DoubleBuffered = true;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(TableForm_KeyDown);

            existingClickedPositions = new List<PointF>();
            columnNames = loadColumnNames();

            //Create file with column header if file does not exits
            if (!File.Exists(GlobalVariables.csvRawDataFilePath))
            {
                string csvHeader = $"{columnNames}" + Environment.NewLine;
                File.WriteAllText(GlobalVariables.csvRawDataFilePath, csvHeader, Encoding.UTF8);
            }

            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

            this.MouseClick += Form1_MouseClick; // Wire up the event handler
            //LoadPointsFromCSV(); // Load points from CSV file
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
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Recalculate the drawing area when the form is resized
            CalculateDrawingArea();
            LoadPointsFromCSV();
            Refresh();
        }

        private void CalculateDrawingArea()
        {
            int margin = 100; // Minimum margin size

            // Calculate the available width and height for the square
            int availableWidth = this.Size.Width - 2 * margin;
            int availableHeight = this.Size.Height - 2 * margin;

            // Determine the size of the square based on the smaller dimension
            int squareSize = Math.Min(availableWidth, availableHeight);

            // Calculate the coordinates of the square to center it within the available space
            int x = margin + (availableWidth - squareSize) / 2;
            int y = margin + (availableHeight - squareSize) / 2;

            // Update the drawing area rectangle
            drawingArea = new Rectangle(x, y, squareSize, squareSize);
        }

        private Font FontFromBinaryString(string fontData)
        {
            byte[] binaryData = Convert.FromBase64String(fontData);

            using (MemoryStream stream = new MemoryStream(binaryData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Font)formatter.Deserialize(stream);
            }
        }

        private void LoadTableFromCSV()
        {
            if (File.Exists(GlobalVariables.csvAdminTableFilePath))
            {

                string[] lines;

                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminTableFilePath);
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

                    if (values.Length == 7)
                    {
                        labelTitle.Text = values[0];
                        labelXAxis.Text = values[1];
                        labelYAxis.Text = values[2];

                        labelYAxis.AutoSize = true;
                        labelYAxis.MaximumSize = new Size(300, 0);
                        labelYAxis.MinimumSize = new Size(300, 0);
                        labelYAxis.TextAlign = ContentAlignment.TopCenter;

                        int maxWidth = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.75);

                        labelXAxis.AutoSize = true;
                        labelXAxis.MaximumSize = new Size(maxWidth, 0);
                        labelXAxis.MinimumSize = new Size(maxWidth, 0);
                        labelXAxis.TextAlign = ContentAlignment.TopCenter;

                        labelTitle.AutoSize = true;
                        labelTitle.MaximumSize = new Size(maxWidth, 0);
                        labelTitle.MinimumSize = new Size(maxWidth, 0);
                        labelTitle.TextAlign = ContentAlignment.TopCenter;

                        labelTitle.Left = (this.ClientSize.Width - labelTitle.Width) / 2;
                        labelXAxis.Left = (this.ClientSize.Width - labelXAxis.Width) / 2;

                        

                        existingColour = ColorTranslator.FromHtml(values[3]);
                        selectedColour = ColorTranslator.FromHtml(values[4]);

                        string fontTitle = values[5]; 
                        Font loadedFontTitle = FontFromBinaryString(fontTitle);
                        labelTitle.Font = loadedFontTitle;
                        
                        string fontXYaxis = values[6]; 
                        Font loadedFontXYaxis = FontFromBinaryString(fontXYaxis);
                        labelXAxis.Font = loadedFontXYaxis;
                        labelYAxis.Font = loadedFontXYaxis;

                    }
                }
            } 
        }

        private string LoadBackgroundImageFromCSV()
        {
            if (File.Exists(GlobalVariables.csvAdminAdvanceFilePath))
            {
                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminAdvanceFilePath);
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

        private void LoadPointsFromCSV()
        {

            // Clear the existing points
            existingClickedPositions.Clear();

            // Load all existing points to screen
            if (File.Exists(GlobalVariables.csvRawDataFilePath))
            {
                // Calculate the inverse scaling factors
                float inverseScaleX = drawingArea.Width / 10f;
                float inverseScaleY = drawingArea.Height / 10f;

                string[] lines;

                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvRawDataFilePath);
                        break;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

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
            while (true)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(GlobalVariables.csvRawDataFilePath, true))
                    {
                        string currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                        writer.WriteLine($"'{currentDate},{currPoint.X},{currPoint.Y}");
                    }
                    break;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // check if all admin csv exixts, if dosent prompt message box
            if (!File.Exists(GlobalVariables.csvAdminAdvanceFilePath) || !File.Exists(GlobalVariables.csvAdminQuestionsFilePath))
            {
                MessageBox.Show("Please set details in admin page and save!");
            } else if (string.IsNullOrWhiteSpace(File.ReadAllText(GlobalVariables.csvAdminQuestionsFilePath)))
            {
                
                MessageBox.Show("Please set questions in admin page and save!");
            }
            else
            {
                if (!hasClicked && drawingArea.Contains(e.Location))
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
                    hasClicked = true;

                    timer = new System.Threading.Timer(OnTimerElapsed, null, timerToQuestionPage, Timeout.Infinite); // Start the timer for x seconds
                }
            }
            
        }

        private void OnTimerElapsed(object state) 
        {
            hasClicked = false;
            // Invoke the navigation to a new page on the UI thread
            Invoke(new Action(() =>
            {
                timer.Dispose(); // Dispose the timer
                timer = null; // Set the timer reference to null

                QuestionForm newForm = new QuestionForm(lastRowNumber); // Navigate to a new page
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

                    // Update the FillEllipse brush with existingColour
                    if (selectedColour != null)
                    {
                        using (Brush brush = new SolidBrush(existingColour))
                        {
                            e.Graphics.FillEllipse(brush, dotX, dotY, dotSize, dotSize);
                        }
                    }
                    else
                    {
                        e.Graphics.FillEllipse(Brushes.Blue, dotX, dotY, dotSize, dotSize);
                    }
                }
            }

            //Paint current dot
            if (drawingArea.Contains(clickedPosition))
            {
                int dotX = clickedPosition.X - dotSize / 2;
                int dotY = clickedPosition.Y - dotSize / 2;

                // Update the FillEllipse brush with existingColour
                if (selectedColour != null)
                {
                    using (Brush brush = new SolidBrush(selectedColour))
                    {
                        e.Graphics.FillEllipse(brush, dotX, dotY, dotSize, dotSize);
                    }
                } else
                {
                    e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
                }
                
            }

            // Draw X and Y axis labels
            int xSteps = 21; // Specify the number of steps on the X axis (from -10 to 10)
            int ySteps = 21; // Specify the number of steps on the Y axis (from -10 to 10)
            int stepSizeX = drawingArea.Width / xSteps;
            int stepSizeY = drawingArea.Height / ySteps;

            using (Font font = new Font("Arial", 10))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    // Draw X axis labels
                    for (int i = -10; i <= 10; i++)
                    {
                        int labelX = drawingArea.Left + ((i + 10) * stepSizeX);
                        int labelY = drawingArea.Bottom + 5;

                        string label = i.ToString(); // Adjust the label based on your requirements
                        e.Graphics.DrawString(label, font, Brushes.Black, labelX, labelY, format);
                    }

                    // Draw Y axis labels
                    for (int i = -10; i <= 10; i++)
                    {
                        int labelX = drawingArea.Left - 25;
                        int labelY = drawingArea.Bottom - ((i + 10) * stepSizeY);

                        string label = i.ToString(); // Adjust the label based on your requirements
                        e.Graphics.DrawString(label, font, Brushes.Black, labelX, labelY, format);
                    }
                }
            }

        }

    }
}

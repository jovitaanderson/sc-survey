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
        private Rectangle verticalLine;
        private Rectangle horizontalLine;
        private int lineWidth = 2; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds

        private string columnNames;
        private int timerToQuestionPage = 1000;
        private int timerToShowAllPoints = 1000;

        private int lastRowNumber;
        private bool hasClicked = false;

        private Color existingColour;
        private Color selectedColour;

        private string tableBackgroundPath = "";
        private string InnerGraphBackgroundPath = "";


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

            nextButton.Visible = false;

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

            //this.MouseClick += Form1_MouseClick; // Wire up the event handler
            //LoadPointsFromCSV(); // Load points from CSV file

            //Display Background Image
            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(tableBackgroundPath) && File.Exists(tableBackgroundPath))
            {
                // Set the background image of the Windows Forms application
                this.BackgroundImage = Image.FromFile(tableBackgroundPath);

                // Adjust the background image display settings
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            if (!string.IsNullOrEmpty(InnerGraphBackgroundPath) && File.Exists(InnerGraphBackgroundPath))
            {
                // Set the background image of the Windows Forms application
                Image image = Image.FromFile(InnerGraphBackgroundPath);
                graphPanel.BackgroundImage = image; // Set the smaller image as the background image
                graphPanel.BackgroundImageLayout = ImageLayout.Stretch;

                int percentageWidth =  70; // Width percentage (50%)
                int percentageHeight = 80; // Height percentage (50%)

                // Get the screen dimensions
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;

                // Calculate the desired dimensions based on the percentages
                int desiredWidth = (int)(screenWidth * (percentageWidth / 100.0));
                int desiredHeight = (int)(screenHeight * (percentageHeight / 100.0));

                // Set the size of the graphPanel
                graphPanel.Size = new Size(desiredWidth, desiredHeight);
                //graphPanel.Size = new Size(500, 500); // Set the size of the smaller panel

                // Calculate the coordinates to center the smaller Panel on a maximized window
                int x = (screenWidth - graphPanel.Width) / 2;
                int y = (screenHeight - graphPanel.Height) / 2;

                // Set the location of the smaller Panel
                graphPanel.Location = new Point(x, y+ 40);

                //this.Controls.Add(smallerPanel); // Add it to the form or a container control
                CalculateDrawingArea();
            }

            LoadTableFromCSV();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Recalculate the drawing area when the form is resized
            //CalculateDrawingArea();
            LoadPointsFromCSV();
            //Refresh();
        }

        private void CalculateDrawingArea()
        {
            int margin = 50; // Minimum margin size

            // Calculate the available width and height for the square
            int availableWidth = graphPanel.Width - 2 * margin;
            int availableHeight = graphPanel.Height - 2 * margin;

            // Determine the size of the square based on the smaller dimension
            int squareSize = Math.Min(availableWidth, availableHeight);

            // Calculate the coordinates of the square to center it within the available space
            int x = margin + (availableWidth - squareSize) / 2;
            int y = margin + (availableHeight - squareSize) / 2;

            // Update the drawing area rectangle
            drawingArea = new Rectangle(x, y, squareSize, squareSize);

            verticalLine = new Rectangle(x, y, squareSize/2, squareSize);
            horizontalLine = new Rectangle(x, y, squareSize, squareSize / 2);
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

                    if (values.Length == 12)
                    {
                        labelTitle.Text = values[0];
                        labelXAxis.Text = values[1];
                        labelXAxis2.Text = values[2];
                        labelYAxis.Text = values[3];
                        labelYAxis2.Text = values[4];

                        int maxWidthTitle = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.25);
                        int maxHeightTitle = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.20);
                        int maxWidthAxis = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.40);
                        int maxHeightXAxis = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.11);
                        int maxHeightYAxis = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.35);
                        
                        // Title 
                        labelTitle.AutoSize = true;
                        labelTitle.MaximumSize = new Size(maxWidthTitle, maxHeightTitle);
                        labelTitle.MinimumSize = new Size(0, 0);
                        int mid = Screen.PrimaryScreen.Bounds.Width / 2 - labelTitle.Width/2;
                        int mid_h = Screen.PrimaryScreen.Bounds.Height / 2;  
                        labelTitle.Location = new Point(mid, 20);
                        labelTitle.TextAlign = ContentAlignment.MiddleLeft;

                        int margin = 20;
                        int maxWidthYAxis = (graphPanel.Width - drawingArea.Width) / 2 - margin*2;
                        int maxHeightYAxiss = graphPanel.Height - ((graphPanel.Height - drawingArea.Height) + margin * 2);
                        // Left y axis label
                        labelYAxis.AutoSize = true;
                        labelYAxis.MaximumSize = new Size(maxWidthYAxis, maxHeightYAxiss);
                        labelYAxis.MinimumSize = new Size(0, 0);
                        int height = labelYAxis.Height;
                        labelYAxis.TextAlign = ContentAlignment.MiddleCenter;
                        //labelYAxis.Left = (this.ClientSize.Width - drawingArea.Width - labelYAxis.Width ) / 2;
                        //labelYAxis.Location = new Point(graphPanel.Location.X, graphPanel.Location.Y);
                        int left_padding = (Screen.PrimaryScreen.Bounds.Width) / 2;
                        //labelYAxis.Location = new Point((graphPanel.Width - labelYAxis.Width) / 2, (graphPanel.Height - labelYAxis.Height) / 2);
                        int left_position = (graphPanel.Width/2 - drawingArea.Width/ 2) - labelYAxis.Width - margin;
                        int middle_position = (graphPanel.Height / 2) - (labelYAxis.Height/2);
                        labelYAxis.Location = new Point(left_position, middle_position);
                        //labelYAxis.Top = (this.ClientSize.Height - labelYAxis.Height) / 2;
                        //int padding = (graphPanel.Height - labelYAxis.Height) / 2;
                        //labelYAxis.Padding = new Padding(left_padding+100, padding, 0, padding);

                        // Right y axis label
                        int yAxisRight_left = (graphPanel.Width / 2) + drawingArea.Width / 2 + margin;
                        labelYAxis2.AutoSize = true;
                        labelYAxis2.MaximumSize = new Size(maxWidthYAxis, maxHeightYAxiss);
                        labelYAxis2.MinimumSize = new Size(0, 0);
                        labelYAxis2.TextAlign = ContentAlignment.MiddleCenter;
                        int middle2_position = (graphPanel.Height / 2) - (labelYAxis2.Height / 2);

                        labelYAxis2.Location = new Point(yAxisRight_left, middle2_position);
                        //labelYAxis2.Left = (this.ClientSize.Width/2) + (drawingArea.Width/2);
                        //labelYAxis2.Top = (this.ClientSize.Height - labelYAxis2.Height) / 2;


                        // Top x axis label
                        labelXAxis2.AutoSize = true;
                        labelXAxis2.MaximumSize = new Size(maxWidthAxis, maxHeightXAxis);
                        labelXAxis2.MinimumSize = new Size(0, 0);
                        //labelXAxis2.TextAlign = ContentAlignment.TopCenter;
                        int center = (graphPanel.Width / 2) - (labelXAxis2.Width /2);
                        int xAxisTop_top = (graphPanel.Height - drawingArea.Height)/2 - labelXAxis2.Height/2 - margin;
                        labelXAxis2.Location = new Point(center, xAxisTop_top);
                        //labelXAxis2.Left = (this.ClientSize.Width - labelXAxis2.Width) / 2;


                        // Bottom x axis label
                        labelXAxis.AutoSize = true;
                        labelXAxis.MaximumSize = new Size(maxWidthAxis, maxHeightXAxis);
                        labelXAxis.MinimumSize = new Size(0, 0);
                        labelXAxis.TextAlign = ContentAlignment.TopCenter;
                        int xAxisBot_top = graphPanel.Height - ((graphPanel.Height - drawingArea.Height) / 2 - labelXAxis.Height / 2 - margin);
                        labelXAxis.Location = new Point(center, xAxisBot_top);
                        //labelXAxis.Left = (this.ClientSize.Width - labelXAxis.Width) / 2;


                        existingColour = ColorTranslator.FromHtml(values[5]);
                        selectedColour = ColorTranslator.FromHtml(values[6]);

                        loadContentToComponent(values, 7, labelTitle);
                        loadContentToComponent(values, 8, labelXAxis2);
                        loadContentToComponent(values, 9, labelXAxis);
                        loadContentToComponent(values, 10, labelYAxis);
                        loadContentToComponent(values, 11, labelYAxis2);

                    }
                }
            } 
        }

        void loadContentToComponent(string[] values, int ValueIndex, Label label)
        {
            string[] textProperties = values[ValueIndex].Split(';'); // Assuming text font and alignment data is at index 5
            string fontTitle = textProperties[0]; // First ; is text font

            Font loadedFontTitle = FontFromBinaryString(fontTitle);
            label.Font = loadedFontTitle;
            label.Height = (int)Math.Ceiling(FontFromBinaryString(fontTitle).GetHeight()) + Padding.Vertical; ;

            if (textProperties.Length > 2)
            {
                String textAlign = textProperties[1]; // Second ; is text align property
                String textWrap = textProperties[2]; // Third ; is text wrap property
                if (Enum.TryParse(textAlign, out ContentAlignment alignment))
                {
                    label.TextAlign = alignment;
                }
                else
                {
                    label.TextAlign = ContentAlignment.TopLeft; //default ContentAlignment
                }
                label.AutoSize = textWrap.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            label.Height = (int)Math.Ceiling(loadedFontTitle.GetHeight()) + Padding.Vertical;
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
                    if (values.Length >= 5)
                    {
                        if (values[3] != null)
                        {
                            string imagePath = Path.Combine(values[3]);
                            tableBackgroundPath = imagePath;
                            if (values[4] != null)
                            {
                                InnerGraphBackgroundPath = Path.Combine(values[4]);
                            }

                            return imagePath;

                        }
                        else
                        {
                            return null;
                        }
                    }
                    else {
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
                float inverseScaleX = drawingArea.Width / 20f; // Range: -10 to 10
                float inverseScaleY = -drawingArea.Height / 20f; // Range: -10 to 10


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
                        float originalX = (x * inverseScaleX) + drawingArea.X + (drawingArea.Width / 2f);
                        float originalY = (y * inverseScaleY) + drawingArea.Y + (drawingArea.Height / 2f);
                        //float originalY = (10f - y) * inverseScaleY + drawingArea.Y + (drawingArea.Height / 2f);
                        existingClickedPositions.Add(new PointF(originalX, originalY));
                    }
                }

                //Refresh(); // Redraw the form to display the loaded points
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
            LoadTimerFromCSV();
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
                    float scaleX = 20f / drawingArea.Width; // Range: -10 to 10
                    float scaleY = 20f / drawingArea.Height; // Range: -10 to 10


                    float scaledX = ((e.Location.X - drawingArea.X) * scaleX) - 10f; // Range: -10 to 10
                    float scaledY = 10f - ((e.Location.Y - drawingArea.Y) * scaleY); // Range 10 to -10

                    //int scaledXInt = (int)Math.Round(scaledX);
                    //int scaledYInt = (int)Math.Round(scaledY);

                    PointF point = new PointF(scaledX, scaledY); // Create a PointF instance with the float values

                    existingClickedPositions.Add(point);
                    clickedPosition = e.Location;

                    Refresh(); // Redraw the form to display the dots
                    SavePointToCSV(point);
                    hasClicked = true;
                    Refresh();
                    nextButton.Visible = true;
                    timer = new System.Threading.Timer(OnTimerElapsed, null, timerToQuestionPage, Timeout.Infinite); // Start the timer for x seconds
                }
            }
            
        }

        private void LoadTimerFromCSV()
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

                    
                    if (values[0] != null && int.TryParse(values[0], out int interval))
                    {
                        timerToQuestionPage = interval * 1000;
                    }
                    else
                    {
                        timerToQuestionPage = 10 * 1000; // Default timer interval is 10s
                    }
                }
                else
                {
                    timerToQuestionPage = 10 * 1000; // Default timer interval is 10s
                }
            }
            else
            {
                timerToQuestionPage = 10 * 1000; // Default timer interval is 10s
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

                TableForm newForm = new TableForm(); // Navigate to a new page
                newForm.Show();
                this.Hide();
            }));
        }

        private void graphPanel_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, drawingArea, Color.Black, 2, ButtonBorderStyle.Solid, Color.Black, 2, ButtonBorderStyle.Solid, Color.Black, 2, ButtonBorderStyle.Solid, Color.Black, 2, ButtonBorderStyle.Solid);
            ControlPaint.DrawBorder(e.Graphics, verticalLine, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Black, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid);
            ControlPaint.DrawBorder(e.Graphics, horizontalLine, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Black, lineWidth, ButtonBorderStyle.Solid);
            
            if (hasClicked == true)
            {
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
                }
                else
                {
                    e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
                }
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        { 
            /*
            base.OnPaint(e);

            ControlPaint.DrawBorder(e.Graphics, drawingArea, Color.Black, 2, ButtonBorderStyle.Solid, Color.Black, 2, ButtonBorderStyle.Solid, Color.Black, 2, ButtonBorderStyle.Solid, Color.Black, 2, ButtonBorderStyle.Solid);

            ControlPaint.DrawBorder(e.Graphics, verticalLine, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Black, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid);
            ControlPaint.DrawBorder(e.Graphics, horizontalLine, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Transparent, lineWidth, ButtonBorderStyle.Solid, Color.Black, lineWidth, ButtonBorderStyle.Solid);


            if (hasClicked == true)
            {
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
            */
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            hasClicked = false;
            timer.Dispose();
            timer = null;
            QuestionForm newForm = new QuestionForm(lastRowNumber); // Navigate to a new page
            newForm.Show();
            nextButton.Visible = false;
            this.Hide();
        }
    }
}

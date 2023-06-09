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
        //private int lineWidth = 5; // Specify the width of the border
        private int dotSize = 10;
        private System.Threading.Timer timer; // Timer to wait for 3 seconds

        private string columnNames;
        private int timerToQuestionPage = 1000;
        private int backgroundRadius = 90;
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
                if(timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }

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

            LoadTimerAndRadiusFromCSV();

            //Display Background Image
            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(tableBackgroundPath) && File.Exists(tableBackgroundPath))
            {
                // Set the background image of the Windows Forms application
                this.BackgroundImage = Image.FromFile(tableBackgroundPath);

                // Adjust the background image display settings
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            int percentageWidth = 70; // Width percentage (50%)
            int percentageHeight = 80; // Height percentage (50%)

            // Get the screen dimensions
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calculate the desired dimensions based on the percentages
            int desiredWidth = (int)(screenWidth * (percentageWidth / 100.0));
            int desiredHeight = (int)(screenHeight * (percentageHeight / 100.0));

            // Set the size of the graphPanel
            graphPanel.Size = new Size(desiredWidth, desiredHeight);

            // Calculate the coordinates to center the smaller Panel on a maximized window
            int x = (screenWidth - graphPanel.Width) / 2;
            int y = (screenHeight - graphPanel.Height) / 2;
            
            // Set the location of the smaller Panel
            graphPanel.Location = new Point(x, y + 40);
            graphPanel.BorderRadius = backgroundRadius;

            CalculateDrawingArea();

            if (!string.IsNullOrEmpty(InnerGraphBackgroundPath) && File.Exists(InnerGraphBackgroundPath))
            {
                // Set the background image of the Windows Forms application
                Image image = Image.FromFile(InnerGraphBackgroundPath);
                graphPanel.BackgroundImage = image; // Set the smaller image as the background image
                graphPanel.BackgroundImageLayout = ImageLayout.Stretch;

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
            int margin = 80; // Minimum margin size

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
                        labelTitle.Text = values[0].Replace("\0", ",");
                        labelXAxis2.Text = values[1].Replace("\0", ",");
                        labelXAxis.Text = values[2].Replace("\0", ",");
                        labelYAxis.Text = values[3].Replace("\0", ",");
                        labelYAxis2.Text = values[4].Replace("\0", ",");

                        // Load saved font and text properties
                        loadContentToComponent(values, 7, labelTitle); //auto size for title effect as  height and width is set
                        loadContentToComponent(values, 8, labelXAxis2);
                        loadContentToComponent(values, 9, labelXAxis);
                        loadContentToComponent(values, 10, labelYAxis);
                        loadContentToComponent(values, 11, labelYAxis2);


                        int margin = 20;

                        int maxWidthTitle = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width - (margin * 2));
                        int maxHeightTitle = Convert.ToInt32( graphPanel.Location.Y - (margin * 2));

                        // Title 
                        labelTitle.MinimumSize = new Size(maxWidthTitle, maxHeightTitle);
                        labelTitle.MaximumSize = new Size(maxWidthTitle, maxHeightTitle);
                        labelTitle.Location = new Point(margin, margin);

                        // Y axis height and width constraints 
                        int maxWidthYAxis = drawingArea.Left - margin * 2;
                        int maxHeightYAxis = graphPanel.Height - (drawingArea.Top * 2 + margin * 2);

                        // Left y axis label
                        labelYAxis.MinimumSize = new Size(0, 0);
                        labelYAxis.MaximumSize = new Size(maxWidthYAxis, maxHeightYAxis);
                        int yAxisLeft_left = drawingArea.Left - labelYAxis.Width - margin;
                        int yAxisLeft_top = 0;
                        if (labelYAxis.Height == maxHeightYAxis)
                        {
                            yAxisLeft_top = (graphPanel.Height / 2) - (labelYAxis.Height / 2) + margin;
                        }
                        else {
                            yAxisLeft_top = (graphPanel.Height / 2) - (labelYAxis.Height / 2) ;
                        }
                        
                        labelYAxis.Location = new Point(yAxisLeft_left, yAxisLeft_top);

                        // Right y axis label
                        labelYAxis2.MinimumSize = new Size(0, 0);
                        labelYAxis2.MaximumSize = new Size(maxWidthYAxis, maxHeightYAxis);
                        int yAxisRight_left = drawingArea.Right + margin;
                        int yAxisRight_top = 0;
                        if (labelYAxis.Height == maxHeightYAxis)
                        {
                            yAxisRight_top = (graphPanel.Height / 2) - (labelYAxis2.Height / 2) + margin;
                        }
                        else
                        {
                            yAxisRight_top = (graphPanel.Height / 2) - (labelYAxis2.Height / 2);
                        }
                        labelYAxis2.Location = new Point(yAxisRight_left, yAxisRight_top);


                        // X axis height and width constraints 
                        int maxWidthXAxis = graphPanel.Width - margin * 2;
                        int maxHeightXAxis = drawingArea.Top - margin * 2;

                        // Top x axis label
                        labelXAxis2.MinimumSize = new Size(0, 0);
                        labelXAxis2.MaximumSize = new Size(maxWidthXAxis, maxHeightXAxis);
                        int xAxisTop_left = (graphPanel.Width / 2) - (labelXAxis2.Width /2);
                        int xAxisTop_top = drawingArea.Top - labelXAxis2.Height - margin;
                        labelXAxis2.Location = new Point(xAxisTop_left, xAxisTop_top);

                        // Bottom x axis label
                        labelXAxis.MinimumSize = new Size(0, 0);
                        labelXAxis.MaximumSize = new Size(maxWidthXAxis, maxHeightXAxis);

                        int xAxisBot_left = (graphPanel.Width / 2) - (labelXAxis.Width / 2);
                        int xAxisBot_top = drawingArea.Bottom + labelXAxis.Height - margin;
                        labelXAxis.Location = new Point(xAxisBot_left, xAxisBot_top);

                        existingColour = ColorTranslator.FromHtml(values[5]);
                        selectedColour = ColorTranslator.FromHtml(values[6]);

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

            if (textProperties.Length > 3)
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

                if (!string.IsNullOrEmpty(textProperties[3]))
                {
                    if (int.TryParse(textProperties[3].Replace("\0", ","), out int foreColorArgb))
                    {
                        Color foreColor = Color.FromArgb(foreColorArgb);
                        label.ForeColor = foreColor;
                    }
                    else
                    {
                        label.ForeColor = Color.Black; // Default forecolor if parsing fails
                    }
                }
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
                        if (values.Length >= 9)
                            loadContentToComponentForNextButton(values[8], nextButton);

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

        //helper method to load Next button font styles to page
        void loadContentToComponentForNextButton(string value, Button button)
        {
            string[] textProperties = value.Split(';'); // Assuming text font and alignment data is at index 5
            string backgroundColor = textProperties[0]; // First ; is text font
            string foreColor = textProperties[1];

            // Retrieve the forecolor value and set it to the label's ForeColor property
            if (!string.IsNullOrEmpty(backgroundColor))
            {
                if (int.TryParse(backgroundColor.Replace("\0", ","), out int foreColorArgb))
                {
                    Color nextBackground = Color.FromArgb(foreColorArgb);
                    button.BackColor = nextBackground;
                }
                else
                {
                    button.BackColor = Color.Transparent; // Default forecolor if parsing fails
                }
            }
            if (!string.IsNullOrEmpty(foreColor))
            {
                if (int.TryParse(foreColor.Replace("\0", ","), out int foreColorArgb))
                {
                    Color nextForeground = Color.FromArgb(foreColorArgb);
                    button.ForeColor = nextForeground;
                }
                else
                {
                    button.ForeColor = Color.Black; // Default forecolor if parsing fails
                }
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

        private void graphPanel_MouseClick(object sender, MouseEventArgs e)
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
                    float scaleX = 20f / drawingArea.Width; // Range: -10 to 10
                    float scaleY = 20f / drawingArea.Height; // Range: -10 to 10


                    float scaledX = ((e.Location.X - drawingArea.X) * scaleX) - 10f; // Range: -10 to 10
                    float scaledY = 10f - ((e.Location.Y - drawingArea.Y) * scaleY); // Range 10 to -10


                    PointF point = new PointF(scaledX, scaledY); // Create a PointF instance with the float values

                    existingClickedPositions.Add(point);
                    clickedPosition = e.Location;

                    //Refresh(); // Redraw the form to display the dots
                    graphPanel.Invalidate();
                    SavePointToCSV(point);
                    hasClicked = true;
                    //Refresh();
                    graphPanel.Invalidate();
                    nextButton.Visible = true;
                    timer = new System.Threading.Timer(OnTimerElapsed, null, timerToQuestionPage, Timeout.Infinite); // Start the timer for x seconds
                }
            }
            
        }

        private void LoadTimerAndRadiusFromCSV()
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
                    if (values.Length >= 6 && int.TryParse(values[5], out int radius)) {
                        backgroundRadius = radius;
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
                newForm.WindowState = FormWindowState.Maximized;
                newForm.FormBorderStyle = FormBorderStyle.None;
                newForm.Show();
                this.Hide();
            }));
        }

        private void graphPanel_Paint(object sender, PaintEventArgs e)
        {

            // Get the graphics object
            Graphics g = e.Graphics;

            // Define the start and end points of the arrow
            int startX = drawingArea.Location.X;
            int startY = drawingArea.Bottom;
            int endX = startX + drawingArea.Width;
            int endY = drawingArea.Location.Y;
            int x = drawingArea.Location.X + drawingArea.Width / 2;
            int y = drawingArea.Location.Y + drawingArea.Height/2;

            // Draw a horizontal line
            float lineWidth = 5.0f; // Desired line width

            using (Pen pen = new Pen(Color.Black, lineWidth))
            {
                g.DrawLine(pen, startX, y, endX, y);
                g.DrawLine(pen, x, startY, x, endY);

            }

            endX = endX + (int)Math.Ceiling(lineWidth);
            endY = endY - (int)Math.Ceiling(lineWidth );



            // Draw the arrowhead
            int arrowSize = 12; // Adjust the size of the arrowhead /width
            int arrowOffset = arrowSize*2; // Adjust the offset of the arrowhead /height
            int arrowX = endX - arrowSize - arrowOffset;
            int arrowY = endY + arrowSize + arrowOffset;

            Point[] arrowPoints = new Point[]
            {
                new Point(arrowX, y - arrowSize),
                new Point(arrowX, y + arrowSize),
                new Point(endX, y)
            };

            Point[] arrowPointsTop = new Point[]
            {
                new Point(x - arrowSize, arrowY),
                new Point(x + arrowSize, arrowY),
                new Point(x, endY)
            };

            g.FillPolygon(Brushes.Black, arrowPoints);
            g.FillPolygon(Brushes.Black, arrowPointsTop);

            if (hasClicked == true)
            {
                // Paint existing dots
                foreach (PointF position in existingClickedPositions)
                {
                    Point roundedPosition = Point.Round(position); // Convert PointF to Point

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
                }
                else
                {
                    e.Graphics.FillEllipse(Brushes.Red, dotX, dotY, dotSize, dotSize);
                }
            }

        }

        /*
        protected override void OnPaint(PaintEventArgs e)
        { 
            
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
            
        }*/

        private void nextButton_Click(object sender, EventArgs e)
        {
            hasClicked = false;
            timer.Dispose();
            timer = null;

            // Set the opacity of the current form to 0
           // this.Opacity = 1;

            // Create a new instance of the form
            QuestionForm newForm = new QuestionForm(lastRowNumber);
            newForm.WindowState = FormWindowState.Maximized;
            newForm.FormBorderStyle = FormBorderStyle.None;

            /*newForm.Opacity = 0;

            // Subscribe to the Load event of the new form
            newForm.Load += (loadSender, loadEventArgs) =>
            {
                // When the new form has finished loading, gradually increase its opacity
                System.Windows.Forms.Timer opacityTimer = new System.Windows.Forms.Timer();
                opacityTimer.Interval = 30; // Adjust the interval as needed
                opacityTimer.Tick += (timerSender, timerEventArgs) =>
                {
                    if (newForm.Opacity < 1)
                        newForm.Opacity += 0.1; // Adjust the increment as needed
                    else
                        opacityTimer.Stop(); // Stop the timer when opacity reaches 1
                };
                opacityTimer.Start();
            };*/

            // Show the new form
            newForm.Show();

            // Hide the current form
            this.Hide();
        }
    }
}

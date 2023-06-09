using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingWinForms
{
    public class RoundedPanel:Panel
    {
        //Fields
        private int borderRadius = 30;
        private int borderThickness = 5;
        private Color borderColor = Color.Red;

        //Constructor
        public RoundedPanel() { 
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.Size = new Size(350, 200);
        }

        //Properties
        public int BorderRadius 
        {
            get => borderRadius;
            set { borderRadius = value; this.Invalidate(); } 
        }

        public int BorderThickness 
        { 
            get => borderThickness; 
            set { borderThickness = value; this.Invalidate(); }
        }

        public Color BorderColor 
        { 
            get => borderColor;
            set { borderColor = value; this.Invalidate(); }
        }

        //Methods
        private GraphicsPath GetRoundedPath(RectangleF rectangle, float radius) { 
        
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Height - radius, radius, radius, 0, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Height - radius, radius, radius, 90, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Y, radius, radius, 180, 90);
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Y, radius, radius, 270, 90);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        //Overriden methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //BorderRadius
            RectangleF rectangleF = new RectangleF(0,0,this.Width,this.Height);
            if (borderRadius>2) {
                using (GraphicsPath graphicsPath = GetRoundedPath(rectangleF, borderRadius))
                using (Pen pen = new Pen(this.Parent.BackColor, 2))
                {
                    this.Region = new Region(graphicsPath);
                    e.Graphics.DrawPath(pen, graphicsPath);
                    using (Pen borderPen = new Pen(borderColor, borderThickness)) // Specify the border color and thickness here
                    {
                        e.Graphics.DrawPath(borderPen, graphicsPath);
                    }
                }
            }
            else {
                this.Region = new Region(rectangleF);
            }

        }
        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // BorderRadius
            RectangleF rectangleF = new RectangleF(0, 0, this.Width, this.Height);
            if (borderRadius > 2)
            {
                using (GraphicsPath graphicsPath = GetRoundedPath(rectangleF, borderRadius))
                using (Pen borderPen = new Pen(Color.Red, 5)) // Specify the border color and thickness here
                {
                    this.Region = new Region(graphicsPath);
                    

                    // Draw the inner region with the background color
                    e.Graphics.FillPath(new SolidBrush(this.BackColor), graphicsPath);
                    e.Graphics.DrawPath(borderPen, graphicsPath);
                }
            }
            else
            {
                this.Region = new Region(rectangleF);
                //e.Graphics.FillRectangle(new SolidBrush(this.BackColor), ClientRectangle);
            }
        }*/


    }
}

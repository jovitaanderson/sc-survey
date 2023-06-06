
namespace TestingWinForms
{
    partial class ThankYouScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelEndMessage = new System.Windows.Forms.Label();
            this.btnMain = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelEndMessage
            // 
            this.labelEndMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelEndMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelEndMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEndMessage.Location = new System.Drawing.Point(278, 170);
            this.labelEndMessage.Name = "labelEndMessage";
            this.labelEndMessage.Size = new System.Drawing.Size(300, 55);
            this.labelEndMessage.TabIndex = 0;
            this.labelEndMessage.Text = "label1";
            this.labelEndMessage.Click += new System.EventHandler(this.labelEndMessage_Click);
            // 
            // btnMain
            // 
            this.btnMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMain.Location = new System.Drawing.Point(324, 244);
            this.btnMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMain.Name = "btnMain";
            this.btnMain.Size = new System.Drawing.Size(226, 50);
            this.btnMain.TabIndex = 1;
            this.btnMain.Text = "Back to Main";
            this.btnMain.UseVisualStyleBackColor = true;
            this.btnMain.Click += new System.EventHandler(this.btnMain_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            // 
            // ThankYouScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.btnMain);
            this.Controls.Add(this.labelEndMessage);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ThankYouScreen";
            this.Text = "ThankYouScreen";
            this.Load += new System.EventHandler(this.ThankYouScreen_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelEndMessage;
        private System.Windows.Forms.Button btnMain;
        private System.Windows.Forms.Timer timer1;
    }
}
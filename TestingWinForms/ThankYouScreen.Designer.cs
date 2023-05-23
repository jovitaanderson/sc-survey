
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
            this.labelEndMessage.AutoSize = true;
            this.labelEndMessage.Location = new System.Drawing.Point(342, 126);
            this.labelEndMessage.Name = "labelEndMessage";
            this.labelEndMessage.Size = new System.Drawing.Size(46, 17);
            this.labelEndMessage.TabIndex = 0;
            this.labelEndMessage.Text = "label1";
            // 
            // btnMain
            // 
            this.btnMain.Location = new System.Drawing.Point(302, 192);
            this.btnMain.Name = "btnMain";
            this.btnMain.Size = new System.Drawing.Size(126, 23);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnMain);
            this.Controls.Add(this.labelEndMessage);
            this.Name = "ThankYouScreen";
            this.Text = "ThankYouScreen";
            this.Load += new System.EventHandler(this.ThankYouScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEndMessage;
        private System.Windows.Forms.Button btnMain;
        private System.Windows.Forms.Timer timer1;
    }
}
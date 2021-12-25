
namespace MagazineLayoutDesigner
{
    partial class ImageManagerWindow
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
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.widthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.heightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(415, 10);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(71, 13);
            this.widthLabel.TabIndex = 0;
            this.widthLabel.Text = "Ширина, мм:";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(415, 49);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(70, 13);
            this.heightLabel.TabIndex = 2;
            this.heightLabel.Text = "Высота, мм:";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(418, 91);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(418, 120);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // widthNumericUpDown
            // 
            this.widthNumericUpDown.Location = new System.Drawing.Point(418, 26);
            this.widthNumericUpDown.Maximum = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.widthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthNumericUpDown.Name = "widthNumericUpDown";
            this.widthNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.widthNumericUpDown.TabIndex = 6;
            this.widthNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthNumericUpDown.ValueChanged += new System.EventHandler(this.widthNumericUpDown_ValueChanged);
            // 
            // heightNumericUpDown
            // 
            this.heightNumericUpDown.Location = new System.Drawing.Point(418, 65);
            this.heightNumericUpDown.Maximum = new decimal(new int[] {
            277,
            0,
            0,
            0});
            this.heightNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightNumericUpDown.Name = "heightNumericUpDown";
            this.heightNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.heightNumericUpDown.TabIndex = 7;
            this.heightNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightNumericUpDown.ValueChanged += new System.EventHandler(this.heightNumericUpDown_ValueChanged);
            // 
            // ImageManagerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 450);
            this.Controls.Add(this.heightNumericUpDown);
            this.Controls.Add(this.widthNumericUpDown);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.widthLabel);
            this.MaximizeBox = false;
            this.Name = "ImageManagerWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ImageManagerWindow";
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.NumericUpDown widthNumericUpDown;
        private System.Windows.Forms.NumericUpDown heightNumericUpDown;
    }
}
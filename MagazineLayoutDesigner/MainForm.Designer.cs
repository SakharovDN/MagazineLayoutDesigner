
namespace MagazineLayoutDesigner
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTextFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(298, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTextToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.savePageToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // loadTextToolStripMenuItem
            // 
            this.loadTextToolStripMenuItem.Name = "loadTextToolStripMenuItem";
            this.loadTextToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadTextToolStripMenuItem.Text = "Загрузить текст";
            this.loadTextToolStripMenuItem.Click += new System.EventHandler(this.loadTextToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addImageToolStripMenuItem,
            this.removeImageToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.imageToolStripMenuItem.Text = "Изображение";
            // 
            // addImageToolStripMenuItem
            // 
            this.addImageToolStripMenuItem.Name = "addImageToolStripMenuItem";
            this.addImageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addImageToolStripMenuItem.Text = "Добавить";
            this.addImageToolStripMenuItem.Click += new System.EventHandler(this.addImageToolStripMenuItem_Click);
            // 
            // removeImageToolStripMenuItem
            // 
            this.removeImageToolStripMenuItem.CheckOnClick = true;
            this.removeImageToolStripMenuItem.Name = "removeImageToolStripMenuItem";
            this.removeImageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeImageToolStripMenuItem.Text = "Удалить";
            this.removeImageToolStripMenuItem.Click += new System.EventHandler(this.removeImageToolStripMenuItem_Click);
            // 
            // savePageToolStripMenuItem
            // 
            this.savePageToolStripMenuItem.Name = "savePageToolStripMenuItem";
            this.savePageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.savePageToolStripMenuItem.Text = "Сохранить страницу";
            this.savePageToolStripMenuItem.Click += new System.EventHandler(this.savePageToolStripMenuItem_Click);
            // 
            // openTextFileDialog
            // 
            this.openTextFileDialog.Filter = "text files (*.txt, *.doc, *.docx)|*.txt; *.doc; *.docx|All files (*.*)|*.*";
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.Filter = "image files (*.jpeg, *.jpg, *.png)|*.jpeg; *.jpg; *.png|All files (*.*)|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.FileName = "page.png";
            this.saveFileDialog.Filter = "image files (*.jpeg, *.jpg, *.png)|*.jpeg; *.jpg; *.png|All files (*.*)|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 275);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MagazineLayoutDesigner";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTextToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openTextFileDialog;
        private System.Windows.Forms.OpenFileDialog openImageFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem savePageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeImageToolStripMenuItem;
    }
}


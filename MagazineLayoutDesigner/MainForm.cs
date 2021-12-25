namespace MagazineLayoutDesigner
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        #region Fields

        private readonly PagePanel _pagePanel;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            _pagePanel = new PagePanel(new Point(10, menuStrip.Height + 10));
            Controls.Add(_pagePanel);
            Width = _pagePanel.Width + 40;
            Height = menuStrip.Height + _pagePanel.Height + 60;
        }

        #endregion

        #region Methods

        private void loadTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openTextFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            switch (Path.GetExtension(openTextFileDialog.FileName))
            {
                case ".txt":
                    _pagePanel.LoadTextFromTxt(openTextFileDialog.FileName);
                    break;

                case ".doc":
                case ".docx":
                    _pagePanel.LoadTextFromDoc(openTextFileDialog.FileName);
                    break;

                default:
                    MessageBox.Show("Файл не может быть прочитан", "Ошибка");
                    break;
            }
        }

        private void addPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openImageFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            switch (Path.GetExtension(openImageFileDialog.FileName))
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                    var imageManagerWindow = new ImageManagerWindow(openImageFileDialog.FileName);

                    if (imageManagerWindow.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    _pagePanel.LoadImage(imageManagerWindow.SelectedImage);
                    break;

                default:
                    MessageBox.Show("Выберите изображение формата .png, .jpg, .jpeg.", "Ошибка");
                    break;
            }
        }

        private void savePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var bitmap = new Bitmap(_pagePanel.Width, _pagePanel.Height);
            _pagePanel.DrawToBitmap(bitmap, new Rectangle(0, 0, _pagePanel.Width, _pagePanel.Height));
            bitmap.Save(saveFileDialog.FileName);
        }

        #endregion
    }
}

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
        }

        #endregion

        #region Methods
        

        private void loadTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            switch (Path.GetExtension(openFileDialog.FileName))
            {
                case ".txt":
                case ".doc":
                case ".docx":
                    _pagePanel.LoadText(openFileDialog.FileName);
                    break;

                default:
                    MessageBox.Show("Файл не может быть прочитан", "Ошибка");
                    break;
            }
        }

        private void addPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}

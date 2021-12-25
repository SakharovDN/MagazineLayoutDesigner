namespace MagazineLayoutDesigner
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class ImageManagerWindow : Form
    {
        #region Fields

        private int _selectedImageWidth;
        private int _selectedImageHeight;
        private readonly Image _image;

        #endregion

        #region Properties

        public PictureBox SelectedImage { get; set; }

        #endregion

        #region Constructors

        public ImageManagerWindow(string fileName)
        {
            InitializeComponent();
            _image = Image.FromFile(fileName);
            var selectedImagePreview = new PictureBox
            {
                Image = _image,
                BackColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(_image.Width, _image.Height),
                MaximumSize = new Size(400, 400),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(selectedImagePreview);
        }

        #endregion

        #region Methods

        private void widthTextBox_TextChanged(object sender, EventArgs e)
        {
            _selectedImageWidth = string.IsNullOrEmpty(widthTextBox.Text) ? 0 : int.Parse(widthTextBox.Text);
        }

        private void heightTextBox_TextChanged(object sender, EventArgs e)
        {
            _selectedImageHeight = string.IsNullOrEmpty(widthTextBox.Text) ? 0 : int.Parse(heightTextBox.Text);
        }

        private void widthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                return;
            }

            e.Handled = true;
        }

        private void heightTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (_selectedImageHeight == 0 || _selectedImageWidth == 0)
            {
                MessageBox.Show("Размеры изображение должны быть больше 0", "Ошибка");
                return;
            }

            if (_selectedImageHeight > 297 || _selectedImageWidth > 210)
            {
                MessageBox.Show("Размеры изображения не могут превышать размеров страницы", "Ошибка");
                return;
            }

            SelectedImage = new PictureBox
            {
                Image = _image,
                Size = new Size(_selectedImageWidth * 4, _selectedImageHeight * 4),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion
    }
}

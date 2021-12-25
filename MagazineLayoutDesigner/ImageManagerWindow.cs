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

        private void okButton_Click(object sender, EventArgs e)
        {
            SelectedImage = new PictureBox
            {
                Image = _image,
                Size = new Size(_selectedImageWidth * PageParameters.SCALE, _selectedImageHeight * PageParameters.SCALE),
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

        private void widthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _selectedImageWidth = Convert.ToInt32(widthNumericUpDown.Value);
            _selectedImageHeight = _selectedImageWidth * _image.Height / _image.Width;
            heightNumericUpDown.Text = _selectedImageHeight.ToString();
        }

        private void heightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _selectedImageHeight = Convert.ToInt32(heightNumericUpDown.Value);
            _selectedImageWidth = _selectedImageHeight * _image.Width / _image.Height;
            widthNumericUpDown.Text = _selectedImageWidth.ToString();
        }

        #endregion
    }
}

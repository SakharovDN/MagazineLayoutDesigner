namespace MagazineLayoutDesigner
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class ImageManagerWindow : Form
    {
        #region Fields

        /// <summary>
        /// Широта выбранного изображения
        /// </summary>
        private int _imageWidth;

        /// <summary>
        /// Высота выбранного изображения
        /// </summary>
        private int _imageHeight;

        #endregion

        #region Properties

        /// <summary>
        /// Объект Image, в котором хранится выбранное изображение
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Объект Size, в котором хранятся задаваемые параметра размера выбранного изображения
        /// </summary>
        public Size ImageSize { get; set; }

        #endregion

        #region Constructors

        public ImageManagerWindow(string fileName)
        {
            InitializeComponent();
            Image = Image.FromFile(fileName);
            var selectedImagePreview = new PictureBox
            {
                Image = Image,
                BackColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(Image.Width, Image.Height),
                MaximumSize = new Size(400, 400),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(selectedImagePreview);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик нажатия кнопки OК
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            ImageSize = new Size(_imageWidth * PageParameters.SCALE, _imageHeight * PageParameters.SCALE);
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки Отмена
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Обработчик изменения значения задаваемой широты изображения
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void widthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _imageWidth = Convert.ToInt32(widthNumericUpDown.Value);
            _imageHeight = _imageWidth * Image.Height / Image.Width;
            heightNumericUpDown.Text = _imageHeight.ToString();
        }

        /// <summary>
        /// Обработчик изменения значения задаваемой высоты изображения
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void heightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _imageHeight = Convert.ToInt32(heightNumericUpDown.Value);
            _imageWidth = _imageHeight * Image.Width / Image.Height;
            widthNumericUpDown.Text = _imageWidth.ToString();
        }

        #endregion
    }
}

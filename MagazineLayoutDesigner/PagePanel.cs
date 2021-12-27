namespace MagazineLayoutDesigner
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using DocumentFormat.OpenXml.Packaging;

    public sealed class PagePanel : Panel
    {
        #region Fields

        /// <summary>
        /// Массив всех слов
        /// </summary>
        private string[] _textContent;

        private readonly TextFormatter _textFormatter;

        #endregion

        #region Properties

        /// <summary>
        /// PictureBox с выбранным изображением
        /// </summary>
        public List<ImagePictureBox> Images { get; set; }

        #endregion

        #region Constructors

        public PagePanel(Point location)
        {
            BorderStyle = BorderStyle.FixedSingle;
            Location = location;
            Width = PageParameters.WIDTH;
            Height = PageParameters.HEIGHT;
            Margin = new Padding(PageParameters.MARGIN);
            BackColor = Color.White;
            Images = new List<ImagePictureBox>();
            _textFormatter = new TextFormatter(Width, Height, Margin.All);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод загружающий текст из txt файла
        /// </summary>
        /// <param name = "fileName"></param>
        public void LoadTextFromTxt(string fileName)
        {
            _textContent = File.ReadAllText(fileName).Split();
            RenderPage();
        }

        /// <summary>
        /// Метод загружающий текст из word файла
        /// </summary>
        /// <param name = "fileName"></param>
        public void LoadTextFromDoc(string fileName)
        {
            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fileName, false))
                {
                    if (wordDocument.MainDocumentPart?.Document.Body != null)
                    {
                        _textContent = wordDocument.MainDocumentPart.Document.Body.InnerText.Split();
                    }
                }

                RenderPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Метод добавляющий изображение
        /// </summary>
        /// <param name = "selectedImage"></param>
        /// <param name = "size"></param>
        public void LoadImage(Image selectedImage, Size size)
        {
            var image = new ImagePictureBox(selectedImage, size, this);
            image.Location = new Point(Margin.All, Margin.All);
            image.ImageLocationChanged += InvokeRenderPage;
            Images.Add(image);
            _textFormatter.AddImage(image);
            RenderPage();
        }

        /// <summary>
        /// Метод удаления изображения
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void RemoveImage(object sender, MouseEventArgs e)
        {
            if (!(sender is ImagePictureBox image))
            {
                return;
            }

            Images.Remove(image);
            _textFormatter.RemoveImage(image);
            RenderPage();
        }

        /// <summary>
        /// Обработчик, включающий режим удаления изображений
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void TurnOnRemoveMode(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;

            foreach (ImagePictureBox image in Images)
            {
                image.MouseClick += RemoveImage;
                image.MouseUp -= image.HandleMouseUp;
                image.MouseMove -= image.HandleMouseMove;
                image.MouseDown -= image.HandleMouseDown;
            }
        }

        /// <summary>
        /// Обработчик, отключающий режим удаления изоюражений
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void TurnOffRemoveMode(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;

            foreach (ImagePictureBox image in Images)
            {
                image.MouseClick -= RemoveImage;
                image.MouseUp += image.HandleMouseUp;
                image.MouseMove += image.HandleMouseMove;
                image.MouseDown += image.HandleMouseDown;
            }
        }

        /// <summary>
        /// Обработчик, который вызывается, когда изображение меняет координаты
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void InvokeRenderPage(object sender, EventArgs e)
        {
            RenderPage();
        }

        /// <summary>
        /// метод отображающий контент страницы на панели
        /// </summary>
        private void RenderPage()
        {
            Controls.Clear();

            foreach (ImagePictureBox image in Images)
            {
                Controls.Add(image);
            }

            if (_textContent != null && _textContent.Length > 0)
            {
                if (_textFormatter.TrySelectLayoutParameters(_textContent, out List<Label> labels))
                {
                    Controls.AddRange(labels.ToArray());
                }
                else
                {
                    MessageBox.Show("Текст слишком большой. Разбейте на части.");
                }
            }
        }

        #endregion
    }
}

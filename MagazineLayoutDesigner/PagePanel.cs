namespace MagazineLayoutDesigner
{
    using System;
    using System.Collections.Concurrent;
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

        /// <summary>
        /// PictureBox с выбранным изображением
        /// </summary>
        private ImagePictureBox _image;

        /// <summary>
        /// Размер шрифта
        /// </summary>
        private double _fontSize;

        /// <summary>
        /// Межстрочный множитель
        /// </summary>
        private double _lineFactor;

        /// <summary>
        /// Ширина свободного места в строке
        /// </summary>
        private int _lineFreeSpaceWidth;

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
        /// <param name = "image"></param>
        /// <param name = "size"></param>
        public void LoadImage(Image image, Size size)
        {
            _image = new ImagePictureBox(image, size, this);
            _image.Location = new Point(Margin.All, Margin.All);
            _image.ImageLocationChanged += InvokeRenderPage;
            RenderPage();
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

            if (_image != null)
            {
                Controls.Add(_image);
            }

            if (_textContent != null && _textContent.Length > 0)
            {
                if (!TrySelectLayoutParameters())
                {
                    MessageBox.Show("Текст слишком большой. Разбейте на части.");
                }
            }
        }

        /// <summary>
        /// Метод подбирающий размер шрифта и межстрочный интервал
        /// </summary>
        /// <returns>true если получилось подобрать размер шрифта и межстрочный интервал, иначе false</returns>
        private bool TrySelectLayoutParameters()
        {
            for (_lineFactor = PageParameters.MAX_LINE_FACTOR;
                 _lineFactor >= PageParameters.MIN_LINE_FACTOR;
                 _lineFactor -= PageParameters.LINE_FACTOR_STEP)
            {
                for (_fontSize = PageParameters.MAX_FONT_SIZE;
                     _fontSize >= PageParameters.MIN_FONT_SIZE;
                     _fontSize -= PageParameters.FONT_POINT_IN_MM)
                {
                    if (!TryFillPageWithText(out List<Label> labels))
                    {
                        continue;
                    }

                    Controls.AddRange(labels.ToArray());
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Метод заполняющий страницу текстом
        /// </summary>
        /// <param name = "labels">Список Label'ов, который добавляется на модель</param>
        /// <returns>true если получилось заполнить страницу, иначе false</returns>
        private bool TryFillPageWithText(out List<Label> labels)
        {
            // очередь из слов
            var words = new ConcurrentQueue<string>(_textContent);
            // список Label'ов
            labels = new List<Label>();
            // задаем начальные координаты 
            int x = Margin.All, y = Margin.All;
            // задаем ширину свободного места в строке
            _lineFreeSpaceWidth = Width - 2 * Margin.All;

            // цикл, пока очередь слов не опустеет, команда TryDequeue достает из очереди слово и удаляет из очереди
            while (words.TryDequeue(out string word))
            {
                // инициализируем label
                var label = new Label
                {
                    AutoSize = true,
                    Text = word,
                    Font = new Font(PageParameters.DEFAULT_FONT_FAMILY, (float)_fontSize)
                };

                // бесконечный цикл, но по сути это цикл "пока в строке есть место"
                while (true)
                {
                    // если ширина свободного места меньше ширины label'a, то переносим координаты на новую строку, и начинаем цикл заново
                    if (_lineFreeSpaceWidth < label.PreferredWidth)
                    {
                        y += Convert.ToInt32(Math.Round(PageParameters.LINE_SPACING * _lineFactor, 0));
                        _lineFreeSpaceWidth = Width - 2 * Margin.All;
                        x = Margin.All;
                        continue;
                    }

                    // если есть изображение, и label как-то не пересекается с ним, то переносим координату x правее изображения, уменьшаем ширину свободного месте и начинаем цикл заново
                    if (_image != null)
                    {
                        if (y + label.PreferredHeight > _image.Location.Y && y + label.PreferredHeight < _image.Location.Y + _image.Height
                            || y > _image.Location.Y && y < _image.Location.Y + _image.Height)
                        {
                            if (x + label.PreferredWidth > _image.Location.X && x + label.PreferredWidth < _image.Location.X + _image.Width
                                || x <= _image.Location.X && x + label.PreferredWidth >= _image.Location.X + _image.Width)
                            {
                                x = _image.Location.X + _image.Width;
                                _lineFreeSpaceWidth = Width - _image.Location.X - _image.Width - Margin.All;
                                continue;
                            }
                        }
                    }

                    // если кончилось место в странице, и слова в очереди остались, то выходим из метода с false (заполнить страницу не получилось)
                    if (y > Height - Margin.All - label.PreferredHeight)
                    {
                        if (words.Count > 0)
                        {
                            return false;
                        }
                    }

                    // останавливаем цикл если слово получилось вставить
                    break;
                }

                // устанавливаем подобранные координаты для label'a, добавляем его в список, увеличивем координату x и уменьшаем ширину свободного пространства для следующего слова
                label.Location = new Point(x, y);
                labels.Add(label);
                x += label.PreferredWidth;
                _lineFreeSpaceWidth -= label.PreferredWidth;
            }

            // цикл остановился, выходим из метода с true (заполнить страницу получилось)
            return true;
        }

        #endregion
    }
}

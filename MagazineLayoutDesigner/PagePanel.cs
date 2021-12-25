namespace MagazineLayoutDesigner
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
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

            Controls.Remove(image);
            Images.Remove(image);
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
                for (_fontSize = PageParameters.MAX_FONT_SIZE; _fontSize >= PageParameters.MIN_FONT_SIZE; _fontSize -= PageParameters.FONT_POINT)
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
            var line = new List<Label>();
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
                    Begin:

                    // если ширина свободного места меньше ширины label'a, то переносим координаты на новую строку, и начинаем цикл заново
                    if (_lineFreeSpaceWidth < label.PreferredWidth)
                    {
                        y += Convert.ToInt32(Math.Round(PageParameters.LINE_SPACING * _lineFactor, 0));
                        _lineFreeSpaceWidth = Width - 2 * Margin.All;
                        x = Margin.All;
                        AlignToWidth(line, Width - Margin.All);
                        line = new List<Label>();
                        goto Begin;
                    }

                    // если есть изображения, и label как-то не пересекается с одним из них, то переносим координату x правее изображения, уменьшаем ширину свободного месте и начинаем цикл заново
                    if (Images != null)
                    {
                        foreach (ImagePictureBox image in Images
                                                         .Where(
                                                              image => y + label.PreferredHeight > image.Location.Y
                                                                       && y + label.PreferredHeight < image.Location.Y + image.Height
                                                                       || y > image.Location.Y && y < image.Location.Y + image.Height).Where(
                                                              image =>
                                                                  x + label.PreferredWidth > image.Location.X
                                                                  && x + label.PreferredWidth < image.Location.X + image.Width
                                                                  || x <= image.Location.X
                                                                  && x + label.PreferredWidth >= image.Location.X + image.Width))
                        {
                            x = image.Location.X + image.Width;
                            _lineFreeSpaceWidth = Width - image.Location.X - image.Width - Margin.All;
                            AlignToWidth(line, image.Location.X);
                            line = new List<Label>();
                            goto Begin;
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
                line.Add(label);
                x += label.PreferredWidth;
                _lineFreeSpaceWidth -= label.PreferredWidth;
            }

            // цикл остановился, выходим из метода с true (заполнить страницу получилось)
            return true;
        }

        /// <summary>
        /// Метод, который выравнивает строку текста по ширине
        /// </summary>
        /// <param name = "labelLine"></param>
        /// <param name = "lineEnd"></param>
        private static void AlignToWidth(IReadOnlyList<Label> labelLine, int lineEnd)
        {
            if (labelLine.Count < 2)
            {
                return;
            }

            int freeSpace = lineEnd - labelLine[0].Location.X;
            int wordSpacing = labelLine.Aggregate(freeSpace, (current, label) => current - label.PreferredWidth) / labelLine.Count;
            int remainderAmount = labelLine.Aggregate(freeSpace, (current, label) => current - label.PreferredWidth) % labelLine.Count;
            int x = labelLine[0].Location.X + labelLine[0].PreferredWidth;
            int[] spaces = new int[labelLine.Count - 1];

            for (int i = 0; i < spaces.Length; i++)
            {
                if (remainderAmount < 1)
                {
                    spaces[i] = wordSpacing;
                }
                else
                {
                    spaces[i] = wordSpacing + 1;
                    remainderAmount--;
                }
            }

            for (int i = 0; i < spaces.Length; i++)
            {
                labelLine[i + 1].Location = new Point(x + spaces[i], labelLine[i + 1].Location.Y);
                x += labelLine[i + 1].PreferredWidth + spaces[i];
            }
        }

        #endregion
    }
}

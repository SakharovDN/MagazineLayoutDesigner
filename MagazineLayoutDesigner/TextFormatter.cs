namespace MagazineLayoutDesigner
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class TextFormatter
    {
        #region Fields

        /// <summary>
        /// Ширина страницы
        /// </summary>
        private readonly int _pageWidth;

        /// <summary>
        /// Высота страницы
        /// </summary>
        private readonly int _pageHeight;

        /// <summary>
        /// Отступ страницы
        /// </summary>
        private readonly int _pageMargin;

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

        /// <summary>
        /// Список изображений, которые нужно будет учитывать при форматировании текста
        /// </summary>
        private readonly List<ImagePictureBox> _images;

        #endregion

        #region Constructors

        public TextFormatter(int pageWidth, int pageHeight, int pageMargin)
        {
            _pageWidth = pageWidth;
            _pageHeight = pageHeight;
            _pageMargin = pageMargin;
            _images = new List<ImagePictureBox>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод подбирающий размер шрифта и межстрочный интервал
        /// </summary>
        /// <returns>true если получилось подобрать размер шрифта и межстрочный интервал, иначе false</returns>
        public bool TrySelectLayoutParameters(string[] textContent, out List<Label> labels)
        {
            for (_lineFactor = PageParameters.MAX_LINE_FACTOR;
                 _lineFactor >= PageParameters.MIN_LINE_FACTOR;
                 _lineFactor -= PageParameters.LINE_FACTOR_STEP)
            {
                for (_fontSize = PageParameters.MAX_FONT_SIZE; _fontSize >= PageParameters.MIN_FONT_SIZE; _fontSize -= PageParameters.FONT_POINT)
                {
                    if (!TryFillPageWithText(textContent, out labels))
                    {
                        continue;
                    }

                    return true;
                }
            }

            labels = null;
            return false;
        }

        /// <summary>
        /// Метод удаления изображения
        /// </summary>
        /// <param name = "image"></param>
        public void RemoveImage(ImagePictureBox image)
        {
            _images.Remove(image);
        }

        /// <summary>
        /// Метод добавления изображения
        /// </summary>
        /// <param name = "image"></param>
        public void AddImage(ImagePictureBox image)
        {
            _images.Add(image);
        }

        /// <summary>
        /// Метод заполняющий страницу текстом
        /// </summary>
        /// <param name = "textContent"></param>
        /// <param name = "labels">Список Label'ов, который добавляется на модель</param>
        /// <returns>true если получилось заполнить страницу, иначе false</returns>
        private bool TryFillPageWithText(IEnumerable<string> textContent, out List<Label> labels)
        {
            // очередь из слов
            var words = new ConcurrentQueue<string>(textContent);
            // список Label'ов
            labels = new List<Label>();
            var line = new List<Label>();
            // задаем начальные координаты 
            int x = _pageMargin, y = _pageMargin;
            // задаем ширину свободного места в строке
            _lineFreeSpaceWidth = _pageWidth - 2 * _pageMargin;

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
                        _lineFreeSpaceWidth = _pageWidth - 2 * _pageMargin;
                        x = _pageMargin;
                        AlignToWidth(line, _pageWidth - _pageMargin);
                        line = new List<Label>();
                        goto Begin;
                    }

                    // если есть изображения, и label как-то не пересекается с одним из них, то переносим координату x правее изображения, уменьшаем ширину свободного месте и начинаем цикл заново
                    if (_images.Count > 0)
                    {
                        foreach (ImagePictureBox image in _images
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
                            _lineFreeSpaceWidth = _pageWidth - image.Location.X - image.Width - _pageMargin;
                            AlignToWidth(line, image.Location.X);
                            line = new List<Label>();
                            goto Begin;
                        }
                    }

                    // если кончилось место в странице, и слова в очереди остались, то выходим из метода с false (заполнить страницу не получилось)
                    if (y > _pageHeight - _pageMargin - label.PreferredHeight)
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
            int wordSpacing = labelLine.Aggregate(freeSpace, (current, label) => current - label.PreferredWidth) / (labelLine.Count - 1);
            int remainderAmount = labelLine.Aggregate(freeSpace, (current, label) => current - label.PreferredWidth) % (labelLine.Count - 1);
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

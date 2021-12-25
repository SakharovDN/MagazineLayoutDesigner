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

        private string[] _textContent;
        private PictureBox _image;
        private double _fontSize;
        private double _lineSpacing;
        private double _lineFactor;
        private int _lineFreeSpaceWidth;
        private Point _mouseDownLocation;

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

        public void LoadTextFromTxt(string fileName)
        {
            _textContent = File.ReadAllText(fileName).Split();
            RenderPage();
        }

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

        public void LoadImage(PictureBox selectedImage)
        {
            _image = selectedImage;
            _image.Location = new Point(Margin.All, Margin.All);
            _image.MouseDown += ImageMouseDown;
            _image.MouseMove += ImageMouseMove;
            _image.MouseUp += ImageMouseUp;
            RenderPage();
        }

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
                    _lineSpacing = GetLineSpacing();

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

        private bool TryFillPageWithText(out List<Label> labels)
        {
            var words = new ConcurrentQueue<string>(_textContent);
            labels = new List<Label>();
            int x = Margin.All, y = Margin.All;
            _lineFreeSpaceWidth = Width - 2 * Margin.All;

            while (words.TryDequeue(out string word))
            {
                var label = new Label
                {
                    AutoSize = true,
                    Text = word,
                    Font = new Font(PageParameters.DEFAULT_FONT_FAMILY, (float)_fontSize)
                };

                while (true)
                {
                    if (_lineFreeSpaceWidth < label.PreferredWidth)
                    {
                        y += Convert.ToInt32(Math.Round(PageParameters.LINE_SPACING * _lineFactor, 0));
                        _lineFreeSpaceWidth = Width - 2 * Margin.All;
                        x = Margin.All;
                        continue;
                    }

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

                    if (y > Height - Margin.All - label.PreferredHeight)
                    {
                        if (words.Count > 0)
                        {
                            return false;
                        }
                    }

                    break;
                }

                label.Location = new Point(x, y);
                labels.Add(label);
                x += label.PreferredWidth;
                _lineFreeSpaceWidth -= label.PreferredWidth;
            }

            return true;
        }

        private double GetLineSpacing()
        {
            var label = new Label
            {
                AutoSize = true,
                Text = @" ",
                Font = new Font(PageParameters.DEFAULT_FONT_FAMILY, (float)_fontSize)
            };
            return label.PreferredWidth * _lineFactor;
        }

        private void ImageMouseUp(object sender, MouseEventArgs e)
        {
            if (_image.Left < Margin.All)
            {
                _image.Left = Margin.All;
            }

            if (_image.Location.X + _image.Width > Width - Margin.All)
            {
                _image.Location = new Point(Width - Margin.All - _image.Width, _image.Location.Y);
            }

            if (_image.Top < Margin.All)
            {
                _image.Top = Margin.All;
            }

            if (_image.Location.Y + _image.Height > Height - Margin.All)
            {
                _image.Location = new Point(_image.Location.X, Height - Margin.All - _image.Height);
            }

            RenderPage();
        }

        private void ImageMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (_image.Left >= Margin.All && _image.Location.X + _image.Width <= Width - Margin.All && _image.Top >= Margin.All
                && _image.Location.Y + _image.Height <= Height - Margin.All)
            {
                _image.Left = e.X + _image.Left - _mouseDownLocation.X;
                _image.Top = e.Y + _image.Top - _mouseDownLocation.Y;
            }
            else if (_image.Left < Margin.All)
            {
                _image.Left = Margin.All;
            }
            else if (_image.Location.X + _image.Width > Width - Margin.All)
            {
                _image.Location = new Point(Width - Margin.All - _image.Width, _image.Location.Y);
            }
            else if (_image.Top < Margin.All)
            {
                _image.Top = Margin.All;
            }
            else if (_image.Location.Y + _image.Height > Height - Margin.All)
            {
                _image.Location = new Point(_image.Location.X, Height - Margin.All - _image.Height);
            }
        }

        private void ImageMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDownLocation = e.Location;
            }
        }

        #endregion
    }
}

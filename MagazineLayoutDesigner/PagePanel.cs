namespace MagazineLayoutDesigner
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public sealed class PagePanel : Panel
    {
        #region Constants

        private const int WIDTH = 210 * 4;
        private const int HEIGHT = 297 * 4;
        private const int MARGIN = 10 * 4;
        private const string DEFAULT_FONT_FAMILY = "Times New Roman";
        private const double FONT_POINT_IN_MM = 0.352777777777777777777;

        private const double MIN_FONT_SIZE = 9.0 * FONT_POINT_IN_MM * 3;
        private const double MAX_FONT_SIZE = 12.0 * FONT_POINT_IN_MM * 3;
        private const double MIN_LINE_FACTOR = 1;
        private const double MAX_LINE_FACTOR = 1.15;

        #endregion

        #region Fields

        private string[] _textContent;
        private ConcurrentQueue<string> _words;
        private double _fontSize;
        private double _lineSpacing;
        private double _lineFactor;
        private int _lineFreeSpaceWidth;

        #endregion

        #region Constructors

        public PagePanel(Point location)
        {
            BorderStyle = BorderStyle.FixedSingle;
            Location = location;
            Width = WIDTH;
            Height = HEIGHT;
            BackColor = Color.White;
            _words = new ConcurrentQueue<string>();

            //using (Graphics g = Graphics.FromImage(BackgroundImage = new Bitmap(WIDTH, HEIGHT)))
            //{
            //    var p = new Pen(Color.Black, 3);
            //    var point1 = new Point(MARGIN, MARGIN);
            //    var point2 = new Point(MARGIN, HEIGHT - MARGIN);
            //    g.DrawLine(p, point1, point2);
            //    point1 = new Point(MARGIN, MARGIN);
            //    point2 = new Point(WIDTH - MARGIN, MARGIN);
            //    g.DrawLine(p, point1, point2);
            //    point1 = new Point(WIDTH - MARGIN, HEIGHT - MARGIN);
            //    point2 = new Point(MARGIN, HEIGHT - MARGIN);
            //    g.DrawLine(p, point1, point2);
            //    point1 = new Point(WIDTH - MARGIN, MARGIN);
            //    point2 = new Point(WIDTH - MARGIN, HEIGHT - MARGIN);
            //    g.DrawLine(p, point1, point2);
            //}
        }

        #endregion

        #region Methods

        public void LoadText(string fileName)
        {
            Controls.Clear();
            _textContent = File.ReadAllText(fileName).Split();
            MessageBox.Show(TrySelectLayoutParameters() ? $"Размер шрифта: {_fontSize}\nМежстрочный множитель: {_lineFactor}" : "Текст не влезает");
        }

        private bool TrySelectLayoutParameters()
        {
            for (_lineFactor = MAX_LINE_FACTOR; _lineFactor >= MIN_LINE_FACTOR; _lineFactor -= 0.01)
            {
                for (_fontSize = MAX_FONT_SIZE; _fontSize >= MIN_FONT_SIZE; _fontSize -= FONT_POINT_IN_MM*4)
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
            _words = new ConcurrentQueue<string>(_textContent);
            labels = new List<Label>();
            int x = MARGIN, y = MARGIN;
            _lineFreeSpaceWidth = Width - 2 * MARGIN;

            while (_words.TryDequeue(out string word))
            {
                var label = new Label
                {
                    AutoSize = true,
                    Text = word,
                    Font = new Font(DEFAULT_FONT_FAMILY, (float)_fontSize)
                };

                if (_lineFreeSpaceWidth < label.PreferredWidth)
                {
                    y += (int)_lineSpacing;
                    _lineFreeSpaceWidth = Width - 2 * MARGIN;
                    x = MARGIN;
                }

                if (y > HEIGHT - MARGIN - label.PreferredHeight)
                {
                    if (_words.Count > 0)
                    {
                        return false;
                    }
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
                Font = new Font(DEFAULT_FONT_FAMILY, (float)_fontSize)
            };
            return label.PreferredWidth * _lineFactor;
        }

        #endregion
    }
}

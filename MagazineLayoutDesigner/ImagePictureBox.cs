namespace MagazineLayoutDesigner
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Класс наследуемый от PictureBox для отображения изображения
    /// </summary>
    public class ImagePictureBox : PictureBox
    {
        #region Fields

        /// <summary>
        /// Объект Point, который хранит координаты, где была нажата ЛКМ
        /// </summary>
        private Point _mouseDownLocation;

        #endregion

        #region Events

        /// <summary>
        /// Событие вызываемое при изменении координат изображения
        /// </summary>
        public event EventHandler ImageLocationChanged;

        #endregion

        #region Constructors

        public ImagePictureBox(Image image, Size size, Control parent)
        {
            Parent = parent;
            Image = image;
            Size = size;
            SizeMode = PictureBoxSizeMode.Zoom;
            MouseDown += HandleMouseDown;
            MouseMove += HandleMouseMove;
            MouseUp += HandleMouseUp;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик, когда пользователь отпускает ЛКМ
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void HandleMouseUp(object sender, MouseEventArgs e)
        {
            // Если изображение находится левее пределов, возвращаем на левый край
            if (Left < Parent.Margin.All)
            {
                Left = Parent.Margin.All;
            }

            // Если изображение находится правее пределов, возвращаем на правый край
            if (Location.X + Width > Parent.Width - Parent.Margin.All)
            {
                Location = new Point(Parent.Width - Parent.Margin.All - Width, Location.Y);
            }

            // Если изображение находится выше пределов, возвращаем на верхний край
            if (Top < Parent.Margin.All)
            {
                Top = Parent.Margin.All;
            }

            // Если изображение находится ниже пределов, возвращаем на нижний край
            if (Location.Y + Height > Parent.Height - Parent.Margin.All)
            {
                Location = new Point(Location.X, Parent.Height - Parent.Margin.All - Height);
            }

            ImageLocationChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Обработчик, когда пользователь двигает мышью при нажатой ЛКМ
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void HandleMouseMove(object sender, MouseEventArgs e)
        {
            // если это не ЛКМ, то выход из метода
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            // Если изображение находится в рарешенных вертикальном и горизонтальном диапазонах, то меняем координаты изображения
            if (Left >= Parent.Margin.All && Location.X + Width <= Parent.Width - Parent.Margin.All && Top >= Parent.Margin.All
                && Location.Y + Height <= Parent.Height - Parent.Margin.All)
            {
                Left = e.X + Left - _mouseDownLocation.X;
                Top = e.Y + Top - _mouseDownLocation.Y;
            }
            // если изображение пытается уйти влево за пределы, возращаем его на левый край 
            else if (Left < Parent.Margin.All)
            {
                Left = Parent.Margin.All;
            }
            // если изображение пытается уйти вправо за пределы, возращаем его на правый край 
            else if (Location.X + Width > Parent.Width - Parent.Margin.All)
            {
                Location = new Point(Parent.Width - Parent.Margin.All - Width, Location.Y);
            }
            // если изображение пытается уйти наверх за пределы, возращаем его на верхний край 
            else if (Top < Parent.Margin.All)
            {
                Top = Parent.Margin.All;
            }
            // если изображение пытается уйти вниз за пределы, возращаем его на нижний край 
            else if (Location.Y + Height > Parent.Height - Parent.Margin.All)
            {
                Location = new Point(Location.X, Parent.Height - Parent.Margin.All - Height);
            }
        }

        /// <summary>
        /// Обработчик, когда пользователь нажимает на ЛКМ
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        public void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDownLocation = e.Location;
            }
        }

        #endregion
    }
}

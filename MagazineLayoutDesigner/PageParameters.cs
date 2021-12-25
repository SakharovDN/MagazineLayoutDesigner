namespace MagazineLayoutDesigner
{
    public static class PageParameters
    {
        #region Constants

        /// <summary>
        /// Масштаб (Тут всё завязано на пикселях, поэтому я сделал масштабирвоание размеров страницы, шрифта и межстрочного интервала.
        /// Если нужно увеличить или уменьшить размер всего, просто меняешь это значение)
        /// </summary>
        public const int SCALE = 4;

        /// <summary>
        /// Ширина страницы
        /// </summary>
        public const int WIDTH = 210 * SCALE;

        /// <summary>
        /// Высота страницы
        /// </summary>
        public const int HEIGHT = 297 * SCALE;

        /// <summary>
        /// размер отступа
        /// </summary>
        public const int MARGIN = 10 * SCALE;

        /// <summary>
        /// Шрифт
        /// </summary>
        public const string DEFAULT_FONT_FAMILY = "Times New Roman";

        /// <summary>
        /// Пункт шрифта, 0.26458333333 - это 1 пиксель в мм
        /// </summary>
        public const double FONT_POINT = 0.26458333333333 * SCALE;

        /// <summary>
        /// Минимальный допустимый размер шрифта
        /// </summary>
        public const double MIN_FONT_SIZE = 9.0 * FONT_POINT;

        /// <summary>
        /// Максимальный допустимый размер шрифта
        /// </summary>
        public const double MAX_FONT_SIZE = 12.0 * FONT_POINT;

        /// <summary>
        /// Одинарный междустрочный интервал
        /// </summary>
        public const double LINE_SPACING = 4.233 * SCALE;

        /// <summary>
        /// Минимальный допустиый межстрочный множитель
        /// </summary>
        public const double MIN_LINE_FACTOR = 1;

        /// <summary>
        /// Максимальный допустимый межстрочный множитель
        /// </summary>
        public const double MAX_LINE_FACTOR = 1.15;

        /// <summary>
        /// Шаг изменения межстрочного множителя
        /// </summary>
        public const double LINE_FACTOR_STEP = 0.01;

        #endregion
    }
}

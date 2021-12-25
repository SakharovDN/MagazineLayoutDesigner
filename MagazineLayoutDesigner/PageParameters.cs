namespace MagazineLayoutDesigner
{
    public static class PageParameters
    {
        #region Constants

        public const int SCALE = 3;
        public const int WIDTH = 210 * SCALE;
        public const int HEIGHT = 297 * SCALE;
        public const int MARGIN = 10 * SCALE;
        public const string DEFAULT_FONT_FAMILY = "Times New Roman";
        public const double FONT_POINT_IN_MM = 3.0 / 4.0 * SCALE * 0.352777777777777777777;
        public const double MIN_FONT_SIZE = 9.0 * FONT_POINT_IN_MM;
        public const double MAX_FONT_SIZE = 12.0 * FONT_POINT_IN_MM;
        public const double MIN_LINE_FACTOR = 1;
        public const double MAX_LINE_FACTOR = 1.15;
        public const double LINE_FACTOR_STEP = 0.01;

        #endregion
    }
}

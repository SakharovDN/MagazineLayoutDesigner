namespace MagazineLayoutDesigner
{
    public class NonFitResult
    {
        #region Properties

        public int WordAmount { get; set; }

        public int FontSize { get; set; }

        #endregion

        #region Constructors

        public NonFitResult(int wordAmount, int fontSize)
        {
            WordAmount = wordAmount;
            FontSize = fontSize;
        }

        #endregion
    }
}

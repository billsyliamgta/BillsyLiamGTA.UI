namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuListItemChangedArgs
    {
        #region Properties

        public int Index { get; set; }

        public dynamic Value { get; set; }

        #endregion

        #region Constructors

        public UIMenuListItemChangedArgs(int index, dynamic value)
        {
            Index = index;
            Value = value;
        }

        #endregion
    }
}
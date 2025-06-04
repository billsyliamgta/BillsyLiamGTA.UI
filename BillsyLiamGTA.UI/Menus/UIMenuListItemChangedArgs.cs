namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuListItemChangedArgs
    {
        #region Fields

        public int Index { get; set; }

        public dynamic Value { get; set; }

        #endregion

        public UIMenuListItemChangedArgs(int index, dynamic value)
        {
            Index = index;
            Value = value;
        }
    }
}
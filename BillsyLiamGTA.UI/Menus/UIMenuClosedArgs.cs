namespace BillsyLiamGTA.UI.Menu
{
    /// <summary>
    /// A class representing the arguments of an menu closing.
    /// </summary>
    public class UIMenuClosedArgs
    {
        #region Properties

        /// <summary>
        /// The menu that was just closed.
        /// </summary>
        public UIMenu Menu { get; }

        #endregion

        public UIMenuClosedArgs(UIMenu menu) 
        {
            Menu = menu;
        }
    }
}
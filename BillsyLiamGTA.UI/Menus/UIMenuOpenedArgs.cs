namespace BillsyLiamGTA.UI.Menu
{
    /// <summary>
    /// A class representing the arguments of an menu opening.
    /// </summary>
    public class UIMenuOpenedArgs
    {
        #region Properties

        /// <summary>
        /// The menu that was just opened.
        /// </summary>
        public UIMenu Menu { get; }

        #endregion

        #region Constructors

        public UIMenuOpenedArgs(UIMenu menu)
        {
            Menu = menu;
        }

        #endregion
    }
}
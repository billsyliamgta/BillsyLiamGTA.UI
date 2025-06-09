namespace BillsyLiamGTA.UI.Menu
{
    /// <summary>
    /// A class representing the arguments of an item activation.
    /// </summary>
    public class UIMenuItemActivatedArgs
    {
        #region Properites

        /// <summary>
        /// The item that was just activated by the menu.
        /// </summary>
        public UIMenuBaseItem Item { get; }

        #endregion
        
        public UIMenuItemActivatedArgs(UIMenuBaseItem item)
        {
            Item = item;
        }
    }
}
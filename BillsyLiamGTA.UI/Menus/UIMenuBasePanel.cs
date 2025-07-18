namespace BillsyLiamGTA.UI.Menus
{
    public abstract class UIMenuBasePanel
    {
        #region Properties

        public float Height { get; set; }

        public enum eDrawOrder
        {
            None = -1,
            BelowHeader = 0,
            BelowItems = 1,
            BelowDescription = 2
        }

        public eDrawOrder DrawOrder { get; set; } = eDrawOrder.BelowHeader;

        #endregion

        #region Constructors

        public UIMenuBasePanel(eDrawOrder drawOrder = eDrawOrder.BelowHeader)
        {
            DrawOrder = drawOrder;
        }

        #endregion

        #region Functions

        public virtual void Draw(float x, float y, float width)
        {
        }

        #endregion
    }
}
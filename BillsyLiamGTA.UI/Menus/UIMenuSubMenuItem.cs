using System;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuSubMenuItem :  UIMenuBaseItem
    {
        #region Properties

        public UIMenu SubMenu { get; set; }

        #endregion

        #region Constructors

        public UIMenuSubMenuItem(string title, string description, UIMenu subMenu) : base(title, description)
        {
            SubMenu = subMenu;
        }

        #endregion

        #region Functions

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            Activated += (object sender, UIMenuItemActivatedArgs e) =>
            {
                if (SubMenu == null) // Throw an exception if the submenu is null.
                {
                    throw new ArgumentNullException("ERROR: Submenu item was activated, however the submenu is null.");
                }

                // If not null, open the submenu.
                Parent.Visible = false;
                SubMenu.PreviousMenu = Parent;
                SubMenu.CurrentSelection = 0;
                SubMenu.Visible = true;
            };
        }

        #endregion
    }
}
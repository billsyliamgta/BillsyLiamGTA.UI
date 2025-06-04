using System;
using System.Drawing;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuSubMenuItem :  UIMenuBaseItem
    {
        public UIMenu SubMenu { get; set; }

        public UIMenuSubMenuItem(string title, string description, UIMenu subMenu) : base(title, description)
        {
            SubMenu = subMenu;
        }

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            Activated += (object sender, UIMenuItemActivatedArgs e) =>
            {
                if (SubMenu == null) // throw an exception if the submenu is null
                {
                    throw new ArgumentNullException("ERROR: UIMenuSubMenuItem was activated, however the submenu is null.");
                }

                Parent.Visible = false;
                SubMenu.PreviousMenu = Parent;
                SubMenu.CurrentSelection = 0;
                SubMenu.Visible = true;
            };
        }
    }
}
using System.Drawing;
using GTA;
using BillsyLiamGTA.UI.Elements;
using static GTA.Game;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuCheckboxItem : UIMenuBaseItem
    {
        public UIMenuCheckboxItem(string title, string description) : base(title, description)
        {

        }
        /// <summary>
        /// The checkbox's texture name.
        /// </summary>
        private string _txn = "shop_box_blank";
        /// <summary>
        /// Whether the checkbox is ticked or not.
        /// </summary>
        public bool IsTicked
        {
            get
            {
                if (_txn == "shop_box_tick" || _txn == "shop_box_tickb")
                {
                    return true;
                }

                return false;
            }
        }

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            UISprite checkbox = new UISprite(new TextureAsset("commonmenu", _txn), new PointF(x + width - 50, y - 6), new SizeF(50, 50));
            if (checkbox.IsCursorAbove() && IsControlJustPressed(Control.Attack) || IsSelected && IsControlJustPressed(Control.FrontendAccept))
            {
                _txn = !IsTicked ? "shop_box_tick" : "shop_box_blank";
            }
            if (_txn == "shop_box_tick" && IsSelected)
            {
                _txn = "shop_box_tickb";
            }
            else if (_txn == "shop_box_tickb" && !IsSelected)
            {
                _txn = "shop_box_tick";
            }
            else if (_txn == "shop_box_blank" && IsSelected)
            {
                _txn = "shop_box_blankb";
            }
            else if (_txn == "shop_box_blankb" && !IsSelected)
            {
                _txn = "shop_box_blank";
            }
            checkbox.Draw();
        }
    }
}
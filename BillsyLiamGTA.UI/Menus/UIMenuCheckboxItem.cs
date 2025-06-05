using BillsyLiamGTA.UI.Elements;
using GTA;
using System;
using System.Collections.Generic;
using System.Drawing;

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
            SSprite checkbox = new SSprite(new TextureAsset("commonmenu", _txn), new PointF(x + width - 50, y - 6), new SizeF(50, 50));
            if (checkbox.IsCursorAbove() && Input.IsControlJustPressed(InputControl.Attack) || IsSelected && Input.IsControlJustPressed(InputControl.FrontendAccept))
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
using System.Drawing;
using GTA;
using BillsyLiamGTA.UI.Elements;
using static GTA.Game;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuCheckboxItem : UIMenuBaseItem
    {
        #region Properties

        /// <summary>
        /// The texture object for the checkbox.
        /// </summary>
        public Texture CheckboxTexture;

        /// <summary>
        /// Whether the checkbox is ticked or not.
        /// </summary>
        private bool _isTicked;
        public bool IsTicked => _isTicked;

        #endregion

        #region Constructors

        public UIMenuCheckboxItem(string title, string description) : base(title, description)
        {
            CheckboxTexture = new Texture("commonmenu");
            _isTicked = false;
            UpdateTextureName();
        }

        #endregion

        #region Functions

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);

            UISprite checkbox = new UISprite(CheckboxTexture, new PointF(x + width - 50, y - 7), new SizeF(50, 50));

            bool clicked = (checkbox.IsCursorAbove() && IsControlJustPressed(Control.Attack)) ||
                           (IsSelected && IsControlJustPressed(Control.FrontendAccept));

            if (clicked)
            {
                _isTicked = !_isTicked;
            }

            UpdateTextureName();
            checkbox.Draw();
        }

        private void UpdateTextureName()
        {
            if (_isTicked)
                CheckboxTexture.Name = IsSelected ? "shop_box_tickb" : "shop_box_tick";
            else
                CheckboxTexture.Name = IsSelected ? "shop_box_blankb" : "shop_box_blank";
        }

        #endregion
    }
}
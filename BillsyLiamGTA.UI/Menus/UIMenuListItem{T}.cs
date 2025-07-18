using System;
using System.Drawing;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;
using static GTA.Game;
using GTA.Math;
using GTA.UI;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuListItem<T> : UIMenuBaseItem
    {
        #region Properties

        public int Index { get; set; } = 0;

        public List<T> Items { get; set; }

        public dynamic CurrentValue
        {
            get
            {
                return Items[Index];
            }
        }

        public event UIMenuListItemChangedEventHandler ItemChanged;

        #endregion

        #region Constructors

        public UIMenuListItem(string title, string description, List<T> items) : base(title, description)
        {
            if (items.Count == 0)
            {
                throw new Exception("ERROR: UIMenuListItem<T> requires at least one item in the list to function.");
            }

            Items = items;
        }

        #endregion

        #region Functions

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            UIText text = new UIText(Items[Index].ToString(), new PointF(x - (IsSelected ? 25 : 10) + width - 2, y + (IsSelected ? 3 : 5)), 0.345f, TextColour, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Right);
            if (IsSelected)
            {
                UISprite arrowRight = new UISprite(new Texture("commonmenu", "arrowright"), new PointF(x + width - 28, y + 7), new SizeF(23f, 23f), TextColour);
                UISprite arrowLeft = new UISprite(new Texture("commonmenu", "arrowleft"), new PointF(x + width - 48 - text.Width, y + 7), new SizeF(23f, 23f), TextColour);
                if (IsControlJustPressed(Control.FrontendRight) || arrowRight.IsCursorAbove() && IsControlJustPressed(Control.Attack))
                {
                    GoRight();
                }
                if (IsControlJustPressed(Control.FrontendLeft) || arrowLeft.IsCursorAbove() && IsControlJustPressed(Control.Attack))
                {
                    GoLeft();
                }
                arrowRight.Draw();
                arrowLeft.Draw();
            }
            text.Draw();
        }

        public void GoRight()
        {
            if (Index < Items.Count - 1)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                Index++;
                ItemChanged?.Invoke(this, new UIMenuListItemChangedArgs(Index, CurrentValue));
            }
        }

        public void GoLeft()
        {
            if (Index > 0)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                Index--;
                ItemChanged?.Invoke(this, new UIMenuListItemChangedArgs(Index, CurrentValue));
            }
        }

        #endregion
    }
}
﻿using System;
using System.Collections.Generic;
using System.Drawing;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuListItem<T> : UIMenuBaseItem
    {
        #region Fields

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

        public UIMenuListItem(string title, string description, List<T> items) : base(title, description)
        {
            if (items.Count == 0)
            {
                throw new Exception("ERROR: UIMenuListItem<T> requires at least one item in the list to function.");
            }

            Items = items;
        }

        #region Methods

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            UIText text = new UIText(Items[Index].ToString(), new PointF(x - (IsSelected ? 25 : 10) + width, y + 5), 0.345f, SuitableTextColor, UIText.eTextFonts.FONT_STANDARD, UIText.eTextAlignments.Right);
            if (IsSelected)
            {
                UISprite arrowRight = new UISprite(new TextureAsset("commonmenu", "arrowright"), new PointF(x - 28 + width, y + 7), new SizeF(25f, 25f), SuitableTextColor);
                UISprite arrowLeft = new UISprite(new TextureAsset("commonmenu", "arrowleft"), new PointF(x - 45 - text.Width + width, y + 7), new SizeF(25f, 25f), SuitableTextColor);
                if (Input.IsControlJustPressed(InputControl.FrontendRight) || arrowRight.IsCursorAbove() && Input.IsControlJustPressed(InputControl.Attack))
                {
                    GoRight();
                }
                if (Input.IsControlJustPressed(InputControl.FrontendLeft) || arrowLeft.IsCursorAbove() && Input.IsControlJustPressed(InputControl.Attack))
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
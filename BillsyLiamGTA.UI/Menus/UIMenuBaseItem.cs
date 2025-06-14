﻿using GTA.Native;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Menu
{
    public abstract class UIMenuBaseItem
    {
        #region Fields

        /// <summary>
        /// The title of the item.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description of the item.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Whether the mouse is currently hovered over the item.
        /// </summary>
        public bool IsHovered { get; set; } = false;
        /// <summary>
        /// If the item is currently selected in the menu.
        /// </summary>
        public bool IsSelected { get; set; } = false;
        /// <summary>
        /// The height of the item.
        /// </summary>
        public float Height { get; protected set; } = 37f;

        public Color DefaultTabColor { get; set; } = Color.FromArgb(155, 0, 0, 0);

        public Color HoveredTabColor { get; set; } = Color.FromArgb(155, 205, 205, 205);

        public Color SelectedTabColor { get; set; } = Color.FromArgb(255, 255, 255, 255);

        public Color DefaultTextColor { get; set; } = Color.FromArgb(255, 255, 255, 255);

        public Color SelectedTextColor { get; set; } = Color.FromArgb(255, 0, 0, 0);

        public Color SuitableTabColor
        {
            get
            {
                return IsHovered ? HoveredTabColor : (IsSelected ? SelectedTabColor : DefaultTabColor);
            }
        }

        public Color SuitableTextColor
        {
            get
            {
                return IsSelected ? SelectedTextColor : DefaultTextColor;
            }
        }

        public event UIMenuItemActivatedEventHandler Activated;

        public UIMenu Parent { get; set; }

        #endregion

        public UIMenuBaseItem(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public virtual void Draw(float x, float y, float width)
        {
            UIRectangle tab = new UIRectangle(
                new PointF(x, y),
                new SizeF(width, Height),
                SuitableTabColor
            );
            tab.Draw();
            new UIText(Title, new PointF(x + 10, y + 5), 0.345f, SuitableTextColor, UIText.eTextFonts.FONT_STANDARD, UIText.eTextAlignments.Left).Draw();
            if (IsSelected)
            {
                if (Input.IsControlJustPressed(InputControl.FrontendAccept) || Input.IsControlJustPressed(InputControl.Attack) && SafezoneTools.IsCursorInArea(tab.Position, tab.Size))
                {
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    Activated?.Invoke(this, new UIMenuItemActivatedArgs(this));
                }
            }
        }
    }
}
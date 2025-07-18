using GTA;
using GTA.Native;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;
using static GTA.Game;
using System.Collections.Generic;
using BillsyLiamGTA.UI.Scaleform;

namespace BillsyLiamGTA.UI.Menu
{
    public abstract class UIMenuBaseItem
    {
        #region Fields

        private const string selectGXTEntry = "FMMC_MC1"; /* GXT: Select */

        private const string backGXTEntry = "FMMC_MC2"; /* GXT: Back */

        #endregion

        #region Properties

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
        public float Height { get; protected set; } = 35.88f;
        /// <summary>
        /// Return's the colour of the text based on whether the item is selected or not.
        /// </summary>
        public Color TextColour
        {
            get
            {
                return IsSelected ? Color.FromArgb(255, 0, 0, 0) : Color.FromArgb(255, 255, 255, 255);
            }
        }
        /// <summary>
        /// A list of instructional buttons that will be displayed when the item is selected.
        /// </summary>
        public List<InstructionalButtonContainer> InstructionalButtons = new List<InstructionalButtonContainer>()
        {
            new InstructionalButtonContainer(Control.FrontendAccept, "Select"),
            new InstructionalButtonContainer(Control.FrontendCancel, "Back")
        };
        /// <summary>
        /// An event that is raised when the item is activated.
        /// </summary>
        public event UIMenuItemActivatedEventHandler Activated;
        /// <summary>
        /// The UIMenu that this item belongs to.
        /// </summary>
        public UIMenu Parent { get; set; }

        #endregion

        #region Constructors

        public UIMenuBaseItem(string title, string description)
        {
            Title = title;
            Description = description;
        }

        #endregion

        #region Functions

        public virtual void Draw(float x, float y, float width)
        {         
            if (IsHovered) // If the item is hovered, draw a rectangle behind it.
            {
                new UIRectangle(
                    new PointF(x, y),
                    new SizeF(width, Height),
                    Color.FromArgb(155, 205, 205, 205)
                ).Draw();
            }

            if (IsSelected) // If the item is selected, draw a gradient navigation background and allow it to be activated.
            {
                UISprite tab = new UISprite(
                    new Texture("commonmenu", "gradient_nav"),
                    new PointF(x, y),
                    new SizeF(width, Height),
                    Color.FromArgb(255, 255, 255, 255)
                );
                tab.Draw();
                if (GameTime - Parent.MenuOpenedGameTime >= 250)
                {
                    if (IsControlJustPressed(Control.FrontendAccept) || IsControlJustPressed(Control.CursorAccept) && SafezoneTools.IsCursorInArea(tab.Position, tab.Size))
                    {
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                        Activated?.Invoke(this, new UIMenuItemActivatedArgs(this));
                    }
                }
            }

            // And finally draw the title text.
            new UIText(Title, new PointF(x + 10, y + 4), 0.345f, TextColour, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Left).Draw();
        }

        #endregion
    }
}
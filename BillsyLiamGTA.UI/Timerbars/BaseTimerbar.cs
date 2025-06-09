using GTA.Native;
using System.Drawing;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    /// <summary>
    /// A base class for creating Timerbars.
    /// </summary>
    public abstract class BaseTimerbar
    {
        #region Fields
        /// <summary>
        /// If the timerbar is thin or not.
        /// </summary>
        public bool Thin { get; set; } = false;
        /// <summary>
        /// Title of the timerbar.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Color of the timerbar.
        /// </summary>
        public Color Color { get; set: } = Color.FromArgb(140, 255, 255, 255);
        /// <summary>
        /// Secordary color of the timerbar.
        /// </summary>
        public Color OverlayColor { get; set; }

        #endregion

        #region Constructors
            
        public BaseTimerbar(string title, bool thin)
        {
            Title = title;
            Thin = thin;
            TimerbarPool.Add(this);
        }

        #endregion

        #region Methods

        public virtual void Draw(float y)
        {
            y += Thin ? bgThinOffset : bgOffset;
            // Draw the background
            Function.Call(Hash.DRAW_SPRITE, "timerbars", "all_black_bg", bgBaseX, y, timerBarWidth, Thin ? timerBarThinHeight : timerBarHeight, 0, Color.R, Color.G, Color.B, Color.A, false, 0);
            if (OverlayColor != null) // Draw the secondary color of the timerbar will draw if its not null
            {
                Function.Call(Hash.DRAW_SPRITE, "timerbars", "all_white_bg", bgBaseX, y, timerBarWidth, Thin ? timerBarThinHeight : timerBarHeight, 0, OverlayColor.R, OverlayColor.G, OverlayColor.B, 140, false, 0);
            }
            // Draw the text
            DrawText(Title, initialX, y - 0.011f, 0, titleScale + 0.1f, Color.FromArgb(255, 240, 240, 240), 2, titleWrap);
        }

        #endregion
    }
}

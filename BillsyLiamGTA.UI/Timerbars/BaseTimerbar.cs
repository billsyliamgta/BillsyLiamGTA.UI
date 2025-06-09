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

        public Color OverlayColor { get; set; }

        #endregion

        public BaseTimerbar(string title, bool thin)
        {
            Title = title;
            Thin = thin;
            TimerbarPool.Add(this);
        }

        public virtual void Draw(float y)
        {
            y += Thin ? bgThinOffset : bgOffset;
            // draw the background
            Function.Call(Hash.DRAW_SPRITE, "timerbars", "all_black_bg", bgBaseX, y, timerBarWidth, Thin ? timerBarThinHeight : timerBarHeight, 0, 255, 255, 255, 140, false, 0);
            // draw the text
            if (OverlayColor != null)
            {
                Function.Call(Hash.DRAW_SPRITE, "timerbars", "all_white_bg", bgBaseX, y, timerBarWidth, Thin ? timerBarThinHeight : timerBarHeight, 0, OverlayColor.R, OverlayColor.G, OverlayColor.B, 140, false, 0);
            }
            DrawText(Title, initialX, y - 0.011f, 0, titleScale + 0.1f, Color.FromArgb(255, 240, 240, 240), 2, titleWrap);
        }
    }
}
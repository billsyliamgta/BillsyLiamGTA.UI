using GTA.Native;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class ProgressTimerbar : BaseTimerbar
    {
        #region Properties

        /// <summary>
        /// The progress value. This ranges between 0.0 and 1.0.
        /// </summary>
        public float Progress { get; set; }
        /// <summary>
        /// Colour of the bar's foreground.
        /// </summary>
        public Color FgColour = Color.FromArgb(255, 140, 140, 140);
        /// <summary>
        /// Colour of the bar's background.
        /// </summary>
        public Color Colour = Color.FromArgb(255, 240, 240, 240);

        #endregion

        #region Constructors

        public ProgressTimerbar(string text, float progress = 0.5f) : base(text, true)
        {
            Progress = progress;
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            base.Draw(y);
            y += barOffset;
            Function.Call(Hash.DRAW_RECT, progressBaseX, y, progressWidth, progressHeight, FgColour.R, FgColour.G, FgColour.B, FgColour.A, false);
            float progress = Extensions.Clamp(Progress, 0.0f, 1.0f);
            float fgWidth = progressWidth * progress;
            float fgX = (progressBaseX - progressWidth * 0.5f) + (fgWidth * 0.5f);
            Function.Call(Hash.DRAW_RECT, fgX, y, fgWidth, progressHeight, Colour.R, Colour.G, Colour.B, Colour.A, false);
        }

        #endregion
    }
}
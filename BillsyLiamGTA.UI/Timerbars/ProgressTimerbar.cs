using GTA.Native;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class ProgressTimerbar : BaseTimerbar
    {
        #region Fields
        /// <summary>
        /// The progress value. This ranges between 0.0 and 1.0.
        /// </summary>
        public float Progress { get; set; }
        /// <summary>
        /// Color of the bar's foreground.
        /// </summary>
        public Color FgColor = Color.FromArgb(255, 140, 140, 140);
        /// <summary>
        /// Main color of the bar.
        /// </summary>
        public Color Color = Color.FromArgb(255, 240, 240, 240);

        #endregion
        public ProgressTimerbar(string text, float progress = 0.5f) : base(text, true)
        {
            Progress = progress;
        }

        public override void Draw(float y)
        {
            base.Draw(y);
            y += barOffset;
            Function.Call(Hash.DRAW_RECT, progressBaseX, y, progressWidth, progressHeight, FgColor.R, FgColor.G, FgColor.B, FgColor.A, false);
            float progress = Extensions.Clamp(Progress, 0.0f, 1.0f);
            float fgWidth = progressWidth * progress;
            float fgX = (progressBaseX - progressWidth * 0.5f) + (fgWidth * 0.5f);
            Function.Call(Hash.DRAW_RECT, fgX, y, fgWidth, progressHeight, Color.R, Color.G, Color.B, Color.A, false);
        }
    }
}
using GTA.Native;
using System.Drawing;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class ProgressTimerbar : BaseTimerbar
    {
        #region Fields

        public float Progress { get; set; }

        public Color FgColor = Color.FromArgb(255, 140, 140, 140);

        public Color Color = Color.FromArgb(255, 240, 240, 240);

        #endregion
        public ProgressTimerbar(string text, float progress) : base(text, true)
        {
            Progress = progress;
        }

        public override void Draw(float y)
        {
            base.Draw(y);
            y += barOffset;
            Function.Call(Hash.DRAW_RECT, progressBaseX, y, progressWidth, progressHeight, FgColor.R, FgColor.G, FgColor.B, FgColor.A, false);
            float progress = Clamp(Progress, 0.0f, 1.0f);
            float fgWidth = progressWidth * Progress;
            float fgX = (progressBaseX - progressWidth * 0.5f) + (fgWidth * 0.5f);
            Function.Call(Hash.DRAW_RECT, fgX, y, fgWidth, progressHeight, Color.R, Color.G, Color.B, Color.A, false);
        }
    }
}
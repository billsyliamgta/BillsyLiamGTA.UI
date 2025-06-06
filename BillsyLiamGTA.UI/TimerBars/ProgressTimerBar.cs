using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.TimerBars
{
    public class ProgressTimerBar : TimerBarBase
    {
        #region Fields

        public float Progress { get; set; } = 0.5f;

        #endregion
        public ProgressTimerBar(string text, float progress = 0.5f) : base(text)
        {
            Progress = progress;
        }

        public override void Draw(SizeF res, PointF safe, int interval)
        {
            base.Draw(res, safe, interval);
            PointF start = new PointF((int)res.Width - safe.X - 160, (int)res.Height - safe.Y - (30 + (4 * interval)));
            new SRectangle(start, new SizeF(150f * Progress, 17f), Color.FromArgb(255, 93, 182, 229)).Draw();
            new SRectangle(start, new SizeF(150f, 17f), Color.FromArgb(155, 93, 182, 229)).Draw();

            if (Progress < 0f)
            {
                Progress = 0f;
            }

            if (Progress > 1f)
            {
                Progress = 1f;
            }
        }
    }
}
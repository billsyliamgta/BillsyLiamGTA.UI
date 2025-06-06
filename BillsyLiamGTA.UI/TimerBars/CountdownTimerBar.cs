using System;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.TimerBars
{
    public class CountdownTimerBar : TimerBarBase
    {
        #region Fields

        /// <summary>
        /// Default color of the timerbar's countdowm text.
        /// </summary>
        public Color DefaultCountdownTextColor { get; set; } = Color.FromArgb(255, 255, 255);
        /// <summary>
        /// Low color of the timerbar's countdown text.
        /// </summary>
        public Color LowCountdownTextColor { get; set; } = Color.FromArgb(224, 50, 50);
        /// <summary>
        /// The variable timer used to count down.
        /// </summary>
        VariableTimer VariableTimer { get; set; }

        #endregion

        public CountdownTimerBar(string text, int time) : base(text)
        {
            VariableTimer = new VariableTimer(time);
            VariableTimer.Start();
        }

        public override void Draw(SizeF res, PointF safe, int interval)
        {
            base.Draw(res, safe, interval);
            VariableTimer.Update(1.0f);
            var time = TimeSpan.FromMilliseconds(VariableTimer.Counter);
            new SText(time.ToString(@"mm\:ss"), new PointF((int)res.Width - safe.X - 10f, (int)res.Height - safe.Y - (40 + 4 * interval)), 0.46f, time.Seconds < 30 ? Color.Red : Color.White, SText.eTextFonts.FONT_STANDARD, SText.eTextAlignments.Right).Draw();
        }
    }
}
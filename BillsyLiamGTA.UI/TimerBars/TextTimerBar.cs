using System.Drawing;

namespace BillsyLiamGTA.UI.TimerBars
{
    public class TextTimerBar : TimerBarBase
    {
        public TextTimerBar(string text) : base(text)
        {
            
        }

        public override void Draw(SizeF res, PointF safe, int interval)
        {
            base.Draw(res, safe, interval);
        }
    }
}
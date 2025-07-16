using System.Drawing;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class TextTimerbar : BaseTimerbar
    {
        #region Properties

        public string Subtitle { get; set; }

        #endregion

        #region Constructors

        public TextTimerbar(string text, string subtitle) : base(text, false)
        {
            Subtitle = subtitle;
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            base.Draw(y);
            y += textOffset;
            DrawText(Subtitle, initialX, y, 0, textScale, Color.FromArgb(255, 240, 240, 240), 2, textWrap);
        }

        #endregion
    }
}
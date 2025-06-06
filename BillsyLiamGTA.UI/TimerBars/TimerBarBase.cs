using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.TimerBars
{
    /// <summary>
    /// A base class for creating timerbars.
    /// </summary>
    public abstract class TimerBarBase
    {
        #region Fields

        /// <summary>
        /// The text of the timerbar.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The font of the timerbar's text.
        /// </summary>
        public SText.eTextFonts TextFont { get; set; } = SText.eTextFonts.FONT_STANDARD;
        /// <summary>
        /// The color of the timerbar's text.
        /// </summary>
        public Color TextColor { get; set; } = Color.White;
        /// <summary>
        /// Color of the timerbar sprite.
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.FromArgb(155, 255, 255, 255);

        #endregion

        #region Constructors

        public TimerBarBase(string text)
        {
            Text = text;
        }

        #endregion

        #region Methods

        public virtual void Draw(SizeF res, PointF safe, int interval)
        {
            new SSprite(new TextureAsset("timerbars", "all_black_bg"), new PointF((int)res.Width - safe.X - 298, (int)res.Height - safe.Y - (40 + (4 * interval))), new SizeF(300f, 37f), BackgroundColor).Draw();
            new SText(Text, new PointF((int)res.Width - safe.X - 220, (int)res.Height - safe.Y - (34 + (4 * interval))), 0.3f, TextColor, TextFont, SText.eTextAlignments.Right).Draw();
        }

        #endregion
    }
}
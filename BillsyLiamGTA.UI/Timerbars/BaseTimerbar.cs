using GTA.Native;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public abstract class BaseTimerbar
    {
        #region Fields

        public bool Thin { get; set; } = false;

        public string Title { get; set; }

        public bool Visible { get; set; } = true;

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
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Title);
            Function.Call(Hash.SET_TEXT_JUSTIFICATION, 2);
            Function.Call(Hash.SET_TEXT_WRAP, 0.0, titleWrap);
            Function.Call(Hash.SET_TEXT_FONT, 0);
            Function.Call(Hash.SET_TEXT_SCALE, 0.0, titleScale + 0.1f);
            Function.Call(Hash.SET_TEXT_COLOUR, 240, 240, 240, 255);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, initialX, y - 0.011f, 0);
        }
    }
}
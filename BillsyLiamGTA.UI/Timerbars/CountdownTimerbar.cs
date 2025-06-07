using System;
using GTA;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class CountdownTimerbar : BaseTimerbar
    {
        #region Fields

        public VariableTimer VariableTimer { get; set; }

        #endregion

        public CountdownTimerbar(string text, int interval) : base(text, false)
        {
            VariableTimer = new VariableTimer(interval);
            VariableTimer.Start();
        }

        public override void Draw(float y)
        {
            VariableTimer.Update(Game.TimeScale);
            base.Draw(y);
            y += textOffset;
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, TimeSpan.FromMilliseconds(VariableTimer.Counter).ToString(@"mm\:ss"));
            Function.Call(Hash.SET_TEXT_JUSTIFICATION, 2);
            Function.Call(Hash.SET_TEXT_WRAP, 0.0, textWrap);
            Function.Call(Hash.SET_TEXT_FONT, 0);
            Function.Call(Hash.SET_TEXT_SCALE, 0.0, textScale);
            Function.Call(Hash.SET_TEXT_COLOUR, 240, 240, 240, 255);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, initialX, y, 0);
        }
    }
}
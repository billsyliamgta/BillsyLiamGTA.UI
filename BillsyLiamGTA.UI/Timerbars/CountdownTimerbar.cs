using System;
using System.Drawing;
using GTA;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;
using GTA.Native;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class CountdownTimerbar : BaseTimerbar
    {
        #region Fields

        private int InternalTimer;

        private bool _flashing { get; set; } = false;

        public bool Flashing
        {
            get
            {
                return _flashing;
            }
            set
            {
                if (value)
                {
                    InternalTimer = Game.GameTime + FlashTime;
                }

                _flashing = value;
            }
        }

        public int FlashTime { get; set; } = 30000;

        public int FlashInterval { get; set; } = 500;

        private int Alpha { get; set; } = 255;

        public Color FlashingColor { get; set; } = Color.FromArgb(224, 50, 50);

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
            var time = TimeSpan.FromMilliseconds(VariableTimer.Counter);
            int alpha = Alpha;

            if (time.Seconds <= 30 && !Flashing)
            {
                Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GTA_ONLINE_STOP_SCORE");
                Function.Call(Hash.TRIGGER_MUSIC_EVENT, "FM_COUNTDOWN_30S");
                OverlayColor = Color.FromArgb(224, 50, 50);
                Flashing = true;
            }

            if (Flashing)
            {
                if (Game.GameTime < InternalTimer)
                {
                    alpha = (Game.GameTime / FlashInterval) % 2 == 0 ? 255 : 0;
                }
            }

            VariableTimer.OnTimerExpired += (sender) =>
            {
                TimerbarPool.Remove(this);
            };

            DrawText(time.ToString(@"mm\:ss"), initialX, y, 0, textScale, Flashing ? Color.FromArgb(alpha, FlashingColor.R, FlashingColor.G, FlashingColor.B) : Color.FromArgb(alpha, 240, 240, 240), 2, textWrap);
        }
    }
}
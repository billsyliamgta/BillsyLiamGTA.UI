﻿using System;
using System.Drawing;
using GTA;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class CountdownTimerbar : BaseTimerbar
    {
        #region Properties

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

        public Color FlashingColour { get; set; } = Color.FromArgb(224, 50, 50);

        public VariableTimer VariableTimer { get; set; }

        #endregion

        #region Constructors

        public CountdownTimerbar(string text, int interval) : base(text, false)
        {
            VariableTimer = new VariableTimer(interval);
            VariableTimer.Start();
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            VariableTimer.Update(Game.TimeScale);
            base.Draw(y);
            y += textOffset;
            var time = TimeSpan.FromMilliseconds(VariableTimer.Counter);
            int alpha = Alpha;

            if (Function.Call<bool>(Hash.PREPARE_MUSIC_EVENT, "FM_COUNTDOWN_30S"))
            {
                if (time.Minutes == 0 && time.Seconds < 31 && !Flashing)
                {
                    Function.Call(Hash.TRIGGER_MUSIC_EVENT, "FM_COUNTDOWN_30S");
                    OverlayColour = Color.FromArgb(224, 50, 50);
                    Flashing = true;
                }
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

            DrawText(time.ToString(@"mm\:ss"), initialX, y, 0, textScale, Flashing ? Color.FromArgb(alpha, FlashingColour.R, FlashingColour.G, FlashingColour.B) : Color.FromArgb(alpha, 240, 240, 240), 2, textWrap);
        }

        #endregion
    }
}
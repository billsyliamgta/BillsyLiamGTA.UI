using GTA;
using System;

namespace BillsyLiamGTA.UI.Elements
{
    public class VariableTimer
    {
        #region Fields

        public delegate void TimerExpired(object sender);

        private int TimerMax;

        private decimal TimerCounter;

        private bool IsRunning;

        public bool AutoReset;

        public int Counter => (int)TimerCounter;

        public event TimerExpired OnTimerExpired;

        #endregion

        #region Constructor

        public VariableTimer(int interval)
        {
            TimerCounter = interval;
            TimerMax = interval;
        }

        #endregion

        #region Methods

        public void AddTime(decimal amount)
        {
            TimerCounter += amount;
        }

        public void RemoveTime(decimal amount)
        {
            TimerCounter -= amount;
            if (TimerCounter < 0m)
            {
                TimerCounter = default(decimal);
            }
        }

        public void Update(float timescale)
        {
            if (!IsRunning)
            {
                return;
            }
            float num = Game.LastFrameTime * 1000f;
            TimerCounter -= (decimal)(num * timescale);
            if (TimerCounter <= 0m)
            {
                OnTimerExpired?.Invoke(this);
                if (AutoReset)
                {
                    TimerCounter += (decimal)TimerMax;
                }
                else
                {
                    Stop();
                }
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Reset()
        {
            TimerCounter = TimerMax;
        }

        #endregion
    }
}
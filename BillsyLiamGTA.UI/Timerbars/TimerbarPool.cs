using GTA;
using GTA.UI;
using GTA.Native;
using System;
using System.Collections.Generic;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class TimerbarPool : Script
    {
        #region Fields

        private static List<BaseTimerbar> pool;

        #endregion

        public TimerbarPool()
        {
            pool = new List<BaseTimerbar>();
            Tick += OnTick;
        }

        #region Methods

        public static void Add(BaseTimerbar bar)
        {
            if (pool != null)
            {
                if (!pool.Contains(bar))
                {
                    pool.Add(bar);
                }
            }
        }

        public static void Remove(BaseTimerbar bar)
        {
            if (pool != null)
            {
                if (pool.Contains(bar))
                {
                    pool.Remove(bar);
                }
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (pool?.Count > 0)
            {
                while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
                {
                    Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "timerbars", false);
                    Wait(0);
                }

                Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 6);
                Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 7);
                Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 8);
                Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 9);
                Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, 82, 66);
                Function.Call(Hash.SET_SCRIPT_GFX_ALIGN_PARAMS, 0.0, 0.0, gfxAlignWidth, gfxAlignHeight);
                float drawY = LoadingPrompt.IsActive || MenuHandler.AreAnyMenusOpen ? initialBusySpinnerY : initialY;
                for (int i = 0; i < pool.Count; i++)
                {
                    BaseTimerbar bar = pool[i];
                    bar.Draw(drawY);
                    drawY -= bar.Thin ? timerBarThinMargin : timerBarMargin;
                }
                Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
            }
            else if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "timerbars"))
            {
                Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "timerbars");
            }
        }

        #endregion
    }
}
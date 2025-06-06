using System;
using System.Drawing;
using System.Collections.Generic;
using BillsyLiamGTA.UI.Elements;
using GTA;
using GTA.UI;
using GTA.Native;

namespace BillsyLiamGTA.UI.TimerBars
{
    public class TimerBarPool : Script
    {
        private static List<TimerBarBase> pool;

        public TimerBarPool()
        {
            pool = new List<TimerBarBase>();
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (pool?.Count > 0)
            {
                SizeF res = SafezoneTools.ResolutionMaintainRatio;
                PointF safe = SafezoneTools.SafezoneBounds;
                int off = 0;
                if (LoadingPrompt.IsActive || MenuHandler.AreAnyMenusOpen)
                {
                    off = 10;
                }
                for (int i = 0; i < pool.Count; i++)
                {
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 6); // Vehicle Name
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 7); // Area Name
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 8); // Vehicle Class
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 9); // Street Name
                    pool[i].Draw(res, safe, (i * 10) + off);
                }
            }
        }

        public static void Add(TimerBarBase bar)
        {
            if (pool != null)
            {
                if (pool.Contains(bar)) return;
                pool.Add(bar);
            }
        }

        public static void Remove(TimerBarBase bar)
        {
            if (pool != null)
            {
                if (!pool.Contains(bar)) return;
                pool.Remove(bar);
            }
        }
    }
}
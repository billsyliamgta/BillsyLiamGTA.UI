using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using BillsyLiamGTA.UI.Menu;
using BillsyLiamGTA.UI.Scaleform;

namespace BillsyLiamGTA.UI.Elements
{
    public class MenuHandler : Script
    {
        private static List<UIMenu> pool;

        public static bool AreAnyMenusOpen
        {
            get
            {
                if (pool != null)
                {
                    if (pool.Count > 0)
                    {
                        foreach (UIMenu menu in pool)
                        {
                            if (menu.Visible)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        public MenuHandler()
        {
            pool = new List<UIMenu>();
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (pool?.Count > 0)
            {
                foreach (UIMenu menu in pool)
                {
                    menu.Draw();
                }
            }
        }

        public static void Add(UIMenu menu)
        {
            if (menu != null && !pool.Contains(menu))
            {
                pool.Add(menu);
            }
        }

        public static void Remove(UIMenu menu)
        {
            if (menu != null && pool.Contains(menu))
            {
                pool.Remove(menu);
            }
        }
    }
}
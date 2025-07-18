using System;
using System.Collections.Generic;
using GTA;
using BillsyLiamGTA.UI.Menu;
using BillsyLiamGTA.UI.Scaleform;

namespace BillsyLiamGTA.UI.Elements
{
    public class MenuHandler : Script
    {
        #region Properties

        private static List<UIMenu> pool;

        private static InstructionalButtons instructionalButtons;

        private static int LastUpdatedSelection = -1;

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

        #endregion

        #region Constructors

        public MenuHandler()
        {
            pool = new List<UIMenu>();
            instructionalButtons = new InstructionalButtons();
            instructionalButtons.IsClickableWithMouse = true;
            Tick += OnTick;
        }

        #endregion

        #region Functions

        private void OnTick(object sender, EventArgs e)
        {
            if (pool?.Count > 0)
            {
                foreach (UIMenu menu in pool)
                {
                    menu.Draw();

                    if (menu.Visible && menu.HasItems())
                    {
                        if (!instructionalButtons.IsLoaded)
                        {
                            instructionalButtons.Load();
                        }
                        else
                        {
                            if (LastUpdatedSelection != menu.CurrentSelection)
                            {
                                instructionalButtons.SetContainers(menu.SelectedItem.InstructionalButtons);
                                instructionalButtons.UpdateScaleform();
                                LastUpdatedSelection = menu.CurrentSelection;
                            }

                            instructionalButtons.Draw();
                        }
                    }
                }
            }
            else
            {
                if (instructionalButtons.IsLoaded)
                {
                    instructionalButtons.Dispose();
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

        #endregion
    }
}
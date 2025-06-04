using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsyLiamGTA.UI.Menu
{
    internal class TestMenuScript : Script
    {
        public TestMenuScript()
        {
            Tick += OnTick;
        }

        UIMenu MainMenu;

        UIMenu HeritageMenu;

        UIMenuListItem<string> GenderListItem;

        UIMenuSubMenuItem HeritageSubItem;

        UIMenuSubMenuItem FeaturesSubMenuItem;

        UIMenuSubMenuItem AppearanceSubItem;

        UIMenuSubMenuItem ApparelSubItem;

        UIMenuSubMenuItem StatsSubItem;

        UIMenuNormalItem SaveItem;

        private void OnTick(object sender, EventArgs e)
        {
            if (MainMenu == null)
            {
                Setup();
            }
        }

        public void Setup()
        {
            MainMenu = new UIMenu("Character Creator", "NEW CHARACTER", false);
            MainMenu.TitleFont = Elements.SText.eTextFonts.FONT_CURSIVE;
            MainMenu.SubtitleColor = Color.FromArgb(47, 92, 115);
            HeritageMenu = new UIMenu("Character Creator", "HERITAGE", false);
            HeritageMenu.TitleFont = Elements.SText.eTextFonts.FONT_CURSIVE;
            HeritageMenu.SubtitleColor = Color.FromArgb(47, 92, 115);
            HeritageMenu.ParentPanel = new UIMenuParentPanel();
            HeritageMenu.ParentPanel.Parent = HeritageMenu;
            GenderListItem = new UIMenuListItem<string>("Sex", "Choose the Sex of your Character.", new List<string>() { "Male", "Female" });
            GenderListItem.ItemChanged += (sender, e) =>
            {
                int model = 1885233650;
                if (e.Value == "Female")
                {
                    model = -1667301416;
                }

                if (Function.Call<bool>(Hash.IS_MODEL_VALID, model) && Function.Call<bool>(Hash.IS_MODEL_IN_CDIMAGE, model))
                {
                    if (Function.Call<int>(Hash.GET_ENTITY_MODEL, Function.Call<int>(Hash.PLAYER_PED_ID)) != model)
                    {
                        Function.Call(Hash.REQUEST_MODEL, model);
                        while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, model)) Wait(0);
                        Function.Call(Hash.SET_PLAYER_MODEL, Function.Call<int>(Hash.PLAYER_ID), model);
                        Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, model);
                        Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Function.Call<int>(Hash.PLAYER_PED_ID));
                    }
                }
            };
            HeritageSubItem = new UIMenuSubMenuItem("Heritage", "", HeritageMenu);
            FeaturesSubMenuItem = new UIMenuSubMenuItem("Features", "", null);
            AppearanceSubItem = new UIMenuSubMenuItem("Appearance", "", null);
            ApparelSubItem = new UIMenuSubMenuItem("Apparel", "", null);
            StatsSubItem = new UIMenuSubMenuItem("Stats", "", null);
            SaveItem = new UIMenuNormalItem("Save & Continue", "");
            SaveItem.DefaultTabColor = Color.FromArgb(47, 92, 115);
            SaveItem.HoveredTabColor = Color.FromArgb(174, 219, 242);
            SaveItem.SelectedTabColor = Color.FromArgb(93, 182, 229);
            SaveItem.DefaultTextColor = Color.White;
            SaveItem.SelectedTextColor = Color.White;
            MainMenu.AddItem(GenderListItem);
            MainMenu.AddItem(HeritageSubItem);
            MainMenu.AddItem(FeaturesSubMenuItem);
            MainMenu.AddItem(AppearanceSubItem);
            MainMenu.AddItem(StatsSubItem);
            MainMenu.AddItem(SaveItem);
            MainMenu.AddItem(new UIMenuSliderItem("dy", "d"));
            MainMenu.MaxOnScreenItems = 3;
            MainMenu.Visible = true;
        }
    }
}
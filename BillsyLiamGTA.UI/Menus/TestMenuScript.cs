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

        UIMenuListItem<string> MomList;

        UIMenuListItem<string> DadList;

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
            MomList = new UIMenuListItem<string>("Mom", "Select the Mom of your Character.", new List<string>()
            {
                Game.GetLocalizedString("FEMALE_0"),
                Game.GetLocalizedString("FEMALE_1"),
                Game.GetLocalizedString("FEMALE_2"),
                Game.GetLocalizedString("FEMALE_3"),
                Game.GetLocalizedString("FEMALE_4"),
                Game.GetLocalizedString("FEMALE_5"),
                Game.GetLocalizedString("FEMALE_6"),
                Game.GetLocalizedString("FEMALE_7"),
                Game.GetLocalizedString("FEMALE_8"),
                Game.GetLocalizedString("FEMALE_9"),
                Game.GetLocalizedString("FEMALE_10"),
                Game.GetLocalizedString("FEMALE_11"),
                Game.GetLocalizedString("FEMALE_12"),
                Game.GetLocalizedString("FEMALE_13"),
                Game.GetLocalizedString("FEMALE_14"),
                Game.GetLocalizedString("FEMALE_15"),
                Game.GetLocalizedString("FEMALE_16"),
                Game.GetLocalizedString("FEMALE_17"),
                Game.GetLocalizedString("FEMALE_18"),
                Game.GetLocalizedString("FEMALE_19"),
                Game.GetLocalizedString("FEMALE_20"),
                Game.GetLocalizedString("SPECIAL_FEMALE_0")
            });
            MomList.ItemChanged += (sender, e) =>
            {
                MomList.Parent.ParentPanel.Mom = e.Index;
            };
            DadList = new UIMenuListItem<string>("Dad", "Select the Dad of your Character.", new List<string>()
            {
                Game.GetLocalizedString("MALE_0"),
                Game.GetLocalizedString("MALE_1"),
                Game.GetLocalizedString("MALE_2"),
                Game.GetLocalizedString("MALE_3"),
                Game.GetLocalizedString("MALE_4"),
                Game.GetLocalizedString("MALE_5"),
                Game.GetLocalizedString("MALE_6"),
                Game.GetLocalizedString("MALE_7"),
                Game.GetLocalizedString("MALE_8"),
                Game.GetLocalizedString("MALE_9"),
                Game.GetLocalizedString("MALE_10"),
                Game.GetLocalizedString("MALE_11"),
                Game.GetLocalizedString("MALE_12"),
                Game.GetLocalizedString("MALE_13"),
                Game.GetLocalizedString("MALE_14"),
                Game.GetLocalizedString("MALE_15"),
                Game.GetLocalizedString("MALE_16"),
                Game.GetLocalizedString("MALE_17"),
                Game.GetLocalizedString("MALE_18"),
                Game.GetLocalizedString("MALE_19"),
                Game.GetLocalizedString("MALE_20"),
                Game.GetLocalizedString("SPECIAL_MALE_0"),
                Game.GetLocalizedString("SPECIAL_MALE_1"),
                Game.GetLocalizedString("SPECIAL_MALE_2"),
            });
            DadList.ItemChanged += (sender, e) =>
            {
                DadList.Parent.ParentPanel.Dad = e.Index;
            };
            HeritageMenu.AddItem(MomList);
            HeritageMenu.AddItem(DadList);
            MainMenu.AddItem(GenderListItem);
            MainMenu.AddItem(HeritageSubItem);
            MainMenu.AddItem(FeaturesSubMenuItem);
            MainMenu.AddItem(AppearanceSubItem);
            MainMenu.AddItem(StatsSubItem);
            MainMenu.AddItem(SaveItem);
            MainMenu.Visible = true;
        }
    }
}
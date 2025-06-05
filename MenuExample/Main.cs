using System;
using GTA;
using BillsyLiamGTA.UI.Menu;
using System.Drawing;

namespace MenuExample
{
    public class Main : Script
    {
        public Main()
        {
            Tick += OnTick;
        }

        #region Fields

        UIMenu Menu;

        UIMenuNormalItem NormalItem;

        UIMenuNormalItem NormalColouredItem;

        UIMenuNormalItem NormalColouredItem2;

        UIMenuCheckboxItem CheckboxItem;

        UIMenuListItem<string> ListItem;

        UIMenuSliderItem SliderItem;

        #endregion

        void ShowMenu()
        {
            Menu = new UIMenu("Menu Example", "Meow Meow Meow");
            NormalItem = new UIMenuNormalItem("Normal Item", "");
            NormalColouredItem = new UIMenuNormalItem("Pretty colours", "");
            NormalColouredItem.DefaultTabColor = Color.FromArgb(155, 203, 54, 148);
            NormalColouredItem2 = new UIMenuNormalItem("More pretty colours", "");
            NormalColouredItem2.DefaultTabColor = Color.FromArgb(255, 133, 85);
            CheckboxItem = new UIMenuCheckboxItem("Tick? Sure thing!", "");
            ListItem = new UIMenuListItem<string>("List of cool things", "On each item you can set specific description text! Veddy nice.", new System.Collections.Generic.List<string>() { "Chicken burger", "Pasta" });
            SliderItem = new UIMenuSliderItem("Slider Item", "");
            Menu.AddItem(NormalItem);
            Menu.AddItem(NormalColouredItem);
            Menu.AddItem(NormalColouredItem2);
            Menu.AddItem(CheckboxItem);
            Menu.AddItem(ListItem);
            Menu.AddItem(SliderItem);
            Menu.AddParentPanel(new UIMenuParentPanel());
            Menu.Visible = true;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Menu == null)
            {
                ShowMenu();
            }
            else if (Menu != null)
            {                
                if (Game.IsControlJustPressed(Control.Context))
                {
                    Menu.Visible = !Menu.Visible;
                }

                if (Game.IsControlJustPressed(Control.ContextSecondary))
                {
                    Menu.BannerEnabled = false;
                }
            }
        }
    }
}
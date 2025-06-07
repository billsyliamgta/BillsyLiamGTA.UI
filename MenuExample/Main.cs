using System;
using GTA;
using BillsyLiamGTA.UI.Menu;
using System.Drawing;
using BillsyLiamGTA.UI.Timerbars;
using System.Collections.Generic;

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

        TextTimerbar TextTimerbar;

        ProgressTimerbar ProgressTimerbar;

        CountdownTimerbar CountdownTimerbar;

        CheckpointTimerbar CheckpointTimerbar;

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
                TextTimerbar = new TextTimerbar("TEXT", false);
                ProgressTimerbar = new ProgressTimerbar("PROGRESS", 0.1f);
                CountdownTimerbar = new CountdownTimerbar("TIME REMAINING", 120000);
                List<CheckpointTimerbar.Checkpoint> checkpoints = new List<CheckpointTimerbar.Checkpoint>();
                for (int i = 0; i < 5; i++)
                {
                    checkpoints.Add(new CheckpointTimerbar.Checkpoint(Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 114, 204, 114), Color.FromArgb(255, 224, 50, 50), i < 2 ? CheckpointTimerbar.CheckpointState.InProgress : CheckpointTimerbar.CheckpointState.Default));
                }
                CheckpointTimerbar = new CheckpointTimerbar("CHECKPOINTS", checkpoints);
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

                ProgressTimerbar.Progress += 0.001f;
            }
        }
    }
}
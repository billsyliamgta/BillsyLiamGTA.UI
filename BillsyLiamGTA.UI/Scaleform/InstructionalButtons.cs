using GTA;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;
using System.Collections.Generic;
using System;

namespace BillsyLiamGTA.UI.Scaleform
{
    public struct InstructionalButtonContainer
    {
        #region Fields

        public Control Button { get; set; }

        public string Text { get; set; }

        #endregion

        public InstructionalButtonContainer(Control button, string text)
        {
            Button = button;
            Text = text;
        }
    }

    public class InstructionalButtons : BaseScaleform
    {
        #region Fields

        private List<InstructionalButtonContainer> Containers;

        private bool Inited { get; set; } = false;

        public bool IsClickableWithMouse { get; set; } = false;

        #endregion

        public InstructionalButtons(params InstructionalButtonContainer[] containers) : base("INSTRUCTIONAL_BUTTONS")
        {
            Containers = new List<InstructionalButtonContainer>();
            if (containers.Length > 0)
            {
                for (int i = 0; i < containers.Length; i++)
                {
                    AddContainer(containers[i]);
                }
            }
        }

        #region Methods

        public void AddContainer(InstructionalButtonContainer container)
        {
            if (Containers != null)
            {
                if (!Containers.Contains(container))
                {
                    Containers.Add(container);
                    UpdateScaleform();
                }
            }
        }

        public void RemoveContainer(InstructionalButtonContainer container)
        {
            if (Containers != null)
            {
                if (Containers.Contains(container))
                {
                    Containers.Remove(container);
                    UpdateScaleform();
                }
            }
        }

        public void SetContainers(List<InstructionalButtonContainer> containers)
        {
            Containers = containers;
        }

        /// <summary>
        /// Pushes the scaleform movie functions to display the containers.
        /// </summary>
        public void UpdateScaleform()
        {
            if (IsLoaded)
            {
                CallFunction("CLEAR_ALL");
                CallFunction("TOGGLE_MOUSE_BUTTONS", IsClickableWithMouse ? 1 : 0);
                if (Containers != null)
                {
                    if (Containers.Count > 0)
                    {
                        for (int i = 0; i < Containers.Count; i++)
                        {
                            CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)Containers[i].Button), Containers[i].Text, true, (int)Containers[i].Button);
                        }
                    }
                }
                CallFunction("DRAW_INSTRUCTIONAL_BUTTONS");
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (!Inited)
            {
                UpdateScaleform();
                Inited = true;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Inited = false;
        }

        #endregion
    }
}
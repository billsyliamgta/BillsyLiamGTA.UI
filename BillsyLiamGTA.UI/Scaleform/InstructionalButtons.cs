using BillsyLiamGTA.UI.Elements;
using GTA.Native;
using System.Collections.Generic;
using System.Drawing;

namespace BillsyLiamGTA.UI.Scaleform
{
    public struct InstructionalButtonContainer
    {
        public InputControl Button { get; set; }

        public string Text { get; set; }

        public InstructionalButtonContainer(InputControl button, string text)
        {
            Button = button;
            Text = text;
        }
    }

    public class InstructionalButtons : BaseScaleform
    {
        #region Fields

        private List<InstructionalButtonContainer> Containers;

        public Color BackgroundColor { get; set; } = Color.FromArgb(155, 0, 0, 0);

        #endregion

        public InstructionalButtons() : base("INSTRUCTIONAL_BUTTONS")
        {
            Containers = new List<InstructionalButtonContainer>();
        }

        public void AddContainer(InstructionalButtonContainer container)
        {
            if (Containers != null)
            {
                if (!Containers.Contains(container))
                {
                    Containers.Add(container);
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
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
            CallFunction("CLEAR_ALL");
            CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
            if (Containers != null)
            {
                if (Containers.Count > 0)
                {
                    for (int i = 0; i < Containers.Count; i++)
                    {
                        CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)Containers[i].Button), Containers[i].Text);
                    }
                }
            }
            CallFunction("DRAW_INSTRUCTIONAL_BUTTONS");
        }
    }
}
using BillsyLiamGTA.UI.Elements;
using GTA.Native;
using System.Collections.Generic;

namespace BillsyLiamGTA.UI.Scaleform
{
    public class InstructionalButtonContainer
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
        public List<InstructionalButtonContainer> Containers;

        public InstructionalButtons() : base("INSTRUCTIONAL_BUTTONS")
        {
            Containers = new List<InstructionalButtonContainer>();
        }

        public void Draw()
        {
            CallFunction("CLEAR_ALL");
            CallFunction("CREATE_CONTAINER");
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
            CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", 0);
            RenderFullscreen();
        }

        public void AddButton(InstructionalButtonContainer button)
        {
            if (Containers != null)
            {
                if (!Containers.Contains(button))
                {
                    Containers.Add(button);
                }
            }
        }

        public void RemoveButton(InstructionalButtonContainer button)
        {
            if (Containers != null)
            {
                if (Containers.Contains(button))
                {
                    Containers.Remove(button);
                }
            }
        }
    }
}
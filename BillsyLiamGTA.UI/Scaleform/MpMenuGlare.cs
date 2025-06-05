using GTA;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Scaleform
{
    public class MpMenuGlare : BaseScaleform
    {
        public MpMenuGlare() : base("MP_MENU_GLARE")
        {

        }

        public override void Draw()
        {
            base.Draw();
            CallFunction("SET_DATA_SLOT", GameplayCamera.RelativeHeading);
        }
    }
}
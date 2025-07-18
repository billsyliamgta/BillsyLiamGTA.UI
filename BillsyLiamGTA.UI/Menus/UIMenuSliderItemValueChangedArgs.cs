namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuSliderItemValueChangedArgs
    {
        #region Properties

        public float Value { get; set; }

        #endregion

        #region Constructors

        public UIMenuSliderItemValueChangedArgs(float value)
        {
            Value = value;
        }

        #endregion
    }
}
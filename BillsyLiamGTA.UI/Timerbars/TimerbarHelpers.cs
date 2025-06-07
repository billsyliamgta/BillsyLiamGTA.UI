namespace BillsyLiamGTA.UI.Timerbars
{
    public static class TimerbarHelpers
    {
        #region Fields

        public const float gfxAlignWidth = 0.952f;

        public const float gfxAlignHeight = 0.949f;

        public const float initialX = 0.795f;

        public const float initialY = 0.923f;

        public const float initialBusySpinnerY = 0.887f;

        public const float bgBaseX = 0.874f;

        public const float progressBaseX = 0.913f;

        public const float checkpointBaseX = 0.9445f;

        public const float bgOffset = 0.008f;

        public const float bgThinOffset = 0.012f;

        public const float textOffset = -0.011f;

        public const float playerTitleOffset = -0.005f;

        public const float barOffset = 0.012f;

        public const float checkpointOffsetX = 0.0094f;

        public const float checkpointOffsetY = 0.012f;

        public const float timerBarWidth = 0.165f;

        public const float timerBarHeight = 0.035f;

        public const float timerBarThinHeight = 0.028f;

        public const float timerBarMargin = 0.0399f;

        public const float timerBarThinMargin = 0.0319f;

        public const float progressWidth = 0.069f;

        public const float progressHeight = 0.011f;

        public const float checkpointWidth = 0.012f;

        public const float checkpointHeight = 0.023f;

        public const float titleScale = 0.202f;

        public const float titleWrap = 0.867f;

        public const float textScale = 0.494f;

        public const float textWrap = 0.95f;

        public const float playerTitleScale = 0.447f;

        #endregion

        #region Methods

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        #endregion
    }
}
using GTA.Native;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuSliderItem : UIMenuBaseItem
    {
        #region Fields

        /// <summary>
        /// Value of the slider. This ranges between 0.0 - 1.0.
        /// </summary>
        public float Value { get; set; } = 1f;
        /// <summary>
        /// The slider's multiplier. Increasing this will make the slider go faster when sliding etc.
        /// </summary>
        public float SlideMultiplier { get; set; } = 1f;
        /// <summary>
        /// Whether the value is increasing.
        /// </summary>
        private bool InternalSlidingRight { get; set; } = false;
        /// <summary>
        /// Whether the value is decreasing.
        /// </summary>
        private bool InternalSlidingLeft { get; set; } = false;
        /// <summary>
        /// The color of the slider.
        /// </summary>
        public Color SliderColor { get; set; } = Color.FromArgb(93, 182, 229);

        private int SoundId;
        /// <summary>
        /// If the gender icons are enabled or not.
        /// </summary>
        public bool GenderIconsEnabled { get; set; } = false;
        /// <summary>
        /// The color of the gender icons when the item is not selected.
        /// </summary>
        public Color GenderIconsDefaultColor { get; set; } = Color.White;
        /// <summary>
        /// The color of the gender icons when the item is selected.
        /// </summary>
        public Color GenderIconsSelectedColor { get; set; } = Color.Black;
        /// <summary>
        /// The suitable color to be used when drawing the item.
        /// </summary>
        public Color SuitableGenderIconsColor
        {
            get
            {
                return IsSelected ? GenderIconsSelectedColor : GenderIconsDefaultColor;
            }
        }
        /// <summary>
        /// The value changed event handler.
        /// </summary>
        public UIMenuSliderItemValueChangedEventHandler ValueChanged;

        #endregion

        #region Constructors

        public UIMenuSliderItem(string title, string description, float value = 0.5f) : base(title, description)
        {
            Value = value;
        }

        #endregion

        #region Methods

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            new UIRectangle(new PointF(x + width - 210 - (GenderIconsEnabled ? 25 : 0), y + 14), new SizeF(200f, 10.25f), Color.FromArgb(155, SliderColor.R, SliderColor.G, SliderColor.B)).Draw();
            new UIRectangle(new PointF(x + width - 210 - (GenderIconsEnabled ? 25 : 0) + (100 * Extensions.Clamp(Value, 0.0f, 1.0f)), y + 14), new SizeF(100f, 10.25f), Color.FromArgb(255, SliderColor.R, SliderColor.G, SliderColor.B)).Draw();
            new UIRectangle(new PointF(x + width - 110 - (GenderIconsEnabled ? 25 : 0), y + 10), new SizeF(3f, 17f), Color.White).Draw();
            if (GenderIconsEnabled)
            {
                new UISprite(new TextureAsset("mpleaderboard", "leaderboard_female_icon"), new PointF(x + width - 275, y), new SizeF(40f, 40f), SuitableGenderIconsColor).Draw();
                new UISprite(new TextureAsset("mpleaderboard", "leaderboard_male_icon"), new PointF(x + width - 35, y), new SizeF(40f, 40f), SuitableGenderIconsColor).Draw();
            }
            if (IsSelected)
            {
                if (Input.IsControlJustPressed(InputControl.FrontendRight) && !InternalSlidingRight && !InternalSlidingLeft)
                {
                    SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, SoundId, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    InternalSlidingRight = true;
                }
                else if (Input.IsControlJustPressed(InputControl.FrontendLeft) && !InternalSlidingRight && !InternalSlidingLeft)
                {
                    SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, SoundId, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    InternalSlidingLeft = true;
                }

                if (InternalSlidingRight) // Increase the value
                {
                    Value = Extensions.Clamp(Value + 0.02f * SlideMultiplier, 0.0f, 1.0f);
                    ValueChanged?.Invoke(this, new UIMenuSliderItemValueChangedArgs(Value));
                    if (Input.IsControlJustReleased(InputControl.FrontendRight) || Value >= 1.0f)
                    {
                        Function.Call(Hash.STOP_SOUND, SoundId);
                        Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                        SoundId = 0;
                        InternalSlidingRight = false;
                    }
                }

                if (InternalSlidingLeft) // Decrease the value
                {
                    Value = Extensions.Clamp(Value - 0.02f * SlideMultiplier, 0.0f, 1.0f);
                    ValueChanged?.Invoke(this, new UIMenuSliderItemValueChangedArgs(Value));
                    if (Input.IsControlJustReleased(InputControl.FrontendLeft) || Value <= 0.0f)
                    {
                        Function.Call(Hash.STOP_SOUND, SoundId);
                        Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                        SoundId = 0;
                        InternalSlidingLeft = false;
                    }
                }
            }
        }

        #endregion
    }
}
using System.Drawing;
using BillsyLiamGTA.UI.Elements;
using GTA.Native;

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

        public Color SuitableGenderIconsColor
        {
            get
            {
                return IsSelected ? GenderIconsSelectedColor : GenderIconsDefaultColor;
            }
        }

        public UIMenuSliderItemValueChangedEventHandler ValueChanged;

        #endregion

        public UIMenuSliderItem(string title, string description, float value = 0.5f) : base(title, description)
        {
            Value = value;
        }

        #region Methods

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            new UIRectangle(new PointF(x + width - 210 - (GenderIconsEnabled ? 25 : 0), y + 14), new SizeF(200f, 10.25f), Color.FromArgb(155, SliderColor.R, SliderColor.G, SliderColor.B)).Draw();
            new UIRectangle(new PointF(x + width - 210 - (GenderIconsEnabled ? 25 : 0) + (100 * Value), y + 14), new SizeF(100f, 10.25f), Color.FromArgb(255, SliderColor.R, SliderColor.G, SliderColor.B)).Draw();
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

                if (InternalSlidingRight) // increase the value
                {
                    Value += 0.02f;
                    ValueChanged?.Invoke(this, new UIMenuSliderItemValueChangedArgs(Value));
                    if (Input.IsControlJustReleased(InputControl.FrontendRight) || Value > 1.0f)
                    {
                        Function.Call(Hash.STOP_SOUND, SoundId);
                        Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                        SoundId = 0;
                        InternalSlidingRight = false;
                    }
                }

                if (InternalSlidingLeft) // decrease the value
                {
                    Value -= 0.02f;
                    ValueChanged?.Invoke(this, new UIMenuSliderItemValueChangedArgs(Value));
                    if (Input.IsControlJustReleased(InputControl.FrontendLeft) || Value < 0.0f)
                    {
                        Function.Call(Hash.STOP_SOUND, SoundId);
                        Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                        SoundId = 0;
                        InternalSlidingLeft = false;
                    }
                }
            }

            // stop the value from going out of bounds
            if (Value > 1f)
            {
                Value = 1f;
            }
            else if (Value < 0f)
            {
                Value = 0f;
            }
        }

        #endregion
    }
}
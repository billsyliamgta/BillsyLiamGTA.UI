using GTA;
using GTA.Native;
using System.Drawing;
using BillsyLiamGTA.UI.Elements;
using static GTA.Game;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuSliderItem : UIMenuBaseItem
    {
        #region Fields

        private const float slideIncreaseValue = 0.02f;

        private const float slideMinValue = 0.0f;

        private const float slideMaxValue = 1.0f;   

        #endregion

        #region Properties

        /// <summary>
        /// Value of the slider. This ranges between 0.0f - 1.0f.
        /// </summary>
        public float Value { get; set; } = 1f;
        /// <summary>
        /// The slider's multiplier.
        /// </summary>
        public float SlideMultiplier { get; set; } = 1f;
        /// <summary>
        /// Whether the value is increasing or not.
        /// </summary>
        private bool SlidingRightInternal { get; set; } = false;
        /// <summary>
        /// Whether the value is decreasing or not.
        /// </summary>
        private bool SlidingLeftInternal { get; set; } = false;
        /// <summary>
        /// The colour of the slider.
        /// </summary>
        public Color SliderColour { get; set; } = Color.FromArgb(93, 182, 229);
        /// <summary>
        /// The sound id for sliding.
        /// </summary>
        private int SoundId;
        /// <summary>
        /// If the gender icons are enabled or not.
        /// </summary>
        public bool GenderIconsEnabled { get; set; } = false;
        /// <summary>
        /// The colour of the gender icons when the item is not selected.
        /// </summary>
        public Color GenderIconsDefaultColour { get; set; } = Color.White;
        /// <summary>
        /// The colour of the gender icons when the item is selected.
        /// </summary>
        public Color GenderIconsSelectedColour { get; set; } = Color.Black;
        /// <summary>
        /// The suitable colour to be used when drawing the item.
        /// </summary>
        public Color SuitableGenderIconsColour
        {
            get
            {
                return IsSelected ? GenderIconsSelectedColour : GenderIconsDefaultColour;
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

        #region Functions

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            new UIRectangle(new PointF(x + width - 210 - (GenderIconsEnabled ? 25 : 0), y + 15), new SizeF(200f, 9f), Color.FromArgb(155, SliderColour.R, SliderColour.G, SliderColour.B)).Draw();
            new UIRectangle(new PointF(x + width - 210 - (GenderIconsEnabled ? 25 : 0) + (100 * Value), y + 15), new SizeF(100f, 9f), Color.FromArgb(255, SliderColour.R, SliderColour.G, SliderColour.B)).Draw();
            new UIRectangle(new PointF(x + width - 110 - (GenderIconsEnabled ? 25 : 0), y + 11), new SizeF(3f, 17f), Color.White).Draw();
            if (GenderIconsEnabled)
            {
                new UISprite(new Texture("mpleaderboard", "leaderboard_female_icon"), new PointF(x + width - 275, y), new SizeF(40f, 40f), SuitableGenderIconsColour).Draw();
                new UISprite(new Texture("mpleaderboard", "leaderboard_male_icon"), new PointF(x + width - 35, y), new SizeF(40f, 40f), SuitableGenderIconsColour).Draw();
            }
            if (IsSelected)
            {
                if (IsControlJustPressed(Control.FrontendRight) && !SlidingRightInternal && !SlidingLeftInternal)
                {
                    SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, SoundId, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    SlidingRightInternal = true;
                }
                else if (IsControlJustPressed(Control.FrontendLeft) && !SlidingRightInternal && !SlidingLeftInternal)
                {
                    SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, SoundId, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    SlidingLeftInternal = true;
                }

                if (SlidingRightInternal) // Increase the value.
                {
                    Value = Extensions.Clamp(Value + slideIncreaseValue * SlideMultiplier, slideMinValue, slideMaxValue); // Clamp the value to stop it from going out of range.
                    ValueChanged?.Invoke(this, new UIMenuSliderItemValueChangedArgs(Value));
                    if (IsControlJustReleased(Control.FrontendRight) || Value >= slideMaxValue)
                    {
                        Function.Call(Hash.STOP_SOUND, SoundId);
                        Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                        SoundId = 0;
                        SlidingRightInternal = false;
                    }
                }

                if (SlidingLeftInternal) // Decrease the value.
                {
                    Value = Extensions.Clamp(Value - slideIncreaseValue * SlideMultiplier, slideMinValue, slideMaxValue); // Clamp the value to stop it from going out of range.
                    ValueChanged?.Invoke(this, new UIMenuSliderItemValueChangedArgs(Value));
                    if (IsControlJustReleased(Control.FrontendLeft) || Value <= slideMinValue)
                    {
                        Function.Call(Hash.STOP_SOUND, SoundId);
                        Function.Call(Hash.RELEASE_SOUND_ID, SoundId);
                        SoundId = 0;
                        SlidingLeftInternal = false;
                    }
                }
            }
        }

        #endregion
    }
}
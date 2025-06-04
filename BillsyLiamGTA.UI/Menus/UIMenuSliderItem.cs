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
        /// Whether the value is increasing.
        /// </summary>
        private bool InternalSlidingRight { get; set; } = false;
        /// <summary>
        /// Whether the value is decreasing.
        /// </summary>
        private bool InternalSlidingLeft { get; set; } = false;

        #endregion

        public UIMenuSliderItem(string title, string description, float value = 0.5f) : base(title, description)
        {
            Value = value;
        }

        #region Methods

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            new SRectangle(new PointF(x + width - 210, y + 13), new SizeF(190f, 11.25f), Color.FromArgb(155, 47, 92, 115)).Draw();
            new SRectangle(new PointF(x + width - 210 + (90 * Value), y + 13), new SizeF(100f, 11.25f), Color.FromArgb(255, 93, 182, 229)).Draw();

            if (IsSelected)
            {
                if (Input.IsControlJustPressed(InputControl.FrontendRight) && !InternalSlidingRight && !InternalSlidingLeft)
                { 
                    InternalSlidingRight = true;
                }
                else if (Input.IsControlJustPressed(InputControl.FrontendLeft) && !InternalSlidingRight && !InternalSlidingLeft)
                {
                    InternalSlidingLeft = true;
                }

                if (InternalSlidingRight) // increase the value
                {
                    Value += 0.03f;
                    if (Input.IsControlJustReleased(InputControl.FrontendRight) || Value >= 1.0f)
                    {
                        InternalSlidingRight = false;
                    }
                }

                if (InternalSlidingLeft) // decrease the value
                {
                    Value -= 0.03f;
                    if (Input.IsControlJustReleased(InputControl.FrontendLeft) || Value <= 0.0f)
                    {
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
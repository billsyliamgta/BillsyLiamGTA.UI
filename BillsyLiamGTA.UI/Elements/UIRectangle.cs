using System.Drawing;
using GTA.UI;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    /// <summary>
    /// A class for drawing Scaled Rectangles.
    /// </summary>
    public class UIRectangle
    {
        #region Properties

        public PointF Position { get; set; }

        public SizeF Size { get; set; }

        public Color Colour { get; set; } = Color.FromArgb(255, 255, 255);

        #endregion

        public UIRectangle(PointF position, SizeF size, Color colour)
        {
            Position = position;
            Size = size;
            Colour = colour;
        }

        #region Functions

        public void Draw()
        {
            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;
            float w = (Size.Width / width);
            float h = (Size.Height / height);
            float x = (Position.X / width) + w * 0.5f;
            float y = (Position.Y / height) + h * 0.5f;
            Function.Call(Hash.DRAW_RECT, x, y, w, h, Colour.R, Colour.G, Colour.B, Colour.A);
        }

        #endregion
    }
}
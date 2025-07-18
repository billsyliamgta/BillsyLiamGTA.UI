using System.Drawing;
using GTA.UI;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    /// <summary>
    /// A class for drawing scaled sprites.
    /// </summary>
    public class UISprite
    {
        #region Properties

        public Texture Texture { get; private set; }

        public PointF Position { get; set; }

        public SizeF Size { get; set; }

        public float Heading { get; set; } = 0f;

        public Color Colour { get; set; } = Color.FromArgb(255, 255, 255);

        #endregion

        #region Constructors

        public UISprite(Texture texture, PointF position, SizeF size)
        {
            Texture = texture;
            Position = position;
            Size = size;
        }

        public UISprite(Texture texture, PointF position, SizeF size, Color colour)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Colour = colour;
        }

        #endregion

        #region Functions

        public void Draw()
        {
            if (!Texture.IsLoaded)
            {
                Texture.Load();
                return;
            }

            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;
            float w = (Size.Width / width);
            float h = (Size.Height / height);
            float x = (Position.X / width) + w * 0.5f;
            float y = (Position.Y / height) + h * 0.5f;
            Function.Call(Hash.DRAW_SPRITE, Texture.Dictionary, Texture.Name, x, y, w, h, Heading, Colour.R, Colour.G, Colour.B, Colour.A);
        }

        public bool IsCursorAbove() => SafezoneTools.IsCursorInArea(Position, Size);

        public void Dispose() => Texture?.Dispose();

        #endregion
    }
}
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    /// <summary>
    /// A class for drawing Scaled Sprites.
    /// </summary>
    public class UISprite
    {
        public TextureAsset TextureAsset { get; private set; }

        public PointF Position { get; set; }

        public SizeF Size { get; set; }

        public float Heading { get; set; }

        public Color Color { get; set; } = Color.FromArgb(255, 255, 255);

        public UISprite(TextureAsset textureAsset, PointF position, SizeF size)
        {
            TextureAsset = textureAsset;
            Position = position;
            Size = size;
        }

        public UISprite(TextureAsset textureAsset, PointF position, SizeF size, Color color)
        {
            TextureAsset = textureAsset;
            Position = position;
            Size = size;
            Color = color;
        }

        public void Draw()
        {
            if (!TextureAsset.IsLoaded)
            {
                TextureAsset.Load();
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
            Function.Call(Hash.DRAW_SPRITE, TextureAsset.Dictionary, TextureAsset.Name, x, y, w, h, Heading, Color.R, Color.G, Color.B, Color.A);
        }

        public bool IsCursorAbove() => SafezoneTools.IsCursorInArea(Position, Size);
        public void Dispose() => TextureAsset?.Dispose();
    }
}
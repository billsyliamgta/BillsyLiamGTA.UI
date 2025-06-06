using System;
using System.Drawing;
using GTA.UI;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    public static class SafezoneTools
    {
        #region Fields

        public static SizeF ResolutionMaintainRatio
        {
            get
            {
                int screenw = Screen.Resolution.Width;
                int screenh = Screen.Resolution.Height;
                float ratio = (float)screenw / screenh;
                float width = 1080f * ratio;
                return new SizeF(width, 1080f);
            }
        }

        public static Point SafezoneBounds
        {
            get
            {
                float t = Function.Call<float>(Hash.GET_SAFE_ZONE_SIZE);
                double g = Math.Round(Convert.ToDouble(t), 2);
                g = (g * 100) - 90;
                g = 10 - g;
                float screenw = Screen.ScaledWidth;
                float screenh = Screen.Height;
                float ratio = (float)screenw / screenh;
                float wmp = ratio * 5.4f;
                return new Point((int)Math.Round(g * wmp), (int)Math.Round(g * 5.4f));

            }
        }

        public static PointF Cursor
        {
            get
            {
                
                float posX = Function.Call<float>(Hash.GET_CONTROL_UNBOUND_NORMAL, 2, 239) * (1080F * Screen.AspectRatio);
                float posY = Function.Call<float>(Hash.GET_CONTROL_UNBOUND_NORMAL, 2, 240) * 1080f;
                return new PointF(posX, posY);
            }
        }

        #endregion

        #region Screen Scaling

        public static float ToXRelative(this float x) => x / (1080f * Screen.AspectRatio);

        public static float ToYRelative(this float y) => y / 1080f;

        #endregion

        public static bool IsCursorInArea(PointF position, SizeF size)
        {
            bool isX = Cursor.X >= position.X && Cursor.X <= position.X + size.Width;
            bool isY = Cursor.Y > position.Y && Cursor.Y < position.Y + size.Height;
            return isX && isY;
        }
    }
}
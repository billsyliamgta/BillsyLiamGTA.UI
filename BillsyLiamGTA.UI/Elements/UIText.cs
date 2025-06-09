using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using GTA.UI;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    public class UIText
    {
        #region Enums

        public enum eTextFonts : int
        {
            FONT_STANDARD = 0,
            FONT_CURSIVE = 1,
            FONT_ROCKSTAR_TAG = 2,
            FONT_LEADERBOAR0D = 3,
            FONT_CONDENSED = 4,
            FONT_STYLE_FIXED_WIDTH_NUMBERS = 5,
            FONT_CONDENSED_NOT_GAMERNAME = 6,
            FONT_STYLE_PRICEDOWN = 7,
            FONT_STYLE_TAXI = 8,
        }

        public enum eTextAlignments : int
        {
            Left = 0,
            Centered = 1,
            Right = 2
        }

        #endregion

        #region Fields

        public string Text { get; set; }

        public eTextFonts Font { get; set; }

        public eTextAlignments Alignment { get; set; }

        public PointF Position { get; set; }

        public PointF RelativePosition
        {
            get
            {
                return new PointF(Position.X.ToXRelative(), Position.Y.ToYRelative());
            }
        }

        public float Scale { get; set; } = 1f;

        public Color Color { get; set; } = Color.FromArgb(255, 255, 255, 255);

        public bool Shadow { get; set; } = false;

        public bool Outline { get; set; } = false;

        public float Wrap { get; set; } = 0f;

        public float Width
        {
            get
            {
                List<string> strings = new List<string>() { Text };
                Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, "CELL_EMAIL_BCON");
                foreach (string text in strings.ToList())
                {
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
                }
                strings.Clear();
                Function.Call(Hash.SET_TEXT_FONT, (int)Font);
                Function.Call(Hash.SET_TEXT_SCALE, 1.0f, Scale);
                Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
                switch (Alignment)
                {
                    case eTextAlignments.Centered:
                        Function.Call(Hash.SET_TEXT_CENTRE, true);
                        break;
                    case eTextAlignments.Right:
                        Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
                        Function.Call(Hash.SET_TEXT_WRAP, 0, Position.X);
                        break;
                }
                return Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, true) * 1f * (1080f * Screen.AspectRatio);
            }
        }

        public int LineCount
        {
            get
            {
                Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING, "CELL_EMAIL_BCON");
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Text);
                return Function.Call<int>(Hash.END_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING, Position.X.ToXRelative(), Position.Y.ToYRelative()); 
            }
        }

        public float LineHeight
        {
            get
            {
                return 1080 * Function.Call<float>(Hash.GET_RENDERED_CHARACTER_HEIGHT, Scale, (int)Font);
            }
        }

        #endregion

        public UIText(string text, PointF position, float scale, Color color, eTextFonts font, eTextAlignments alignment)
        {
            Text = text;
            Position = position;
            Scale = scale;
            Color = color;
            Font = font;
            Alignment = alignment;
        }

        #region Methods

        public void Draw()
        {
            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            float width = height * ratio;

            float x = (Position.X) / width;
            float y = (Position.Y) / height;
            Function.Call(Hash.SET_TEXT_FONT, (int)Font);
            Function.Call(Hash.SET_TEXT_SCALE, 1f, Scale);
            Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
            if (Shadow)
            {
                Function.Call(Hash.SET_TEXT_DROP_SHADOW);
            }
            if (Outline)
            {
                Function.Call(Hash.SET_TEXT_OUTLINE);
            }

            switch (Alignment)
            {
                case eTextAlignments.Centered:
                    Function.Call(Hash.SET_TEXT_CENTRE, true);
                    break;
                case eTextAlignments.Right:
                    Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
                    Function.Call(Hash.SET_TEXT_WRAP, 0, x);
                    break;
            }

            if (Wrap > 0)
            {
                float xsize = (Position.X + Wrap) / width;
                Function.Call(Hash.SET_TEXT_WRAP, x, xsize);
            }
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y);
        }

        #endregion
    }
}
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BillsyLiamGTA.UI.Elements
{
    public class UIText
    {
        #region Enums

        public enum eFonts : int
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

        public enum eAlignments : int
        {
            Left = 0,
            Centre = 1,
            Right = 2
        }

        #endregion

        #region Fields

        public List<string> Chunks = new List<string>();

        #endregion

        #region Properties

        private string text = string.Empty;

        public string Text
        {
            get => text;
            set
            {
                text = value ?? throw new ArgumentNullException(nameof(value));
                SplitString();
            }
        }

        public eFonts Font { get; set; }

        public eAlignments Alignment { get; set; }

        public PointF Position { get; set; }

        public PointF RelativePosition
        {
            get
            {
                return new PointF(Position.X.ToXRelative(), Position.Y.ToYRelative());
            }
        }

        public float Scale { get; set; } = 1f;

        public Color Colour { get; set; } = Color.FromArgb(255, 255, 255, 255);

        public bool Shadow { get; set; } = false;

        public bool Outline { get; set; } = false;

        public float Wrap = 0f;

        public unsafe float Width
        {
            get
            {
                Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, "CELL_EMAIL_BCON");
                Add();
                return Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, true) * 1f * (1080f * Screen.AspectRatio);
            }
        }

        public unsafe int LineCount
        {
            get
            {
                Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING, "CELL_EMAIL_BCON");
                Add();
                return Function.Call<int>(Hash.END_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING, Position.X.ToXRelative(), Position.Y.ToYRelative());
            }
        }

        public unsafe float LineHeight
        {
            get
            {
                Add();
                return 1080 * Function.Call<float>(Hash.GET_RENDERED_CHARACTER_HEIGHT, Scale, (int)Font);
            }
        }

        #endregion

        #region Constructors

        public UIText(string text, PointF position, float scale, Color colour, eFonts font, eAlignments alignment)
        {
            Text = text;
            Position = position;
            Scale = scale;
            Colour = colour;
            Font = font;
            Alignment = alignment;
        }

        #endregion

        #region Functions

        private void SplitString()
        {
            // Create a new list to hold the chunks
            List<string> newChunks = new List<string>();
            // Get the byte representation of the string
            byte[] bytes = Encoding.ASCII.GetBytes(Text);

            // Split the string into chunks of 90 bytes
            for (int i = 0; i < bytes.Length; i += 90)
            {
                int length = Math.Min(90, bytes.Length - i);
                string chunk = Encoding.ASCII.GetString(bytes, i, length);
                newChunks.Add(chunk);
            }

            // Replace chunks with the newly created list
            Chunks = newChunks;
        }

        private unsafe void GetScreenParams(float* x, float* y, float* ratio, float* width)
        {
            int screenw = Screen.Resolution.Width;
            int screenh = Screen.Resolution.Height;
            float height = 1080f;
            *ratio = (float)screenw / screenh;
            *width = height * *ratio;

            *x = (Position.X) / *width;
            *y = (Position.Y) / height;
        }

        private unsafe void Add()
        {
            float x, y, ratio, width;
            GetScreenParams(&x, &y, &ratio, &width);
            foreach (string chunk in Chunks)
            {
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, chunk);
            }
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
                case eAlignments.Centre:
                    Function.Call(Hash.SET_TEXT_CENTRE, true);
                    break;
                case eAlignments.Right:
                    Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
                    Function.Call(Hash.SET_TEXT_WRAP, 0, x);
                    break;
            }
            if (Wrap != 0)
            {
                float xsize = (Position.X + Wrap) / width;
                Function.Call(Hash.SET_TEXT_WRAP, x, xsize);
            }
            Function.Call(Hash.SET_TEXT_FONT, (int)Font);
            Function.Call(Hash.SET_TEXT_SCALE, 1.0f, Scale);
            Function.Call(Hash.SET_TEXT_COLOUR, Colour.R, Colour.G, Colour.B, Colour.A);
        }

        public unsafe void Draw()
        {
            float x, y, ratio, width;
            GetScreenParams(&x, &y, &ratio, &width);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "CELL_EMAIL_BCON");
            Add();
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y);
        }

        #endregion
    }
}
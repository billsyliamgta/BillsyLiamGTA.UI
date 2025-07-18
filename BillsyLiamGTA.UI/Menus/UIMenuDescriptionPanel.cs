using BillsyLiamGTA.UI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BillsyLiamGTA.UI.Menus
{
    public class UIMenuDescriptionPanel : UIMenuBasePanel
    {
        public string Text { get; set; }

        public UIMenuDescriptionPanel(string text, eDrawOrder drawOrder = eDrawOrder.BelowHeader) : base(drawOrder)
        {
            Text = text;
        }

        public override void Draw(float x, float y, float width)
        {
            base.Draw(x, y, width);
            if (!string.IsNullOrEmpty(Text))
            {
                UIText descriptionText = new UIText(Text, new PointF(x + 10f, y + 5), 0.345f, Color.White, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Left);
                descriptionText.Wrap = width - 20;
                UISprite descriptionBg = new UISprite(new Texture("commonmenu", "gradient_bgd"), new PointF(x, y), SizeF.Empty, Color.FromArgb(200, 255, 255, 255));
                int lineCount = descriptionText.LineCount;
                descriptionBg.Size = new SizeF(width, (lineCount * (descriptionText.LineHeight + 5)) + (lineCount) + 10);
                Height = descriptionBg.Size.Height;
                descriptionBg.Draw();
                new UIRectangle(new PointF(x, y), new SizeF(width, 2f), Color.Black).Draw();
                descriptionText.Draw();
            }
        }
    }
}
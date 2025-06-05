using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuParentPanel
    {
        #region Fields

        public int Mom { get; set; }

        public int Dad { get; set; }

        private int AnimAlpha { get; set; } = 0;

        public UIMenu Parent { get; set; }

        #endregion

        #region Constructors

        public UIMenuParentPanel() { }

        public UIMenuParentPanel(int mom, int dad)
        {
            Mom = mom;
            Dad = dad;
        }

        

        #endregion

        #region Methods

        private string getMomTextureName()
        {
            switch (Mom)
            {
                case 21:
                    {
                        return "special_female_0";
                    }
            }

            return $"female_{Mom}";
        }

        private string getDadTextureName()
        {
            switch (Dad)
            {
                case 21:
                    {
                        return "special_male_0";
                    }
                case 22:
                    {
                        return "special_male_1";
                    }
                case 23:
                    {
                        return "special_male_2";
                    }
            }

            return $"male_{Dad}";
        }

        public void Draw(float y)
        {
            if (Parent.Visible)
            {
                if (AnimAlpha < 255)
                {
                    AnimAlpha += 5;
                }
            }

            Parent.MenuClosed += (sender, e) =>
            {
                AnimAlpha = 0;
            };

            PointF safe = SafezoneTools.SafezoneBounds;
            new SSprite(new TextureAsset("pause_menu_pages_char_mom_dad", "mumdadbg"), new PointF(safe.X - 2f, safe.Y + y), new SizeF(435, 235)).Draw();
            new SSprite(new TextureAsset("char_creator_portraits", getMomTextureName()), new PointF(safe.X - 2f + 5, safe.Y + y), new SizeF(225, 235), Color.FromArgb(AnimAlpha, 255, 255, 255)).Draw();
            new SSprite(new TextureAsset("char_creator_portraits", getDadTextureName()), new PointF(safe.X - 2f + 200, safe.Y + y), new SizeF(225, 235), Color.FromArgb(AnimAlpha, 255, 255, 255)).Draw();
            new SSprite(new TextureAsset("pause_menu_pages_char_mom_dad", "vignette"), new PointF(safe.X - 2f, safe.Y + y), new SizeF(435, 235)).Draw();
        }

        #endregion
    }
}
using System.Drawing;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Menu
{
    public class UIMenuParentPanel
    {
        #region Properties

        public int Mom { get; set; }

        public int Dad { get; set; }

        private int Alpha { get; set; } = 0;

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
                if (Alpha < 255)
                {
                    Alpha += 5;
                }
            }

            Parent.MenuClosed += (sender, e) =>
            {
                Alpha = 0;
            };

            PointF safe = SafezoneTools.SafezoneBounds;
            new UISprite(new TextureAsset("pause_menu_pages_char_mom_dad", "mumdadbg"), new PointF(safe.X, safe.Y + y), new SizeF(Parent.Width, 235)).Draw();
            new UISprite(new TextureAsset("char_creator_portraits", getMomTextureName()), new PointF(safe.X + 10, safe.Y + y), new SizeF(225, 235), Color.FromArgb(Alpha, 255, 255, 255)).Draw();
            new UISprite(new TextureAsset("char_creator_portraits", getDadTextureName()), new PointF(safe.X + 200, safe.Y + y), new SizeF(225, 235), Color.FromArgb(Alpha, 255, 255, 255)).Draw();
            new UISprite(new TextureAsset("pause_menu_pages_char_mom_dad", "vignette"), new PointF(safe.X, safe.Y + y), new SizeF(Parent.Width, 235)).Draw();
        }

        #endregion
    }
}
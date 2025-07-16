using System;
using System.Drawing;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using static GTA.Game;
using BillsyLiamGTA.UI.Elements;
using BillsyLiamGTA.UI.Scaleform;

namespace BillsyLiamGTA.UI.Menu
{
    /// <summary>
    /// A class for creating Rockstar like UI menus.
    /// </summary>
    public class UIMenu
    {
        #region Properties

        /// <summary>
        /// The visibility of the menu.
        /// </summary>
        private bool _visible;
        /// <summary>
        /// The visibility of the menu.
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (value)
                {
                    if (GlareEffectEnabled && GlareEffectScaleform == null)
                    {
                        GlareEffectScaleform = new MpMenuGlare();
                        GlareEffectScaleform.Load();
                    }
                    MenuOpened?.Invoke(this, new UIMenuOpenedArgs(this));
                }
                else
                {
                    GlareEffectScaleform?.Dispose();
                    GlareEffectScaleform = null;
                    BannerTexture?.Dispose();
                    MenuClosed?.Invoke(this, new UIMenuClosedArgs(this));
                }

                _visible = value;
            }
        }
        /// <summary>
        /// If the menu's banner is enabled or not.
        /// </summary>
        public bool BannerEnabled { get; set; } = true;
        /// <summary>
        /// The menu banner's texture asset.
        /// </summary>
        public TextureAsset BannerTexture { get; set; }
        /// <summary>
        /// The colour of the menu's banner texture.
        /// </summary>
        public Color BannerColour { get; set; } = Color.FromArgb(255, 255, 255, 255);
        /// <summary>
        /// The menu's glare scaleform.
        /// </summary>
        public MpMenuGlare GlareEffectScaleform;
        /// <summary>
        /// Whether the menu's glare effect is enabled or not.
        /// </summary>
        public bool GlareEffectEnabled { get; set; } = true;
        /// <summary>
        /// Whether the menu can be closed by the player or not.
        /// </summary>
        public bool CanCloseMenu { get; set; } = true;
        /// <summary>
        /// Size of the menu's banner.
        /// </summary>
        private Size BannerSize = new Size(432, 98);
        /// <summary>
        /// Size of the menu's header.
        /// </summary>
        private Size HeaderSize = new Size(432, 37);
        /// <summary>
        /// The width of the menu.
        /// </summary>
        private int _width { get; set; } = 432;
        /// <summary>
        /// The width of the menu.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                BannerSize.Width = value;
                HeaderSize.Width = value;
                _width = value;
            }
        }
        /// <summary>
        /// The calculated height of the menu.
        /// </summary>
        public int CalculatedHeight
        {
            get
            {
                int height = HeaderSize.Height;
                if (BannerEnabled)
                {
                    height += BannerSize.Height;
                }
                return height;
            }
        }
        /// <summary>
        /// The gap between the menu and the scroll indicator.
        /// </summary>
        public int ScrollIndicatorMargin { get; set; } = 3;
        /// <summary>
        /// The gap between the menu and the description.
        /// </summary>
        public int DescriptionMargin { get; set; } = 5;
        /// <summary>
        /// The menu's parent panel. Recreation from GTA Online heritage UI.
        /// </summary>
        private UIMenuParentPanel ParentPanel { get; set; }
        /// <summary>
        /// The menu banner's title text.
        /// </summary>
        public string Title { get; set; } = "Title";
        /// <summary>
        /// The font of the menu banner's title.
        /// </summary>
        public UIText.eFonts TitleFont { get; set; } = UIText.eFonts.FONT_CONDENSED;
        /// <summary>
        /// The menu banner's title text colour.
        /// </summary>
        public Color TitleColour { get; set; } = Color.White;
        /// <summary>
        /// The menu's header subtitle text.
        /// </summary>
        public string Subtitle { get; set; } = "Subtitle";
        /// <summary>
        /// The menu's header subtitle text colour.
        /// </summary>
        public Color SubtitleColour { get; set; } = Color.White;
        /// <summary>
        /// The current selection of the menu. This is the index of the item in the <see cref="Items"/> list.
        /// </summary>
        private int _currentSelection;
        /// <summary>
        /// The current selection of the menu. This is the index of the item in the <see cref="Items"/> list.
        /// </summary>
        public int CurrentSelection
        {
            get
            {
                return _currentSelection;
            }
            set
            {
                if (value > Items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "ERROR: Current Selection is greater than items count.");
                }

                _currentSelection = value;
            }
        }
        /// <summary>
        /// The max amount of items that be drawn on screen from this menu.
        /// </summary>
        public int MaxOnScreenItems { get; set; } = 4;
        /// <summary>
        /// The list of controls that are disabled when the menu is visible.
        /// </summary>
        public List<Control> DisabledControls = new List<Control>()
        {
            Control.FrontendPause,
            Control.FrontendPauseAlternate,
            Control.Phone,
            Control.PhoneUp,
            Control.PhoneDown,
            Control.PhoneLeft,
            Control.PhoneRight,
            Control.Aim,
            Control.Attack,
            Control.Attack2,
            Control.VehicleAim,
            Control.VehicleAttack,
            Control.VehicleAttack2,
            Control.LookUpDown,
            Control.LookLeftRight,
            (Control)37,
            (Control)199,
        };
        /// <summary>
        /// A list containing the menu's items.
        /// </summary>
        private List<UIMenuBaseItem> Items { get; set; }
        /// <summary>
        /// Return's the currently selected item in the menu.
        /// </summary>
        public UIMenuBaseItem SelectedItem
        {
            get
            {
                return Items[CurrentSelection];
            }
        }
        /// <summary>
        /// The previous menu if this has been opened from <see cref="UIMenuSubMenuItem"/>.
        /// </summary>
        public UIMenu PreviousMenu { get; set; }
        /// <summary>
        /// The menu opened event handler.
        /// </summary>
        public UIMenuOpenedEventHandler MenuOpened;
        /// <summary>
        /// The menu closed event handler.
        /// </summary>
        public UIMenuClosedEventHandler MenuClosed;

        private SizeF SearchAreaSize = new SizeF(30, 1080);

        private PointF SearchAreaRight = new PointF(1f.ToXScaled() - 30, 0);

        #endregion

        #region Constructors

        public UIMenu(string title, string subtitle, params UIMenuBaseItem[] items)
        {
            Items = new List<UIMenuBaseItem>();
            AddItems(items);
            Title = title;
            Subtitle = subtitle;
            BannerTexture = new TextureAsset("commonmenu", "interaction_bgd");
            MenuHandler.Add(this);
        }

        public UIMenu(string title, string subtitle, TextureAsset bannerTexture, params UIMenuBaseItem[] items)
        {
            Items = new List<UIMenuBaseItem>();
            AddItems(items);
            Title = title;
            Subtitle = subtitle;
            BannerTexture = bannerTexture;
            MenuHandler.Add(this);
        }

        #endregion

        #region Methods

        private void DrawDescription(string text, float x, float y)
        {
            y += DescriptionMargin;
            if (!string.IsNullOrEmpty(text))
            {
                UIText descriptionText = new UIText(text, new PointF(x + 10f, y + 5), 0.345f, Color.White, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Left);
                descriptionText.Wrap = Width - 10;
                UISprite descriptionBg = new UISprite(new TextureAsset("commonmenu", "gradient_bgd"), new PointF(x, y), SizeF.Empty, Color.FromArgb(155, 255, 255, 255));
                int lineCount = descriptionText.LineCount;
                descriptionBg.Size = new SizeF(Width, (lineCount * (descriptionText.LineHeight + 5)) + (lineCount) + 10);
                descriptionBg.Draw();
                new UIRectangle(new PointF(x, y), new SizeF(Width, 2f), Color.Black).Draw();
                descriptionText.Draw();
            }
        }

        public void Draw()
        {
            if (!Visible) return;

            if (DisabledControls?.Count > 0)
            {
                for (int i = 0; i < DisabledControls.Count; i++)
                {
                    DisableControlThisFrame(DisabledControls[i]);
                    Function.Call(Hash.HUD_SUPPRESS_WEAPON_WHEEL_RESULTS_THIS_FRAME);
                }
            }

            if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE))
            {
                Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);

                if (SafezoneTools.IsCursorInArea(PointF.Empty, SearchAreaSize))
                {
                    Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 6);
                    GameplayCamera.RelativeHeading += 5;
                }
                else if (SafezoneTools.IsCursorInArea(SearchAreaRight, SearchAreaSize))
                {
                    Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 7);
                    GameplayCamera.RelativeHeading -= 5;
                }
                else
                {
                    Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 1);
                }
            }

            PointF safe = SafezoneTools.SafezoneBounds;
            if (BannerEnabled)
            {
                new UISprite(BannerTexture, new PointF(safe.X, safe.Y), BannerSize, BannerColour).Draw();
                new UIText(Title, new PointF(safe.X + 10, safe.Y + (BannerSize.Height * 0.25f) - (0.9f * 0.25f)), 0.9f, TitleColour, TitleFont, UIText.eAlignments.Left).Draw();
                if (GlareEffectEnabled && GlareEffectScaleform != null)
                {
                    GlareEffectScaleform.Draw();
                }
            }
            
            new UIRectangle(new PointF(safe.X, safe.Y + (BannerEnabled ? BannerSize.Height : 0)), new SizeF(BannerSize.Width, 37), Color.Black).Draw();
            new UIText(Subtitle.ToUpper(), new PointF(safe.X + 10, safe.Y + (BannerEnabled ? BannerSize.Height : 0) + 5), 0.345f, SubtitleColour, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Left).Draw();
            float y = CalculatedHeight;
            if (ParentPanel != null)
            {
                ParentPanel.Draw(y);
                y += 235;
            }
            if (Items?.Count > 0)
            {
                new UIText($"{CurrentSelection + 1} / {Items?.Count}", new PointF(safe.X + Width - 10, safe.Y + (BannerEnabled ? BannerSize.Height : 0) + 5), 0.345f, Color.White, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Right).Draw();

                int totalItems = Items.Count;
                int startIndex = 0;
                int endIndex = Math.Min(MaxOnScreenItems - 1, totalItems - 1);

                if (CurrentSelection >= MaxOnScreenItems)
                {
                    startIndex = Math.Min(CurrentSelection - (MaxOnScreenItems - 1), totalItems - MaxOnScreenItems);
                    endIndex = Math.Min(startIndex + MaxOnScreenItems - 1, totalItems - 1);
                }

                for (int i = startIndex; i <= endIndex; i++)
                {
                    Items[i].IsHovered = i != CurrentSelection && SafezoneTools.IsCursorInArea(
                        new PointF(safe.X, safe.Y + y),
                        new SizeF(Width, Items[i].Height));

                    if (Items[i].IsHovered && IsControlJustPressed(Control.Attack))
                    {
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                        CurrentSelection = i;
                    }

                    Items[i].IsSelected = i == CurrentSelection;
                    Items[i].Draw(safe.X, safe.Y + y, Width);
                    y += Items[i].Height;
                }

                if (Items.Count > MaxOnScreenItems)
                {
                    y += ScrollIndicatorMargin;
                    new UIRectangle(new PointF(safe.X, safe.Y + y), new SizeF(Width, 40), Color.FromArgb(200, 0, 0, 0)).Draw();
                    new UISprite(new TextureAsset("commonmenu", "shop_arrows_upanddown"), new PointF(safe.X + (Width / 2) - 30, safe.Y + y - 5), new SizeF(50f, 50f)).Draw();
                    y += 40;
                }

                if (SelectedItem != null)
                {
                    DrawDescription(SelectedItem.Description, safe.X, safe.Y + y);
                }

                
                if (IsControlJustPressed(Control.FrontendDown) || IsControlJustPressed(Control.PhoneScrollForward))
                {
                    GoDown();
                }
                else if (IsControlJustPressed(Control.FrontendUp) || IsControlJustPressed(Control.PhoneScrollBackward))
                {
                    GoUp();
                }
            }
            else
            {
                DrawDescription("Appropriating the mom'n'pop aesthetic is all the rage right now, as is discretely storing your laundered revenue, and there's no property on the market right now catering to both needs better than the Hands On Car Wash!~n~~n~Owning this business will also boost the value of Product sold from a Counterfeit Cash Factory.", safe.X, safe.Y + y);
            }

            if (IsControlJustPressed(Control.FrontendCancel))
            {
                if (PreviousMenu != null)
                {
                    Back();
                    Script.Wait(0);
                    PreviousMenu.CurrentSelection = 0;
                    PreviousMenu.Visible = true;
                }
                else
                {
                    if (CanCloseMenu)
                    {
                        Back();
                    }
                    else
                    {
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
            }
        }

        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void Back()
        {
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CANCEL", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            Visible = false;
        }
        /// <summary>
        /// Simulates the input of going down the menu.
        /// </summary>
        public void GoDown()
        {
            if (CurrentSelection < Items.Count - 1)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                CurrentSelection++;
            }
            else if (CurrentSelection == Items.Count - 1)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                CurrentSelection = 0;
            }
        }
        /// <summary>
        /// Simulates the input of going up the menu.
        /// </summary>
        public void GoUp()
        {
            if (CurrentSelection > 0)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                CurrentSelection--;
            }
            else if (CurrentSelection == 0)
            {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                CurrentSelection = Items.Count - 1;
            }
        }
        /// <summary>
        /// Adds the item into the menu if its not already present.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(UIMenuBaseItem item)
        {
            if (Items != null)
            {
                if (!Items.Contains(item))
                {
                    item.Parent = this;
                    Items.Add(item);
                }
            }
        }
        /// <summary>
        /// Similar to <see cref="AddItem(UIMenuBaseItem)"/> except this method adds multiple items instead of one.
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(params UIMenuBaseItem[] items)
        {
            if (items.Length > 0)
            {
                foreach (UIMenuBaseItem item in items)
                {
                    AddItem(item);
                }
            }
        }
        /// <summary>
        /// Remove's the item from the menu if its already present.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(UIMenuBaseItem item)
        {
            if (Items != null)
            {
                if (Items.Contains(item))
                {
                    item.Parent = null;
                    Items.Remove(item);
                }
            }
        }
        /// <summary>
        /// Similar to <see cref="RemoveItem(UIMenuBaseItem)"/> except this method removes multiple items instead of one.
        /// </summary>
        /// <param name="items"></param>
        public void RemoveItems(params UIMenuBaseItem[] items)
        {
            if (items.Length > 0)
            {
                foreach (UIMenuBaseItem item in items)
                {
                    RemoveItem(item);
                }
            }
        }
        /// <summary>
        /// Adds a parent panel to the menu.
        /// </summary>
        /// <param name="panel"></param>
        public void AddParentPanel(UIMenuParentPanel panel)
        {
            panel.Parent = this;
            ParentPanel = panel;
        }
        /// <summary>
        /// Remove's the menu's parent panel if there is one.
        /// </summary>
        public void RemoveParentPanel()
        {
            if (ParentPanel != null)
            {
                ParentPanel.Parent = null;
                ParentPanel = null;
            }
        }

        #endregion
    }
}
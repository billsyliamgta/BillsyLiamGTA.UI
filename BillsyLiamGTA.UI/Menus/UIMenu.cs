using System;
using System.Drawing;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;
using BillsyLiamGTA.UI.Scaleform;

namespace BillsyLiamGTA.UI.Menu
{
    /// <summary>
    /// A class for creating Rockstar like UI menus.
    /// </summary>
    public class UIMenu
    {
        #region Fields

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
                    InstructionalButtons?.Load();
                    MenuOpened?.Invoke(this, new UIMenuOpenedArgs(this));
                }
                else
                {
                    GlareEffectScaleform?.Dispose();
                    GlareEffectScaleform = null;
                    InstructionalButtons?.Dispose();
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
        /// The texture asset object of the banner texture.
        /// </summary>
        public TextureAsset BannerTexture { get; set; }
        /// <summary>
        /// The banner's color.
        /// </summary>
        public Color BannerColor { get; set; } = Color.FromArgb(255, 255, 255, 255);
        /// <summary>
        /// The menu's glare scaleform object.
        /// </summary>
        public MpMenuGlare GlareEffectScaleform;
        /// <summary>
        /// If the menu's glare effect is enabled or not.
        /// </summary>
        public bool GlareEffectEnabled { get; set; } = true;
        /// <summary>
        /// Whether the menu can be closed by the player or not.
        /// </summary>
        public bool CanCloseMenu { get; set; } = true;
        /// <summary>
        /// Size of the menu's banner.
        /// </summary>
        private Size BannerSize = new Size(435, 98);
        /// <summary>
        /// Size of the menu's header.
        /// </summary>
        private Size HeaderSize = new Size(435, 37);
        /// <summary>
        /// The width of the menu.
        /// </summary>
        private int _width { get; set; } = 435;
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
        /// The title of the menu.
        /// </summary>
        public string Title { get; set; } = "Title";
        /// <summary>
        /// The font of the menu's title.
        /// </summary>
        public UIText.eTextFonts TitleFont { get; set; } = UIText.eTextFonts.FONT_CONDENSED;
        /// <summary>
        /// The title's text color.
        /// </summary>
        public Color TitleColor { get; set; } = Color.White;
        /// <summary>
        /// The subtitle of the menu.
        /// </summary>
        public string Subtitle { get; set; } = "Subtitle";
        /// <summary>
        /// The subtitle text's color.
        /// </summary>
        public Color SubtitleColor { get; set; } = Color.White;
        /// <summary>
        /// The private current selection value of the menu.
        /// </summary>
        private int _currentSelection;
        /// <summary>
        /// Current selection of the menu. If you set this to a value greater than the number of items, it will throw an exception.
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
        /// The max amount of items that be drawn on screen.
        /// </summary>
        public int MaxOnScreenItems { get; set; } = 5;
        /// <summary>
        /// The list of controls that are disabled when the menu is visible.
        /// </summary>
        public List<InputControl> DisabledControls = new List<InputControl>()
        {
            InputControl.FrontendPause,
            InputControl.FrontendPauseAlternate,
            InputControl.Phone,
            InputControl.PhoneUp,
            InputControl.PhoneDown,
            InputControl.PhoneLeft,
            InputControl.PhoneRight,
            InputControl.Aim,
            InputControl.Attack,
            InputControl.Attack2,
            InputControl.VehicleAim,
            InputControl.VehicleAttack,
            InputControl.VehicleAttack2,
            InputControl.LookUpDown,
            InputControl.LookLeftRight,
            (InputControl)37,
            (InputControl)199,
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
        /// <summary>
        /// The object for handling instructional buttons.
        /// </summary>
        public InstructionalButtons InstructionalButtons { get; set; }

        #endregion

        #region Constructors

        public UIMenu(string title, string subtitle, params UIMenuBaseItem[] items)
        {
            Items = new List<UIMenuBaseItem>();
            AddItems(items);
            Title = title;
            Subtitle = subtitle;
            BannerTexture = new TextureAsset("commonmenu", "interaction_bgd");
            InstructionalButtons = new InstructionalButtons();
            InstructionalButtons.AddContainer(new InstructionalButtonContainer(InputControl.FrontendAccept, "Select"));
            InstructionalButtons.AddContainer(new InstructionalButtonContainer(InputControl.FrontendCancel, "Back"));
            MenuHandler.Add(this);
        }

        public UIMenu(string title, string subtitle, TextureAsset bannerTexture, params UIMenuBaseItem[] items)
        {
            Items = new List<UIMenuBaseItem>();
            AddItems(items);
            Title = title;
            Subtitle = subtitle;
            BannerTexture = bannerTexture;
            InstructionalButtons = new InstructionalButtons();
            InstructionalButtons.AddContainer(new InstructionalButtonContainer(InputControl.FrontendAccept, "Select"));
            InstructionalButtons.AddContainer(new InstructionalButtonContainer(InputControl.FrontendCancel, "Back"));
            MenuHandler.Add(this);
        }

        #endregion

        #region Methods
        /// <summary>
        /// A method for drawing the current item's description if the string is not null or empty.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DrawDescription(string text, float x, float y)
        {
            y += DescriptionMargin;
            if (!string.IsNullOrEmpty(text))
            {
                UIText descriptionText = new UIText(text, new PointF(x + 10f, y + 5), 0.345f, Color.White, UIText.eTextFonts.FONT_STANDARD, UIText.eTextAlignments.Left);
                descriptionText.Wrap = Width - 10;
                UISprite descriptionBg = new UISprite(new TextureAsset("commonmenu", "gradient_bgd"), new PointF(x - 2f, y), new SizeF(Width, (descriptionText.LineCount * (descriptionText.LineHeight + 5)) + (descriptionText.LineCount - 1) + 10), Color.FromArgb(155, 255, 255, 255));
                descriptionBg.Draw();
                descriptionText.Draw();
            }
        }
        /// <summary>
        /// Draw's the menu. This isn't needed by the developer, because <see cref="MenuHandler"/> will handle the drawing.
        /// </summary>
        public void Draw()
        {
            if (!Visible) return;

            if (Screen.IsHelpTextDisplayed)
            {
                Screen.ClearHelpText();
            }

            if (DisabledControls?.Count > 0)
            {
                for (int i = 0; i < DisabledControls.Count; i++)
                {
                    Input.DisableControlThisFrame(DisabledControls[i]);
                    Function.Call(Hash.HUD_SUPPRESS_WEAPON_WHEEL_RESULTS_THIS_FRAME);
                }
            }

            if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE))
            {
                Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
            }

            InstructionalButtons?.Draw();

            PointF safe = SafezoneTools.SafezoneBounds;
            if (BannerEnabled)
            {
                new UISprite(BannerTexture, new PointF(safe.X - 2f, safe.Y), BannerSize, BannerColor).Draw();
                new UIText(Title, new PointF(safe.X + 10, safe.Y + BannerSize.Height / 5), 0.9f, TitleColor, TitleFont, UIText.eTextAlignments.Left).Draw();
                if (GlareEffectEnabled && GlareEffectScaleform != null)
                {
                    GlareEffectScaleform.Draw();
                }
            }
            
            new UIRectangle(new PointF(safe.X - 2f, safe.Y + (BannerEnabled ? BannerSize.Height : 0)), new SizeF(BannerSize.Width, 37), Color.Black).Draw();
            new UIText(Subtitle.ToUpper(), new PointF(safe.X + 10, safe.Y + (BannerEnabled ? BannerSize.Height : 0) + 5), 0.345f, SubtitleColor, UIText.eTextFonts.FONT_STANDARD, UIText.eTextAlignments.Left).Draw();
            new UIText($"{CurrentSelection + 1}/{Items?.Count}", new PointF(safe.X + Width - 10, safe.Y + (BannerEnabled ? BannerSize.Height : 0) + 5), 0.345f, Color.White, UIText.eTextFonts.FONT_STANDARD, UIText.eTextAlignments.Right).Draw();
            float y = CalculatedHeight;
            if (ParentPanel != null)
            {
                ParentPanel.Draw(y);
                y += 235;
            }
            if (Items?.Count > 0)
            {
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
                        new PointF(safe.X - 2f, safe.Y + y),
                        new SizeF(Width, Items[i].Height));

                    if (Items[i].IsHovered && Input.IsControlJustPressed(InputControl.Attack))
                    {
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                        CurrentSelection = i;
                    }

                    Items[i].IsSelected = i == CurrentSelection;

                    Items[i].Draw(safe.X - 2f, safe.Y + y, Width);
                    y += Items[i].Height;
                }

                if (Items.Count > MaxOnScreenItems)
                {
                    y += ScrollIndicatorMargin;
                    new UIRectangle(new PointF(safe.X - 2f, safe.Y + y), new SizeF(Width, 40), Color.FromArgb(200, 0, 0, 0)).Draw();
                    new UISprite(new TextureAsset("commonmenu", "shop_arrows_upanddown"), new PointF(safe.X + (Width / 2) - 30, safe.Y + y - 5), new SizeF(50f, 50f)).Draw();
                    y += 40;
                }

                if (SelectedItem != null)
                {
                    DrawDescription(SelectedItem.Description, safe.X, safe.Y + y);
                }
            }
            else
            {
                DrawDescription("Are you feeling a bit silly? This is an empty UIMenu, no items have been added.", safe.X, safe.Y + y);
            }
            if (Input.IsControlJustPressed(InputControl.FrontendCancel))
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
            else if (Input.IsControlJustPressed(InputControl.FrontendDown) || Input.IsControlJustPressed(InputControl.PhoneScrollForward))
            {
                GoDown();
            }
            else if (Input.IsControlJustPressed(InputControl.FrontendUp) || Input.IsControlJustPressed(InputControl.PhoneScrollBackward))
            {
                GoUp();
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
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            if (CurrentSelection < Items.Count - 1)
            {
                CurrentSelection++;
            }
            else if (CurrentSelection == Items.Count - 1)
            {
                CurrentSelection = 0;
            }
        }
        /// <summary>
        /// Simulates the input of going up the menu.
        /// </summary>
        public void GoUp()
        {
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            if (CurrentSelection > 0)
            {
                CurrentSelection--;
            }
            else if (CurrentSelection == 0)
            {
                CurrentSelection = Items.Count - 1;
            }
        }
        /// <summary>
        /// Add's the item into the menu if its not already present.
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
        /// Add's a parent panel to the menu.
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
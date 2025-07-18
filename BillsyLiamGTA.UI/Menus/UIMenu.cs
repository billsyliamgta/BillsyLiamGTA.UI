using System;
using System.Drawing;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using static GTA.Game;
using BillsyLiamGTA.UI.Elements;
using BillsyLiamGTA.UI.Scaleform;
using BillsyLiamGTA.UI.Menus;
using System.Linq;

namespace BillsyLiamGTA.UI.Menu
{
    /// <summary>
    /// A class for creating Rockstar like UI menus.
    /// </summary>
    public class UIMenu
    {
        #region Properties

        /// <summary>
        /// When the menu was opened in game time.
        /// </summary>
        public int MenuOpenedGameTime { get; private set; } = 0;
        /// <summary>
        /// When the menu was closed in game time.
        /// </summary>
        public int MenuClosedGameTime { get; private set; } = 0;
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
                    if (GlareEffectEnabled)
                    {
                        GlareEffectScaleform?.Load();
                    }
                    BannerTexture?.Load();
                    MenuOpenedGameTime = GameTime;
                    MenuOpened?.Invoke(this, new UIMenuOpenedArgs(this));
                }
                else
                {
                    GlareEffectScaleform?.Dispose();
                    BannerTexture?.Dispose();
                    MenuClosedGameTime = GameTime;
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
        public Texture BannerTexture { get; set; }
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
        /// Whether the menu controls are disabled or not.
        /// </summary>
        public bool DisableControls { get; set; } = false;
        /// <summary>
        /// Size of the menu's banner.
        /// </summary>
        private SizeF BannerSize = new SizeF(432f, 97.2f);
        /// <summary>
        /// Size of the menu's header.
        /// </summary>
        private SizeF HeaderSize = new SizeF(432f, 35.88f);
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
        public float CalculatedHeight
        {
            get
            {
                float height = HeaderSize.Height;
                if (BannerEnabled)
                {
                    height += BannerSize.Height;
                }
                return height;
            }
        }

        private int StartIndex;

        private int EndIndex;

        /// <summary>
        /// The gap between the menu and the scroll indicator.
        /// </summary>
        public int ScrollIndicatorMargin { get; set; } = 3;
        /// <summary>
        /// The gap between the menu and the description.
        /// </summary>
        public int DescriptionMargin { get; set; } = 5;

        public int PanelMargin { get; set; } = 3;
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
                    throw new ArgumentOutOfRangeException(nameof(value), "ERROR: Current Selection is greater than the total item count.");
                }

                _currentSelection = value;
            }
        }
        /// <summary>
        /// The max amount of items that be drawn on screen from this menu.
        /// </summary>
        public int MaxOnScreenItems { get; set; } = 7;
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

        private List<UIMenuBasePanel> Panels { get; set; }

        /// <summary>
        /// Return's the currently selected item in the menu.
        /// </summary>
        public UIMenuBaseItem SelectedItem
        {
            get
            {
                if (Items != null)
                {  
                    if (Items.Count > 0)
                    {
                        return Items[CurrentSelection];
                    }
                }

                return null;
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
            Panels = new List<UIMenuBasePanel>();
            AddItems(items);
            Title = title;
            Subtitle = subtitle;
            GlareEffectScaleform = new MpMenuGlare();
            BannerTexture = new Texture("commonmenu", "interaction_bgd");
            MenuHandler.Add(this);
        }

        public UIMenu(string title, string subtitle, Texture bannerTexture, params UIMenuBaseItem[] items)
        {
            Items = new List<UIMenuBaseItem>();
            Panels = new List<UIMenuBasePanel>();
            AddItems(items);
            Title = title;
            Subtitle = subtitle;
            GlareEffectScaleform = new MpMenuGlare();
            BannerTexture = bannerTexture;
            MenuHandler.Add(this);
        }

        #endregion

        #region Functions

        private void UpdateIndexes()
        {
            int totalItems = Items.Count;
            int maxVisible = MaxOnScreenItems;

            // Scroll forward by block if selection passes current window.
            if (CurrentSelection > EndIndex)
            {
                StartIndex = CurrentSelection - (maxVisible - 1);
                StartIndex = Math.Min(StartIndex, totalItems - maxVisible);
            }
            // Scroll up just enough to keep selection visible.
            else if (CurrentSelection < StartIndex)
            {
                StartIndex = CurrentSelection;
                StartIndex = Math.Max(0, StartIndex);
            }

            EndIndex = Math.Min(StartIndex + maxVisible - 1, totalItems - 1);
        }

        int GetVisibleItemCount()
        {
            if (Items.Count == 0 || StartIndex > EndIndex)
                return 0;

            return EndIndex - StartIndex + 1;
        }

        /// <summary>
        /// Handles the menu controls.
        /// </summary>
        public void Controls()
        {
            if (!DisableControls)
            {
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

                if (IsControlJustPressed(Control.FrontendDown) || IsControlJustPressed(Control.PhoneScrollForward))
                {
                    GoDown();
                }
                else if (IsControlJustPressed(Control.FrontendUp) || IsControlJustPressed(Control.PhoneScrollBackward))
                {
                    GoUp();
                }
                else if (IsControlJustPressed(Control.FrontendCancel) || IsControlJustPressed(Control.PhoneCancel))
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
        }

        public unsafe void Draw()
        {
            if (!Visible) return;

            Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 10 /*HELP_TEXT*/);

            if (DisabledControls?.Count > 0)
            {
                for (int i = 0; i < DisabledControls.Count; i++)
                {
                    DisableControlThisFrame(DisabledControls[i]);
                    Function.Call(Hash.HUD_SUPPRESS_WEAPON_WHEEL_RESULTS_THIS_FRAME);
                }
            }

            PointF safe = SafezoneTools.SafezoneBounds;
            if (BannerEnabled)
            {
                new UISprite(BannerTexture, new PointF(safe.X, safe.Y), BannerSize, BannerColour).Draw();
                new UIText(Title, new PointF(safe.X + 10, safe.Y + (BannerSize.Height * 0.25f) - (0.75f * 0.25f)), 0.75f, TitleColour, TitleFont, UIText.eAlignments.Left).Draw();
                if (GlareEffectEnabled)
                {
                    GlareEffectScaleform?.Draw();
                }
            }

            float y = BannerEnabled ? BannerSize.Height : 0;
            new UIRectangle(new PointF(safe.X, safe.Y + y), HeaderSize, Color.Black).Draw();
            new UIText(Subtitle.ToUpper(), new PointF(safe.X + 10, safe.Y + y + 4), 0.345f, SubtitleColour, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Left).Draw();
            new UIText($"{CurrentSelection + 1} / {Items?.Count}", new PointF(safe.X + Width - 10, safe.Y + y + 5), 0.345f, Color.White, UIText.eFonts.FONT_STANDARD, UIText.eAlignments.Right).Draw();
            y += HeaderSize.Height;

            DrawPanelsFromOrder(UIMenuBasePanel.eDrawOrder.BelowHeader, safe, &y);

            if (Items?.Count > 0)
            {
                UpdateIndexes();

                new UISprite(new Texture("commonmenu", "gradient_bgd"), new PointF(safe.X, safe.Y + y), new SizeF(Width, 35.88f * GetVisibleItemCount()), Color.FromArgb(200, 255, 255, 255)).Draw();

                for (int i = StartIndex; i <= EndIndex; i++)
                {
                    Items[i].IsHovered = i != CurrentSelection && SafezoneTools.IsCursorInArea(
                        new PointF(safe.X, safe.Y + y),
                        new SizeF(Width, Items[i].Height));

                    if (Items[i].IsHovered && IsControlJustPressed(Control.CursorAccept))
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
                    new UISprite(
                        new Texture("commonmenu", "shop_arrows_upanddown"),
                        new PointF(safe.X + (Width * 0.5f) - 25f, safe.Y + y - 5),
                        new SizeF(50f, 50f)
                    ).Draw();
                    y += 40;
                }
            }

            DrawPanelsFromOrder(UIMenuBasePanel.eDrawOrder.BelowItems, safe, &y);

            if (SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(SelectedItem.Description))
                {
                    y += PanelMargin;
                    new UIMenuDescriptionPanel(SelectedItem.Description).Draw(safe.X, safe.Y + y, Width);
                    y += PanelMargin;
                }
            }

            Controls();
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
        /// Whether or not the menu has any items in it.
        /// </summary>
        /// <returns></returns>
        public bool HasItems()
        {
            if (Items != null)
            {
                if (Items.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Adds the panel into the menu if its not already present.
        /// </summary>
        /// <param name="item"></param>
        public void AddPanel(UIMenuBasePanel panel)
        {
            if (Panels != null)
            {
                if (!Panels.Contains(panel))
                {
                    Panels.Add(panel);
                }
            }
        }
        /// <summary>
        /// Remove's the panel from the menu if its already present.
        /// </summary>
        /// <param name="item"></param>
        public void RemovePanel(UIMenuBasePanel panel)
        {
            if (Panels != null)
            {
                if (Panels.Contains(panel))
                {
                    Panels.Remove(panel);
                }
            }
        }

        private unsafe void DrawPanelsFromOrder(UIMenuBasePanel.eDrawOrder order, PointF safe, float* y)
        {
            if (Panels?.Count > 0)
            {
                foreach (UIMenuDescriptionPanel panel in Panels)
                {
                    if (panel.DrawOrder == order)
                    {
                        *y += PanelMargin;
                        panel.Draw(safe.X, safe.Y + *y, Width);
                        *y += panel.Height;
                    }
                }
            }
        }

        #endregion
    }
}
# UIMenu

## Creating a Menu with a Banner

```
Menu = new UIMenu("Menu Example", "Meow Meow Meow");
NormalItem = new UIMenuNormalItem("Normal Item", "");
NormalColouredItem = new UIMenuNormalItem("Pretty colours", "");
NormalColouredItem.DefaultTabColor = Color.FromArgb(155, 203, 54, 148);
NormalColouredItem2 = new UIMenuNormalItem("More pretty colours", "");
NormalColouredItem2.DefaultTabColor = Color.FromArgb(255, 133, 85);
CheckboxItem = new UIMenuCheckboxItem("Tick? Sure thing!", "");
ListItem = new UIMenuListItem<string>("List of cool things", "On each item you can set specific description text! Veddy nice.", new System.Collections.Generic.List<string>() { "Chicken burger", "Pasta" });
SliderItem = new UIMenuSliderItem("Slider Item", "");
Menu.AddItem(NormalItem);
Menu.AddItem(NormalColouredItem);
Menu.AddItem(NormalColouredItem2);
Menu.AddItem(CheckboxItem);
Menu.AddItem(ListItem);
Menu.AddItem(SliderItem);
Menu.AddParentPanel(new UIMenuParentPanel());
Menu.Visible = true;

```
## Banner Texture

The banner's texture can be changed like this: ```Menu.BannerTexture = new TextureAsset("texture_dict", "texture_name");```

## No Banner

You can toggle the menu's banner by using ```Menu.BannerEnabled``` boolean.

## Scrolling

The menu will turn into a "scroll menu" if the item count is greater than ```MaxOnScreenItems``` (by default this is 5).


# Items

## Activated

When any item is pressed by tbe player the Activated event invokes. See the example below to implement, your own functions. ⚠️ This only needs to be called ONCE!

```
Item.Activated += (sender, e) =>
{
    // Your code functions here
};
```

## IsHovered

Use the ```Item.IsHovered``` boolean to check if the cursor (if active) is currently hovering over the item.

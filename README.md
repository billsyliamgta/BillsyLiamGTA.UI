# A UI library for Grand Theft Auto V
![](https://raw.githubusercontent.com/billsyliamgta/BillsyLiamGTA.UI/refs/heads/main/ui_example.JPG)
Lightweight, fast and realiable menus with dynamic items, instructional buttons, events and full mouse support.

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
// do not do this in a loop as it will keep loading the scaleform(s) etc.
Menu.Visible = true;

```
## Banner Texture

The banner's texture can be changed like this: ```Menu.BannerTexture = new TextureAsset("texture_dict", "texture_name");```

## No Banner

You can toggle the menu's banner by using ```Menu.BannerEnabled``` boolean.

## Scrolling

The menu will turn into a "scroll menu" if the item count is greater than ```MaxOnScreenItems``` (by default this is 5).


# All Items

## Activated

When any item is pressed by the player the Activated event invokes. See the example below to implement, your own functions. ⚠️ This only needs to be called ONCE!

```
Item.Activated += (sender, e) =>
{
    // Your code functions here
    Function.Call(Hash.BEGIN_TEXT_COMMAND_PRINT, "STRING");
    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Item activated!");
    Function.Call(Hash.END_TEXT_COMMAND_PRINT, 2000, true);
};
```

## IsHovered

Use the ```Item.IsHovered``` boolean to check if the cursor (if active) is currently hovering over the item.

# UIMenuCheckboxItem

A tick box item. You can whether its ticked or not - and even set, by using the ```CheckboxItem.IsChecked``` boolean.

# UIMenuListItem<T>

Makes a list item containing anything you like strings, int's etc. Use ```ListItem.Items``` to add and remove objects.

```ListItem.Index``` gets/sets the current index being displayed.

```ListItem.CurrentValue``` returns the current index's value as the desired object.

# UIMenuSliderItem

The value of the slider ranges between 0.0 and 1.0 (float), use  ```SliderItem.Value```.

When the slider's value is changed the ValueChanged event invokes. See the example below to implement, your own functions. ⚠️ This only needs to be called ONCE!

```
SliderItem.ValueChanged += (sender, e) =>
{
    // Your code functions here
    Function.Call(Hash.BEGIN_TEXT_COMMAND_PRINT, "STRING");
    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Value changed: " + e.Value);
    Function.Call(Hash.END_TEXT_COMMAND_PRINT, 2000, true);
};
```

Gender icons can be toggled like the GTA Online character creator heritage menu, by ```SliderItem.GenderIconsEnabled``` boolean.

# Timerbars
![](https://raw.githubusercontent.com/billsyliamgta/BillsyLiamGTA.UI/refs/heads/main/timerbars.png)

Example:
```
TextTimerbar = new TextTimerbar("TEXT", false);
ProgressTimerbar = new ProgressTimerbar("PROGRESS", 0.1f);
CountdownTimerbar = new CountdownTimerbar("TIME REMAINING", 120000);
List<CheckpointTimerbar.Checkpoint> checkpoints = new List<CheckpointTimerbar.Checkpoint>();
for (int i = 0; i < 5; i++)
{
    checkpoints.Add(new CheckpointTimerbar.Checkpoint(Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 114, 204, 114), Color.FromArgb(255, 224, 50, 50), i < 2 ? CheckpointTimerbar.CheckpointState.InProgress : CheckpointTimerbar.CheckpointState.Default));
}
CheckpointTimerbar = new CheckpointTimerbar("CHECKPOINTS", checkpoints);
```

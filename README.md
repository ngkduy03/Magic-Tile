# **Case study project**

### **About:**

This is a case study project, which is a piano tile game. Because this project is meant to showcase the game's gameplay mechanics, the UI of the menu scene and other setting features have not been developed fully. The UI/UX is minimal and enough to showcase the main purpose. Please keep it in mind when experiencing the project

## **Gameplay mechanic:**

When the player presses start, the song plays, the backgorund and the **end line** play its animation,  and the tiles drop down, which syncs to the rhythm.

There are 2 types of tile, the **tap tile** and the **press tile**.

**tap tile** - tap the tile and the tile will fade and score points.

**press tile** - press and hold the tile until the overlay of the tile is full to score an extra point.

If any tile reaches all of its body to the ***end line***, the game stops and is over.

## **Score system:**

When scoring the tile, there will be an animation of the tile and the grade point.

**Cool** - get 1 point if you score the tile outside the **end line**.

**Great** - get 2 points  if you score the tile inside the **end line**.

**Perfect** - get 3 points  if you score the tile just reach right on the **end line** with a narrow window.

When score **Perfect**, it will genrate a **xCombo** which is multiplied by the ***Perfect*** grade point to score.

For each consecutive **Perfect** you get, the amount of **xCombo** you gain, which means the score will be bigger every time you gain more **xCombo**.

You lose all the **xCombo** if you break the **Perfect** chain.

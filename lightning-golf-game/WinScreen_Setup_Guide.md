# Win Screen Setup Guide

This guide will help you set up the win screen UI for all levels in your mini-golf game.

## Implementation Status

✅ **Completed:**
- WinScreenUI.cs script created with full functionality
- LevelManager.cs modified to support win screen mode
- Basic prefab structure created

🔄 **To Complete in Unity Editor:**
- Create proper UI elements in the prefab
- Add UI components to each level scene
- Configure button references and settings

## Step 1: Create the Win Screen UI Prefab

1. **Open Unity Editor**

2. **Create a new Canvas in the Hierarchy:**
   - Right-click in Hierarchy → UI → Canvas
   - Name it "WinScreenCanvas"
   - Set Canvas Scaler to "Scale With Screen Size" (Reference Resolution: 1920x1080)

3. **Create the main panel:**
   - Right-click on WinScreenCanvas → UI → Panel
   - Name it "WinScreenPanel"
   - Set color to black with alpha 200 (semi-transparent background)
   - Set anchors to fill entire screen (Anchor Min: 0,0 Anchor Max: 1,1)

4. **Add UI elements to WinScreenPanel:**

   **Title Text:**
   - Right-click WinScreenPanel → UI → Text - TextMeshPro
   - Name it "LevelCompleteText"
   - Text: "Level Complete!"
   - Font Size: 48, Color: White, Bold
   - Anchors: Center-Top, Position: (0, -100)

   **Shots Text:**
   - Right-click WinScreenPanel → UI → Text - TextMeshPro
   - Name it "ShotsText"
   - Text: "Shots: 0"
   - Font Size: 32, Color: White
   - Anchors: Center, Position: (0, 50)

   **Star Rating Text:**
   - Right-click WinScreenPanel → UI → Text - TextMeshPro
   - Name it "StarRatingText"
   - Text: "★★★"
   - Font Size: 40, Color: Yellow
   - Anchors: Center, Position: (0, 0)

   **Main Menu Button:**
   - Right-click WinScreenPanel → UI → Button - TextMeshPro
   - Name it "MainMenuButton"
   - Button text: "Main Menu"
   - Anchors: Center-Bottom, Position: (-120, 100)
   - Size: (200, 50)

   **Next Level Button:**
   - Right-click WinScreenPanel → UI → Button - TextMeshPro
   - Name it "NextLevelButton"
   - Button text: "Next Level"
   - Anchors: Center-Bottom, Position: (120, 100)
   - Size: (200, 50)

5. **Add the WinScreenUI script:**
   - Select the WinScreenCanvas GameObject
   - Add Component → Scripts → WinScreenUI
   - Configure the script references:
     - Win Screen Panel: WinScreenPanel
     - Level Complete Text: LevelCompleteText
     - Shots Text: ShotsText
     - Star Rating Text: StarRatingText
     - Main Menu Button: MainMenuButton
     - Next Level Button: NextLevelButton

6. **Save as Prefab:**
   - Drag WinScreenCanvas from Hierarchy to Assets/Prefabs folder
   - Name it "WinScreenUI"
   - Delete the original from Hierarchy

## Step 2: Add Win Screen to Each Level

**For Level1:**
1. Open the Level1 scene
2. Drag the WinScreenUI prefab into the Hierarchy
3. Find the LevelManager GameObject
4. In the LevelManager component:
   - Set "Use Win Screen" to true
   - Set "Auto Load Next Level" to false
5. Set WinScreenPanel to inactive by default (uncheck in Inspector)

**For Level2 and Level3:**
- Repeat the same process for each level scene

## Step 3: Configure Level Manager Settings

In each level's LevelManager component:
- **Level Complete Delay**: 1f (1 second delay before showing win screen)
- **Use Win Screen**: true
- **Auto Load Next Level**: false
- **Next Level Name**: Leave empty (auto-detected)

## Step 4: Test the Implementation

1. **Play Level1**
2. **Complete the level** (get ball in hole)
3. **Verify win screen appears** with:
   - Level name
   - Shot count
   - Star rating (3 stars ≤3 shots, 2 stars ≤5 shots, 1 star ≤8 shots)
   - Working buttons

4. **Test navigation:**
   - "Main Menu" should return to MainMenu scene
   - "Next Level" should go to Level2

## Troubleshooting

**Win screen doesn't appear:**
- Check that LevelManager has "Use Win Screen" enabled
- Verify WinScreenUI script is attached to the Canvas
- Ensure all UI references are properly assigned

**Buttons don't work:**
- Check button OnClick events are properly configured
- Verify scene names match in build settings

**Shots count shows wrong number:**
- Ensure ShotsCounter is properly incrementing in your ball controller
- Check that ShotsCounter.Instance is accessible

## Static Data Implementation

Currently implemented with static/stub data:
- Star rating based on simple shot count thresholds
- No data persistence between sessions
- Fallback to showing "5 shots" if ShotsCounter is unavailable

This provides a functional win screen that can be enhanced later with full stats tracking and persistence.
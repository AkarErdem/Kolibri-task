
# Kolibri Take Home Task

The project was developed utilizing Unity `2021.3.13f1` version.

The given task required the implementation of the following 4 requirements:

* It’s possible to start a second mine.
* It’s possible to switch back and forth between both mines.
* When switching to a mine, it must be possible to continue from the last state.
    * The same amount of “money” is still available as before in the mine.
    * The amount of mine shafts unlocked is still the same as before.
    * The upgrade levels are still the same as before.
* The button and panel are implemented according to the mockup including
animations.

To meet these requirements, I created the following systems:
* Save system that supports multiple mines.
* UI system which contains buttons and panels adaptable. It also has the animations from the mockup as well.

## UI System
I started by implementing the UI system. I adapted the given UI naming and expanded it more. For the UI, I used a naming convention of `type_name` where "type" refers to the type of UI element, such as "page", "panel", "btn", "img", "txt", or "scroll".

1- `page`: The page contains most of the UI elements inside of it. It covers and blocks the whole UI and it always needs to stretch to all Canvas.

2- `panel`: It is similar to the page but it can not block the whole canvas. It covers some parts of the UI. Also, every panel needs to have a page root at the end.

3- `btn`: UI element that has a Button component on it.

4- `img`: UI element that has an Image component on it that is specifically used.

5- `txt`: UI element with TextMeshPro component.

6- `scroll`: UI element that has ScrollRect component on it.

Some examples for the naming: `txt_cash`, `btn_map`, `page_mine_selection`, `panel_header`

UI was broken in different resolutions so I wanted to fix it first. I adapted the given UI and made it scalable to any resolution. 

I edited Hud prefab, added a layout group, and made it expandable so that it will match any resolution. In the given mockup, the given device was an iPhone 6s and it has an aspect ratio of 16:9. I set my editor settings to the same for a better approach. 

I broke down the HUD into three panels (top, bottom, and middle) to easily set the canvas element's anchors. For example money and map icons always need to be on the top and tutorial info needs to be at the bottom. And middle contains elements that need to stay at the center such as pages.

For the Mine Selection Panel, I created the Map Button first and then added its functionality using UniRX on the HudController. Then I created a scroll that would hold the mines as elements, allowing me to create them dynamically. I used DOTween for the UI animations instead of the Animator, since it can cause the canvas to regenerate every frame. I also disabled Raycast Target from images and texts that were not needed for optimization purposes.

For the camera, I created a class called CameraOffset that contains 2 Vector2s for the Up and Down limits of the camera.

## Save System
For the save system, I wanted to adapt to our scheme. So I created a save system which will is injected at the Initializer. This way we can use our save data in the starting phase.

I have created a folder under the _Scripts called DataPersistence which contains all the save-related events.

In this folder, we have the scripts below:
* GameData: Contains all the saveable game data such as mines, finance, and so on.
* IFileHandler: Writes and reads data from a file. Has the methods of `Save` and `Load`.
* ISaveModel: Has functions `SaveGame` and `LoadGame`. We can save the game from ISaveModel. 
* ISaveable: Has the `OnSave` method which will be called during the saving process.
* DataPersistenceController: Implements ways to save data using `ISaveModel` and UniRX. For example: saves the game on application quit using UniRX. Also loads the game save on the constructor.

### Game Initializer
On the GameInitializer, I have added our newly implemented scripts and injected save data to the required scripts. This way I was able to save everything! I saved workers' positions, stashes, upgrades, and player money and last entered mine. 

### Scene Loader
A custom interface, ISceneLoader, was created to manage scene loading exclusively. This interface includes methods for loading and reloading scenes, as well as an `IsLoading` reactive property. The loading screen is activated through the use of UniRX on the HudController whenever the `IsLoading` property triggers.

### Game Config
To facilitate mine-specific configurations, variables were organized into a class called `MineConfig` within the Game Config. Default starting values, such as 500 gold, were added to provide players with a consistent starting experience. Additionally, an XOR (Exclusive Or) encryption-decryption system was implemented to secure JSON files, with the option to enable or disable this feature through the Game Config.

The Game Config also includes two useful buttons: `SetMineNames`, which sets all mine names based on their index, and `ShowDataDirectoryPath`, which opens the saved data location for easy access.

## Conclusion
I hope this solution meets the requirements for the task and demonstrates my understanding of game development and programming concepts. If you have any questions or feedback, please let me know.

Erdem Akar

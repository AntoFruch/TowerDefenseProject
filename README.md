# Tower Defense Project

This is the result of the final project for CSC4253 courses at Télécom SudParis.
TowerDefense is tower defense type game with cartoon style, it has been made for educational purposes which are detailed here : https://enseignements.telecom-sudparis.eu/fiche.php?c=CSC4253;

## Play the Game
1. Download the final build _TowerDefenseProject-Build.zip_ provided in the root folder.
2. Unzip it and run TowerDefense.exe
3. In order to play the game, you will have to upload maps in the Maps folder located in the persistantDataPath :
    * **Windows** : AppData/LocalLow/AntoJuAlexProductions/TowerDefense/Maps
    * **MacOS** : Library/Application Support/AntoJuAlexProductions/TowerDefense/Maps
4. In game, You can click on the "refresh" button in the map selection screen to refresh what maps are in the Maps folder.
5. Enjoy !

I only provided the Windows build here, but it is possible to build the game for the OS / platform you'll like by cloning the repo, opening it in Unity, and rebuild it.  

## Maps
The maps files are images, whatever the size.<br>  Here is an exemple :<br> 
 <div style="text-align: center;">
    <img src="Maps/map_03.png"
         alt="MapExemple"
         width="300"
         style="image-rendering: pixelated; image-rendering: crisp-edges;">
</div>

#### How to make your own map :
You will have to draw pixels according to the color code
| Color | RGB             | Type                 |
|--------|-----------------|----------------------|
| <div style="width:20px;height:20px;background:#E5E5E5;border:1px solid #000;"></div> | (229, 229, 229) | Non-buildable zone |
| <div style="width:20px;height:20px;background:#FFE97F;border:1px solid #000;"></div> | (255, 233, 127) | Path |
| <div style="width:20px;height:20px;background:#FFB27F;border:1px solid #000;"></div> | (255, 178, 127) | Intersection |
| <div style="width:20px;height:20px;background:#00FF21;border:1px solid #000;"></div> | (0, 255, 33)    | Spawns |
| <div style="width:20px;height:20px;background:#FF0000;border:1px solid #000;"></div> | (255, 0, 0)     | Destination |
| <div style="width:20px;height:20px;background:#FFFFFF;border:1px solid #000;"></div> | Other           | Buildable zone |
    
The layout must also folow those rules :
* There must at least 1 spawn and 1 destination.
* There must exist at least one valid path from every spawn to an end.
* Every turns, crossing, split must be indicated by an Intersection




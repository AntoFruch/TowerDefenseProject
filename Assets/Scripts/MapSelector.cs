using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using JetBrains.Annotations;
public class MapSelector : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button map1;
    private Button map2;
    private Button map3;

    public static TileType[][] selectedMap;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        map1 = uiDocument.rootVisualElement.Q<Button>("MAP1");
        map2 = uiDocument.rootVisualElement.Q<Button>("MAP2");
        map3 = uiDocument.rootVisualElement.Q<Button>("MAP3");


        map3.clicked += Map3OnClicked;
    }


    private void Map3OnClicked()
    {
        selectedMap = FileAPI.ImageToTileTypeArray(FileAPI.ReadImageAsTexture2D("../Maps/map_03_fix.png"));
        SceneManager.LoadScene("SampleScene");
    }
}

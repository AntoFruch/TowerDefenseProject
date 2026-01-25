using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    [Header("Tower Attributes")]
    [SerializeField] protected int range = 3;
    public int Range => range;
    Dictionary<Renderer,Material> originalMaterials ;

    [Header("Economy")]
    public BuildingType buildingType;
    [SerializeField] private float resaleRatio = 0.5f;
    
    protected virtual void Start()
    {
        Game.Instance.buildings.Add(this);
        originalMaterials = new Dictionary<Renderer, Material>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        
        foreach (Renderer renderer in renderers)
        {
            originalMaterials[renderer] = renderer.material;
        }
    }
    protected virtual void Update()
    {
    }
    void OnDestroy()
    {
        Game.Instance?.buildings.Remove(this);
    }
    public void RedHighlight()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (originalMaterials.ContainsKey(renderer))
            {
                renderer.material = Resources.Load<Material>("Materials/InvalidHighlight");
            }
            
        }
    }
    public void GreenHighlight()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (originalMaterials.ContainsKey(renderer))
            {
                renderer.material = Resources.Load<Material>("Materials/ValidHighlight");
            }
        }
    }
    public void Unhighlight()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (originalMaterials.ContainsKey(renderer))
            {
                renderer.material = originalMaterials[renderer];
            }
        }
    }

    public void SellBuilding()
    {
        if (MoneyManager.Instance != null) {
            int currentCost = MoneyManager.Instance.GetCost(this.buildingType);
            int refund = Mathf.FloorToInt(currentCost*resaleRatio);
            MoneyManager.Instance.AddMoney(refund);
        }

        if (Game.Instance.buildings.Contains(this))
        {
            Game.Instance.buildings.Remove(this);
        }

        Destroy(gameObject);

    }

    protected bool active = true;
    public void SetActive(bool b)
    {
        active = b;
    }
}

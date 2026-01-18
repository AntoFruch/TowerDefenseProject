using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    [Header("Tower Attributes")]
    [SerializeField] protected float range = 3f;
    public float realRange {get;protected set;}
    Dictionary<Renderer,Material> originalMaterials ;

    [Header("Economy")]
    public int cost = 100; 
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

        realRange = (2 * range + 1)/2;
        CreateRange();
    }
    protected virtual void Update()
    {
    }
    void OnDestroy()
    {
        Game.Instance?.buildings.Remove(this);
    }
    
    void CreateRange()
    {
        GameObject rangeCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        rangeCylinder.transform.SetParent(transform);
        rangeCylinder.transform.position = transform.position;
        rangeCylinder.transform.localScale = new Vector3(2*realRange,0.01f,2*realRange);
        Destroy(rangeCylinder.GetComponent<Collider>());
        if (this is Tower)
        {
            rangeCylinder.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Ranges/TowerRange");
        } 

        if(this is Installation)
        {
            rangeCylinder.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Ranges/BoostRange");
        }
        // add other building types here
        
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
            int resaleAmount = Mathf.FloorToInt(cost * resaleRatio);
            MoneyManager.Instance.AddMoney(resaleAmount);
        }

        Destroy(gameObject);

    }
}

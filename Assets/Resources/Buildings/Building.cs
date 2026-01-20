using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Building : MonoBehaviour {

    [Header("Tower Attributes")]
    [SerializeField] protected int range = 3;
    public int Range => range;
    Dictionary<Renderer,Material> originalMaterials ;
    
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
        foreach (GameObject go in RangesManager.Instance.ranges[this])
        {
            Destroy(go);
        }
        RangesManager.Instance.ranges.Remove(this);
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

    protected bool active;
    public void SetActive(bool b)
    {
        active = b;
    }
}
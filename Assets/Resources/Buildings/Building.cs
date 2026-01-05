using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

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
    
    void OnDestroy()
    {
        Game.Instance?.buildings.Remove(this);
    }

    public void RedHighlight()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = Resources.Load<Material>("Materials/InvalidHighlight");
        }
    }
    public void GreenHighlight()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            
            renderer.material = Resources.Load<Material>("Materials/ValidHighlight");
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
}
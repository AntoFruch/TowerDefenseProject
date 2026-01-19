using UnityEngine;

public class EnergyLink : MonoBehaviour
{
    private Transform nodeA;
    private Transform nodeB;

    private LineRenderer line;

    private Vector3 heightOffset;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        heightOffset = new Vector3(0,0.2f,0);
    }

    void Update()
    {
        if (nodeA != null && nodeB != null)
        {
            line.SetPosition(0, nodeA.position+heightOffset);
            line.SetPosition(1, nodeB.position+heightOffset);
        }
    }

    public void SetNodeA(Transform nodeA)
    {
        this.nodeA = nodeA;
    }

    public void SetNodeB(Transform nodeB)
    {
        this.nodeB = nodeB;
    }
}

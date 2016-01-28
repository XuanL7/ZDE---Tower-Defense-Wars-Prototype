using UnityEngine;
using System.Collections;

public class PathBlockedScript : MonoBehaviour {
    public Transform spawnPoint;
    public Transform endPoint;

    [HideInInspector]
    public bool blockedPath = false;

    void Update()
    {
        Pathfinding.GraphNode startNode = AstarPath.active.GetNearest(spawnPoint.transform.position, Pathfinding.NNConstraint.Default).node;
        Pathfinding.GraphNode endNode = AstarPath.active.GetNearest(endPoint.transform.position, Pathfinding.NNConstraint.Default).node;
        if (Pathfinding.PathUtilities.IsPathPossible(startNode, endNode))
        {
            blockedPath = true;
            Debug.Log("Valid Placement");
        }
        else
        {
            Debug.Log("Invalid Placement");
            blockedPath = false;
        }
    }
}

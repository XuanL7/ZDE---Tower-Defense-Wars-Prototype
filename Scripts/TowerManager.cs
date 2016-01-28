using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour {

	public Transform aiSpawnPoint;
	public Transform aiEndPoint;
	public bool lockToTile = false; // Sets tower placement to plane transform position
	public Button[] towerButtons; // holds buttons for tower placement

	[HideInInspector]
	public List<GameObject> playerTowers; // holds all player towers in scene
	[HideInInspector]
	public List<GameObject> aiTowers; // holds all computer towers in scene
	[HideInInspector]
	public bool hasPlaced = true; // check if currently placing object

	private GameObject tower; // for new tower placement
	private bool newTower = false; // if UI button success
	private bool validPlacement = false; // determines if tower placement possible
	private Vector3 placement; // xyz pos of ground surface to place tower
	private Ray ray; // to find location of tower placement
	private int layerMask = 1 << 9;

	private UpgradeManager upMan;

	/** PUBLIC UTILITY METHODS **/

	/// <summary>
	/// SetTower() used to instantiate new tower to place
	/// </summary>
	/// <param name="setTower"></param>
	public void SetTower(GameObject setTower)
	{
		if (setTower != null && hasPlaced && upMan.purchaseTower(setTower.GetComponent<TowerController>().cost))
		{
			tower = Instantiate(setTower) as GameObject;
			newTower = true;
			hasPlaced = false;
		}
	}

	/** PRIVATE UTILITY METHODS **/

	private bool PathBlocked()
	{
		Pathfinding.GraphNode startNode = AstarPath.active.GetNearest(aiSpawnPoint.transform.position, Pathfinding.NNConstraint.Default).node;
		Pathfinding.GraphNode endNode = AstarPath.active.GetNearest(aiEndPoint.transform.position, Pathfinding.NNConstraint.Default).node;
		if (Pathfinding.PathUtilities.IsPathPossible(startNode, endNode))
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public bool TowerOnTower(GameObject tower)
	{
		if (tower.GetComponentInChildren<TowerCollisionDetector>().badSpot)
			return true;
		return false;
	}

	/// <summary>
	/// change layer of object and children
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="newLayer"></param>
	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}

		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
			if (null == child)
			{
				continue;
			}
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	/** STANDARD UNITY METHODS **/

	void Awake()
	{
		if ((upMan = GameObject.FindGameObjectWithTag("Game Logic").GetComponent<UpgradeManager>()) == null)
			Debug.Log("Error: Failed to find component UpgradeManager on Game Logic in TowerManager!");

		foreach(Button button in towerButtons)
		{
			button.interactable = false;
		}
	}

	void Update () {
		if (newTower)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Input.GetMouseButtonDown(0) && validPlacement)
			{
				GameObject oldTower = tower;
				tower = null;


				int parentLayer = oldTower.layer;
				SetLayerRecursively(oldTower, 9); // set all children to "Ground"
				oldTower.layer = parentLayer; // Set parent to "Object"

				if (PathBlocked() || TowerOnTower(oldTower))
				{
					Debug.Log("BAD PLACEMENT");
					SetLayerRecursively(oldTower, 8);
					oldTower.layer = parentLayer;
					tower = oldTower;
					oldTower = null;
				}
				else
				{
					oldTower.GetComponent<TowerController>().SetLightColor(Color.blue);
					playerTowers.Add(oldTower);
					hasPlaced = true;
					newTower = false;
				}
			}
			else
			{
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				{
					if (hit.collider.tag == "Ground")
					{
						validPlacement = true;
						tower.GetComponent<TowerController>().SetLightColor(Color.green);
						if (!lockToTile)
						{
							tower.transform.position = hit.point;
						}
						else
						{
							tower.transform.position = hit.collider.transform.position;
						}
					}
					else
					{
						validPlacement = false;
						tower.GetComponent<TowerController>().SetLightColor(Color.red);
						tower.transform.position = hit.point;
					}
				}
			}
		}
	}
}

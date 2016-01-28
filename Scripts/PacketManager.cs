using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// Class to hold groups of same type packets
public class PacketGroup
{
	/**       CONSTRUCTORS           **/

	public PacketGroup() { _packets = new List<GameObject>(); }

	/**        PUBLIC MEMBERS        **/

	[HideInInspector]
	public GameObject _packetPrefab;
	[HideInInspector]
	public int sendingCount = 0; // number if packets set to send
	[HideInInspector]
	public bool packetsSent = false; // FLAG: True if packets from group sent spawn

	/**      PRIVATE MEMBERS          **/

	private List<GameObject> _packets;
	private string _ident = "";
	private string _packetType = "none";
	private int healthCount = 0;
	private int armorCount = 0;
	private int speedCount = 0;
	private int shieldCount = 0;
	private int shieldRateCount = 0;
	private int shieldDelayCount = 0;
	private int shieldChargeCount = 0;

	/**      GETTERS/STTER          S **/

	public string Ident
	{
		get { return _ident; }
		set { _ident = value; }
	}
	public string PacketType
	{
		get { return _packetType; }
		set { _packetType = value; }
	}
	public List<GameObject> Packets
	{
		get { return _packets; }
		set { _packets = value; }
	}
	public int PacketCount()
	{
		return _packets.Count;
	}

	/**  PUBLIC UTILITY METHODS       **/

	public PacketStats GetGroupStats()
	{
		return _packets[0].GetComponent<PacketController>().GetStats;
	}
	public int TotalUpgrades()
	{
		return healthCount + armorCount + speedCount + shieldCount + shieldRateCount + shieldDelayCount + shieldChargeCount;
	}
	public bool isHome()
	{
		foreach (GameObject packet in _packets)
		{
			if (packet.activeSelf)
			{
				return false;
			}
		}
		return true;
	}
	public float TotalHealthCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (healthCount < _packets[0].GetComponent<PacketController>().packMax.healthMax)
			return packet.Get_Upgrade_Health_Cost() * PacketCount();
		else
			return 0;
	}
	public float TotalArmorCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (armorCount < _packets[0].GetComponent<PacketController>().packMax.armorMax)
			return packet.Get_Upgrade_Armor_Cost() * PacketCount();
		else
			return 0;
	}
	public float TotalSpeedCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (speedCount < _packets[0].GetComponent<PacketController>().packMax.speedMax)
			return packet.Get_Upgrade_Speed_Cost() * PacketCount();
		else
			return 0;
	}
	public float TotalShieldCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (shieldCount < _packets[0].GetComponent<PacketController>().packMax.shieldMax)
			return packet.Get_Upgrade_Shields_Cost() * PacketCount();
		else
			return 0;
	}
	public float TotalShieldRateCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (shieldRateCount < _packets[0].GetComponent<PacketController>().packMax.shiedRateMax)
			return packet.Get_Upgrade_ShieldRate_Cost() * PacketCount();
		else
			return 0;
	}
	public float TotalShieldChargeCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (shieldChargeCount < _packets[0].GetComponent<PacketController>().packMax.shieldChargeMax)
			return packet.Get_Upgrade_ShieldCharge_Cost() * PacketCount();
		else
			return 0;
	}
	public float TotalShieldDelayCost()
	{
		PacketController packet = _packets[0].GetComponent<PacketController>();
		if (shieldDelayCount < _packets[0].GetComponent<PacketController>().packMax.shieldDelayMax)
			return packet.Get_Upgrade_ShieldDelay_Cost() * PacketCount();
		else
			return 0;
	}
	public void UpgradeHealth()
	{
		foreach(GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_Health();
		}
		++healthCount;
	}
	public void UpgradeArmor()
	{
		foreach (GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_Armor();
		}
		++armorCount;
	}
	public void UpgradeSpeed()
	{
		foreach (GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_Speed();
		}
		++speedCount;
	}
	public void UpgradeShields()
	{
		foreach (GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_Shields();
		}
		++shieldCount;
	}
	public void UpgradeShieldRate()
	{
		foreach (GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_ShieldRate();
		}
		++shieldRateCount;
	}
	public void UpgradeShieldCharge()
	{
		foreach (GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_ShieldCharge();
		}
		++shieldChargeCount;
	}
	public void UpgradeShieldDelay()
	{
		foreach (GameObject packet in _packets)
		{
			packet.GetComponent<PacketController>().Upgrade_ShieldDelay();
		}
		++shieldDelayCount;
	}
	public float AddPacketCost()
	{
		if (_packets.Count >= 5)
			return 0;
		else
			return _packets[0].GetComponent<PacketController>().packetCosts.addPacketCost;
	}
	public float SendCost()
	{
		return _packets[0].GetComponent<PacketController>().packetCosts.sendCost;
	}
	public bool AreSendPacketsRemaining()
	{
		if (sendingCount >= _packets.Count)
			return false;
		return true;
	}
	public void SetPacket_SendNextWave()
	{
		packetsSent = false;
		foreach(GameObject packet in _packets)
		{
			PacketController pack = packet.GetComponent<PacketController>();
			if (!pack.sendNextWave)
			{
				pack.sendNextWave = true;
				++sendingCount;
				break;
			}
		}
	}
	public bool SpawnNextPacket()
	{
		foreach (GameObject packet in _packets)
		{
			PacketController pack = packet.GetComponent<PacketController>();
			if (pack.sendNextWave == true)
			{
				pack.Enable();
				return true;
			}
		}
		sendingCount = 0;
		packetsSent = true;
		return false;
	}
}

public class PacketManager : MonoBehaviour
{
	/**           PUBLIC MEMBERS          **/

	public GameObject[] basePackets; // Base packet for initial loading
	public Transform[] playerWaypoints; // Holds ["spawn", "end"] objects as waypoints
	public Transform[] aiWaypoints; // Holds ["spawn", "end"] objects as waypoints
	public float groupSpawnOffset = 2; // distance from each packet during spawn
	public Button[] packetButtons; // buttons for packet waves
	public float waitBetweenSpawns = 5; // time between each packet spawning.

	/**          PRIVATE MEMBERS          **/

	//private UpgradeManager upMan;
	private List<PacketGroup> playerPackets = new List<PacketGroup>(); // A list of PacketGroups for player
	private List<PacketGroup> aiPackets = new List<PacketGroup>(); // A list of PacketGroups for AI
	private int groupSentCount = 0; // Sentinal value to check for all groups sent all packets; when value = list<Packets>().Count

	/**    PUBLIC UTILITY METHODS         **/

	public PacketGroup GetPacketGroup(string identity, string player)
	{
		if (player == "Player")
		{
			foreach (PacketGroup group in playerPackets)
			{
				if (group.Ident == identity)
					return group;
			}
		}
		else if (player == "AI")
		{
			foreach (PacketGroup group in aiPackets)
			{
				if (group.Ident == identity)
					return group;
			}
		}    
		else
		{
			Debug.Log("Error: No such player: " + player);
			return null;
		}
		return null;
	}
	public PacketGroup GetPacketGroup(int index, string player)
	{
		if (player == "Player")
		{
			if (index >= 0 && index < playerPackets.Count)
				return playerPackets[index];
		}
		else if (player == "AI")
		{
		   if (index >= 0 && index < aiPackets.Count)
			   return aiPackets[index];
		}
		else
		{
			Debug.Log("Error: No such player: " + player);
			return null;
		}
		return null;
	}

	public void SpawnMixedWave()
	{
		StartCoroutine(SpawnMixedPackets());
	}
	IEnumerator SpawnMixedPackets()
	{
		groupSentCount = 0;
		while (groupSentCount < playerPackets.Count)
			foreach (PacketGroup group in playerPackets)
			{
				StartCoroutine(SpawnMixedPacket(group));
				yield return new WaitForSeconds(waitBetweenSpawns);
			}

		groupSentCount = 0;
		while (groupSentCount < aiPackets.Count)
			foreach (PacketGroup group in aiPackets)
			{
				StartCoroutine(SpawnMixedPacket(group));
				yield return new WaitForSeconds(waitBetweenSpawns);
			}

	}
	IEnumerator SpawnMixedPacket(PacketGroup group)
	{
		if (!group.packetsSent && !group.SpawnNextPacket())
			++groupSentCount;
		yield return new WaitForSeconds(waitBetweenSpawns);   //Wait
	}

	public void SpawnNormalWave()
	{
		StartCoroutine(SpawnNormalPackets());
	}
	IEnumerator SpawnNormalPackets()
	{
		yield return new WaitForSeconds(waitBetweenSpawns);
		foreach (PacketGroup group in playerPackets)
		{
			StartCoroutine(SpawnNormalPacket(group));
			yield return new WaitForSeconds(waitBetweenSpawns);
		}

		yield return new WaitForSeconds(waitBetweenSpawns);
		foreach (PacketGroup group in aiPackets)
		{
			StartCoroutine(SpawnNormalPacket(group));
			yield return new WaitForSeconds(waitBetweenSpawns);
		}
	}
	IEnumerator SpawnNormalPacket(PacketGroup group)
	{
		while(group.SpawnNextPacket())
			yield return new WaitForSeconds(waitBetweenSpawns);
	}

	public bool ArePacketsCompleteWave()
	{
		foreach (PacketGroup group in playerPackets)
		{
			if (!group.isHome() || group.sendingCount > 0)
			{
				return false;
			}
		}
		foreach (PacketGroup group in aiPackets)
		{
			if (!group.isHome() || group.sendingCount > 0)
			{
				return false;
			}
		}
		return true;
	}
	public void sendPacket(string identity, string player)
	{
		if (player == "Player")
		{
			foreach (PacketGroup group in playerPackets)
			{
				if (group.Ident == identity)
					if (group.AreSendPacketsRemaining())
						group.SetPacket_SendNextWave();
			}
		}
		else if (player == "AI")
		{
			foreach (PacketGroup group in aiPackets)
			{
				if (group.AreSendPacketsRemaining())
					group.SetPacket_SendNextWave();
			}
		}
		else
			Debug.Log("Error: No such player: " + player);
	}
	public float GetPacketSendCost(string identity, string player)
	{
		if (player == "Player")
		{
			foreach (PacketGroup group in playerPackets)
			{
				if (group.Ident == identity)
					return group.SendCost();
			}
		}
		else if (player == "AI")
		{
			foreach (PacketGroup group in aiPackets)
			{
				return group.SendCost();
			}
		}
		else
			Debug.Log("Error: No such player: " + player);
		return -1;
	}
	public PacketStats GetStatsFromGroup(string identity, string player)
	{
		PacketStats myStats;
		if (player == "Player")
		{
			foreach (PacketGroup group in playerPackets)
			{
				if (group.Ident == identity)
				{
					myStats = group.GetGroupStats();
					return myStats;
				}
			}
		}
		else if (player == "AI")
		{
			foreach (PacketGroup group in aiPackets)
			{
				if (group.Ident == identity)
				{
					myStats = group.GetGroupStats();
					return myStats;
				}
			}
		}
		else
		{
			Debug.Log("Error: No such player: " + player);
			return null;
		}

		return null;
	}
	public void AddPacket(string identity, string player)
	{
		PacketGroup group = GetPacketGroup(identity, player);
		GameObject newPacket = Instantiate(group._packetPrefab) as GameObject;

		newPacket.gameObject.tag = group.Packets[0].gameObject.tag;

		PacketController newPcon = newPacket.GetComponent<PacketController>();

		Transform spawn = GetSpawnWaypoint(player);
		Transform target = GetTargetWaypoint(player);

		newPcon.Spawn = spawn;
		newPcon.Disable();
		newPcon.SetTarget(target);
		group.Packets.Add(newPacket);
	}

	/**    PRIVATE UTILITY METHODS        **/
	private void SetPacketValues(PacketController packet, string tagName, Transform spawnLocation, Transform targetLocation)
	{
		packet.gameObject.tag = tagName;
		packet.Spawn = spawnLocation;
		packet.Disable();
		packet.SetTarget(targetLocation);
	}
	private Transform GetSpawnWaypoint(string player)
	{
		if (player == "Player")
			return playerWaypoints[0];
		else if (player == "AI")
			return aiWaypoints[0];
		else
			return null;
	}
	private Transform GetTargetWaypoint(string player)
	{
		if (player == "Player")
			return playerWaypoints[1];
		else if (player == "AI")
			return aiWaypoints[1];
		else
			return null;
	}

	/** STANDARD UNITY METHODS **/

	void Awake()
	{
		if (basePackets.Length != 5 || packetButtons.Length != 5)
			Debug.Log("Error: Incorrect numbero f packet types and associated buttons. Both must be 5!");

		// Loading each packet type into player group:
		for (int i = 0; i < basePackets.Length; ++i )
		{
			// Player load:
			GameObject newPacket = Instantiate(basePackets[i]) as GameObject;
			SetPacketValues(newPacket.GetComponent<PacketController>(), "Player", playerWaypoints[0], playerWaypoints[1]);
			PacketGroup group = new PacketGroup();
			group._packetPrefab = basePackets[i];
			group.Packets.Add(newPacket);
			group.PacketType = newPacket.GetComponent<PacketController>().Type;
			group.Ident = packetButtons[i].GetComponentInChildren<Text>().text;
			playerPackets.Add(group);
		}

		// Loading each packet type into AI group:
		for (int i = 0; i < basePackets.Length; ++i)
		{
			// Player load:
			GameObject newPacket = Instantiate(basePackets[i]) as GameObject;
			SetPacketValues(newPacket.GetComponent<PacketController>(), "AI", aiWaypoints[0], aiWaypoints[1]);
			PacketGroup group = new PacketGroup();
			group._packetPrefab = basePackets[i];
			group.Packets.Add(newPacket);
			group.PacketType = newPacket.GetComponent<PacketController>().Type;
			group.Ident = packetButtons[i].GetComponentInChildren<Text>().text;
			aiPackets.Add(group);
		}
	}
}

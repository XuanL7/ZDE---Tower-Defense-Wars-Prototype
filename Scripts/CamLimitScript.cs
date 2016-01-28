using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraBounds
{
    public float max_x = 0;
    public float max_z = 0;
    public float min_x = 0;
    public float min_z = 0;
}

public class CamLimitScript : MonoBehaviour {
    public CameraBounds bounds = new CameraBounds();
}

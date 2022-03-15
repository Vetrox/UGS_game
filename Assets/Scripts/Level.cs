using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelFile
{
    public string displayName;
    public int bpm;
    public float start_offset; // in seconds
    public string path;
    public string data;
}

public class Level : MonoBehaviour
{
    public Transform floorTileStraight2Prefab;
    public Transform floorTileStraight3Prefab;
    public Transform floorTileStraight4Prefab;
    public Transform floorTileStraight5Prefab;
    public Transform floorTileConnectorU;
    public Transform floorTileConnectorLR;
    public Transform floorTileConnectorRL;
    public Transform sawPrefab;
    public Transform pipePrefab;
    public Transform goalPrefab;
    public Transform wallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        cookLevel();
    }

    private void cookLevel()
    {
        int lane = 0;
        float y = 0;
        int z = -3;
        Instantiate(floorTileStraight3Prefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
        z += 3;
        y -= 0.00001f;

        foreach (char c in GameManager.GetCurrentLevel().data) {
            y -= 0.00001f;
            switch (c) {
                case '-':
                    Instantiate(floorTileStraight5Prefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    break;
                case 'l':
                    Instantiate(floorTileConnectorRL, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    Instantiate(floorTileStraight4Prefab, new Vector3(lane - 1, y, z + 1), Quaternion.identity, this.transform);
                    lane--;
                    break;
                case 'r':
                    Instantiate(floorTileConnectorLR, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    Instantiate(floorTileStraight4Prefab, new Vector3(lane + 1, y, z + 1), Quaternion.identity, this.transform);
                    lane++;
                    break;
                case 'u':
                    Instantiate(floorTileConnectorU, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    Instantiate(floorTileStraight4Prefab, new Vector3(lane, y + 1, z + 1), Quaternion.identity, this.transform);
                    y++;
                    break;
                case 'd':
                    Instantiate(floorTileStraight5Prefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    Instantiate(sawPrefab, new Vector3(lane, y + 2.5f, z + 1), sawPrefab.transform.rotation, this.transform);
                    break;
                case 'j':
                    Instantiate(floorTileStraight5Prefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    Instantiate(pipePrefab, new Vector3(lane, y + 1, z + 1), Quaternion.identity, this.transform);
                    break;
                case 'w':
                    Instantiate(floorTileStraight5Prefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    Instantiate(wallPrefab, new Vector3(lane, y + 1, z + 1), Quaternion.identity, this.transform);
                    break;
            }

            z += 5;
        }

        Instantiate(goalPrefab, new Vector3(lane, y+2, z), Quaternion.identity, this.transform);
        Instantiate(floorTileStraight3Prefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
    }
}

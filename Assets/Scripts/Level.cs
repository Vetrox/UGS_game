using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class LevelFile
{
    public string displayName;
    public int bpm;
    public float start_offset; // in seconds
    public string data;
}

public class Level : MonoBehaviour
{
    public string levelName;
    LevelFile levelFile;

    public Transform floorTilePrefab;
    public Transform sawPrefab;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset levelTextAsset = Resources.Load<TextAsset>("Levels/" + levelName);
        print(levelTextAsset);
        levelFile = JsonUtility.FromJson<LevelFile>(levelTextAsset.text);
        cookLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void cookLevel()
    {
        int lane = 0;
        int y = 0;
        int z = 0;

        foreach (char c in levelFile.data) {
            switch (c) {
                case '-':
                    Instantiate(floorTilePrefab, new Vector3(lane, y, z), Quaternion.identity, this.transform);
                    break;
                case 'l':
                    Instantiate(floorTilePrefab, new Vector3(++lane, y, --z), Quaternion.identity, this.transform);
                    break;
                case 'r':
                    Instantiate(floorTilePrefab, new Vector3(--lane, y, --z), Quaternion.identity, this.transform);
                    break;
                case 'u':
                    Instantiate(floorTilePrefab, new Vector3(lane, ++y, --z), Quaternion.identity, this.transform);
                    break;
                case 'd':
                    Instantiate(floorTilePrefab, new Vector3(lane, --y, --z), Quaternion.identity, this.transform);
                    Instantiate(sawPrefab, new Vector3(lane, y + 4, z), Quaternion.identity, this.transform);
                    break;
            }

            z += 5;
        }
    }
}

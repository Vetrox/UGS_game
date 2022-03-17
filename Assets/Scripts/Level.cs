using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelFile
{
    public string id;
    public string displayName;
    public string interpret;
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
    public Transform rotatePrefab;
    public Transform wallPrefab;

    private Mesh collisionMesh;
    private MeshCollider colliderComponent;

    // Start is called before the first frame update
    void Start()
    {
        colliderComponent = GetComponent<MeshCollider>();
        cookLevel();
    }

    private void cookLevel()
    {
        int lane = 0;
        float y = 0;
        int z = 0;

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        void addStraightIndices() {
            indices.Add(vertices.Count + 0 - 8);
            indices.Add(vertices.Count + 1 - 8);
            indices.Add(vertices.Count + 4 - 8);
            indices.Add(vertices.Count + 4 - 8);
            indices.Add(vertices.Count + 1 - 8);
            indices.Add(vertices.Count + 5 - 8);
            indices.Add(vertices.Count + 2 - 8);
            indices.Add(vertices.Count + 1 - 8);
            indices.Add(vertices.Count + 6 - 8);
            indices.Add(vertices.Count + 6 - 8);
            indices.Add(vertices.Count + 1 - 8);
            indices.Add(vertices.Count + 5 - 8);
            indices.Add(vertices.Count + 3 - 8);
            indices.Add(vertices.Count + 2 - 8);
            indices.Add(vertices.Count + 7 - 8);
            indices.Add(vertices.Count + 7 - 8);
            indices.Add(vertices.Count + 2 - 8);
            indices.Add(vertices.Count + 6 - 8);
            indices.Add(vertices.Count + 0 - 8);
            indices.Add(vertices.Count + 3 - 8);
            indices.Add(vertices.Count + 4 - 8);
            indices.Add(vertices.Count + 3 - 8);
            indices.Add(vertices.Count + 7 - 8);
            indices.Add(vertices.Count + 4 - 8);
        }

        void instantiateStraight5(float dx, float dy, float dz) {
            Instantiate(floorTileStraight5Prefab, new Vector3(lane + dx, y + dy, z + dz), Quaternion.identity, this.transform);
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 0.0f + dy, z + 5.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 1.0f + dy, z + 5.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 1.0f + dy, z + 5.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 0.0f + dy, z + 5.0f + dz));
            addStraightIndices();
        }

        void instantiateStraight4(float dx, float dy, float dz) {
            Instantiate(floorTileStraight4Prefab, new Vector3(lane + dx, y + dy, z + dz), Quaternion.identity, this.transform);
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 0.0f + dy, z + 4.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 1.0f + dy, z + 4.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 1.0f + dy, z + 4.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 0.0f + dy, z + 4.0f + dz));
            addStraightIndices();
        }

        void instantiateLR(float dx, float dy, float dz) {
            Instantiate(floorTileConnectorLR, new Vector3(lane + dx, y + dy, z + dz), Quaternion.identity, this.transform);
            vertices.Add(new Vector3(lane + 1.5f + dx, y + 0.0f + dy, z + 0.0f + dz));
            vertices.Add(new Vector3(lane + 1.5f + dx, y + 1.0f + dy, z + 0.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 1.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 1.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  1 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count + 10 - 12);
            indices.Add(vertices.Count +  1 - 12);
            indices.Add(vertices.Count + 10 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  9 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  1 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  1 - 12);
            indices.Add(vertices.Count +  0 - 12);
        }

        void instantiateRL(float dx, float dy, float dz) {
            Instantiate(floorTileConnectorRL, new Vector3(lane + dx, y + dy, z + dz), Quaternion.identity, this.transform);
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 1.5f + dx, y + 0.0f + dy, z + 0.0f + dz));
            vertices.Add(new Vector3(lane - 1.5f + dx, y + 1.0f + dy, z + 0.0f + dz));
            vertices.Add(new Vector3(lane - 1.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 1.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  0 - 12);
            indices.Add(vertices.Count +  0 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  1 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  9 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  9 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count + 10 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count + 10 - 12);
            indices.Add(vertices.Count + 11 - 12);
        }

        void instantiateU(float dx, float dy, float dz) {
            Instantiate(floorTileConnectorU, new Vector3(lane + dx, y + dy, z + dz), Quaternion.identity, this.transform);
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 2.0f + dy, z + 0.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 2.0f + dy, z + 0.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 0.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane - 0.5f + dx, y + 2.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 2.0f + dy, z + 1.0f + dz));
            vertices.Add(new Vector3(lane + 0.5f + dx, y + 1.0f + dy, z + 1.0f + dz));
            indices.Add(vertices.Count +  1 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  2 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count + 10 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  9 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  0 - 12);
            indices.Add(vertices.Count +  4 - 12);
            indices.Add(vertices.Count +  9 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  9 - 12);
            indices.Add(vertices.Count + 10 - 12);
            indices.Add(vertices.Count +  5 - 12);
            indices.Add(vertices.Count +  6 - 12);
            indices.Add(vertices.Count +  0 - 12);
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  0 - 12);
            indices.Add(vertices.Count +  3 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count + 11 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  7 - 12);
            indices.Add(vertices.Count +  8 - 12);
            indices.Add(vertices.Count +  6 - 12);
        }

        vertices.Add(new Vector3(-0.5f, 0.0f, -4.0f));
        vertices.Add(new Vector3(-0.5f, 1.0f, -4.0f));
        vertices.Add(new Vector3(0.5f, 1.0f, -4.0f));
        vertices.Add(new Vector3(0.5f, 0.0f, -4.0f));
        instantiateStraight4(0.0f, 0.0f, -4.0f);

        foreach (char c in GameManager.GetCurrentLevel().data.Replace(" ", "")) {
            switch (c) {
                case '-':
                    instantiateStraight5(0.0f, 0.0f, 0.0f);
                    break;
                case 'l':
                    instantiateRL(0.0f, 0.0f, 0.0f);
                    instantiateStraight4(-1.0f, 0.0f, 1.0f);
                    lane--;
                    break;
                case 'r':
                    instantiateLR(0.0f, 0.0f, 0.0f);
                    instantiateStraight4(1.0f, 0.0f, 1.0f);
                    lane++;
                    break;
                case 'u':
                    instantiateU(0.0f, 0.0f, 0.0f);
                    instantiateStraight4(0.0f, 1.0f, 1.0f);
                    y++;
                    break;
                case 'd':
                    instantiateStraight5(0.0f, 0.0f, 0.0f);
                    Instantiate(sawPrefab, new Vector3(lane, y + 2.5f, z + 1), sawPrefab.transform.rotation, this.transform);
                    break;
                case 'j':
                    instantiateStraight5(0.0f, 0.0f, 0.0f);
                    Instantiate(pipePrefab, new Vector3(lane, y + 1, z + 1), Quaternion.identity, this.transform);
                    break;
                case 'w':
                    instantiateStraight5(0.0f, 0.0f, 0.0f);
                    Instantiate(wallPrefab, new Vector3(lane, y + 1, z + 1), Quaternion.identity, this.transform);
                    break;
                case 'c':
                    instantiateStraight5(0.0f, 0.0f, 0.0f);
                    Instantiate(rotatePrefab, new Vector3(lane, y + 1, z + 1), Quaternion.identity, this.transform);
                    break;
            }

            z += 5;
        }
        
        collisionMesh = new Mesh();
        collisionMesh.vertices = vertices.ToArray();
        collisionMesh.triangles = indices.ToArray();
        colliderComponent.sharedMesh = collisionMesh;

        instantiateStraight5(0.0f, 0.0f, 0.0f);
    }
}

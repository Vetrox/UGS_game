using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private class HousePair {
        public Transform left;
        public Transform right;

        public HousePair(Transform left, Transform right)
        {
            this.left = left;
            this.right = right;
        }
    }

    public Transform player;
    // each prefab has to have the same z dimension
    public Transform[] housePrefabs;

    // relative to player of course
    public float minZ = -5.0f, maxZ = 20.0f;

    // the first element has the lowest z-position
    private Queue<HousePair> houses;
    private float largestZ;

    void generate()
    {
        int hi = Random.Range(0, housePrefabs.Length);
        Transform houseLeft = Instantiate(housePrefabs[hi], new Vector3(player.transform.position.x - 5.0f, player.transform.position.y - 2.0f, largestZ), Quaternion.identity);
        hi = Random.Range(0, housePrefabs.Length);
        Transform houseRight = Instantiate(housePrefabs[hi], new Vector3(player.transform.position.x + 5.0f, player.transform.position.y - 2.0f, largestZ), Quaternion.AngleAxis(180.0f, Vector3.up));
        houses.Enqueue(new HousePair(houseLeft, houseRight));
        largestZ += 4;
    }

    void Start()
    {
        // generate houses for left and right, respectively
        houses = new Queue<HousePair>();
        largestZ = minZ;
        for (int i = 0; i < (maxZ - minZ) / 4.0f; i++) {
            generate();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (houses.Count > 0) {
            HousePair housePairWithLeastZ = houses.Peek();
            if (housePairWithLeastZ.left.transform.position.z < player.transform.position.z + minZ) {
                houses.Dequeue();
                Destroy(housePairWithLeastZ.left.gameObject);
                Destroy(housePairWithLeastZ.right.gameObject);
                generate();
            }
        }
    }
}

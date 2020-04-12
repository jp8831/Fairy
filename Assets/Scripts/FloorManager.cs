using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public float offset;
    public float hOffset;
    public int floorCount;

    void Start ()
    {
        for (int f = 0; f < floorCount; f++)
        {
            Vector3 position = transform.position;
            position += Vector3.up * offset * f;
            position += Vector3.right * hOffset * (f % 2.0f == 0.0f ? 1.0f : -1.0f);
            GameObject newFloor = Instantiate (floorPrefab, position, Quaternion.identity);
        }
    }
}

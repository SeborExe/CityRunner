using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] List<Transform> levelParts = new List<Transform>();
    [SerializeField] Player player;

    [SerializeField] private Vector3 nextPartPosition;
    [SerializeField] private float partDrawDistance;

    private float partDeleteDistance = 15f;

    private void Update()
    {
        GeneratePart();
        DeletePart();
    }

    private void GeneratePart()
    {
        while ((nextPartPosition.x - player.transform.position.x) < partDrawDistance)
        {
            Transform part = levelParts[Random.Range(0, levelParts.Count)];
            Transform newPart = Instantiate(part, nextPartPosition - part.Find("StartPoint").position, Quaternion.identity, transform);

            nextPartPosition = newPart.Find("EndPoint").position;
        }
    }

    private void DeletePart()
    {
        if (transform.childCount > 0)
        {
            Transform part = transform.GetChild(0);
            Vector3 distance = player.transform.position - part.transform.position;

            if (distance.x > partDeleteDistance)
            {
                Destroy(part.gameObject);
            }
        }
    }
}

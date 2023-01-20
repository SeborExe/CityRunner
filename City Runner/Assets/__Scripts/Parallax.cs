using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;
    private GameObject cam;

    [SerializeField] float parallaxEffect;

    private void Start()
    {
        cam = Camera.main.gameObject;

        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x) * (1 - parallaxEffect); // how for moved relativity to camera
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        /*
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
        */
    }
}

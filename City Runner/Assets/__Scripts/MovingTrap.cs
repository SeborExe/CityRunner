using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : Trap
{
    [Header("Moving Trap")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private int nextPosition;
    [SerializeField] private float trapSpeed;
    [SerializeField] private float rotationMultiplier;
    [SerializeField] private bool randomNextPosition = false;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[nextPosition].position, trapSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoints[nextPosition].position) < 0.5f)
        {
            if (!randomNextPosition)
            {
                nextPosition++;
            }
            else
            {
                nextPosition = UnityEngine.Random.Range(0, movePoints.Length);
            }

            if (nextPosition >= movePoints.Length)
            {
                nextPosition = 0;
            }
        }

        Rotate();
    }

    private void Rotate()
    {
        if (transform.position.x > movePoints[nextPosition].position.x)
        {
            transform.Rotate(new Vector3(0, 0, 100f * rotationMultiplier) * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -100f * rotationMultiplier) * Time.deltaTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}

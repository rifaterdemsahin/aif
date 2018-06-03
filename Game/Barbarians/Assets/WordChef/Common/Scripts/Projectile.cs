using UnityEngine;
using System;

public class Projectile: MonoBehaviour {
    public Transform destination;
    public float speed;
    public Action<Projectile> onDestroy;
    public Vector3 direction;

    private const int STAND = 0;
    private const int MOVE = 1;

    private int state;

    public void Move()
    {
        state = MOVE;
    }

    private void Update()
    {
        if (state == MOVE)
        {
            transform.position += Time.deltaTime * speed * direction;
        }

        if (CUtils.IsOutOfScreen(transform.position, 0.5f))
        {
            DoDestroy();
        }
    }

    public void DoDestroy()
    {
        onDestroy(this);
    }
}
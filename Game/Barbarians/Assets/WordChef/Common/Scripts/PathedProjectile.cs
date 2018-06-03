using UnityEngine;

public class PathedProjectile: MonoBehaviour {
    private Transform destination;
    private float speed;

    private void Start()
    {

    }

    public void Initalize(Transform destination, float speed)
    {
        this.destination = destination;
        this.speed = speed;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * speed);
        float distance = (destination.transform.position - transform.position).sqrMagnitude;
        if (distance > 0.01f * 0.01f)
            return;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
    }

}
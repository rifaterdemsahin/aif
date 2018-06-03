using UnityEngine;
using System.Collections;

public class PathedProjectileSpawner : MonoBehaviour {
    public Transform destination;
    public PathedProjectile Projectile;
    public float speed;
    public float fireRate;

    private float nextShotInSeconds;

	private void Start () {
        nextShotInSeconds = fireRate;
	}

    private void Update()
    {
        if ((nextShotInSeconds -= Time.deltaTime) > 0)
            return;
        nextShotInSeconds = fireRate;

        PathedProjectile projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initalize(destination, speed);
	}

    // This function is not working for unknown reason (Windows 8.1, 64bit)
    private void OnDrawGimos()
    {
        if (destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, destination.position);
    }
}

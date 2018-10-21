using UnityEngine;
using System.Collections;

public class Bullet: MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.
    public string ignoreTag = "Bullet";

	void Start () 
	{        
        // Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
        Destroy(gameObject, 4);

	}


	void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (//collision.tag != ignoreTag && 
            collision.name != gameObject.transform.root.name && collision.tag != ignoreTag)
        {
             GameObject explosion = new GameObject("Explosion");
             explosion.transform.position = transform.position;
             explosion.tag = "ExplosionFX_bullet";
             Destroy(explosion, 0.5f);
             CircleCollider2D explosionRadius = explosion.AddComponent<CircleCollider2D>();
             explosionRadius.radius = 2.5f;

             OnExplode();
             Destroy(gameObject);
       
        }
    }

}//Rocket

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : PlayerControl {

    private Transform hero;
    private Gun gun;
    private float shootAngle = 30;


	void Start () {

        hero = GameObject.FindGameObjectWithTag("Player").transform;

        gun = GetComponentInChildren<Gun>();

	}
	
	float CalculaVelocidad(Transform target, float angle)
    {
        var dir = target.position - transform.position;

        var h = dir.y;
        dir.y = 0;
        var dist = dir.magnitude;
        var a = angle * Mathf.Deg2Rad;

        dist += h / Mathf.Tan(a);

        return Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sign(2 * a)) * Random.Range(1.2f, 1.8f);

    }

	void Update () {
		
        if (hasTurn)
        {
            if (facingRight)
                Flip();

            if (tilt != shootAngle)
            {
                if (tilt < shootAngle)
                    RotaIzquierdaDown();
                else
                    RotaDerechaDown();
            }
            else
            {
                StartCoroutine(WaitALittleToShoot());
                
            }
        }
        else
        {
            shootAngle = Random.Range(25, 45);
        }

	}

    IEnumerator WaitALittleToShoot()
    {
        float timeToWait = Random.Range(1f, 2f);
        yield return new WaitForSeconds(timeToWait);
        gun.Fire(CalculaVelocidad(hero, shootAngle));
        RotaDerechaUp();
        RotaIzquierdaUp();
    }




}//EnemyAI

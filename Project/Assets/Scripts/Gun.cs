using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;              // Prefab of the rocket.
    public Sprite attackBarSprite;

    private enum States { Down, Fire, Up }
    private States state = States.Up;
    private float speed = 0f;
    private float targetSpeed = 0f;

    private Transform attackBar;

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.

    public delegate void triggerDelegate();
    public event triggerDelegate gunFired;


    void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();

        GameObject attackBarObject = new GameObject("Power");
        attackBar = attackBarObject.transform;
        attackBar.SetParent(transform);
        attackBar.localPosition = Vector3.zero;
        attackBar.localRotation = Quaternion.identity;
        attackBar.localScale = Vector3.up * 2 + Vector3.forward;
        SpriteRenderer rend = attackBarObject.AddComponent<SpriteRenderer>();
        rend.sprite = attackBarSprite;
        rend.sortingLayerID = transform.root.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
    
    
	}


	void Update ()
	{
        switch (state)
        {
            case States.Down:
                speed += Time.deltaTime * 30;
                attackBar.localScale += Vector3.right * 0.01f;
                attackBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, attackBar.localScale.x);
                break;
            case States.Fire:
                Fire();
                state = States.Up;
                break;
        }
	}

    public void FireUp()
    {
        state = States.Fire;
    }

    public void FireDown()
    {
        state = States.Down;
    }

    public void Fire()
    {
        anim.SetTrigger("Shoot");
        GetComponent<AudioSource>().Play();

        if (transform.root.GetComponent<PlayerControl>().facingRight)
        {
            Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, transform.rotation, this.gameObject.transform) as Rigidbody2D;
            bulletInstance.velocity = transform.right.normalized * speed;
            

            targetSpeed = speed = 0;
            attackBar.localScale = Vector3.up * 2 + Vector3.forward;

        }
        else
        {
            Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, transform.rotation, this.gameObject.transform) as Rigidbody2D;
            bulletInstance.velocity = transform.right.normalized * -speed;
           

            targetSpeed = speed = 0;
            attackBar.localScale = Vector3.up * 2 + Vector3.forward;
        }

        if (gunFired != null)
        {
            gunFired();
        }

    }



    public void Fire(float targetSpeed)
    {
        this.targetSpeed = targetSpeed;
        speed = targetSpeed;
        Fire();
       
    }


}//Gun

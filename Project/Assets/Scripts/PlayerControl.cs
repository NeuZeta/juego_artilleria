using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.


	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.

    protected float tilt;
    private List<KeyCode> acciones = new List<KeyCode>();
    private Transform pivot;

    private float axisIncrease;      //Incremento mientras el boton está pulsado

    [HideInInspector]
    public bool hasTurn = false;
    

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
        pivot = transform.Find("pivot");
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
			jump = true;



	}

	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
        

        if (h == 0)
        {
            if (acciones.Contains(KeyCode.LeftArrow))
            {
                if (axisIncrease > -1)
                {
                    axisIncrease -= 0.1f;
                }
                h = axisIncrease;
                Debug.Log(h);
            }
            else if (acciones.Contains(KeyCode.RightArrow))
            {
                if (axisIncrease < 1)
                {
                    axisIncrease += 0.1f;
                }
                h = axisIncrease;
                Debug.Log(h);
            }
            else
            {
                axisIncrease = 0f;
            }
        }

        anim.SetFloat("Speed", Mathf.Abs(h));

        if (acciones.Contains(KeyCode.UpArrow))
        {
            tilt += 1.0f;
        }
        if (acciones.Contains(KeyCode.DownArrow))
        {
            tilt -= 1.0f;
        }

        tilt = Mathf.Clamp(tilt, 0, 75);
   
        if (facingRight)
        pivot.rotation = Quaternion.Euler(0, 0, tilt);
        else
        pivot.rotation = Quaternion.Euler(0, 0, -tilt);

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

		
		if(jump)
		{
			anim.SetTrigger("Jump");

			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			jump = false;
		}

    }
	
	
	public void Flip ()
	{
        facingRight = !facingRight;

        tilt = 0;

        Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;  
	}


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!GetComponent<AudioSource>().isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}


	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}

    public void PlayerJump()
    {
        if (grounded)
        jump = true;
    }

    private void ActualizarAccionDown(KeyCode code)
    {
        if (!acciones.Contains(code)) acciones.Add(code);
    }

    private void ActualizarAccionUp(KeyCode code)
    {
        if (acciones.Contains(code)) acciones.Remove(code);
    }

    public void MueveDerechaDown()
    {
        ActualizarAccionDown(KeyCode.RightArrow);
    }

    public void MueveIzquierdaDown()
    {
        ActualizarAccionDown(KeyCode.LeftArrow);
    }

    public void RotaIzquierdaDown()
    { 
        ActualizarAccionDown(KeyCode.UpArrow);
    }

    public void RotaDerechaDown()
    {
        ActualizarAccionDown(KeyCode.DownArrow);

    }

    public void MueveDerechaUp()
    {
        ActualizarAccionUp(KeyCode.RightArrow);
    }

    public void MueveIzquierdaUp()
    {
        ActualizarAccionUp(KeyCode.LeftArrow);
    }

    public void RotaIzquierdaUp()
    {
        ActualizarAccionUp(KeyCode.UpArrow);
    }

    public void RotaDerechaUp()
    {
        ActualizarAccionUp(KeyCode.DownArrow);
    }




} //PlayerControl

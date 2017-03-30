using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float maxSpeed = 6.0f;
	[SerializeField] private float JumpSpeed = 6.0f;
	[SerializeField] private Transform groundCheck;

	private bool facingRight = false;
	private bool grounded = false;
	private float moveDirection;
	private Rigidbody2D rb;

	private Animator anim;


	public float groundRadius = 0.2f;
	public LayerMask ground;

	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void Update(){
		anim.SetFloat ("VelX", Mathf.Abs (moveDirection));
		anim.SetBool ("isGrounded", grounded);
	}
	void Flip(){
		facingRight = !facingRight;
		transform.Rotate (Vector3.up, 180.0f, Space.World);
	}

	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, ground);
		//
		if(EstadoJuego.estadoJuego.touch){
			//moveDirection = Input.GetAxisRaw ("Horizontal");
			moveDirection = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
			//print("tactil");
		}else{
			moveDirection = Input.acceleration.x * 2;
			//print("acelelrometro");
		}
		
		if(Input.GetKeyDown(KeyCode.Space)){
			Saltar();
		}
		rb.velocity = new Vector2 (moveDirection * maxSpeed, rb.velocity.y);
		if (moveDirection > 0.0f && !facingRight) {
			Flip ();
		} else if (moveDirection < 0.0f && facingRight) {
			Flip();
		}

	}

	public void Saltar(){
		if(grounded){
		rb.AddForce(new Vector2(0.0f, JumpSpeed));
		}
	}

	

	
}

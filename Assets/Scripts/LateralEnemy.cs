using System.Collections;
using UnityEngine;

public class LateralEnemy : MonoBehaviour {
[SerializeField] Vector2 leftPosition;
	[SerializeField] Vector2 rightPosition;
	[SerializeField] float speed;
	private bool used = false;
	private Vector3 newTarget;
	private bool facingRight = false;

	// Use this for initialization
	void Start () {
		StartCoroutine (Move (rightPosition));
	}

	IEnumerator Move(Vector2 target) {

		yield return new WaitForSeconds (0.5f);
		while (Mathf.Abs((target.x - transform.localPosition.x)) > 0.20f) {

			Vector3 direction = target.x == leftPosition.x ? Vector2.left : Vector2.right;
			transform.localPosition += direction * Time.deltaTime * speed;

			yield return null;
		}

		

		newTarget = target.x == leftPosition.x ? rightPosition : leftPosition;
		Flip ();
		gameObject.SetActive(false);
		used = true;
		
	}
	void OnTriggerEnter2D(Collider2D other){
			
			if(other.CompareTag("Player") ){
				
				//gameObject.SetActive(false);
				if(GameManager.instance.Lifes<=1){
					GameManager.instance.loseLife();
					Time.timeScale = 0;
				}else{
					GameManager.instance.loseLife();
				}
				
				
			}
	}

	void OnEnable(){
		if(used){
			speed += 0.3f;
			StartCoroutine (Move (newTarget));
		}
	}
	void Flip(){
		facingRight = !facingRight;
		transform.Rotate (Vector3.up, 180.0f, Space.World);
	}
}


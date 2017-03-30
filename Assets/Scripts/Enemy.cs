using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] private float fallSpeed;
	private Rigidbody2D rb;
	private BoxCollider2D coll;
	private bool used = false;
	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<BoxCollider2D>();
		transform.position = new Vector2(Random.Range(-7.4f, 7.4f),8f);
		
	}
		

	
	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Player") ){
			ObjectPoolerSimple.instance.pooledObjectsList.Remove(gameObject);
			ObjectPoolerSimple.instance.DestroyPooledObject(gameObject);
			ObjectPoolerSimple.instance.pooledObjectsList.Add(gameObject);
			if(GameManager.instance.Lifes<=1){
				GameManager.instance.loseLife();
				Time.timeScale = 0;
			}else{
				GameManager.instance.loseLife();
			}
			
		}else if(other.CompareTag("Floor")){
			GameManager.instance.SpawnExplosion(transform.position.x);
			used = true;
			rb.simulated = false;
			coll.enabled= false;
			Invoke("Coin",0.5f);
			Destroy();
		}

	}
	void Destroy(){
		GameManager.instance.sonido();
		ObjectPoolerSimple.instance.pooledObjectsList.Remove(gameObject);
		ObjectPoolerSimple.instance.DestroyPooledObject(gameObject);
		ObjectPoolerSimple.instance.pooledObjectsList.Add(gameObject);
	}

	
	void OnEnable(){
			transform.position = new Vector2(Random.Range(-7.4f, 7.4f),8f);
			if(used){
				rb.simulated = true;
				coll.enabled = true;

			}
			
	}
	void Coin(){
		GameManager.instance.SpawnCoin(transform.position.x);
	}



}

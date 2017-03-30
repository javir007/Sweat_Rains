using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public GameObject coin;//gameobject or prefab that is going to pool
	public int coinAmount; //number of clone at start
	public float CoinspawnDelay;
	public bool pausa = false;
	public bool GameOvered = false;
	public bool activarIntersticial = false;

	[SerializeField] private GameObject player;
	[SerializeField] private float spawnDelay;
	[SerializeField] private GameObject Panel;
	[SerializeField] private GameObject Canvas;
	[SerializeField] private float timeBetweenWaves;
	[SerializeField] private Text waveText;
	[SerializeField] private Text CoinsText;
	[SerializeField] private Text LifesText;
	[SerializeField] private Text moneytxt;
	[SerializeField] private Text enemiestxt;
	[SerializeField] private Text scoretxt;
	[SerializeField] private Text bestscoretxt;
	[SerializeField] private Text totalZanahorias;
	[SerializeField] private int enemiesPerWave= 5;
	[SerializeField] private GameObject lateralEnemy;
	[SerializeField] private GameObject waveBanner;
	[SerializeField] private AudioClip item;
	[SerializeField] private AudioClip explo;
	[SerializeField] private AudioClip GameOvertheme;
	[SerializeField] private GameObject explocion;
	[SerializeField] private GameObject playerExplo;
	[SerializeField] private GameObject MainCamera;
	[SerializeField] private GameObject AdsButton;
	[SerializeField] private GameObject touchMovemet;
	[SerializeField] private GameObject circleMovement;
	[SerializeField] private Text Contador;
	[SerializeField] private GameObject PanelContador;

	private int waveNumber = 1;
	private int money;
	private int enemiesPerSpawn = 0;
	private  List<GameObject> coinsList;
	private  List<GameObject> explosionList;
	private GameObject tempGameObject;
	private GameObject tempExploGameObject;
	private int lifes = 2;
	private bool waveOver = false;
	private int points=0;
	private AudioSource audio;
	private bool touch = false;
	public  static  int  partidaSiguiente = 1;
	private const int partidaParaIntersticial = 5;


	public bool Touch{
		get{
			return touch;
		}
	}

	public int Points{
		get{
			return points;
		}
		set{
			points = value;
		}
	}

	
	public bool WaveOver{
		get{
			return waveOver;
		}
	}
	public GameObject Player{
		get{
			return player;
		}
	}
	public int Money{
		get{
			return money;
		}
		set{
			money = value;
		}
	}
	public int Lifes{
		get{
			return lifes;
		}
		set{
			lifes = value;
		}
	}

	void Awake()
		{
			instance = this; //initialize singleton
		}
	// Use this for initialization
	void Start () {
		touch = EstadoJuego.estadoJuego.touch;
		audio = GetComponent<AudioSource>();
		CoinsText.text = money.ToString();
		LifesText.text = lifes.ToString();
		if(enemiesPerWave > ObjectPoolerSimple.instance.pooledObjectsList.Count){
			ObjectPoolerSimple.instance.maxGrowAmount = enemiesPerWave;
		}
		StartCoroutine(spawn());
		startCoins();
		startExplosion();


	}
	IEnumerator spawn(){
		if(enemiesPerSpawn < enemiesPerWave){
			for(int i = 0; i < enemiesPerWave; i++){
				GameObject newEnemy = ObjectPoolerSimple.instance.GetPooledObject();
				enemiesPerSpawn++;
				newEnemy.SetActive(true);
				points++;
				yield return new WaitForSeconds(spawnDelay);
			} 
			
		}
		if(enemiesPerWave > ObjectPoolerSimple.instance.pooledObjectsList.Count){
			ObjectPoolerSimple.instance.maxGrowAmount = enemiesPerWave;
		}
		if(enemiesPerSpawn < enemiesPerWave){
				StartCoroutine(spawn());
		}
		print("fin oleada");
		if(enemiesPerSpawn == enemiesPerWave){
			waveisOver();
			yield return new WaitForSeconds(timeBetweenWaves);
			waveBanner.SetActive (false);
			StartCoroutine(spawn());
			waveOver = false;
			if(waveNumber>3){
				lateralEnemy.SetActive(true);
				if(waveNumber%3 == 0){
					if(spawnDelay>0.1){
						spawnDelay -= 0.1f;
						print (spawnDelay.ToString());
						
					}
				}
			}
		}
	}

	public void waveisOver(){
		waveNumber++;
		waveText.text = "Wave "+ waveNumber;
		print (waveNumber.ToString ());
		waveBanner.SetActive (true);
		waveOver = true;
		enemiesPerWave ++;
		enemiesPerSpawn=0;
		
		
	}

	private void startCoins(){
		coinsList = new List<GameObject>();
               //instantiate pooledAmount of clones
			for (int i=0; i<coinAmount; i++)
			{
				tempGameObject = (GameObject)Instantiate(coin);
				tempGameObject.SetActive(false);
				coinsList.Add(tempGameObject);//assign in list of clones
			}
	}

	public void SpawnCoin(float point){
		GameObject newCoin = GetPooledcoin();
		newCoin.transform.position = new Vector2(point,-3.75f);
		newCoin.SetActive(true);
		StartCoroutine(removeCoin(newCoin));

		
		
	}

	private void startExplosion(){
		explosionList = new List<GameObject>();
               //instantiate pooledAmount of clones
			for (int i=0; i<coinAmount; i++)
			{
				tempExploGameObject = (GameObject)Instantiate(explocion);
				tempExploGameObject.SetActive(false);
				explosionList.Add(tempExploGameObject);//assign in list of clones
			}
	}

	public void SpawnExplosion(float point){
		GameObject newExplo = GetPooledExplosion();
		newExplo.transform.position = new Vector2(point,-4.065f);
		newExplo.SetActive(true);
		StartCoroutine(removeExplo(newExplo));

	}

	private GameObject GetPooledExplosion()
		{
               //return first non used clone from list
			for (int i=0; i<explosionList.Count; i++)
			{
				if (!explosionList [i].activeInHierarchy)
					return explosionList [i];
			}
			return null;
		}

	private GameObject GetPooledcoin()
		{
               //return first non used clone from list
			for (int i=0; i<coinsList.Count; i++)
			{
				if (!coinsList [i].activeInHierarchy)
					return coinsList [i];
			}
			return null;
		}
	public void increasePoint(){
		audio.PlayOneShot(item);
		money++;
		CoinsText.text = money.ToString();
	}

	public void loseLife(){
		if(!pausa){
			playerExplo.SetActive(true);
			playerExplo.transform.position = new Vector2(player.transform.position.x,-4.068846f);
			player.GetComponent<AudioSource>().Play();
			lifes--;
			LifesText.text = lifes.ToString();
			if(lifes<=0){
				Panel.SetActive(true);
				Canvas.SetActive(false);
				GameOver();
			}
		}
	}

	public void Restart(){
		Time.timeScale = 1f;
		if (activarIntersticial) {
			if(partidaSiguiente >= partidaParaIntersticial){
				EasyGoogleMobileAds.GetInterstitialManager().ShowInterstitial();
				partidaSiguiente = 1;
			}else{
				partidaSiguiente ++;
			}
		}
		Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

	}

	public void Pause(){
		
		Time.timeScale = 0;
		activarJugador(true);
		pausa = true;
	}

	public void Wrapper(){
		Time.timeScale = 1;
		StartCoroutine(activar());
		PanelContador.SetActive (true);
		StartCoroutine (conteo ());
		
		
		
	}
	IEnumerator activar(){
		yield return new WaitForSeconds(2);
		activarJugador(false);
		pausa = false;
	}

	public void sonido(){
		audio.PlayOneShot(explo);
	}

	void activarJugador(bool opcion){
		player.GetComponent<Rigidbody2D>().isKinematic = opcion;
		player.GetComponent<BoxCollider2D>().enabled = !opcion;
		player.GetComponent<PlayerMovement>().enabled = !opcion;
	}

	IEnumerator removeCoin(GameObject coin){
		yield return new WaitForSeconds(CoinspawnDelay);
		coin.SetActive(false);
	}
	IEnumerator removeExplo(GameObject coin){
		yield return new WaitForSeconds(1);
		coin.SetActive(false);
	}
	
	public void GameOver(){
		DestroyAllObject ();
		EstadoJuego.estadoJuego.touch = touch;
		ObjectPoolerSimple.instance.DestroyAllObject ();
		MainCamera.GetComponent<AudioSource>().clip = GameOvertheme;
		MainCamera.GetComponent<AudioSource>().Play();
		totalZanahorias.gameObject.SetActive(true);
		totalZanahorias.text = EstadoJuego.estadoJuego.Monedero.ToString();
		GameOvered = true;
		int puntos = money+points;
		EstadoJuego.estadoJuego.Monedero += money;
		if(puntos>=EstadoJuego.estadoJuego.Puntos){
			EstadoJuego.estadoJuego.Puntos = puntos;
		}
		EstadoJuego.estadoJuego.muertes++;
		moneytxt.text = money.ToString();
		enemiestxt.text = points.ToString();
		scoretxt.text = puntos.ToString();
		EstadoJuego.estadoJuego.Guardar();
		bestscoretxt.text = EstadoJuego.estadoJuego.Puntos.ToString();
	}
	public void Silencio(){
		EstadoJuego.estadoJuego.sonido = true;
		AudioListener.pause = EstadoJuego.estadoJuego.sonido;
	}
	public void VolumenPrendido(){
		EstadoJuego.estadoJuego.sonido = false;
		AudioListener.pause = EstadoJuego.estadoJuego.sonido;
	}
	public void Control(bool opcion){
		touch = opcion;
		}

	public void DestroyAllObject()
	{
		for (int i=0; i<coinsList.Count; i++)
		{
			if (coinsList [i].activeInHierarchy)
				coinsList [i].SetActive(false);
		}

		for (int i=0; i<explosionList.Count; i++)
		{
			if (explosionList [i].activeInHierarchy)
				explosionList [i].SetActive(false);
		}
	}

	IEnumerator conteo(){
		Contador.text = 3.ToString ();
		yield return new WaitForSeconds (1);
		Contador.text = 2.ToString ();
		yield return new WaitForSeconds (1);
		Contador.text = 1.ToString ();
		yield return new WaitForSeconds (0.5f);
		PanelContador.SetActive (false);

	}

	public void Video(int ganancia){
		money += ganancia;
		moneytxt.text = money.ToString();
	}



	
}

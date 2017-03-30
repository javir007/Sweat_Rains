using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class EstadoJuego : MonoBehaviour {

	public static EstadoJuego  estadoJuego;
	public int Monedero = 0;
	public bool sonido = false;
	public int Puntos = 0;
	public int muertes = 0;
	public bool touch = false;
	

	private String rutaArchivo;

	void Awake(){

		//audio.Play ();
		rutaArchivo = Application.persistentDataPath + "/datos.dat";
		if (estadoJuego == null) {
			estadoJuego = this;
			DontDestroyOnLoad (gameObject);
		} else if (estadoJuego != this) {
			Destroy(gameObject);		
		}

	}

	// Use this for initialization
	void Start () {
		
		Cargar ();


		string[] testDeviceIDs = new string[]{ "1037D37CAEF3081E01787FDAF371FA5D" };
		EasyGoogleMobileAds.GetInterstitialManager ().SetTestDevices (true, testDeviceIDs);

		EasyGoogleMobileAds.GetInterstitialManager ().PrepareInterstitial ("ca-app-pub-1966397002893488/8340016453");
	}
	
	// Update is called once per frame


	public void Cargar(){

		if (File.Exists (rutaArchivo)) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (rutaArchivo, FileMode.Open);
				DatosAGuardar datos = (DatosAGuardar)bf.Deserialize (file);
				Monedero = datos.TotalMonedas;
				Puntos = datos.TotalPuntos;
				sonido = datos.volumen;
				muertes = datos.muertes_jugadas;
				touch = datos.touch;
				file.Close ();
				} else {
			Monedero = 0;
			Puntos = 0;
		}
	}

	public void Guardar(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(rutaArchivo);

		DatosAGuardar datos = new DatosAGuardar ();
		datos.TotalPuntos = Puntos;
		datos.TotalMonedas = Monedero;
		datos.volumen = sonido;
		datos.muertes_jugadas = muertes;
		datos.touch = touch;
		bf.Serialize (file, datos);
		file.Close ();
	}
}
[Serializable]
class DatosAGuardar{
	public int TotalMonedas;
	public int TotalPuntos;
	public bool volumen;
	public int muertes_jugadas;
	public bool touch = false;

}
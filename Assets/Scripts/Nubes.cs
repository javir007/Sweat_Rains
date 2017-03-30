using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nubes : MonoBehaviour {
	[SerializeField] private float objectSpeed = 1;
	[SerializeField] private float resetPosition = -21.65f;
	[SerializeField] private float startPosition = 79.67f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	 void Update () {

		if (!GameManager.instance.GameOvered) {
			transform.Translate(Vector3.left * (objectSpeed * Time.deltaTime));
			if (transform.localPosition.x <= resetPosition) {
				Vector3 newPos = new Vector3(startPosition, transform.position.y, transform.position.z);
				transform.position = newPos;
			}
		}

	}
}

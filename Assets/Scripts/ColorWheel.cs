using UnityEngine;
using System.Collections;

public class ColorWheel : MonoBehaviour {


	// Use this for initialization
	void Start () {
		transform.SetParent (this.gameObject.transform, false);

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Space)) {
			transform.localScale = new Vector2 (1, 1);
		} else {
			transform.localScale = new Vector2 (0, 0);
		}
	}
}

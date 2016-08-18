using UnityEngine;
using System.Collections;

public class SplatterChecker: MonoBehaviour {
	//GameObject parent = GetComponent<GameObject>();



	// Use this for initialization
	void Start () {
		transform.SetParent (this.gameObject.transform, false);
			
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//transform.localRotation = Quaternion.identity;
		//transform.localPosition = Vector2.zero;
		//transform.localScale = Vector2.one;

	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
			CharacterControllerScript script = GetComponentInParent<CharacterControllerScript> ();
			if (coll.tag == "Splatter") {
			CharacterControllerScript.splatters.Add (coll.gameObject);
			}
		Debug.Log ("in");
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		//CharacterControllerScript script = GetComponentInParent<CharacterControllerScript> ();
		if (coll.tag == "Splatter") {
			CharacterControllerScript.splatters.Remove (coll.gameObject);
		}	
		Debug.Log ("out");
	}

	
}

using UnityEngine;
using System.Collections;

public class Splatter : MonoBehaviour {

	public Color color;

	// Use this for initialization
	void Start () {
	
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		Debug.Log ("r :" + color.r + " g: " + color.g + " b: " + color.r);
		renderer.color = color;

	}
	
	// Update is called once per frame
	void Update () {
		//this.GetComponent<SpriteRenderer>().color = color;
	}

	public void UpdateSplatterColor()
	{

	}
}

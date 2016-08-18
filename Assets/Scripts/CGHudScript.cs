using UnityEngine;
using System.Collections;

public class CGHudScript: MonoBehaviour {
	
	public SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
		transform.SetParent (this.gameObject.transform, false);
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer> ();

	}

	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCGHud(float rotation, Color color)
	{
		renderer.color = color;
	}
}

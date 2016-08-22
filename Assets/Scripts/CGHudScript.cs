using UnityEngine;
using System.Collections;

public class CGHudScript: MonoBehaviour {
	private SpriteRenderer renderer;
	private Color color;
	// Use this for initialization
	void Start () {
		//transform.SetParent (this.gameObject.transform, false);
		SpriteRenderer renderer = this.GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCGHud(float rotation, Color color, float chroma)
	{
		if (chroma == 0) {
			transform.localScale = new Vector3 ( 0, 0, 0 );
		} else {
			transform.localScale = new Vector3 ( 1, 1, 1 );
			SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
			renderer.color = color;
			transform.eulerAngles = Vector3.forward * rotation;
		}
	}
}

using UnityEngine;
using System.Collections;

public class BucketFill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		Bucket bucket = GetComponentInParent<Bucket>();
		renderer.color = bucket.color;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

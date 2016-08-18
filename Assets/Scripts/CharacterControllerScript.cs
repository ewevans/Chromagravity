using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CharacterControllerScript : MonoBehaviour {

	public float maxSpeed;
	public GameObject painting;
	Quaternion iniRot;
	public static List<GameObject> splatters;
	public CGHudScript CGHud; 


	// Use this for initialization
	void Start () {
		CGHudScript CGHud = GetComponentInChildren<CGHudScript>();
		splatters = new List<GameObject>();
		iniRot = transform.rotation;
	}

	// Update is called once per frame
	void FixedUpdate () {
	
		Rigidbody2D body = GetComponent<Rigidbody2D> ();

		if (!Input.GetKey (KeyCode.Space)) {
			float moveX = Input.GetAxis ("Horizontal");
			float moveY = Input.GetAxis ("Vertical");

			Color avgRGB = new Color (0, 0, 0);

			if (splatters.Count == 0) {
				//CG force
				Vector2 CompressedPos = new Vector2 ((int)transform.position.x * 20, (int)transform.position.y * 20);
				//Debug.Log(CompressedPos.x + " " + CompressedPos.y);

				Lab avgLab = new Lab (0, 0, 0);
				for (int i = (int)CompressedPos.x - 3; i < (int)CompressedPos.x + 3; i++) {
					for (int j = (int)CompressedPos.y - 3; j < (int)CompressedPos.y + 3; j++) {
						avgLab += PaintingController.CompressedLabArray [(int)CompressedPos.x + (i - (int)CompressedPos.x), (int)CompressedPos.y + (j - (int)CompressedPos.y)];
					}
				}
				avgLab = avgLab / new Lab (25, 25, 25);

		
				avgRGB = ColorUtil.ConvertLABtoRGB (avgLab);
				Debug.Log (avgRGB.r + " " + avgRGB.g + " " + avgRGB.b); 
			} else {
				foreach (GameObject obj in splatters) {
					Splatter splatter = obj.GetComponent<Splatter> ();
					avgRGB.r += splatter.color.r;// * 255f;
					avgRGB.g += splatter.color.g;// * 255f;
					avgRGB.b += splatter.color.b;// * 255f;
				}
				avgRGB.r /= splatters.Count; 
				avgRGB.g /= splatters.Count; 
				avgRGB.b /= splatters.Count; 
			}

			//Debug.Log (avgRGB.r + " " + avgRGB.g + " " + avgRGB.b);
			float chroma = 0;
			float hue = ColorUtil.GetHueLightnessFromRGB (avgRGB, ref chroma);

			hue = (hue / 180f * Mathf.PI);
			//Debug.Log (hue);

			//body.AddForce (new Vector2 ((float)Mathf.Cos (hue)*14, (float)Mathf.Sin (hue))*14, ForceMode2D.Force);

			//input force
			body.AddForce (new Vector2 (((moveX * 3) + ((float)Mathf.Cos (hue)) * 4), ((moveY * 3)) + ((float)Mathf.Sin (hue)) * 4), ForceMode2D.Impulse);

	//		CGHud.UpdateCGHud (0, avgRGB);


			if (body.velocity.magnitude > maxSpeed) {
				body.velocity = body.velocity.normalized * maxSpeed;
			}

		} else {
			body.velocity = Vector2.zero;
		}

	
		//face direction of moving by rotation

	}
	void LateUpdate() {
		transform.rotation = iniRot;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Bucket") {
			Destroy (coll.gameObject);
		}
	}	

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacterControllerScript : MonoBehaviour {

	public float maxSpeed;
	public GameObject painting;
	Quaternion iniRot;
	public static List<GameObject> splatters;
	public CGHudScript CGHud; 
	public int bucketAmount;
	public Color bucketColor;
	public GameObject BucketHUD;
	public GameObject BucketHUDBG;

	// Use this for initialization
	void Start () {
		CGHudScript CGHud = GetComponentInChildren<CGHudScript>();
		splatters = new List<GameObject>();
		iniRot = transform.rotation;
	}

	// Update is called once per frame
	void FixedUpdate () {
	
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		/*
		 
	if (Input.touches > 1)
	{
	//color wheel
	}
	else if (Input1`






*/


		if (Input.GetKeyDown(KeyCode.Space) && bucketAmount >= 25)
		//if (Input.touches.Length > 2 && bucketAmount >= 25)
		{
			GameObject splatter = (GameObject)Resources.Load("Splatter");
			splatter.GetComponent<Splatter> ().color = bucketColor;

			GameObject splatterObj = (GameObject)Instantiate(splatter);
			splatterObj.transform.position = this.transform.position;
			//splatterObj.transform.localScale = new Vector3 (1, 1, 1);

			bucketAmount -= 25;
				
		}	

		Color avgRGB = new Color (0, 0, 0);

		if (!Input.GetKey (KeyCode.LeftControl)) {

			//if (Input.touches.Length == 1) {
			float moveX = 0;
			float moveY = 0;
			moveX = Input.GetAxis ("Horizontal");
			moveY = Input.GetAxis ("Vertical");
			//camera.unproject

			/*if (Input.touches [0].position.x > this.transform.position.x) {
				moveX = 1;
			}
			if (Input.touches [0].position.y > this.transform.position.y) {
				moveY = 1;
			}	
			if (Input.touches [0].position.x < this.transform.position.x) {
				moveX = -1;
			}	
			if (Input.touches [0].position.y < this.transform.position.y) {
				moveY = -1;
			}	*/


			if (splatters.Count == 0) {
				//CG force
				Vector2 CompressedPos = new Vector2 ((int)transform.position.x * 20, (int)transform.position.y * 20);
				//Debug.Log(CompressedPos.x + " " + CompressedPos.y);

				Lab avgLab = new Lab (0, 0, 0);
				for (int i = (int)CompressedPos.x - 3; i < (int)CompressedPos.x + 3; i++) {
					for (int j = (int)CompressedPos.y - 3; j < (int)CompressedPos.y + 3; j++) {
						if (PaintingController.CompressedLabArray.GetLength(0) > ((int)CompressedPos.x + (i - (int)CompressedPos.x)) && PaintingController.CompressedLabArray.GetLength(1)  > ((int)CompressedPos.y + (j - (int)CompressedPos.y)))
							avgLab += PaintingController.CompressedLabArray [(int)CompressedPos.x + (i - (int)CompressedPos.x), (int)CompressedPos.y + (j - (int)CompressedPos.y)];
					}
				}
				avgLab = avgLab / new Lab (25, 25, 25);

		
				avgRGB = ColorUtil.ConvertLABtoRGB (avgLab);
				avgRGB.r /= 255f; 
				avgRGB.g /= 255f; 
				avgRGB.b /= 255f;

				//Debug.Log (avgRGB.r + " " + avgRGB.g + " " + avgRGB.b); 
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
			//Debug.Log (chroma);

			CGHud.UpdateCGHud (-1*hue, avgRGB, chroma);


			hue = (hue / 180f * Mathf.PI);
			//Debug.Log (hue);

			//body.AddForce (new Vector2 ((float)Mathf.Cos (hue)*14, (float)Mathf.Sin (hue))*14, ForceMode2D.Force);

			//input force
			//Debug.Log("Move x/y: " + moveX + " " + moveY);
			//Debug.Log("hue X: " + (float)Mathf.Cos (hue) + " " + (float)Mathf.Sin (hue));
			//flipping y axis to fix 
			if (chroma != 0) {
				body.AddForce (new Vector2 (((moveX * 2) + ((float)Mathf.Cos (hue)) * 3), ((moveY * 2)) + (-1 * (float)Mathf.Sin (hue)) * 3), ForceMode2D.Impulse);
			} else {
				body.AddForce (new Vector2 ((moveX * 2), (moveY * 2)), ForceMode2D.Impulse);
			}



			if (body.velocity.magnitude > maxSpeed) {
				body.velocity = body.velocity.normalized * maxSpeed;
			}

		} else {
			body.velocity = Vector2.zero;
		}

		UpdateBucketHUD (avgRGB, bucketAmount);
		//face direction of moving by rotation

	}
	void LateUpdate() {
		transform.rotation = iniRot;
	}

	void UpdateBucketHUD(Color color, int amount){
		BucketHUD.GetComponent<Image> ().fillAmount = amount / 100f;
		BucketHUDBG.GetComponent<Image> ().color = bucketColor;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Bucket") {
			Destroy (coll.gameObject);
			Bucket bucket = coll.gameObject.GetComponent<Bucket> ();

			if (bucketAmount > 0) {

				bucketAmount += bucket.amount;
				bucketColor = ColorUtil.AvgColor (bucketColor, bucket.color);
			} else {
				bucketAmount = 25;
				bucketColor = bucket.color;
			}
		}

		if (coll.gameObject.name == "Door") {
			//end level
			//Destroy (coll.gameObject);
			SceneManager.LoadScene(0);
		}
	}	

}
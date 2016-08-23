using UnityEngine;
using System.Collections;

public class PaintingController : MonoBehaviour {

	//private ColorUtil colorUtil; 


	//private Texture2D painting = null;

	public static Lab[,] CompressedLabArray;
	public Sprite paintingSprite;
	public int compressionScale;


	// Use this for initialization
	void Start () {
		//get texture data
		SetSprite();
		CompressedLabArray = MakeCompressedLab (paintingSprite.texture);


	//	painting = textureFromSprite(sprite);
	//	Debug.Log (painting.ToString ());

	}

	Lab[,] MakeCompressedLab(Texture2D input)
	{
		Color[,] RgbArray = GetPixels (paintingSprite.texture);
		Lab[,] rawLabArray = RgbArrayToLabArray (RgbArray); 
		return CompressLabArray (rawLabArray);
	}


	// Update is called once per frame
	void Update () {
	
	}

	public static Texture2D textureFromSprite(Sprite sprite)
	{
		if(sprite.rect.width != sprite.texture.width){
			Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
				(int)sprite.textureRect.y, 
				(int)sprite.textureRect.width, 
				(int)sprite.textureRect.height );
			newText.SetPixels(newColors);
			newText.Apply();
			return newText;
		} else
			return sprite.texture;
	}
		
	void SetSprite()
	{
		gameObject.GetComponent<SpriteRenderer> ().sprite = paintingSprite;
	}

	Color[,] GetPixels(Texture2D input)
	{
		Color[] flatArray = input.GetPixels();
		Color[,] pixelArray = new Color[input.width, input.height];
		for (int i = 0; i < flatArray.Length; i++) {
			pixelArray [i % input.width, i / input.width] = flatArray [i];
		}

		return pixelArray;
	}


	Lab[,] RgbArrayToLabArray(Color[,] RgbArray)
	{
		Lab[,] rawLabArray= new Lab[RgbArray.GetLength(0), RgbArray.GetLength(1)];
		for (int i = 0; i < RgbArray.GetLength (0); i++) 
		{
			for (int j = 0; j < RgbArray.GetLength (1); j++) 
			{
				rawLabArray[i,j] = ColorUtil.ConvertRGBtoLAB (RgbArray[i,j]);
			}
		}
		return rawLabArray;
	}


	Lab[,] CompressLabArray (Lab[,] rawLabArray)
	{
		Lab[,] LabArray = new Lab[rawLabArray.GetLength(0)/5, rawLabArray.GetLength(1)/5];
		Debug.Log (rawLabArray.GetLength (0) + "  " + LabArray.GetLength (0));
		//for every 5 by 5 box going from  column top to bottom then row left to right
		for (int i = 0; i < rawLabArray.GetLength (0)-5; i=i+5) 
		{
			for (int j = 0; j < rawLabArray.GetLength (1)-5; j=j+5) 
			{
				Lab comps = new Lab(0,0,0); 
				//for each box, 
				for (int x = 0; x < 5; x++) 
				{
					for (int y = 0; y < 5; y++)
					{
						comps += rawLabArray [i + x, j + y];
					}
				}
				LabArray [i / 5, j / 5] = (comps / (new Lab (25, 25, 25)));//making not decimal
			}
		}
		Debug.Log (LabArray [25, 26].L + " " + LabArray [25, 26].a + " " + LabArray [25, 26].b + " ");
		return LabArray;
	}
}

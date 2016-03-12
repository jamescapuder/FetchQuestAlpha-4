using UnityEngine;
using System.Collections;

public class TownStructModel : MonoBehaviour {

	private TownStructure own;

	private Material mat;
	private Renderer rend;
	// Use this for initialization
	public void init(TownStructure owner, int TownStructure){
		this.own = owner;


		transform.parent = own.transform;					// Set the model's parent to the gem.
		System.Random rnd = new System.Random();

	//	Camera camera = own.parentGameManager.GetComponent<Camera>();

		//transform.localPosition = camera.ScreenToWorldPoint(new Vector3(x,y,0));		// Center the model on the parent.
		Vector3 p = own.transform.position;
		p = Camera.main.ScreenToWorldPoint (p);
		print (p);
		p.z = -1;
		transform.localPosition = new Vector3 (0f, 0.2f, -1.0f);
		//owner.GetComponent<BoxCollider2D> ().offset = p;

		name = "Struct Model";									// Name the object.



		mat = GetComponent<Renderer>().material;								// Get the material component of this quad object.
		mat.mainTexture = Resources.Load<Texture2D>("TextureFold/361_structure2");	// Set the texture.  Must be in Resources folder.
		mat.color = new Color(1f,1f,1);											// Set the color (easy way to tint things).
		mat.shader = Shader.Find ("Sprites/Default");	

		rend = GetComponent<Renderer> ();
		rend.sortingLayerName = "buildingLayer";


	}
}

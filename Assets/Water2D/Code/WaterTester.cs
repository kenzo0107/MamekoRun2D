using UnityEngine;
using System.Collections;

public class WaterTester : MonoBehaviour {


	public Water2D water;
	public float force;
	public int size = 0;
	
	//public float waterHeightChanger = 100;
	
	public GameObject objectToInstantiate;
	
	void Awake()
	{
		//Physics.gravity = new Vector3(0,-500,0);	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 touchPosition =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, water.transform.position.z - transform.position.z));
			
			if (objectToInstantiate != null)
			{
				GameObject instatiatedObject =  Instantiate(objectToInstantiate, touchPosition+Vector3.forward*10 , Quaternion.identity) as GameObject;
				instatiatedObject.transform.eulerAngles = new Vector3(0,-180, Random.Range(0,360));
				instatiatedObject.transform.localScale = Vector3.one*Random.Range(0.5f,1.3f);
				if (instatiatedObject.rigidbody != null)
					instatiatedObject.rigidbody.mass = Mathf.Lerp(0.1f,1f,Mathf.InverseLerp(0.5f,1.3f, instatiatedObject.transform.localScale.x));
				else
					instatiatedObject.rigidbody2D.mass = Mathf.Lerp(0.1f,1f,Mathf.InverseLerp(0.5f,1.3f, instatiatedObject.transform.localScale.x));
			}
			else
			{
				water.ObjectEnteredWater(touchPosition, force,size, true);
			}
			
		}
	//water.SetHeight(waterHeightChanger);
	
	}
}

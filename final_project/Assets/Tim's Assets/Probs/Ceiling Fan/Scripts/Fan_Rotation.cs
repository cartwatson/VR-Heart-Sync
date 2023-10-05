using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Fan_Rotation : MonoBehaviour {

	//variable for speed
	public float speed=75;


	
void Update () {
		
		//rotation around z

		transform.Rotate((new Vector3(0,0,1)) * Time.deltaTime*speed); 
	}
}

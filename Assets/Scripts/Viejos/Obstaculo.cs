using UnityEngine;
using System.Collections;

public class Obstaculo : MonoBehaviour 
{
	public float ReduccionVel = 0;
	public float TiempEmpDesapa = 1;
	float Tempo1 = 0;
	public float TiempDesapareciendo = 1;
	float Tempo2 = 0;
	public string PlayerTag = "Player";
	
	bool Chocado = false;
	bool Desapareciendo = false;

	Rigidbody rig;
	Collider collGo;
	// Use this for initialization
	void Start () 
	{
		rig = GetComponent<Rigidbody>();
		collGo = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Chocado)
		{
			Tempo1 += Time.deltaTime;
			if(Tempo1 > TiempEmpDesapa)
			{
				Chocado = false;
				Desapareciendo = true;
				rig.useGravity = false;
				collGo.enabled = false;
			}
		}
		
		if(Desapareciendo)
		{
			Tempo2 += Time.deltaTime;
			if(Tempo2 > TiempDesapareciendo)
			{
				gameObject.SetActiveRecursively(false);
			}
		}
	}
	
	void OnCollisionEnter(Collision coll)
	{
		if(coll.transform.tag == PlayerTag)
		{
			Chocado = true;
		}
	}
}

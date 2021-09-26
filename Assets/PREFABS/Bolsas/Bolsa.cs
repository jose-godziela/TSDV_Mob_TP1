using UnityEngine;

public class Bolsa : MonoBehaviour
{
	public Pallet.Valores Monto;
	//public int IdPlayer = 0;
	public string TagPlayer = "";
	public Texture2D ImagenInventario;
	Player Pj = null;
	
	bool Desapareciendo;
	public GameObject Particulas;
	public float TiempParts = 2.5f;


	Renderer renderBolsa;
	Collider collBolsa;
	ParticleSystem parSystem;

	void Start () 
	{
		Monto = Pallet.Valores.Valor2;
		
		
		if(Particulas != null)
			Particulas.SetActive(false);

		renderBolsa = GetComponent<Renderer>();
		collBolsa = GetComponent<Collider>();
		parSystem = Particulas.GetComponent<ParticleSystem>();
	}
	
	void Update ()
	{
		
		if(Desapareciendo)
		{
			TiempParts -= Time.deltaTime;
			if(TiempParts <= 0)
			{
				renderBolsa.enabled = true;
				collBolsa.enabled = true;

				parSystem.Stop();
				gameObject.SetActive(false);
			}
		}
		
	}
	
	void OnTriggerEnter(Collider coll)
	{
		if(coll.tag == TagPlayer)
		{
			Pj = coll.GetComponent<Player>();
			//if(IdPlayer == Pj.IdPlayer)
			//{
				if(Pj.AgregarBolsa(this))
					Desaparecer();
			//}
		}
	}
	
	public void Desaparecer()
	{
		parSystem.Play();
		Desapareciendo = true;
		
		renderBolsa.enabled = false;
		collBolsa.enabled = false;
		
		if(Particulas != null)
		{
			parSystem.Play();
		}
	
	}
}

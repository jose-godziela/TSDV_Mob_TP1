using UnityEngine;

public class ContrCalibracion : MonoBehaviour
{
	public Player Pj;

	public float TiempEspCalib = 3;
	float Tempo2 = 0;
	
	public enum Estados{Calibrando, Tutorial, Finalizado}
	public Estados EstAct = Estados.Calibrando;
	
	public ManejoPallets Partida;
	public ManejoPallets Llegada;
	public Pallet P;
    public ManejoPallets palletsMover;
	
	GameManager GM;

	//----------------------------------------------------//
	Renderer partidaRenderer;
	Renderer llegadaRenderer;
	Collider partidaCollider;
	Collider llegadaCollider;
	Renderer pRenderer;

	void Start () 
	{
        palletsMover.enabled = false;
        Pj.ContrCalib = this;
		
		GM = GameObject.Find("GameMgr").GetComponent<GameManager>();
		
		P.CintaReceptora = Llegada.gameObject;
		Partida.Recibir(P);

		partidaRenderer = Partida.GetComponent<Renderer>();
		partidaCollider = Partida.GetComponent<Collider>();
		llegadaRenderer = Llegada.GetComponent<Renderer>();
		llegadaCollider = Llegada.GetComponent<Collider>();
		pRenderer = P.GetComponent<Renderer>();

		SetActivComp(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(EstAct == ContrCalibracion.Estados.Tutorial)
		{
			if(Tempo2 < TiempEspCalib)
			{
				Tempo2 += Time.deltaTime;
				if(Tempo2 > TiempEspCalib)
				{
					 SetActivComp(true);
				}
			}
		}
	}

	public void IniciarTesteo()
	{
		EstAct = ContrCalibracion.Estados.Tutorial;
        palletsMover.enabled = true;
    }
	
	public void FinTutorial()
	{
		EstAct = ContrCalibracion.Estados.Finalizado;
        palletsMover.enabled = false;
        GM.FinCalibracion(Pj.IdPlayer);
	}
	
	void SetActivComp(bool estado)
	{
		if(partidaRenderer != null)
			partidaRenderer.enabled = estado;
		partidaCollider.enabled = estado;
		if(llegadaRenderer != null)
			llegadaRenderer.enabled = estado;
		llegadaCollider.enabled = estado;
		pRenderer.enabled = estado;
	}
}

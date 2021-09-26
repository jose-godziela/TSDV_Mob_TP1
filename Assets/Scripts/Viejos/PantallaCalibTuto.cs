using UnityEngine;
using UnityEngine.UI;

public class PantallaCalibTuto : MonoBehaviour 
{
	//public Texture2D[] ImagenesDelTuto;
	//-------------------------------
	public Sprite[] ImagenesDelTutoImg;
	//-------------------------------
	public float Intervalo = 1.2f;//tiempo de cada cuanto cambia de imagen
	float TempoIntTuto = 0;
	int EnCursoTuto = 0;
	
	//public Texture2D[] ImagenesDeCalib;
	//-------------------------------
	public Sprite[] ImagenesDeCalibImg;
	//-------------------------------

	int EnCursoCalib = 0;
	float TempoIntCalib = 0;
	
	//public Texture2D ImaReady;
	//-------------------------------
	public Sprite ImaReady;
	
	public ContrCalibracion ContrCalib;

	public SpriteRenderer MainImage;

	// Update is called once per frame
	void Update () 
	{
		switch(ContrCalib.EstAct)
		{
		case ContrCalibracion.Estados.Calibrando:
			//pongase en posicion para iniciar
			TempoIntCalib += Time.deltaTime;
			if(TempoIntCalib >= Intervalo)
			{
				TempoIntCalib = 0;
				if(EnCursoCalib + 1 < ImagenesDeCalibImg.Length)
					EnCursoCalib++;
				else
					EnCursoCalib = 0;
			}
			//GetComponent<Renderer>().material.mainTexture = ImagenesDeCalib[EnCursoCalib];
			MainImage.sprite = ImagenesDeCalibImg[EnCursoCalib];

			break;
			
		case ContrCalibracion.Estados.Tutorial:
			//tome la bolsa y depositela en el estante
			TempoIntTuto += Time.deltaTime;
			if(TempoIntTuto >= Intervalo)
			{
				TempoIntTuto = 0;
				if(EnCursoTuto + 1 < ImagenesDelTutoImg.Length)
					EnCursoTuto++;
				else
					EnCursoTuto = 0;
			}
			//GetComponent<Renderer>().material.mainTexture = ImagenesDelTuto[EnCursoTuto];
			MainImage.sprite = ImagenesDelTutoImg[EnCursoTuto];

			break;
			
		case ContrCalibracion.Estados.Finalizado:
			//esperando al otro jugador		
			//GetComponent<Renderer>().material.mainTexture = ImaReady;
			MainImage.sprite = ImaReady;

			break;
		}
			
			
	}
}

using UnityEngine;
using UnityEngine.UI;

public class LoopTextura : MonoBehaviour 
{
	public float Intervalo = 1;
	float Tempo = 0;
	
	public Sprite[] Imagenes;
	public Image CanvasImage;
	int Contador = 0;

	// Use this for initialization
	void Start () 
	{
		CanvasImage = GetComponent<Image>();
		if (Imagenes.Length > 0)
        {
			CanvasImage.sprite = Imagenes[0];
			//GetComponent<Renderer>().material.mainTexture = Imagenes[0];
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		Tempo += Time.deltaTime;
		
		if(Tempo >= Intervalo)
		{
			Tempo = 0;
			Contador++;
			if(Contador >= Imagenes.Length)
			{
				Contador = 0;
			}
			CanvasImage.sprite = Imagenes[Contador];
			//GetComponent<Renderer>().material.mainTexture = Imagenes[Contador];
		}
	}
}

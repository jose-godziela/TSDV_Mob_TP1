using UnityEngine;

public class ControlDireccion : MonoBehaviour 
{
	public bool Habilitado = true;
	public string PlayerNumber;
	float Giro;

	InputCamion _input;

    private void Start()
    {
		Giro = 0;
		_input = InputManager.Instance.GetInput(PlayerNumber);
	}

    void Update () 
	{
		Giro = _input.GetHorizontal();

		if (!Habilitado)
			return;

		gameObject.SendMessage("SetGiro", Giro);
	}

	public float GetGiro()
	{
		return Giro;
	}
}

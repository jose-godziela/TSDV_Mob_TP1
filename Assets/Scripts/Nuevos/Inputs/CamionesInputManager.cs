using UnityEngine;

//FACTORY PATTERN
public class CamionesInputManager : InputManager
{
    protected override InputCamion CreateInput(string player)
    {
        InputCamion input = null;

#if UNITY_EDITOR
        input = new CamionInputTouchKeys(player);
#elif UNITY_ANDROID
        input = new CamionInputTouch(player);
#else
        input = new CamionInputKeys(player);
#endif

        return input;
    }

    public string Player1;
    public string Player2;

    public InputCamion player1;
    public InputCamion player2;

    private void Start()
    {
        player1 = GetInput(Player1);
        player2 = GetInput(Player2);
    }
}
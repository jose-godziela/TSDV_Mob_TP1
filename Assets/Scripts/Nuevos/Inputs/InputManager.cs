using UnityEngine;
using System.Collections.Generic;

public abstract class InputManager : MonoBehaviour
{
    static InputManager instance = null;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InputManager>();
            return instance;
        }
    }

    List<InputCamion> inputs = new List<InputCamion>();
    public InputCamion GetInput(string player)
    {
        var input = inputs.Find(inp => inp.Player == player);

        if(input == null)
        {
            input = CreateInput(player);
            inputs.Add(input);
        }

        return input;
    }

    protected abstract InputCamion CreateInput(string player);

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
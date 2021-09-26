using UnityEngine;

public abstract class InputCamion
{
    public enum Buttons
    {
        Start,
        Left,
        Right,
        Down
    }

    public string Player = "";
    public void SetPlayer(string player)
    {
        Player = player;
    }

    public abstract bool GetButton(Buttons button);
    public abstract float GetHorizontal();

    public abstract void SetHorizontal(float val);
}

public class CamionInputKeys : InputCamion
{
    string ButtonToString(Buttons button)
    {
        switch (button)
        {
            case Buttons.Start:
                return $"Start{Player}";
            case Buttons.Left:
                return $"Left{Player}";
            case Buttons.Right:
                return $"Right{Player}";
            case Buttons.Down:
                return $"Down{Player}";
            default:
                return "";
        }
    }

    public CamionInputKeys(string player)
    {
        SetPlayer(player);
    }

    public override bool GetButton(Buttons button)
    {
        string btnStr = ButtonToString(button);
        return Input.GetButton(btnStr);
    }

    public override float GetHorizontal()
    {
        return Input.GetAxis($"Horizontal{Player}");
    }

    public override void SetHorizontal(float val)
    {
        //.............
    }
}

public class CamionInputTouch : InputCamion
{
    public CamionInputTouch(string player)
    {
        SetPlayer(player);
    }

    public override bool GetButton(Buttons button)
    {
        return false;
    }

    public override float GetHorizontal()
    {
        return horizontal;
    }

    float horizontal;
    public override void SetHorizontal(float val)
    {
        horizontal = Mathf.Clamp(val,-1,1);
    }
}

public class CamionInputTouchKeys : InputCamion
{
    CamionInputKeys camionInputsKeys;
    CamionInputTouch camionInputsTouchs;
    
    public CamionInputTouchKeys(string player)
    {
        camionInputsKeys = new CamionInputKeys(player);
        camionInputsTouchs = new CamionInputTouch(player);

        SetPlayer(player);
    }

    public override bool GetButton(Buttons button)
    {
        return camionInputsKeys.GetButton(button);
    }

    public override float GetHorizontal()
    {
        return camionInputsTouchs.GetHorizontal() + camionInputsKeys.GetHorizontal();
    }

    public override void SetHorizontal(float val)
    {
        camionInputsTouchs.SetHorizontal(val);
    }
}
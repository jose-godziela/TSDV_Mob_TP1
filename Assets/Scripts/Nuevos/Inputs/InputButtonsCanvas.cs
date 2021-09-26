using UnityEngine;

public class InputButtonsCanvas : MonoBehaviour
{
    public PalletMover palletMoverDeploy;
    public PalletMover palletMoverCalibration;
    GameManager refGm;

    public bool activateInputsTuto = false;

    private void Start()
    {
        refGm = FindObjectOfType<GameManager>();
    }

    //-------------------------------------------------------------
    public void StartSinglePlayer()
    {
        refGm?.SetSinglePlayer();
        activateInputsTuto = true;
    }
    public void StartPlayer1Multi()
    {
        refGm?.SetPlayer1Multi();
        activateInputsTuto = true;
    }
    public void StartPlayer2Multi()
    {
        refGm?.SetPlayer2Multi();
        activateInputsTuto = true;
    }
    //-------------------------------------------------------------
    //=============================================================
    //-------------------------------------------------------------
    public void FirstStepDeploy()
    {
        if (!palletMoverDeploy.Tenencia() && palletMoverDeploy.Desde.Tenencia())
            palletMoverDeploy.PrimerPaso();
    }

    public void FirstStepCalibration()
    {
        palletMoverCalibration.PrimerPaso();
    }
    //-------------------------------------------------------------
    public void SecondStepDeploy()
    {
        if (palletMoverDeploy.Tenencia())
            palletMoverDeploy.SegundoPaso();
    }

    public void SecondStepCalibration()
    {
        palletMoverCalibration.SegundoPaso();
    }

    //-------------------------------------------------------------
    public void ThirdStepDeploy()
    {
        if(palletMoverDeploy.segundoCompleto && palletMoverDeploy.Tenencia())
            palletMoverDeploy.TercerPaso();
    }

    public void ThirdStepCalibration()
    {
        palletMoverCalibration.TercerPaso();
    }
    //-------------------------------------------------------------
}

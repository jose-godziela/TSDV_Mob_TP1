using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletMover : ManejoPallets {

    public string PlayerInput;

    public ManejoPallets Desde, Hasta;
    public bool segundoCompleto = false;

    private void Update() 
    {
        if (!Tenencia() && Desde.Tenencia() && InputManager.Instance.GetInput(PlayerInput).GetButton(InputCamion.Buttons.Left))
        {
            PrimerPaso();
        }
        if (Tenencia() && InputManager.Instance.GetInput(PlayerInput).GetButton(InputCamion.Buttons.Down))
        {
            SegundoPaso();
        }
        if (segundoCompleto && Tenencia() && InputManager.Instance.GetInput(PlayerInput).GetButton(InputCamion.Buttons.Right))
        {
            TercerPaso();
        }
    }

    public void PrimerPaso() {
        Desde.Dar(this);
        segundoCompleto = false;
    }
    public void SegundoPaso() {
        base.Pallets[0].transform.position = transform.position;
        segundoCompleto = true;
    }
    public void TercerPaso() {
        Dar(Hasta);
        segundoCompleto = false;
    }

    public override void Dar(ManejoPallets receptor) {
        if (Tenencia()) {
            if (receptor.Recibir(Pallets[0])) {
                Pallets.RemoveAt(0);
            }
        }
    }
    public override bool Recibir(Pallet pallet) {
        if (!Tenencia()) {
            pallet.Portador = this.gameObject;
            base.Recibir(pallet);
            return true;
        }
        else
            return false;
    }
}

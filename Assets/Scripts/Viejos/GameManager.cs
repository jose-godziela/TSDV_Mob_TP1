using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //public static Player[] Jugadoers;

    public static GameManager Instancia;

    public float TiempoDeJuego = 180;

    public enum EstadoJuego { Calibrando, Jugando, Finalizado }
    public EstadoJuego EstAct = EstadoJuego.Calibrando;

    public enum ModoDeJuego { SinglePlayer, LocalMultiplayer }
    public ModoDeJuego ModoActual;

    public PlayerInfo PlayerInfo1 = null;
    public PlayerInfo PlayerInfo2 = null;

    public Player Player1;
    public Player Player2;

    Frenado player1Frenado;
    Frenado player2Frenado;
    ControlDireccion player1ContDireccion;
    ControlDireccion player2ContDireccion;
    Visualizacion player1Visualizacion;
    Visualizacion player2Visualizacion;

    //mueve los esqueletos para usar siempre los mismos
    public Transform Esqueleto1;
    public Transform Esqueleto2;
    //public Vector3[] PosEsqsCalib;
    public Vector3[] PosEsqsCarrera;

    bool PosSeteada = false;

    bool ConteoRedresivo = true;
    public Rect ConteoPosEsc;
    public float ConteoParaInicion = 3;
    public GUISkin GS_ConteoInicio;

    public Rect TiempoGUI = new Rect();
    public GUISkin GS_TiempoGUI;
    Rect R = new Rect();

    public float TiempEspMuestraPts = 3;

    //posiciones de los camiones dependientes del lado que les toco en la pantalla
    //la pos 0 es para la izquierda y la 1 para la derecha
    public Vector3[] PosCamionesCarrera = new Vector3[3];
    //posiciones de los camiones para el tutorial
    public Vector3 PosCamion1Tuto = Vector3.zero;
    public Vector3 PosCamion2Tuto = Vector3.zero;

    //listas de GO que activa y desactiva por sub-escena
    //escena de calibracion
    public GameObject[] ObjsCalibracion1;
    public GameObject[] ObjsCalibracion2;
    //escena de tutorial
    public GameObject[] ObjsTuto1;
    public GameObject[] ObjsTuto2;
    //la pista de carreras
    public GameObject[] ObjsCarrera;
    //de las descargas se encarga el controlador de descargas


    IList<int> users;

    //--------------------------------------------------------//

    void Awake()
    {
        GameManager.Instancia = this;
    }

    void Start()
    {
        IniciarCalibracion();

        SearchPlayerComponents();
    }

    void Update()
    {
        //CIERRA LA APLICACION
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.Get()?.QuitGame();
        }

        switch (EstAct)
        {
            case EstadoJuego.Calibrando:

                switch (ModoActual)
                {
                    case ModoDeJuego.SinglePlayer:

                        if (Input.GetKey(KeyCode.Mouse0) &&
                        Input.GetKey(KeyCode.Keypad0))
                        {
                            if (PlayerInfo1 != null && PlayerInfo2 != null)
                            {
                                FinCalibracion(0);

                                FinTutorial(0);
                            }
                        }

                        //Nuevo
                        if (PlayerInfo1.PJ == null && InputManager.Instance.GetInput("1").GetButton(InputCamion.Buttons.Start))
                        {
                            SetSinglePlayer();
                        }
                        //Antiguo
                        //if (PlayerInfo1.PJ == null && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
                        //{
                        //    PlayerInfo1 = new PlayerInfo(0, Player1);
                        //    PlayerInfo1.LadoAct = Visualizacion.Lado.Centro;
                        //    SetPosicion(PlayerInfo1);
                        //}

                        if (PlayerInfo1.PJ != null)
                        {
                            if (PlayerInfo1.FinTuto2)
                            {
                                EmpezarCarrera();
                            }
                        }

                        break;
                    case ModoDeJuego.LocalMultiplayer:

                        //SKIP EL TUTORIAL
                        if (Input.GetKey(KeyCode.Mouse0) &&
                        Input.GetKey(KeyCode.Keypad0))
                        {
                            if (PlayerInfo1 != null && PlayerInfo2 != null)
                            {
                                FinCalibracion(0);
                                FinCalibracion(1);

                                FinTutorial(0);
                                FinTutorial(1);
                            }
                        }

                        if (PlayerInfo1.PJ == null && Input.GetKeyDown(KeyCode.W))
                        {
                            SetPlayer1Multi();
                        }

                        if (PlayerInfo2.PJ == null && Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            SetPlayer2Multi();
                        }

                        //cuando los 2 pj terminaron los tutoriales empiesa la carrera
                        if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
                        {
                            if (PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
                            {
                                EmpezarCarrera();
                            }
                        }

                        break;
                }               
                break;

            case EstadoJuego.Jugando:

                //SKIP LA CARRERA
                if (Input.GetKey(KeyCode.Mouse1) &&
                   Input.GetKey(KeyCode.Keypad0))
                {
                    TiempoDeJuego = 0;
                }

                if (TiempoDeJuego <= 0)
                {
                    FinalizarCarrera();
                }

                if (ConteoRedresivo)
                {
                    ConteoParaInicion -= Time.deltaTime;
                    if (ConteoParaInicion < 0)
                    {
                        EmpezarCarrera();
                        ConteoRedresivo = false;
                        
                    }
                }
                else
                {
                    //baja el tiempo del juego
                    TiempoDeJuego -= Time.deltaTime;
                }

                break;


            case EstadoJuego.Finalizado:

                switch (ModoActual)
                {
                    case ModoDeJuego.SinglePlayer:
                        SceneLoader.Get()?.GoFinalPointsSinglePlayer();
                        TransferScores.Instance?.SaveScorePlayer1(Player1.Dinero);
                        break;
                    case ModoDeJuego.LocalMultiplayer:
                        SceneLoader.Get()?.GoFinalPointsMultiLocal();
                        TransferScores.Instance?.SaveScorePlayer1(Player1.Dinero);
                        TransferScores.Instance?.SaveScorePlayer2(Player2.Dinero);
                        break;
                }

                break;
        }
    }
    
    void SearchPlayerComponents()
    {
        player1Frenado = Player1.GetComponent<Frenado>();
        player2Frenado = Player2.GetComponent<Frenado>();
        player1ContDireccion = Player1.GetComponent<ControlDireccion>();
        player2ContDireccion = Player2.GetComponent<ControlDireccion>();
        player1Visualizacion = Player1.GetComponent<Visualizacion>();
        player2Visualizacion = Player2.GetComponent<Visualizacion>();
    }
    
    public void SetSinglePlayer()
    {
        PlayerInfo1 = new PlayerInfo(0, Player1);
        PlayerInfo1.LadoAct = Visualizacion.Lado.Centro;
        SetPosicion(PlayerInfo1);
    }

    public void SetPlayer1Multi()
    {
        PlayerInfo1 = new PlayerInfo(0, Player1);
        PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
        SetPosicion(PlayerInfo1);
    }

    public void SetPlayer2Multi()
    {
        PlayerInfo2 = new PlayerInfo(1, Player2);
        PlayerInfo2.LadoAct = Visualizacion.Lado.Der;
        SetPosicion(PlayerInfo2);
    }
    //----------------------------------------------------------//

    public void IniciarCalibracion()
    {
        for (int i = 0; i < ObjsCalibracion1.Length; i++)
        {
            ObjsCalibracion1[i].SetActiveRecursively(true);
            ObjsCalibracion2[i].SetActiveRecursively(true);
        }

        for (int i = 0; i < ObjsTuto2.Length; i++)
        {
            ObjsTuto2[i].SetActiveRecursively(false);
            ObjsTuto1[i].SetActiveRecursively(false);
        }

        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActiveRecursively(false);
        }

        switch (ModoActual)
        {
            case ModoDeJuego.SinglePlayer:
                Player1.CambiarACalibracion();
                break;
            case ModoDeJuego.LocalMultiplayer:
                Player1.CambiarACalibracion();
                Player2.CambiarACalibracion();
                break;
        }
    }

    void EmpezarCarrera()
    {
        player1Frenado.RestaurarVel();
        player1ContDireccion.Habilitado = true;

        player2Frenado.RestaurarVel();
        player2ContDireccion.Habilitado = true;
    }

    void FinalizarCarrera()
    {
        EstAct = EstadoJuego.Finalizado;

        TiempoDeJuego = 0;

        switch (ModoActual)
        {
            case ModoDeJuego.SinglePlayer:

                if (PlayerInfo1.LadoAct == Visualizacion.Lado.Centro)
                    DatosPartida.LadoGanadaor = DatosPartida.Lados.None;

                DatosPartida.PtsGanador = Player1.Dinero;
                player1Frenado.Frenar();
                Player1.ContrDesc.FinDelJuego();

                break;
            case ModoDeJuego.LocalMultiplayer:

                if (Player1.Dinero > Player2.Dinero)
                {
                    //lado que gano
                    if (PlayerInfo1.LadoAct == Visualizacion.Lado.Der)
                        DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
                    else
                        DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;

                    //puntajes
                    DatosPartida.PtsGanador = Player1.Dinero;
                    DatosPartida.PtsPerdedor = Player2.Dinero;
                }
                else
                {
                    //lado que gano
                    if (PlayerInfo2.LadoAct == Visualizacion.Lado.Der)
                        DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
                    else
                        DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;

                    //puntajes
                    DatosPartida.PtsGanador = Player2.Dinero;
                    DatosPartida.PtsPerdedor = Player1.Dinero;
                }

                player1Frenado.Frenar();
                player2Frenado.Frenar();

                Player1.ContrDesc.FinDelJuego();
                Player2.ContrDesc.FinDelJuego();
                break;
        }
    }

    //se encarga de posicionar la camara derecha para el jugador que esta a la derecha y viseversa
    void SetPosicion(PlayerInfo pjInf)
    {
        pjInf.PJ.GetComponent<Visualizacion>().SetLado(pjInf.LadoAct);
        //en este momento, solo la primera vez, deberia setear la otra camara asi no se superponen
        pjInf.PJ.ContrCalib.IniciarTesteo();
        PosSeteada = true;

        if(Instancia != null)
        {
            if(ModoActual == ModoDeJuego.SinglePlayer)
            {
                player1Visualizacion.SetLado(Visualizacion.Lado.Centro);
                return;
            }
        }

        if (pjInf.PJ == Player1)
        {
            if (pjInf.LadoAct == Visualizacion.Lado.Izq)
                player2Visualizacion.SetLado(Visualizacion.Lado.Der);
            else
                player2Visualizacion.SetLado(Visualizacion.Lado.Izq);
        }
        else
        {
            if (pjInf.LadoAct == Visualizacion.Lado.Izq)
                player1Visualizacion.SetLado(Visualizacion.Lado.Der);
            else
                player1Visualizacion.SetLado(Visualizacion.Lado.Izq);
        }

    }

    void CambiarACarrera()
    {
        Esqueleto1.transform.position = PosEsqsCarrera[0];
        Esqueleto2.transform.position = PosEsqsCarrera[1];

        for (int i = 0; i < ObjsCarrera.Length; i++)
        {
            ObjsCarrera[i].SetActiveRecursively(true);
        }


        //desactivacion de la calibracion
        PlayerInfo1.FinCalibrado = true;

        for (int i = 0; i < ObjsTuto1.Length; i++)
        {
            ObjsTuto1[i].SetActiveRecursively(true);
        }

        for (int i = 0; i < ObjsCalibracion1.Length; i++)
        {
            ObjsCalibracion1[i].SetActiveRecursively(false);
        }

        PlayerInfo2.FinCalibrado = true;

        for (int i = 0; i < ObjsCalibracion2.Length; i++)
        {
            ObjsCalibracion2[i].SetActiveRecursively(false);
        }

        for (int i = 0; i < ObjsTuto2.Length; i++)
        {
            ObjsTuto2[i].SetActiveRecursively(true);
        }

        switch (ModoActual) 
        {
            case ModoDeJuego.SinglePlayer:

                if (PlayerInfo1.LadoAct == Visualizacion.Lado.Centro)
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[2];
                }

                Player1.transform.forward = Vector3.forward;
                player1Frenado.Frenar();
                Player1.CambiarAConduccion();

                player1Frenado.RestaurarVel();
                //player1ContDireccion.Habilitado = false;

                Player1.transform.forward = Vector3.forward;
                EstAct = EstadoJuego.Jugando;
                break;
            case ModoDeJuego.LocalMultiplayer:

                //posiciona los camiones dependiendo de que lado de la pantalla esten
                if (PlayerInfo1.LadoAct == Visualizacion.Lado.Izq)
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[0];
                    Player2.gameObject.transform.position = PosCamionesCarrera[1];
                }
                else
                {
                    Player1.gameObject.transform.position = PosCamionesCarrera[1];
                    Player2.gameObject.transform.position = PosCamionesCarrera[0];
                }

                Player1.transform.forward = Vector3.forward;
                player1Frenado.Frenar();
                Player1.CambiarAConduccion();

                Player2.transform.forward = Vector3.forward;
                player2Frenado.Frenar();
                Player2.CambiarAConduccion();

                //keeps moving them
                player1Frenado.RestaurarVel();
                player2Frenado.RestaurarVel();
                
                player1ContDireccion.Habilitado = false;
                player2ContDireccion.Habilitado = false;
                
                Player1.transform.forward = Vector3.forward;
                Player2.transform.forward = Vector3.forward;

                EstAct = GameManager.EstadoJuego.Jugando;
                break;
        }
    }

    public void FinTutorial(int playerID)
    {
        if(ModoActual == ModoDeJuego.LocalMultiplayer)
        {
            if (playerID == 0)
            {
                PlayerInfo1.FinTuto2 = true;

            }
            else if (playerID == 1)
            {
                PlayerInfo2.FinTuto2 = true;
            }

            if (PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
            {
                CambiarACarrera();
            }
        }
        else
        {
            if (playerID == 0)
            {
                PlayerInfo1.FinTuto2 = true;
            }
            if (PlayerInfo1 != null && PlayerInfo1.FinTuto2)
            {
                CambiarACarrera();
            }
        }
    }

    public void FinCalibracion(int playerID)
    {
        if(ModoActual == ModoDeJuego.LocalMultiplayer)
        {
            if (playerID == 0)
            {
                PlayerInfo1.FinTuto1 = true;

            }
            else if (playerID == 1)
            {
                PlayerInfo2.FinTuto1 = true;
            }

            if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
                if (PlayerInfo1.FinTuto1 && PlayerInfo2.FinTuto1)
                    CambiarACarrera();//CambiarATutorial();
        }
        else
        {
            if (playerID == 0)
            {
                PlayerInfo1.FinTuto1 = true;
            }

            if (PlayerInfo1.PJ != null)
                if (PlayerInfo1.FinTuto1)
                    CambiarACarrera();//CambiarATutorial();
        }
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public PlayerInfo(int tipoDeInput, Player pj)
        {
            TipoDeInput = tipoDeInput;
            PJ = pj;
        }

        public bool FinCalibrado = false;
        public bool FinTuto1 = false;
        public bool FinTuto2 = false;

        public Visualizacion.Lado LadoAct;

        public int TipoDeInput = -1;

        public Player PJ;
    }
}

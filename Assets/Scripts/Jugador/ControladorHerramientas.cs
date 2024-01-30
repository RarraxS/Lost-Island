using UnityEngine;
using UnityEngine.Tilemaps;

public class ControladorHerramientas : MonoBehaviour
{
    private Jugador personaje;
    private Rigidbody2D rb;

    [SerializeField] private float offsetDistance = 1f;

    // Marcar casilla actual --------------------------------------------

    [SerializeField] private float tama�oAreaInteractuable = 1.2f;
    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private TileMapReadController tileMapReadController;
    [SerializeField] private float distanciaMaxima;

    // Arar --------------------------------------------------------------

    [SerializeField] private TileBase piezaArada;
    [SerializeField] private Tilemap arado;
    [SerializeField] private int energiaArar;

    //--------------------------------------------------------------------


    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;


    private static ControladorHerramientas instance;
    public static ControladorHerramientas Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        personaje = GetComponent<Jugador>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        TileSeleccionado();
        Mostrar();
        Marker();

        Arar();
        if (Input.GetMouseButtonDown(0))
        {
            UsarHerramientaWorld();
        }
    }

    private void TileSeleccionado()
    {
        //Guarda qu� tile se est� seleccionando, guarda su posici�n
        posicionTileSeleccionado = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    private void Mostrar()
    {
        //Mientras el rat�n se encuentre a una cierta distancia del jugador permite interacruar con lo seleccionado
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Si el rat�n est� m�s lejos de la distancia m�xima permitida, el marcador tile del rat�n no se mostrar�
        seleccionado = Vector2.Distance(characterPosition, cameraPosition) < distanciaMaxima;
        markerManager.Show(seleccionado);
    }

    private void Marker()
    {
        //Marca el tile que se ha seleccionado en este frame
        markerManager.markedCellPosition = posicionTileSeleccionado;
    }

    private Vector3Int GetMouseTilePosition()
    {
        //Coje la posici�n del tile marcado por el rat�n
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return arado.WorldToCell(mouseWorldPos);
    }

    //--------------------------------------------------------------------

    private void Arar()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Coge la posici�n del tile que se est� seleccionando y al tileset que se le pasa por par�metro 
            //se le apllica el tile paasado por referencia
            Vector3Int posicionMouse = GetMouseTilePosition();
            TileBase tileEnPosicion = arado.GetTile(posicionMouse);

            if (tileEnPosicion == null)
            {
                // No hay un tile en la posici�n actual, puedes hacer algo aqu�
                arado.SetTile(posicionMouse, piezaArada); // Ejemplo: establecer un nuevo tile en la posici�n
                Jugador.Instance.energia -= energiaArar;
            }
        }
    }



    //--------------------------------------------------------------------


    private void UsarHerramientaWorld()
    {
        //Lo que hace es guardar la �ltima posici�n hacia la que se
        //movi� el jugador y permite picar para esa direcci�n 

        Vector2 posicion = rb.position + personaje.ultimoMotionVector * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(posicion, tama�oAreaInteractuable);

        foreach (Collider2D c in colliders)
        {
            HitHerramientas hit = c.GetComponent<HitHerramientas>();
            if (hit != null)
            {
                hit.Hit();
                break;
            }
        }
    }
}

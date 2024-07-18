using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ControladorHerramientas : MonoBehaviour
{
    private Jugador personaje;
    private Rigidbody2D rb;

    // General ------------------------------------------------------------

    [SerializeField] private Tilemap worldRecolectables;//Tilemap que contiene todaslas posiciones en las que pueden haber objetos recolectables

    // Marcar casilla actual ----------------------------------------------

    [SerializeField] private MarkerManager markerManager;
    [SerializeField] private float distanciaMaximaMarcador;

    private Vector3Int posicionTileSeleccionado;
    private bool seleccionado;

    // Objetos Recolectables ------------------------------------------------


    [SerializeField] private float distanciaEntreTiles;


    // Arar ---------------------------------------------------------------

    [SerializeField] private TileBase piezaArada;
    [SerializeField] private Tilemap arado;
    [SerializeField] private int energiaArar;

    //----------------------------------------------------------------------

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
        Indicador();

        
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.permitirUsarHerramineta == true)
        {
            //Coge la posici�n del tile que se est� seleccionando y al tileset que se le pasa por par�metro 
            //se le apllica el tile paasado por referencia
            Vector3Int posicionMouse = GetMouseTilePosition();


            AccederRecolectable(posicionMouse);

            Arar(posicionMouse);
        }
    }

    #region Marcador
    //------------------------------------------------------------------------------------------------------------

    private void Indicador()
    {
        GetGridPosition(Input.mousePosition, true);
        Mostrar();
        Marker();
    }


    public void GetGridPosition(Vector2 position, bool mousePosition)
    {
        //Guarda la posici�n del mouse y la convierte a tile para saber sobre cu�l est�

        Vector3 worldPosition;

        //Guarda la posicion del mouse en el mundo
        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        //Convierte esa posicion del mundo en una posicion de tile
        Vector3Int gridPosition = worldRecolectables.WorldToCell(worldPosition);

        posicionTileSeleccionado = gridPosition;
    }

    private void Mostrar()
    {
        //Mientras el rat�n se encuentre a una cierta distancia del jugador permite interacruar con lo seleccionado

        //Guarda la posici�n tanto del jugador como del mouse en ese momento
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Si el rat�n est� m�s lejos de la distancia m�xima permitida, el marcador tile del rat�n no se mostrar�
        seleccionado = Vector2.Distance(characterPosition, cameraPosition) < distanciaMaximaMarcador;
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
        return worldRecolectables.WorldToCell(mouseWorldPos);
    }

    //------------------------------------------------------------------------------------------------------------
    #endregion
    

    // Arar ----------------------------------------------------------------

    private void Arar(Vector3Int posicion)
    {
        TileBase tileEnPosicion = arado.GetTile(posicion);

        if (tileEnPosicion == null)
        {
            // No hay un tile en la posici�n actual, puedes hacer algo aqu�
            arado.SetTile(posicion, piezaArada); // Ejemplo: establecer un nuevo tile en la posici�n
            Jugador.Instance.energia -= energiaArar;
        }
    }

    //----------------------------------------------------------------------

    private void AccederRecolectable(Vector3Int posicion)
    {
        posicion.x = posicion.x + 1;
        posicion.y = posicion.y + 1;
        posicion.z = 0;


        Vector2 posicionMouse = new Vector2(posicion.x, posicion.y);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(posicionMouse, distanciaEntreTiles);


        foreach (Collider2D hitbox in colliders)
        {
            if(hitbox != null && hitbox.gameObject.name != this.gameObject.name)
            {
                ObjetosRecolectables objetoRecolectable = hitbox.gameObject.GetComponent<ObjetosRecolectables>(); 


                if (objetoRecolectable != null && posicion == objetoRecolectable.transform.position)
                {
                    objetoRecolectable.ClasificarGolpe();
                }

                if (objetoRecolectable.componenteSpriteRenderer.enabled == false && 
                    Toolbar.Instance.herramientaSeleccionada.item.semilla == true)
                {
                    objetoRecolectable.CambiarAndAparecerObjeto(Toolbar.Instance.herramientaSeleccionada.item.worldItem);
                }
            }
        }
    }
    //-------------------------------------------------------------------------
}
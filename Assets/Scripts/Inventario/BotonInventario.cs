using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class BotonInventario : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    //Variables referentes a los items a nivel t�cnico --------------------------------------------
    public Item item;
    public int cantidad;
    //---------------------------------------------------------------------------------------------

    //Variables referentes a los items a nivel visual ---------------------------------------------
    [SerializeField] private Image icono;
    [SerializeField] private TMP_Text textCantidad;
    [SerializeField] private int numeroClasificador;
    [SerializeField] private DescripcionesController descripcionesController;
    //---------------------------------------------------------------------------------------------

    //Variables referentes a la toolbar -----------------------------------------------------------
    public GameObject seleccionado;
    public bool casillaToolbar;
    //---------------------------------------------------------------------------------------------

    private void Awake()
    {
        DesactivarVisualizacion();
    }

    private void Update()
    {
        //Si una casilla est� vac�a se oculta tanto la imagen base de los
        //objetos como tambi�n el texto que indica la cantidad de los mismos
        if (item == null)
        {
            DesactivarVisualizacion();
        }

        //Si una casilla est� ocupada por un objeto se muestra
        //tanto la imagen base de los objetos y tambi�n el texto
        else
        {
            //Se actualiza la imagen de icono por
            //al del objeto que tiene en el momento
            icono.sprite = item.sprite;
            icono.gameObject.SetActive(true);

            //Si solo hay una unidad del objeto entonces el texto
            //que indica la cantidad que tienes de decho item no
            //se muestra. Esto sirve tant para los stackeables
            //como para los no stackeables, dado que estos tienen 
            //una cantidad m�xima de 1 por casilla del inventario
            if (cantidad == 1)
            {
                textCantidad.text = "";
            }

            //Si la cantidad de objeto que posee el jugador en una
            //misma casilla es superior a 1 entonces se muestra el
            //valor de dicha cantidad en el texto
            else
            {
                textCantidad.text = cantidad.ToString("0");
            }
        }
    }

    private void DesactivarVisualizacion()
    {
        //La imagen del objeto en la casilla del inventario
        //y el texto de la cantidad del item se ocultan
        //en para que todo quede m�s limpio y m�s claro

        icono.gameObject.SetActive(false);

        textCantidad.text = "";
    }

    public void ClickIzquierdoInventario()
    {
        //Se trata de la l�gica que se ejecuta cuando se pulsa el
        //bot�n casilla con el bot�n izquierdo del rat�n

        //Si se intenta colocar un objeto sobre una casilla que ya contiene
        //ese objeto y el objeto es stackeable entonces se suman las unidades
        if (DragAndDropController.Instance.itemDnD == item && DragAndDropController.Instance.itemDnD.stackeable == true)
        {
            DragAndDropController.Instance.Anadir(numeroClasificador);
        }

        //En cualquier otro caso lo que ocurre es que se intercambian el objeto
        //que hay en la casilla clicada con el objeto que hay en el DnD
        else
        {
            DragAndDropController.Instance.Copiar(numeroClasificador);
        }
    }

    private void ClickDerechoInventario()
    {
        //Se trata de la l�gica que se ejecuta cuando se pulsa el
        //bot�n casilla con el bot�n derecho del rat�n

        //Si se intenta colocar un objeto sobre una casilla que ya contiene
        //ese objeto y el objeto es stackeable entonces se le suma una unidad
        //a la casilla del inventario con la que se est� interactuando
        if (DragAndDropController.Instance.itemDnD == item && 
            DragAndDropController.Instance.itemDnD.stackeable == true)
        {
            DragAndDropController.Instance.AnadirIndividual(numeroClasificador);
        }

        //En cualquier otro caso lo que ocurre es que se intercambian el objeto
        //que hay en la casilla clicada con el objeto que hay en el DnD
        else
        {
            DragAndDropController.Instance.CopiarIndividual(numeroClasificador);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Se trata de la funci�n que permite el funcionamiento del click
        //derecho sobre las casillas del inventario dado que el componente
        //button solo tiene programado para aceptar el click izquierdo del rat�n.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // L�gica para cuando pulsas el clic derecho del rat�n
            ClickDerechoInventario();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Si la casilla sobre la que est� el rat�n posee un objeto y adem�s
        //no pertenece a la toolbar, entonces el panel de descripciones
        //se hace visible y se actualiza con la informaci�n de dicho objeto
        if (item != null && casillaToolbar == false)
        {
            descripcionesController.textNombre.text = item.nombre;
            descripcionesController.textDescripciones.text = item.descripcion;
            descripcionesController.panelDescripciones.SetActive(true);
        }

        //En caso contrario dicho panel se oculta
        else
        {
            descripcionesController.panelDescripciones.SetActive(false);
        }
    }
}
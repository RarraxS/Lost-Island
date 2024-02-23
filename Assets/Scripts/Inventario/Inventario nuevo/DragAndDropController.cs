using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropController : MonoBehaviour
{
    //Drag and drop visual
    [SerializeField] private GameObject dragAndDrop;
    private RectTransform iconTransform;
    [SerializeField] private Vector3 distancia;
    private Image itemIconImage;

    //Par�metros del item que se est� mostrando actualmente
    public  string nombreDnD;
    [SerializeField] private Sprite spriteDnD;
    [SerializeField] private string descripcionDnD;
    [SerializeField] private bool stackeableDnD;
    [SerializeField] private int cantidadDnD;

    //Par�metros del item a copiar
    [SerializeField] private string nombreNuevo;
    [SerializeField] private Sprite spriteNuevo;
    [SerializeField] private string descripcionNuevo;
    [SerializeField] private bool stackeableNuevo;
    [SerializeField] private int cantidadNuevo;


    private static DragAndDropController instance;
    public static DragAndDropController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    
        iconTransform = dragAndDrop.GetComponent<RectTransform>();
        itemIconImage = dragAndDrop.GetComponent<Image>();
    }

    void Update()
    {
        //Actualiza la posici�n del objeto y su sprite
        iconTransform.position = Input.mousePosition + distancia;
        itemIconImage.sprite = spriteDnD;
    }

    public void Copiar(int numeroClasificatorio)
    {
        //Permite intercambiar el objeto que hay en el inventario con el que hay en el Drag and Drop

        //Rellena las variables "contenedoras" de los datos del objeto
        nombreNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].nombre;
        spriteNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].sprite;
        descripcionNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].descripcion;
        stackeableNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].stackeable;
        cantidadNuevo = Inventario.Instance.slotInventario[numeroClasificatorio].cantidad;

        //Actualiza los datos de las variables del inventario
        Inventario.Instance.slotInventario[numeroClasificatorio].nombre = nombreDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].sprite = spriteDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].descripcion = descripcionDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].stackeable = stackeableDnD;
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad = cantidadDnD;

        //Pasa las variables del "contenedor" a las variables que realmente utiliza el Drag and Drop
        nombreDnD = nombreNuevo;
        spriteDnD = spriteNuevo;
        descripcionDnD = descripcionNuevo;
        stackeableDnD = stackeableNuevo;
        cantidadDnD = cantidadNuevo;

        //Limpia el "contenedor" para que pueda acoger al pr�ximo objeto
        nombreNuevo = null;
        spriteNuevo = null;
        descripcionNuevo = null;
        stackeableNuevo = false;
        cantidadNuevo = 0;
    }

    public void Anadir(int numeroClasificatorio)
    {
        //Si el obeto que se est� moviendo con el DnD se trata de colocar encima de una casilla
        //con el mismo objeto se suma a la casilla del inventario

        //Suma las cantidades
        Inventario.Instance.slotInventario[numeroClasificatorio].cantidad += cantidadDnD;

        //Limpia el los datos de las variables del DnD para que pueda acoger al pr�ximo objeto
        nombreDnD = null;
        spriteDnD = null;
        descripcionDnD = null;
        stackeableDnD = false;
        cantidadDnD = 0;
    }
}

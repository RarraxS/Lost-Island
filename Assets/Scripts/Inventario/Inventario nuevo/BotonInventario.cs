using UnityEngine.UI;
using UnityEngine;

public class BotonInventario : MonoBehaviour
{
    public string nombre;
    public Sprite sprite;
    public string descripcion;
    public bool stackeable;
    public int cantidad;

    [SerializeField]private Image icono;

    private void Start()
    {
        
    }

    
    private void Update()
    {
        icono.sprite = sprite;
    }
}

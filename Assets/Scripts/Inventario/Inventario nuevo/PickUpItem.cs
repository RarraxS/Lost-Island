using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    Transform jugador;//Es el objeto al que va a seguir el item

    [SerializeField] float distanciaMaximaInteraccion;//La distancia m�xima a la que un objeto seguir� al jugador
    [SerializeField] int velocidad;//La velocidad con la que el objeto persigue al jugador
    [SerializeField] Item item;//El item que se le sumar� al inventario del jugador cuando recoja el objeto


    void Start()
    {
        jugador = GameManager.Instance.Player.transform;
    }

    
    void Update()
    {
        //Seguir al jugador ---------------------------------------------------------------------------------------
        
        float distance = Vector3.Distance(transform.position, jugador.position);
        if (distance > distanciaMaximaInteraccion)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        //---------------------------------------------------------------------------------------------------------
        if (distance < 0.1f)
        {
            //Recorre todas las posiciones del inventario, si hay alguna posici�n que ya tenga ese objeto y
            //el objeto es stackeable lo mete ah�, o si no hay ninguna posici�n en la que haya acumulaciones
            //de ese objeto encuentra alguna posici�n vac�a y entonces lo mete ah�. si no tiene espacio,
            //entonces el objeto no segue al jugador
            for (int i = 0; i < Inventario.Instance.slotInventario.Length; i++)
            {
                if (item.nombre == Inventario.Instance.slotInventario[i].nombre && item.stackeable == true)
                {
                    //Modificar este para que en vez de crear uno nuevo sume a la cantidad que ya hay
                    Debug.Log("Sumado");
                    Inventario.Instance.AnadirInventario(i, item.nombre, item.sprite, item.descripcion, item.stackeable);
                    Destroy(gameObject);
                    break;
                }

                if (Inventario.Instance.slotInventario[i].nombre == "")
                {
                    Debug.Log("A�adido");
                    Inventario.Instance.AnadirInventario(i, item.nombre, item.sprite, item.descripcion, item.stackeable);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}

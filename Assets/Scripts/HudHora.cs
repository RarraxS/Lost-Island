using UnityEngine;

public class HudHora : MonoBehaviour
{
    [SerializeField] private int estacion = 1;
    public static Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        //Actualiza el animator de las estaciones con el sprite de la estaci�n correspondiente 
        animator.SetInteger("numEstacion", estacion);

        //Cuando se llega al d�a 31 se cambia a la siguiente estaci�n
        if (GameManager.diaEstaciones > 30)
        {
            estacion++;
        }

        if(estacion == 5)
        {
            estacion = 1;
        }
    }
}
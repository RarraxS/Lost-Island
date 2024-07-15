using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour, IObserver
{
    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;
    [SerializeField] private bool permitirGolpear;
    [SerializeField] private int energiaGolpear;
    [SerializeField] private int puntosDeVida;

    public List<Drops> drops;
    [SerializeField] private float distanciaMaximaAparicion;


    [SerializeField] private bool semilla;
    [SerializeField] private bool estacionDeCultivo;
    private int numDiasPasados;


    public bool rotarEntrarColision;
    [SerializeField] private bool ignorarTransparencia;
    private bool observando;


    [SerializeField] private GameObject objetoSinColision;
    [SerializeField] private Color colorTransparencia;


    private Transform tr;

    public SpriteRenderer componenteSpriteRenderer;
    public PolygonCollider2D componenteHitboxColision;
    public PolygonCollider2D componenteHitboxSinColision;
    public Animator componenteAnimator;


    [SerializeField] private List<ItemAndProbability> spawneables;



    private void Start()
    {
        tr = GetComponent<Transform>();

        componenteSpriteRenderer = GetComponent<SpriteRenderer>();
        componenteHitboxColision = GetComponent<PolygonCollider2D>();
        componenteHitboxSinColision = objetoSinColision.GetComponent<PolygonCollider2D>();
        componenteAnimator = GetComponent<Animator>();

        Observar();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CambiarAndAparecerObjeto(spawneables[1].worldItem.name);
            //componenteAnimator.SetInteger("dias", numDiasPasados += 1);
        }
    }

    #region Golpear objetos
    public void ClasificarGolpe()
    {
        Debug.Log("Golpear");
        if (permitirGolpear == true)
        {
            if (Toolbar.Instance.herramientaSeleccionada.item != null)
            {
                if ((Toolbar.Instance.herramientaSeleccionada.item.herramienta == herramientaNecesaria ||
                herramientaNecesaria == "") && Toolbar.Instance.herramientaSeleccionada.item.herramienta != "")
                {
                    Golpear(Toolbar.Instance.herramientaSeleccionada.item.damageHerramienta);
                }

                else
                    Golpear(1);
            }

            else if (herramientaNecesaria == "")
            {
                Golpear(1);
            }

            else
                return;
        }
    }

    public void Golpear(int damage) 
    {
        puntosDeVida -= damage;
        
        if (puntosDeVida <= 0)
        {
            puntosDeVida= 0;

            GenerarDropsAndOcultar();
        }
    }

    #endregion

    #region Aparecer y desaparecer objetos
    private void GenerarDropsAndOcultar()
    {
        for (int numDrop = 0; numDrop < drops.Count; numDrop++)
        {
            if (drops[numDrop].dropeable != null)
            {
                for (int numProbabilidades = drops[numDrop].probabilidades.Count - 1; numProbabilidades >= 0; numProbabilidades--)
                {
                    Debug.Log("Bucle");
                    int random = UnityEngine.Random.Range(1, 101);

                    if (random <= drops[numDrop].probabilidades[numProbabilidades].probabilidad)
                    {
                        Debug.Log(numProbabilidades);
                        for (int cantidadDrops = drops[numDrop].probabilidades[numProbabilidades].cantidad; cantidadDrops > 0; cantidadDrops--)
                        {
                            float distanciaAparicion;

                            distanciaAparicion = UnityEngine.Random.Range(0, distanciaMaximaAparicion);

                            Vector3 posicionAleatoria = (UnityEngine.Random.insideUnitSphere * distanciaAparicion) + tr.position;
                            posicionAleatoria.z = 0;

                            Instantiate(drops[numDrop].dropeable, posicionAleatoria, Quaternion.identity);
                        }
                        break;
                    }
                }
            }
        }

        componenteSpriteRenderer.enabled = false;
        componenteHitboxColision.enabled = false;
        componenteHitboxSinColision.enabled = false;
        componenteAnimator.enabled = false;
    }

    private void CambiarAndAparecerObjeto(string name)
    {
        for (int i = 0; i < spawneables.Count; i++)
        {
            if (spawneables[i].worldItem.gameObject.name == name)
            {
                //Necesario buscar una forma optima y escalable de pasar los datos de un objeto recolectable a otro
                //para no tener que andar modificando esta parte del c�digo cada que se implemente una nueva variable


                componenteSpriteRenderer.sprite = spawneables[i].worldItem.componenteSpriteRenderer.sprite;



                puntosDeVida = spawneables[i].worldItem.puntosDeVida;
                herramientaNecesaria = spawneables[i].worldItem.herramientaNecesaria;
                nivelMinimoDeHerramienta = spawneables[i].worldItem.nivelMinimoDeHerramienta;
                distanciaMaximaAparicion = spawneables[i].worldItem.distanciaMaximaAparicion;
                permitirGolpear = spawneables[i].worldItem.permitirGolpear;
                energiaGolpear = spawneables[i].worldItem.energiaGolpear;
                ignorarTransparencia = spawneables[i].worldItem.ignorarTransparencia;
                rotarEntrarColision = spawneables[i].worldItem.rotarEntrarColision;


                for (int j = 0; j < drops.Count; j++)
                {
                    if (spawneables[i].worldItem.drops[j] != null)
                    {
                        drops[j] = spawneables[i].worldItem.drops[j];
                    }

                    else
                    {
                        drops[j] = null;
                    }
                }


                componenteHitboxColision.points = spawneables[i].worldItem.componenteHitboxColision.points;
                componenteHitboxColision.isTrigger = spawneables[i].worldItem.componenteHitboxColision.isTrigger;
                componenteHitboxSinColision.points = spawneables[i].worldItem.componenteHitboxSinColision.points;

                //----------------------------------------------------------------------------------------------

                componenteSpriteRenderer.enabled = true;
                componenteHitboxColision.enabled = true;
                componenteHitboxSinColision.enabled = true;
                componenteAnimator.enabled = true;

                if (semilla == true)
                {
                    numDiasPasados = componenteAnimator.GetInteger("dias");
                }

                Observar();

                break;
            }
        }
    }

    #endregion

    #region Alternar color

    public void TransparentarObjeto()
    {
        if (ignorarTransparencia == false)
        {
            componenteSpriteRenderer.color = colorTransparencia;
        }
    }

    public void RestablecerColor()
    {
        if (ignorarTransparencia == false)
        {
            componenteSpriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    #endregion

    private void Observar()
    {
        if (semilla == true || gameObject.activeSelf)
        {
            ObserverManager.Instance.AddObserver(this);
            observando = true;
        }

        else if (observando == true)
        {
            ObserverManager.Instance.RemoveObserver(this);
            observando = false;
        }
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "dia completado")
        {
            //Programar el paso de un dia para las plantas regadas, y la probabilidad de aparicion para cuando no hay ningun objeto
            if (!gameObject.activeSelf)
            {
                //Programar aqui el tirar los dados para ver si spawnea alg�n objeto al cambiar de dia
            }

            if (semilla == true)
            {
                //Programar el avance de dia para las semillas
                componenteAnimator.SetInteger("dias", numDiasPasados += 1);
            }
        }
    }
}


//Esta va a ser la clase que contenga las variables de probabilidad y los gameobjects
//que van a swawnear de forma aleatoria por el mapa a lo largo de los dias

[Serializable]
public class ItemAndProbability
{
    public ObjetosRecolectables worldItem;

    public float probabilidad;
}

//-----------------------------------------------------------------------------------------

//Estas clases se van a encargar de almacenar la informacion de los dropeables

[Serializable]
public class Drops
{
    public GameObject dropeable;

    public List<CuantityProbabilities> probabilidades;
}

[Serializable]
public class CuantityProbabilities
{
    public int cantidad;

    public float probabilidad;
}
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TileBase tile;

    public Vector3Int markedCellPosition;
    private Vector3Int oldCellPosition;
    private bool show;

    private void Update()
    {
        if (show == false)
        {
            return;
        }
        targetTilemap.SetTile(oldCellPosition, null); //Marca que el tile seleccionado en el frame anterior se vac�e
        targetTilemap.SetTile(markedCellPosition, tile); //Marca que el tile seleccionado en este frame se rellene con el tile que se le ha pasado por par�metro
        oldCellPosition = markedCellPosition; //El que ahora era el tile nuevo pasa a ser el tile viejo y el proceso se repite
    }

    public void Show(bool seleccionado)
    {
        //Dependiendo de si la variable "seleccionado" es true o flase se muestra el tile al que est� apuntando el rat�n
        show = seleccionado;
        targetTilemap.gameObject.SetActive(show);
    }
}

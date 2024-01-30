using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemContainer))]
public class EditorItemContainer : Editor
{
    public override void OnInspectorGUI()
    {
        //A�ade un bot�n al inspector, concretamente al "ItemContainer" el cual se trata del inventario.
        //Esto no afecta al juego, es solo en el inspector y para realizar pruebas

        ItemContainer container = target as ItemContainer;
        if (GUILayout.Button("Limpiar inventario"))
        {
            for (int i = 0; i < container.slots.Count; i++)
            {
                container.slots[i].Clear();
            }
        }
        DrawDefaultInspector();
    }
}

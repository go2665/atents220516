using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInfo : MonoBehaviour
{
    public GameObject infoPrefab;
    public Material infoMaterial;

    private GameObject MakeInfoObject()
    {
        GameObject obj = Instantiate(infoPrefab, transform);
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = infoMaterial;

        return obj;
    }

    public void MarkBombInfo(Vector3 position)
    {
        GameObject obj = MakeInfoObject();
        obj.transform.position = position + Vector3.up;
    }
}

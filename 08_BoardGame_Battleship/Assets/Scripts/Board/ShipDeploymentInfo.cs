using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ShipDeploymentInfo : MonoBehaviour
{
    public GameObject infoPrefab;
    public Material[] infoMaterials;

    Dictionary<ShipType, List<GameObject>> infoObjects;

    private void Awake()
    {
        infoObjects = new Dictionary<ShipType, List<GameObject>>(ShipManager.Inst.ShipTypeCount);
        infoObjects[ShipType.Carrier] = new List<GameObject>();
        infoObjects[ShipType.Battleship] = new List<GameObject>();
        infoObjects[ShipType.Destroyer] = new List<GameObject>();
        infoObjects[ShipType.Submarine] = new List<GameObject>();
        infoObjects[ShipType.PatrolBoat] = new List<GameObject>();

    }

    private GameObject MakeInfoObject(ShipType type)
    {
        GameObject obj = Instantiate(infoPrefab, transform);
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = infoMaterials[(int)(type - 1)];

        return obj;
    }

    public void MarkShipDeplymentInfo(ShipType type, Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject obj = MakeInfoObject(type);
            obj.transform.position = positions[i];
            infoObjects[type].Add(obj);
        }
    }

    public void UnMarkShipDeplymentInfo(ShipType type)
    {
        // 만들어놓았던 InfoObject 삭제(배 종류에 맞게)
        foreach(var infoObj in infoObjects[type])
        {
            Destroy(infoObj);
        }
        infoObjects[type].Clear();
    }
}

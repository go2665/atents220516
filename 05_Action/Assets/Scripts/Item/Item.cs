using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    private void Start()
    {
        Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }
}

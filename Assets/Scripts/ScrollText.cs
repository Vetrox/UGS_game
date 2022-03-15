using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScrollText : MonoBehaviour
{
    private Text textComp;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 40 * Time.deltaTime, transform.position.z);
    }

    void Start()
    {
        textComp = GetComponent<Text>();
        textComp.text = @"Credits

All and everything

Bastian
Felix



Part of the music was provided by Wintergatan.";
    }

  
}

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

Lane Switch

Leading Actor
Wall-B

Supporting Actors
Saw
Pipe
Wall
Steel-Beam

Background Actors
House1 (+ Wrap)
House2 (+ Wrap)
House3 (+ Wrap)
House4 (+ Wrap)

Casting Directors
Bastian
Felix

Lead costume designer
Bastian
Felix

Associate producers
Bastian
Felix

Lead editors
Bastian
Felix

Production designers
Bastian
Felix

Director of photography
Bastian
Felix

Executive producers
Bastian
Felix

Line producers
Bastian
Felix

Sound Director
Bastian
Felix

Executive Sound Directors
Bastian
Felix



Fonts

Source Sans Family by Adobe (OFL)
Future Techno Italic (Freeware, non-commercial)



Audio

Laser-shot sound (151022) by bubaproducer (CC)
See freesound.net

Part of the music was provided by Wintergatan
See wintergatan.net";
    }

  
}

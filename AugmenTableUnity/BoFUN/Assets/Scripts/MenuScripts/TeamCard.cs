using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamCard : MonoBehaviour
{
    //public 
    public Image backgroundImage;

    public Color BackgroundColor
    {
        get => backgroundImage.color;
        set => backgroundImage.color = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!backgroundImage)
        {
            Debug.Log("Incorrect Team Card setup");
            return;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

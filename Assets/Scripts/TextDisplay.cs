using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[Serializable]
public class StringEvent : UnityEvent<string> {}
public class TextDisplay : MonoBehaviour
{
    [SerializeField] public StringEvent stringEvent;
    // TextMeshProUGUI textDisplay;
    public void SendUpdate(float input)
    {
        stringEvent.Invoke(input.ToString());  
    }
    public void SendUpdate(int input)
    {
        stringEvent.Invoke(input.ToString());  
    }
}

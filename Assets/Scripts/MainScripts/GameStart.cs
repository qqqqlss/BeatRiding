using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public SerialController serialController;

    private void Start()
    {
        
    }

    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;
        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.LogWarning("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.LogWarning("Connection attempt failed or disconnection detected");
        else
        {
            Debug.Log("Message arrived: " + message);
            switch (message)
            {
                case "L":   //left button use
                    SceneManager.LoadScene("Scenes/InputName-start");
                    break;
                case "R":    //Right button use
                    SceneManager.LoadScene("Scenes/InputName-start");
                    break;
            }
        }
    }
}

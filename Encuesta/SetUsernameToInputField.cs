using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class SetUsernameToInputField : UdonSharpBehaviour
{
    // Campo de entrada donde se mostrará el nombre de usuario
    public InputField targetInputField; 

    // Conjunto de objetos que se activarán o desactivarán
    public GameObject[] gameObjectsToToggle;

    // Almacena el nombre de usuario del jugador
    private string username;

    private void Start()
    {
        // Obtención del jugador local
        VRCPlayerApi localPlayer = Networking.LocalPlayer;

        if (localPlayer != null)
        {
            // Asignar el nombre de usuario del jugador a la variable username
            username = localPlayer.displayName; 
            
            // Asignar el nombre de usuario al campo de entrada
            targetInputField.text = username;
        }
    }

    public void ToggleUsernameAndObjects()
    {
        // Si el texto del campo de entrada es igual al nombre de usuario, lo vaciamos, en caso contrario, le asignamos el nombre de usuario
        if (targetInputField.text == username)
        {
            targetInputField.text = "";
        }
        else
        {
            targetInputField.text = username;
        }

        // Activamos o desactivamos los objetos en gameObjectsToToggle
        foreach (GameObject obj in gameObjectsToToggle)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}

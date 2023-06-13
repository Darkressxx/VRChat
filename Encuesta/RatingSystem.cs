using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;

public class RatingSystem : UdonSharpBehaviour
{
    // Valor actual de la calificación
    private int currentRating = 0;
    
    // Botón actualmente seleccionado
    private RatingButton currentSelectedButton;

    // Función que devuelve el valor de la calificación actual
    public int GetCurrentRating()
    {
        return currentRating;
    }

    // Función para establecer la calificación. Recibe un nuevo valor de calificación y el botón que fue presionado
    public void SetRating(int newRating, RatingButton button)
    {
        // Si no hay calificación actual o si la nueva calificación es diferente a la actual
        if (currentRating == 0 || currentRating != newRating)
        {
            // Si hay un botón seleccionado actualmente, lo deseleccionamos
            if (currentSelectedButton != null)
            {
                currentSelectedButton.UpdateButtonState(false);
            }

            // Asignamos la nueva calificación y el botón seleccionado
            currentRating = newRating;
            currentSelectedButton = button;
            
            // Actualizamos el estado del botón seleccionado
            currentSelectedButton.UpdateButtonState(true);
        }
    }
}

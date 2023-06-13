using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;

public class RatingButton : UdonSharpBehaviour
{
    // Valor de calificación asignado al botón
    [SerializeField] private int buttonRating;

    // Referencia al sistema de calificación
    [SerializeField] private RatingSystem ratingSystem;

    // Objeto que se activa cuando el botón está deseleccionado
    [SerializeField] private GameObject deselected;

    // Objeto que se activa cuando el botón está seleccionado
    [SerializeField] private GameObject selected;

    private void Start()
    {
        // Inicialmente, el botón está deseleccionado
        deselected.SetActive(true);
        selected.SetActive(false);
    }

    public void OnButtonClick()
    {
        // Cuando se hace clic en el botón, se establece la calificación en el sistema de calificación
        ratingSystem.SetRating(buttonRating, this);
    }

    public void UpdateButtonState(bool isSelected)
    {
        // Si el botón está seleccionado, se activa el objeto "selected" y se desactiva el objeto "deselected"
        // Si el botón no está seleccionado, se hace lo contrario
        deselected.SetActive(!isSelected);
        selected.SetActive(isSelected);
    }
}

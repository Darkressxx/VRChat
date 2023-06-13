using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using System;

public class ConcatenateAndEncode : UdonSharpBehaviour
{   
    // Arreglo de campos de entrada
    public InputField[] inputFields;

    // Arreglo de sistemas de calificación
    public RatingSystem[] ratingSystems;
    
    // Campo de entrada para el resultado
    public InputField resultInputField;
    
    // URL base para la petición, aquí se reemplaza la ID de implementación
    [SerializeField] private string baseUrl = "https://script.google.com/macros/s/Colocar_la_ID_de_implementación/exec?message=";
    
    // Intervalo de actualización en frames
    public int updateInterval = 10;
    // Contador de frames
    private int frameCounter = 0;

    private void Update()
    {
        frameCounter++;
        // Cuando el contador de frames alcanza el intervalo de actualización,
        // concatenamos y codificamos los valores
        if (frameCounter >= updateInterval)
        {
            ConcatenateAndEncodeValues();
            // Reiniciamos el contador de frames
            frameCounter = 0;
        }
    }
    
    // Método para convertir una cadena de texto a una matriz de bytes en UTF-8
    public byte[] StringToUTF8ByteArray(string str)
    {
        // Este bloque de código está generando una matriz de bytes que representa la cadena de caracteres en UTF-8
        // Toma en cuenta los diferentes tamaños que pueden tener los caracteres dependiendo del rango de sus codepoints
        // El código es básicamente una implementación manual de la codificación UTF-8
        int size = 0;
        foreach (char c in str)
        {
            int codepoint = char.ConvertToUtf32(str, str.IndexOf(c));
            if ((codepoint >= 0x0000) && (codepoint <= 0x007F))
            {
                size += 1;
            }
            else if ((codepoint >= 0x0080) && (codepoint <= 0x07FF))
            {
                size += 2;
            }
            else if ((codepoint >= 0x0800) && (codepoint <= 0xFFFF))
            {
                size += 3;
            }
            else if ((codepoint >= 0x10000) && (codepoint <= 0x10FFFF))
            {
                size += 4;
            }
        }
        byte[] result = new byte[size];
        int index = 0;
        foreach (char c in str)
        {
            int codepoint = char.ConvertToUtf32(str, str.IndexOf(c));
            if ((codepoint >= 0x0000) && (codepoint <= 0x007F))
            {
                result[index++] = (byte)codepoint;
            }
            else if ((codepoint >= 0x0080) && (codepoint <= 0x07FF))
            {
                result[index++] = (byte)(0xC0 | (codepoint >> 6));
                result[index++] = (byte)(0x80 | (codepoint & 0x3F));
            }
            else if ((codepoint >= 0x0800) && (codepoint <= 0xFFFF))
            {
                result[index++] = (byte)(0xE0 | (codepoint >> 12));
                result[index++] = (byte)(0x80 | ((codepoint >> 6) & 0x3F));
                result[index++] = (byte)(0x80 | (codepoint & 0x3F));
            }
            else if ((codepoint >= 0x10000) && (codepoint <= 0x10FFFF))
            {
                result[index++] = (byte)(0xF0 | (codepoint >> 18));
                result[index++] = (byte)(0x80 | ((codepoint >> 12) & 0x3F));
                result[index++] = (byte)(0x80 | ((codepoint >> 6) & 0x3F));
                result[index++] = (byte)(0x80 | (codepoint & 0x3F));
            }
        }
        // Retornamos el resultado como una matriz de bytes
        return result;
    }

    // Método para concatenar y codificar los valores de los campos de entrada y sistemas de calificación
    public void ConcatenateAndEncodeValues()
    {
        string concatenatedValues = "";
        // Concatenamos los valores de los campos de entrada con un tabulador y punto y coma
        foreach (InputField inputField in inputFields)
        {
            concatenatedValues += inputField.text + "\t;";
        }
        // Concatenamos los valores de los sistemas de calificación con un tabulador y punto y coma
        foreach (RatingSystem ratingSystem in ratingSystems)
        {
            concatenatedValues += ratingSystem.GetCurrentRating().ToString() + "\t;";
        }
        // Si el último carácter es un punto y coma, lo eliminamos
        if (concatenatedValues.Length > 0 && concatenatedValues[concatenatedValues.Length - 1] == ';')
        {
            concatenatedValues = concatenatedValues.Remove(concatenatedValues.Length - 1);
        }
        // Convertimos la cadena concatenada a una matriz de bytes en UTF-8
        byte[] byteArray = StringToUTF8ByteArray(concatenatedValues);
        // Codificamos la matriz de bytes a una cadena en base 64
        string encodedResult = Convert.ToBase64String(byteArray);
        // Asignamos la URL completa al campo de entrada del resultado
        resultInputField.text = baseUrl + encodedResult;
    }
}
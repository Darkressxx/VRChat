using UdonSharp;
using UnityEngine;
using VRC.SDK3.Image;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public class URL : UdonSharpBehaviour
{
    [SerializeField, Tooltip("Campo de entrada para la URL")]
    private VRCUrlInputField urlInputField;

    // Se utiliza el material de imagen como solución para ejecutar el método DownloadImage,
    // ya que requiere un parámetro de material, aunque no se use para la funcionalidad deseada.
    private Material imageMaterial;

    private VRCImageDownloader imageDownloader;
    private IUdonEventReceiver udonEventReceiver;

    private void Start()
    {
        // Instanciar el objeto imageDownloader
        imageDownloader = new VRCImageDownloader(); 

        // Definir el receptor de eventos Udon
        udonEventReceiver = (IUdonEventReceiver)this; 
    }

    // Este método envía una solicitud a la URL especificada mediante una solicitud POST.
    public void SendRequestFromURL()
    {
        // Obtiene la URL del campo de entrada
        VRCUrl url = urlInputField.GetUrl(); 

        if (string.IsNullOrEmpty(url.Get()))
        {
            // Si la URL está vacía, mostrar error
            Debug.LogError("[URL] ¡URL vacía!"); 
            return;
        }

        var requestInfo = new TextureInfo();
        requestInfo.GenerateMipMaps = true;

        // Descarga la imagen de la URL usando VRCImageDownloader.
        // El parámetro imageMaterial no se utiliza para la funcionalidad deseada,
        // pero es necesario para ejecutar el método DownloadImage.
        imageDownloader.DownloadImage(url, imageMaterial, udonEventReceiver, requestInfo);
    }

    // Método a sobrescribir que se llama cuando la solicitud de imagen tiene éxito.
    public override void OnImageLoadSuccess(IVRCImageDownload result)
    {
        // La solicitud de imagen tuvo éxito, maneja la respuesta si es necesario.
    }

    // Método a sobrescribir que se llama cuando la solicitud de imagen falla.
    public override void OnImageLoadError(IVRCImageDownload result)
    {
        // La solicitud de imagen falló, maneja el error si es necesario.
    }

    private void OnDestroy()
    {
        // Limpieza del imageDownloader cuando se destruye el objeto
        imageDownloader.Dispose();
    }
}

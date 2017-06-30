using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    private ReadJSON readJSON;
    public GameObject sphere3;
    private Scene sceneTemp;
    private Photo itemTemp;
    public Text Texto_1;
    public string ola ;
    private int cont = 0;




    public void Start() {
        //Texto_1.text = "El texto esta vacio par de putos";
        StartCoroutine("Start2");
    }





    IEnumerator Start2()
    {
        
      
        readJSON = GameObject.Find("sphere3").GetComponent<ReadJSON>();     //Capturamos el objeto sphere3 y todo su contenido
       
        while (readJSON.end == false)
        {                              //Esperamos que termine de leer el JSON
            
           
            cont++;
            yield return new WaitForSeconds(0.2f);
        }
       
        if (readJSON.end == true){
           
            sphere3 = gameObject;
            sceneTemp = readJSON.scene;                                    //Rescatamos el objeto scene que contiene toda la informacion del JSON   
            ApplyScene(sceneTemp.firstPhotoId);                            //LLamamos a aplicar la primera escena por primera vez, rescantando el firsphotoId
                                                                           //Buscamos los tootltip que tiene la imagen
        }else
        {
            
        }
       
    }







    IEnumerator DownloadImage(Photo photo)
    {                             //Metodo para descargar una imagen de internet 


     
        if(ola == "") {
            //Texto_1.text = "caca2";

        }else
        {

           
            Texto_1.text = photo.uri;

        }
        WWW www = new WWW(photo.uri);
        yield return www;
        Texture text = new Texture();            
        sphere3.GetComponent<Renderer>().material.mainTexture = www.texture;
        

    }

    


    public void ApplyScene(int nextId)
    {                                    //Metodo para aplicar y cargar cada imagen
        for (int i = 0; i < sceneTemp.photosList.Count; i++)
        {
            if ((sceneTemp.photosList[i].photo_id) == nextId)
            {
                itemTemp = sceneTemp.photosList[i];
               StartCoroutine("DownloadImage", itemTemp);
                //Debug.Log(itemTemp.scaudio);Realiza la descarga desde internet de la imagen a mostrar en la escena 
               


            }
        }

    }












}

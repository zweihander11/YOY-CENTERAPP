using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CreacionReferencias : MonoBehaviour {
    public List<string> imagenes;   
    string url;
    public List<string> urls;   
    public int num_referencias=0,count=0, aux_numreferencias=0,ref_1=0,ref_2=0,ref_3=0,ref_4=0,num_imagen,max_fotos=0,a=0,orden=1,num_orden=0;
    public string var;
    public Text Texto_1;
    public Text Texto_2;
    public GameObject sphere3;
    public InputField Input;
    public Button Button_1; //Boton imagen siguiente//
    public Button Button_2; //Boton aceptar (definir orden)//
    public Button Button_6; //Boton destruir universo//
    public Button Button_4; //Boton (inmgresar referencia)//
    public Button Button_5; //Boton imagen anterior//
    public Button Button_7; //Boton inicializar (ingresar numero imagen)//
    public Button Button_8; //Boton (establecer referencias a imagen)//

    









    // Use this for initialization
    void Start () {        
       // EliminarFichero();        
        DefinirImagenes();
             
        
       

    }
    public void DefinirImagenes(){
        Button_1.gameObject.SetActive(false);
        Button_2.gameObject.SetActive(false);
        Button_5.gameObject.SetActive(false);
        Button_4.gameObject.SetActive(false);
        
        Button_8.gameObject.SetActive(false);
        var = "Ingrese el numero de imagenes que existen";
        SetText(var, Texto_1);        
    }


    public void Inicializar(){
        max_fotos = int.Parse(Input.text);
        Input.gameObject.SetActive(false);       
        //Button_4.gameObject.SetActive(true);
        Button_1.gameObject.SetActive(true);
        Button_5.gameObject.SetActive(true);
        Button_2.gameObject.SetActive(true);
        Button_7.gameObject.SetActive(false);
        //GameObject.Find("Input");     
        //variable_1 = new TextoDinamico();
        var = "Defina la imagen en la posicion: " + (orden);
        SetText(var, Texto_1);
        var = "Imagen Nº: " + (num_imagen + 1);
        SetText(var, Texto_2);
        CargaUrls();
        StartCoroutine("DescargaImagen");
    

    }

    public void IncrementarPosicion(){
        orden++;
   


    }
    public void CargaUrls(){
        int i;
        urls = new List<string>();         
        //transform.gameObject.GetComponent<Text>().text = "123";
        // Texto.text = "hola";
        for (i = 1; i <= max_fotos; i++){
            url = ("http://res.cloudinary.com/dubte7kn2/image/upload/CentroArte/1_" + i + ".jpg");
            urls.Add(url);
            //  esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // esfera.transform.position = new Vector3((UnityEngine.Random.value * 461) + 10, (UnityEngine.Random.value * 300) + 10, 0F);
            // esfera.transform.localScale += new Vector3(9F, 9F, 9F);
            // esferas.Add(esfera);
            //Debug.Log("Imagen url: " + urls[i]);
        }
    }


    

    public void EstablecerOrden(){      
        Input.gameObject.SetActive(true);      
        Button_2.gameObject.SetActive(false);     
        Button_4.gameObject.SetActive(true);
        num_imagen = a;
        num_orden = a;
        var = "Establezca las referencias para orden: "+orden+" del mapa";
        SetText(var, Texto_1);
        var = "Imagen Nº: " + (num_imagen + 1);
        SetText(var, Texto_2);
        Button_2.gameObject.SetActive(false);

    }
       
   public void IngresarReferencia() {
        num_referencias = int.Parse(Input.text);       
        Input.gameObject.SetActive(false);        
       // var = "Establezca las " + num_referencias + " referencias para la imagen: " + num_imagen;
        //SetText(var);
        aux_numreferencias = num_referencias;
        Button_4.gameObject.SetActive(false);       
        Button_8.gameObject.SetActive(true);
        var = "Establezca las " + num_referencias + " referencias para orden: " + (orden) + " del mapa";
        SetText(var, Texto_1);
        var = "Imagen Nº: " + (num_imagen + 1);
        SetText(var, Texto_2);
        

    }   

    public void EstablecerReferencias(){

        aux_numreferencias--;
        if (count< num_referencias ){         
            var = "Se ha establecido la referencia";
            SetText(var, Texto_1);
            var = "Imagen Nº: " + (num_imagen + 1);
            SetText(var, Texto_2);
            

            if (count == 0){
                ref_1 = a+1;
            }
            if (count == 1){
                ref_2 = a+1;
            }
            if (count == 2){
                ref_3 = a+1;
            }
            if (count == 3){
                ref_4 = a+1;
            }
            count++;
      
        }
        if (aux_numreferencias <= 0) {     

            Debug.Log("Se ha reiniciado");
                count = 0;
                Guardar_informacion();
                IncrementarPosicion();
                Reiniciar();
            }
       
               
        
        
    }


    private void SetText(string var, Text Texto){
        Texto.text = var;
    }


    public void AnteriorImagen(){
        if (a == 0){
            a = max_fotos - 1;
            StartCoroutine("DescargaImagen");
            

        }
        else{
            a--;
            StartCoroutine("DescargaImagen");
            
        }
        
    }

    public void SiguienteImagen(){
        if (a == max_fotos - 1) {
            a = 0;
            StartCoroutine("DescargaImagen");
            

        }
        else{

            a++;
            StartCoroutine("DescargaImagen");
            
        }
    }

    public void DestruirUniverso(){
        Application.Quit();
        

    }

    IEnumerator DescargaImagen(){
        num_imagen = a;
        if (a <= max_fotos && a>= 0){
            var = "Imagen Nº: " + (num_imagen+1);
            SetText(var, Texto_2);
            //Debug.Log("Descargando url:" + urls[a]);
            WWW www = new WWW(urls[a]);
            yield return www;
            //Renderer renderer = GetComponent<Renderer>();
            //renderer.material.mainTexture = www.texture;
            sphere3.GetComponent<Renderer>().material.mainTexture = www.texture as Texture;
        }
      
            

           
        
    }

    private void Guardar_informacion(){ //Metodo para guardar el fichero y la posicion de la camara//        
        //string lines = "Imagen numero:" +index+" Establecido el norte en angulo euler:"+pos;
        //string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");
        System.IO.StreamWriter fichero = new System.IO.StreamWriter(@"Assets\Resources\ImagenesReferencias.txt", true);
        //System.IO.StreamWriter fichero = new System.IO.StreamWriter("ImagenesReferencias.txt", true);
        //fichero.WriteLine("\n Orden"+orden+ "Imagen" + (num_orden+1) + "referencias" + num_referencias + "Refencias"+ref_1+"-"+ref_2+"-"+ref_3+"-" + ref_4+"\n"); // Lo mismo que cuando escribimos por consola       
        fichero.WriteLine("\n"+max_fotos+"-"+orden+ "-" + (num_orden+1)+ "-"+(urls[num_orden])+ "-" + num_referencias + "-"+ref_1+"-"+ref_2+"-"+ref_3+"-" + ref_4+"\n"); // Lo mismo que cuando escribimos por consola       
        fichero.Close(); // Al cerrar el fichero nos aseguramos que no queda ningún dato por guardar        
    }

    private void EliminarFichero(){    //Metodo para eliminar fichero//
         // Delete a file by using File class static method...
        if (System.IO.File.Exists(@"Assets\Resources\ImagenesReferencias.txt"))
        {
            // Use a try block to catch IOExceptions, to
            // handle the case of the file already being
            // opened by another process.
            try
            {
                System.IO.File.Delete((@"Assets\Resources\ImagenesReferencias.txt"));
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }


    private void Reiniciar(){
        
        Input.gameObject.SetActive(false);
        Button_4.gameObject.SetActive(false);     
        Button_2.gameObject.SetActive(true);
        Button_8.gameObject.SetActive(false);
        var = "Defina la imagen en la posicion: " + (orden);
        SetText(var, Texto_1);
        a = 0;        
        StartCoroutine("DescargaImagen");

    }
    private void ReiniciarReferencias() {
        ref_1 = 0;
        ref_2 = 0;
        ref_3 = 0;
        ref_4 = 0;
    }


    void Update(){
        //if (Input.GetKeyDown("f"))
        //{
         //   StartCoroutine("DescargaHebras");
        //}
    }



}


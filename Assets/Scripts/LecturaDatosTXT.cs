using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;


public class LecturaDatosTXT : MonoBehaviour {
    public GameObject sphere3;
    string url;
    public List<string> urls;
    public List<InfoOrden> Infos;
    int max_fotos=2, pos,i=0;   
    public CreateMap crearmapa;

    public static LecturaDatosTXT instance;
    //string file = null;
    



    // Use this for initialization
    void Start () {
        crearmapa = GameObject.Find("sphere3").GetComponent<CreateMap>();
        LeerFicheroReferencias();

        Inicializar();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Inicializar(){
        CargaUrls();        
        StartCoroutine("DescargaImagen");



    }

    IEnumerator DescargaImagen(){

        if (pos <= max_fotos && pos >= 0)
        {

            //Debug.Log("Descargando url:" + urls[a]);
            WWW www = new WWW(urls[pos]);
            yield return www;
            //Renderer renderer = GetComponent<Renderer>();
            //renderer.material.mainTexture = www.texture;
            sphere3.GetComponent<Renderer>().material.mainTexture = www.texture as Texture;
        }
    }



     public void CargaUrls(){
        
        int i;
        urls = new List<string>();
        //transform.gameObject.GetComponent<Text>().text = "123";
        // Texto.text = "hola";
        for (i = 1; i <= max_fotos; i++)
        {
            url = ("http://res.cloudinary.com/dubte7kn2/image/upload/CentroArte/1_" + i + ".jpg");
            urls.Add(url);
            //  esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // esfera.transform.position = new Vector3((UnityEngine.Random.value * 461) + 10, (UnityEngine.Random.value * 300) + 10, 0F);
            // esfera.transform.localScale += new Vector3(9F, 9F, 9F);
            // esferas.Add(esfera);
            //Debug.Log("Imagen url: " + urls[i]);
        }
    }



    /*void ObtenerData()
    {
        if (File.Exists(path))
        {
            this.file = File.ReadAllText(path);
            return;
        }
        StartCoroutine("DownloadData");
    }


    IEnumerator DownloadData()
    {
        WWW www = new WWW(url);
        yield return www;
        // Check www is right
        this.file = www.text;
        File.WriteAllText(path, this.file);
        Debug.Log(file);
    }


    */



    public void LeerFicheroReferencias(){
        
        int i = 0, orden=0, num_imagen = 0, num_referencias = 0, ref_1 = 0, ref_2 = 0, ref_3 = 0, ref_4 = 0;
        int counter = 0;
        string line,url=" " ;
        //ObtenerData();
        List<InfoOrden> Infos = new List<InfoOrden>();
        // Read the file and display it line by line.  
        System.IO.StreamReader file = new System.IO.StreamReader(@"Assets\Resources\ImagenesReferencias.txt");
        while ((line = file.ReadLine()) != null){
            string[] lines = Regex.Split(line, "-");
            foreach (string lineas in lines){

                //Segun el fichero: 1-2-3-4-5-6-7//
                //1:Numero orden; 2:Numero de imagen; 3:Numero de referencias; 4:Referencia 1; 5: Referencia 2; 6:Referencia 3; 7:Referencia 4//mg n,.khb               
                switch (i)
                {
                    case 0:
                        Console.WriteLine("Case 1");
                        //max_fotos = Int32.Parse(lines[i]);
                        max_fotos = int.Parse(lines[i]);
                        //Debug.Log(max_fotos);
                        break;
                    case 1:
                        Console.WriteLine("Case 1");
                        //orden = Int32.Parse(lines[i]);
                        orden = int.Parse(lines[i]);
                        //Debug.Log(orden);
                        break;
                    case 2:
                        Console.WriteLine("Case 2");
                        // num_imagen = Int32.Parse(lines[i]);
                        num_imagen = int.Parse(lines[i]);
                        //Debug.Log(num_imagen);
                        break;
                    case 3:
                        Console.WriteLine("Case 2");
                        //num_referencias = Int32.Parse(lines[i]);
                        url = lines[i];
                        //Debug.Log(num_referencias);
                        break;
                    case 4:
                        Console.WriteLine("Case 2");
                        //num_referencias = Int32.Parse(lines[i]);
                        num_referencias = int.Parse(lines[i]);
                        //Debug.Log(num_referencias);
                        break;
                    case 5:
                        Console.WriteLine("Case 2");
                        //ref_1 = Int32.Parse(lines[i]);
                        ref_1 = int.Parse(lines[i]);
                        //Debug.Log(ref_1);
                        break;
                    case 6:
                        Console.WriteLine("Case 2");
                        //ref_2 = Int32.Parse(lines[i]);
                        ref_2 = int.Parse(lines[i]);
                        //Debug.Log(ref_2);
                        break;
                    case 7:
                        Console.WriteLine("Case 2");
                        //ref_3 = Int32.Parse(lines[i]);
                        ref_3 = int.Parse(lines[i]);
                        //Debug.Log(ref_3);
                        break;
                    case 8:
                        Console.WriteLine("Case 2");
                        //ref_4 = Int32.Parse(lines[i]);
                        ref_4 = int.Parse(lines[i]);
                        //Debug.Log(ref_4);
                        break;  
                    default:
                        Debug.Log("error");
                        break;
                       
                }

                
                i++;
                if (i == 9)
                {
                    i = 0;
                }
            }
            
            Infos.Add(new InfoOrden { Max_fotos = max_fotos, Orden = orden, Num_imagen = num_imagen, Url = url, Num_referencia = num_referencias, Ref_1 = ref_1, Ref_2 = ref_2, Ref_3 = ref_3, Ref_4 = ref_4 });
            counter++;
        }

        file.Close();
        // Suspend the screen.   
       
        //crearmapa.Inicializar(Infos);         
    }












}

using Newtonsoft.Json.Linq; //JSON:in käsittelyä varten, parempi kuin Unityn oma
using System.Collections; //IEnumerator (Coroutinea) varten
using UnityEngine;
using UnityEngine.InputSystem; //(new) Input System,  Keyboard varten. Jos ei löydy, asenna package managerista Input System (Unity Registry)
using UnityEngine.Networking; //UnityWebRequest varten

public class ServerConnection : MonoBehaviour
{
    string address;

    [SerializeField] GameObject cubePrefab; //prefab, joka instansioidaan kun välilyöntiä painetaan

    void Start()
    {
        address = "http://localhost:3000/unitytest";
    }

    private void Update()
    {
        //välilyöntiä painetaan

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Kun unitystä tehdään pyyntö eli request 
            // serverille; jää Unity odottamaan mahdollista vastausta
            // eli responsea
            StartCoroutine(GetWebData(address)); //Coroutine, koska vastauksessa voi kestää
        }
    }

    IEnumerator GetWebData(string address) {

        UnityWebRequest request = UnityWebRequest.Get(address); 
        yield return request.SendWebRequest(); //lähetetään /mysql -reittiin GET-request

        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error: " + request.error);
        }
        else //response tuli Unityyn
        {            
            Debug.Log(request.downloadHandler.text);
            ProcessServerResponse(request.downloadHandler.text);
            
        }       
    }

    void ProcessServerResponse(string rawResponse)
    {
        print("response tuli Unityyn");
        // JSON voi tulla ns.raw-muodossa {"X:"\\5\\"position"\\:-4\\")} jota voi olla hankala itse parsia.
        //Newtonsoftilla on tähän näppärä JObject, "represents JSON Object", joka parsii jsonin.
        JObject json = JObject.Parse(rawResponse);

        if (json["action"].ToString() == "InstantiateCube") //mikäli jsonista löytyy key nimeltään action ja sen value on InstantiateCube
        {
            InstantiateCube(json);
        }


    }

    void InstantiateCube(JObject json)
    {
        Vector3 cubePosition = new Vector3(
            json["position"]["x"].Value<int>(), //tällä syntaksilla saadaan jsonista sisäkkäinen tieto (tässä x-tieto positionista)
            json["position"]["y"].Value<int>(),
            json["position"]["z"].Value<int>()
            );
        Instantiate(cubePrefab, cubePosition, Quaternion.identity);
    }

}

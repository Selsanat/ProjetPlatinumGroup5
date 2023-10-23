using OpenAI_API.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI_API.Images;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class PlanMalicieu2 : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text OutText;
    public Material mat;
    // Start is called before the first frame update
    async Task stp()
    {
        var api = new OpenAI_API.OpenAIAPI("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");
        var chat = api.Chat.CreateConversation();

        /// give instruction as System
        chat.AppendSystemMessage("Tu es un anonciateur dans un jeu vidéo. Ton rôle est de faire des commentaire rigolo, cinglant, presque insultant, cherchant a faire des blagues en te moquant des joueurs. Enerve toi quand on dit le mot teliau. Quand c'est le mot Floriane, soit extrêment bienveillant");

        // give a few examples as user and assistant
        chat.AppendUserInput("Teliau est mort");
        chat.AppendExampleChatbotOutput("Teliau vient encore de mourrir ? Même sa mort était ridicule");
        chat.AppendUserInput("Tim a tué vincent");
        chat.AppendExampleChatbotOutput("C'est un meurtre, que dis-je, un MASSACRE !!!");

        // now let's ask it a question'
        chat.AppendUserInput(text.text);
        // and get the response
        OutText.text = "Ca cherche, 2sec ...";
        string response = await chat.GetResponseFromChatbotAsync();
        OutText.text= response; // "Yes"

        /*// and continue the conversation by asking another
        chat.AppendUserInput("Is this an animal? Chair");
        // and get another response
        response = await chat.GetResponseFromChatbotAsync();
        Console.WriteLine(response); // "No"*/

        // the entire chat history is available in chat.Messages
        /*foreach (ChatMessage msg in chat.Messages)
        {
            print($"{msg.Role}: {msg.Content}");
        }*/
    }

    async Task Dalle()
    {
        var api = new OpenAI_API.OpenAIAPI("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");
        var chat = api.Chat.CreateConversation();
        //async Task<ImageResult> CreateImageAsync(ImageGenerationRequest request);

        // for example
        //var result = await api.ImageGenerations.CreateImageAsync(new ImageGenerationRequest("A drawing of a computer writing a test", 1, ImageSize._512));
        // or
        var result = await api.ImageGenerations.CreateImageAsync(text.text);

        StartCoroutine(GetTexture(result.Data[0].Url));
        print(result.Data[0].Url);
    }

    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            print(www.error);
        }
        else
        {
             mat.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
             
        }
    }
    public void GPT()
    {
        _ = stp();
    }
    public void DalleGenerate()
    {
        _ = Dalle();
    }
}

using OpenAI_API.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlanMalicieu2 : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text OutText;
    // Start is called before the first frame update
    async Task stp()
    {
        var api = new OpenAI_API.OpenAIAPI("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");
        var chat = api.Chat.CreateConversation();

        /// give instruction as System
        chat.AppendSystemMessage("Tu es un anonciateur dans un jeu vidéo. Ton rôle est de faire des commentaire rigolo, cherchant a faire des blagues en te moquant des joueurs.");

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

    public void test()
    {
        _ = stp();
    }
}

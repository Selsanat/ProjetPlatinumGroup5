using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OpenAITool : MonoBehaviour
{
    public string prompt;
    public string Out;
    public string Key;
    public async Task stp()
    {
        var api = new OpenAI_API.OpenAIAPI(Key);
        Out = "Ca charge...";

        var chat = api.Chat.CreateConversation();
        chat.AppendUserInput(prompt);
        Out = await chat.GetResponseFromChatbotAsync();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI_API.Chat;
using OpenAI_API.ChatFunctions;
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

        ChatRequest request = new ChatRequest();
        request.Messages = new List<ChatMessage>();

        api.Chat.CreateChatCompletionAsync(request);
        chat.AppendUserInput(prompt);
        Out = await chat.GetResponseFromChatbotAsync();
    }

}

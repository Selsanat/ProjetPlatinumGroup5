using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.ChatFunctions;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class OpenAITool : MonoBehaviour
{
    public string prompt;
    public string Out;
    public string Key;
    private ChatRequest request;
    OpenAIAPI api;
    public async Task stp()
    {
        api ??= new OpenAI_API.OpenAIAPI(Key);


        request ??= new ChatRequest();

        request.Messages ??= new List<ChatMessage>();
        request.Messages.Add(new ChatMessage(ChatMessageRole.User, prompt));
        //string param = @"{ ""type"" : ""object"",""properties"": { ""texte"": { ""type"": ""string"",""description"": ""Le texte a ecrire"", }, },""required"": [""texte""], }";
        Dictionary<string, object> param = new Dictionary<string, object>
        {
            {"type", "object"},
            {
                "properties", new Dictionary<string, object>
                {
                    {"texte", new Dictionary<string, object>
                        {
                            {"type", "string"}, 
                            { "description", "Le texte a ecrire"}
                        }
                    }
                }
            },
            {"required", new List<string> {"texte"}}
        };
        request.Functions = new List<Function>
        {
            new Function("Ecrire", "Ecrit un texte dans la console", param)
        };
        Out = "Ca charge...";
        var result = await api.Chat.CreateChatCompletionAsync(request);
        ChatChoice firstChoice = result.Choices[0];
        if (firstChoice.FinishReason == "function_call")
        {
            if (firstChoice.Message.FunctionCall.Name == "Ecrire") Ecrire(firstChoice.Message.FunctionCall.Arguments);
            request.Messages.Add(firstChoice.Message);
            request.Messages.Add(new ChatMessage(ChatMessageRole.Function, firstChoice.Message.Content));
            result = await api.Chat.CreateChatCompletionAsync(request.Messages);
            Out = result.Choices[0].Message.Content;
            print(Out);
        }
        else
        {
            request.Messages.Add(firstChoice.Message);
            Out = firstChoice.Message.Content;
            print("wtf");
        }
    }

    void Ecrire(string texte)
    {
        print(texte);
    }

}
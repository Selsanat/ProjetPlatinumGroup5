using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.ChatFunctions;
using UnityEditor;
using UnityEngine;


public class OpenAITool : MonoBehaviour
{
    public string prompt;
    public string Out;
    public string Key;
    public ChatRequest request;
    OpenAIAPI api;
    public string TempFilePath = "Assets/Scripts/PlanMalicieux/ToolMalicieux/test/";
    public bool TempFileExists => System.IO.File.Exists(TempFilePath + "AIExCommandTest.cs");
    public async Task stp()
    {
        api ??= new OpenAI_API.OpenAIAPI(Key);


        request ??= new ChatRequest();

        request.Messages ??= new List<ChatMessage>();
        request.Messages.Add(new ChatMessage(ChatMessageRole.User, prompt));
        //string param = @"{ ""type"" : ""object"",""properties"": { ""texte"": { ""type"": ""string"",""description"": ""Le texte a ecrire"", }, },""required"": [""texte""], }";
        Dictionary<string, object> EcrireParam = new Dictionary<string, object>
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
            {"required", new List<string> {"texte"}},
        };
        Dictionary<string, object> ExCreatedCodeParam = new Dictionary<string, object>
        {
            {"type", "object"},
            {
                "properties", new Dictionary<string, object>
                {
                    {"prompt", new Dictionary<string, object>
                        {
                            {"type", "string"},
                            { "description", "Instructions du joueur"}
                        }
                    }
                }
            },
            {"required", new List<string> {"prompt"}},
        };
        Dictionary<string, object> CreateCodeParam = new Dictionary<string, object>
        {
            {"type", "object"},
            {
                "properties", new Dictionary<string, object>
                {
                    {"prompt", new Dictionary<string, object>
                        {
                            {"type", "string"},
                            { "description", "Description de ce que le code a ecrire doit faire"}
                        }
                    }
                }
            },
            {"required", new List<string> {"prompt"}},
        };
        request.Functions = new List<Function>
        {
            new Function("Ecrire", "Ecrit un texte dans la console", EcrireParam),
            new Function("ExCreatedCode", "Does what the user instructed from the prompt. It changes the scene/environment according to what has been asked in the prompt", ExCreatedCodeParam),
            new Function("CreateScript", "Gives/generate the script asked by the User. This function can only be called if the user expressly asked for a script", CreateCodeParam)
        };
        Out = "Ca charge...";
        print("envoie de requete");
        var result = await api.Chat.CreateChatCompletionAsync(request);
        Out = "";
        ChatChoice firstChoice = result.Choices[0];
        
        if (firstChoice.FinishReason == "function_call")
        {
            print(firstChoice.Message.FunctionCall.Name);
            if (firstChoice.Message.FunctionCall.Name == "python")
            {
                stp();
                return;
            }
            if (firstChoice.Message.FunctionCall.Name == "Ecrire") Ecrire(firstChoice.Message.FunctionCall.Arguments);
            if (firstChoice.Message.FunctionCall.Name == "ExCreatedCode")
            {
                ExCreatedCode(firstChoice.Message.FunctionCall.Arguments);
                print("ExCreatedCode a fait :");
            }
            if (firstChoice.Message.FunctionCall.Name == "CreateScript")
            {
                CreateScript(firstChoice.Message.FunctionCall.Arguments);
                print("CreateScript a fait :");
            }
            request.Messages.Add(firstChoice.Message);
            /* request.Messages.Add(new ChatMessage(ChatMessageRole.Function, firstChoice.Message.Content));
             result = await api.Chat.CreateChatCompletionAsync(request.Messages);
             Out = result.Choices[0].Message.Content;
             print(Out);*/
        }
        else
        {
            request.Messages.Add(firstChoice.Message);
            Out = firstChoice.Message.Content;
        }
    }

    void Ecrire(string texte)
    {
        print(texte);
    }

    async Task ExCreatedCode(string prompt)
    {
        ChatRequest requete = new ChatRequest();
        requete.Messages = new List<ChatMessage>();
        string instruction = "Write a Unity Editor script in c#.\n" +
            " - It provides its functionality as a menu item placed \"Edit\" > \"Do Task\".\n" +
            " - It doesn't provide any editor window. It immediately does the task when the menu item is invoked.\n" +
            " - Don't use GameObject.FindGameObjectsWithTag.\n" +
            " - There is no selected object. Find game objects manually.\n" +
            " - I only need the script body. Don't add any explanation.\n" +
            " - Do not specify the code language" +
            " - If you have to declare a class, add the prefix `Class` " +
            "The task is described as follows:\n" + prompt;

        requete.Messages.Add(new ChatMessage(ChatMessageRole.System, instruction));
        var conv = await api.Chat.CreateChatCompletionAsync(requete);

        Out = conv.Choices[0].Message.Content;
        Out = Out.Replace("```csharp", string.Empty);
        Out = Out.Replace("csharp", string.Empty);
        Out = Out.Replace("```", string.Empty);
        Out = Out.Replace("c#", string.Empty);

        string Usings = "using System.Collections;\n" +
                        "using System.Collections.Generic;\n" +
                        "using UnityEditor;\n" +
                        "using UnityEngine;\n";

        // UnityEditor internal method: ProjectWindowUtil.CreateScriptAssetWithContent
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAssetWithContent", flags);
        method.Invoke(null, new object[] { TempFilePath + "AIExCommandTest.cs", Usings + Out });
    }

    async Task CreateScript(string prompt)
    {
        ChatRequest requete = new ChatRequest();
        requete.Messages = new List<ChatMessage>();
        string instruction = "Write a script in c#.\n" +
                             " - It provides its functionality \n" +
                             " - It does the task when attached to a gameobject in play.\n" +
                             " - Don't use GameObject.FindGameObjectsWithTag.\n" +
                             " - There is no selected object. Find game objects manually.\n" +
                             " - I only need the script body. Don't add any explanation.\n" +
                             " - Do not specify the code language" +
                             " - If you have to declare a class, add the prefix `Class` " +
                             "The task is described as follows:\n" + prompt;

        requete.Messages.Add(new ChatMessage(ChatMessageRole.System, instruction));
        var conv = await api.Chat.CreateChatCompletionAsync(requete);

        Out = conv.Choices[0].Message.Content;
        Out = Out.Replace("```csharp", string.Empty);
        Out = Out.Replace("csharp", string.Empty);
        Out = Out.Replace("```", string.Empty);
        Out = Out.Replace("c#", string.Empty);

        string Usings = "using System.Collections;\n" +
                        "using System.Collections.Generic;\n" +
                        "using UnityEditor;\n" +
                        "using UnityEngine;\n";

        // UnityEditor internal method: ProjectWindowUtil.CreateScriptAssetWithContent
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAssetWithContent", flags);
        method.Invoke(null, new object[] { TempFilePath+ "AICommandTest.cs", Usings + Out });
    }
}
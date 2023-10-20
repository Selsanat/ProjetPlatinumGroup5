using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Whisper.net;
using Whisper.net.Ggml;

public class PlanMalicieu3 : MonoBehaviour
{
    async Task Blabla()
    {
        var modelName = "ggml-base.bin";
        if (!File.Exists(modelName))
        {
            using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(GgmlType.Base);
            using var fileWriter = File.OpenWrite(modelName);
            await modelStream.CopyToAsync(fileWriter);
        }

        using var whisperFactory = WhisperFactory.FromPath("ggml-base.bin");

        using var processor = whisperFactory.CreateBuilder()
            .WithLanguage("auto")
            .Build();
    }
}

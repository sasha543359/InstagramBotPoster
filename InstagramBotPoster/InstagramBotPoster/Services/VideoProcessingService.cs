using FFMpegCore;

namespace InstagramBotPoster.Services;

internal class VideoProcessingService
{
    static VideoProcessingService()
    {
        GlobalFFOptions.Configure(options => options.BinaryFolder = @"C:\ffmpeg\bin");
    }

    public async Task ProcessVideoAsync(string inputVideoPath, string outputVideoPath)
    {
        await FFMpegArguments
               .FromFileInput(inputVideoPath)
               .OutputToFile(outputVideoPath, true, options => options
                   .WithCustomArgument("-vf \"scale=1440:2560\"")  // Масштабируем до 1440x2560
                   .WithFramerate(60)  // FPS 60
                                       // .WithVideoBitrate(12816000)  // 12816 kbps
                   .WithCustomArgument("-map_metadata -1")  // Удаление метаданных

                   // Аудио настройки
                   // .WithAudioBitrate(314000)  // 314 kbps
                   .WithAudioSamplingRate(48000)  // 48 kHz
                   .WithCustomArgument("-ac 2")  // Стерео
               )
               .ProcessAsynchronously();

        Console.WriteLine("Видео успешно обработано и сохранено в формате 9:16 с 60 FPS.");
    }
}

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
                                .WithCustomArgument("-vf \"scale=1080:1920\"")  // Масштабируем до 1080x1920
                                .WithFramerate(60)  // FPS 60 (стабильный и стандартный для Instagram)
                                .WithVideoBitrate(4500000)  // Средний битрейт 4.5 Мбит/с
                                .WithCustomArgument("-minrate 3500000")  // Минимальный битрейт 3.5 Мбит/с
                                .WithCustomArgument("-maxrate 8000000")  // Максимальный битрейт 5 Мбит/с
                                .WithCustomArgument("-bufsize 12000000")  // Размер буфера 10 Мбит
                                .WithCustomArgument("-map_metadata -1")  // Удаление метаданных

                                // Аудио настройки
                                .WithAudioBitrate(128000)  // 128 kbps (Instagram стандарт)
                                .WithAudioSamplingRate(44100)  // 44.1 kHz (рекомендуется Instagram)
                                .WithCustomArgument("-ac 2")  // Стерео
                         )
                         .ProcessAsynchronously();

        Console.WriteLine("Видео успешно обработано и сохранено в формате 9:16 с 60 FPS.");
    }
}

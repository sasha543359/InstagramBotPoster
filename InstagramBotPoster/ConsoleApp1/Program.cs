using FFMpegCore;
using System;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GlobalFFOptions.Configure(options => options.BinaryFolder = @"C:\ffmpeg\bin");

            string videosPath = @"C:\Users\Computer\Desktop\noprocessedVideo";
            string resultsPath = @"C:\Users\Computer\Desktop\ResultsVideo";

            // Проверим, существует ли папка с видео
            if (!Directory.Exists(videosPath))
            {
                Console.WriteLine($"Папка с видео не найдена: {videosPath}");
                return;
            }

            // Проверим, существует ли папка для результатов, если нет — создадим
            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }

            // Получаем все файлы с расширениями .mp4, .mov и т.д.
            var videoFiles = Directory.GetFiles(videosPath, "*.*", SearchOption.TopDirectoryOnly)
                                      .Where(f => f.EndsWith(".mp4") || f.EndsWith(".mov") || f.EndsWith(".avi"))
                                      .ToList();

            if (videoFiles.Count == 0)
            {
                Console.WriteLine("В папке нет видео для обработки.");
                return;
            }

            Random random = new();

            foreach (var videoFile in videoFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(videoFile);
                string outputFilePath = Path.Combine(resultsPath, $"{fileName}_processed.mp4");

                Console.WriteLine($"Обработка видео: {videoFile}");

                try
                {
                    FFMpegArguments
                         .FromFileInput(videoFile)
                         .OutputToFile(outputFilePath, true, options => options
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
                         .ProcessSynchronously();

                    Console.WriteLine($"Видео успешно обработано: {outputFilePath}");

                    // Удаляем оригинальный файл после успешной обработки
                    // File.Delete(videoFile);
                    // Console.WriteLine($"Удалено исходное видео: {videoFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке {videoFile}: {ex.Message}");
                }
            }

            Console.WriteLine("Все видео успешно обработаны.");
        }
    }
}

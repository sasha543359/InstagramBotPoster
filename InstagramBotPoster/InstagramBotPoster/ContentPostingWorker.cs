using InstagramBotPoster.Models;
using InstagramBotPoster.Services;

namespace InstagramBotPoster
{
    public class ContentPostingWorker : BackgroundService
    {
        private readonly ILogger<ContentPostingWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public ContentPostingWorker(ILogger<ContentPostingWorker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("������ ContentPostingWorker...");

            var profiles = _configuration.GetSection("InstagramProfiles").Get<List<BrowserProfile>>();

            foreach (var profile in profiles)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                _logger.LogInformation($"�������� ������� ��� �������: {profile.Name}");

                try
                {
                    var contentService = new ContentPostingService(profile);
                    contentService.StartPosting();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"������ ��� ��������� ������� {profile.Name}: {ex.Message}");
                }

                await Task.Delay(2000, stoppingToken); // �������� ����� ���������
            }

            _logger.LogInformation("��� ������� ����������. ��������� ������...");

            Environment.Exit(0);
        }
    }
}

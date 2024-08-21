using MastodonBot.Core.Config;
using MastodonBot.Core.Repositories;
using MastodonBot.Core.Services;
using MastodonBot.Umbraco.Migrations;
using MastodonBot.Umbraco.Repository;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace MastodonBot.Umbraco
{
    public class Composer : ComponentComposer<MastodonTootTrackingDbComponent>
    {
        public override void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddOptions<MastodonBotConfig>()
                .Bind(builder.Config.GetSection(MastodonBotConfig.Key));
            builder.Services.AddTransient<IStatusRecordRepository, StatusRecordRepository>()
                .AddTransient<IMastodonService, MastodonService>();
            base.Compose(builder);
        }
    }
}

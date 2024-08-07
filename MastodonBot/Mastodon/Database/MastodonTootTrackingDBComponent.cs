using MastodonBot.Mastodon.Migrations;
using Microsoft.AspNetCore.Components;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace MastodonBot.Mastodon.Database
{
	public class MastodonTootTrackingDBComponent : IComponent, Umbraco.Cms.Core.Composing.IComponent
	{
		private readonly ICoreScopeProvider _coreScopeProvider;
		private readonly IMigrationPlanExecutor _migrationPlanExecutor;
		private readonly IKeyValueService _keyValueService;
		private readonly IRuntimeState _runtimeState;

		public MastodonTootTrackingDBComponent(ICoreScopeProvider coreScopeProvider,
			IMigrationPlanExecutor migrationPlanExecutor, IKeyValueService keyValueService, IRuntimeState runtimeState)
		{
			_coreScopeProvider = coreScopeProvider ?? throw new ArgumentNullException(nameof(coreScopeProvider));
			_migrationPlanExecutor = migrationPlanExecutor ?? throw new ArgumentNullException(nameof(migrationPlanExecutor));
			_keyValueService = keyValueService ?? throw new ArgumentNullException(nameof(keyValueService));
			_runtimeState = runtimeState ?? throw new ArgumentNullException(nameof(runtimeState));
		}

		public void Attach(RenderHandle renderHandle)
		{
			throw new NotImplementedException();
		}

		public void Initialize()
		{
			if (_runtimeState.Level < RuntimeLevel.Run)
			{
				return;
			}

			// Create a migration plan for a specific project/feature
			// We can then track that latest migration state/step for this project/feature
			var migrationPlan = new MigrationPlan("MastodonToots");

			// This is the steps we need to take
			// Each step in the migration adds a unique value
			migrationPlan.From(string.Empty)
				.To<AddMastodonTable>("mastodon-toots");

			// Go and upgrade our site (Will check if it needs to do the work or not)
			// Based on the current/latest step
			var upgrader = new Upgrader(migrationPlan);
			upgrader.Execute(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
		}

		public Task SetParametersAsync(ParameterView parameters)
		{
			throw new NotImplementedException();
		}

		public void Terminate()
		{
		}
	}
}
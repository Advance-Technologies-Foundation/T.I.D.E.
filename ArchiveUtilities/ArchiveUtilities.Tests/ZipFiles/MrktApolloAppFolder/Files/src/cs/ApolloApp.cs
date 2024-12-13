using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using ATF.Repository.Providers;
using Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using MrktApolloApp.CustomException;
using MrktApolloApp.DomainModels;
using MrktApolloApp.RestClient;
using MrktApolloApp.Services;
using MrktApolloApp.Utils;
using MrktApolloApp.VirtualEntityQueryExecutor;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Messaging.Common;
using WebSocket = MrktApolloApp.Services.WebSocket;

namespace MrktApolloApp
{
	public class ApolloApp : IApolloApp
	{

		#region Constants: Private

#pragma warning disable S1072
		private const string BaseUrlSysSettingsCode = "MrktApolloBaseApiUrl";
#pragma warning restore S1072

		#endregion

		#region Fields: Private

		private readonly Collection<IDisposable> _disposables = new Collection<IDisposable>();
		private readonly ServiceCollection _serviceCollection = new ServiceCollection();
		private readonly IServiceScope _scope;
		private readonly ILog _logger = LogManager.GetLogger("ApolloApp");

		[ExcludeFromCodeCoverage]
		private static IMsgChannelManager GetMsgChannelManager(){
			try {
				return MsgChannelManager.Instance;
			} catch (Exception e) {
				return null;
			}
		}

		#endregion

		#region Fields: Internal

		internal Func<IApolloApp, string, EnrichedAccount> EnrichAccountFunc =
			(app, domain) => app.GetService<IAccountEnrichmentServe>()
				.GetEnrichedData(domain);

		internal Func<IApolloApp, Guid, EnrichedPerson> EnrichPersonFunc =
			(app, contactId) => app.GetService<IPersonEnrichmentService>()
				.GetEnrichedData(contactId);

		#endregion

		#region Constructors: Public

		public ApolloApp(){
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			IMsgChannelManager msgChannelManager = GetMsgChannelManager();
			if (msgChannelManager != null) {
				_serviceCollection.AddSingleton(msgChannelManager);
			}
			_serviceCollection.AddTransient<ApiKeyMessageHandler>();
			_serviceCollection.AddTransient<ISysSettingsUtil, SysSettingsUtil>();
			_serviceCollection.AddHttpClient("ApolloApi", (sp, client) => {
					var sysSettingsUtil = sp.GetService<ISysSettingsUtil>();
					var baseUrl = sysSettingsUtil.GetSysSettingValueByCode(BaseUrlSysSettingsCode).ToString();
					
					client.BaseAddress = new Uri(baseUrl);
					client.DefaultRequestHeaders.Add("Accept", "application/json");
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue {
						NoCache = true
					};
				})
				.AddHttpMessageHandler(provider => new ApiKeyMessageHandler(userConnection));
			_serviceCollection.AddTransient<IWebSocket, WebSocket>();
			_serviceCollection.AddSingleton(_logger);
			_serviceCollection.AddTransient<IApolloRestClient, ApolloRestClient>();
			_serviceCollection.AddTransient<IAccountEnrichmentServe, AccountEnrichmentServe>();
			_serviceCollection.AddTransient<IPersonEnrichmentService, PersonEnrichmentService>();
			_serviceCollection.AddTransient<IDataProvider>(sp =>
				new LocalDataProvider(ClassFactory.Get<UserConnection>()));

			_serviceCollection.AddTransient<IEntityMapper, EntityMapper>(sp =>
				new EntityMapper(userConnection, sp.GetRequiredService<IDataProvider>()));
			
			_serviceCollection.AddTransient<IEsqFilterParser, EsqFilterParser>();
			_serviceCollection.AddTransient<IEsqColumnParser, EsqColumnParser>();

			_logger.InfoFormat("Building container for {0}", "ApolloApp");
			ServiceProvider container = _serviceCollection.BuildServiceProvider(true);
			_logger.InfoFormat("Succeeded building container for {0}", "ApolloApp");
			_disposables.Add(container);
			_scope = container.CreateScope();
			_disposables.Add(_scope);
		}

		#endregion

		#region Methods: Protected

		[ExcludeFromCodeCoverage]
		protected virtual void Dispose(bool disposing){
			foreach (IDisposable disposable in _disposables) {
				disposable?.Dispose();
			}
		}

		#endregion

		#region Methods: Public

		[ExcludeFromCodeCoverage]
		public void Dispose(){
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public EnrichedAccount EnrichAccount(string domain) => EnrichAccountFunc(this, domain);

		public EnrichedPerson EnrichPerson(Guid contactId) => EnrichPersonFunc(this, contactId);

		public void PostWebSocketMessage(string senderName, string commandName, string schemaName, Guid recordId,
			string message, UserConnection userConnection = null) =>
			GetService<IWebSocket>()
				.PostMessage(senderName, commandName, schemaName, recordId, message, userConnection);

		public T GetService<T>(){
			try {
				T service = _scope.ServiceProvider.GetService<T>();
				if (service == null) {
					throw new ServiceNotFoundException<T>();
				}
				return service;
			} catch (Exception ex) {
				_logger.ErrorFormat("Error {0} while resolving service {3}\n{1}\n{2}", ex.GetType(), ex.Message,
					ex.StackTrace, typeof(T).FullName);
				throw;
			}
		}

		#endregion

	}
}
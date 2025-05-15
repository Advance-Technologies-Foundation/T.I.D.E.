using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using Common.Logging;
using Terrasoft.Core;
using Terrasoft.Core.Configuration;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace AtfTIDE {
	public class SysSchemaMonitor {
		private ConcurrentQueue<SysSchema> sysSchemaQueue = new ConcurrentQueue<SysSchema>();
		private const int SleepDelay = 1000;
		
		private static bool _stopRequested = false;
		
		private SysSchemaMonitor(){
			
		}
		
		public static void Start(){
			var userConnection = ClassFactory.Get<UserConnection>();
			EntitySchema entitySchema = userConnection.EntitySchemaManager.GetInstanceByName(nameof(SysSchema));
			DateTime lastCheck = DateTime.UtcNow;
			
			while(!_stopRequested) {
				
				EntitySchemaQuery esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, nameof(SysSchema));
				EntitySchemaQueryColumn id = esq.AddColumn("Id");
				EntitySchemaQueryColumn createdOn = esq.AddColumn("CreatedOn");
				EntitySchemaQueryColumn modifiedOn = esq.AddColumn("ModifiedOn");
				EntitySchemaQueryColumn name = esq.AddColumn("Name");
				EntitySchemaQueryColumn caption = esq.AddColumn("Caption");
				EntitySchemaQueryColumn managerName = esq.AddColumn("ManagerName");
				EntitySchemaQueryColumn descriptor = esq.AddColumn("Descriptor");
				EntitySchemaQueryColumn metadata = esq.AddColumn("MetaData");
				IEntitySchemaQueryFilterItem filter = esq.CreateFilterWithParameters(FilterComparisonType.Greater, "ModifiedOn", lastCheck);
				esq.Filters.Add(filter);
				
				EntityCollection changedEntity = esq.GetEntityCollection(userConnection);
				if(changedEntity.Count == 0) {
					Thread.Sleep(SleepDelay);
					continue;
				}
				lastCheck = DateTime.UtcNow;
				foreach (Entity entity in changedEntity) {
					
					var nameVal = entity.GetTypedColumnValue<string>(name.Name);
					LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", nameVal);
					
					var captionVal = entity.GetTypedColumnValue<string>(caption.Name);
					LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", captionVal);
					
					var managerVal = entity.GetTypedColumnValue<string>(managerName.Name);
					LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", managerVal);
					
					byte[] metadataBytes= entity.GetBytesValue("MetaData");
					string metadataStr = Encoding.UTF8.GetString(metadataBytes);
					LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", metadataStr);
				
					byte[] descriptorBytes = entity.GetBytesValue("Descriptor");
					string descriptorStr = Encoding.UTF8.GetString(descriptorBytes);
					LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", descriptorStr);
					
				}
				
				Thread.Sleep(SleepDelay);
			}
		}
		
		public static void Stop(){
			_stopRequested = true;
		}
		
		
		
		private void GetSourceCode(Guid id){
			var userConnection = ClassFactory.Get<UserConnection>();
			var sysSchemaSourceEntity = userConnection.EntitySchemaManager.GetInstanceByName(nameof(SysSchemaContent)).CreateEntity(userConnection);
			
			
			
			
		}
		
		
	}
}

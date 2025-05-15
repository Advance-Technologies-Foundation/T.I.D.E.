using System;
using System.Text;
using Common.Logging;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;

namespace AtfTIDE.EventListener {
	/// <summary>
	/// Handles event layer for "SysSchema" entity
	/// </summary>
	/// <remarks>
	/// Creatio triggers the mechanism of the Entity event layer after executing the object event subprocesses
	/// See <a href="https://academy.creatio.com/docs/developer/back_end_development/objects_business_logic/overview"> academy</a> documentation for details.
	/// </remarks>
	[EntityEventListener(SchemaName = "SysSchema")]
	internal class SysSchemaEventListener : BaseEntityEventListener {

		#region Methods: Private

		private void OnValidating(object sender, EntityValidationEventArgs e){
			//throw new NotImplementedException();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Occurs on AFTER a record is deleted.
		/// </summary>
		/// <param name="sender">Entity</param>
		/// <param name="e"></param>
		public override void OnDeleted(object sender, EntityAfterEventArgs e){
			base.OnDeleted(sender, e);
			Entity entity = (Entity)sender;
		}

		/// <summary>
		/// Occurs on BEFORE a record is deleted.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnDeleting(object sender, EntityBeforeEventArgs e){
			base.OnDeleting(sender, e);
			Entity entity = (Entity)sender;
			entity.Validating += OnValidating;
		}

		/// <summary>
		/// Occurs on AFTER a record is inserted.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnInserted(object sender, EntityAfterEventArgs e){
			base.OnInserted(sender, e);
			Entity entity = (Entity)sender;
		}

		
		/// <summary>
		/// Occurs on AFTER a record is saved.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnSaved(object sender, EntityAfterEventArgs e){
			base.OnSaved(sender, e);
			Entity entity = (Entity)sender;
			Guid id = entity.GetTypedColumnValue<Guid>("Id");
			LogManager.GetLogger("Common").InfoFormat("Updating contact with id:{0}", id);

			try {
				byte[] metadata =entity.GetBytesValue("MetaData");
				string metadataStr = Encoding.UTF8.GetString(metadata);
				LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", metadataStr);
				
				byte[] descriptor = entity.GetBytesValue("Descriptor");
				string descriptorStr = Encoding.UTF8.GetString(descriptor);
				LogManager.GetLogger("Common").InfoFormat("Metadata String :{0}", descriptorStr);
				
			}
			catch (Exception exception) {
				LogManager.GetLogger("Common").ErrorFormat("Error :{0}\r\n{1}", exception.Message, exception);
			}
			
		}

		

		/// <summary>
		/// Occurs on AFTER a record is updated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnUpdated(object sender, EntityAfterEventArgs e){
			base.OnUpdated(sender, e);
			Entity entity = (Entity)sender;
		}

		
		#endregion

	}
}


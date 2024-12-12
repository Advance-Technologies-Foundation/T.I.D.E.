using System.Diagnostics.CodeAnalysis;
using MrktApolloApp;

namespace Terrasoft.Core.Process.Configuration
{
	
	#region Class: MrktProcessUserTask_ApolloInvokeUiCommand

	
	/// <exclude/>
	public partial class MrktProcessUserTask_ApolloInvokeUiCommand
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {
			ApolloApplication.Instance
				.PostWebSocketMessage(SenderName, CommandName, SchemaName, RecordId, Message);
			return true;
		}

		#endregion

		#region Methods: Public

	
		[ExcludeFromCodeCoverage]
		public override string GetExecutionData() {
			return string.Empty;
		}


		#endregion

	}

	#endregion

}


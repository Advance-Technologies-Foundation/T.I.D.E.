namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: InstallConsoleGitInstallScriptSchema

	/// <exclude/>
	public class InstallConsoleGitInstallScriptSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public InstallConsoleGitInstallScriptSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public InstallConsoleGitInstallScriptSchema(InstallConsoleGitInstallScriptSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("9c3a6004-259d-44b7-a5cc-5eb37b18f89b");
			Name = "InstallConsoleGitInstallScript";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("2a01b7c9-305b-49c3-8cb9-6420254af905");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,0,10,85,142,65,10,2,49,12,69,215,83,152,59,100,169,27,15,160,75,17,153,181,122,128,88,51,67,160,182,37,105,69,144,222,221,150,209,69,55,31,242,146,255,127,192,227,147,52,162,37,184,146,8,106,152,211,238,24,252,204,75,22,76,28,252,104,62,163,25,178,178,95,186,19,161,67,229,49,223,29,91,176,14,85,97,242,154,208,185,106,215,224,232,204,233,7,46,86,56,38,216,195,212,129,211,155,108,78,65,160,21,252,147,94,129,31,176,110,104,115,83,146,154,230,201,182,79,32,119,227,118,245,13,77,74,211,50,154,242,5,10,68,2,21,208,0,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("9c3a6004-259d-44b7-a5cc-5eb37b18f89b"));
		}

		#endregion

	}

	#endregion

}


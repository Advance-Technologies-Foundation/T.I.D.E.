define("Page_Commit", /**SCHEMA_DEPS*/[]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/()/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "CancelButton",
				"values": {
					"color": "default",
					"size": "large",
					"iconPosition": "only-text"
				}
			},
			{
				"operation": "merge",
				"name": "SaveButton",
				"values": {
					"clicked": {
						"request": "crt.RunBusinessProcessRequest",
						"params": {
							"processName": "AtfProcess_SaveWorkspaceToGit",
							"processRunType": "RegardlessOfThePage",
							"saveAtProcessStart": true,
							"showNotification": true
						}
					},
					"caption": "#ResourceString(SaveButton_caption)#"
				}
			},
			{
				"operation": "insert",
				"name": "Input_d70ebn1",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.Input",
					"label": "$Resources.Strings.PageParameters_TextParameter1_rnto2n4",
					"labelPosition": "above",
					"control": "$PageParameters_TextParameter1_rnto2n4",
					"multiline": false
				},
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_CommitMessage",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 3
					},
					"type": "crt.Input",
					"label": "#ResourceString(Input_CommitMessage_label)#",
					"control": "",
					"placeholder": "",
					"tooltip": "#ResourceString(Input_CommitMessage_tooltip)#",
					"readonly": false,
					"multiline": true,
					"labelPosition": "above",
					"visible": true
				},
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 1
			}
		]/**SCHEMA_VIEW_CONFIG_DIFF*/,
		viewModelConfigDiff: /**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [
					"attributes"
				],
				"values": {
					"PageParameters_TextParameter1_rnto2n4": {
						"modelConfig": {
							"path": "PageParameters.RepositoryId"
						}
					}
				}
			}
		]/**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/,
		modelConfigDiff: /**SCHEMA_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [],
				"values": {
					"dataSources": {}
				}
			}
		]/**SCHEMA_MODEL_CONFIG_DIFF*/,
		handlers: /**SCHEMA_HANDLERS*/[
			{
				request: "atf.InstallAllAppsClicked",
				handler: async (request, next) => {
					await next?.handle(request);
					
					const handlerChain = sdk.HandlerChainService.instance;
					
					await handlerChain.process({
						type: 'crt.OpenPageRequest',
						$context: request.$context,
						scopes: [...request.scopes],
						schemaName: "Page_LogTerminal"
					});
					
					await handlerChain.process({
						type: "crt.RunBusinessProcessRequest",
						$context: request.$context,
						scopes: [...request.scopes],
						processName: "AtfProcess_ForceUpdateAutoSyncRepos",
						processRunType: "RegardlessOfThePage",
						saveAtProcessStart: true,
						showNotification: true
					});
					await next?.handle(request);
				}
			},
			
			
			
		]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
define("Page_Commit", /**SCHEMA_DEPS*/["@creatio-devkit/common"]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/(sdk)/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "PageTitle",
				"values": {
					"caption": "#MacrosTemplateString(#ResourceString(PageTitle_caption)#)#",
					"visible": true,
					"headingLevel": "label"
				}
			},
			{
				"operation": "merge",
				"name": "MainContainer",
				"values": {
					"alignItems": "stretch"
				}
			},
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
				"operation": "remove",
				"name": "SaveButton"
			},
			{
				"operation": "insert",
				"name": "Input_lw2u8hn",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 2
					},
					"type": "crt.Input",
					"label": "$Resources.Strings.PageParameters_TextParameter1_pka9oej",
					"labelPosition": "above",
					"control": "$PageParameters_TextParameter1_pka9oej",
					"multiline": true,
					"visible": true,
					"readonly": false,
					"placeholder": "#ResourceString(Input_lw2u8hn_placeholder)#",
					"tooltip": ""
				},
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_DO",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_DO_caption)#",
					"color": "primary",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "atf.ButtonDoClicked"
					},
					"clickMode": "default"
				},
				"parentName": "FooterContainer",
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
					"PageParameters_TextParameter1_pka9oej": {
						"modelConfig": {}
					},
					"PageParameters_RepositoryId": {
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
				request: "atf.ButtonDoClicked",
				handler: async (request, next) => {
					const handlerChain = sdk.HandlerChainService.instance;
					const repositoryId = await request.$context.PageParameters_RepositoryId;
										
					// await handlerChain.process({
					// 	type: 'crt.CancelRecordChangesRequest',
					// 	$context: request.$context,
					// });
					
					const commitMessage = await request.$context.PageParameters_TextParameter1_pka9oej;
					await handlerChain.process({
						type: "crt.RunBusinessProcessRequest",
						$context: request.$context,
						scopes: [...request.scopes],
						processName: "AtfProcess_SaveWorkspaceToGit",
						"showNotification": true,
						processParameters:{
							"Repository": repositoryId,
							"CommitMessage": commitMessage
						}
					});
					return await handlerChain.process({
						type: 'crt.ClosePageRequest',
						$context: request.$context,
					});
				}
			}, 
		]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
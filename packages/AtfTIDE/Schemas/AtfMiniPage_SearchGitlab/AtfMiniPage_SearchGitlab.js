define("AtfMiniPage_SearchGitlab", /**SCHEMA_DEPS*/[]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/()/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "DataGridMain",
				"values": {
					"columns": [
						{
							"id": "c41ad310-1025-430d-d69c-745e0f037608",
							"code": "MainDS_Name",
							"caption": "#ResourceString(MainDS_Name)#",
							"dataValueType": 28
						},
						{
							"id": "ec92f63b-05fe-9ca9-eb0f-f55d1f5fd6d0",
							"code": "MainDS_AtfGitServer",
							"caption": "#ResourceString(MainDS_AtfGitServer)#",
							"dataValueType": 10
						},
						{
							"id": "b24fb2ac-f079-ff41-91e2-f8410bbec092",
							"code": "MainDS_NameSpace",
							"caption": "#ResourceString(MainDS_NameSpace)#",
							"dataValueType": 30
						},
						{
							"id": "2c98cb9b-ecab-3781-46fe-5a31eee9b845",
							"code": "MainDS_CloneUrl",
							"caption": "#ResourceString(MainDS_CloneUrl)#",
							"dataValueType": 44
						},
						{
							"id": "87a75075-ced3-0525-eff5-b502b6132190",
							"code": "MainDS_Description",
							"caption": "#ResourceString(MainDS_Description)#",
							"dataValueType": 43
						}
					],
					"features": {
						"editable": {
							"enable": true,
							"itemsCreation": false,
							"floatingEditPanel": true
						},
						"rows": {
							"selection": {
								"enable": true,
								"multiple": true
							}
						},
						"header": {
							"toolbar": {
								"columnsSelection": false
							}
						}
					},
					"placeholder": false
				}
			}
		]/**SCHEMA_VIEW_CONFIG_DIFF*/,
		viewModelConfigDiff: /**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [
					"attributes"
				],
				"values": {
					"DataGridMain_PredefinedFilter": {
						"value": null
					}
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"DataGridMain",
					"modelConfig"
				],
				"values": {
					"filterAttributes": [
						{
							"name": "SearchFilterMain_DataGridMain",
							"loadOnChange": true
						},
						{
							"loadOnChange": true,
							"name": "DataGridMain_PredefinedFilter"
						}
					]
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"DataGridMain",
					"viewModelConfig",
					"attributes"
				],
				"values": {
					"MainDS_Name": {
						"modelConfig": {
							"path": "MainDS.Name"
						}
					},
					"MainDS_AtfGitServer": {
						"modelConfig": {
							"path": "MainDS.AtfGitServer"
						}
					},
					"MainDS_NameSpace": {
						"modelConfig": {
							"path": "MainDS.NameSpace"
						}
					},
					"MainDS_CloneUrl": {
						"modelConfig": {
							"path": "MainDS.CloneUrl"
						}
					},
					"MainDS_Description": {
						"modelConfig": {
							"path": "MainDS.Description"
						}
					},
					"MainDS_Id": {
						"modelConfig": {
							"path": "MainDS.Id"
						}
					}
				}
			}
		]/**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/,
		modelConfigDiff: /**SCHEMA_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [
					"dataSources",
					"MainDS",
					"config"
				],
				"values": {
					"entitySchemaName": "AtfVirtual_GitLabProject"
				}
			},
			{
				"operation": "merge",
				"path": [
					"dataSources",
					"MainDS",
					"config",
					"attributes"
				],
				"values": {
					"Name": {
						"path": "Name"
					},
					"AtfGitServer": {
						"path": "AtfGitServer"
					},
					"NameSpace": {
						"path": "NameSpace"
					},
					"CloneUrl": {
						"path": "CloneUrl"
					},
					"Description": {
						"path": "Description"
					}
				}
			}
		]/**SCHEMA_MODEL_CONFIG_DIFF*/,
		handlers: /**SCHEMA_HANDLERS*/[
			{
				request: "crt.HandleViewModelAttributeChangeRequest",
				handler: async (request, next) => {
					console.log(request.attributeName);
				}
			}
			
		]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
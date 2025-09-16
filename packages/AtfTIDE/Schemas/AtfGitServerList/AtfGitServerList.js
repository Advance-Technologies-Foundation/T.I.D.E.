define("AtfGitServerList", /**SCHEMA_DEPS*/[]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/()/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "PageTitle",
				"values": {
					"caption": "#MacrosTemplateString(#ResourceString(PageTitle_caption)#)#",
					"visible": true
				}
			},
			{
				"operation": "remove",
				"name": "AddButton"
			},
			{
				"operation": "remove",
				"name": "DataImportButton"
			},
			{
				"operation": "remove",
				"name": "MenuItem_ImportFromExcel"
			},
			{
				"operation": "remove",
				"name": "OpenLandingDesigner"
			},
			{
				"operation": "remove",
				"name": "ActionButton"
			},
			{
				"operation": "remove",
				"name": "MenuItem_ExportToExcel"
			},
			{
				"operation": "merge",
				"name": "MainFilterContainer",
				"values": {
					"visible": true,
					"alignItems": "stretch"
				}
			},
			{
				"operation": "remove",
				"name": "FolderTreeActions"
			},
			{
				"operation": "remove",
				"name": "LookupQuickFilterByTag"
			},
			{
				"operation": "remove",
				"name": "RightFilterContainer"
			},
			{
				"operation": "remove",
				"name": "DataTable_Summaries"
			},
			{
				"operation": "remove",
				"name": "RefreshButton"
			},
			{
				"operation": "remove",
				"name": "FolderTree"
			},
			{
				"operation": "merge",
				"name": "DataTable",
				"values": {
					"columns": [
						{
							"id": "d35bc4ca-ceb2-0277-0c8e-8a93fa2430e1",
							"code": "PDS_Name",
							"caption": "#ResourceString(PDS_Name)#",
							"dataValueType": 28
						},
						{
							"id": "49892df6-6d1d-502b-b3ff-e07e6b1c08e0",
							"code": "PDS_Url",
							"caption": "#ResourceString(PDS_Url)#",
							"dataValueType": 28
						},
						{
							"id": "766be58a-0718-94cf-4cb2-28d57399ae5f",
							"code": "PDS_UserName",
							"caption": "#ResourceString(PDS_UserName)#",
							"dataValueType": 28
						},
						{
							"id": "ccaa921f-fc99-6027-8ce7-f5098c976d16",
							"code": "PDS_AccessToken",
							"caption": "#ResourceString(PDS_AccessToken)#",
							"dataValueType": 28
						},
						{
							"id": "f3527aa4-0dcc-f5c8-f7ff-c750d472d24e",
							"code": "PDS_Default",
							"caption": "#ResourceString(PDS_Default)#",
							"dataValueType": 12,
							"width": 194
						},
						{
							"id": "729d6795-b786-b5fe-f54a-c092a5d08068",
							"code": "PDS_GitRepositoryType",
							"caption": "#ResourceString(PDS_GitRepositoryType)#",
							"dataValueType": 10
						}
					],
					"placeholder": false,
					"visible": true,
					"fitContent": true,
					"referenceSchema": "Entity_18d813a",
					"selectionState": "$DataTable_SelectionState",
					"_selectionOptions": {
						"attribute": "DataTable_SelectionState"
					}
				}
			},
			{
				"operation": "insert",
				"name": "Button_OpenRepositoryList",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_OpenRepositoryList_caption)#",
					"color": "default",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "crt.OpenPageRequest",
						"params": {
							"schemaName": "AtfTIDE_ListPage"
						}
					},
					"clickMode": "default"
				},
				"parentName": "ActionButtonsContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_FindApp",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_FindApp_caption)#",
					"color": "default",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "crt.RunBusinessProcessRequest",
						"params": {
							"processName": "AtfFindAppRepositoriesOnServer",
							"processRunType": "ForTheSelectedRecords",
							"showNotification": true,
							"dataSourceName": "PDS",
							"parameterMappings": {
								"AtfGitServer": "Id"
							},
							"filters": "$Items | crt.ToCollectionFilters : 'Items' : $DataTable_SelectionState | crt.SkipIfSelectionEmpty : $DataTable_SelectionState",
							"sorting": "$ItemsSorting",
							"selectionStateAttributeName": "DataTable_SelectionState"
						}
					},
					"clickMode": "default"
				},
				"parentName": "ActionButtonsContainer",
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
					"Items_PredefinedFilter": {
						"value": null
					}
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"Items",
					"viewModelConfig",
					"attributes"
				],
				"values": {
					"PDS_Name": {
						"modelConfig": {
							"path": "PDS.Name"
						}
					},
					"PDS_Url": {
						"modelConfig": {
							"path": "PDS.Url"
						}
					},
					"PDS_UserName": {
						"modelConfig": {
							"path": "PDS.UserName"
						}
					},
					"PDS_AccessToken": {
						"modelConfig": {
							"path": "PDS.AccessToken"
						}
					},
					"PDS_Default": {
						"modelConfig": {
							"path": "PDS.Default"
						}
					},
					"PDS_GitRepositoryType": {
						"modelConfig": {
							"path": "PDS.GitRepositoryType"
						}
					}
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"Items",
					"modelConfig"
				],
				"values": {
					"filterAttributes": [
						{
							"name": "SearchFilter_Items",
							"loadOnChange": true
						},
						{
							"loadOnChange": true,
							"name": "Items_PredefinedFilter"
						}
					]
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"FolderTree_visible"
				],
				"values": {
					"modelConfig": {}
				}
			}
		]/**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/,
		modelConfigDiff: /**SCHEMA_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [
					"dataSources",
					"PDS",
					"config"
				],
				"values": {
					"entitySchemaName": "AtfGitServer",
					"attributes": {
						"Name": {
							"path": "Name"
						},
						"Url": {
							"path": "Url"
						},
						"UserName": {
							"path": "UserName"
						},
						"AccessToken": {
							"path": "AccessToken"
						},
						"Default": {
							"path": "Default"
						},
						"GitRepositoryType": {
							"path": "GitRepositoryType"
						}
					}
				}
			}
		]/**SCHEMA_MODEL_CONFIG_DIFF*/,
		handlers: /**SCHEMA_HANDLERS*/[]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
define("AtfTIDE_ListPage", /**SCHEMA_DEPS*/["@creatio-devkit/common"]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/(sdk)/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "move",
				"name": "MainHeader",
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "merge",
				"name": "MainHeader",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 2,
						"rowSpan": 1
					}
				}
			},
			{
				"operation": "merge",
				"name": "AddButton",
				"values": {
					"size": "large"
				}
			},
			{
				"operation": "merge",
				"name": "MenuItem_ImportFromExcel",
				"values": {
					"clicked": {
						"request": "crt.ImportDataRequest",
						"params": {
							"entitySchemaName": "AtfRepository"
						}
					}
				}
			},
			{
				"operation": "move",
				"name": "MainFilterContainer",
				"parentName": "SectionContentWrapper",
				"propertyName": "items",
				"index": 0
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
				"operation": "merge",
				"name": "LeftFilterContainer",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 2,
						"rowSpan": 1
					}
				}
			},
			{
				"operation": "remove",
				"name": "FolderTreeActions"
			},
			{
				"operation": "merge",
				"name": "LookupQuickFilterByTag",
				"values": {
					"config": {
						"caption": "#ResourceString(LookupQuickFilterByTag_config_caption)#",
						"hint": "#ResourceString(LookupQuickFilterByTag_config_hint)#",
						"icon": "tag-icon",
						"iconPosition": "left-icon",
						"entitySchemaName": "Tag_Virtual_Schema",
						"defaultValue": null,
						"recordsFilter": null
					}
				}
			},
			{
				"operation": "merge",
				"name": "RightFilterContainer",
				"values": {
					"layoutConfig": {
						"column": 3,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 1
					}
				}
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
							"id": "f252f581-0ccf-44ac-b7c9-c00df2ad9919",
							"code": "PDS_AtfName",
							"caption": "#ResourceString(PDS_AtfName)#",
							"dataValueType": 1,
							"width": 169
						},
						{
							"id": "69eff662-f9fe-3cda-187d-e00abf947474",
							"code": "PDS_AtfApplication",
							"caption": "#ResourceString(PDS_AtfApplication)#",
							"dataValueType": 10,
							"width": 155
						},
						{
							"id": "df2022d2-6ccc-fb59-0730-a434a04b3e56",
							"code": "PDS_AtfAccessToken",
							"caption": "#ResourceString(PDS_AtfAccessToken)#",
							"dataValueType": 28,
							"width": 244
						},
						{
							"id": "a534afb6-c37c-9c55-6510-14f5db3235f3",
							"code": "PDS_AtfUserName",
							"caption": "#ResourceString(PDS_AtfUserName)#",
							"dataValueType": 28,
							"width": 203
						},
						{
							"id": "7cf7b26f-3e55-6834-8d19-d29dcdb0fc18",
							"code": "PDS_AtfAutoSync",
							"caption": "#ResourceString(PDS_AtfAutoSync)#",
							"dataValueType": 12,
							"width": 122
						},
						{
							"id": "b3e9456a-3654-a0bf-4385-d6411875a17c",
							"code": "PDS_AtfRepositoryUrl",
							"caption": "#ResourceString(PDS_AtfRepositoryUrl)#",
							"dataValueType": 44,
							"width": 575
						}
					],
					"visible": true,
					"fitContent": true
				}
			},
			{
				"operation": "remove",
				"name": "DataTable",
				"properties": [
					"layoutConfig"
				]
			},
			{
				"operation": "insert",
				"name": "Button_SearchGitlab",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_SearchGitlab_caption)#",
					"color": "default",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "crt.OpenSelectionWindowRequest",
						"params": {
							"schemaName": "AtfMiniPage_SeatrchGitlab",
							"entitySchemaName": "AtfVirtual_GitLabProject"
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
				"name": "Button_Install_Updates",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_Install_Updates_caption)#",
					"color": "warn",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "atf.InstallAllAppsClicked"
					},
					"clickMode": "default"
				},
				"parentName": "ActionButtonsContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Button_ynk6wyx",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_ynk6wyx_caption)#",
					"color": "default",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "crt.OpenPageRequest",
						"params": {
							"schemaName": "AtfGitServerList"
						}
					},
					"clickMode": "default"
				},
				"parentName": "ActionButtonsContainer",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "MenuItem_UpdateTide",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(MenuItem_UpdateTide_caption)#",
					"visible": true,
					"clicked": {
						"request": "atf.InstallTideClicked"
					},
					"icon": "reload-icon"
				},
				"parentName": "ActionButton",
				"propertyName": "menuItems",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "MenuItem_UpdateClio",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(MenuItem_UpdateClio_caption)#",
					"visible": true,
					"clicked": {
						"request": "crt.RunBusinessProcessRequest",
						"params": {
							"processName": "AtfProcess_TryInstallClio",
							"processRunType": "RegardlessOfThePage",
							"saveAtProcessStart": true,
							"showNotification": true
						}
					},
					"icon": "checkmark-icon"
				},
				"parentName": "ActionButton",
				"propertyName": "menuItems",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "MenuItem_InstallConsoleGit",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(MenuItem_InstallConsoleGit_caption)#",
					"visible": true,
					"clicked": {
						"request": "atf.InstallConsoleGit"
					},
					"icon": "warning-filled-icon"
				},
				"parentName": "ActionButton",
				"propertyName": "menuItems",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "MenuItem_SearchGitlab",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(MenuItem_SearchGitlab_caption)#",
					"visible": true,
					"icon": "view-eye-icon",
					"clicked": {
						"request": "atf.SearchGitlab"
					},
				},
				"parentName": "ActionButton",
				"propertyName": "menuItems",
				"index": 4
			},
			{
				"operation": "insert",
				"name": "GridContainer_yj15bgg",
				"values": {
					"type": "crt.GridContainer",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"rows": "minmax(max-content, 32px)",
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"items": [],
					"fitContent": true,
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"alignItems": "stretch",
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 3,
						"rowSpan": 1
					}
				},
				"parentName": "MainFilterContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Gallery_qmmqbs4",
				"values": {
					"type": "crt.Gallery",
					"selectionState": "$Gallery_qmmqbs4_SelectionState",
					"_selectionOptions": {
						"attribute": "Gallery_qmmqbs4_SelectionState"
					},
					"itemConfig": {
						"defaultSize": "small",
						"templateValuesMapping": {
							"caption": "Gallery_qmmqbs4DS_AtfName",
							"description": null,
							"image": null,
							"id": "Gallery_qmmqbs4DS_Id"
						},
						"specificPageRecordId": "Gallery_qmmqbs4DS_Id"
					},
					"visible": true,
					"specificPage": null,
					"items": "$Gallery_qmmqbs4",
					"useSpecificPage": null,
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					}
				},
				"parentName": "GridContainer_yj15bgg",
				"propertyName": "items",
				"index": 0
			}
		]/**SCHEMA_VIEW_CONFIG_DIFF*/,
		viewModelConfigDiff: /**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [
					"attributes"
				],
				"values": {
					"Gallery_qmmqbs4": {
						"isCollection": true,
						"modelConfig": {
							"path": "Gallery_qmmqbs4DS",
							"filterAttributes": [
								{
									"loadOnChange": true,
									"name": "Gallery_qmmqbs4_PredefinedFilter"
								}
							]
						},
						"viewModelConfig": {
							"attributes": {
								"Gallery_qmmqbs4DS_Id": {
									"modelConfig": {
										"path": "Gallery_qmmqbs4DS.Id"
									}
								},
								"Gallery_qmmqbs4DS_AtfName": {
									"modelConfig": {
										"path": "Gallery_qmmqbs4DS.AtfName"
									}
								}
							}
						}
					},
					"Gallery_qmmqbs4_PredefinedFilter": {
						"value": {
							"items": {
								"564166a0-9617-4aae-9b77-4743410e163e": {
									"filterType": 2,
									"comparisonType": 2,
									"isEnabled": true,
									"trimDateTimeParameterToDate": false,
									"leftExpression": {
										"expressionType": 0,
										"columnPath": "AtfApplication"
									},
									"isAggregative": false,
									"dataValueType": 10,
									"referenceSchemaName": "SysInstalledApp",
									"isNull": false
								}
							},
							"logicalOperation": 0,
							"isEnabled": true,
							"filterType": 6,
							"rootSchemaName": "AtfRepository"
						}
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
					"PDS_AtfName": {
						"modelConfig": {
							"path": "PDS.AtfName"
						}
					},
					"PDS_AtfApplication": {
						"modelConfig": {
							"path": "PDS.AtfApplication"
						}
					},
					"PDS_AtfAccessToken": {
						"modelConfig": {
							"path": "PDS.AtfAccessToken"
						}
					},
					"PDS_AtfUserName": {
						"modelConfig": {
							"path": "PDS.AtfUserName"
						}
					},
					"PDS_AtfAutoSync": {
						"modelConfig": {
							"path": "PDS.AtfAutoSync"
						}
					},
					"PDS_AtfRepositoryUrl": {
						"modelConfig": {
							"path": "PDS.AtfRepositoryUrl"
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
							"name": "Items_PredefinedFilter",
							"loadOnChange": true
						},
						{
							"name": "LookupQuickFilterByTag_Items",
							"loadOnChange": true
						},
						{
							"name": "SearchFilter_Items",
							"loadOnChange": true
						}
					]
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"Items",
					"modelConfig",
					"sortingConfig"
				],
				"values": {
					"default": [
						{
							"direction": "desc",
							"columnName": "AtfRepositoryUrl"
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
					"dataSources"
				],
				"values": {
					"Gallery_qmmqbs4DS": {
						"type": "crt.EntityDataSource",
						"scope": "viewElement",
						"config": {
							"entitySchemaName": "AtfRepository",
							"attributes": {}
						}
					}
				}
			},
			{
				"operation": "merge",
				"path": [
					"dataSources",
					"PDS",
					"config"
				],
				"values": {
					"entitySchemaName": "AtfRepository",
					"attributes": {
						"AtfName": {
							"path": "AtfName"
						},
						"AtfApplication": {
							"path": "AtfApplication"
						},
						"AtfAccessToken": {
							"path": "AtfAccessToken"
						},
						"AtfUserName": {
							"path": "AtfUserName"
						},
						"AtfAutoSync": {
							"path": "AtfAutoSync"
						},
						"AtfRepositoryUrl": {
							"path": "AtfRepositoryUrl"
						}
					}
				}
			}
		]/**SCHEMA_MODEL_CONFIG_DIFF*/,
		handlers: /**SCHEMA_HANDLERS*/[
			{
				request: "crt.HandleViewModelInitRequest",
				handler: async (request, next) => {
					await next?.handle(request);
					const sysSettingsService = new sdk.SysSettingsService();
					const settingTideUpdateValue = await sysSettingsService.getByCode('AtfTideUpdateAvailable');
					const settingClioUpdateValue = await sysSettingsService.getByCode('AtfClioUpdateAvailable');
					request.$context.IsQuestionContainerVisible = settingTideUpdateValue.value || settingClioUpdateValue.value;
					request.$context.IsClioUpdateButtonVisibile = settingClioUpdateValue.value;
					request.$context.IsTideUpdateButtonVisibile = settingTideUpdateValue.value;
					
					
					
					const endpoint = "/rest/Tide/CaptureClioArgs";
					const httpClientService = new sdk.HttpClientService();
					await httpClientService.get(endpoint)
					
					await next?.handle(request);
				}
			},
			{
				request: "atf.InstallClioClicked",
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
						processName: "AtfProcess_TryInstallClio",
						processRunType: "RegardlessOfThePage",
						saveAtProcessStart: true,
						showNotification: true
					});
					
					await next?.handle(request);
				}
			},
			
			{
				request: "atf.InstallTideClicked",
				handler: async (request, next) => {
					await next?.handle(request);
					
					const handlerChain = sdk.HandlerChainService.instance;
					await handlerChain.process({
						type: "crt.RunBusinessProcessRequest",
						$context: request.$context,
						scopes: [...request.scopes],
						processName: "AtfProcess_InstallTide",
						processRunType: "RegardlessOfThePage",
						saveAtProcessStart: true,
						showNotification: true
					});
					
					await handlerChain.process({
						type: 'crt.OpenPageRequest',
						$context: request.$context,
						scopes: [...request.scopes],
						schemaName: "Page_LogTerminal"
					});
					
					await next?.handle(request);
				}
			},
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
			
			{
				request: "atf.InstallConsoleGit",
				handler: async (request, next) => {
					const endpoint = "rest/Tide/InstallConsoleGit";
					const httpClientService = new sdk.HttpClientService();
					const response = await httpClientService.get(endpoint);
				}
			},
			{
				request: "atf.SearchGitlab",
				handler: async (request, next) => {
					const handlerChain = sdk.HandlerChainService.instance;
					await handlerChain.process({
						type: "crt.OpenSelectionWindowRequest",
						$context: request.$context,
						scopes: [...request.scopes],
						schemaName: 'AtfMiniPage_SearchGitlab',
						entitySchemaName: "AtfVirtual_GitLabProject",
						showNotification: true,
						afterClosed: async (result) => {
							if (!result.canceled) {
								const lookupValues = await result.getLookupValues();
								console.log(lookupValues);
								const value = lookupValues[0];
								if (value) {
									alert(value?.displayValue ?? '');
								}
							}
						},
					});
					return next?.handle(request);
				}
			}
			
			
			
		]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
define("AtfMiniPage_GitlabSearch", /**SCHEMA_DEPS*/[]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/()/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "MainContainer",
				"values": {
					"alignItems": "stretch"
				}
			},
			{
				"operation": "insert",
				"name": "FlexContainer_yy57mng",
				"values": {
					"layoutConfig": {
						"column": 1,
						"colSpan": 1,
						"row": 1,
						"rowSpan": 1
					},
					"type": "crt.FlexContainer",
					"direction": "row",
					"items": [],
					"fitContent": true
				},
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "SearchFilter_Element",
				"values": {
					"type": "crt.SearchFilter",
					"placeholder": "#ResourceString(SearchFilter_Element_placeholder)#",
					"_filterOptions": {
						"expose": [
							{
								"attribute": "SearchFilter_Element_DataGrid_pofhyrz",
								"converters": [
									{
										"converter": "crt.SearchFilterAttributeConverter",
										"args": [
											"DataGrid_pofhyrz"
										]
									}
								]
							}
						],
						"from": [
							"SearchFilter_Element_SearchValue",
							"SearchFilter_Element_FilteredColumnsGroups"
						]
					}
				},
				"parentName": "FlexContainer_yy57mng",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "DataGrid_Element",
				"values": {
					"layoutConfig": {
						"column": 1,
						"colSpan": 1,
						"row": 2,
						"rowSpan": 1
					},
					"type": "crt.DataGrid",
					"features": {
						"rows": {
							"selection": {
								"enable": true,
								"multiple": true
							},
							"numeration": false
						},
						"editable": {
							"enable": false,
							"itemsCreation": false,
							"floatingEditPanel": false
						}
					},
					"items": "$DataGrid_pofhyrz",
					"activeRow": "$DataGrid_pofhyrz_ActiveRow",
					"selectionState": "$DataGrid_pofhyrz_SelectionState",
					"_selectionOptions": {
						"attribute": "DataGrid_pofhyrz_SelectionState"
					},
					"primaryColumnName": "DataGrid_pofhyrzDS_Id",
					"columns": [
						{
							"id": "f603fb97-b9b7-892f-a503-822c4068421e",
							"code": "DataGrid_pofhyrzDS_Name",
							"caption": "#ResourceString(DataGrid_pofhyrzDS_Name)#",
							"dataValueType": 28,
							"width": 266.99998474121094
						},
						{
							"id": "2d9bc0e4-0f2b-938f-bfed-1907cd8ea74a",
							"code": "DataGrid_pofhyrzDS_AtfGitServer",
							"caption": "#ResourceString(DataGrid_pofhyrzDS_AtfGitServer)#",
							"dataValueType": 10,
							"width": 295
						},
						{
							"id": "b2f097aa-bff1-903a-d57b-c0ea8809c27e",
							"code": "DataGrid_pofhyrzDS_NameSpace",
							"caption": "#ResourceString(DataGrid_pofhyrzDS_NameSpace)#",
							"dataValueType": 30,
							"width": 292
						},
						{
							"id": "42c7e958-c5b7-746f-9aa7-ced70415ddb1",
							"code": "DataGrid_pofhyrzDS_CloneUrl",
							"caption": "#ResourceString(DataGrid_pofhyrzDS_CloneUrl)#",
							"dataValueType": 29,
							"width": 624.9971313476562
						},
						{
							"id": "d82cafcd-77b1-3722-addd-eaa5d3a971b1",
							"code": "DataGrid_pofhyrzDS_Description",
							"caption": "#ResourceString(DataGrid_pofhyrzDS_Description)#",
							"dataValueType": 43,
							"width": 528.9999847412109
						}
					],
					"placeholder": false,
					"bulkActions": []
				},
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "DataGrid_pofhyrz_AddTagsBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Add tag",
					"icon": "tag-icon",
					"clicked": {
						"request": "crt.AddTagsInRecordsRequest",
						"params": {
							"dataSourceName": "DataGrid_pofhyrzDS",
							"filters": "$DataGrid_pofhyrz | crt.ToCollectionFilters : 'DataGrid_pofhyrz' : $DataGrid_pofhyrz_SelectionState | crt.SkipIfSelectionEmpty : $DataGrid_pofhyrz_SelectionState"
						}
					},
					"items": []
				},
				"parentName": "DataGrid_Element",
				"propertyName": "bulkActions",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "DataGrid_pofhyrz_RemoveTagsBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Remove tag",
					"icon": "delete-button-icon",
					"clicked": {
						"request": "crt.RemoveTagsInRecordsRequest",
						"params": {
							"dataSourceName": "DataGrid_pofhyrzDS",
							"filters": "$DataGrid_pofhyrz | crt.ToCollectionFilters : 'DataGrid_pofhyrz' : $DataGrid_pofhyrz_SelectionState | crt.SkipIfSelectionEmpty : $DataGrid_pofhyrz_SelectionState"
						}
					}
				},
				"parentName": "DataGrid_pofhyrz_AddTagsBulkAction",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "DataGrid_pofhyrz_ExportToExcelBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Export to Excel",
					"icon": "export-button-icon",
					"clicked": {
						"request": "crt.ExportDataGridToExcelRequest",
						"params": {
							"viewName": "DataGrid_Element",
							"filters": "$DataGrid_pofhyrz | crt.ToCollectionFilters : 'DataGrid_pofhyrz' : $DataGrid_pofhyrz_SelectionState | crt.SkipIfSelectionEmpty : $DataGrid_pofhyrz_SelectionState"
						}
					}
				},
				"parentName": "DataGrid_Element",
				"propertyName": "bulkActions",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "DataGrid_pofhyrz_DeleteBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Delete",
					"icon": "delete-button-icon",
					"clicked": {
						"request": "crt.DeleteRecordsRequest",
						"params": {
							"dataSourceName": "DataGrid_pofhyrzDS",
							"filters": "$DataGrid_pofhyrz | crt.ToCollectionFilters : 'DataGrid_pofhyrz' : $DataGrid_pofhyrz_SelectionState | crt.SkipIfSelectionEmpty : $DataGrid_pofhyrz_SelectionState"
						}
					}
				},
				"parentName": "DataGrid_Element",
				"propertyName": "bulkActions",
				"index": 2
			}
		]/**SCHEMA_VIEW_CONFIG_DIFF*/,
		viewModelConfigDiff: /**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [
					"attributes"
				],
				"values": {
					"DataGrid_pofhyrz": {
						"isCollection": true,
						"modelConfig": {
							"path": "DataGrid_pofhyrzDS",
							"filterAttributes": [
								{
									"name": "SearchFilter_Element_DataGrid_pofhyrz",
									"loadOnChange": true
								}
							],
							"sortingConfig": {
								"default": [
									{
										"direction": "desc",
										"columnName": "Name"
									}
								]
							}
						},
						"viewModelConfig": {
							"attributes": {
								"DataGrid_pofhyrzDS_Name": {
									"modelConfig": {
										"path": "DataGrid_pofhyrzDS.Name"
									}
								},
								"DataGrid_pofhyrzDS_AtfGitServer": {
									"modelConfig": {
										"path": "DataGrid_pofhyrzDS.AtfGitServer"
									}
								},
								"DataGrid_pofhyrzDS_NameSpace": {
									"modelConfig": {
										"path": "DataGrid_pofhyrzDS.NameSpace"
									}
								},
								"DataGrid_pofhyrzDS_CloneUrl": {
									"modelConfig": {
										"path": "DataGrid_pofhyrzDS.CloneUrl"
									}
								},
								"DataGrid_pofhyrzDS_Description": {
									"modelConfig": {
										"path": "DataGrid_pofhyrzDS.Description"
									}
								},
								"DataGrid_pofhyrzDS_Id": {
									"modelConfig": {
										"path": "DataGrid_pofhyrzDS.Id"
									}
								}
							}
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
					"dataSources": {
						"DataGrid_pofhyrzDS": {
							"type": "crt.EntityDataSource",
							"scope": "viewElement",
							"config": {
								"entitySchemaName": "AtfVirtual_GitLabProject",
								"attributes": {
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
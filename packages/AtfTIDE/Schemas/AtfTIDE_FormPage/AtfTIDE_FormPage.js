define("AtfTIDE_FormPage", /**SCHEMA_DEPS*/["@creatio-devkit/common"]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/(sdk)/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "SaveButton",
				"values": {
					"size": "large",
					"iconPosition": "only-text"
				}
			},
			{
				"operation": "merge",
				"name": "CardToggleContainer",
				"values": {
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
					"direction": "row",
					"gap": "small",
					"wrap": "nowrap"
				}
			},
			{
				"operation": "merge",
				"name": "CardContentWrapper",
				"values": {
					"padding": {
						"left": "small",
						"right": "small",
						"top": "none",
						"bottom": "none"
					},
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"alignItems": "stretch"
				}
			},
			{
				"operation": "merge",
				"name": "SideContainer",
				"values": {
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
					"justifyContent": "start",
					"gap": "small",
					"wrap": "nowrap"
				}
			},
			{
				"operation": "merge",
				"name": "SideAreaProfileContainer",
				"values": {
					"columns": [
						"minmax(64px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"visible": true,
					"alignItems": "stretch"
				}
			},
			{
				"operation": "merge",
				"name": "Tabs",
				"values": {
					"styleType": "default",
					"mode": "tab",
					"bodyBackgroundColor": "primary-contrast-500",
					"selectedTabTitleColor": "auto",
					"tabTitleColor": "auto",
					"underlineSelectedTabColor": "auto",
					"headerBackgroundColor": "auto"
				}
			},
			{
				"operation": "merge",
				"name": "GeneralInfoTab",
				"values": {
					"iconPosition": "only-text"
				}
			},
			{
				"operation": "merge",
				"name": "CardToggleTabPanel",
				"values": {
					"styleType": "default",
					"bodyBackgroundColor": "primary-contrast-500",
					"selectedTabTitleColor": "auto",
					"tabTitleColor": "auto",
					"underlineSelectedTabColor": "auto",
					"headerBackgroundColor": "auto"
				}
			},
			{
				"operation": "insert",
				"name": "GridContainer_4s9rh1f",
				"values": {
					"type": "crt.GridContainer",
					"columns": [
						"minmax(32px, 1fr)",
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
					"alignItems": "stretch"
				},
				"parentName": "MainHeaderBottom",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Button_LOADFROMGIT",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_LOADFROMGIT_caption)#",
					"color": "primary",
					"disabled": false,
					"size": "large",
					"iconPosition": "left-icon",
					"visible": true,
					"icon": "export-data-button-icon",
					"clicked": {
						"request": "atf.CaptureClioArgs",
						"params": {
							"processName": "AtfProcess_LoadWorkspaceFromGit",
							"processRunType": "ForTheSelectedPage",
							"showNotification": true,
							"recordIdProcessParameterName": "Repository"
						}
					},
					"clickMode": "default"
				},
				"parentName": "CardToggleContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_SaveToGit_Green",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_SaveToGit_Green_caption)#",
					"color": "accent",
					"disabled": false,
					"size": "large",
					"iconPosition": "left-icon",
					"visible": true,
					"icon": "import-data-button-icon",
					"clicked": {
						"request": "atf.CaptureClioArgs",
						"params": {
							"processName": "AtfProcess_SaveWorkspaceToGit",
							"processRunType": "ForTheSelectedPage",
							"showNotification": true,
							"recordIdProcessParameterName": "Repository"
						}
					},
					"clickMode": "default"
				},
				"parentName": "CardToggleContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "AtfName",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.Input",
					"label": "$Resources.Strings.AtfName",
					"control": "$AtfName",
					"labelPosition": "auto",
					"visible": true,
					"readonly": false,
					"placeholder": "",
					"tooltip": "#ResourceString(AtfName_tooltip)#"
				},
				"parentName": "SideAreaProfileContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "ComboBox_Application",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.ComboBox",
					"label": "$Resources.Strings.PDS_AtfApplication_8tomm9h",
					"labelPosition": "auto",
					"control": "$PDS_AtfApplication_8tomm9h",
					"listActions": [],
					"showValueAsLink": true,
					"controlActions": [],
					"visible": false,
					"readonly": false,
					"placeholder": "",
					"tooltip": "#ResourceString(ComboBox_Application_tooltip)#",
					"valueDetails": null
				},
				"parentName": "SideAreaProfileContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Button_LinkWithApplication",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 3,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.Button",
					"caption": "#ResourceString(Button_LinkWithApplication_caption)#",
					"color": "primary",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "crt.RunBusinessProcessRequest",
						"params": {
							"processName": "AtfProcess_LinkAppWithRepository",
							"processRunType": "ForTheSelectedPage",
							"showNotification": true,
							"recordIdProcessParameterName": "Repository"
						}
					},
					"clickMode": "default"
				},
				"parentName": "SideAreaProfileContainer",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "GridContainer_5wijoep",
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
					"padding": {
						"top": "medium",
						"right": "large",
						"bottom": "medium",
						"left": "large"
					},
					"color": "primary",
					"borderRadius": "medium",
					"visible": true,
					"alignItems": "stretch"
				},
				"parentName": "SideContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "ExpansionPanel_State",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.ExpansionPanel",
					"tools": [],
					"items": [],
					"title": "#ResourceString(ExpansionPanel_State_title)#",
					"toggleType": "default",
					"togglePosition": "before",
					"expanded": true,
					"labelColor": "auto",
					"fullWidthHeader": false,
					"titleWidth": 20,
					"padding": {
						"top": "small",
						"bottom": "small",
						"left": "none",
						"right": "none"
					},
					"fitContent": true,
					"visible": true,
					"alignItems": "stretch"
				},
				"parentName": "GridContainer_5wijoep",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridContainer_tq00a58",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 24px)",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_State",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_fk8z4ob",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"gap": "none",
					"alignItems": "center",
					"items": [],
					"layoutConfig": {
						"colSpan": 1,
						"column": 1,
						"row": 1,
						"rowSpan": 1
					}
				},
				"parentName": "GridContainer_tq00a58",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridContainer_hu8i5l3",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 32px)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": [],
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"alignItems": "stretch"
				},
				"parentName": "ExpansionPanel_State",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Checkbox_AtfLocked",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.Checkbox",
					"label": "$Resources.Strings.PDS_exeColumn10_b6z2b0g",
					"labelPosition": "right",
					"control": "$PDS_exeColumn10_b6z2b0g",
					"visible": true,
					"readonly": false,
					"placeholder": "",
					"tooltip": ""
				},
				"parentName": "GridContainer_hu8i5l3",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Checkbox_AtfUpdateAvailable",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.Checkbox",
					"label": "$Resources.Strings.PDS_AtfUpdateAvailable_thzvlv4",
					"labelPosition": "right",
					"control": "$PDS_AtfUpdateAvailable_thzvlv4",
					"visible": true,
					"readonly": false,
					"placeholder": "",
					"tooltip": ""
				},
				"parentName": "GridContainer_hu8i5l3",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Checkbox_AtfAutoSync",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 3,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.Checkbox",
					"label": "$Resources.Strings.PDS_AtfAutoSync_zrzeu6i",
					"labelPosition": "right",
					"control": "$PDS_AtfAutoSync_zrzeu6i"
				},
				"parentName": "GridContainer_hu8i5l3",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "ExpansionPanel_w1nj9pl",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.ExpansionPanel",
					"tools": [],
					"items": [],
					"title": "#ResourceString(ExpansionPanel_w1nj9pl_title)#",
					"toggleType": "default",
					"togglePosition": "before",
					"expanded": false,
					"labelColor": "auto",
					"fullWidthHeader": false,
					"titleWidth": 20,
					"padding": {
						"top": "small",
						"bottom": "small",
						"left": "none",
						"right": "none"
					},
					"fitContent": true,
					"visible": false,
					"alignItems": "stretch"
				},
				"parentName": "GridContainer_5wijoep",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridContainer_ejvgf2o",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 24px)",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": [],
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"alignItems": "stretch"
				},
				"parentName": "ExpansionPanel_w1nj9pl",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_qp4lnra",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"gap": "none",
					"alignItems": "center",
					"items": [],
					"layoutConfig": {
						"colSpan": 1,
						"column": 1,
						"row": 1,
						"rowSpan": 1
					}
				},
				"parentName": "GridContainer_ejvgf2o",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridContainer_lv08x1q",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 32px)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": [],
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"alignItems": "stretch"
				},
				"parentName": "ExpansionPanel_w1nj9pl",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_Login",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.Input",
					"label": "$Resources.Strings.PDS_AtfLogin_g255988",
					"labelPosition": "auto",
					"control": "$PDS_AtfLogin_g255988",
					"multiline": false,
					"visible": false,
					"readonly": false,
					"placeholder": "",
					"tooltip": ""
				},
				"parentName": "GridContainer_lv08x1q",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_Password",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.PasswordInput",
					"label": "$Resources.Strings.PDS_AtfPassword_rrf2i89",
					"labelPosition": "auto",
					"control": "$PDS_AtfPassword_rrf2i89",
					"multiline": false,
					"visible": false,
					"readonly": false,
					"placeholder": "",
					"tooltip": ""
				},
				"parentName": "GridContainer_lv08x1q",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "WebInput_CreatioUrl",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 3,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.WebInput",
					"label": "$Resources.Strings.PDS_AtfCreatioUrl_hgv046w",
					"labelPosition": "auto",
					"control": "$PDS_AtfCreatioUrl_hgv046w",
					"visible": false,
					"readonly": false,
					"placeholder": "",
					"tooltip": ""
				},
				"parentName": "GridContainer_lv08x1q",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "ExpansionPanel_qif7wzo",
				"values": {
					"type": "crt.ExpansionPanel",
					"tools": [],
					"items": [],
					"title": "#ResourceString(ExpansionPanel_qif7wzo_title)#",
					"toggleType": "default",
					"togglePosition": "before",
					"expanded": true,
					"labelColor": "auto",
					"fullWidthHeader": false,
					"titleWidth": 20,
					"padding": {
						"top": "small",
						"bottom": "small",
						"left": "none",
						"right": "none"
					},
					"fitContent": true
				},
				"parentName": "ExpansionPanel_w1nj9pl",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridContainer_ac6ppq5",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 24px)",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_qif7wzo",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_rmlrtmi",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"gap": "none",
					"alignItems": "center",
					"items": [],
					"layoutConfig": {
						"colSpan": 1,
						"column": 1,
						"row": 1,
						"rowSpan": 1
					}
				},
				"parentName": "GridContainer_ac6ppq5",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridContainer_pioaw7c",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 32px)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_qif7wzo",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "WebInput_RepositoryUrl",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.WebInput",
					"label": "$Resources.Strings.PDS_AtfRepositoryUrl_yidr394",
					"labelPosition": "above",
					"control": "$PDS_AtfRepositoryUrl_yidr394",
					"tooltip": ""
				},
				"parentName": "GeneralInfoTabContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_ActiveBranch",
				"values": {
					"layoutConfig": {
						"column": 2,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.Input",
					"label": "$Resources.Strings.PDS_AtfActiveBranch_zvr5u2c",
					"labelPosition": "above",
					"control": "$PDS_AtfActiveBranch_zvr5u2c",
					"visible": true,
					"readonly": true,
					"placeholder": "",
					"tooltip": "",
					"multiline": false
				},
				"parentName": "GeneralInfoTabContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Input_irahwlv",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.Input",
					"label": "$Resources.Strings.PDS_AtfUserName_zwx7r4g",
					"labelPosition": "above",
					"control": "$PDS_AtfUserName_zwx7r4g",
					"multiline": false,
					"visible": true,
					"readonly": false,
					"placeholder": "",
					"tooltip": ""
				},
				"parentName": "GeneralInfoTabContainer",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "Input_AtfAccessToken",
				"values": {
					"layoutConfig": {
						"column": 2,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.PasswordInput",
					"label": "$Resources.Strings.PDS_AtfAccessToken_nzu843o",
					"labelPosition": "above",
					"control": "$PDS_AtfAccessToken_nzu843o",
					"multiline": false
				},
				"parentName": "GeneralInfoTabContainer",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "FlexContainer_yh1guh4",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"items": [],
					"fitContent": true,
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "large",
						"right": "none",
						"bottom": "large",
						"left": "none"
					},
					"alignItems": "stretch",
					"justifyContent": "start",
					"gap": "small",
					"wrap": "wrap"
				},
				"parentName": "GeneralInfoTab",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Button_SetActiveBranch",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_SetActiveBranch_caption)#",
					"color": "warn",
					"disabled": false,
					"size": "large",
					"iconPosition": "left-icon",
					"visible": true,
					"clicked": {
						"request": "crt.RunBusinessProcessRequest",
						"params": {
							"processName": "AtfProcess_SetActiveBranch",
							"processRunType": "ForTheSelectedRecords",
							"showNotification": true,
							"dataSourceName": "GridDetail_t9wy0f2DS",
							"parameterMappings": {
								"Branch": "Id"
							},
							"filters": "$GridDetail_t9wy0f2 | crt.ToCollectionFilters : 'GridDetail_t9wy0f2' : $GridDetail_t9wy0f2_SelectionState | crt.SkipIfSelectionEmpty : $GridDetail_t9wy0f2_SelectionState",
							"sorting": "$GridDetail_t9wy0f2Sorting",
							"selectionStateAttributeName": "GridDetail_t9wy0f2_SelectionState"
						}
					},
					"clickMode": "default",
					"icon": "checkmark-icon"
				},
				"parentName": "FlexContainer_yh1guh4",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_SyncBranchesWithRepo",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_SyncBranchesWithRepo_caption)#",
					"color": "accent",
					"disabled": false,
					"size": "large",
					"iconPosition": "left-icon",
					"visible": true,
					"clicked": {
						"request": "crt.RunBusinessProcessRequest",
						"params": {
							"processName": "ATFSyncronizeBranchesWithRepo",
							"processRunType": "ForTheSelectedPage",
							"showNotification": true,
							"recordIdProcessParameterName": "RepositoryId"
						}
					},
					"clickMode": "default",
					"icon": "reload-icon"
				},
				"parentName": "FlexContainer_yh1guh4",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridDetail_t9wy0f2DS",
				"values": {
					"type": "crt.DataGrid",
					"features": {
						"rows": {
							"selection": false,
							"numeration": false
						},
						"editable": {
							"enable": false,
							"itemsCreation": false,
							"floatingEditPanel": false
						}
					},
					"items": "$GridDetail_t9wy0f2",
					"visible": true,
					"fitContent": true,
					"primaryColumnName": "GridDetail_t9wy0f2DS_Id",
					"columns": [
						{
							"id": "73135d14-5d9c-c1b8-26da-c0ea4cdf7454",
							"code": "GridDetail_t9wy0f2DS_AtfName",
							"caption": "#ResourceString(GridDetail_t9wy0f2DS_AtfName)#",
							"dataValueType": 28
						}
					],
					"placeholder": false,
					"activeRow": "$GridDetail_t9wy0f2_ActiveRow",
					"selectionState": "$GridDetail_t9wy0f2_SelectionState",
					"_selectionOptions": {
						"attribute": "GridDetail_t9wy0f2_SelectionState"
					},
					"bulkActions": []
				},
				"parentName": "GeneralInfoTab",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "GridDetail_t9wy0f2_AddTagsBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Add tag",
					"icon": "tag-icon",
					"clicked": {
						"request": "crt.AddTagsInRecordsRequest",
						"params": {
							"dataSourceName": "GridDetail_t9wy0f2DS",
							"filters": "$GridDetail_t9wy0f2 | crt.ToCollectionFilters : 'GridDetail_t9wy0f2' : $GridDetail_t9wy0f2_SelectionState | crt.SkipIfSelectionEmpty : $GridDetail_t9wy0f2_SelectionState"
						}
					},
					"items": []
				},
				"parentName": "GridDetail_t9wy0f2DS",
				"propertyName": "bulkActions",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetail_t9wy0f2_RemoveTagsBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Remove tag",
					"icon": "delete-button-icon",
					"clicked": {
						"request": "crt.RemoveTagsInRecordsRequest",
						"params": {
							"dataSourceName": "GridDetail_t9wy0f2DS",
							"filters": "$GridDetail_t9wy0f2 | crt.ToCollectionFilters : 'GridDetail_t9wy0f2' : $GridDetail_t9wy0f2_SelectionState | crt.SkipIfSelectionEmpty : $GridDetail_t9wy0f2_SelectionState"
						}
					}
				},
				"parentName": "GridDetail_t9wy0f2_AddTagsBulkAction",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetail_t9wy0f2_ExportToExcelBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Export to Excel",
					"icon": "export-button-icon",
					"clicked": {
						"request": "crt.ExportDataGridToExcelRequest",
						"params": {
							"viewName": "GridDetail_t9wy0f2DS",
							"filters": "$GridDetail_t9wy0f2 | crt.ToCollectionFilters : 'GridDetail_t9wy0f2' : $GridDetail_t9wy0f2_SelectionState | crt.SkipIfSelectionEmpty : $GridDetail_t9wy0f2_SelectionState"
						}
					}
				},
				"parentName": "GridDetail_t9wy0f2DS",
				"propertyName": "bulkActions",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridDetail_t9wy0f2_DeleteBulkAction",
				"values": {
					"type": "crt.MenuItem",
					"caption": "Delete",
					"icon": "delete-button-icon",
					"clicked": {
						"request": "crt.DeleteRecordsRequest",
						"params": {
							"dataSourceName": "GridDetail_t9wy0f2DS",
							"filters": "$GridDetail_t9wy0f2 | crt.ToCollectionFilters : 'GridDetail_t9wy0f2' : $GridDetail_t9wy0f2_SelectionState | crt.SkipIfSelectionEmpty : $GridDetail_t9wy0f2_SelectionState"
						}
					}
				},
				"parentName": "GridDetail_t9wy0f2DS",
				"propertyName": "bulkActions",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "ExpansionPanel_srhpdgo",
				"values": {
					"type": "crt.ExpansionPanel",
					"tools": [],
					"items": [],
					"title": "#ResourceString(ExpansionPanel_srhpdgo_title)#",
					"toggleType": "default",
					"togglePosition": "before",
					"expanded": true,
					"labelColor": "auto",
					"fullWidthHeader": false,
					"titleWidth": 20,
					"padding": {
						"top": "small",
						"bottom": "small",
						"left": "none",
						"right": "none"
					},
					"fitContent": true,
					"visible": false,
					"alignItems": "stretch"
				},
				"parentName": "GeneralInfoTab",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "GridContainer_eta67n6",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 24px)",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_srhpdgo",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_rlolpw6",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"gap": "none",
					"alignItems": "center",
					"items": [],
					"layoutConfig": {
						"colSpan": 1,
						"column": 1,
						"row": 1,
						"rowSpan": 1
					},
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"justifyContent": "start",
					"wrap": "wrap"
				},
				"parentName": "GridContainer_eta67n6",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetailAddBtn_3ky6mp6",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(GridDetailAddBtn_3ky6mp6_caption)#",
					"icon": "add-button-icon",
					"iconPosition": "only-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.CreateRecordRequest",
						"params": {
							"entityName": "AtfRepositoryBranch"
						}
					},
					"visible": false,
					"clickMode": "default"
				},
				"parentName": "FlexContainer_rlolpw6",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetailRefreshBtn_bpq27gy",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(GridDetailRefreshBtn_bpq27gy_caption)#",
					"icon": "reload-icon",
					"iconPosition": "only-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.LoadDataRequest",
						"params": {
							"config": {
								"loadType": "reload",
								"useLastLoadParameters": true
							},
							"dataSourceName": "GridDetail_t9wy0f2DS"
						}
					},
					"visible": false,
					"clickMode": "default"
				},
				"parentName": "FlexContainer_rlolpw6",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridDetailSettingsBtn_n0jlqjb",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(GridDetailSettingsBtn_n0jlqjb_caption)#",
					"icon": "actions-button-icon",
					"iconPosition": "only-icon",
					"color": "default",
					"size": "medium",
					"clickMode": "menu",
					"menuItems": [],
					"visible": false
				},
				"parentName": "FlexContainer_rlolpw6",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "GridDetailExportDataBtn_cqxwofy",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(GridDetailExportDataBtn_cqxwofy_caption)#",
					"icon": "export-button-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.ExportDataGridToExcelRequest",
						"params": {
							"viewName": "GridDetail_t9wy0f2DS"
						}
					}
				},
				"parentName": "GridDetailSettingsBtn_n0jlqjb",
				"propertyName": "menuItems",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetailImportDataBtn_nk414hp",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(GridDetailImportDataBtn_nk414hp_caption)#",
					"icon": "import-button-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.ImportDataRequest",
						"params": {
							"entitySchemaName": "AtfRepositoryBranch"
						}
					}
				},
				"parentName": "GridDetailSettingsBtn_n0jlqjb",
				"propertyName": "menuItems",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridDetailSearchFilter_m373eyy",
				"values": {
					"type": "crt.SearchFilter",
					"placeholder": "#ResourceString(GridDetailSearchFilter_m373eyy_placeholder)#",
					"iconOnly": true,
					"_filterOptions": {
						"expose": [
							{
								"attribute": "GridDetailSearchFilter_m373eyy_GridDetail_t9wy0f2",
								"converters": [
									{
										"converter": "crt.SearchFilterAttributeConverter",
										"args": [
											"GridDetail_t9wy0f2"
										]
									}
								]
							}
						],
						"from": [
							"GridDetailSearchFilter_m373eyy_SearchValue",
							"GridDetailSearchFilter_m373eyy_FilteredColumnsGroups"
						]
					}
				},
				"parentName": "FlexContainer_rlolpw6",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "GridContainer_7bizhk0",
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
						"left": "medium"
					},
					"alignItems": "stretch"
				},
				"parentName": "FlexContainer_rlolpw6",
				"propertyName": "items",
				"index": 4
			},
			{
				"operation": "insert",
				"name": "GridContainer_oxp83zc",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 32px)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_srhpdgo",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "TabContainer_Git",
				"values": {
					"type": "crt.TabContainer",
					"items": [],
					"caption": "#ResourceString(TabContainer_Git_caption)#",
					"iconPosition": "only-text",
					"visible": true
				},
				"parentName": "Tabs",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridContainer_fsokns6",
				"values": {
					"type": "crt.GridContainer",
					"items": [],
					"rows": "minmax(32px, max-content)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"alignItems": "stretch"
				},
				"parentName": "TabContainer_Git",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridContainer_aax6po0",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 2,
						"rowSpan": 1
					},
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
					"alignItems": "stretch"
				},
				"parentName": "GridContainer_fsokns6",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_mirw4pt",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.FlexContainer",
					"direction": "row",
					"items": [],
					"fitContent": true,
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "medium",
						"right": "none",
						"bottom": "medium",
						"left": "none"
					},
					"alignItems": "stretch",
					"justifyContent": "start",
					"gap": "small",
					"wrap": "wrap"
				},
				"parentName": "GridContainer_aax6po0",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_LoadChangesToLocalCopy",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_LoadChangesToLocalCopy_caption)#",
					"color": "primary",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "atf.OnLoadChangesToLocalCopyClick",
						"params": {
							"processName": "AtfProcess_ExportAppToLocalGitCopy",
							"processRunType": "ForTheSelectedPage",
							"saveAtProcessStart": true,
							"showNotification": true,
							"recordIdProcessParameterName": "Repository"
						}
					},
					"clickMode": "default",
					"icon": null
				},
				"parentName": "FlexContainer_mirw4pt",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_GetDiff",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_GetDiff_caption)#",
					"color": "accent",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"clicked": {
						"request": "atf.OnGetDiffCLicked"
					},
					"visible": true,
					"icon": null,
					"clickMode": "default"
				},
				"parentName": "FlexContainer_mirw4pt",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "Button_saveToGit_Red",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_saveToGit_Red_caption)#",
					"color": "warn",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "atf.CaptureClioArgs",
						"params": {
							"processName": "AtfProcess_SaveWorkspaceToGit",
							"processRunType": "ForTheSelectedPage",
							"showNotification": true,
							"recordIdProcessParameterName": "Repository"
						}
					},
					"clickMode": "default"
				},
				"parentName": "FlexContainer_mirw4pt",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "Button_InstallLocalCopy",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_InstallLocalCopy_caption)#",
					"color": "outline",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-text",
					"visible": true,
					"clicked": {
						"request": "atf.LoadWorkspaceFromLocalCopy",
						"params": {
							"processName": "AtfProcess_LoadWorkspaceFromLocalCopy",
							"processRunType": "ForTheSelectedPage",
							"saveAtProcessStart": true,
							"showNotification": true,
							"recordIdProcessParameterName": "Repository"
						}
					},
					"clickMode": "default"
				},
				"parentName": "FlexContainer_mirw4pt",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "ExpansionPanel_rgvl3td",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 2,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "crt.ExpansionPanel",
					"tools": [],
					"items": [],
					"title": "#ResourceString(ExpansionPanel_rgvl3td_title)#",
					"toggleType": "default",
					"togglePosition": "before",
					"expanded": true,
					"labelColor": "auto",
					"fullWidthHeader": false,
					"titleWidth": 20,
					"padding": {
						"top": "small",
						"bottom": "medium",
						"left": "none",
						"right": "none"
					},
					"fitContent": true,
					"visible": "$FileChangesVisible",
					"alignItems": "stretch"
				},
				"parentName": "GridContainer_aax6po0",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridContainer_yupjka2",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 24px)",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": "none"
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": [],
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"alignItems": "stretch"
				},
				"parentName": "ExpansionPanel_rgvl3td",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_rfh0f89",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"gap": "none",
					"alignItems": "center",
					"items": [],
					"layoutConfig": {
						"colSpan": 1,
						"column": 1,
						"row": 1,
						"rowSpan": 1
					},
					"visible": true,
					"color": "transparent",
					"borderRadius": "none",
					"padding": {
						"top": "none",
						"right": "none",
						"bottom": "none",
						"left": "none"
					},
					"justifyContent": "start",
					"wrap": "wrap"
				},
				"parentName": "GridContainer_yupjka2",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetailAddBtn_fzbi431",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(GridDetailAddBtn_fzbi431_caption)#",
					"icon": "add-button-icon",
					"iconPosition": "only-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.CreateRecordRequest",
						"params": {
							"entityName": "Contact"
						}
					},
					"visible": false,
					"clickMode": "default"
				},
				"parentName": "FlexContainer_rfh0f89",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetailRefreshBtn_qhby3xs",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(GridDetailRefreshBtn_qhby3xs_caption)#",
					"icon": "reload-icon",
					"iconPosition": "only-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.LoadDataRequest",
						"params": {
							"config": {
								"loadType": "reload",
								"useLastLoadParameters": true
							},
							"dataSourceName": "GridDetail_62r7nr2DS"
						}
					},
					"visible": true,
					"clickMode": "default"
				},
				"parentName": "FlexContainer_rfh0f89",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridDetailSettingsBtn_5ijlpwq",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(GridDetailSettingsBtn_5ijlpwq_caption)#",
					"icon": "actions-button-icon",
					"iconPosition": "only-icon",
					"color": "default",
					"size": "medium",
					"clickMode": "menu",
					"menuItems": [],
					"visible": false
				},
				"parentName": "FlexContainer_rfh0f89",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "GridDetailExportDataBtn_7e2jx3x",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(GridDetailExportDataBtn_7e2jx3x_caption)#",
					"icon": "export-button-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.ExportDataGridToExcelRequest",
						"params": {
							"viewName": "GridDetail_62r7nr2"
						}
					}
				},
				"parentName": "GridDetailSettingsBtn_5ijlpwq",
				"propertyName": "menuItems",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetailImportDataBtn_j7mr4n2",
				"values": {
					"type": "crt.MenuItem",
					"caption": "#ResourceString(GridDetailImportDataBtn_j7mr4n2_caption)#",
					"icon": "import-button-icon",
					"color": "default",
					"size": "medium",
					"clicked": {
						"request": "crt.ImportDataRequest",
						"params": {
							"entitySchemaName": "Contact"
						}
					}
				},
				"parentName": "GridDetailSettingsBtn_5ijlpwq",
				"propertyName": "menuItems",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "GridDetailSearchFilter_z1pzuec",
				"values": {
					"type": "crt.SearchFilter",
					"placeholder": "#ResourceString(GridDetailSearchFilter_z1pzuec_placeholder)#",
					"iconOnly": true,
					"_filterOptions": {
						"expose": [
							{
								"attribute": "GridDetailSearchFilter_z1pzuec_GridDetail_62r7nr2",
								"converters": [
									{
										"converter": "crt.SearchFilterAttributeConverter",
										"args": [
											"GridDetail_62r7nr2"
										]
									}
								]
							}
						],
						"from": [
							"GridDetailSearchFilter_z1pzuec_SearchValue",
							"GridDetailSearchFilter_z1pzuec_FilteredColumnsGroups"
						]
					}
				},
				"parentName": "FlexContainer_rfh0f89",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "Button_DiscardChanges",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_DiscardChanges_caption)#",
					"color": "default",
					"disabled": false,
					"size": "medium",
					"iconPosition": "left-icon",
					"visible": true,
					"clicked": {
						"request": "atf.DiscardChanges"
					},
					"clickMode": "default",
					"icon": "delete-button-icon"
				},
				"parentName": "FlexContainer_rfh0f89",
				"propertyName": "items",
				"index": 4
			},
			{
				"operation": "insert",
				"name": "GridContainer_1sfenh7",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 32px)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_rgvl3td",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridDetail_62r7nr2",
				"values": {
					"type": "crt.DataGrid",
					"layoutConfig": {
						"colSpan": 2,
						"column": 1,
						"row": 1,
						"rowSpan": 8
					},
					"features": {
						"rows": {
							"toolbar": false,
							"columns": {
								"adding": false,
								"toolbar": false
							},
							"selection": {
								"enable": true,
								"multiple": true,
								"toolbar": false
							},
							"numeration": true
						},
						"editable": {
							"enable": false,
							"floatingEditPanel": false,
							"itemsCreation": false
						}
					},
					"items": "$GridDetail_62r7nr2",
					"visible": true,
					"fitContent": true,
					"primaryColumnName": "GridDetail_62r7nr2DS_Id",
					"columns": [
						{
							"id": "635ea97c-2b28-d0c5-1ea6-939f43aeb706",
							"code": "GridDetail_62r7nr2DS_AtfFileStatus",
							"caption": "#ResourceString(GridDetail_62r7nr2DS_AtfFileStatus)#",
							"dataValueType": 27,
							"width": 183,
							"sticky": true
						},
						{
							"id": "6230a5f1-2787-6ef7-1302-15e470828141",
							"code": "GridDetail_62r7nr2DS_AtfFileName",
							"caption": "#ResourceString(GridDetail_62r7nr2DS_AtfFileName)#",
							"dataValueType": 30,
							"width": 481
						}
					],
					"placeholder": false
				},
				"parentName": "GridContainer_1sfenh7",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GitDiff_5jxv7mx",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 3,
						"colSpan": 1,
						"rowSpan": 1
					},
					"type": "atf.GitDiff",
					"diffContent": "$DiffContent"
				},
				"parentName": "GridContainer_aax6po0",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "TabContainer_Messages",
				"values": {
					"type": "crt.TabContainer",
					"items": [],
					"caption": "#ResourceString(TabContainer_Messages_caption)#",
					"iconPosition": "only-text",
					"visible": true
				},
				"parentName": "Tabs",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "GridContainer_3lr88rv",
				"values": {
					"type": "crt.GridContainer",
					"items": [],
					"rows": "minmax(32px, max-content)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					}
				},
				"parentName": "TabContainer_Messages",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_LogMessageTab",
				"values": {
					"type": "crt.RichTextEditor",
					"label": "#ResourceString(Input_LogMessageTab_label)#",
					"control": "$AllMessages",
					"placeholder": "",
					"tooltip": "",
					"readonly": true,
					"multiline": true,
					"labelPosition": "above",
					"visible": true
				},
				"parentName": "TabContainer_Messages",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "TabContainer_Advanced",
				"values": {
					"type": "crt.TabContainer",
					"items": [],
					"caption": "#ResourceString(TabContainer_Advanced_caption)#",
					"iconPosition": "only-text",
					"visible": true
				},
				"parentName": "Tabs",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "GridContainer_lp9rdsy",
				"values": {
					"type": "crt.GridContainer",
					"items": [],
					"rows": "minmax(32px, max-content)",
					"columns": [
						"minmax(32px, 1fr)",
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					}
				},
				"parentName": "TabContainer_Advanced",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "ExpansionPanel_ApplicationHash",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 2,
						"rowSpan": 1
					},
					"type": "crt.ExpansionPanel",
					"tools": [],
					"items": [],
					"title": "#ResourceString(ExpansionPanel_ApplicationHash_title)#",
					"toggleType": "default",
					"togglePosition": "before",
					"expanded": true,
					"labelColor": "auto",
					"fullWidthHeader": false,
					"titleWidth": 20,
					"padding": {
						"top": "small",
						"bottom": "small",
						"left": "none",
						"right": "none"
					},
					"fitContent": true,
					"visible": true,
					"alignItems": "stretch"
				},
				"parentName": "GridContainer_lp9rdsy",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "GridContainer_55xmbgc",
				"values": {
					"type": "crt.GridContainer",
					"rows": "minmax(max-content, 24px)",
					"columns": [
						"minmax(32px, 1fr)"
					],
					"gap": {
						"columnGap": "large",
						"rowGap": 0
					},
					"styles": {
						"overflow-x": "hidden"
					},
					"items": []
				},
				"parentName": "ExpansionPanel_ApplicationHash",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "FlexContainer_49hq911",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"gap": "none",
					"alignItems": "center",
					"items": [],
					"layoutConfig": {
						"colSpan": 1,
						"column": 1,
						"row": 1,
						"rowSpan": 1
					}
				},
				"parentName": "GridContainer_55xmbgc",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_AtfAvailableAppHash",
				"values": {
					"type": "crt.Input",
					"label": "$Resources.Strings.PDS_AtfAvailableAppHash_xu2vola",
					"labelPosition": "auto",
					"control": "$PDS_AtfAvailableAppHash_xu2vola",
					"multiline": false
				},
				"parentName": "ExpansionPanel_ApplicationHash",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_AtfInstalledAppHash",
				"values": {
					"type": "crt.Input",
					"label": "$Resources.Strings.PDS_AtfInstalledAppHash_zifcf8b",
					"labelPosition": "auto",
					"control": "$PDS_AtfInstalledAppHash_zifcf8b",
					"multiline": false
				},
				"parentName": "ExpansionPanel_ApplicationHash",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "MessagesTabContainer",
				"values": {
					"type": "crt.TabContainer",
					"tools": [],
					"items": [],
					"caption": "#ResourceString(MessagesTabContainer_caption)#",
					"iconPosition": "left-icon",
					"visible": true,
					"icon": "mail-icon"
				},
				"parentName": "CardToggleTabPanel",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "FlexContainer_482ntir",
				"values": {
					"type": "crt.FlexContainer",
					"direction": "row",
					"alignItems": "center",
					"items": []
				},
				"parentName": "MessagesTabContainer",
				"propertyName": "tools",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Label_86uqz8y",
				"values": {
					"type": "crt.Label",
					"caption": "#MacrosTemplateString(#ResourceString(Label_86uqz8y_caption)#)#",
					"labelType": "headline-3",
					"labelThickness": "default",
					"labelEllipsis": false,
					"labelColor": "#0D2E4E",
					"labelBackgroundColor": "transparent",
					"labelTextAlign": "start",
					"visible": true
				},
				"parentName": "FlexContainer_482ntir",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Button_ClearLogs",
				"values": {
					"type": "crt.Button",
					"caption": "#ResourceString(Button_ClearLogs_caption)#",
					"color": "default",
					"disabled": false,
					"size": "large",
					"iconPosition": "only-icon",
					"visible": true,
					"icon": "delete-button-icon",
					"clicked": {
						"request": "atf.ClearLogs"
					}
				},
				"parentName": "FlexContainer_482ntir",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "FlexContainer_6jlfs5p",
				"values": {
					"type": "crt.FlexContainer",
					"items": [],
					"direction": "column"
				},
				"parentName": "MessagesTabContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "Input_LogMessage",
				"values": {
					"type": "crt.Input",
					"label": "#ResourceString(Input_LogMessage_label)#",
					"control": "$AllMessages",
					"placeholder": "",
					"tooltip": "",
					"readonly": true,
					"multiline": true,
					"labelPosition": "auto",
					"visible": true
				},
				"parentName": "FlexContainer_6jlfs5p",
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
					"SocketMessageReceivedFunc": {},
					"AllMessages": {
						"modelConfig": {}
					},
					"FileChangesVisible": {},
					"AtfName": {
						"modelConfig": {
							"path": "PDS.AtfName"
						}
					},
					"DiffContent": {},
					"PDS_AtfRepositoryUrl_yidr394": {
						"modelConfig": {
							"path": "PDS.AtfRepositoryUrl"
						}
					},
					"PDS_AtfAccessToken_nzu843o": {
						"modelConfig": {
							"path": "PDS.AtfAccessToken"
						}
					},
					"GridDetail_t9wy0f2": {
						"isCollection": true,
						"modelConfig": {
							"path": "GridDetail_t9wy0f2DS",
							"filterAttributes": [
								{
									"name": "GridDetailSearchFilter_m373eyy_GridDetail_t9wy0f2",
									"loadOnChange": true
								}
							]
						},
						"viewModelConfig": {
							"attributes": {
								"GridDetail_t9wy0f2DS_AtfName": {
									"modelConfig": {
										"path": "GridDetail_t9wy0f2DS.AtfName"
									}
								},
								"GridDetail_t9wy0f2DS_Id": {
									"modelConfig": {
										"path": "GridDetail_t9wy0f2DS.Id"
									}
								}
							}
						}
					},
					"PDS_AtfUserName_zwx7r4g": {
						"modelConfig": {
							"path": "PDS.AtfUserName"
						}
					},
					"PDS_AtfLogin_g255988": {
						"modelConfig": {
							"path": "PDS.AtfLogin"
						}
					},
					"PDS_AtfPassword_rrf2i89": {
						"modelConfig": {
							"path": "PDS.AtfPassword"
						}
					},
					"PDS_AtfCreatioUrl_hgv046w": {
						"modelConfig": {
							"path": "PDS.AtfCreatioUrl"
						}
					},
					"PDS_AtfActiveBranch_zvr5u2c": {
						"modelConfig": {
							"path": "PDS.AtfActiveBranch"
						}
					},
					"PDS_AtfApplication_8tomm9h": {
						"modelConfig": {
							"path": "PDS.AtfApplication"
						}
					},
					"GridDetail_62r7nr2": {
						"isCollection": true,
						"modelConfig": {
							"path": "GridDetail_62r7nr2DS",
							"filterAttributes": [
								{
									"name": "GridDetailSearchFilter_z1pzuec_GridDetail_62r7nr2",
									"loadOnChange": true
								}
							]
						},
						"viewModelConfig": {
							"attributes": {
								"GridDetail_62r7nr2DS_AtfFileStatus": {
									"modelConfig": {
										"path": "GridDetail_62r7nr2DS.AtfFileStatus"
									}
								},
								"GridDetail_62r7nr2DS_AtfFileName": {
									"modelConfig": {
										"path": "GridDetail_62r7nr2DS.AtfFileName"
									}
								},
								"GridDetail_62r7nr2DS_Id": {
									"modelConfig": {
										"path": "GridDetail_62r7nr2DS.Id"
									}
								}
							}
						}
					},
					"PDS_exeColumn10_b6z2b0g": {
						"modelConfig": {
							"path": "PDS.AtfLocked"
						}
					},
					"PDS_AtfUpdateAvailable_thzvlv4": {
						"modelConfig": {
							"path": "PDS.AtfUpdateAvailable"
						}
					},
					"PDS_AtfInstalledAppHash_zifcf8b": {
						"modelConfig": {
							"path": "PDS.AtfInstalledAppHash"
						}
					},
					"PDS_AtfAvailableAppHash_xu2vola": {
						"modelConfig": {
							"path": "PDS.AtfAvailableAppHash"
						}
					},
					"PDS_AtfAutoSync_zrzeu6i": {
						"modelConfig": {
							"path": "PDS.AtfAutoSync"
						}
					}
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"CardState"
				],
				"values": {
					"modelConfig": {}
				}
			},
			{
				"operation": "merge",
				"path": [
					"attributes",
					"Id",
					"modelConfig"
				],
				"values": {
					"path": "PDS.Id"
				}
			}
		]/**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/,
		modelConfigDiff: /**SCHEMA_MODEL_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"path": [],
				"values": {
					"primaryDataSourceName": "PDS",
					"dependencies": {
						"GridDetail_t9wy0f2DS": [
							{
								"attributePath": "AtfRepository",
								"relationPath": "PDS.Id"
							}
						],
						"GridDetail_62r7nr2DS": [
							{
								"attributePath": "Id",
								"relationPath": "PDS.Id"
							}
						]
					}
				}
			},
			{
				"operation": "merge",
				"path": [
					"dataSources"
				],
				"values": {
					"PDS": {
						"type": "crt.EntityDataSource",
						"config": {
							"entitySchemaName": "AtfRepository"
						},
						"scope": "page"
					},
					"GridDetail_t9wy0f2DS": {
						"type": "crt.EntityDataSource",
						"scope": "viewElement",
						"config": {
							"entitySchemaName": "AtfRepositoryBranch",
							"attributes": {
								"AtfName": {
									"path": "AtfName"
								}
							}
						}
					},
					"GridDetail_62r7nr2DS": {
						"type": "crt.EntityDataSource",
						"scope": "viewElement",
						"config": {
							"entitySchemaName": "AtfGitChangedFiles",
							"attributes": {
								"AtfFileStatus": {
									"path": "AtfFileStatus"
								},
								"AtfFileName": {
									"path": "AtfFileName"
								}
							}
						}
					}
				}
			}
		]/**SCHEMA_MODEL_CONFIG_DIFF*/,
		handlers: /**SCHEMA_HANDLERS*/[
			{
				request: 'atf.LoadWorkspaceFromLocalCopy',
				handler: async (request, next) => {
					const handlerChain = sdk.HandlerChainService.instance;
					const dialogConfig = {
						titleCaption: "WARNING - IRREVERSABLE ACTION",
						messageCaption: "You are about to install local copy onto Creatio. This will overwrite all changes made in Creatio. Are you sure you want to proceed?",
						yesButtonCaption: "Yes",
						noButtonCaption: "No"
					};
					
					const dialogResult = await handlerChain.process({
						type: 'crt.ShowDialogRequest',
						$context: this.$context,
						dialogConfig: {
							data: {
								title: dialogConfig.titleCaption,
								message: dialogConfig.messageCaption,
								actions: [
									{
										key: 'No',
										config: {
											color: 'default',
											caption: dialogConfig.noButtonCaption
										}
									},
									{
										key: 'Yes',
										config: {
											color: 'primary',
											caption: dialogConfig.yesButtonCaption
										}
									},
								]
							}
						}
					});
					
					if(dialogResult === 'Yes'){
						await handlerChain.process({
							type: 'crt.RunBusinessProcessRequest',
							$context: request.$context,
							scopes: [...request.scopes],
							processName: request.processName,
							processRunType: request.processRunType,
							saveAtProcessStart: true,
							showNotification: true,
							recordIdProcessParameterName: request.recordIdProcessParameterName,
						});
					}
				}
			},
			{
				request: 'atf.OnLoadChangesToLocalCopyClick',
				handler: async (request, next) => {
					const handlerChain = sdk.HandlerChainService.instance;
					await handlerChain.process({
						type: 'crt.RunBusinessProcessRequest',
						$context: request.$context,
						scopes: [...request.scopes],
						processName: request.processName,
						processRunType: request.processRunType,
						saveAtProcessStart: true,
						showNotification: true,
						recordIdProcessParameterName: request.recordIdProcessParameterName,
					});
					await handlerChain.process({
						type: 'atf.OnGetDiffCLicked',
						$context: request.$context,
						scopes: [...request.scopes]
					});
					return next?.handle(request);
				}
			},
			
			{
				request: 'atf.CaptureClioArgs',
				handler: async (request, next) => {
					const endpoint = "/rest/Tide/CaptureClioArgs";
					const httpClientService = new sdk.HttpClientService();
					await httpClientService.get(endpoint)
					const handlerChain = sdk.HandlerChainService.instance;
					await handlerChain.process({
						type: 'crt.RunBusinessProcessRequest',
						$context: request.$context,
						scopes: [...request.scopes],
						processName: request.processName,
						processRunType: request.processRunType,
						recordIdProcessParameterName: request.recordIdProcessParameterName,
					});
				}
			},
			{
				request: 'crt.HandleViewModelInitRequest',
				handler: async (request, next) => {
					const { $context } = request;
					$context.SocketMessageReceivedFunc = async function(event, message) {
						if (message.Header.Sender === "Clio") {
							const body = JSON.parse(message.Body)
							if(body.commandName ==='Show logs') {
								const allMessages = await request.$context.AllMessages ?? "";
								// request.$context.AllMessages = body.message?.trim() +  "\r\n" + allMessages;
								request.$context.AllMessages = body.message?.trim() + "<br/>" + "\r\n" + allMessages;
							}
						}
					}
					Terrasoft.ServerChannel.on(Terrasoft.EventName.ON_MESSAGE, (await $context.SocketMessageReceivedFunc), $context);
					request.$context.FileChangesVisible = false;
					const endpoint = "/rest/Tide/CaptureClioArgs";
					const httpClientService = new sdk.HttpClientService();
					await httpClientService.get(endpoint)
					return next?.handle(request);
				}
			},
			{
				request: 'atf.OnGetDiffCLicked',
				handler: async (request, next) => {
					const handlerChain = sdk.HandlerChainService.instance;
					await handlerChain.process({
						type: 'crt.LoadDataRequest',
						$context: request.$context,
						scopes: request.scopes,
						dataSourceName: 'GridDetail_62r7nr2DS', config: {
							loadType: "reload",
							useLastLoadParameters: true
						}
					});
					
					request.$context.FileChangesVisible = true;
					const id = await request.$context.Id
					const endpoint = `/rest/Tide/GetDiffForRepository?repositoryId=${id}`;
					const httpClientService = new sdk.HttpClientService();
					const response = await httpClientService.get(endpoint);
					request.$context.DiffContent = response.body;
					return next?.handle(request);
				}
			},
			{
				request: 'atf.DiscardChanges',
				handler: async (request, next) => {
					const selectedRows = request.$context.attributes.GridDetail_62r7nr2_SelectedRows;
					let filesToDiscard = [];
					for (let i = 0; i < selectedRows.length; i++) {
						const id = request.$context.attributes.GridDetail_62r7nr2[i].attributes.GridDetail_62r7nr2DS_Id;
						if(id === selectedRows[i]){
							const fileName = request.$context.attributes.GridDetail_62r7nr2[i].attributes.GridDetail_62r7nr2DS_AtfFileName;
							console.log(fileName);
							filesToDiscard.push(fileName);
						}
					}
					var endpoint = `/rest/Tide/DiscardFileChanges`;
					const httpClientService = new sdk.HttpClientService();
					
					const body = {
						files: filesToDiscard,
						repositoryId: await request.$context.Id
					}
					
					const response = await httpClientService.post(endpoint, body);
					if(response && response.body==="OK"){
						const handlerChain = sdk.HandlerChainService.instance;
						await handlerChain.process({
							type: 'atf.OnGetDiffCLicked',
							$context: request.$context,
							scopes: [...request.scopes],
						});
						
						await handlerChain.process({
							type: 'crt.LoadDataRequest',
							$context: request.$context,
							scopes: request.scopes,
							dataSourceName: 'GridDetail_62r7nr2DS', config: {
								loadType: "reload",
								useLastLoadParameters: true
							}
						});
						
					}
					return next?.handle(request);
				},
			},
			{
				request: 'atf.ClearLogs',
				handler: async (request, next) => {
					request.$context.AllMessages = "";
					return next?.handle(request);
				}
			}

		]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
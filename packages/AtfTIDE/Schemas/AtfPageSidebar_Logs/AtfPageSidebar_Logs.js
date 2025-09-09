define("AtfPageSidebar_Logs", /**SCHEMA_DEPS*/[]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/()/**SCHEMA_ARGS*/ {
	return {
		viewConfigDiff: /**SCHEMA_VIEW_CONFIG_DIFF*/[
			{
				"operation": "merge",
				"name": "MainContainer",
				"values": {
					"padding": {
						"top": "14px",
						"bottom": "large",
						"right": "small",
						"left": "small"
					}
				}
			},
			{
				"operation": "insert",
				"name": "Button_ClearLogs",
				"values": {
					"type": "crt.Button",
					"clicked": {
						"request": "atf.ClearLogs"
					},
					"caption": "#ResourceString(Button_ClearLogs_caption)#",
					"color": "default",
					"disabled": false,
					"size": "medium",
					"iconPosition": "only-icon",
					"visible": true,
					"icon": "delete-button-icon"
				},
				"parentName": "ActionButtonsContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "RichTextEditor_zunkpu4",
				"values": {
					"type": "crt.RichTextEditor",
					"label": "#ResourceString(RichTextEditor_zunkpu4_label)#",
					"control": "$AllMessages",
					"labelPosition": "above",
					"placeholder": "",
					"tooltip": "",
					"needHandleSave": true,
					"filesStorage": {
						"masterRecordColumnValue": "$Id",
						"entitySchemaName": "SysFile",
						"recordColumnName": "RecordId"
					},
					"caption": "#ResourceString(RichTextEditor_zunkpu4_caption)#",
					"visible": true,
					"readonly": true,
					"toolbarDisplayMode": null
				},
				"parentName": "MainContainer",
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
					}
				}
			}
		]/**SCHEMA_VIEW_MODEL_CONFIG_DIFF*/,
		modelConfigDiff: /**SCHEMA_MODEL_CONFIG_DIFF*/[]/**SCHEMA_MODEL_CONFIG_DIFF*/,
		handlers: /**SCHEMA_HANDLERS*/[
			{
				request: 'atf.ClearLogs',
				handler: async (request, next) => {
					request.$context.AllMessages = '';
				}
			},
			{
				request: 'crt.HandleViewModelInitRequest',
				handler: async (request, next) => {
					const { $context } = request;

					request.$context.AllMessages = "Initialize action ...<br>"
						+"Setup connection to server ...<br>"
						+"Loading ...";

					$context.SocketMessageReceivedFunc = async function(event, message) {
						if (message.Header.Sender === "Clio") {
							const body = JSON.parse(message.Body)
							if(body.commandName ==='Show logs') {
								const allMessages = await request.$context.AllMessages ?? "";
								const newMessages = body.message?.split('\r\n').reverse().join('<br>');
								request.$context.AllMessages = newMessages + "<br>" + allMessages;
							}
						}
					}
					Terrasoft.ServerChannel.on(Terrasoft.EventName.ON_MESSAGE, (await $context.SocketMessageReceivedFunc), $context);
					return next?.handle(request);
				}
			},
		]/**SCHEMA_HANDLERS*/,
		converters: /**SCHEMA_CONVERTERS*/{}/**SCHEMA_CONVERTERS*/,
		validators: /**SCHEMA_VALIDATORS*/{}/**SCHEMA_VALIDATORS*/
	};
});
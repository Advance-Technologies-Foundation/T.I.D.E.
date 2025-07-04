define("Page_LogTerminal", /**SCHEMA_DEPS*/["@creatio-devkit/common"]/**SCHEMA_DEPS*/, function/**SCHEMA_ARGS*/(sdk)/**SCHEMA_ARGS*/ {
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
				"operation": "merge",
				"name": "CancelButton",
				"values": {
					"clicked": {
						"request": "crt.ClosePageRequest"
					},
					"caption": "#ResourceString(CancelButton_caption)#",
					"color": "default",
					"size": "large",
					"iconPosition": "only-text",
					"clickMode": "default"
				}
			},
			{
				"operation": "remove",
				"name": "SaveButton"
			},
			{
				"operation": "insert",
				"name": "FlexContainer_0boicqp",
				"values": {
					"layoutConfig": {
						"column": 1,
						"row": 1,
						"colSpan": 1,
						"rowSpan": 12
					},
					"type": "crt.FlexContainer",
					"direction": "column",
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
					"justifyContent": "start",
					"gap": "small",
					"wrap": "nowrap"
				},
				"parentName": "MainContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "RichTextEditor_LogMessages",
				"values": {
					"type": "crt.RichTextEditor",
					"label": "#ResourceString(RichTextEditor_LogMessages_label)#",
					"labelPosition": "above",
					"placeholder": "",
					"tooltip": "",
					"needHandleSave": true,
					"control": "$AllMessages",
					"caption": "#ResourceString(RichTextEditor_LogMessages_caption)#",
					"visible": true,
					"readonly": false,
					"toolbarDisplayMode": null
				},
				"parentName": "FlexContainer_0boicqp",
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
				request: 'atf.HandleViewModelInitRequest',
				handler: async (request, next) => {
					const handlerChain = sdk.HandlerChainService.instance;
					return await handlerChain.process({
						type: 'crt.ClosePageRequest',
						$context: request.$context,
					});
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
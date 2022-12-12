import Events from './events.js';
import IndexEventHandler from './handlers/indexEventHandler.js';
import Editor from './editor.js';

const state = {
  apiKeyInput: document.getElementById('apiKey'),
  apiKeyError: document.getElementById('apiKeyError'),
  appInput: document.getElementById('app'),
  appError: document.getElementById('appError'),
  recordInput: document.getElementById('record'),
  fieldsButton: document.getElementById('fieldsButton'),
  fieldsModal: document.getElementById('fieldsModal'),
  fieldsSearchBox: document.getElementById('fieldsSearchBox'),
  fieldsList: document.getElementById('fieldsList'),
  operatorsButton: document.getElementById('operatorsButton'),
  operatorsModal: document.getElementById('operatorsModal'),
  mathOperatorsList: document.getElementById('mathOperatorsList'),
  comparisonOperatorsList: document.getElementById('comparisonOperatorsList'),
  logicalOperatorsList: document.getElementById('logicalOperatorsList'),
  functionsButton: document.getElementById('functionsButton'),
  editor: document.getElementById('editor'),
};

const editorView = Editor.setup(state);

document.addEventListener(Events.click, e =>
  IndexEventHandler.handleDocumentClick(e, state)
);

state.fieldsButton.addEventListener(Events.click, e =>
  IndexEventHandler.handleFieldsButtonClick(e, state)
);

state.operatorsButton.addEventListener(Events.click, e => 
  IndexEventHandler.handleOperatorsButtonClick(e, state)
);

state.apiKeyInput.addEventListener(Events.change, e => {
  IndexEventHandler.handleApiInputChange(e, state);
});

state.apiKeyInput.addEventListener(Events.input, e =>
  IndexEventHandler.handleApiInput(e, state)
);

state.appInput.addEventListener(Events.change, e =>
  IndexEventHandler.handleAppInputChange(e, state)
);

state.fieldsSearchBox.addEventListener(Events.input, e =>
  IndexEventHandler.handleFieldsSearchBoxInput(e, state)
);

state.fieldsList.addEventListener(Events.click, e =>
  IndexEventHandler.handleFieldsListClick(e, state, editorView)
);
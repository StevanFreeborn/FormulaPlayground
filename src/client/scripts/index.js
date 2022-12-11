import Events from './events.js';
import IndexEventHandler from './handlers/indexEventHandler.js';
import Editor from './editor.js';

const state = {
  apiKeyInput: document.getElementById('apiKey'),
  apiKeyError: document.getElementById('apiKeyError'),
  appInput: document.getElementById('app'),
  recordInput: document.getElementById('record'),
  fieldsButton: document.getElementById('fieldsButton'),
  fieldsModal: document.getElementById('fieldsModal'),
  fieldsSearchBox: document.getElementById('fieldsSearchBox'),
  fieldsList: document.getElementById('fieldsList'),
  operatorsButton: document.getElementById('operatorsButton'),
  functionsButton: document.getElementById('functionsButton'),
  editor: document.getElementById('editor'),
};

const editor = Editor.setup(state);

state.fieldsButton.addEventListener(Events.click, e => {
  if (state.fieldsModal.style.left || state.fieldsModal.style.top) {
    state.fieldsModal.style.left = '';
    state.fieldsModal.style.top = '';
    return;
  }
  const buttonPosition = e.currentTarget.getBoundingClientRect();
  state.fieldsSearchBox.focus();
  state.fieldsModal.style.left = buttonPosition.x + 'px';
  state.fieldsModal.style.top = (buttonPosition.y + buttonPosition.height) + 'px';
})

state.apiKeyInput.addEventListener(Events.change, e => {
  IndexEventHandler.handleApiInputChange(e, state);
});

state.apiKeyInput.addEventListener(Events.input, e =>
  IndexEventHandler.handleApiInput(e, state)
);

state.appInput.addEventListener(Events.change, e =>
  IndexEventHandler.handleAppInputChange(e, state)
);

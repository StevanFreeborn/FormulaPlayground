import Events from './events.js';
import IndexEventHandler from './handlers/indexEventHandler.js';
import Editor from './editor.js';

const state = {
  apiKeyInput: document.getElementById('apiKey'),
  apiKeyError: document.getElementById('apiKeyError'),
  appInput: document.getElementById('app'),
  recordInput: document.getElementById('record'),
  editor: document.getElementById('editor'),
};

const editor = Editor.setup(state);

state.apiKeyInput.addEventListener(Events.change, e => {
  IndexEventHandler.handleApiInputChange(e, state);
});

state.apiKeyInput.addEventListener(Events.input, e =>
  IndexEventHandler.handleApiInput(e, state)
);

state.appInput.addEventListener(Events.change, e =>
  IndexEventHandler.handleAppInputChange(e, state)
);

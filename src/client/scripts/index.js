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
  editorView: Editor.setup(document.getElementById('editor')),
  showApiKeyError(message) {
    this.apiKeyError.innerText = message;
  },
  resetAppInput() {
    this.appInput.selectedIndex = 0;
    while (this.appInput.childElementCount > 1) {
      this.appInput.removeChild(state.appInput.lastChild);
    }
  },
  addAppOption(app) {
    const option = document.createElement('option');
    option.id = app.id;
    option.value = app.id;
    option.text = app.name;
    this.appInput.append(option);
  },
  addAppOptions(apps) {
    apps.forEach(app => this.addAppOption(app));
  },
  showAppInput() {
    this.appInput.classList.remove('visually-hidden');
  },
  resetRecordInput() {
    this.recordInput.classList.add('visually-hidden');
    this.recordInput.value = '';
  },
  isFieldsModalDisplayed() {
    return this.fieldsModal.style.left || this.fieldsModal.style.top;
  },
  showFieldsModal() {
    const pos = this.fieldsButton.getBoundingClientRect();
    this.fieldsModal.style.left = pos.x + 'px';
    this.fieldsModal.style.top = pos.y + pos.height + 'px';
    this.fieldsSearchBox.focus();
  },
  hideFieldsModal() {
    this.fieldsModal.style.left = '';
    this.fieldsModal.style.top = '';
    this.fieldsSearchBox.value = '';
    this.filterFieldsList(this.fieldsSearchBox.value);
  },
  hideFieldName(element, filterValue) {
    const isMatch = element.innerText.toLowerCase().includes(filterValue);

    if (isMatch) {
      element.style.display = '';
      return;
    }

    element.style.display = 'none';
  },
  filterFieldsList(filter) {
    const fieldNameElements = [...this.fieldsList.getElementsByTagName('li')];

    if (fieldNameElements[0].id == 'fieldsPlaceHolder') {
      return;
    }

    fieldNameElements.forEach(fieldNameElement =>
      this.hideFieldName(fieldNameElement, filter)
    );
  },
  insertFieldToken(tokenText) {
    const fieldToken = `{:${tokenText}}`;
    const transaction = this.editorView.state.replaceSelection(fieldToken);
    this.editorView.dispatch(transaction);
  },
  focusOnEditor() {
    this.editorView.focus();
  },
  isOperatorsModalDisplayed() {
    return this.operatorsModal.style.left || this.operatorsModal.style.top;
  },
  showOperatorsModal() {
    const pos = this.operatorsButton.getBoundingClientRect();
    this.operatorsModal.style.left = pos.x + 'px';
    this.operatorsModal.style.top = pos.y + pos.height + 'px';
  },
  hideOperatorsModal() {
    this.operatorsModal.style.left = '';
    this.operatorsModal.style.top = '';
  },
};

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
  IndexEventHandler.handleFieldsListClick(e, state)
);

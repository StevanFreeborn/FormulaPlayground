import Editor from './editor.js';
import Events from './events.js';
import IndexEventHandler from './handlers/indexEventHandler.js';
import { CUSTOM_FUNCTIONS, OPERATORS } from './constants.js';
import OperatorTypes from './operatorTypes.js';

const state = {
  timezoneInput: document.getElementById('timezone'),
  apiKeyInput: document.getElementById('apiKey'),
  apiKeyError: document.getElementById('apiKeyError'),
  appContainer: document.getElementById('appContainer'),
  appInput: document.getElementById('app'),
  appError: document.getElementById('appError'),
  recordContainer: document.getElementById('recordContainer'),
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
  functionsModal: document.getElementById('functionsModal'),
  functionsSearchBox: document.getElementById('functionsSearchBox'),
  functionTabButtons: document.getElementsByClassName('fn-type-btn'),
  functionsList: document.getElementById('functionsList'),
  editorView: Editor.setup(document.getElementById('editor')),
  runFormulaButton: document.getElementById('runFormulaButton'),
  validateSyntaxButton: document.getElementById('validateSyntaxButton'),
  validationModal: new bootstrap.Modal(document.getElementById('validationModal')),
  validationModalHeader: document.getElementById('validationModalHeader'),
  validationModalTitle: document.getElementById('validationModalTitle'),
  validationModalTitleIcon: document.getElementById('validationModalTitleIcon'),
  validationModalBody: document.getElementById('validationModalBody'),
  formulaResult: document.getElementById('formulaResult'),
  createOperatorListElement(operator) {
    const listElement = document.createElement('li');
    listElement.classList.add('container');
    listElement.classList.add('operator');
    listElement.setAttribute('data-operator-symbol', operator.symbol);

    const rowElement = document.createElement('div');
    rowElement.classList.add('row');

    const symbolElement = document.createElement('div');
    symbolElement.classList.add('col-3');
    symbolElement.classList.add('text-center');
    symbolElement.innerHTML = operator.symbol;

    const nameElement = document.createElement('div');
    nameElement.classList.add('col-9');
    nameElement.classList.add('text-start');
    nameElement.classList.add('text-nowrap');
    nameElement.innerHTML = operator.name;

    rowElement.append(symbolElement);
    rowElement.append(nameElement);

    listElement.append(rowElement);

    return listElement;
  },
  addMathOperatorListItem(operator) {
    const listElement = this.createOperatorListElement(operator);
    this.mathOperatorsList.append(listElement);
  },
  addComparsionOperatorListItem(operator) {
    const listElement = this.createOperatorListElement(operator);
    this.comparisonOperatorsList.append(listElement);
  },
  addLogicalOperatorListItem(operator) {
    const listElement = this.createOperatorListElement(operator);
    this.logicalOperatorsList.append(listElement);
  },
  setupOperatorsList() {
    OPERATORS.forEach(operator => {
      switch (operator.type) {
        case OperatorTypes.math:
          this.addMathOperatorListItem(operator);
          break;
        case OperatorTypes.comparison:
          this.addComparsionOperatorListItem(operator);
          break;
        case OperatorTypes.logical:
          this.addLogicalOperatorListItem(operator);
          break;
        default:
          break;
      }
    });
  },
  createFunctionListElement(fn) {
    const listElement = document.createElement('li');
    listElement.classList.add('function');
    listElement.innerHTML = fn.name;
    listElement.setAttribute('data-type', fn.type);
    listElement.setAttribute('data-snippet', fn.snippet);
    return listElement;
  },
  addFunctionListElement(fn) {
    const listElement = this.createFunctionListElement(fn);
    this.functionsList.append(listElement);
  },
  setupFunctionsList() {
    CUSTOM_FUNCTIONS
    .sort((a,b) => (a.name > b.name) ? 1 : ((b.name > a.name) ? -1 : 0))
    .forEach(fn => this.addFunctionListElement(fn));
  },
  showApiKeyError(message) {
    this.apiKeyError.innerText = message;
  },
  resetApiKeyError() {
    this.apiKeyError.innerText = '';
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
    this.appContainer.classList.remove('visually-hidden');
  },
  hideAppInput() {
    this.appContainer.classList.add('visually-hidden');
  },
  resetAppInput() {
    this.hideAppInput();
    this.appInput.selectedIndex = 0;
    while (this.appInput.childElementCount > 1) {
      this.appInput.removeChild(state.appInput.lastChild);
    }
  },
  showAppError(message) {
    this.appError.innerText = message;
  },
  resetAppError() {
    this.appError.innerText = '';
  },
  showRecordInput() {
    state.recordContainer.classList.remove('visually-hidden');
  },
  hideRecordInput() {
    this.recordContainer.classList.add('visually-hidden');
  },
  resetRecordInput() {
    this.hideRecordInput();
    this.recordInput.value = '';
  },
  isFieldsModalDisplayed() {
    return this.fieldsModal.style.left || this.fieldsModal.style.top;
  },
  showFieldsModal() {
    const pos = this.fieldsButton.getBoundingClientRect();
    this.fieldsModal.style.left = pos.x + 'px';
    this.fieldsModal.style.top = pos.y + pos.height + 'px';
    this.fieldsModal.classList.remove('visually-hidden');
    this.fieldsSearchBox.focus();
  },
  hideFieldsModal() {
    this.fieldsModal.style.left = '';
    this.fieldsModal.style.top = '';
    this.fieldsSearchBox.value = '';
    this.fieldsModal.classList.add('visually-hidden');
    this.filterFieldsList(this.fieldsSearchBox.value);
  },
  hideFieldName(element, filterValue) {
    const isMatch = element.innerText
      .toLowerCase()
      .includes(filterValue.toLowerCase());

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
  clearFieldsList() {
    while (this.fieldsList.childElementCount > 0) {
      this.fieldsList.removeChild(this.fieldsList.lastChild);
    }
  },
  addFieldsListPlaceHolder() {
    const placeHolder = document.createElement('li');
    placeHolder.id = 'fieldsPlaceHolder';
    placeHolder.innerText = 'No fields yet retrieved.';
    this.fieldsList.append(placeHolder);
  },
  addFieldListItem(field) {
    const fieldElement = document.createElement('li');
    const fieldNameElement = document.createElement('span');
    fieldNameElement.innerText = field.name;

    fieldNameElement.classList.add('field-name');
    fieldElement.append(fieldNameElement);
    this.fieldsList.append(fieldElement);
  },
  addFieldListItems(fields) {
    fields.forEach(field => this.addFieldListItem(field));
  },
  insertFieldToken(tokenText) {
    const fieldToken = `{:${tokenText}}`;
    const transaction = this.editorView.state.replaceSelection(fieldToken);
    this.editorView.dispatch(transaction);
  },
  insertOperator(operator) {
    const transaction = this.editorView.state.replaceSelection(operator);
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
    this.operatorsModal.classList.remove('visually-hidden');
  },
  hideOperatorsModal() {
    this.operatorsModal.style.left = '';
    this.operatorsModal.style.top = '';
    this.operatorsModal.classList.add('visually-hidden');
  },
  isFunctionsModalDisplayed() {
    return this.functionsModal.style.left || this.functionsModal.style.top;
  },
  showFunctionsModal() {
    const pos = this.functionsButton.getBoundingClientRect();
    this.functionsModal.style.left = pos.x + 'px';
    this.functionsModal.style.top = pos.y + pos.height + 'px';
    this.functionsModal.classList.remove('visually-hidden');
  },
  hideFunctionsModal() {
    this.functionsModal.style.left = '';
    this.functionsModal.style.top = '';
    this.functionsModal.classList.add('visually-hidden');
  },
  getActiveFunctionTab() {
    return document.querySelector('.fn-type-btn.active');
  },
  hideFunctionListElement(element, nameFilter, typeFilter) {
    const elementText = element.innerText.toLowerCase();
    const elementType = element.getAttribute('data-type');

    const isNameMatch = elementText.includes(nameFilter.toLowerCase());
    const isTypeMatch = elementType == typeFilter || typeFilter == 'all';

    if (isNameMatch && isTypeMatch) {
      element.style.display = '';
      return;
    }

    element.style.display = 'none';
  },
  filterFunctionsList(nameFilter, typeFilter) {
    const functionListElements = [
      ...this.functionsList.getElementsByTagName('li'),
    ];

    functionListElements.forEach(functionListElement =>
      this.hideFunctionListElement(functionListElement, nameFilter, typeFilter)
    );
  },
  insertFunctionSnippet(snippet) {
    const transaction = this.editorView.state.replaceSelection(snippet);
    this.editorView.dispatch(transaction);
  },
  displayFormulaResult(result) {
    this.formulaResult.innerHTML = result;
  },
  resetValidationModal() {
    this.validationModalBody.innerHTML = '';
    this.validationModalHeader.classList.remove('border-invalid');
    this.validationModalHeader.classList.remove('border-valid');
    this.validationModalTitleIcon.classList.remove('icon-invalid');
    this.validationModalTitleIcon.classList.remove('icon-valid');
  },
  setupInvalidModalHeader() {
    const invalidIcon = `
    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-exclamation-triangle-fill" viewBox="0 0 16 16">
      <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
    </svg>
    `;

    const invalidHeaderTitle = 'Formula Syntax Invalid';

    this.validationModalHeader.classList.add('border-invalid');
    this.validationModalTitleIcon.innerHTML = invalidIcon;
    this.validationModalTitleIcon.classList.add('icon-invalid');
    this.validationModalTitle.innerText = invalidHeaderTitle;
  },
  setupRunErrorModalHeader() {
    const invalidIcon = `
    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-exclamation-triangle-fill" viewBox="0 0 16 16">
      <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
    </svg>
    `;

    const invalidHeaderTitle = 'Run Error';

    this.validationModalHeader.classList.add('border-invalid');
    this.validationModalTitleIcon.innerHTML = invalidIcon;
    this.validationModalTitleIcon.classList.add('icon-invalid');
    this.validationModalTitle.innerText = invalidHeaderTitle;
  },
  setupValidModalHeader() {
    const validIcon = `
    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-check-circle-fill" viewBox="0 0 16 16">
      <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
    </svg>
    `;

    const validHeaderTitle = 'Formula Syntax Valid';

    this.validationModalHeader.classList.add('border-valid');
    this.validationModalTitleIcon.innerHTML = validIcon;
    this.validationModalTitleIcon.classList.add('icon-valid');
    this.validationModalTitle.innerText = validHeaderTitle;
  },
  getFormulaRequest() {
    return {
      apiKey: this.apiKeyInput.value,
      appId: this.appInput.value ? this.appInput.value : 0,
      recordId: this.recordInput.value ? this.recordInput.value : 0,
      formula: this.editorView.state.doc.toJSON().join('\n'),
      timezone: this.timezoneInput.value,
    }
  },
  setupEventListeners() {
    const eventHandler = new IndexEventHandler(this);

    document.addEventListener(Events.click, eventHandler.handleDocumentClick);
    this.fieldsButton.addEventListener(
      Events.click,
      eventHandler.handleFieldsButtonClick
    );
    this.operatorsButton.addEventListener(
      Events.click,
      eventHandler.handleOperatorsButtonClick
    );
    this.apiKeyInput.addEventListener(
      Events.change,
      eventHandler.handleApiInputChange
    );
    this.apiKeyInput.addEventListener(
      Events.input,
      eventHandler.handleApiInput
    );
    this.appInput.addEventListener(
      Events.change,
      eventHandler.handleAppInputChange
    );
    this.fieldsSearchBox.addEventListener(
      Events.input,
      eventHandler.handleFieldsSearchBoxInput
    );
    this.fieldsList.addEventListener(
      Events.click,
      eventHandler.handleFieldsListClick
    );
    this.mathOperatorsList.addEventListener(
      Events.click,
      eventHandler.handleOperatorListClick
    );
    this.comparisonOperatorsList.addEventListener(
      Events.click,
      eventHandler.handleOperatorListClick
    );
    this.logicalOperatorsList.addEventListener(
      Events.click,
      eventHandler.handleOperatorListClick
    );
    this.functionsButton.addEventListener(
      Events.click,
      eventHandler.handleFunctionsButtonClick
    );
    this.functionsSearchBox.addEventListener(
      Events.input,
      eventHandler.handleFnSearchBoxInput
    );
    [...this.functionTabButtons].forEach(button =>
      button.addEventListener(
        Events.click,
        eventHandler.handleFunctionTabButtonClick
      )
    );
    this.functionsList.addEventListener(
      Events.click,
      eventHandler.handleFunctionsListClick
    );
    this.runFormulaButton.addEventListener(
      Events.click,
      eventHandler.handleRunFormulaButtonClick
    );
    this.validateSyntaxButton.addEventListener(
      Events.click,
      eventHandler.handleValidateSyntaxButtonClick
    );
  },
};

state.setupOperatorsList();
state.setupFunctionsList();
state.setupEventListeners();
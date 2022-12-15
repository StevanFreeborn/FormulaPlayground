import Editor from './editor.js';
import Events from './events.js';
import IndexEventHandler from './handlers/indexEventHandler.js';
import { CUSTOM_FUNCTIONS, OPERATORS } from './constants.js';
import OperatorTypes from './operatorTypes.js';

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
  functionsModal: document.getElementById('functionsModal'),
  functionsSearchBox: document.getElementById('functionsSearchBox'),
  functionTabButtons: document.getElementsByClassName('fn-type-btn'),
  functionsList: document.getElementById('functionsList'),
  editorView: Editor.setup(document.getElementById('editor')),
  runFormulaButton: document.getElementById('runFormulaButton'),
  validateSyntaxButton: document.getElementById('validateSyntaxButton'),
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
    CUSTOM_FUNCTIONS.forEach(fn => this.addFunctionListElement(fn));
  },
  showApiKeyError(message) {
    this.apiKeyError.innerText = message;
  },
  resetApiKeyError() {
    this.apiKeyError.innerText = '';
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
  showAppError(message) {
    this.appError.innerText = message;
  },
  resetAppError() {
    this.appError.innerText = '';
  },
  showRecordInput() {
    state.recordInput.classList.remove('visually-hidden');
  },
  hideRecordInput() {
    this.recordInput.classList.add('visually-hidden');
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
    const isMatch = element.innerText.toLowerCase().includes(filterValue.toLowerCase());

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
  resetFunctionsList() {
    while (this.functionsList.childElementCount > 0) {
      this.functionsList.removeChild(this.functionsList.lastChild);
    }
  },
  filterFunctionsList(nameFilter, typeFilter) {
    const functionListElements = [
      ...this.functionsList.getElementsByTagName('li'),
    ];

    functionListElements.forEach(functionListElement =>
      this.hideFunctionListElement(functionListElement, nameFilter, typeFilter)
    );
  },
  displayFormulaResult(result) {
    this.formulaResult.innerHTML = result;
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

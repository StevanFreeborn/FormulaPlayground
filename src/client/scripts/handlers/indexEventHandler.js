import AppService from '../services/appService.js';
import FieldService from '../services/fieldService.js';

export default class IndexEventHandler {
  static handleApiInputChange = async (e, state) => {
    state.resetRecordInput();
    state.resetAppInput();

    if (!e.currentTarget.value) {
      return;
    }

    const res = await AppService.getApps(e.currentTarget.value);

    if (res.ok == false) {
      const data = await res.json();
      const errorMessage = data?.error ?? 'Unable to retrieve apps.';
      state.showApiKeyError(errorMessage);
      return;
    }

    const apps = await res.json();

    state.addAppOptions(apps);
    state.showAppInput();
  };

  static handleApiInput = (e, state) => {
    state.apiKeyError.innerText = '';
    state.appError.innerText = '';
  };

  static handleAppInputChange = async (e, state) => {
    const clearFieldsList = () => {
      while (state.fieldsList.childElementCount > 0) {
        state.fieldsList.removeChild(state.fieldsList.lastChild);
      }
    };

    clearFieldsList();
    state.appError.innerText = '';

    const placeHolder = document.createElement('li');
    placeHolder.id = 'fieldsPlaceHolder';
    placeHolder.innerText = 'No fields yet retrieved.';
    state.fieldsList.append(placeHolder);

    if (!e.currentTarget.value) {
      state.recordInput.classList.add('visually-hidden');
      return;
    }

    const response = await FieldService.getFields(
      state.apiKeyInput.value,
      e.currentTarget.value
    );

    if (response.ok == false) {
      const data = await response.json();
      const errorMessage = data?.error ?? 'Unable to retrieve fields list.';
      state.appError.innerText = errorMessage;
      return;
    }

    state.recordInput.classList.remove('visually-hidden');

    clearFieldsList();

    const fields = await response.json();

    fields.forEach(field => {
      const fieldElement = document.createElement('li');
      const fieldNameElement = document.createElement('span');
      fieldNameElement.innerText = field.name;

      fieldNameElement.classList.add('field-name');
      fieldElement.append(fieldNameElement);
      state.fieldsList.append(fieldElement);
    });
  };

  static handleFieldsButtonClick = (e, state) => {
    if (state.isFieldsModalDisplayed()) {
      state.hideFieldsModal();
      return;
    }

    state.showFieldsModal();
  };

  static handleOperatorsButtonClick = (e, state) => {
    if (state.isOperatorsModalDisplayed()) {
      state.hideOperatorsModal();
      return;
    }

    state.showOperatorsModal();
  };

  static handleFieldsSearchBoxInput = (e, state) => {
    const filterValue = e.currentTarget.value.toLowerCase();
    state.filterFieldsList(filterValue);
  };

  static handleFieldsListClick = (e, state) => {
    if (e.target.id == 'fieldsPlaceHolder') {
      return;
    }

    state.insertFieldToken(e.target.innerText);
    state.hideFieldsModal();
    state.focusOnEditor();
  };

  static handleDocumentClick = (e, state) => {
    const isNotFldModalFldButtonOrAChild =
      e.target != state.fieldsModal &&
      !state.fieldsModal.contains(e.target) &&
      !state.fieldsButton.contains(e.target) &&
      e.target != state.fieldsButton;

    const isNotOpModalOpButtonOrAChild =
      e.target != state.operatorsModal &&
      !state.operatorsModal.contains(e.target) &&
      !state.operatorsButton.contains(e.target) &&
      e.target != state.operatorsButton;

    if (isNotFldModalFldButtonOrAChild && state.isFieldsModalDisplayed()) {
      state.hideFieldsModal();
    }

    if (isNotOpModalOpButtonOrAChild && state.isOperatorsModalDisplayed()) {
      state.hideOperatorsModal();
    }
  };
}

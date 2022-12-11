import { showPanel } from '@codemirror/view';
import AppService from '../services/appService.js';
import FieldService from '../services/fieldService.js';

export default class IndexEventHandler {
  static handleApiInputChange = async (e, state) => {
    state.recordInput.classList.add('visually-hidden');
    state.recordInput.value = '';

    state.appInput.selectedIndex = 0;

    while (state.appInput.childElementCount > 1) {
      state.appInput.removeChild(state.appInput.lastChild);
    }

    if (!e.currentTarget.value) {
      state.appInput.classList.add('visually-hidden');
      return;
    }

    const res = await AppService.getApps(e.currentTarget.value);

    if (res.ok == false) {
      state.appInput.classList.add('visually-hidden');
      const data = await res.json();
      const errorMessage = data?.error ?? 'Unable to retrieve apps.';
      state.apiKeyError.innerText = errorMessage;
      return;
    }

    const apps = await res.json();

    apps.forEach(app => {
      const option = document.createElement('option');
      option.id = app.id;
      option.value = app.id;
      option.text = app.name;

      state.appInput.append(option);
    });

    state.appInput.classList.remove('visually-hidden');
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

  static handleFieldsButtonClick = async (e, state) => {
    if (state.fieldsModal.style.left || state.fieldsModal.style.top) {
      state.fieldsModal.style.left = '';
      state.fieldsModal.style.top = '';
      return;
    }
  
    const buttonPosition = e.currentTarget.getBoundingClientRect();
    state.fieldsSearchBox.focus();
    state.fieldsModal.style.left = buttonPosition.x + 'px';
    state.fieldsModal.style.top = (buttonPosition.y + buttonPosition.height) + 'px';
  }

  static handleFieldsSearchBoxInput = (e, state) => {
    const filterValue = e.currentTarget.value.toLowerCase();
    const fieldNameElements = state.fieldsList.getElementsByTagName('li');

    if (fieldNameElements[0].id == 'fieldsPlaceHolder') {
      return;
    }

    [...fieldNameElements].forEach(fieldNameElement => {
      const isMatch = fieldNameElement.innerText
        .toLowerCase()
        .includes(filterValue);
      if (isMatch) {
        fieldNameElement.style.display = '';
        return;
      }

      fieldNameElement.style.display = 'none';
    });
  };

  static handleDocumentClick = (e, state) => {
    if (
      e.target != state.fieldsModal &&
      !state.fieldsModal.contains(e.target) &&
      !state.fieldsButton.contains(e.target) &&
      e.target != state.fieldsButton
    ) {
      state.fieldsModal.style.left = '';
      state.fieldsModal.style.top = '';
    }
  };
}

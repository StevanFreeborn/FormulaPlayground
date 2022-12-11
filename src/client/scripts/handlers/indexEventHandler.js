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
  };

  static handleAppInputChange = async (e, state) => {
    while (state.fieldsList.childElementCount > 0) {
      state.fieldsList.removeChild(state.fieldsList.lastChild);
    }
    
    // const placeHolder = document.createElement('li');
    // placeHolder.id = 'fieldsPlaceHolder';
    // placeHolder.innerText = 'No fields yet retrieved.';
    // state.fieldsList.append(placeHolder);

    if (!e.currentTarget.value) {
      state.recordInput.classList.add('visually-hidden');
      return;
    }

    state.recordInput.classList.remove('visually-hidden');

    const response = await FieldService.getFields(state.apiKeyInput.value, e.currentTarget.value);

    if (response.ok == false) {
      const data = await response.json();
      const errorElement = document.createElement('li');
      const errorMessageElement = document.createElement('span');
      errorMessageElement.classList.add('text-danger');
      errorMessageElement.innerText = data?.error ?? 'Unable to retrieve fields list.';
      errorElement.append(errorMessageElement);
      state.fieldsList.append(errorElement);
      return;
    }

    const fields = await response.json();

    fields.forEach(field => {
      const fieldElement = document.createElement('li');
      const fieldNameElement = document.createElement('span');
      fieldNameElement.innerText = field.name;

      fieldNameElement.classList.add('field-name');
      fieldElement.append(fieldNameElement);
      state.fieldsList.append(fieldElement);
    })
  }

  static handleFieldsSearchBoxInput = (e, state) => {
    const filterValue = e.currentTarget.value.toLowerCase();
    const fieldNameElements = state.fieldsList.getElementsByTagName('li');

    [...fieldNameElements].forEach(fieldNameElement => {
      const isMatch = fieldNameElement.innerText.toLowerCase().includes(filterValue);
      if (isMatch) {
        fieldNameElement.style.display = '';
        return;
      }

      fieldNameElement.style.display = 'none';
    })

  }
}

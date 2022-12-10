import AppService from '../services/appService.js';

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

  static handleAppInputChange = (e, state) => {
    if (!e.currentTarget.value) {
      state.recordInput.classList.add('visually-hidden');
      return;
    }
  
    state.recordInput.classList.remove('visually-hidden');
  }
}

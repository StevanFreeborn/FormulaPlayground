const apiKeyInput = document.getElementById('apiKey');
const apiKeyError = document.getElementById('apiKeyError');
const appInput = document.getElementById('app');
const recordInput = document.getElementById('record');

apiKeyInput.addEventListener('change', async e => {
  e.preventDefault();
  appInput.selectedIndex = 0;
  
  while (appInput.childElementCount > 1) {
    appInput.removeChild(appInput.lastChild);
  }

  if (!e.currentTarget.value) {
    appInput.classList.add('visually-hidden');
    return;
  }
  
  const res = await fetch('http://localhost:5085/api/apps', {
    method: 'GET',
    headers: {
      accept: 'application/json',
      'x-api-version': 2,
      'x-apikey': e.currentTarget.value,
    }
  });

  if (res.ok == false) {
    appInput.classList.add('visually-hidden');
    const data = await res.json();
    const errorMessage = data?.error ?? 'Unable to retrieve apps.';
    apiKeyError.innerText = errorMessage;
    return;
  }

  const apps = await res.json();

  apps.forEach(app => {
    const option = document.createElement('option');
    option.id = app.id;
    option.value = app.id;
    option.text = app.name;

    appInput.append(option);
  });

  appInput.classList.remove('visually-hidden');
});

apiKeyInput.addEventListener('input', e => apiKeyError.innerText = '');

appInput.addEventListener('change', e => {
  if (!e.currentTarget.value) {
    recordInput.classList.add('visually-hidden');
    return;
  }

  recordInput.classList.remove('visually-hidden');
});
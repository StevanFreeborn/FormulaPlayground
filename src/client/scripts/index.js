const apiKeyInput = document.getElementById('apiKey')

apiKeyInput.addEventListener('change', async e => {
  e.preventDefault();
  
  const res = await fetch('http://localhost:5085/api/apps', {
    method: 'GET',
    headers: {
      accept: 'application/json',
      'x-api-version': 2,
      'x-apikey': e.currentTarget.value,
    }
  });

  const data = await res.json();

  console.log(data);
});
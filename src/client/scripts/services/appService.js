export default class AppService {
  static baseUrl = 'http://localhost:5085/api/apps';

  static getApps = async apiKey => {
    return await fetch(this.baseUrl, {
      method: 'GET',
      headers: {
        accept: 'application/json',
        'x-api-version': 2,
        'x-apikey': apiKey,
      },
    });
  };
}

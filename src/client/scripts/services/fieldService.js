import {
  BASE_URL,
  API_KEY_HEADER_NAME,
  API_VERSION_HEADER_NAME,
  API_VERSION_HEADER_VALUE,
  ACCEPT_HEADER_NAME,
  JSON_ACCEPT_HEADER_VALUE,
} from '../constants.js';

export default class FieldService {
  static endpoint = `${BASE_URL}/api/fields`;

  static getFields = async (apiKey, appId) => {
    const headers = {};
    headers[API_KEY_HEADER_NAME] = apiKey;
    headers[API_VERSION_HEADER_NAME] = API_VERSION_HEADER_VALUE;
    headers[ACCEPT_HEADER_NAME] = JSON_ACCEPT_HEADER_VALUE;

    return await fetch(this.endpoint, {
      method: 'POST',
      headers: headers,
      body: { appId },
    });
  };
}
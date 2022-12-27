import {
  BASE_URL,
  API_KEY_HEADER_NAME,
  API_VERSION_HEADER_NAME,
  API_VERSION_HEADER_VALUE,
  CONTENT_TYPE_HEADER_NAME,
  JSON_TYPE_HEADER_VALUE,
} from '../constants.js';

export default class FieldService {
  static endpoint = `${BASE_URL}/api/fields`;

  static getFields = async (apiKey, appId) => {
    const headers = {};
    headers[API_KEY_HEADER_NAME] = apiKey;
    headers[API_VERSION_HEADER_NAME] = API_VERSION_HEADER_VALUE;
    headers[CONTENT_TYPE_HEADER_NAME] = JSON_TYPE_HEADER_VALUE;

    return await fetch(this.endpoint, {
      method: 'POST',
      headers: headers,
      body: JSON.stringify({ appId: parseInt(appId) }),
    });
  };
}
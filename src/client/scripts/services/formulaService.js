import {
  BASE_URL,
  API_KEY_HEADER_NAME,
  API_VERSION_HEADER_NAME,
  API_VERSION_HEADER_VALUE,
  CONTENT_TYPE_HEADER_NAME,
  JSON_TYPE_HEADER_VALUE,
} from '../constants.js';

export default class FormulaService {
  static endpoint = `${BASE_URL}/api/formulas`;

  static runFormula = async request => {
    return await fetch(`${this.endpoint}/run`, {
      method: 'POST',
      headers: this.getRequestHeaders(request),
      body: this.getRequestBody(request),
    });
  };

  static validateFormula = async request => {
    return await fetch(`${this.endpoint}/validate`, {
      method: 'POST',
      headers: this.getRequestHeaders(request),
      body: this.getRequestBody(request),
    });
  };

  static getRequestHeaders = request => {
    const headers = {};
    headers[API_KEY_HEADER_NAME] = request.apiKey;
    headers[API_VERSION_HEADER_NAME] = API_VERSION_HEADER_VALUE;
    headers[CONTENT_TYPE_HEADER_NAME] = JSON_TYPE_HEADER_VALUE;
    return headers;
  };

  static getRequestBody = request => {
    return JSON.stringify({
      appId: parseInt(request.appId),
      recordId: parseInt(request.recordId),
      formula: request.formula,
      timezone: request.timezone,
    });
  };
}

export const BASE_URL = process.env.NODE_ENV === 'development'
? 'http://localhost:5085'
: 'https://criminalmindsapi.azurewebsites.net';

export const API_KEY_HEADER_NAME = 'x-apikey';
export const API_VERSION_HEADER_NAME = 'x-api-version';
export const API_VERSION_HEADER_VALUE = 2;
export const ACCEPT_HEADER_NAME = 'Accept';
export const JSON_TYPE_HEADER_VALUE = 'application/json';
export const CONTENT_TYPE_HEADER_NAME = 'Content-Type';
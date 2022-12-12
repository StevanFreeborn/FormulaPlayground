export const BASE_URL = process.env.NODE_ENV === 'development'
? 'http://localhost:5085'
: 'https://criminalmindsapi.azurewebsites.net';

export const API_KEY_HEADER_NAME = 'x-apikey';
export const API_VERSION_HEADER_NAME = 'x-api-version';
export const API_VERSION_HEADER_VALUE = 2;
export const ACCEPT_HEADER_NAME = 'Accept';
export const JSON_TYPE_HEADER_VALUE = 'application/json';
export const CONTENT_TYPE_HEADER_NAME = 'Content-Type';
export const OPERATORS = [
  { name: 'Add/Concatenate', symbol: '+', group: 'math'},
  { name: 'Subtract', symbol: '-', group: 'math'},
  { name: 'Multiply', symbol: '*', group: 'math'},
  { name: 'Divide', symbol: '/', group: 'math'},
  { name: 'Equal', symbol: '==', group: 'comparison'},
  { name: 'Not Equal', symbol: '!=', group: 'comparison'},
  { name: 'Less Than', symbol: '<', group: 'comparison'},
  { name: 'Greater Than', symbol: '>', group: 'comparison'},
  { name: 'Less Than or Equal', symbol: '<=', group: 'comparison'},
  { name: 'Greater Than or Equal', symbol: '>=', group: 'comparison'},
  { name: 'AND', symbol: '&&', group: 'logical'},
  { name: 'OR', symbol: '||', group: 'logical'}
];
export const CUSTOM_FUNCTIONS = [
  { name: 'Sum', type: 'number'},
  { name: 'Average', type: 'number'},
  { name: 'Max', type: 'number'},
  { name: 'Min', type: 'number'},
];
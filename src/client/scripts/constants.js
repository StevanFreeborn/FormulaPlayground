import OperatorTypes from "./operatorTypes.js";
import FunctionTypes from "./functionTypes.js";

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
  { name: 'Add/Concatenate', symbol: '+', type: OperatorTypes.math},
  { name: 'Subtract', symbol: '-', type: OperatorTypes.math},
  { name: 'Multiply', symbol: '*', type: OperatorTypes.math},
  { name: 'Divide', symbol: '/', type: OperatorTypes.math},
  { name: 'Equal', symbol: '==', type: OperatorTypes.comparison},
  { name: 'Not Equal', symbol: '!=', type: OperatorTypes.comparison},
  { name: 'Less Than', symbol: '<', type: OperatorTypes.comparison},
  { name: 'Greater Than', symbol: '>', type: OperatorTypes.comparison},
  { name: 'Less Than or Equal', symbol: '<=', type: OperatorTypes.comparison},
  { name: 'Greater Than or Equal', symbol: '>=', type: OperatorTypes.comparison},
  { name: 'AND', symbol: '&&', type: OperatorTypes.logical},
  { name: 'OR', symbol: '||', type: OperatorTypes.logical}
];
export const CUSTOM_FUNCTIONS = [
  { name: 'Sum', type: FunctionTypes.number},
  { name: 'Average', type: FunctionTypes.number},
  { name: 'Max', type: FunctionTypes.number},
  { name: 'Min', type: FunctionTypes.number},
];
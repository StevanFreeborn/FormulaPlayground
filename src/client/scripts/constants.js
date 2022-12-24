import OperatorTypes from "./operatorTypes.js";
import FunctionTypes from "./functionTypes.js";

export const BASE_URL = process.env.NODE_ENV === 'development'
? 'https://localhost:44304'
: 'https://formulaplayground.azurewebsites.net/';

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
  { name: 'Sum', type: FunctionTypes.number, snippet: 'Sum(reference_or_array)'},
  { name: 'Average', type: FunctionTypes.number, snippet: 'Average(reference_or_array)'},
  { name: 'Max', type: FunctionTypes.number, snippet: 'Max(reference_or_array)'},
  { name: 'Min', type: FunctionTypes.number, snippet: 'Min(reference_or_array)'},
  { name: 'Round', type: FunctionTypes.number, snippet: 'Round(number, number_of_digits)'},
  { name: 'RoundUp', type: FunctionTypes.number, snippet: 'RoundUp(number, number_of_digits)'},
];
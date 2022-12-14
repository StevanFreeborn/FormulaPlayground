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
  { name: 'Trim', type: FunctionTypes.text, snippet: 'Trim(text)'},
  { name: 'Len', type: FunctionTypes.text, snippet: 'Len(text)'},
  { name: 'Join', type: FunctionTypes.text, snippet: 'Join([value1, value2, ...], "separator")'},
  { name: 'Left', type: FunctionTypes.text, snippet: 'Left(text, number_of_characters)'},
  { name: 'Right', type: FunctionTypes.text, snippet: 'Right(text, number_of_characters)'},
  { name: 'If', type: FunctionTypes.logical, snippet: 'if (logical_test) {\n  value_if_true\n} else {\n  value_if_false\n}' },
  { name: 'And', type: FunctionTypes.logical, snippet: 'And(logical1, logical2, ...)' },
  { name: 'CurrentMonth', type: FunctionTypes.date, snippet: 'CurrentMonth()' },
  { name: 'CurrentYear', type: FunctionTypes.date, snippet: 'CurrentYear()' },
  { name: 'Date', type: FunctionTypes.date, snippet: 'Date(year, month, day)' },
  { name: 'DateAdd', type: FunctionTypes.date, snippet: 'DateAdd(date, number, format)' },
  { name: 'DateAddSpan', type: FunctionTypes.date, snippet: 'DateAddSpan(date, Time Span Field)' },
  { name: 'DateDiff', type: FunctionTypes.date, snippet: 'DateDiff(end_date, start_date, format)' },
  { name: 'DateSubtractSpan', type: FunctionTypes.date, snippet: 'DateSubtractSpan(date, Time Span Field)' },
  { name: 'DaysSince', type: FunctionTypes.date, snippet: 'DaysSince(date)' },
  { name: 'DaysSinceIsGreaterThan', type: FunctionTypes.date, snippet: 'DaysSinceIsGreaterThan(date, number_of_days)' },
  { name: 'DaysUntil', type: FunctionTypes.date, snippet: 'DaysUntil(date)' },
  { name: 'DaysUntilIsGreaterThan', type: FunctionTypes.date, snippet: 'DaysUntilIsGreaterThan(date, number_of_days)' },
  { name: 'FullYearsSince', type: FunctionTypes.date, snippet: 'FullYearsSince(date)' },
  { name: 'GetDayOfMonth', type: FunctionTypes.date, snippet: 'GetDayOfMonth(date)' },
  { name: 'GetDayOfWeek', type: FunctionTypes.date, snippet: 'GetDayOfWeek(date)'},
  { name: 'GetDayOfYear', type: FunctionTypes.date, snippet: 'GetDayOfYear(date)'},
  { name: 'GetMonth', type: FunctionTypes.date, snippet: 'GetMonth(date)' },
  { name: 'GetWeekOfMonth', type: FunctionTypes.date, snippet: 'GetWeekOfMonth(date)' },
  { name: 'GetWeekOfYear', type: FunctionTypes.date, snippet: 'GetWeekOfYear(date)' },
  { name: 'GetYear', type: FunctionTypes.date, snippet: 'GetYear(date)' },
  { name: 'IsAfterToday', type: FunctionTypes.date, snippet: 'IsAfterToday(date)' },
  { name: 'IsBeforeToday', type: FunctionTypes.date, snippet: 'IsBeforeToday(date)' },
  { name: 'IsCurrentMonth', type: FunctionTypes.date, snippet: 'IsCurrentMonth(date)' },
  { name: 'IsCurrentWeek', type: FunctionTypes.date, snippet: 'IsCurrentWeek(date)' },
  { name: 'IsCurrentYear', type: FunctionTypes.date, snippet: 'IsCurrentYear(date)' },
  { name: 'IsOnOrAfterToday', type: FunctionTypes.date, snippet: 'IsOnOrAfterToday(date)' },
  { name: 'IsOnOrBeforeToday', type: FunctionTypes.date, snippet: 'IsOnOrBeforeToday(date)' },
  { name: 'IsToday', type: FunctionTypes.date, snippet: 'IsToday(date)' },
  { name: 'IsWithinNextDays', type: FunctionTypes.date, snippet: 'IsWithinNextDays(date, number_of_days)' },
  { name: 'IsWithinPriorDays', type: FunctionTypes.date, snippet: 'IsWithinPriorDays(date, number_of_days)' },
  { name: 'FormatDate', type: FunctionTypes.date, snippet: 'FormatDate(date, customFormat)'},
  { name: 'GetNextAnnualDate', type: FunctionTypes.date, snippet: 'GetNextAnnualDate(date) OR (month, day)' },
  { name: 'GetNextFutureDateByDays', type: FunctionTypes.date, snippet: 'GetNextFutureDateByDays(date, number_of_days)' },
  { name: 'GetNextFutureDateBySpan', type: FunctionTypes.date, snippet: 'GetNextFutureDateBySpan(date, Time Span Field)' },
  { name: 'GetNextMonthlyDate', type: FunctionTypes.date, snippet: 'GetNextMonthlyDate(date) OR (day)' },
  { name: 'Workdays', type: FunctionTypes.date, snippet: 'Workdays(start_date, end_date)' },
  { name: 'WorkdaysSince', type: FunctionTypes.date, snippet: 'WorkdaysSince(date)' },
  { name: 'WorkdaysUntil', type: FunctionTypes.date, snippet: 'WorkdaysUntil(date)' },
];
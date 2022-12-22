
using server.Models;

namespace server.Services;

public interface IFormulaService
{
  public FormulaRunResult RunFormula(string formula, FormulaContext formulaContext);
  public FormulaValidationResult ValidateFormula(string formula, FormulaContext formulaContext);
}

using server.Models;

namespace server.Services;

public interface IFormulaService
{
  public Task<FormulaRunResult> RunFormula(string formula, FormulaContext formulaContext);
  public Task<FormulaValidationResult> ValidateFormula(string formula, FormulaContext formulaContext);
}
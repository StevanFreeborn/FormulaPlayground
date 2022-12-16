
using server.Models;

namespace server.Services;

public interface IFormulaService
{
  public FormulaRunResult RunFormula(string formula);
  public FormulaValidationResult ValidateFormula(string formula);
}
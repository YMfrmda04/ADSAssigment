using System;
namespace ExpressionEvaluation
{
    public class VariableManager
    {
        private Dictionary<string, double> variables = new Dictionary<string, double>();

        public void SetVariable(string variable, double value)
        {
            try
            {
                variables[variable] = value;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public double GetVariableValue(string variable)
        {
            if (variables.ContainsKey(variable))
                return variables[variable];
            else
                throw new InvalidOperationException($"Variable {variable} not assigned.");
        }
    }
}


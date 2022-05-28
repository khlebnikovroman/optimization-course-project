using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;

using PropertyChanged;
using static OptimizatonMethods.Models.Parameters;

using Expression = org.matheval.Expression;


namespace OptimizatonMethods.Models
{
    public static class ParameterBuilder
    {
        public static Parameter BuildParameter(JsonElement json)
        {
            var p = new Parameter()
            {
                displayName = json.GetProperty("displayName").GetString(),
                symbol = json.GetProperty("symbol").GetString(),
                unit = json.GetProperty("unit").GetString(),
                Value = json.GetProperty("defaultValue").GetDouble(),
            };

            try
            {
                p.desired = json.GetProperty("desired").GetBoolean();
            }
            catch (Exception e)
            {
                p.desired = false;
            }
            return p;
        }
    }
    public static class RestrictionBuilder
    {
        public static Restriction BuildRestriction(JsonElement json, Parameters parameters)
        {
            var restriction = new Restriction()
            {
                expression = new Expression(json.GetProperty("func").GetString()),
                displayName = json.GetProperty("displayName").GetString(),
                value = json.GetProperty("value").GetDouble(),
                parameters = parameters
            };
            
            return restriction; //todo
        }
    }


    [AddINotifyPropertyChangedInterface]
    public class Parameter
    {
        public string displayName { get; init; }
        public string symbol { get; init; }
        public string unit { get; init; }
        public double Value { get; set; }
        public bool desired = false;
        public bool display {
            get
            {
                return !desired;
            }
        }

        public override string ToString()
        {
            return displayName + " " + symbol + ", " + unit;
        }
    }

    public class ParameterWithBounds
    {
        public Parameter parameter { get; init; }
        public double min { get; set; } = double.PositiveInfinity;
        public double max { get; set; } = double.NegativeInfinity;
    }
    public interface IRestriction
    {
        public bool GetValue();
        public void Bind();
        public double value { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class Restriction : IRestriction
    {
        public string displayName { get; set; }
        public Expression expression { get; set; }
        public Parameters parameters { get; set; }
        public double value { get; set; }
        public void Bind()
        {
            foreach (var variable in expression.getVariables())
            {
                if (variable == "value")
                {
                    expression.Bind("value", value);
                }
                else
                {
                    expression.Bind(variable, parameters[variable].Value);
                }
            }
        }
        public bool GetValue()
        {
            Bind();
            return expression.Eval<bool>();
        }

        public override string ToString()
        {
            return displayName;
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class Restrictions : IRestriction, IEnumerable<Restriction>
    {
        private List<Restriction> restrictions = new List<Restriction>();


        public void Add(Restriction restriction)
        {
            restrictions.Add(restriction);
        }
        public void Bind()
        {
            foreach (IRestriction restriction in restrictions)
            {
                restriction.Bind();
            }
        }

        public double value { get; set; }

        public bool GetValue()
        {
            foreach (IRestriction restriction in restrictions)
            {
                if (!restriction.GetValue())
                {
                    return false;
                }
            }
            return true;
        }
        
        public IEnumerator<Restriction> GetEnumerator()
        {
            return restrictions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return restrictions.GetEnumerator();
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class Parameters: IEnumerable<Parameter>
    {
        public List<Parameter> parameters = new List<Parameter>();
        public void Add(Parameter parameter)
        {
            parameters.Add(parameter);
        }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }

        public Parameter this[string symbol]
        {
            get { return parameters.Single(p => p.symbol == symbol); }
            set
            {
                var p = parameters.Find(p => p.symbol == symbol);
                p = value;
            }
        }


    }
    [AddINotifyPropertyChangedInterface]
    public class MathModelTest
    {
        public enum MinMax
        {
            Min,
            Max,
        }
        public IOptimMethod Method { get; set; }
        public string desiredParameterName { get; set; }
        public string Latex { get; set; }
        public string Task { get; set; }
        public MinMax minMax;
        public ParameterWithBounds p1
        {
            get
            {
                return desiredParameters[0];
            }
        }
        public ParameterWithBounds p2
        {
            get
            {
                return desiredParameters[1];
            }
        }
        public Parameters parameters { get; set; } = new Parameters();
        
        public Restrictions restrictions { get; set; } = new Restrictions();
        public Expression funcExpression;
        private ParameterWithBounds[] desiredParameters  = new ParameterWithBounds[2] ; //только два параметра можем вычислять
        public void rebind()
        {
            restrictions.Bind();
            var f = funcExpression.getVariables();
            foreach (var variable in funcExpression.getVariables())
            {
                funcExpression.Bind(variable, parameters[variable].Value);
            }
        }

        public MathModelTest(JsonElement json)
        {
            var strParameters = json.GetProperty("parameters").EnumerateArray(); //todo
            foreach (var strParameter in strParameters)
            {
                parameters.Add(ParameterBuilder.BuildParameter(strParameter));
            }


            var strRestrictions = json.GetProperty("restrictions").EnumerateArray(); //todo
            foreach (var strRestriction in strRestrictions)
            {
                restrictions.Add(RestrictionBuilder.BuildRestriction(strRestriction, parameters));
            }

            //desiredParameters = parameters.Where(p => p.desired).ToArray();
            funcExpression = new Expression(json.GetProperty("function").GetString());
            desiredParameterName = json.GetProperty("desiredParameterName").GetString();
            Latex = json.GetProperty("functionLatex").GetString();
            Task = json.GetProperty("task").GetString();

            var mm = json.GetProperty("minmax").GetString();

            if (mm=="min")
            {
                minMax = MinMax.Min;
            }
            if (mm == "max")
            {
                minMax = MinMax.Max;
            }
            buildParametersWithBound();
        }

        public void buildParametersWithBound()
        {
            var desired = parameters.Where(p => p.desired);
            var bounds = new Dictionary<string, List<double>>();
            var parametersWithBounds = new List<ParameterWithBounds>();

            foreach (var parameter in desired)
            {
                parametersWithBounds.Add(new ParameterWithBounds(){parameter = parameter});
            }
            foreach (var res in restrictions)
            {
                var values = res.expression.getVariables();
                values.Remove("value");

                if (values.Count == 1) //  скорее всего это параметр с ограничением (проверять мы это не будем, а надо)
                {
                    var resSym = values[0];

                    if (!bounds.ContainsKey(resSym))
                    {
                        bounds[resSym] = new List<double>();
                    }
                    bounds[resSym].Add(res.value);
                }
            }

            foreach (var bound in bounds)
            {
                var param = parametersWithBounds.Single(p => p.parameter.symbol == bound.Key);
                param.min = bound.Value.Min();
                param.max = bound.Value.Max();
            }

            desiredParameters = parametersWithBounds.ToArray();
        }

        public double Function()
        {
            rebind();
            return funcExpression.Eval<double>();
        }

        public double Function(double x1,double x2)
        {
            rebind();
            var sym = p1.parameter.symbol;
            funcExpression.Bind(sym, x1);
            sym = p2.parameter.symbol;
            funcExpression.Bind(sym, x2);
            return funcExpression.Eval<double>();
        }

        public bool Conditions(double x1, double x2)
        {
            rebind();
            foreach ( var res in restrictions)
            {
                foreach (var v in res.expression.getVariables())
                {
                    if (v == p1.parameter.symbol)
                    {
                        res.expression.Bind(v, x1);
                    }
                    else if (v == p2.parameter.symbol)
                    {
                        res.expression.Bind(v, x2);
                    }
                }
            }
            foreach ( var res in restrictions)
            {
                if (!res.expression.Eval<bool>())
                {
                    return false;
                }
            }
            return true;
        }
        public Point3D Calculate()
        {
            if (minMax == MinMax.Min)
            {
                return Method.FindOptimalParametersMin();
            }
            else
            {
                return Method.FindOptimalParametersMax();
            }
        }
        
    }
}

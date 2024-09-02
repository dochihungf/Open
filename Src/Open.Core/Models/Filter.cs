namespace Open.Core.Models;

// public class Filter
// {
//     public string Field { get; set; }
//     public string Operator { get; set; }
//     public string? Value { get; set; }
//     public string? Logic { get; set; }
//     public IEnumerable<Filter>? Filters { get; set; }
//
//     public Filter()
//     {
//         Field = string.Empty;
//         Operator = string.Empty;
//     }
//
//     public Filter(string field, string @operator)
//     {
//         Field = field;
//         Operator = @operator;
//     }
// }

public class Filter
{
    public List<Field> Fields { get; set; }

    private string _formula { get; set; }

    public string Formula
    {
        get
        {
            if (string.IsNullOrEmpty(_formula))
            {
                BuildFormula();
            }
            return RemoveExtraWhitespace(_formula);
        }
        set
        {
            _formula = value;
        }
    }

    private void BuildFormula()
    {
        if (Fields != null && Fields.Any())
        {
            var indexs = new List<string>();
            for (int i = 0; i < Fields.Count; i++)
            {
                indexs.Add("{" + i + "}");
            }
            _formula = string.Join(" AND ", indexs.ToArray());
        }
    }
    
    private string RemoveExtraWhitespace(string statement)
    {
        if (string.IsNullOrEmpty(statement))
        {
            return "";
        }

        statement = statement.Trim();

        var currentLength = statement.Length;
        while (true)
        {
            statement = statement.Replace("  ", " ");
            if (currentLength == statement.Length)
            {
                return statement;
            }
            currentLength = statement.Length;
        }
    }
}




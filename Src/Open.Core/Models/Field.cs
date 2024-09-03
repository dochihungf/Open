using Microsoft.IdentityModel.Tokens;
using Open.Constants;

namespace Open.Core.Models;

public class Field
{
    private string _fieldName { get; set; }

    public string FieldName
    {
        get
        {
            if (!string.IsNullOrEmpty(_fieldName))
            {
                if (_fieldName[0] >= 97 && _fieldName[0] <= 122)
                {
                    _fieldName = char.ToUpper(_fieldName[0]) + _fieldName.Substring(1);
                }
            }

            return _fieldName;
        }
        set
        {
            _fieldName = value;
        }
    }

    private string _value { get; set; }

    public string Value
    {
        get
        {
            if (_value == null)
            {
                return string.Empty;
            }

            return _value;
        }
        set
        {
            _value = value;
        }
    }

    public WhereType Condition { get; set; } = WhereType.E;
}

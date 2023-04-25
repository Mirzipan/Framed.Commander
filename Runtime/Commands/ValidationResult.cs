using System.Collections.Generic;

namespace Mirzipan.Heist.Commands
{
    public struct ValidationResult : IEqualityComparer<ValidationResult>
    {
        public uint Code;

        public bool Success => Code == 0;

        public ValidationResult(uint code)
        {
            Code = code;
        }

        public static readonly ValidationResult Pass = new ValidationResult(0);

        #region Equality

        public bool Equals(ValidationResult x, ValidationResult y)
        {
            return x.Code == y.Code;
        }

        public int GetHashCode(ValidationResult obj)
        {
            return (int)obj.Code;
        }

        public static bool operator ==(ValidationResult lhs, ValidationResult rhs)
        {
            return lhs.Code == rhs.Code;
        }

        public static bool operator !=(ValidationResult lhs, ValidationResult rhs)
        {
            return lhs.Code != rhs.Code;
        }
        
        #endregion Equality
    }
}
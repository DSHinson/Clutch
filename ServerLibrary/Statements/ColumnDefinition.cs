using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Statements
{
    public class ColumnDefinition
    {
        public string Name { get; }
        public string DataType { get; }
        public List<string> Constraints { get; }

        public ColumnDefinition(string name, string dataType, List<string> constraints = null)
        {
            Name = name;
            DataType = dataType;
            Constraints = constraints ?? new List<string>();
        }

        public override string ToString()
        {
            var constraints = Constraints.Count > 0 ? " " + string.Join(" ", Constraints) : "";
            return $"{Name} {DataType}{constraints}";
        }
    }
}

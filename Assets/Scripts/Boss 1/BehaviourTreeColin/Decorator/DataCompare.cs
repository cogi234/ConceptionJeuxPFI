using System;
using System.Collections.Generic;

namespace BehaviourTreeColin
{
    public class DataCompare<T> : DecoratorNode where T : IComparable<T>
    {
        public enum ComparisonType { Equal, Greater, Lesser }
        
        ComparisonType comparisonType;
        bool invert;
        string dataKey;
        T value;

        public DataCompare(Node child, string dataKey, T value, ComparisonType comparisonType, bool invert = false) : base(child) {
            this.dataKey = dataKey; 
            this.value = value;
            this.comparisonType = comparisonType;
            this.invert = invert;
        }

        public override NodeState Evaluate(Dictionary<string, object> data)
        {
            object dataFound = data[dataKey];
            T dataConverted;

            if (dataFound == null)
                return NodeState.Failure;

            if (dataFound is T)
                dataConverted = (T)dataFound;
            else {
                try
                {
                    dataConverted = (T)Convert.ChangeType(dataFound, typeof(T));
                }
                catch
                {
                    return NodeState.Failure;
                }
            }

            bool result = false;
            switch (comparisonType)
            {
                case ComparisonType.Equal:
                    result = dataConverted.CompareTo(value) == 0;
                    break;
                case ComparisonType.Greater:
                    result = dataConverted.CompareTo(value) > 0;
                    break;
                case ComparisonType.Lesser:
                    result = dataConverted.CompareTo(value) < 0;
                    break;
            }

            if (invert)
                result = !result;

            if (!result)
                return NodeState.Failure;

            return children[0].Evaluate(data);
        }

        public override object Clone()
        {
            return new DataCompare<T>((Node)children[0].Clone(), dataKey, value, comparisonType, invert);
        }
    }
}

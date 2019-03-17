using System;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace CoreBlog.GraphQL
{
    public class GuidValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new GuidValue((Guid)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is Guid;
        }
    }
}

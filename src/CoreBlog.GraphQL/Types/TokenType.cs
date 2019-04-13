using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace CoreBlog.GraphQL.Types {
    public class TokenType : ObjectGraphType<string> {
        public TokenType() {
            Field("token", s => s, false);
        }
    }
}

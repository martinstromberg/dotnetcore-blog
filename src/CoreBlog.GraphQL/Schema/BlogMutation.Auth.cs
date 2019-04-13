using System;
using GraphQL;
using GraphQL.Types;
using JWT.Algorithms;
using JWT.Builder;

namespace CoreBlog.GraphQL.Schema {
    using Types;

    public partial class BlogMutation {
        public void SetupAuthenticationFields() {
            Field<TokenType>(
                name: "tokenAuth",
                description: "Authenticates a user with an email address and a password",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> {
                        Name = "emailAddress",
                    },
                    new QueryArgument<NonNullGraphType<StringGraphType>> {
                        Name = "password"
                    }
                ),
                resolve: ResolveTokenAuth
            );
        }

        public object ResolveTokenAuth(ResolveFieldContext<object> context) {
            var emailAddress = context.GetArgument<string>("emailAddress");
            if (string.IsNullOrWhiteSpace(emailAddress)) {
                throw new ExecutionError("Argument 'emailAddress' cannot be null or whitespace");
            }

            var password = context.GetArgument<string>("password");
            if (string.IsNullOrWhiteSpace(password)) {
                throw new ExecutionError("Argument 'password' cannot be null or whitespace");
            }

            var validation = _userService.ValidateCredentials(emailAddress, password);
            validation.Wait();

            if (!validation.IsCompletedSuccessfully) {
                throw new ExecutionError("An error occurred");
            }

            var user = validation.Result;
            if (user == null) {
                throw new ExecutionError("Email address or password was invalid");
            }

            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_configuration["Authentication:JwtSecret"])
                .AddClaim(
                    ClaimName.ExpirationTime,
                    DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString())
                .AddClaim(
                    "userId",
                    user.UserId
                )
                .AddClaim(
                    "displayName",
                    user.DisplayName
                )
                .Build();
        }
    }
}

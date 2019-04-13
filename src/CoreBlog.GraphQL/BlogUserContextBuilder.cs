using System;
using System.Linq;
using System.Threading.Tasks;
using CoreBlog.GrainModels.Users;
using GraphQL.Server.Transports.AspNetCore;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CoreBlog.GraphQL {
    public class BlogUserContextBuilder : IUserContextBuilder {
        private readonly IConfiguration _configuration;

        public BlogUserContextBuilder(IConfiguration configuration) {
            _configuration = configuration;
        }

        public Task<object> BuildUserContext(HttpContext httpContext) {
            var headers = httpContext?.Request?.Headers;
            if (headers == null) {
                return Task.FromResult<object>(new BlogUserContext(false));
            }
            
            if (!headers.TryGetValue("Authorization", out var authorizationHeader)) {
                return Task.FromResult<object>(new BlogUserContext(false));
            }

            var jwtToken = authorizationHeader
                .Where(s => s.StartsWith("Bearer "))
                .Select(s => s.Substring(7))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(jwtToken)) {
                return Task.FromResult<object>(new BlogUserContext(false));
            }

            User user;
            try {
                user = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_configuration["Authentication:JwtSecret"])
                    .MustVerifySignature()
                    .Decode<User>(jwtToken);
            } catch (TokenExpiredException) {
                user = null;
            } catch (SignatureVerificationException) {
                user = null;
            }

            return Task.FromResult<object>(
                new BlogUserContext(user != null, user)
            );
        }
    }
}

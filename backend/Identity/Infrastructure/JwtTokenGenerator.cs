
namespace Identity.Infrastructure
{
    public abstract class JwtTokenGenerator
    {
        protected readonly JwtProvider _jwtProvider;
        protected JwtTokenGenerator(JwtProvider jwtProvider) 
        {
            _jwtProvider = jwtProvider;
        }

    }
}

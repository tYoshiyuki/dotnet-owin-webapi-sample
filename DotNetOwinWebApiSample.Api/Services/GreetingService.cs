using DotNetOwinWebApiSample.Api.Exceptions;

namespace DotNetOwinWebApiSample.Api.Services
{
    public class GreetingService
    {
        public string Greeting()
        {
            return "Hello";
        }

        public string Greeting(int hour)
        {
            if (hour >= 24) throw new ApiBadRequestException("hourは 0～23 で指定してください。");

            if (hour < 5) return "Good evening";
            if (hour < 12) return "Good morning";
            if (hour < 18) return "Good afternoon";
            return "Good evening";
        }
    }
}
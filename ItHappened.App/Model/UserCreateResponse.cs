namespace ItHappened.App.Model
{
    public class UserCreateResponse
    {
        public string AccessToken { get; set; }

        public UserCreateResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
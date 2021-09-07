namespace GitHubBlazor.Pages
{
    public interface State
    {
        public sealed record Loading : State;

        public sealed record Error(string Message) : State;

        public sealed record Success<T>(T Data) : State;
    }
}
namespace Cadastro.Modelos.Erro
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }

        protected Result(bool success, T? value, string? error)
        {
            IsSuccess = success;
            Value = value;
            ErrorMessage = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);

        public static Result<T> Failure(string error) => new(false, default, error);
    }
}
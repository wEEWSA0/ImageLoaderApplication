namespace ImageLoaderApplication.Dto;

public class RepositoryResult<T>
{
    public T? Data { get; set; }
    public int StatusCode { get; set; }
}

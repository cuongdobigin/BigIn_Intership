namespace MyApp.Application.Dto.Response;




public class PageResponse<T>
{
    public List<T> Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext => Page < TotalPages;
    public bool HasPrevious => Page > 1;
}
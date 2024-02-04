namespace API.Helpers;
public class RequestsParams : PaginationParams
{
    public int UserId { get; set; }
    public string Predicate { get; set; }
}

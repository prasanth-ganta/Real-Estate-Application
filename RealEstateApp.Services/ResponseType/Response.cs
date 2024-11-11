namespace RealEstateApp.Services.ResponseType;

public class Response
{
    public int StatusCode { get; set; }
    public object Value { get; set; }
    public Response(int statusCode, object? value){
        StatusCode=statusCode;
        Value=value;
    }
}

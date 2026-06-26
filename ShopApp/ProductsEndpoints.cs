namespace ShopApp
{
    public static class ProductEndpoints
    {
        public static IResult GetProduct()
        {
            return Results.Ok("Success da");
        }

        public static IEndpointRouteBuilder ProductsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", GetProduct);
            return app;
        }
    }
}

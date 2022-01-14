namespace Sample.FunctionalTests.Models;

public static class Routes
{
  private const string BaseRoute = "api";

  public static class Authors
  {
    private const string BaseRoute = Routes.BaseRoute + "/authors";

    public const string Create = BaseRoute;

    public const string Update = BaseRoute;

    public static string List() => BaseRoute;

    public static string List(int perPage, int page) => $"{BaseRoute}?perPage={perPage}&page={page}";

    public static string Get(int id) => $"{BaseRoute}/{id}";

    public static string Delete(int id) => $"{BaseRoute}/{id}";
  }
}

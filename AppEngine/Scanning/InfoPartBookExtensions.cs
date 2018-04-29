namespace Mt.MediaMan.AppEngine.Scanning
{
  public static class InfoPartBookExtensions
  {
    public static string GetAuthorsString(this InfoPartBook infoPart)
    {
      var authors = string.Join(", ", infoPart.Authors);
      return authors;
    }
  }
}

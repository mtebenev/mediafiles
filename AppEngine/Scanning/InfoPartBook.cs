namespace Mt.MediaMan.AppEngine.Scanning
{
  public class InfoPartBook : InfoPartBase
  {
    public const string IndexField_Title = "Book.Title";
    public const string IndexField_Authors = "Book.Authors";

    public string Title { get; set; }
    public string[] Authors { get; set; }
    public string Isbn { get; set; }
  }
}

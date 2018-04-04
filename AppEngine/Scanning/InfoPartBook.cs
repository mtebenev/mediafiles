namespace Mt.MediaMan.AppEngine.Scanning
{
  public class InfoPartBook : IInfoPart
  {
    public InfoPartBook()
    {
      InfoPartId = "Book";
    }

    public string InfoPartId { get; set; }
    public int CatalogItemId { get; set; }

    public string Title { get; set; }
    public string[] Authors { get; set; }
    public string Isbn { get; set; }
  }
}

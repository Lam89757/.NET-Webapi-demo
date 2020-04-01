namespace WebapiStudy.Models
{
  public class BookCategory
  {
    public BookCategory(string type,bool isChecked)
    {
      this.type = type;
      this.isChecked = isChecked;
    }
    public string type { get; set; }
    public bool isChecked { get; set; }
  }
}
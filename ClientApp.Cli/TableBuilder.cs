using System;
using System.Collections.Generic;
using System.Text;

namespace Mt.MediaFiles.ClientApp.Cli
{
  /// <summary>
  /// Note: from https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
  /// Use https://github.com/Athari/CsConsoleFormat if it's not enough
  /// </summary>
  internal interface ITextRow
  {
    string Output();
    void Output(StringBuilder sb);
    object Tag { get; set; }
  }

  internal class TableBuilder : IEnumerable<ITextRow>
  {
    protected class TextRow : List<string>, ITextRow
    {
      protected TableBuilder Owner = null;

      public TextRow(TableBuilder owner)
      {
        this.Owner = owner;
        if(this.Owner == null)
          throw new ArgumentException("Owner");
      }

      public string Output()
      {
        var sb = new StringBuilder();
        Output(sb);
        return sb.ToString();
      }

      public void Output(StringBuilder sb)
      {
        sb.AppendFormat(Owner.FormatString, this.ToArray());
      }

      public object Tag { get; set; }
    }

    public string Separator { get; set; }

    protected List<ITextRow> Rows = new List<ITextRow>();
    protected List<int> ColLength = new List<int>();

    public TableBuilder()
    {
      Separator = "  ";
    }

    public TableBuilder(string separator)
      : this()
    {
      Separator = separator;
    }

    public ITextRow AddRow(params object[] cols)
    {
      var row = new TextRow(this);
      foreach(var o in cols)
      {
        var str = o.ToString().Trim();
        row.Add(str);
        if(ColLength.Count >= row.Count)
        {
          var curLength = ColLength[row.Count - 1];
          if(str.Length > curLength)
            ColLength[row.Count - 1] = str.Length;
        }
        else
        {
          ColLength.Add(str.Length);
        }
      }

      Rows.Add(row);
      return row;
    }

    protected string _fmtString = null;

    public string FormatString
    {
      get
      {
        if(_fmtString == null)
        {
          var format = "";
          var i = 0;
          foreach(var len in ColLength)
          {
            format += string.Format("{{{0},-{1}}}{2}", i++, len, Separator);
          }

          format += "\r\n";
          _fmtString = format;
        }

        return _fmtString;
      }
    }

    public string Output()
    {
      var sb = new StringBuilder();
      foreach(TextRow row in Rows)
      {
        row.Output(sb);
      }

      return sb.ToString();
    }

    #region IEnumerable Members

    public IEnumerator<ITextRow> GetEnumerator()
    {
      return Rows.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return Rows.GetEnumerator();
    }

    #endregion
  }
}

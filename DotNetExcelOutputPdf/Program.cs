// 実行プログラムの場所にある Excel ファイル
var folderPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
var excelFilePath = Path.Combine(folderPath, "Sample.xlsx");

dynamic? excel = null;
dynamic? books = null;
dynamic? book = null;
dynamic? sheets = null;
try
{
  // Excel.Application の Type を取得
  var type = Type.GetTypeFromProgID("Excel.Application");
  if (type == null)
  {
    Console.WriteLine("Excel がインストールされていません。");
    return;
  }

  // Excel アプリケーションを生成(起動)します
  excel = Activator.CreateInstance(type);
  if (excel == null)
  {
    Console.WriteLine("Excel.Application のインスタンスの生成に失敗しました。");
    return;
  }
  excel.DisplayAlerts = false; // アラート非表示
  excel.Visible = false;       // Excel 非表示

  // ワークブック一覧を参照
  books = excel.Workbooks;

  // Excel ファイルを開く
  book = books.Open(excelFilePath);

  // シート一覧参照
  sheets = book.Worksheets;

  // 全シートを選択
  sheets.Select();

  // Excel のデータを PDF ファイルとして保存
  //   Type     : [XlFixedFormatType] xlTypePDF=PDF(0), xlTypeXPS=XPS(1)
  //   Filename : 出力ファイル名
  //   Quality  : [XlFixedFormatQuality] xlQualityStandard=標準品質(0), xlqualityminimum=最小限の品質(1)
  book.ExportAsFixedFormat(Type: 0, Filename: Path.Combine(folderPath, "Sample.pdf"), Quality: 0);

  Console.WriteLine("PDF 出力に成功しました。");
}
catch (Exception ex)
{
  Console.WriteLine("PDF 出力に失敗しました。");
  Console.WriteLine(ex.ToString());
}
finally
{
  // Excel を終了させる
  if (book != null) book.Close(SaveChanges: false);
  if (excel != null) excel.Quit();

  // 使用した Excel リソースは全て開放する
  if (sheets != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
  if (book != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
  if (books != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
  if (excel != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
}

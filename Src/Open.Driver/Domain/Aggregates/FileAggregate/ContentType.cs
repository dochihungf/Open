using Open.Core.SeedWork;

namespace Open.Driver.Domain.Aggregates;

public enum ContentType
{
    Unknown = 0,
    Text = 1,
    Image = 2,
    Video = 3,
    Audio = 4,
    Document = 5,
    Spreadsheet = 6,
    Presentation = 7,
    Archive = 8, // Ví dụ: .zip, .rar
    Application = 9 // Ví dụ: .exe, .msi
}

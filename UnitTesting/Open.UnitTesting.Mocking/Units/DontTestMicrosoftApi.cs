namespace Open.UnitTesting.Mocking.Units;

public class DontTestMicrosoftApi(IFiles files)
{
    public Task SaveFile(string path, Stream file)
    {
        // more work
        var fileStream = files.OpenWriteStreamTo(path);
        // more work
        return file.CopyToAsync(fileStream);
    }
}

public interface IFiles
{
    Stream OpenWriteStreamTo(string path);
}
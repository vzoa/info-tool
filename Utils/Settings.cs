using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZoaInfoTool.Utils;


public interface ISettingsService
{
    public Dictionary<string, string> Values { get; set; }
    public Task LoadFromFileAsync();
    public void WriteToFileAsync();
}

public class SettingsService : ISettingsService
{
    private readonly string _filepath;
    public Dictionary<string, string> Values { get; set; }

    public SettingsService()
    {
        _filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ZoaInfoTool", "settings.json");
        Values = new Dictionary<string, string>();
    }

    public async Task LoadFromFileAsync()
    {
        if (!File.Exists(_filepath)) return;

        using FileStream openStream = File.OpenRead(_filepath);
        Values = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(openStream);
    }

    public async void WriteToFileAsync()
    {
        var parent = Directory.GetParent(_filepath);
        if (!parent.Exists) parent.Create();

        using FileStream createStream = File.Open(_filepath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(createStream, Values);
        await createStream.DisposeAsync();
    }
}

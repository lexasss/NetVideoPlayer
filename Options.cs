﻿using System.Text.Json;

namespace NetVideoPlayer;

class Options
{
    public int Port { get; set; } = 4900;
    public string VLCPlayerPath { get; set; } = @"C:\Program Files\VideoLAN\VLC\vlc.exe";
    public string VLCOptions { get; set; } = "--fullscreen --no-video-title-show --play-and-exit";
    public string VideoFolder { get; set; } = @".\videos";
    public Dictionary<string, int> Screens { get; set; } = new()
    {
        { "mirror-rear", 1 },
        { "mirror-left", 2 },
        { "mirror-right", 3 },
    };

    public static Options Load()
    {
        if (File.Exists(FILENAME))
        {
            try
            {
                using var optionsStream = new StreamReader(FILENAME);
                var optionsString = optionsStream.ReadToEnd();
                return JsonSerializer.Deserialize<Options>(optionsString) ?? new Options();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return new Options();
    }

    public void Save()
    {
        using var optionsStream = new StreamWriter(FILENAME);
        var optionsString = JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        optionsStream.Write(optionsString);
    }

    // Internal

    static readonly string FILENAME = "options.json";
}

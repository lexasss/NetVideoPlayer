using System.Diagnostics;

namespace NetVideoPlayer;

class Player
{
    public void PlayFromFolder(string folder)
    {
        var videos = _options.Screens.Keys;
        foreach (var video in videos)
        {
            var proc = CallPlayer(folder, video);
            proc.Exited += (s, e) =>
            {
                _processes.Remove(proc);
            };

            _processes.Add(proc);
        }
    }

    // Internal

    readonly Options _options = Options.Load();

    readonly List<Process> _processes = new();

    private Process CallPlayer(string folder, string screenName)
    {
        var vlcOptions = _options.VLCOptions;
        if (_options.Screens.TryGetValue(screenName, out int screenID))
        {
            vlcOptions += $" --qt-fullscreen-screennumber={screenID}";
        }

        var videoFile = Path.Combine(_options.VideoFolder, folder, $"{screenName}.mp4");
        var cmdCommand = $"\"{_options.VLCPlayerPath}\" {vlcOptions} {videoFile}";
        Console.WriteLine($"Playing {cmdCommand}");

        Process proc = new Process();
        proc.StartInfo.FileName = "CMD.exe";
        proc.StartInfo.Arguments = $"/c {cmdCommand}";
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        proc.Start();

        return proc;
    }
}

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetVideoPlayer;

public class App
{
    public static void Main(string[] args)
    {
        var options = Options.Load();

        var listener = new UdpClient(options.Port);
        var endPoint = new IPEndPoint(IPAddress.Any, options.Port);

        try
        {
            while (true)
            {
                Console.WriteLine("\nWaiting for command");
                byte[] bytes = listener.Receive(ref endPoint);
                var request = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                Console.WriteLine($"{endPoint}: {request}");

                if (!HandleRequest(request))
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
            options.Save();
        }
    }

    public static bool HandleRequest(string request)
    {
        if (request == "exit")
            return false;
        else
            _player.PlayFromFolder(request);

        return true;
    }

    // Internal

    readonly static Player _player = new();
}

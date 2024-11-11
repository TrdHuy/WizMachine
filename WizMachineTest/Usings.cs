global using NUnit.Framework;
using System.Collections.Concurrent;
using System.Reflection;
using WizMachine;

public static class FileLockManager
{
    public class TestAsset
    {
        // Constructor nhận đường dẫn asset
        public TestAsset(string assetPath)
        {
            AssetPath = assetPath;
        }

        // Thuộc tính đường dẫn asset
        public string AssetPath { get; }

        // Các thuộc tính tĩnh đại diện cho từng asset path
        public static readonly TestAsset SprFile = new TestAsset("Resources\\test.spr");
        public static readonly TestAsset BinFile = new TestAsset("Resources\\test.bin");
        public static readonly TestAsset PngFile = new TestAsset("Resources\\test.png");
        public static readonly TestAsset _12345SprFile = new TestAsset("Resources\\12345.spr");
        public static readonly TestAsset AlphaFile = new TestAsset("Resources\\alpha.spr");
        public static readonly TestAsset BinFile1 = new TestAsset("Resources\\1.bin");
        public static readonly TestAsset BinFile319x319 = new TestAsset("Resources\\1_319x319.bin");
        public static readonly TestAsset BinFile2 = new TestAsset("Resources\\2.bin");
        public static readonly TestAsset BinFile3 = new TestAsset("Resources\\3.bin");
        public static readonly TestAsset BinFile4 = new TestAsset("Resources\\4.bin");
        public static readonly TestAsset BinFile5 = new TestAsset("Resources\\5.bin");
        public static readonly TestAsset TestTxtPak = new TestAsset("Resources\\testTxtFile.pak");
        public static readonly TestAsset TestTxtPakTxt = new TestAsset("Resources\\testTxtFile.pak.txt");
        public static readonly TestAsset DataWithSprPak = new TestAsset("data.pak");
        public static readonly TestAsset DataForCompressPak = new TestAsset("Resources\\dataForCompressTest.pak");
        public static readonly TestAsset OutputDaPakTxt = new TestAsset("data.pak.txt");
        public static readonly TestAsset InputDaPak = new TestAsset("Resources\\dataWithSpr.pak");
        public static readonly TestAsset InputDaPakTxt = new TestAsset("Resources\\dataWithSpr.pak.txt");
        public static readonly TestAsset InputDataFolderForCompress = new TestAsset("Resources\\dataForCompressTest\\data");
        public static readonly TestAsset i127_TestFile = new TestAsset("Resources\\i127_testfile.spr");
    }

    // ConcurrentDictionary để quản lý lock của từng file path
    private static ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new ConcurrentDictionary<string, SemaphoreSlim>();

    // Hàm acquire nhiều file cùng lúc
    public static void AcquireFiles(params TestAsset[] assets)
    {
        // Sắp xếp file theo tên để tránh deadlock
        var sortedAssets = assets.OrderBy(a => a.AssetPath).ToArray();

        // Tạo danh sách semaphore cho từng file path
        var semaphores = sortedAssets.Select(asset =>
            _fileLocks.GetOrAdd(asset.AssetPath, new SemaphoreSlim(1, 1))
        ).ToArray();

        // Thử acquire toàn bộ semaphore, nếu không thành công thì release toàn bộ và thử lại
        while (true)
        {
            bool acquiredAll = true;

            foreach (var semaphore in semaphores)
            {
                if (!semaphore.Wait(0)) // Thử acquire mà không block
                {
                    acquiredAll = false;
                    break;
                }
            }

            if (acquiredAll)
            {
                // Nếu acquire thành công tất cả, thoát khỏi vòng lặp
                break;
            }
            else
            {
                // Nếu không acquire thành công tất cả, release những cái đã acquire
                foreach (var semaphore in semaphores)
                {
                    if (semaphore.CurrentCount == 0)
                    {
                        semaphore.Release();
                    }
                }

                // Chờ một thời gian ngắn trước khi thử lại để tránh busy-waiting
                Thread.Sleep(10);
            }
        }
    }

    // Hàm release nhiều file cùng lúc
    public static void ReleaseFiles(params TestAsset[] assets)
    {
        var sortedAssets = assets.OrderBy(a => a.AssetPath).ToArray();

        foreach (var asset in sortedAssets)
        {
            if (_fileLocks.TryGetValue(asset.AssetPath, out var fileLock))
            {
                fileLock.Release();
            }
        }
    }
}


[SetUpFixture]
public class GlobalTestSetup
{
    private FileStream globalLogFS;
    private StreamWriter globalLogSW;

    [OneTimeSetUp]
    public void GlobalOneTimeSetUp()
    {
        // Khởi tạo log toàn cục
        var dateTimeNow = DateTime.Now.ToString("ddMMyyHHmmss");
        var logFileName =
            Assembly.GetCallingAssembly().GetName().Name + "_" +
            Assembly.GetCallingAssembly().GetName().Version + "_" +
            dateTimeNow + ".txt";

        var logFolder = "temp\\logs\\GlobalLogs";
        if (!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }

        var filePath = Path.Combine(logFolder, logFileName);
        globalLogFS = new FileStream(filePath, FileMode.Append, FileAccess.Write);
        globalLogSW = new StreamWriter(globalLogFS);
        EngineKeeper.Init(globalLogSW); // Truyền stream global cho EngineKeeper hoặc các component khác
    }

    [OneTimeTearDown]
    public void GlobalOneTimeTearDown()
    {
        // Đóng log toàn cục
        globalLogSW.Close();
        globalLogSW.Dispose();
        globalLogFS.Close();
        globalLogFS.Dispose();
    }
}
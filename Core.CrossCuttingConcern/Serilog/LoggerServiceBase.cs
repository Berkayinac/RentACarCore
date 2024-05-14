using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Serilog;

// Log -> serilog, dosya, veritabanı gibi yerlere loglama yapılabilir.
// Sistemde birisi bir aksiyonu çağırdığı zaman, ne zaman çağırdı, kim çağırdı, hangi parametreler ile hangi metodu çağırdı gibi bir loglama yapılabilir.
// Loglama bizim iş ihtiyaçlarımıza göre içeriği değiştirilebilir.

// Çağrılacak metottaki parametreleri tuttuğumuz class

// serilog.sinks -> nuget paketlere yazdığımızda orada MSSQL'den tut AWS, Email log'a kadar herşey mevcut oradan gereken loglama yapılarını sisteme ekleyebilirsin.
public abstract class LoggerServiceBase
{
    protected ILogger Logger { get; set; } // verdiğimiz logger'ın implementasyonunu gerçekleştiricek.

    protected LoggerServiceBase()
    {
        Logger = null;
    }

    protected LoggerServiceBase(ILogger logger)
    {
        Logger = logger;
    }

    // Loglama türleri -> (verbose) detaylı loglama, fatal(ölümcül hatalı loglama), information, warning, debug, error

    public void Verbose(string message) => Logger.Verbose(message);

    public void Fatal(string message) => Logger.Fatal(message);

    public void Info(string message) => Logger.Information(message);

    public void Warn(string message) => Logger.Warning(message);

    public void Debug(string message) => Logger.Debug(message);

    public void Error(string message) => Logger.Error(message);
}
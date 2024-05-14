using Core.CrossCuttingConcern.Serilog.ConfigurationModels;
using Core.CrossCuttingConcern.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Serilog.Logger;

//https://github.com/serilog-mssql/serilog-sinks-mssqlserver

public class MsSqlLogger : LoggerServiceBase
{
    private readonly IConfiguration _configuration;
    public MsSqlLogger(IConfiguration configuration)
    {
        _configuration = configuration;

        MsSqlConfiguration logConfiguration =
           configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration").Get<MsSqlConfiguration>()
           ?? throw new Exception(SerilogMessages.NullOptionsMessage);

        // SinkOptions -> MsSql'e Loglama işlemini yapabilmek için bu SinkOptions'a ihtiyacımız var.
        MSSqlServerSinkOptions sinkOptions = new()
        {
             TableName = logConfiguration.TableName,
             AutoCreateSqlTable = logConfiguration.AutoCreateSqlTable,
        };

        // Veritabanındaki column'ların ayarlarını yapmak için kullanılır
        // örnek olarak MsSql'de column'ların baş harfleri büyük olur ancak PostgreSQL'de küçük olur vs vs name convention'ları yapmak için bu options kullanılır.
        ColumnOptions columnOptions = new();

        global::Serilog.Core.Logger seriLogConfig = new LoggerConfiguration().WriteTo.MSSqlServer(logConfiguration.ConnectionString, sinkOptions, columnOptions: columnOptions)
            .CreateLogger();

        Logger = seriLogConfig;
    }

}

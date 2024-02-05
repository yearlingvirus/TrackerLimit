// See https://aka.ms/new-console-template for more information
using YamlDotNet.Serialization;
using TrackerLimit.Entitys;
using TrackerLimit.Serialization;
using TrackerLimit.Utils;

try
{
    var yamlFilePath = "app.yaml";
    var yamlContent = File.ReadAllText(yamlFilePath);

    var yamlDeserializer = new DeserializerBuilder()
        //.WithNamingConvention(CamelCaseNamingConvention.Instance) // 使用camelCase命名约定
        .WithTypeConverter(new TorrentClientConverter()) // 注册自定义转换器
        .Build();


    var appConfig = yamlDeserializer.Deserialize<AppConfig>(yamlContent);

    var torrentClient = appConfig.TorrentClient;
    if (torrentClient == null)
    {
        ToolUtils.PrintConsoleLine("\r\nCan not init TorrentClient");
    }
    else
    {
        ToolUtils.PrintConsoleLine($"URL: {torrentClient.Url}");

        torrentClient.Limit();
    }
}
catch (Exception ex)
{
    ToolUtils.PrintConsoleLine($"An exception occurred, {ex.Message}, please check the app.yaml configuration");
}

ToolUtils.PrintConsoleLine("\r\nPress any key to continue");
Console.ReadKey();


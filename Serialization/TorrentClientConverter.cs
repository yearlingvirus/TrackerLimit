using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using TrackerLimit.Enums;
using TrackerLimit.Torrent;

namespace TrackerLimit.Serialization
{
    public class TorrentClientConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(TorrentClient).IsAssignableFrom(type);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var deserializer = new DeserializerBuilder()
                 //.WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            // 假设我们已经在parser的当前位置有一个映射开始
            parser.MoveNext(); // 跳过映射开始
            TorrentClient? client = null;

            while (parser.Current is not MappingEnd)
            {
                var propertyName = parser.Consume<Scalar>().Value;
                if (propertyName == "ClientType")
                {
                    var clientTypeValue = parser.Consume<Scalar>().Value;
                    TorrentClientType clientType = (TorrentClientType)Enum.Parse(typeof(TorrentClientType), clientTypeValue);
                    // 根据ClientType动态决定子类类型
                    switch (clientType)
                    {
                        case TorrentClientType.Tranmission:
                            client = new Tranmission();
                            break;
                        case TorrentClientType.QBittorrent:
                            // client = new QBittorrent();
                            break;
                            // 添加更多case
                    }
                }
                else
                {
                    // 根据属性名设置client的属性
                    if (client != null)
                    {
                        var property = client.GetType().GetProperty(propertyName);
                        if (property != null)
                        {
                            var value = deserializer.Deserialize(parser, property.PropertyType);
                            property.SetValue(client, value);
                        }
                    }
                    else
                    {
                        // 跳过值
                        parser.SkipThisAndNestedEvents();
                    }
                }
            }

            parser.MoveNext(); // 跳过映射结束
            return client;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            // 可以根据需要实现或抛出不支持异常
            throw new NotImplementedException();
        }
    }
}
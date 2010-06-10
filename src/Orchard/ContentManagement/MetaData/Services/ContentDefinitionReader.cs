﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Orchard.ContentManagement.MetaData.Builders;

namespace Orchard.ContentManagement.MetaData.Services {
    public class ContentDefinitionReader : IContentDefinitionReader {
        private readonly IMapper<XElement, IDictionary<string, string>> _settingsReader;

        public ContentDefinitionReader(IMapper<XElement, IDictionary<string, string>> settingsReader) {
            _settingsReader = settingsReader;
        }

        public void Merge(XElement source, ContentTypeDefinitionBuilder builder) {
            builder.Named(XmlConvert.DecodeName(source.Name.LocalName));
            foreach (var setting in _settingsReader.Map(source)) {
                builder.WithSetting(setting.Key, setting.Value);
            }
            foreach (var iter in source.Elements()) {
                var partElement = iter;
                builder.WithPart(
                    XmlConvert.DecodeName(partElement.Name.LocalName),
                    partBuilder => {
                        foreach (var setting in _settingsReader.Map(partElement)) {
                            partBuilder.WithSetting(setting.Key, setting.Value);
                        }
                    });
            }
        }
    }
}
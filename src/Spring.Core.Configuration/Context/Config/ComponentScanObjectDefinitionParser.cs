#region License

/*
 * Copyright � 2010-2011 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System.Xml;
using Spring.Context.Attributes;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;
using Spring.Objects.Factory.Xml;

namespace Spring.Context.Config
{
    /// <summary>
    /// Parses ObjectDefinitions from classes identified by an <see cref="AssemblyObjectDefinitionScanner"/>.
    /// </summary>
	public class ComponentScanObjectDefinitionParser : IObjectDefinitionParser
	{
        /// <summary>
        /// Parse the specified XmlElement and register the resulting
        /// ObjectDefinitions with the <see cref="P:Spring.Objects.Factory.Xml.ParserContext.Registry"/> IObjectDefinitionRegistry
        /// embedded in the supplied <see cref="T:Spring.Objects.Factory.Xml.ParserContext"/>
        /// </summary>
        /// <param name="element">The element to be parsed.</param>
        /// <param name="parserContext">TThe object encapsulating the current state of the parsing process.
        /// Provides access to a IObjectDefinitionRegistry</param>
        /// <returns>The primary object definition.</returns>
        /// <remarks>
        /// 	<p>
        /// This method is never invoked if the parser is namespace aware
        /// and was called to process the root node.
        /// </p>
        /// </remarks>
		public IObjectDefinition ParseElement(XmlElement element, ParserContext parserContext)
		{
			AssemblyObjectDefinitionScanner scanner = ConfigureScanner(parserContext, element);
			IObjectDefinitionRegistry registry = parserContext.Registry;
			
			// Actually scan for objects definitions and register them.
			scanner.ScanAndRegisterTypes(registry);

			AttributeConfigUtils.RegisterAttributeConfigProcessors(registry);

            return null;
		}

        /// <summary>
        /// Configures the scanner.
        /// </summary>
        /// <param name="parserContext">The parser context.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
		protected virtual AssemblyObjectDefinitionScanner ConfigureScanner(ParserContext parserContext, XmlElement element)
		{
			var scanner = new AssemblyObjectDefinitionScanner();

            ConfigureBaseAssemblies(scanner, element);
			
			return scanner;
		}

        private void ConfigureBaseAssemblies(AssemblyObjectDefinitionScanner scanner, XmlElement element)
        {
            var baseAssemblies = element.GetAttribute("base-assemblies");

            if (string.IsNullOrEmpty(baseAssemblies))
                return;

            foreach (var baseAssembly in baseAssemblies.Split(','))
            {
                scanner.WithAssemblyFilter(assy => assy.FullName.StartsWith(baseAssembly));
            }
        }

	}
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.AppMagic.Authoring.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.PowerPlatform.Formulas.Tools.Schemas;
using Microsoft.PowerPlatform.Formulas.Tools.EditorState;

namespace Microsoft.PowerPlatform.Formulas.Tools
{
    // Various data that we can save for round-tripping.
    // Everything here is optional!!
    // Only be written during MsApp. Opaque for source file.
    internal class Entropy
    {
        // These come from volatile properties in properties.json in the msapp
        internal class PropertyEntropy
        {
            public Dictionary<string, int> ControlCount { get; set; }
            public double? DeserializationLoadTime { get; set; }
            public double? AnalysisLoadTime { get; set; }
            public double? ErrorCount { get; set; }
        }

        // Json serialize these. 
        public Dictionary<string, string> TemplateVersions { get; set; } = new Dictionary<string, string>(StringComparer.Ordinal);
        public DateTime? HeaderLastSavedDateTimeUTC { get; set; }
        public string OldLogoFileName { get; set; }

        // To fully round-trip, we need to preserve array order for the various un-ordered arrays that we may split apart.         
        public Dictionary<string, int> OrderDataSource { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> OrderComponentMetadata { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> OrderTemplate { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> OrderXMLTemplate { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public Dictionary<string, int> OrderComponentTemplate { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);

        // outer key is group control name, inner key is child grouped control name 
        public Dictionary<string, Dictionary<string, int>> OrderGroupControls { get; set; } = new Dictionary<string, Dictionary<string, int>>(StringComparer.Ordinal);

        // Key is component name, value is Index. 
        public Dictionary<string, double> ComponentIndexes { get; set; } = new Dictionary<string, double>(StringComparer.Ordinal);

        // Key is new FileName of the duplicate resource, value is Index from Resources.json. 
        public Dictionary<string, int> ResourcesJsonIndices { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Key is control name, value is publish order index
        public Dictionary<string, double> PublishOrderIndices { get; set; } = new Dictionary<string, double>(StringComparer.Ordinal);

        // Key is control name, value is uniqueId
        public Dictionary<string, int> ControlUniqueIds { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        // Key is resource name
        public Dictionary<string, string> LocalResourceRootPaths { get; set; } = new Dictionary<string, string>(StringComparer.Ordinal);

        // Key is resource name, value is filename
        public Dictionary<string, string> LocalResourceFileNames { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Some Component Function Parameter Properties are serialized with a different InvariantScript and DefaultScript.
        // We show the InvariantScript in the .yaml, and persist the difference in Entropy to ensure we have accurate roundtripping behavior.
        // The reason for using string[] is to have a way to lookup InvariantScript when the template custom properties are updated with default values in RepopulateTemplateCustomProperties.
        // Key is the fully qualified function argument name ('FunctionName'_'ScoreVariableName'), eg. SelectAppointment_AppointmentId
        public Dictionary<string, string[]> FunctionParamsInvariantScripts { get; set; } = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        // Key is control name, this should be unused if no datatables are present
        public Dictionary<string, string> DataTableCustomControlTemplateJsons { get; set; }

        public PropertyEntropy VolatileProperties { get; set; }

        // Sometimes, an empty LocalDatabaseReferences is serialized as "" or { }.
        // Need to track which so we can round-trip.
        // if true: "".   Else,  "{}". 
        public bool LocalDatabaseReferencesAsEmpty { get; set; }

        public int GetOrder(DataSourceEntry dataSource)
        {
            // To ensure that that TableDefinitions are put at the end in DataSources.json when the order information is not available.
            var defaultValue = dataSource.TableDefinition != null ? int.MaxValue : -1;
            return this.OrderDataSource.GetOrDefault<string, int>(dataSource.GetUniqueName(), defaultValue);
        }
        public void Add(DataSourceEntry entry, int? order)
        {
            if (order.HasValue)
            {
                this.OrderDataSource[entry.GetUniqueName()] = order.Value;
            }
        }

        public int GetOrder(ComponentsMetadataJson.Entry entry)
        {
            return this.OrderComponentMetadata.GetOrDefault<string, int>(entry.TemplateName, -1);
        }
        public void Add(ComponentsMetadataJson.Entry entry, int order)
        {
            this.OrderComponentMetadata[entry.TemplateName] = order;
        }

        public int GetOrder(TemplateMetadataJson entry)
        {
            return this.OrderTemplate.GetOrDefault<string, int>(entry.Name, -1);
        }
        public void Add(TemplateMetadataJson entry, int order)
        {
            this.OrderTemplate[entry.Name] = order;
        }

        public int GetOrder(TemplatesJson.TemplateJson entry)
        {
            return this.OrderXMLTemplate.GetOrDefault<string, int>(entry.Name, -1);
        }
        public void Add(TemplatesJson.TemplateJson entry, int order)
        {
            this.OrderXMLTemplate[entry.Name] = order;
        }

        public int GetComponentOrder(TemplateMetadataJson entry)
        {
            return this.OrderComponentTemplate.GetOrDefault<string, int>(entry.Name, -1);
        }
        public void AddComponent(TemplateMetadataJson entry, int order)
        {
            this.OrderComponentTemplate[entry.Name] = order;
        }

        public void Add(ResourceJson resource, int order)
        {
            var key = GetResourcesJsonIndicesKey(resource);
            if (!this.ResourcesJsonIndices.ContainsKey(key))
            {
                this.ResourcesJsonIndices.Add(key, order);
            }
        }

        // Using the name of the resource combined with the content kind as a key to avoid collisions across different resource types.
        public string GetResourcesJsonIndicesKey(ResourceJson resource)
        {
            return resource.Content + "-" + resource.Name;
        }

        // The key is of the format ContentKind-ResourceName. eg. Image-close.
        // Removing the 'ContentKind-' gives the resource name
        public string GetResourceNameFromKey(string key)
        {
            var prefix = key.Split(new char[] { '-' }).First();
            return key.Substring(prefix.Length + 1);
        }

        public void SetHeaderLastSaved(DateTime? x)
        {
            this.HeaderLastSavedDateTimeUTC = x;
        }
        public DateTime? GetHeaderLastSaved()
        {
            return this.HeaderLastSavedDateTimeUTC;
        }

        public void SetTemplateVersion(string dataComponentGuid, string version)
        {
            TemplateVersions[dataComponentGuid] = version;
        }

        public string GetTemplateVersion(string dataComponentGuid)
        {
            string version;
            TemplateVersions.TryGetValue(dataComponentGuid, out version);

            // Version string is ok to be null. 
            // DateTime.Now.ToUniversalTime().Ticks.ToString();
            return version;
        }

        public void SetLogoFileName(string oldLogoName)
        {
            this.OldLogoFileName = oldLogoName;
        }

        public void SetProperties(DocumentPropertiesJson documentProperties)
        {
            VolatileProperties = new PropertyEntropy()
            {
                AnalysisLoadTime = documentProperties.AnalysisLoadTime,
                DeserializationLoadTime = documentProperties.DeserializationLoadTime,
                ControlCount = documentProperties.ControlCount,
                ErrorCount = documentProperties.ErrorCount
            };

            documentProperties.AnalysisLoadTime = null;
            documentProperties.DeserializationLoadTime = null;
            documentProperties.ControlCount = null;
            documentProperties.ErrorCount = null;
        }

        public void GetProperties(DocumentPropertiesJson documentProperties)
        {
            if (this.VolatileProperties != null)
            {
                documentProperties.AnalysisLoadTime = VolatileProperties.AnalysisLoadTime;
                documentProperties.DeserializationLoadTime = VolatileProperties.DeserializationLoadTime;
                documentProperties.ControlCount = VolatileProperties.ControlCount;
                documentProperties.ErrorCount = VolatileProperties.ErrorCount;
            }
        }

        public void AddGroupControl(ControlState groupControl)
        {
            var name = groupControl.Name;
            var groupOrder = new Dictionary<string, int>(StringComparer.Ordinal);
            this.OrderGroupControls[name] = groupOrder;
            var order = 0;
            foreach (var child in groupControl.GroupedControlsKey)
            {
                groupOrder.Add(child, order);
                ++order;
            }
        }

        public int GetGroupControlOrder(string groupName, string childName)
        {
            if (!OrderGroupControls.TryGetValue(groupName, out var groupOrder))
                return -1;

            return groupOrder.GetOrDefault(childName, -1);
        }

        public void AddDataTableControlJson(string controlName, string json)
        {
            if (DataTableCustomControlTemplateJsons == null)
            {
                DataTableCustomControlTemplateJsons = new Dictionary<string, string>();
            }

            DataTableCustomControlTemplateJsons.Add(controlName, json);
        }

        public bool TryGetDataTableControlJson(string controlName, out string json)
        {
            json = null;
            if (DataTableCustomControlTemplateJsons == null)
            {
                return false;
            }

            return DataTableCustomControlTemplateJsons.TryGetValue(controlName, out json);
        }

        public string GetDefaultScript(string propName, string defaultValue)
        {
            if (FunctionParamsInvariantScripts.TryGetValue(propName, out string[] value) && value?.Length == 2)
            {
                return value[0];
            }
            return defaultValue;
        }

        public string GetInvariantScript(string propName, string defaultValue)
        {
            if (FunctionParamsInvariantScripts.TryGetValue(propName, out string[] value) && value?.Length == 2)
            {
                return value[1];
            }
            return defaultValue;
        }
    }
}

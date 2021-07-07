// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Xml.Linq;

// Taken from Studio code, mostly unused right now
namespace Microsoft.PowerPlatform.Formulas.Tools.ControlTemplates
{
    internal static class ControlMetadataXNames
    {
        internal static readonly XNamespace ManifestNS = "http://openajax.org/metadata";
        // TASK:72028 - Revisit the use of appMagic in namespace
        internal static readonly XNamespace AppMagicNS = "http://schemas.microsoft.com/appMagic";

        internal static readonly XName WidgetTag = ManifestNS + "widget";
        internal static readonly XName RequirementsTag = ManifestNS + "requires";
        internal static readonly XName RequirementTag = ManifestNS + "require";
        internal static readonly XName JavaScriptTag = ManifestNS + "javascript";
        internal static readonly XName ContentTag = ManifestNS + "content";
        internal static readonly XName EnumsTag = ManifestNS + "enums";
        internal static readonly XName EnumTag = ManifestNS + "enum";
        internal static readonly XName OptionsTag = ManifestNS + "options";
        internal static readonly XName OptionTag = ManifestNS + "option";
        internal static readonly XName LabelTag = ManifestNS + "label";
        internal static readonly XName EnabledForFeatureGateTag = "enabledForFeatureGate";

        internal static readonly XName PropertiesTag = ManifestNS + "properties";
        internal static readonly XName PropertyTag = ManifestNS + "property";
        internal static readonly XName NameAttribute = "name";
        internal static readonly XName TypeAttribute = "type";
        internal static readonly XName AppTypeAttribute = "appType";
        internal static readonly XName DisplayNameAttribute = "displayName";
        internal static readonly XName LocalizedNameAttribute = "localizedName";
        internal static readonly XName TemplateAttribute = "template";
        internal static readonly XName MetaDataIdAttribute = "metaDataId";
        internal static readonly XName ParentMetaDataIdAttribute = "parentMetaDataId";
        internal static readonly XName IsPrimaryControlAttribute = "isPrimaryControl";
        internal static readonly XName DataSourceLocationAttribute = "dataSourceLocation";
        internal static readonly XName VariantAttribute = "variant";
        internal static readonly XName ContentTemplateNameAttribute = "contentTemplate";
        internal static readonly XName DefaultStyleAttribute = "defaultStyle";
        internal static readonly XName RestServiceConnectorIdAttribute = "restServiceConnectorId";
        internal static readonly XName ScreenLayoutTypeAttribute = "screenLayoutType";
        internal static readonly XName DataTypeAttribute = "datatype";
        internal static readonly XName StrictAttribute = "strict";
        internal static readonly XName PropertyMappingTypeAttribute = "mappingType";
        internal static readonly XName DefaultValueAttribute = "defaultValue";
        internal static readonly XName PhoneDefaultValueAttribute = "phoneDefaultValue";
        internal static readonly XName WebDefaultValueAttribute = "webDefaultValue";
        internal static readonly XName NestedDefaultValueAttribute = "nestedDefaultValue";
        internal static readonly XName UnloadedDefaultAttribute = "unloadedDefault";
        internal static readonly XName ComputeValueAttribute = "computeValue";
        internal static readonly XName HiddenAttribute = "hidden";
        internal static readonly XName StyleableAttribute = "styleable";
        internal static readonly XName LocationAttribute = "location";
        internal static readonly XName IdAttribute = "id";
        internal static readonly XName VersionAttribute = "version";
        internal static readonly XName ClassAttribute = "jsClass";
        internal static readonly XName ValueAttribute = "value";
        internal static readonly XName PhoneValueAttribute = "phoneValue";
        internal static readonly XName WebValueAttribute = "webValue";
        internal static readonly XName AppFromDataValueAttribute = "appFromDataValue";
        internal static readonly XName MinimumAttribute = "minimum";
        internal static readonly XName MaximumAttribute = "maximum";
        internal static readonly XName InputAttribute = "input";
        internal static readonly XName OutputAttribute = "output";
        internal static readonly XName FilteredAttribute = "filtered";
        internal static readonly XName SupportsEntityTypeAttribute = "supportsEntityType";
        internal static readonly XName SupportedTypeAttribute = "supportedType";
        internal static readonly XName SupportedFormatAttribute = "supportedFormat";
        internal static readonly XName ModeAttribute = "mode";
        internal static readonly XName CustomizableAttribute = "customizable";
        internal static readonly XName IsFallbackForVariantSelectionAttribute = "isFallbackForVariantSelection";
        internal static readonly XName IsDefaultRuleAttribute = "isdefaultRule";
        internal static readonly XName DelegationCapabilityAttribute = "delegationCapability";
        internal static readonly XName LayoutEnabledAttribute = "layoutEnabled";
        internal static readonly XName SupportedOrientationAttribute = "supportedOrientation";
        internal static readonly XName LayoutGroupIdAttribute = "layoutGroupId";
        internal static readonly XName ApiNameAttribute = "apiName";
        internal static readonly XName MetaDataIdMapKeyAttribute = "metaDataIdMapKey";
        internal static readonly XName RuntimeCostAttribute = "runtimeCost";

        // Extensions to OpenAjax metadata
        internal static readonly XName AutoBindTag = AppMagicNS + "autoBind";
        internal static readonly XName NestedWidgets = AppMagicNS + "nestedWidgets";
        internal static readonly XName DataControlWidgetTag = AppMagicNS + "dataControlWidget";
        internal static readonly XName CategoryTag = AppMagicNS + "category";
        internal static readonly XName DisplayNameTag = AppMagicNS + "displayName";
        internal static readonly XName TooltipTag = AppMagicNS + "tooltip";
        internal static readonly XName PropertyHelperUITag = AppMagicNS + "helperUI";
        internal static readonly XName PassThroughTag = AppMagicNS + "passThroughReference";
        internal static readonly XName NestedAwareTag = AppMagicNS + "nestedAware";
        internal static readonly XName ReferencesNestedControlsTag = AppMagicNS + "referencesNestedControls";
        internal static readonly XName ThisItemInputTag = AppMagicNS + "thisItemInput";
        internal static readonly XName IncludePropertiesTag = AppMagicNS + "includeProperties";
        internal static readonly XName IncludePropertyTag = AppMagicNS + "includeProperty";
        internal static readonly XName CapabilitiesTag = AppMagicNS + "capabilities";
        internal static readonly XName SampleDataSourceTag = AppMagicNS + "sampleDataSource";
        internal static readonly XName ControlVariantsTag = AppMagicNS + "controlVariants";
        internal static readonly XName ControlVariantTag = AppMagicNS + "controlVariant";
        internal static readonly XName ContentTemplatesTag = AppMagicNS + "contentTemplates";
        internal static readonly XName ContentTemplateTag = AppMagicNS + "contentTemplate";
        internal static readonly XName ControlsTag = AppMagicNS + "controls";
        internal static readonly XName ControlTag = AppMagicNS + "control";
        internal static readonly XName DataSourcesTag = AppMagicNS + "datasources";
        internal static readonly XName DataSourceTag = AppMagicNS + "datasource";
        internal static readonly XName CompositionTag = AppMagicNS + "composition";
        internal static readonly XName OverridePropertiesTag = AppMagicNS + "overrideProperties";
        internal static readonly XName OverridePropertyTag = AppMagicNS + "overrideProperty";
        internal static readonly XName RulesTag = AppMagicNS + "rules";
        internal static readonly XName NestedWidgetRulesTag = AppMagicNS + "nestedWidgetRules";
        internal static readonly XName NestedWidgetTemplateTag = AppMagicNS + "nestedWidgetTemplate";
        internal static readonly XName RuleTag = AppMagicNS + "rule";
        internal static readonly XName LayoutRulesTag = AppMagicNS + "layoutRules";
        internal static readonly XName LayoutTag = AppMagicNS + "layout";
        internal static readonly XName NameMapTag = AppMagicNS + "nameMap";
        internal static readonly XName EntryTag = AppMagicNS + "entry";
        internal static readonly XName DependenciesTag = AppMagicNS + "propertyDependencies";
        internal static readonly XName DependencyTag = AppMagicNS + "propertyDependency";
        internal static readonly XName LayoutBindingsTag = AppMagicNS + "layoutBindings";
        internal static readonly XName AppMagicPropertyTag = AppMagicNS + "property";
        internal static readonly XName ColumnNameReferenceTag = AppMagicNS + "columnNameReference";
        internal static readonly XName DataSourceReferenceTag = AppMagicNS + "dataSourceReference";
        internal static readonly XName DataSourcePropertyTag = AppMagicNS + "dataSourceProperty";
        internal static readonly XName IncludeThisItemFormulaPropertyTag = AppMagicNS + "includeThisItemFormula";

        internal static readonly XName RequirementsMetadataTag = AppMagicNS + "requirementsMetadata";
        internal static readonly XName RequiredConditionsTag = AppMagicNS + "requiredConditions";
        internal static readonly XName DisallowedConditionsTag = AppMagicNS + "disallowedConditions";
        internal static readonly XName AppPreviewFlagConditionTag = AppMagicNS + "previewFlag";
        internal static readonly XName UserFeatureFlagConditionTag = AppMagicNS + "userFlag";
        internal static readonly XName FlagConditionNameAttribute = "name";
        internal static readonly XName InsertMetadataTag = AppMagicNS + "insertMetadata";
        internal static readonly XName InsertMetadataCategoryTag = AppMagicNS + "category";
        internal static readonly XName InsertMetadataIsInPreviewAttribute = "isInPreview";
        internal static readonly XName InsertMetadataIsPremiumAttribute = "isPremium";
        internal static readonly XName InsertCategoryNameAttribute = "name";
        internal static readonly XName InsertCategoryPriorityAttribute = "priority";
        internal static readonly XName SupportedParentsTag = AppMagicNS + "supportedParents";
        internal static readonly XName SupportedChildrenTag = AppMagicNS + "supportedChildren";
        internal static readonly XName SupportedControlTag = AppMagicNS + "supportedControl";
        internal static readonly XName SupportedControlNameAttribute = "name";
        internal static readonly XName DisplayMetadataTag = AppMagicNS + "displayMetadata";
        internal static readonly XName DisplayMetadataSectionTag = AppMagicNS + "section";
        internal static readonly XName DisplayPropertyGroupTag = AppMagicNS + "propertyGroup";
        internal static readonly XName DisplayPropertyGroupNameAttribute = "name";
        internal static readonly XName DisplayPropertyTag = AppMagicNS + "property";
        internal static readonly XName DisplayPropertyNameAttribute = "name";
        internal static readonly XName DisplayPropertyLabelOverrideAttribute = "labelOverride";
        internal static readonly XName DisplayPropertyItemsOrderAttribute = "itemsOrder";
        internal static readonly XName DisplayPropertyHideInPropertyPanel = "hideInPropertyPanel";
        internal static readonly XName DisplayPropertyShowOnlyInPropertyPanel = "showOnlyInPropertyPanel";
        internal static readonly XName DisplayPropertyHideInRecordingMode = "hideInRecordingMode";
        internal static readonly XName DisplayPropertyShouldAutoBind = "shouldAutoBind";
        internal static readonly XName DisplayPropertyHasNamemapsAttribute = "hasNameMaps";
        internal static readonly XName DisplayPropertyHasAdvancedNamemapsOnlyAttribute = "hasAdvancedNameMapsOnly";
        internal static readonly XName DisplayPropertyDependentPropertiesAttribute = "dependentProperties";
        internal static readonly XName DisplayWizardPropertyGroupTag = AppMagicNS + "wizardPropertyGroup";
        internal static readonly XName DisplayDataSourceSelectionPropertyTag = AppMagicNS + "dataSourceSelectionCallout";
        internal static readonly XName DisplayDataSourceSelectionPropertyDataSourceAttribute = "dataSourcePropertyName";
        internal static readonly XName ServerProvidesValue = "serverProvidesValue";
        internal static readonly XName WizardStepTag = AppMagicNS + "wizardStep";
        internal static readonly XName DisplayServerPropertyTag = AppMagicNS + "serverProperty";
        internal static readonly XName ConfigureCdsViewsTag = AppMagicNS + "configureCdsViews";
        internal static readonly XName ConfigureCdsViewsPropertyToReplaceAttribute = "propertyToReplace";
        internal static readonly XName ServerPropertyType = "type";
        internal static readonly XName WizardStepLabelAttribute = "label";
        internal static readonly XName WizardStepPanelTitleAttribute = "panelTitle";
        internal static readonly XName WizardStepLinkedLocationAttribute = "linkedLocation";
        internal static readonly XName ComputedValueTypeAttribute = "computedValueType";
        internal static readonly XName DisplayTypeAttribute = "displayType";
        internal static readonly XName WizardStepPropertyServerProvidesValue = "serverProvidesValue";
        internal static readonly XName WizardStepPropertyDependentSteps = "dependentSteps";
        internal static readonly XName ConfigureFieldsPropertyTag = AppMagicNS + "configureFields";
        internal static readonly XName ConfigureFieldsDataSourcePropertyNameAttribute = "dataSourcePropertyName";
        internal static readonly XName ConfigureFieldsSupportsCollectionAttribute = "supportsCollection";
        internal static readonly XName addCustomFieldTypeAttribute = "addCustomFieldType";
        internal static readonly XName fieldOrderingTypeAttribute = "fieldOrderingType";
        internal static readonly XName ConfigureFieldsIsEditableAttribute = "isEditable";
        internal static readonly XName ConfigureFieldsDisplayFormatAttribute = "displayFormat";
        internal static readonly XName ConfigureFieldsGenerateFieldSelectionConfigAttribute = "generateFieldSelectionConfig";
        internal static readonly XName ConfigureFieldsContentContainersAttribute = "contentContainers";
        internal static readonly XName ConfigureFieldsUnsupportedFieldTypesAttribute = "unsupportedFieldTypes";
        internal static readonly XName ConfigureFieldsVariantBuilder = "variantBuilder";
        internal static readonly XName ConfigurePropertyTag = AppMagicNS + "configureProperty";
        internal static readonly XName ConfigurePropertyRuleToSetAttribute = "ruleToSet";
        internal static readonly XName PropertyInvariantNameAttribute = "propertyInvariantName";
        internal static readonly XName FloatieDisplayTypeAttribute = "floatieDisplayType";
        internal static readonly XName ShowInFloatieAttribute = "showInFloatie";

        internal static readonly XName AuthoringClassAttribute = "authoringjsClass";
        internal static readonly XName ConverterAttribute = "converter";
        internal static readonly XName ModelValueConstrainerAttribute = "modelValueConstrainer";
        internal static readonly XName NullDefaultAttribute = "nullDefault";
        internal static readonly XName EditableAttribute = "editable";
        internal static readonly XName IsExprAttribute = "isExpr";
        internal static readonly XName IsLockableAttribute = "isLockable";
        internal static readonly XName IsVersionFlexibleAttribute = "isVersionFlexible";
        internal static readonly XName IsAppFromDataExprAttribute = "isAppFromDataExpr";
        internal static readonly XName FormatAttribute = "format";
        internal static readonly XName DirectionAttribute = "direction";
        internal static readonly XName AuthoringOnlyAttribute = "authoringOnly";
        internal static readonly XName IncludeOnFeatureGateAttribute = "includeOnFeatureGate";
        internal static readonly XName ExcludeOnFeatureGateAttribute = "excludeOnFeatureGate";
        internal static readonly XName PlatformAttribute = "platform";
        internal static readonly XName NeedsStaticLoading = "needsStaticLoading";
        internal static readonly XName HasEditableNameMapAttribute = "hasEditableNameMap";
        internal static readonly XName AcceptsCoercedNullValues = "acceptsCoercedNullValues";
        internal static readonly XName IsPrimaryInputPropertyAttribute = "isPrimaryInputProperty";
        internal static readonly XName IndicatesActiveItemAttribute = "indicatesActiveItem";
        internal static readonly XName IsPrimaryOutputPropertyAttribute = "isPrimaryOutputProperty";
        internal static readonly XName IsPrimaryDescriptionPropertyAttribute = "isPrimaryDescriptionProperty";
        internal static readonly XName IsSecondaryDescriptionPropertyAttribute = "isSecondaryDescriptionProperty";
        internal static readonly XName IsPrimaryBehaviorPropertyAttribute = "isPrimaryBehaviorProperty";
        internal static readonly XName IsSelectableFieldsPropertyPropertyAttribute = "isSelectableFieldsProperty";
        internal static readonly XName IsSelectedFieldPropertyAttribute = "isSelectedFieldProperty";
        internal static readonly XName SinkAttribute = "sink";
        internal static readonly XName SourceAttribute = "source";
        internal static readonly XName IsRecordingReadOnly = "isRecordingReadOnly";
        internal static readonly XName PromptUserBeforeChanging = "promptUserBeforeChanging";
        internal static readonly XName PromptUserTitle = "promptUserTitle";
        internal static readonly XName PromptUserBody = "promptUserBody";
        internal static readonly XName PromptUserOkButtonText = "promptUserOkButtonText";
        internal static readonly XName PromptUserOnValue = "promptUserOnValue";
        internal static readonly XName RequiresWriteableDataSource = "requiresWriteableDataSource";
        internal static readonly XName RequiresRefreshableDataSource = "requiresRefreshableDataSource";
        internal static readonly XName Conversion = AppMagicNS + "conversion";
        internal static readonly XName ConversionAction = AppMagicNS + "conversionAction";
        internal static readonly XName ConversionTypeAttribute = "type";
        internal static readonly XName ConversionNameAttribute = "name";
        internal static readonly XName ConversionFromAttribute = "from";
        internal static readonly XName ConversionToAttribute = "to";
        internal static readonly XName ConversionNewNameAttribute = "newName";
        internal static readonly XName ConversionNewDocVersionAttribute = "newDocVersion";
        internal static readonly XName CanBeCompressed = "canBeCompressed";

        // Control capabilities
        internal static readonly XName SupportsNestedControlsAttribute = "supportsNestedControls";
        internal static readonly XName AllowsNestedControlsWithoutUnlockingAttribute = "allowsNestedControlsWithoutUnlocking";
        internal static readonly XName SupportsNestedComponentsAttribute = "supportsNestedComponents";
        internal static readonly XName ReplicatesNestedControlsAttribute = "replicatesNestedControls";
        internal static readonly XName PassThisItemThroughAttribute = "passThisItemThrough";
        internal static readonly XName ContextualViewsEnabledAttribute = "contextualViewsEnabled";
        internal static readonly XName AllowsPerCharacterFormattingAttribute = "allowsPerCharacterFormatting";
        internal static readonly XName SupportsPagingAttribute = "supportsPaging";
        internal static readonly XName IsTypeBoundToPrimaryInput = "isTypeBoundToPrimaryInput";
        internal static readonly XName isTypeInferredFromPrimaryInput = "isTypeInferredFromPrimaryInput";
        internal static readonly XName AutoBordersAttribute = "autoBorders";
        internal static readonly XName AutoFocusedBordersAttribute = "autoFocusedBorders";
        internal static readonly XName AutoBorderRadiusAttribute = "autoBorderRadius";
        internal static readonly XName AutoFillAttribute = "autoFill";
        internal static readonly XName AutoPointerViewStateAttribute = "autoPointerViewState";
        internal static readonly XName AutoDisabledViewStateAttribute = "autoDisabledViewState";
        internal static readonly XName OutlineIncompatibleAttribute = "outlineIncompatible";
        internal static readonly XName IntangibleAttribute = "intangible";
        internal static readonly XName AppGlobalAttribute = "appGlobal";
        internal static readonly XName ViewContainerAttribute = "viewContainer";
        internal static readonly XName AddPropertiesToParentAttribute = "addPropertiesToParent";
        internal static readonly XName ScreenActiveAwareAttribute = "screenActiveAware";
        internal static readonly XName SharedByMultipleControlsAttribute = "sharedByMultipleControls";
        internal static readonly XName DefaultLayoutNameAttribute = "defaultLayoutName";
        internal static readonly XName FieldSelectionFieldPropertyInvariantNameAttribute = "fieldSelectionFieldPropertyInvariantName";
        internal static readonly XName FieldSelectionDisplayNamePropertyInvariantNameAttribute = "fieldSelectionDisplayNamePropertyInvariantName";
        internal static readonly XName AggregatesChildLayouts = "aggregatesChildLayouts";
        internal static readonly XName ChildrenShowAllProperties = "childrenShowAllProperties";
        internal static readonly XName RequiresCameraAccess = "requiresCameraAccess";
        internal static readonly XName RequiresMicrophoneAccess = "requiresMicrophoneAccess";
        internal static readonly XName RequiresLocalStateAccess = "requiresLocalStateAccess";
        internal static readonly XName RequiresInternetAccess = "requiresInternetAccess";
        internal static readonly XName RequiresEnterpriseAuthentication = "requiresEnterpriseAuthentication";
        internal static readonly XName RequiresPrivateNetworkAccess = "requiresPrivateNetworkAccess";
        internal static readonly XName ReplicationLimitAttribute = "replicationLimit";
        internal static readonly XName ManagesNestedControlBoundsAttribute = "managesNestedControlBounds";
        internal static readonly XName AggregatesChildUpdatesAttribute = "aggregatesChildUpdates";
        internal static readonly XName SubmitsDataAttribute = "submitsData";
        internal static readonly XName SupportsAutomationAttribute = "supportsAutomation";
        internal static readonly XName PromptUserBeforeChangingProperty = "promptUserBeforeChangingProperty";
        internal static readonly XName PromptUserBeforeChangingPropertyValue = "promptUserBeforeChangingPropertyValue";
        internal static readonly XName PromptUserBeforeChangingPropertyTitle = "promptUserBeforeChangingPropertyTitle";
        internal static readonly XName PromptUserBeforeChangingPropertyBody = "promptUserBeforeChangingPropertyBody";
        internal static readonly XName PromptUserBeforeChangingPropertyOkButtonText = "promptUserBeforeChangingPropertyOkButtonText";


        /// <summary>
        /// Whether `SetFocus` function in PowerApps language can be used on the control
        /// </summary>
        /// <remarks>
        /// The SetFocus function requests a control to focus itself.
        /// This capability does not indicate whether a control is tabbable
        /// or focusable through other means.
        /// </remarks>
        /// <see cref="Authoring.Texl:SetFocusFunction"/>
        internal static readonly XName SupportsSetFocusAttribute = "supportsSetFocus";

        // AccessibilityCheck names
        internal static readonly XName AccessibilityChecksTag = AppMagicNS + "accessibilityChecks";
        internal static readonly XName AccessibilityCheckControlIsInteractive = "controlIsInteractive";
        internal static readonly XName AccessibilityCheckTag = AppMagicNS + "accessibilityCheck";
        internal static readonly XName AccessibilityCheckTypeAttribute = "type";
        internal static readonly XName AccessibilityCheckInteractivityAttribute = "interactivity";
        internal static readonly XName AccessibilityCheckLogicAttribute = "condition";
        internal static readonly XName AccessibilityCheckPropertyAttribute = "property";
    }
}

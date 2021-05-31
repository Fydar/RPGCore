<h1>
<img src="../../icon.png" width="54" height="54" align="left" />
RPGCore.Data
</h1>

Serialization standards and support for RPGCore.

## RPGCore.Data.Polymorphic

Serialization supports polymorphic types for serialized objects.

![Usage](../../../docs/samples/RPGCore.Data/PolymorphicInlineOptions.serialize.svg)

Types can be added using the fluent API.

![Usage](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseAutoResolve.configure.svg)

![Usage](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseExplicit.configure.svg)

### RPGCore.Data.Polymorphic.Inline

Attributes can be used to declare what types should serialize to preserve object types.

![Base type with attribute and naming convention](../../../docs/samples/RPGCore.Data/PolymorphicInlineBaseTypeName.types.svg)

An explicit set of types that are serializable can be declared ahead-of-time.

![Base type with explicit attributes](../../../docs/samples/RPGCore.Data/PolymorphicInlineBaseExplicit.types.svg)

Attributes can also be used on the child types.

![Child types with attribute](../../../docs/samples/RPGCore.Data/PolymorphicInlineChildName.types.svg)

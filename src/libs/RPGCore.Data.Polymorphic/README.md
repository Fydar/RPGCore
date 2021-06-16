<h1>
<img src="../../icon.png" width="54" height="54" align="left" />
RPGCore.Data
</h1>

Serialization standards and support for RPGCore.

## RPGCore.Data.Polymorphic

Serialization supports polymorphic types for serialized objects.

![Usage](../../../docs/samples/RPGCore.Data/PolymorphicInlineOptions.serialize.svg)

The descrminator used to determine what type the object is included in the Json output.

![Child types with attribute output](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseAutoResolve.output.svg)

### Resolving subtypes automatically

Subtypes for a known base type can be determined via reflection at runtime using the `ResolveSubTypesAutomatically` method.

![Usage](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseAutoResolve.configure.svg)

When a `RemoveProcedure` from the previous sample is serialized the following json is produced.

![Child types with attribute output](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseAutoResolve.output.svg)

### Registering subtypes explicitly

Subtypes for a known base type can be explicitly registered and configured on a per-type basis.

![Usage](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseExplicit.configure.svg)

When a `RemoveProcedure` from the previous sample is serialized the following json is produced.

![Child types with attribute output](../../../docs/samples/RPGCore.Data/PolymorphicFluentBaseExplicit.output.svg)

## RPGCore.Data.Polymorphic.Inline

Attributes can be used to declare what types should serialize to preserve object types.

![Base type with attribute and naming convention](../../../docs/samples/RPGCore.Data/PolymorphicInlineBaseTypeName.types.svg)

When a type that inherits from this `IProcedure` is serialized, the following json is produced.

![Base type with attribute and naming convention serialized output](../../../docs/samples/RPGCore.Data/PolymorphicInlineBaseTypeName.output.svg)

### Explicit type naming

An explicit set of types that are serializable can be declared ahead-of-time.

![Base type with explicit attributes](../../../docs/samples/RPGCore.Data/PolymorphicInlineBaseExplicit.types.svg)

Attributes can also be used on the child types.

![Child types with attribute](../../../docs/samples/RPGCore.Data/PolymorphicInlineChildName.types.svg)

Using either of the previous demos, when a `RemoveProcedure` is serialized the following json is produced.

![Child types with attribute output](../../../docs/samples/RPGCore.Data/PolymorphicInlineBaseExplicit.output.svg)

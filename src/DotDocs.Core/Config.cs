using DotDocs.Core.Util;
using Newtonsoft.Json;

namespace DotDocs.Core
{
    struct ConfigConstants
    {
        public const string EXTERNAL_PERSPECTIVE = "external";
        public const string INTERNAL_PERSPECTIVE = "internal";
    }

    public enum Perspective
    {
        External,
        Internal,        
        Default = External
    }

    public enum AccessibilityModifier
    {
        Public,
        Protected,
        Internal,
        Private
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public record ClassConfig : AccessibleConfig
    {
        [JsonProperty("denoteIfStatic")]
        public bool? DenoteIfStatic { get; init; } = true;

        [JsonProperty("denoteIfVirtual")]
        public bool? DenoteIfVirtual { get; init; } = true;

        [JsonProperty("denoteIfAbstract")]
        public bool? DenoteIfAbstract { get; init; } = true;

        [JsonConstructor]
        public ClassConfig(
            Perspective perspective,
            bool? showIfPublic, 
            bool? showIfProtected, 
            bool? showIfInternal, 
            bool? showIfInternalProtected, 
            bool? showIfPrivateProtected, 
            bool? showIfPrivate,
            bool? denoteIfStatic,
            bool? denoteIfVirtual,
            bool? denoteIfAbstract
            ) : base(
                perspective,
                showIfPublic, 
                showIfProtected, 
                showIfInternal, 
                showIfInternalProtected,
                showIfPrivateProtected,
                showIfPrivate)
        {
            DenoteIfStatic = denoteIfStatic;
            DenoteIfVirtual = denoteIfVirtual;
            DenoteIfAbstract = denoteIfAbstract;
        }

        public ClassConfig(Perspective perspective) : base(perspective) { }

        public ClassConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate,
                DenoteIfStatic,
                DenoteIfVirtual,
                DenoteIfAbstract
            );
    }

    public record DelegateConfig : AccessibleConfig
    {
        [JsonConstructor]
        public DelegateConfig(
            Perspective perspective,
            bool? showIfPublic, 
            bool? showIfProtected, 
            bool? showIfInternal, 
            bool? showIfInternalProtected, 
            bool? showIfPrivateProtected, 
            bool? showIfPrivate        
            ) : base(
                perspective,
                showIfPublic, 
                showIfProtected, 
                showIfInternal, 
                showIfInternalProtected,
                showIfPrivateProtected,
                showIfPrivate) 
        { }

        public DelegateConfig(Perspective perspective) : base(perspective) { }

        public DelegateConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate
            );
    }

    public record EnumConfig : AccessibleConfig
    {
        [JsonConstructor]
        public EnumConfig(
            Perspective perspective,
            bool? showIfPublic, 
            bool? showIfProtected, 
            bool? showIfInternal, 
            bool? showIfInternalProtected, 
            bool? showIfPrivateProtected, 
            bool? showIfPrivate) 
            : base(
                  perspective,
                  showIfPublic, 
                  showIfProtected, 
                  showIfInternal, 
                  showIfInternalProtected,
                  showIfPrivateProtected,
                  showIfPrivate)
        { }

        public EnumConfig(Perspective perspective) : base(perspective) { }

        public EnumConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate
            );
    }

    public record EventConfig : AccessibleConfig
    {        
        public bool? DenoteIfStatic { get; init; } = true;        
        public bool? DenoteIfVirtual { get; init; } = true;        
        public bool? DenoteIfAbstract { get; init; } = true;

        [JsonConstructor]
        public EventConfig(
                Perspective perspective,
                bool? showIfPublic,
                bool? showIfProtected,
                bool? showIfInternal,
                bool? showIfInternalProtected,
                bool? showIfPrivateProtected,
                bool? showIfPrivate,
                bool? denoteIfStatic = true,
                bool? denoteIfVirtual = true,
                bool? denoteIfAbstract = true
            ) : base(
                perspective,
                showIfPublic,
                showIfProtected,
                showIfInternal,
                showIfInternalProtected,
                showIfPrivateProtected,
                showIfPrivate)
        {
            DenoteIfStatic = denoteIfStatic;
            DenoteIfVirtual = denoteIfVirtual;
            DenoteIfAbstract = denoteIfAbstract;
        }

        public EventConfig(Perspective perspective) : base(perspective) { }

        public EventConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivate,
                DenoteIfStatic,
                DenoteIfVirtual,
                DenoteIfAbstract
            );
    }

    public record FieldConfig : AccessibleConfig
    {
        public bool? DenoteIfConst { get; init; } = true;
        public bool? DenoteIfReadonly { get; init; } = true;

        [JsonConstructor]
        public FieldConfig(
                Perspective perspective,
                bool? showIfPublic,
                bool? showIfProtected,
                bool? showIfInternal,
                bool? showIfInternalProtected,
                bool? showIfPrivateProtected,
                bool? showIfPrivate,
                bool? denoteIfConst = true,
                bool? denoteIfReadonly = true
            ) : base(
                perspective,
                showIfPublic,
                showIfProtected,
                showIfInternal,
                showIfInternalProtected,
                showIfPrivateProtected,
                showIfPrivate)
        {
            DenoteIfConst = denoteIfConst;
            DenoteIfReadonly = denoteIfReadonly;
        }

        public FieldConfig(Perspective perspective) : base(perspective) { }

        public FieldConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivate,
                DenoteIfConst,
                DenoteIfReadonly
            );
    }

    public record InterfaceConfig : AccessibleConfig
    {
        [JsonConstructor]
        public InterfaceConfig(
            Perspective perspective,
            bool? showIfPublic, 
            bool? showIfProtected,
            bool? showIfInternal, 
            bool? showIfInternalProtected,
            bool? showIfPrivateProtected,
            bool? showIfPrivate
            ) : base(
                  perspective,
                  showIfPublic,
                  showIfProtected,
                  showIfInternal,
                  showIfInternalProtected,
                  showIfPrivateProtected,
                  showIfPrivate)
        { }

        public InterfaceConfig(Perspective perspective) : base(perspective) { }

        public InterfaceConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate
            );
    }

    public record MemberConfig
    {
        public PropertyConfig Property { get; init; }
        public FieldConfig Field { get; init; }
        public MethodConfig Method { get; init; }
        public EventConfig Event { get; init; }

        [JsonConstructor]
        public MemberConfig(
            PropertyConfig property,
            FieldConfig field,
            MethodConfig method,
            EventConfig @event
            )
        {
            Property = property;
            Field = field;
            Method = method;
            Event = @event;
        }

        public MemberConfig(
            Perspective perspective
            ) : this(
                new(perspective),
                new(perspective),
                new(perspective),
                new(perspective)
            )
        { }

        public MemberConfig From(Perspective perpsective)
            => new(
                 Property == null ? new PropertyConfig(perpsective) : Property.From(perpsective),
                 Field == null ? new FieldConfig(perpsective) : Field.From(perpsective),
                 Method == null ? new MethodConfig(perpsective) : Method.From(perpsective),
                 Event == null ? new EventConfig(perpsective) : Event.From(perpsective)
                );
    }

    public record MethodConfig : AccessibleConfig
    {
        public bool? DenoteIfStatic { get; init; } = true;
        public bool? DenoteIfVirtual { get; init; } = true;
        public bool? DenoteIfAbstract { get; init; } = true;

        [JsonConstructor]
        public MethodConfig(
            Perspective perspective,
            bool? showIfPublic,
            bool? showIfProtected,
            bool? showIfInternal,
            bool? showIfInternalProtected,
            bool? showIfPrivateProtected,
            bool? showIfPrivate,
            bool? denoteIfStatic = true,
            bool? denoteIfVirtual = true,
            bool? denoteIfAbstract = true
            ) : base(
                  perspective,
                  showIfPublic,
                  showIfProtected,
                  showIfInternal,
                  showIfInternalProtected,
                  showIfPrivateProtected,
                  showIfPrivate)
        {
            DenoteIfStatic = denoteIfStatic;
            DenoteIfVirtual = denoteIfVirtual;
            DenoteIfAbstract = denoteIfAbstract;
        }

        public MethodConfig(Perspective perspective) : base(perspective) { }

        public MethodConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate
            );
    }

    public record PropertyConfig : AccessibleConfig
    {
        public bool? DenoteIfStatic { get; init; } = true;
        public bool? DenoteIfVirtual { get; init; } = true;
        public bool? DenoteIfAbstract { get; init; } = true;
        public bool? DenoteIfReadonly { get; init; } = true;
        public bool? DenoteIfSetonly { get; init; } = true;
        public bool? DenoteIfHasGetter { get; init; } = true;
        public bool? DenoteIfHasSetter { get; init; } = true;
        public bool? DenoteIfGetPublic { get; init; } = true;
        public bool? DenoteIfGetProtected { get; init; } = true;
        public bool? DenoteIfGetInternal { get; init; } = true;
        public bool? DenoteIfGetInternalProtected { get; init; } = true;
        public bool? DenoteIfGetPrivate { get; init; } = true;
        public bool? DenoteIfSetPublic { get; init; } = true;
        public bool? DenoteIfSetProtected { get; init; } = true;
        public bool? DenoteIfSetInternal { get; init; } = true;
        public bool? DenoteIfSetInternalProtected { get; init; } = true;
        public bool? DenoteIfSetPrivate { get; init; } = true;

        [JsonConstructor]
        public PropertyConfig(
            Perspective perspective,
            bool? showIfPublic,
            bool? showIfProtected,
            bool? showIfInternal,
            bool? showIfInternalProtected,
            bool? showIfPrivateProtected,
            bool? showIfPrivate,
            bool? denoteIfStatic = true,
            bool? denoteIfVirtual = true,
            bool? denoteIfAbstract = true,
            bool? denoteIfReadonly = true,
            bool? denoteIfSetonly = true,
            bool? denoteIfHasGetter = true,
            bool? denoteIfHasSetter = true,
            bool? denoteIfGetPublic = true,
            bool? denoteIfGetProtected = true,
            bool? denoteIfGetInternal = true,
            bool? denoteIfGetInternalProtected = true,
            bool? denoteIfGetPrivate = true,
            bool? denoteIfSetPublic = true,
            bool? denoteIfSetProtected = true,
            bool? denoteIfSetInternal = true,
            bool? denoteIfSetInternalProtected = true,
            bool? denoteIfSetPrivate = true          
                ) : base(
                    perspective,
                    showIfPublic,
                    showIfProtected,
                    showIfInternal,
                    showIfInternalProtected,
                    showIfPrivateProtected,
                    showIfPrivate
                )
        {
            DenoteIfStatic = denoteIfStatic;
            DenoteIfVirtual = denoteIfVirtual;
            DenoteIfAbstract = denoteIfAbstract;
            DenoteIfReadonly = denoteIfReadonly;
            DenoteIfSetonly = denoteIfSetonly;
            DenoteIfHasGetter = denoteIfHasGetter;
            DenoteIfHasSetter = denoteIfHasSetter;
            DenoteIfGetPublic = denoteIfGetPublic;
            DenoteIfGetProtected = denoteIfGetProtected;
            DenoteIfGetInternal = denoteIfGetInternal;
            DenoteIfGetInternalProtected = denoteIfGetInternalProtected;
            DenoteIfGetPrivate = denoteIfGetPrivate;
            DenoteIfSetPublic = denoteIfSetPublic;
            DenoteIfSetProtected = denoteIfSetProtected;
            DenoteIfSetInternal = denoteIfSetInternal;
            DenoteIfSetInternalProtected = denoteIfSetInternalProtected;
            DenoteIfSetPrivate = denoteIfSetPrivate;
        }

        public PropertyConfig(Perspective perspective) : base(perspective) { }

        public PropertyConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate
            );
    }

    public record Configuration
    {
        public TypeConfig? Type { get; init; }
        public MemberConfig? Member { get; init; }
        public Perspective Perspective { get; init; }

        [JsonConstructor]
        public Configuration(TypeConfig? type, MemberConfig? member, string? perspective)
        {
            Type = type;
            Member = member;
            Perspective = perspective?.ToPerspective() ?? Perspective.Default;
        }

        public Configuration(TypeConfig? type, MemberConfig? member, Perspective perspective)
        {
            Type = type;
            Member = member;
            Perspective = perspective;
        }

        public Configuration From()
        {
            return new Configuration(
                Type == null ? new TypeConfig(Perspective) : Type.From(Perspective),
                Member == null ? new MemberConfig(Perspective) : Member.From(Perspective),
                Perspective
            );
        }
    }

    public record StructConfig : AccessibleConfig
    {
        [JsonConstructor]
        public StructConfig(
            Perspective perspective,
            bool? ShowIfPublic, 
            bool? ShowIfProtected, 
            bool? ShowIfInternal,
            bool? ShowIfInternalProtected,
            bool? ShowIfPrivateProtected,
            bool? ShowIfPrivate) 
            : base(
                  perspective,
                  ShowIfPublic, 
                  ShowIfProtected, 
                  ShowIfInternal,
                  ShowIfInternalProtected,
                  ShowIfPrivateProtected,
                  ShowIfPrivate)           
        { }

        public StructConfig(Perspective perspective) : base(perspective) { }

        public StructConfig From(Perspective perspective)
            => new(
                perspective,
                ShowIfPublic,
                ShowIfProtected,
                ShowIfInternal,
                ShowIfInternalProtected,
                ShowIfPrivateProtected,
                ShowIfPrivate                 
            );
    }

    public record TypeConfig
    {
        public ClassConfig? Class { get; set; }
        public InterfaceConfig? Interface { get; set; }
        public StructConfig? Struct { get; set; }
        public EnumConfig? Enum { get; set; }
        public DelegateConfig? Delegate { get; set; }

        [JsonConstructor]
        public TypeConfig(
            ClassConfig? @class,        
            InterfaceConfig @interface,
            StructConfig @struct,
            EnumConfig @enum,
            DelegateConfig @delegate
            )
        {
            Class = @class;
            Interface = @interface;
            Struct = @struct;
            Enum = @enum;
            Delegate = @delegate;
        }

        public TypeConfig(
            Perspective perspective
            ) : this(           
                new(perspective),
                new(perspective),
                new(perspective),
                new(perspective),
                new(perspective)
            )
        { }

        public TypeConfig From(Perspective perspective)
            => new(
                    Class == null ? new ClassConfig(perspective) : Class.From(perspective),
                    Interface == null ? new InterfaceConfig(perspective) : Interface.From(perspective),
                    Struct == null ? new StructConfig(perspective) : Struct.From(perspective),
                    Enum == null ? new EnumConfig(perspective) : Enum.From(perspective),
                    Delegate == null ? new DelegateConfig(perspective) : Delegate.From(perspective)
                );
    }

    public abstract record AccessibleConfig(
        [property: JsonProperty("showIfPublic")] bool? ShowIfPublic,
        [property: JsonProperty("showIfProtected")] bool? ShowIfProtected,
        [property: JsonProperty("showIfInternal")] bool? ShowIfInternal,
        [property: JsonProperty("showIfInternalProtected")] bool? ShowIfInternalProtected,
        [property: JsonProperty("showIfPrivateProtected")] bool? ShowIfPrivateProtected,
        [property: JsonProperty("showIfPrivate")] bool? ShowIfPrivate        
    )
    {
        public AccessibleConfig(
            Perspective perspective
            ) : this(
                perspective.From(AccessibilityModifier.Public),
                perspective.From(AccessibilityModifier.Protected),
                perspective.From(AccessibilityModifier.Internal),
                perspective.From(AccessibilityModifier.Internal | AccessibilityModifier.Protected),
                perspective.From(AccessibilityModifier.Private | AccessibilityModifier.Protected),
                perspective.From(AccessibilityModifier.Private))
        { }

        public AccessibleConfig(
            Perspective perspective,
            bool? showIfPublic,
            bool? showIfProtected,
            bool? showIfInternal,
            bool? showIfInternalProtected,
            bool? showIfPrivateProtected,
            bool? showIfPrivate
            ) : this(
                showIfPublic ?? perspective.From(AccessibilityModifier.Public),
                showIfProtected ?? perspective.From(AccessibilityModifier.Protected),
                showIfInternal ?? perspective.From(AccessibilityModifier.Internal),
                showIfInternalProtected ?? perspective.From(AccessibilityModifier.Internal | AccessibilityModifier.Protected),
                showIfPrivateProtected ?? perspective.From(AccessibilityModifier.Private | AccessibilityModifier.Protected),
                showIfPrivate ?? perspective.From(AccessibilityModifier.Private))
        { }
    }
}

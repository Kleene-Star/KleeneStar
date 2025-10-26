# Kleenestar Core Data Model

The **Kleenestar** Core Data Model provides a modular, typed architecture for the structured management of digital information objects. It is based on a clear separation between type definitions (`Class`) and concrete instances (`Object`). Through inheritance, semantic extensibility, and contextual organization via `Workspaces`, it achieves high flexibility and scalability. Attributes are modeled separately into definition (`Field`) and value (`Value`), enabling dynamic UI generation, validation, and polymorphic processing. Configuration is declared via JSON or YAML files outside the database, supporting automated provisioning and consistent multi-stage environments. Complemented by versioning, access control, and semantic linking, it forms a robust foundation for collaborative, revision-safe, and domain-specifically adaptable platform solutions.

## Introduction

The digital working world is characterized by fragmented information flows, isolated tools, and redundant data stores. **Kleenestar** addresses these challenges with a central, modular platform built upon a clearly structured, extensible data model. At its core are three abstracted key objects: `Workspace`, `Entity`, and `Attribute`. They form the foundation for flexible, context-based organization of knowledge, tasks, and digital resources.

These concepts are abstracted through inheritance. The goal is a consistent, extensible model that can efficiently represent both generic and domain-specific information objects.

## Data model

The **Kleenestar** Core Data Model is based on a clearly structured, modular architecture that distinguishes between type definitions, concrete instances, and semantic extensions. The central idea is the separation of structure and content, complemented by contextual organization, polymorphic processing, and flexible extensibility.

The following diagram shows the most important components and their relationships:

```
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                             KleeneStar Core Data Model                               ║
╠══════════════════════════════════════════════════════════════════════════════════════╣
║                                                                                      ║
║       ┌─────────────┐ *           * ┌───────┐ 1          * ┌───────┐                 ║
║       │ Workspace   ├──────────────►│ Class │◄─────────────┤ Field │                 ║
║       └─────┬───────┘               └───────┘              └───────┘                 ║
║             │ 1                         ▲ 1                    ▲ 1                   ║
║             └──────────────────────┐    │                      │                     ║
║                                    ▼ *  │ *                    │ *                   ║
║              ┌──────┐ *        2 ┌──────┴─┐ 1            * ┌───┴───┐                 ║
║              │ Link ├───────────►│ Object │◄───────────────┤ Value │                 ║
║              └──────┘            └────────┘                └───────┘                 ║
║                                   ▲ 1   ▲ 1                    ▲ 1                   ║
║                     ┌─────────────┘     │                      │                     ║
║                     │ *                 │ *                    │ *                   ║
║                ┌────┴────┐         ┌────┴────┐         ┌───────┴───────┐             ║
║                │ Comment │         │ Version │         │ FileReference │             ║
║                └─────────┘         └─────────┘         └───────────────┘             ║
║                                                                                      ║
╚══════════════════════════════════════════════════════════════════════════════════════╝
```

Explanation of relationships:
- **Workspace → Class:** A workspace can contain multiple class types (1:n).
- **Class → Field:** Each class type can reference multiple fields (1:n).
- **Object → Class:** Each instance is based on exactly one class (n:1).
- **Object → Workspace:** Each instance belongs to a workspace (n:1).
- **Object → Value:** An instance can have multiple object values (1:n).
- **Value → Field:** Each value is based on a definition (n:1).
- **Object → Link:** Entities can be connected via links (2:n).
- **Object → Comment / Version / FileReference:** Additional metadata and content (1:n).

### Workspace - The contextual frame

A workspace is a thematic or functional container that defines content, configurations, and semantic boundaries. It represents, for example, a project, a department, a client mandate, or a thematic area.

Key characteristics:

|Field        |Type     |Description
|-------------|---------|------------------------------------
|workspace_id |UUID     |Unique identifier
|icon         |URI      |URI for UI representation (e.g., calendar, tag, email)
|title        |String   |Name of the workspace
|description  |Text     |Context description
|category     |Enum     |Typing (e.g., "Project", "Team")
|tags         |List     |Keywords for categorization
|created_at   |Timestamp|Creation timestamp

Functional meaning: Workspaces structure the platform into logical units. They enable separation of content, aggregation of related entities, and targeted control of visibility and configuration.

### Entity - The abstract information object

In the **Kleenestar** data model, the entity serves as the central concept for structured, typed information objects. An entity describes not only content, but also its semantic role, relationships, and extensibility. It is not a rigid data structure, but an abstract modeling unit that can be flexibly adapted to different use cases through typing, inheritance, and contextualization.

Entities exist in two clearly separated forms:

- **Class:** the formal definition of a type, comparable to a class in object-oriented systems. It specifies which fields exist, which data types are used, and whether the type can be instantiated.
- **Object:** the concrete manifestation of an entity type with actual content and values. It represents an individual object in the system, e.g., an article, a task, or an asset.

Advantages of this separation:

- Consistent structure definition
- Reusability through inheritance
- Dynamic UI generation
- Validation and type checking

Entities are always assigned to a workspace, which contextualizes and logically groups them. They can be linked to each other, versioned, and semantically extended via attributes.

#### Class - Structure and inheritance

An entity type describes the structure, semantics, and rules of an information object. It can appear as an abstract base type or as a concrete subtype.

Structure:

|Field          |Type    |Description
|---------------|--------|-------------------
|type_id        |UUID    |Unique identifier
|icon           |URI     |URI for UI representation (e.g., calendar, tag, email)
|name           |String  |Name of the type
|parent_type_id |UUID    |Inheritance relationship
|is_abstract    |Boolean |Instantiability
|fields         |List    |Structure definition
|description    |Text    |Semantic description

##### Abstract base types

Abstract entities define common structural characteristics and behavior without being instantiated themselves. Examples:

- `AbstractEntity`
- `KnowledgeBaseEntity`
- `ProcessEntity`
- `ResourceEntity`

##### Concrete subtypes

Concrete entities inherit from base types and specify additional fields or behavior:

- `Article` (`KnowledgeBaseEntity`)
- `Task` (`ProcessEntity`)
- `Asset` (`ResourceEntity`)
- `Checklist`, `DecisionLog`, `Template` (`KnowledgeBaseEntity`)

Function: Inheritance enables reusability, type consistency, and polymorphic processing. New subtypes can be defined and registered by developers or customers.

#### Object - Concrete objects

An entity instance is the concrete manifestation of an entity type within a workspace. It contains the actual content and links.

Structure:

|Field           |Type      |Description
|----------------|----------|--------------------
|entity_id       |UUID      |Unique identifier
|type_id         |UUID      |Reference to the entity type
|workspace_id    |UUID      |Membership
|title           |String    |Title
|content         |RichText  |Content
|status          |Enum      |Lifecycle status
|version         |Integer   |Version number
|linked_entities |List      |Linked entities
|created_at      |Timestamp |Creation timestamp

### Attribute - The semantic extension

Attributes are custom fields that augment entities with additional metadata. They consist of a definition and a value modeled separately.

#### Field

The definition describes the formal structure, validity, and validation rules of a custom attribute. It serves as a template for `Value` instances and enables typed, context-dependent extension of entities.

|Field              |Type    |Description
|-------------------|--------|-------------------------------------
|definition_id      |UUID    |Unique identifier
|name               |String  |Field name
|type               |Enum    |Data type (Text, Number, Date, Selection)
|allowed_values     |List    |For selection fields: valid values
|scope              |List    |Valid entity types
|is_required        |Boolean |Required field
|is_unique          |Boolean |Indicates whether the value must be unique within a workspace
|is_multiple        |Boolean |Indicates whether multiple values per entity instance are allowed
|validation_pattern |Regex   |Optional regular expression pattern for format validation (e.g., email)
|default_value      |Mixed   |Default value
|is_editable        |Boolean |Indicates whether the value can be changed after first assignment
|is_visible         |Boolean |Controls visibility in the UI (e.g., for system fields or admin-only)
|is_system          |Boolean |Marks system-critical fields that must not be deleted or overwritten
|placeholder        |String  |Placeholder text in the input field (e.g., "e.g., +49 123 4567890")
|help_text          |String  |Contextual help or description for users
|icon               |URI     |URI for UI representation (e.g., calendar, tag, email)

#### Value

The attribute value stores the actual value of a custom field for a specific entity instance. It is directly linked to an attribute type and constitutes the semantic extension of the information object. Separating definition and value enables clear typing, validation, and reusability.

|Field         |Type      |Description
|--------------|----------|---------------------------
|value_id      |UUID      |Unique identifier
|entity_id     |UUID      |Reference to the entity instance
|definition_id |UUID      |Reference to the definition
|value         |Mixed     |Concrete value
|created_at    |Timestamp |Assignment timestamp
|updated_at    |Timestamp |Last change

Function: Separating definition and value enables validation, reusability, and UI generation. Attributes can be inherited or overridden, depending on the entity type.

## Relationships and interoperability

Entities can be linked across workspace boundaries. The platform supports bidirectional relationships, event-based synchronization, and semantic linking. Each relationship is represented by a link, which can be semantically classified using a link type (e.g., "references", "is part of", "replaces").

Example of a bidirectional relationship:

A task can be linked to an asset required to complete the task. At the same time, the asset references the task in which it is used. This relationship is bidirectional and allows navigation in both directions:

```
Task: "Create presentation"
   └─ LinkType: "uses"
       └─ Asset: "Corporate Template.pptx"

Asset: "Corporate Template.pptx"
   └─ LinkType: "used in"
       └─ Task: "Create presentation"
```       
       
These semantically typed links enable consistent, searchable, and context-based interconnection of information objects.
   
## Validation and consistency rules

The **Kleenestar** Core Data Model ensures that all entity instances are consistent and valid by strictly adhering to the structure definitions of their associated entity types. Validation occurs on multiple levels:

- Required fields: Attribute types can be marked with `is_required = true`. When creating or updating an entity instance, the system checks whether all required fields are filled.
- Data type checking: Each attribute value is checked against the type specified in the definition (e.g., Date, Enum, Text). Invalid formats or type deviations are rejected.
- Value range: For selection fields (Enum), the system checks whether the entered value belongs to the allowed `allowed_values`.
- Scope: Attribute types are only valid for specific entity types. The platform prevents attributes from being used outside their defined scope.
- Regex pattern check: For fields of type Text or String, an optional regular expression (`validation_pattern`) can be stored. This allows checking of complex formats.

These rules ensure a consistent data foundation and enable reliable processing, filtering, and presentation of content.

## UI generation from type definitions

A central feature of the **Kleenestar** model is automatic generation of user interfaces based on entity type definitions. Each entity type contains structured field information that can be directly translated into dynamic forms and views:

- Form fields: For each attribute, an appropriate input element is generated (e.g., dropdown for Enum, calendar for Date, text field for String).
- Frontend validation: Required fields, allowed values, data types, and optionally regex patterns are validated client-side before data is saved.
- Layout and grouping: Attributes can be organized into groups or tabs, based on metadata in the definition.
- Multilingual support: Field names and descriptions can be displayed language-dependently, since type definitions are stored in a language-sensitive manner.
- Manual configuration: The automatically generated UI can be specifically adapted through configuration files or UI overrides. For example, field orders, custom components, conditional logic, or visual styles can be controlled per project or per type.

This creates a flexible, maintainable UI that automatically adapts to new types and extensions- and can be fine-tuned manually when needed without deep UI programming.

## Versioning and history

Every `Object`, every `Value`, and every comment can be versioned to make changes traceable and to transparently document the lifecycle of an information object. Versioning is done via a separate version object, which is linked to the respective element and contains metadata such as timestamp, reason for change, and author.

Mechanisms of versioning:

- Versioning on changes: Every change to content, attributes, or comments automatically creates a new version with a timestamp and an optional change reason.
- Comparison and restoration: Previous versions can be viewed, compared with one another, and restored if necessary-both at the field and the object level.
- Audit trail: The complete history of an entity forms the basis for revision security, regulatory traceability, and collaborative tracking.
- Type-dependent versioning: Depending on the entity type, versioning can be configured granularly- for example, only for certain fields, only on status changes, or on external triggers.

This function is particularly relevant in regulated environments (e.g., medical, legal, finance) or in collaborative processes with high documentation requirements. It enables not only technical traceability, but also semantic transparency over the entire lifecycle of a digital object.

## Access control (ACL) and visibility

The authorization concept is hybrid. It combines a code-based, declarative approach for defining permissions and roles with flexible management of groups and their assignments.

- Permission: An atomic authorization describing a specific action (e.g., `entity:edit`). Declared as a class in code.
- Role: Bundles multiple permissions into a logical role (e.g., `Editor`). Also declared as a class in code.
- Group: A freely definable collection of users (e.g., "Marketing Team", "Project Admins"). Groups are managed in the data model.
- User: A user account in the system.

A user's permissions thus result indirectly from the combination of their group memberships and the roles assigned to these groups in a specific context.

Permissions are primarily assigned at the `Workspace` level and inherited by the `Entities`. These can be overridden at the `Entity` level. Read or write access to `Attributes` is additionally controlled via the `Field`. A user must therefore have the general permission for an action on the `Entity` (e.g., edit) and also fulfill the specific role requirement of the attribute.

Multi-tenancy: In multi-tenant environments, the workspace structure ensures clear separation between organizational units.

These mechanisms ensure data protection, role-based collaboration, and controlled visibility of information.

## Configuration

To enable consistent and automatable system configuration, **Kleenestar** relies on a declarative configuration strategy outside the database. All relevant settings-e.g., UI overrides, type registrations, workspace templates, or authorization rules, can be stored as structured parameter files in JSON or YAML format directly on the server.

Advantages of this architecture:

- Automation: The configuration files are fully compatible with infrastructure management tools such as Puppet, Ansible, or Terraform. This makes it possible to reliably provision multiple identically configured stages (e.g., Dev, Test, Prod).
- Version control: Since the configuration is not in the database, it can be versioned, documented, and audited via Git or other VCS systems.
- Transparency and portability: The configuration is readable, editable, and portable-ideal for containerized deployments or multi-tenant environments.
- Separation of data and control: The platform deliberately separates persisted user data (in the DB) from system-controlling parameters (in the file system), which increases maintainability and security.

This configuration strategy makes **Kleenestar** particularly suitable for complex, scalable, and automatically managed platform landscapes, without sacrificing flexibility or adaptability.

## Extensibility and type registration

The platform supports dynamic registration of new entity types by plugins or external providers. Each newly defined type is based on an existing base type and comes with its own structured field definition. This allows the data model to be flexibly extended without jeopardizing the consistency or integrity of the existing architecture.

Example of type registration:

A third-party plugin registers the new entity type `IncidentReport`, which inherits from `ProcessEntity`. After registration, this type can be used immediately, including UI generation, validation, and semantic linking with existing entities.

## Advantages of the model

The **Kleenestar** Core Data Model is based on clear principles that enable a flexible and consistent information architecture. The following points show how structure, extensibility, and context interact meaningfully to make digital work processes efficient and scalable.

- Separation of structure and content: Entity types define, entity instances store.
- Inheritance and abstraction: Reusable types and polymorphic processing.
- Extensibility: Attributes enable flexible metadata without schema changes.
- Contextualization: Workspaces create semantic order.
- Validation and UI generation: Structure definitions enable dynamic forms and checks.

## Conclusion

The **Kleenestar** Core Data Model forms a robust, typed foundation for the structured management of complex information objects in modular work platforms. Through contextual structuring into workspaces, the typable and linkable modeling of entities, and semantic extensibility via attributes, it creates a flexible system that can cover both generic and domain-specific requirements.

The consistent separation between type definition and instance, support for abstract and inheritable entity types, and dynamic extension through custom attributes make the model a powerful foundation for knowledge-intensive, agile organizations. It lays the groundwork for consistent data management, scalable information architectures, and semantically precise digital work processes.

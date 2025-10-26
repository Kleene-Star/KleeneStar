![KleeneStar](https://raw.githubusercontent.com/kleene-star/.github/main/docs/assets/img/banner.png)

# Kleenestar Core Data Model

In today's digital work environment, information is both the most valuable asset and the greatest source of chaos. Knowledge is scattered across countless applications, data is stored in isolated silos, and workflows are fragmented, leading to inefficiencies, redundancies, and a loss of context. The **Kleenestar** platform is engineered to counteract this entropy by providing a unified, coherent, and semantically rich foundation for digital collaboration.

At the heart of this endeavor lies the **Kleenestar Core Data Model**. It is not merely a database schema but a comprehensive architectural philosophy for modeling, managing, and interconnecting digital information. Its design is guided by the principles of clarity, extensibility, and digital sovereignty. The primary objective is to create a system where the structure of information (`Class`, `Field`) is rigorously separated from its content (`Object`, `Value`). This fundamental separation enables a level of flexibility and automation that is impossible to achieve with rigid, monolithic application architectures.

By establishing clear contextual boundaries through `Workspaces`, facilitating reuse and consistency through inheritance, and making relationships between information first-class citizens (`Link`), the model provides a robust framework. This framework is designed to grow and adapt to specific domain needs without sacrificing its core integrity. This document provides a detailed exploration of this model, its components, and the strategic advantages that arise from its design.

## Architectural Overview: Core Principles of the Data Model

The **Kleenestar** Core Data Model is built upon several key architectural principles that work in concert to deliver a flexible and scalable system. Understanding these principles is essential to grasping the model's full potential.

- **Separation of Structure and Content:** The most critical principle is the strict division between the definition of an information type (`Class`, `Field`) and its concrete instance (`Object`, `Value`). This allows the system to understand the "shape" of data, enabling it to dynamically generate user interfaces, perform validation, and process information polymorphically.
- **Contextualization through Workspaces:** Information does not exist in a vacuum. `Workspaces` act as semantic containers that provide context, control access, and segregate data. This is fundamental for multi-tenancy, project-based collaboration, and organizing information in a way that mirrors human cognitive structures.
- **Explicit, Typed Relationships:** Instead of relying on implicit or simple foreign-key relationships, the model uses a dedicated `Link` entity. This makes relationships first-class citizens, allowing them to be typed, described, and queried. The result is a rich knowledge graph, not just a collection of disconnected tables.
- **Granular, Non-Destructive History:** All changes are captured through a `Version` entity. This delta-based approach ensures a complete, auditable, and storage-efficient history of every object, which is crucial for compliance, accountability, and collaborative workflows.
- **Declarative, Code-Driven Configuration:** System-critical definitions, such as classes and roles, are managed outside the database in version-controllable files (JSON/YAML). This aligns with modern Infrastructure-as-Code (IaC) and GitOps practices, ensuring transparency, repeatability, and maintainability.

The following diagram illustrates the interplay between these core components:

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

### Workspace - The Contextual Frame

A `Workspace` is far more than a simple folder or container. It represents a complete contextual boundary—a digital realm with its own set of users, configurations, and semantic rules. It provides the primary mechanism for data segregation and organization. For example, a workspace could represent a single project, a client account, a department's internal knowledge base, or a thematic research area. Everything within a workspace shares a common context, making information discovery intuitive and access control manageable.

| Field       | Type      | Description
|-------------|-----------|-------------------------------------------
| workspace_id| UUID      | Unique identifier
| icon        | URI       | URI for UI representation
| title       | String    | Name of the workspace
| description | Text      | Contextual description
| category    | Text      | Typing (e.g., "Project", "Team")
| tags        | List      | Keywords for categorization
| created_at  | Timestamp | Creation timestamp

### Entity - The Abstract Information Object

The concept of an `Entity` is central to the model and is realized through two distinct forms: `Class` (the blueprint) and `Object` (the instance). This separation is analogous to classes and objects in object-oriented programming and is the key to the model's power and flexibility.

#### Class - Structure and Inheritance

A `Class` is the formal, abstract definition of an information type. It specifies the structure, semantics, and rules that all its instances must follow. It defines which fields an object of its type will have, their data types, and their validation rules. Through inheritance (`parent_class_id`), classes can form a hierarchy, allowing for the creation of specialized types that reuse and extend the properties of more general base types. This promotes consistency and reduces redundancy.

| Field           | Type    | Description
|-----------------|---------|---------------------------------------------------
| class_id        | UUID    | Unique identifier
| icon            | URI     | URI for UI representation
| name            | String  | Unique name of the type
| parent_class_id | UUID    | Inheritance from another class
| is_abstract     | Boolean | `true` if the class cannot be instantiated
| fields          | List    | Structure definition (references to `Field` objects)
| description     | Text    | Semantic description

#### Object - Concrete Instances

An `Object` is a concrete manifestation of a `Class`. It is a tangible piece of information within the system—be it a task, a document, a contact, or any other domain-specific entity. Each object belongs to exactly one `Workspace`, which provides its context, and is an instance of exactly one `Class`, which defines its structure. The object itself holds core data like a title and content, but its rich semantic meaning comes from its associated `Value`s.

| Field        | Type      | Description
|--------------|-----------|-------------------------------------
| object_id    | UUID      | Unique identifier
| class_id     | UUID      | Reference to the `Class`
| workspace_id | UUID      | Membership in the `Workspace`
| title        | String    | Title of the object
| content      | RichText  | Main content (optional)
| status       | Enum      | Lifecycle status
| created_at   | Timestamp | Creation timestamp
| created_by   | UUID      | Reference to the creating `User`
| updated_at   | Timestamp | Timestamp of the last modification
| updated_by   | UUID      | Reference to the last editing `User`

### Attribute - The Semantic Extension

Attributes provide the mechanism for semantically enriching entities with custom metadata. This concept is also split into two parts: the definition (`Field`) and the instance (`Value`). This separation ensures that custom data is just as structured and validated as core data.

#### Field - Attribute Definition

A `Field` is the blueprint for a piece of metadata. It defines the name, data type, validation rules (`is_required`, `validation_pattern`), and UI-related properties (`placeholder`, `help_text`) of an attribute. A field can be associated with one or more classes, making it a reusable component for defining structure. This is the key to enabling users or developers to extend the data model without altering the core database schema.

| Field              | Type    | Description
|--------------------|---------|-------------------------------------------------
| field_id           | UUID    | Unique identifier
| name               | String  | Field name (unique within the class context)
| type               | Enum    | Data type (Text, Number, Date, Selection, etc.)
| allowed_values     | List    | For `Selection`: valid values
| scope              | List    | Valid classes (`class_id`s) for this field
| is_required        | Boolean | Required field
| is_unique          | Boolean | Value must be unique
| is_multiple        | Boolean | Multiple values per object allowed
| validation_pattern | Regex   | Regular expression for validation
| default_value      | Mixed   | Default value
| is_editable        | Boolean | Value can be changed after creation
| is_visible         | Boolean | Visibility in the UI
| is_system          | Boolean | System-critical field
| placeholder        | String  | Placeholder text for input fields
| help_text          | String  | Contextual help for users
| icon               | URI     | UI icon for the field

#### Value - Attribute Value

A `Value` is the concrete data point that links a `Field` to an `Object`. It holds the actual content for a specific attribute on a specific object instance. For example, if an object of class `Task` has a field `DueDate`, the `Value` entity would store the specific date for that task, linking the `object_id` of the task to the `field_id` of `DueDate`.

| Field      | Type      | Description
|------------|-----------|-------------------------------------
| value_id   | UUID      | Unique identifier
| object_id  | UUID      | Reference to the object instance
| field_id   | UUID      | Reference to the `Field` definition
| value      | Mixed     | The concrete value
| created_at | Timestamp | Assignment timestamp
| updated_at | Timestamp | Last modification

#### Link - Explicit, First-Class Relationships

In many systems, relationships are merely implicit database foreign keys. The **Kleenestar** model elevates relationships to be `Link` entities, which are first-class citizens. A `Link` is a dedicated object that connects a `source_object` to a `target_object` with a specific semantic `link_type` (e.g., "RELATES_TO", "DUPLICATES", "BLOCKS"). This approach transforms the collection of data into a true knowledge graph. It allows relationships themselves to have metadata (like a description or creator), to be queried directly, and to be visualized, enabling a much deeper understanding of how information is interconnected.

| Field            | Type      | Description
|------------------|-----------|----------------------------------------------------------------------
| link_id          | UUID      | Unique identifier
| source_object_id | UUID      | The source object of the relationship
| target_object_id | UUID      | The target object of the relationship
| link_type        | Enum      | Semantic type of the relationship (e.g., "RELATES_TO", "DUPLICATES", "BLOCKS")
| description      | Text      | Optional description of the link
| created_at       | Timestamp | Creation timestamp
| created_by       | UUID      | Reference to the creating `User`

#### Version - Granular, Non-Destructive History

Traceability is not an afterthought but a core feature. Every meaningful change to an `Object` or its `Value`s is captured in a `Version` entity. Instead of wastefully duplicating the entire object on each save, this model uses a delta-based approach. The `Version` object stores only the fields that were changed (`changed_fields`), along with metadata about who made the change, when, and optionally why. This provides a highly efficient, complete, and auditable history, enabling features like undo/redo, change analysis, and compliance reporting without bloating the database.

| Field          | Type      | Description
|----------------|-----------|-----------------------------------------------------------------------------
| version_id     | UUID      | Unique identifier
| object_id      | UUID      | Reference to the versioned `Object`
| change_type    | Enum      | Type of change (e.g., "CREATE", "UPDATE", "DELETE")
| changed_fields | JSON      | A JSON object containing the changed fields and their old/new values
| reason         | Text      | Optional reason for the change (provided by the user)
| created_at     | Timestamp | Timestamp of the change
| created_by     | UUID      | Reference to the `User` who performed the change

#### AccessControlEntry (ACE) - Fine-Grained, Explicit Access Control

Security and access control are handled through an explicit and granular `AccessControlEntry` (ACE) model. Instead of relying on broad, implicit roles, an ACE defines a specific `permission` (e.g., "READ", "WRITE") on a specific `resource` (a `Workspace`, `Object`, or even a `Field`) for a specific `principal` (a `User` or `Group`). This allows for extremely fine-grained control over who can see and do what. The model supports both "allow" and "deny" rules, providing the flexibility needed to manage complex security requirements in collaborative or multi-tenant environments.

| Field          | Type    | Description
|----------------|---------|--------------------------------------------------------------------------
| ace_id         | UUID    | Unique identifier
| principal_id   | UUID    | ID of the principal (can be a `user_id` or `group_id`)
| principal_type | Enum    | Type of the principal ("USER" or "GROUP")
| resource_id    | UUID    | ID of the resource (`workspace_id`, `object_id`, `field_id`)
| resource_type  | Enum    | Type of the resource ("WORKSPACE", "OBJECT", "FIELD")
| permission     | String  | The granted permission (e.g., "READ", "WRITE", "DELETE")
| is_allowed     | Boolean | `true` for "allow", `false` for "deny" (Deny rules take precedence)


### System-Level Features

The architecture of the **Kleenestar** data model is designed for more than just the mere storage and structuring of data. Rather, the underlying design decisions, particularly the strict separation of structure and content, enable a range of higher-level, system-wide functionalities. These features extend beyond pure data management and form the basis for the automation, maintainability, and dynamic adaptability of the entire platform. The following sections describe how the data model directly contributes to the realization of these powerful system capabilities.

#### UI Generation from Type Definitions

A key benefit of the strict separation of structure and content is the ability to automatically generate user interfaces. The platform can read the `Class` and `Field` definitions and dynamically construct forms and views for creating, editing, and displaying objects. A `Field` of type `Date` becomes a date picker, a `Selection` becomes a dropdown, and `is_required` flags translate directly into client-side validation. This dramatically accelerates development and ensures that the UI is always in sync with the data model.

#### Declarative Configuration and Extensibility

The system's core structure is not hidden away in a database. `Class` definitions, roles, and other critical configurations are defined in human-readable JSON or YAML files stored in the file system. This makes the system's configuration transparent, versionable with Git, and deployable as part of an automated CI/CD pipeline. It allows developers to practice Infrastructure-as-Code, ensuring consistency across development, staging, and production environments. Furthermore, new `Class` types can be registered via plugins, allowing the platform to be extended by third parties without compromising its architectural integrity.

## Conclusion

The **Kleenestar** Core Data Model is intentionally designed to be more than just a schema; it is a strategic framework for building adaptable, scalable, and maintainable information systems. By formalizing concepts like `Link`, `Version`, and `AccessControlEntry` and adhering to the strict separation of structure and content, it provides a powerful foundation. It moves beyond simple data storage to create a system that understands the semantics, context, and history of information. This approach mitigates digital chaos, fosters structured collaboration, and provides a solid platform for building the next generation of knowledge management tools.
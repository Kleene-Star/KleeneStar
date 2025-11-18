![KleeneStar](https://raw.githubusercontent.com/kleene-star/.github/main/docs/assets/img/banner.png)

# Architecture Concept

The architecture of **KleeneStar** follows a core-plus-modules model that enables a clear separation between central cross-cutting functions and domain-focused extensions. This principle forms the basis for a modular, scalable, and maintainable platform.

## Core

The Core forms the technical and organizational foundation of the platform. It provides central services used by all modules to ensure consistency, security, and reusability. The Core is designed so that it can be updated and extended independently of the domain modules without impairing their functionality.

## Modules

The modules are domain-focused plugins that are loosely connected to the Core and to each other via clearly defined APIs. They implement concrete use cases and make use of Core services for authentication, data persistence, event processing, and configuration.

All modules communicate exclusively via the interfaces provided by the Core. This achieves loose coupling, allowing modules to be developed, updated, or replaced independently. New modules can be added without changes to the Core as long as they adhere to the defined interfaces.

## Architecture Diagrams

The architecture diagrams illustrate the structure and embedding of **KleeneStar** from both an external and an internal perspective. They form the basis for a shared understanding among developers, architects, administrators, and stakeholders by clearly depicting the essential building blocks, their relationships, and relevant interfaces.

### Context Diagram (System in Its Environment)

The context diagram depicts **KleeneStar** in its system environment and shows how it interacts with external actors and systems. It clarifies the system boundary: What belongs to the platform, what lies outside it, and through which interfaces exchange occurs. At the center stands **KleeneStar** as an integrated platform consisting of the Core and the domain modules. Grouped around it are external systems such as identity providers (LDAP, Active Directory, OpenID Connect, SAML), third-party systems (e.g., ERP, CRM), as well as end users who access the platform through various interfaces (web UI, customer portal).

```
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                               KleeneStar – Context Diagram                           ║
╠══════════════════════════════════════════════════════════════════════════════════════╣
║                                                                                      ║
║         ┌──────────────────────────────────────────────────────┐                     ║
║         │                    External Systems                  │                     ║
║         │ (LDAP, AD, OIDC, SAML, ERP, CRM, File Servers, APIs) │                     ║
║         └──────────────▲───────────────────────────────────────┘                     ║
║                        │                                                             ║
║                        │ Authentication / Data Integration                           ║
║                        │                                                             ║
║ ┌──────────────────────┴───────────────────────────────────────────────────────────┐ ║
║ │                              KleeneStar Platform                                 │ ║
║ ├──────────────────────────────────────────────────────────────────────────────────┤ ║
║ │ ┌────────────────────────────────┐   ┌─────────────────────────────────────────┐ │ ║
║ │ │              Core              │   │             Domain Modules              │ │ ║
║ │ │────────────────────────────────│   │─────────────────────────────────────────│ │ ║
║ │ │ • Authentication               │   │ • Knowledge Management                  │ │ ║
║ │ │ • Authorization                │   │ • Process Management                    │ │ ║
║ │ │ • API Gateway                  │   │ • Asset Management                      │ │ ║
║ │ │ • Event Bus                    │   │ • Customer Portal                       │ │ ║
║ │ │ • Logging & Monitoring         │   │ • User Administration                   │ │ ║
║ │ │ • Internationalization (i18n)  │   │ • Optional Modules (CMS, Reporting, …)  │ │ ║
║ │ │ • Configuration Management     │   └─────────────────────────────────────────┘ │ ║
║ │ └────────────────────────────────┘                                               │ ║
║ │                                                                                  │ ║
║ │                       ┌──────────────────────────┐                               │ ║
║ │                       │        Database          │                               │ ║
║ │                       │ (central / per-module)   │                               │ ║
║ │                       └──────────────────────────┘                               │ ║
║ │                                                                                  │ ║
║ └─────────────────────▲────────────────────────────────────────────────────────────┘ ║
║                       │                                                              ║
║                       │ Access via UI / API                                          ║
║                       │                                                              ║
║            ┌──────────┴──────────────────────────────────────────────┐               ║
║            │                          Users                          │               ║
║            │ (Internal Staff, Customers, Partners, Administrators)   │               ║
║            └─────────────────────────────────────────────────────────┘               ║
║                                                                                      ║
║ Legend:                                                                              ║
║ ▲ / ▼  = Data or control flow                                                        ║
╚══════════════════════════════════════════════════════════════════════════════════════╝
```

### Component Diagram (Core-Plus-Modules)

The core-plus-modules architecture model of **KleeneStar** describes the fundamental structure of the platform and highlights the clear separation between central cross-cutting services and domain-focused modules. The platform Core provides uniform foundational functions such as authentication, authorization, API gateway, event bus, logging, monitoring, internationalization, and configuration management to all modules. Each domain module has its own module core, which implements the module’s basic functions and provides defined extension points. Building on that, plugins can extend the functionality of the module without modifying the module core itself. This architecture ensures loose coupling, high extensibility, and the ability to develop, maintain, and deploy modules and plugins independently of one another. The shared database serves as the persistent foundation for all modules.

```
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                       KleeneStar – Core-Plus-Modules Architecture                    ║
╠══════════════════════════════════════════════════════════════════════════════════════╣
║                                                                                      ║
║ ┌──────────────────────────────────────────────────────────────────────────────────┐ ║
║ │                                     Core                                         │ ║
║ ├──────────────────────────────────────────────────────────────────────────────────┤ ║
║ │ • Authentication / Authorization                                                 │ ║
║ │ • API Gateway                                                                    │ ║
║ │ • Event Bus                                                                      │ ║
║ │ • Logging & Monitoring                                                           │ ║
║ │ • Internationalization (i18n)                                                    │ ║
║ │ • Configuration Management                                                       │ ║
║ └──────────────────────────────────────┬───────────────────────────────────────────┘ ║
║                                        │                                             ║
║                                        │                                             ║
║                      ┌─────────────────▼───────────────────┐                         ║
║                      │            Domain Modules           │                         ║
║                      ├─────────────────────────────────────┤                         ║
║                      │ Each module consists of:            │                         ║
║                      │  • Module Core (basic functions)    │                         ║
║                      │  • optional plugins for extension   │                         ║
║                      │    of functionality                 │                         ║
║                      └─────────────────┬───────────────────┘                         ║
║                                        ¦                                             ║
║   ┌---------------┬---------------┬----┴----------┬-----------------┐                ║
║   ¦               ¦               ¦               ¦                 ¦                ║
║ ┌─▼──────────┐  ┌─▼──────────┐  ┌─▼──────────┐  ┌─▼────────────┐  ┌─▼──────────────┐ ║
║ │ Knowledge  │  │ Process    │  │ Asset      │  │ Customer     │  │ User           │ ║
║ │ Management │  │ Management │  │ Management │  │ Portal       │  │ Administration │ ║
║ └─────┬──────┘  └─────┬──────┘  └─────┬──────┘  └──────┬───────┘  └─────┬──────────┘ ║
║       │               │               │                │                │            ║
║  ┌────▼────┐     ┌────▼────┐     ┌────▼────┐      ┌────▼────┐      ┌────▼────┐       ║
║  │Plugin 1 │     │Plugin 1 │     │Plugin 1 │      │Plugin 1 │      │Plugin 1 │       ║
║  ├─────────┤     ├─────────┤     ├─────────┤      ├─────────┤      ├─────────┤       ║
║  │Plugin 2 │     │Plugin 2 │     │Plugin 2 │      │Plugin 2 │      │Plugin 2 │       ║
║  └─────────┘     └─────────┘     └─────────┘      └─────────┘      └─────────┘       ║
║                                                                                      ║
║ Legend:                                                                              ║
║ ▲ / ▼  = Data or control flow                                                        ║
╚══════════════════════════════════════════════════════════════════════════════════════╝
```

### Event Bus

The **KleeneStar** Event Bus is an independently operated, distributed messaging service that serves as the central backbone for asynchronous communication. It connects all modules, whether they run in the same data center, in different Kubernetes namespaces, or at geographically separated locations, and ensures reliable, scalable, and fault-tolerant event processing.


Architectural characteristics:

- **Distributed cluster architecture:** The Event Bus consists of multiple broker nodes that together form a logical bus.
- **Replication:** Every event is replicated across multiple brokers (configurable replication factor) to ensure fault tolerance and data consistency.
- **Partitioning:** Topics can be split into partitions to enable load distribution and parallel processing.
- **Transparent client connectivity:** Publishers and consumers connect to any broker in the cluster; routing and replication are handled internally.
- **Standardized event schemas:** Uniform, versioned formats (e.g., JSON) ensure interoperability between modules.
- **Reliable delivery:** A combination of pub/sub pattern, event sourcing, and outbox pattern prevents data loss.

```
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                                   KleeneStar – Event Bus                             ║
╠══════════════════════════════════════════════════════════════════════════════════════╣
║                                                                                      ║
║            ┌────────────────────┐                   ┌────────────────────┐           ║
║            | Module A (Producer)|                   | Module B (Producer)|           ║
║            └─────────┬──────────┘                   └─────────┬──────────┘           ║
║                      |                                        |                      ║
║                      │                                        │                      ║
║             ┌────────▼───────┐   ┌────────────────┐   ┌───────▼────────┐             ║
║             | Broker 1       ◄───► Broker 2       ◄───► Broker 3       |             ║
║             │ Leader P0,P1   │   │ Leader P2      │   │ Leader P3      │             ║
║             | Follower P2,P3 |   | Follower P0,P3 |   | Follower P0,P1 |             ║
║             └────────▲───────┘   └────────────────┘   └───────▲────────┘             ║
║                      │                                        │                      ║
║                      |                                        |                      ║
║           ┌──────────┴──────────┐                   ┌─────────┴───────────┐          ║
║           | Module C (Consumer) |                   | Module D (Consumer) |          ║
║           └─────────────────────┘                   └─────────────────────┘          ║
║                                                                                      ║
║ Legend:                                                                              ║
║ - P0..P3 = Partitions                                                                ║
║ - Leader/Follower = replication roles                                                ║
║ - Publisher/Consumer connect to any broker                                           ║
╚══════════════════════════════════════════════════════════════════════════════════════╝
```

Example: A publisher (e.g., Process Management module) connects to a broker in the cluster and publishes a TicketCreated event. The broker stores the event in the responsible partition and automatically replicates it to other broker nodes. Consumers (e.g., Notification module, Reporting module) connect to arbitrary brokers and receive the event from the partition they are registered for. If a broker fails, a replica automatically takes over the leader role for the affected partitions.

### Reverse Index (Inverted Index)

The **KleeneStar** Reverse Index is a distributed, cross-module indexing service that serves as a shared search foundation for all modules. It enables consistent, performant, and scalable full-text search across content produced and maintained by different services, whether stored locally or provided via APIs. As such, the reverse index forms the backbone for semantic linking, contextual navigation, and cross-module research.


Architectural characteristics:

- **Distributed index cluster:** The reverse index consists of multiple index nodes that together form a logical search space. Each node manages segments of the global index and replicates relevant shards for fault tolerance.
- **Cross-module indexing:** Services can register structured or unstructured content (e.g., Markdown, JSON, HTML, metadata). The index normalizes this data and extracts relevant tokens, entities, and relationships.
- **Federated Search API:** Modules access the index via a standardized search interface (REST/GraphQL). Queries are distributed and aggregated internally so that results from multiple sources are returned consistently.
- **Replication and sharding:** Index data is sharded (e.g., by module, language, content type) and replicated across multiple nodes depending on configuration. This increases availability and enables parallel processing.
- **Incremental updates:** Changes to content (e.g., new tickets, updated documentation) are reported to the index via event bus or direct API calls. The index updates affected segments without a full rebuild.
- **Standardized index formats:** Uniform, versioned index schemas (e.g., OpenSearch-compatible) ensure interoperability and enable cross-module filtering, faceting, and ranking.
- **Multi-tenancy and access control:** The index supports tenant-separated data spaces and fine-grained permissions so that modules can access only authorized content.

```
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                             KleeneStar – Reverse Index                               ║
╠══════════════════════════════════════════════════════════════════════════════════════╣
║                                                                                      ║
║            ┌────────────────────┐                 ┌────────────────────┐             ║
║            | Module A (Indexer) |                 | Module B (Indexer) |             ║
║            └─────────┬──────────┘                 └─────────┬──────────┘             ║
║                      │                                      │                        ║
║             ┌────────▼───────┐   ┌──────────────┐   ┌───────▼────────┐               ║
║             | Index Node 1   ◄───► Index Node 2 ◄───► Index Node 3   |               ║
║             │ Shard S0,S1    │   │ Shard S2     │   │ Shard S3       │               ║
║             | Repl. S2,S3    |   | Repl. S0,S3  |   | Repl. S0,S1    |               ║
║             └────────▲───────┘   └──────────────┘   └───────▲────────┘               ║
║                      │                                      │                        ║
║           ┌──────────┴──────────┐                ┌──────────┴──────────┐             ║
║           | Module C (Searcher) |                | Module D (Searcher) |             ║
║           └─────────────────────┘                └─────────────────────┘             ║
║                                                                                      ║
║ Legend:                                                                              ║
║ - S0..S3 = Index shards                                                              ║
║ - Repl. = replication roles                                                          ║
║ - Indexers register content, searchers issue queries                                 ║
╚══════════════════════════════════════════════════════════════════════════════════════╝
```

Example: The Documentation module registers a new Markdown page along with metadata such as title, tags, and language. The reverse index extracts relevant tokens, generates a semantic profile, and stores it in shard S1. Later, the Reporting module issues a search query for "Ticket Lifecycle." The reverse index aggregates matching entries from shards S1 and S3, applies filters based on language and module context, and returns the results. If an index node becomes unavailable, a replica automatically takes over to maintain uninterrupted search functionality.


## Architectural Principles and Guidelines

The architecture of **KleeneStar** is based on clearly defined principles and guidelines that ensure a sustainable, secure, and extensible platform. They constitute the binding framework for all decisions in design, implementation, and operations, ensuring that technical solutions remain consistent, future-proof, and maintainable.

### Foundational Design Principles

These principles form the foundation of the **KleeneStar** architecture. They define how modules, interfaces, and functions are designed to create a robust, flexible, and long-term viable platform.

- **Loose coupling:** Modules and components are designed so that changes in one area have only minimal impact on others. This facilitates maintenance, replacement, and parallel evolution.
- **API-first:** All functions are provided via clearly defined, documented interfaces. This enables consistent integration of internal and external systems and simplifies automation.
- **Security-by-design:** Security aspects are an integral part from the outset, including authentication, authorization, encryption, and audit logging.
- **Configuration over customization:** Functional and technical parameters are primarily controlled via configuration to implement individual requirements without code changes. Configurations are always stored at file level to enable Git versioning and to document changes transparently. For automated distribution and updates of these configuration files, configuration management tools such as Puppet or comparable solutions can be used.
- **Testability:** Architecture and code are structured so that automated tests at all levels (unit, integration, end-to-end) are efficiently possible.

### Technological Guardrails

The technological guardrails define preferred standards, protocols, frameworks, and formats. They create a unified technological framework that ensures interoperability, security, and maintainability while providing sufficient flexibility for future developments.

- **Preferred protocols:** Use of open, standardized protocols such as HTTPS/REST, WebSocket, or gRPC for communication and integration.
- **Frameworks:** Use of proven, long-term supported frameworks (e.g., **WebExpress** framework as the platform base) with an active community and clear update strategy.
- **Data and configuration formats:** Use of interoperable formats such as JSON or YAML for data exchange and XML as the primary configuration format. Configuration files are deliberately stored at file level so they can be versioned in Git and changes documented. For automated distribution and updates of these configurations, configuration management tools like Puppet or similar solutions may be used. In containerized environments (e.g., Docker, Kubernetes, OpenShift), XML configurations can be mounted as volumes or provided via ConfigMaps/Secrets to ensure consistent, versioned, and automatically deployable configuration even in highly scalable scenarios. UTF-8 is the mandatory standard encoding for all text-based formats to preserve compatibility and interoperability.
- **Interoperability:** Design of interfaces and data models compatible with common enterprise systems and open-source solutions.

### Quality Attributes

The quality attributes describe the non-functional properties **KleeneStar** must fulfill sustainably. They define how performant, extensible, maintainable, and integrable the platform is and serve as a benchmark for evaluating the architecture during ongoing operations.

- **Scalability:** Support for horizontal and vertical scaling to handle growing loads and data volumes.
- **Extensibility:** Ability to add new modules and plugins without changing existing components.
- **Maintainability:** Clear structure, clean interfaces, and consistent documentation facilitate troubleshooting and further development.
- **Interoperability:** Open standards and loose coupling enable seamless integration into existing system landscapes.

## Technology Stack and Infrastructure

The **KleeneStar** technology stack is chosen to enable a modular, extensible, and long-term maintainable platform. It combines a modern, stable primary language with proven frameworks, scalable infrastructure components, and clearly defined integration points with external services.

### Programming Languages, Frameworks, Databases, and Messaging Systems

**KleeneStar** is built on a robust technological foundation that combines a modern programming language with proven frameworks and scalable infrastructure. Its architecture integrates databases and messaging systems that ensure reliable internal workflows and seamless external communication. Together, these components form the backbone of the platform’s performance, flexibility, and interoperability.

- **Programming language:** Primarily C# for the development of core components, modules, and plugins. For client-side browser functionality, vanilla JavaScript is used without additional frontend frameworks.
- **Frameworks:** .NET (current LTS release) as the central runtime, supplemented by ASP.NET Core for web APIs.
- **Databases:** Relational databases such as PostgreSQL or Microsoft SQL Server serving as central or module-based persistence layers.
- **Messaging systems:** Asynchronous messaging bus (e.g., RabbitMQ, Apache Kafka, or Azure Service Bus) for decoupled communication between modules and external systems.

### Target Environments

This defines the operational environments in which **KleeneStar** can run and outlines the available deployment options, ranging from classic on-premises installations to modern container orchestration platforms.

- **On-premises:** Fully installable in customer-owned data centers, considering data protection and compliance requirements.
- **Container orchestration:** Support for operation in containerized environments (e.g., Kubernetes, OpenShift, Azure Kubernetes Service) for easy scaling and automated deployment.
- **Supported operating systems:** Primarily Linux-based server environments (e.g., Ubuntu, Red Hat Enterprise Linux) and Windows Server for specific integration scenarios.

Example: An internationally operating enterprise runs **KleeneStar** in a Kubernetes cluster (OpenShift) in its private cloud. The individual core and module containers are deployed via Helm charts; configurations (XML) are mounted as ConfigMaps. Scaling occurs automatically based on load metrics, and updates are applied via rolling update without downtime.

## Integration and Communication Patterns

The integration and communication patterns of **KleeneStar** are designed to enable consistent, secure, and scalable interaction between internal modules and with external systems. They define binding approaches for API design, event processing, data flows, and interface integration.

### API Strategy

**KleeneStar** follows an API-first strategy where all functions are provided via clearly defined, documented interfaces.

- **REST:** Primary paradigm for synchronous, resource-oriented communication and third-party integrations.
- **gRPC:** Binary, low-latency communication between internal services with strict typing.
- **GraphQL:** Optional for flexible queries and avoiding over-/under-fetching. All APIs are versioned, use HTTPS/TLS for transport encryption, and rely on standardized authentication and authorization mechanisms (e.g., OAuth 2.0, OpenID Connect).
- **Versioning:** Implemented via URI paths (e.g., `/api/v1/...`).
- **Security:** All APIs use HTTPS/TLS, authentication via OAuth 2.0 or OpenID Connect, authorization via role-based access control (RBAC).
- **Documentation:** OpenAPI/Swagger definitions are generated automatically and versioned.

Example: A REST endpoint POST `/api/v1/tasks` creates a new task. The client sends JSON data, the server validates it, persists the record, and returns the resource with a unique ID.

### Eventing Mechanisms

For asynchronous communication and loose coupling between modules, **KleeneStar** relies on established eventing patterns:

- **Publish/Subscribe (Pub/Sub):** Events are distributed via a messaging broker so multiple consumers can react independently.
- **Event sourcing:** Important state changes are stored as immutable events to ensure a complete history and reproducibility.
- **Outbox pattern:** For reliable event publication in distributed systems, changes are first persisted in an outbox table and then transactionally forwarded to the event bus.

Example: The Process Management module publishes an IssueCreated event after creating a work item. The Notifications module subscribes to this event and automatically sends an email to the responsible handler.

### Data Flows Between Modules

Data flows within **KleeneStar** are designed to be clearly defined, traceable, and decoupled:

- Modules communicate primarily via APIs or event streams, not via direct database access.
- Data formats are standardized (JSON) to ensure interoperability.
- For complex workflows, either orchestration (central control) or choreography (event-driven self-organization) is used depending on the scenario.

Example: A user uploads a new asset in the Asset Management module. That module emits an `AssetUploaded` event. The Knowledge Management module receives the event, automatically creates a linked knowledge entry, and adds metadata.

### Interfaces to Third-Party Systems

Integration with external systems occurs via clearly defined integration layers:

- Use of open standards and protocols (REST, SOAP, gRPC) for maximum compatibility.
- Use of adapter or connector components to decouple external APIs from internal logic.
- Support for both on-premises systems (e.g., ERP, CRM, DMS) and cloud services.
- Unified security and authentication mechanisms for all external interfaces.

Example: An ERP system is integrated via a REST adapter. When a new customer is created in the ERP, the adapter invokes the **KleeneStar** API POST `/api/v1/customers` to mirror the record in the central platform.

## Conclusion

Through its modular architecture and the consistent separation of core and modules, **KleeneStar** provides a flexible, scalable, and secure platform for knowledge- and project-based work environments. The clear definition of interfaces enables sustainable further development and easy integration of new functionality, while on-premises operation ensures full data sovereignty.

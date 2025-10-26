# Requirements

This document describes the concept for an extensible, web-based on-premises application that consolidates collaborative knowledge management, structured issue processing, and digital asset management into a unified platform. The application will be built on the [WebExpress-Framework](https://github.com/webexpress-framework) to ensure high modularity, scalability, and interoperability.

## Initial Situation

In many companies and organizations, different specialized software solutions are used for knowledge documentation, project control, and the management of digital goods (assets). A typical landscape consists of a wiki system for creating and maintaining articles and documentation, a separate system for tracking tasks and work packages, and another solution for storing and versioning files such as images, documents, or software artifacts. Frequently, there is also a customer portal that provides only a limited view of selected information.

This heterogeneous system landscape leads to significant disadvantages. Data storage is redundant, resulting in inconsistencies and increased maintenance effort. Employees must learn different user interfaces, which lengthens onboarding time and reduces acceptance. The lack of or insufficient integration between systems makes it difficult to obtain a holistic view of projects and knowledge bases. For example, linking an issue to a relevant knowledge article and associated digital assets is often only possible manually via links that can break when changes occur. In addition, implementing an end-to-end search across all systems is complex and error-prone. Managing user rights across multiple systems increases administrative effort and poses a security risk. The need to license, operate, and maintain multiple systems also incurs high costs.

## Objectives

The primary objective of this project is to develop a central, modular platform that increases the efficiency and quality of collaboration in knowledge- and project-based work environments. By eliminating system breaks and creating a unified data and function base, information silos will be dismantled and employee productivity increased.

Specifically, the following goals are pursued:
1. Efficiency gains: A unified user interface and seamless integration of different tools (article creation, issue management, asset management) reduce the workload for employees. Information no longer needs to be copied between systems or linked manually. A powerful, cross-system search enables quick discovery of relevant information, regardless of whether it is a knowledge article, an issue, or an asset.
2. Quality improvement: Centralized data storage ensures the consistency and timeliness of information. Redundancies are avoided, and changes to an information object are immediately available to all relevant tools. Direct linking of issues, documentation, and assets improves traceability of decisions and work steps.
3. Increased flexibility and extensibility: Through a consistent plugin architecture, the platform can be adapted to specific organizational requirements. Customers and third parties should be able to develop and integrate their own modules to create a federated ecosystem of line-of-business applications.
4. Ensuring data sovereignty: On-premises operation allows the organization to retain full control over its data. This is particularly important for organizations with strict data protection and security requirements.
5. Reduction of total cost of ownership (TCO): Consolidating multiple siloed solutions into a single platform based on open source (MIT license) significantly reduces licensing, maintenance, and operating costs.

## Product Description

The product is a web-based application built on the WebExpress framework that serves as a central platform for knowledge management, project control, and asset management. It is characterized by its modular architecture, high configurability, and seamless interoperability of its components.

### Core functions and modules

The platform consists of several core modules (services) that act as separate yet deeply integrated tools. Each module is implemented as a plugin within the **WebExpress** ecosystem.

- Knowledge Management Module: This module enables collaborative creation, editing, and organization of knowledge articles in a hierarchical structure. It offers a rich-text editor, content versioning, commenting features, and fine-grained access control at the space and article level.
- Issue Management Module: This module allows issues, work packages, bugs, or other items to be recorded, assigned, and tracked. Workflows are configurable to map different process models. Issues can be directly linked to knowledge articles and assets.
- Asset Management Module: This module serves as the central repository for storing, versioning, and categorizing digital assets such as documents, images, videos, or code libraries. It provides a preview function and enables linking assets with articles and issues.
- Customer Portal Module: A specialized module that provides external users (customers) with restricted and secure access to selected knowledge articles, issues (e.g., support tickets), and assets. Content visibility is controlled via fine-grained permissions.

### Technical characteristics

- Unified search: A core component is the powerful, unified search function based on the **WebExpress** `IndexManager` and the query language WQL (WebExpress Query Language). Users can run complex, structured queries across all modules, for example, "find all open issues in project X that are linked to knowledge article Y and reference asset Z."
- Plugin architecture: The entire application is built on **WebExpress's** `PluginManager` and `PackageManager` model. Each service is a plugin, which simplifies maintenance, updates, and extension of the platform.
- Identity and access management: The system uses **WebExpress's** `IdentityManager` and supports integration with existing directory services such as LDAP or Active Directory as well as central identity providers (IdPs) via protocols such as OpenID Connect or SAML, enabling integration with Keycloak, Google, Microsoft, or other services.
- Interoperability: Communication and data exchange between modules take place via the defined APIs and the event system of the **WebExpress-Framework**. For example, the issue management module can trigger an event when a task is completed, to which the knowledge management module can respond by updating an article.

## State of the Art and Positioning

The market for collaboration and project management software offers a wide range of solutions that differ in their scope of functions, architecture, and licensing model. The application conceptualized here positions itself as a highly integrated open-source alternative that combines the strengths of established products and addresses their weaknesses through a thoughtful architecture based on the **WebExpress-Framework**.

Commercial, proprietary solutions such as Confluence (for knowledge management) and Jira (for issue management) are market leaders and offer a very extensive range of functions. However, their greatest weakness is that they are two separate products whose integration is possible but license- and configuration-intensive. Data silos remain technically in place, making truly unified search and data consistency difficult. In addition, the on-premises versions are associated with high licensing costs, and customizability is limited to the APIs and add-on marketplaces provided by the vendor. The on-premises versions have been announced as end-of-life as of 2029-03-28.

There are also strong competitors in the open-source space. XWiki is a powerful and extensible wiki platform whose strength lies primarily in knowledge management. Project management functions can be retrofitted via extensions but are not as deeply integrated into the core as in the present concept. OpenProject is a comprehensive project management solution with strong features for both classical and agile methods. However, the knowledge management component is less mature compared to specialized wiki systems. OpenDesk is a promising approach for a suite of open-source applications but focuses heavily on integrating various independent standard software (e.g., Nextcloud, OnlyOffice), which can again lead to a heterogeneous administrative and user landscape.

This conceptualized application stands out from these solutions through several differentiators:
1. Foundational architecture of unity: In contrast to the subsequent integration of separate systems (as with Jira/Confluence or OpenDesk), the solution described here is based on a common foundation-the **WebExpress-Framework**. All modules (knowledge, issues, assets) share the ComponentHub, IdentityManager, and IndexManager. This enables native and deep interoperability that goes beyond simple linking.
2. Powerful, unified query language (WQL): While other systems often provide only simple full-text searches or specific filter dialogs, WQL offers a consistent and learnable language for users to execute complex, structured queries across all data objects. This is a decisive advantage over separate searches in different systems.
3. Lightweight and high-performance plugin development: Because the entire framework is based on C#, developers can create performant and secure plugins using a unified set of tools and language skills, without having to deal with different technology stacks (e.g., Java, PHP, JavaScript frameworks).

## Benefits and Market Opportunities

The development and provision of the described platform provide significant benefits for users and open up promising market opportunities, especially in segments that value data sovereignty, adaptability, and cost efficiency.

Strategic benefits:
The greatest benefit lies in creating a "single source of truth." By eliminating data silos, the quality and availability of information are drastically increased. Decisions can be made on a more consistent and comprehensive data basis. The seamless linking of strategic goals (documented in the knowledge base), operational tasks (mapped in issue management), and related resources (managed in asset management) enables a 360-degree view of all organizational activities. This fosters a more agile and transparent work culture. The open-source nature and plugin architecture enable the development of a federated ecosystem. Organizations can use a base installation and gradually expand it with their own or community-provided line-of-business applications, all based on the same technology and interaction paradigm. This reduces vendor lock-in and fosters innovation.

Market opportunities:
The market for collaboration software is growing steadily, but there is increasing demand for alternatives to dominant cloud providers. Many organizations-particularly in the European region (keyword GDPR), in the public sector, or in regulated industries (e.g., finance, healthcare)-are actively looking for powerful on-premises solutions. The platform conceptualized here serves this niche exactly.
The MIT license is a strong market driver. It allows organizations to use, customize, and even integrate the software into commercial products free of charge. This significantly lowers entry barriers and promotes broad adoption. Market opportunities arise from offering professional services around the platform, such as implementation support, individual customizations, development of specialized plugins, training, and enterprise support contracts. The combination of a free core product and paid services is a proven and successful business model in the open-source environment. The focus on a modern, unified technology (C#/.NET) also makes the platform attractive to a large developer community, accelerating further development and the availability of extensions.

## Constraints

Successful implementation of the project is subject to several technical and organizational constraints, which are largely defined by the choice of the WebExpress framework and the project goals.

Technical constraints:
1. **WebExpress-Framework** as the foundation: The entire application architecture must consistently align with the models of the **WebExpress-Framework**. This specifically concerns the implementation of core functions as plugins (`IPlugin`), the definition of applications (`IApplication`), and the use of central managers such as `PackageManager`, `ApplicationManager`, and `IdentityManager`. Development must be carried out in C#, ensuring a homogeneous technological basis.
2. Data modeling for the IndexManager: To realize the powerful, unified search via WQL, all central data objects (knowledge articles, issues, assets, comments, etc.) must implement the `IIndexItem` interface. The fields to be indexed must be carefully selected and, if necessary, annotated with attributes such as `[IndexIgnore]` to achieve an optimal balance between search depth and index size.
3. Authentication and authorization: External identity services (LDAP/AD, IdPs) are centrally integrated via the `IdentityManager`. Access controls are consistently configured in classes (`IResource`, `IPage`, `IFragment`) via `[Authorization]` attributes. Roles and permissions must be defined uniformly across modules and consistently with the `IdentityManager`.
4. Scalability for large data volumes: Although **WebExpress-Framework** is designed to be lightweight, the application must be designed for large data volumes and many users. This requires careful database planning (the framework itself is database-agnostic) and efficient use of caching mechanisms that **WebExpress-Framework** provides via the `[Cache]` attribute for resources and fragments. Asynchronous operations for long-running processes (e.g., indexing large data volumes) should be handled via the **WebExpress-Framework** `TaskManager` model.

Organizational constraints:
1. Open-source development model: The project will be developed under the MIT license, which requires transparency and community involvement. All source code will be managed in a public Git repository. Contributions from external developers will be integrated via pull requests following a defined review process.
2. Documentation: Comprehensive technical documentation, especially for plugin development, is crucial for building an active developer community. This must be created and maintained in parallel with development.

## Non-Functional Requirements

In addition to functional features, the platform must meet a set of non-functional requirements to ensure secure, performant, highly available, and sustainably maintainable operation. These requirements define quality characteristics that are crucial for stability, usability, and future viability:

- Security: End-to-end transport encryption (TLS), hardening of default configurations, secure defaults for permissions, and audit logging of security-relevant actions.
- Performance: Short response times for typical user actions, efficient indexing, and incremental updates of the search index.
- Availability: Support for rolling updates, optional active/passive redundancy, and defined recovery strategies after outages.
- Maintainability: Clear plugin interfaces, semantic versioning, automated unit and integration tests, and a structured deprecation strategy.
- Internationalization/accessibility: Multilingual support and compliance with common accessibility guidelines.

## Risks and Assumptions

Project success depends largely on the successful establishment and long-term activation of an engaged developer community. Key risks lie in a potentially incomplete functional parity with established single-purpose solutions in the initial phase and in potentially high efforts required for migrating existing systems and data. It is assumed that sufficient personnel and time resources are available to produce and maintain comprehensive documentation, to actively manage the community, and to ensure continuous quality assurance.

## Roadmap

The roadmap outlines the planned development steps of the platform and serves as a strategic guide for the next expansion stages. It prioritizes implementation over time horizons and ensures that core functions are available early, while medium- and long-term advanced features and innovations follow. This creates a clear plan that keeps both short-term results and sustainable further development in view.

- Short term: Focus on implementing the core modules knowledge, issues, and assets as well as providing the WQL base language. In parallel, integrate external identity services and introduce a consistent role and permission model. As a visible deliverable for end users, provide a customer portal MVP to gather early feedback from practice.
- Medium term: Expand WQL functionality with join-like queries and ranking mechanisms. In addition, implement audit and compliance features to meet regulatory requirements. Export and import interfaces and well-defined migration paths from common systems will facilitate the adoption of existing datasets.
- Long term: Use AI-powered search and intelligent recommendations to increase efficiency. Automatic content summaries and real-time collaboration in editors will improve productivity. The platform will be extended with additional modules such as CRM or requirements management to open up new use cases.

## Conclusion and Outlook

The concept describes a promising solution to the widespread problems of fragmented system landscapes in organizations. By consistently using the **WebExpress-Framework** as a unified technological foundation, a platform is created that combines the strengths of specialized tools for knowledge, project, and asset management in a single, highly integrated application. The differentiators-especially native modularity, the end-to-end WQL query language, and the modern, homogeneous technology base-strongly position the product against existing commercial and open-source solutions. The focus on on-premises operation and the permissive MIT license serve an important and growing market niche.

The realization of this project offers the opportunity to establish not just a product but an extensible ecosystem. The greatest challenge will be to attract an active developer community that advances the platform by creating new plugins and evolving the core.

Future development steps could focus on extending the platform with additional modules. For example, a CRM module for managing customer relationships or a module for requirements management would be conceivable. Another important area is improving artificial intelligence within the platform. By integrating AI services, search results could be enhanced, automatic summaries of articles and issues created, or relevant information proactively suggested. Introducing real-time collaboration features in editors (similar to Google Docs) would also be a significant added value. In the long term, the platform could be expanded into a comprehensive "digital workplace" solution that serves as the central hub for digital collaboration in organizations. Success will largely depend on the quality of the initial core modules and the ability to build an engaged community around the project.
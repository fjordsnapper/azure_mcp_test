# Agentic DevOps Framework

> **Note**: This README file was generated using AI assistants (Windsurf/Cascade + Claude Opus) as part of the framework's demonstration of AI-assisted development practices.

An enterprise framework that standardizes how teams design, validate, and deploy software using automation and clear guardrails—improving quality, speed, and compliance across projects.

## Overview

The Agentic DevOps Framework provides a comprehensive approach to enterprise software development that emphasizes:

- **Architecture Excellence**: Standardized patterns that reduce technical debt
- **Quality Assurance Automation**: Automated gates that prevent defects from reaching production
- **Time Savings & Efficiency**: 70-90% reduction in manual deployment and testing activities
- **Enterprise Compliance**: Automated governance and security enforcement
- **Cloud Hosting Agnostic**: Framework works seamlessly across all major cloud providers and on-premises environments

## Artefacts Produced

### Repository Templates
- Standardized repository structure with comprehensive README/docs
- Architectural Decision Records (ADRs) templates
- Consistent branching and commit conventions
- Dependency and versioning approaches

### Development Toolchain
- Pre-commit hooks for local validation (formatting, linting, tests, secret detection)
- IDE configurations and productivity enhancers
- Language-specific tool integrations

### CI/CD Pipeline Templates
- Automated testing and quality gates
- Security scanning and vulnerability assessment
- Deployment automation with environment promotion
- Performance monitoring and regression detection

### Security & Compliance
- Vulnerability scanning rules and configurations
- Secret scanning and prevention mechanisms
- Audit-friendly compliance checks
- Security policy templates

### Platform Templates
- Docker multi-stage build patterns
- Cloud deployment templates (AWS, GCP, Azure, and other major providers)
- Observability and monitoring baselines
- Infrastructure as code patterns

## How to Use This Framework

### Recommended AI Assistant Setup

**Best results are achieved with:**
- **Primary IDE**: Windsurf with Cascade
- **Support Model**: Claude Opus
- **MCP Server**: context7 for comprehensive code and cloud documentation
  - **Configuration**: Must be configured in Windsurf settings to enable MCP server integration
  - **Project-Level Settings**: Can use project-level JSON files (`.windsurf/settings.json`) for team consistency
  - **Setup**: Add context7 MCP server to Windsurf's MCP configuration
- **Alternative**: Any modern IDE with AI assistant capabilities

### Prerequisites

Before using this framework, ensure the following are in place:

- **CLI-Level Cloud API Access**: Authenticated access to cloud provider CLI tools (AWS CLI, Azure CLI, gcloud, etc.)
- **context7 MCP Server**: context7 MCP server available for comprehensive code and cloud vendor documentation
- **Development Environment**: Required tools installed and configured with pre-commit hooks
- **Service Principal Authentication**: OIDC federated credentials or service principals with deployment permissions

### Getting Started

#### 1. Framework Adoption
1. **Read the Foundation**: Start with `framework.md` to understand the principles and methodology
2. **Review Rules**: Examine `rules/conventions and guardrails.md` for specific implementation guidelines
3. **Choose Templates**: Select appropriate repository and CI/CD templates for your technology stack

#### 2. Project Setup
```bash
# Clone a template repository
git clone <template-repo> your-project
cd your-project

# Initialize with framework conventions
# (Automated setup scripts coming soon)
```

#### 3. Configure Development Environment
- Install pre-commit hooks for local validation
- Configure IDE with recommended extensions
- Set up automated formatting and linting

#### 4. Implement CI/CD
- Choose appropriate CI/CD template for your platform
- Configure quality gates and security scanning
- Set up automated deployment pipelines

### Development Workflow

#### Work Item Management
1. **Create Issues First**: Always create GitHub Issues before starting work
2. **Link Commits**: Reference issue numbers in commit messages
3. **Track Metrics**: Log agentic lead time and cycle time where supported

#### Git Workflow
- **Trunk-based Development**: Work with short-lived feature branches
- **Branch Naming**: Use `feature/`, `fix/`, `security/`, `chore/`, `docs/` prefixes
- **Conventional Commits**: Follow `type: description` format

#### Quality Gates
- **Local Validation**: Pre-commit hooks ensure code quality before commits
- **Automated Testing**: Comprehensive test execution on every change
- **Security Scanning**: Continuous vulnerability assessment
- **Coverage Requirements**: Minimum 80% test coverage baseline

### Technology-Specific Implementation

The framework supports multiple technology stacks with specific implementations:

#### .NET Ecosystem
- Testing: xUnit with Coverlet
- Security: Gitleaks, custom PowerShell scripts
- Quality: dotnet format, SonarQube integration
- CI/CD: GitHub Actions or other CI/CD platforms

#### JavaScript/TypeScript
- Testing: Jest with coverage reporting
- Security: npm audit, Snyk vulnerability scanning
- Quality: ESLint, Prettier, TypeScript strict mode
- CI/CD: GitHub Actions with Node.js runners

#### Python
- Testing: pytest with coverage.py
- Security: bandit static analysis, safety dependency scanning
- Quality: black formatting, mypy type checking
- CI/CD: GitHub Actions with Python runners

#### Java
- Testing: JUnit with JaCoCo coverage
- Security: SpotBugs, OWASP Dependency Check
- Quality: Checkstyle, PMD, SpotBugs
- CI/CD: GitHub Actions with Java runners

### Continuous Improvement

#### Metrics to Track
- **Quality Metrics**: Test coverage, defect rates, security vulnerabilities
- **Efficiency Metrics**: Deployment time, manual intervention reduction
- **Adoption Metrics**: Team usage, satisfaction, productivity improvements
- **Business Metrics**: Time-to-market, cost reduction, customer satisfaction

#### Framework Evolution
- Collect feedback from implementation teams
- Refine based on real-world usage patterns
- Update templates and rules continuously
- Share improvements across the organization

## Implementation Strategy

### Adoption Path
1. **Foundation**: Establish core principles and basic guardrails
2. **Automation**: Implement automated enforcement and validation
3. **Integration**: Seamlessly integrate with existing enterprise processes
4. **Optimization**: Refine based on real-world usage and metrics

### Success Factors
- **Executive Support**: Leadership buy-in for framework adoption
- **Team Training**: Comprehensive onboarding and education
- **Tool Integration**: Seamless integration with existing toolchains
- **Continuous Feedback**: Regular collection and implementation of feedback

## Support and Resources

### Documentation Structure
- `framework.md`: Comprehensive framework documentation
- `rules/`: Implementation conventions and guardrails
- Platform-specific setup guides for various cloud providers
- Template repositories: Ready-to-use project templates

### Getting Help
- Review the comprehensive documentation in this repository
- Check technology-specific implementation guides
- Consult with your DevOps team for enterprise-specific requirements
- Use the AI assistants (Windsurf/Cascade + Claude Opus) for guidance

---

**This framework represents a strategic approach to software development that emphasizes principles over prescriptions, automation over manual processes, and continuous improvement over static standards. The goal is to create a living framework that evolves with technology and business needs while maintaining high quality and security standards across any programming language or platform.**

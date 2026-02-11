# AI DevOps Starter Ruleset

Global rules for AI-assisted DevOps workflows.

## Work Item Tracking

1. **Always create GitHub Issues before starting work**
   - Create an issue describing the task before making changes
   - Use clear, descriptive titles
   - Include acceptance criteria where applicable
   - Maintain a certain level of abstraction to prevent creation of too finely grained work-items. F.eks `implement user class + controller`, should not be split into work items for each crud operation.
   - Identify functional requirements and batch together related commits where possible to prevent a `one work-item per commit situation`
   - Use development platforms language tooling / scaffolding where possible

2. **Link commits and PRs to issues**
   - Reference issue numbers in commit messages (e.g., `fix: Resolve binding issue #53`)
   - Link PRs to issues in the PR description
   - Ask to create retroactive issues for any work done without upfront tracking

3. **Close issues with comments**
   - Add a closing comment summarizing what was done
   - Reference the commits/PRs that resolved the issue

## Git Workflow

1. **Branch strategy - Trunk-based development**
   - `main` - Production branch (single source of truth, always deployable)
   - Short-lived feature/fix branches
   - Avoid long-lived environment branches (test, staging, deploy) - this is an anti-pattern
   - Environment promotion is handled through CI/CD pipeline, not branches
   - Reduces configuration drift and merge conflicts
   - Merges flow: feature/fix branches → main

2. **Branch naming conventions**
   - `feature/` - New features
   - `fix/` - Bug fixes
   - `security/` - Security-related changes
   - `chore/` - Maintenance tasks
   - `docs/` - Documentation updates

3. **Commit message format**
   - Use conventional commits: `type: description`
   - Types: `feat`, `fix`, `docs`, `chore`, `security`, `ci`, `refactor`, `test`
   - Keep messages concise but descriptive

4. **Verification before pushing**
   - Always verify changes locally before pushing to remote
   - Run local tests and builds to ensure changes work
   - Review git diff to confirm all changes are intentional
   - Wait for local verification to complete before pushing
   - Never push changes that haven't been tested locally

5. **Pipeline verification after pushing**
   - After pushing to remote, wait 60 seconds for pipeline to start
   - Monitor pipeline status using `gh run list` and `gh run view`
   - Wait for pipeline to complete (either succeed or fail)
   - Do not proceed with further work until pipeline status is determined
   - If pipeline fails, diagnose and fix issues before proceeding
   - Only move forward after pipeline succeeds
   - Use git merge squash for feature changes, when long diagnose / commit cycles create excessively large commit history

6. **Pre-push checks**
   - All commits must pass the local defined unit tests and checks
   - **Secret scanning is mandatory**: Both Gitleaks and TruffleHog must pass
   - For rust projects this is currently `cargo fmt`, `cargo clippy`, and `cargo test`
   - For .NET projects this is currently `dotnet build`, `dotnet test`, and `dotnet format`
   - For python projects this is currently `black`, `isort`, and `pytest`
   - For node.js projects this is currently `npm run build`, `npm run test`, and `npm run format`
   - For go projects this is currently `go fmt`, `go test`, and `go vet`
   - For java projects this is currently `./gradlew build`, `./gradlew test`, and `./gradlew spotlessCheck`
   - For php projects this is currently `php-cs-fixer`, `phpstan`, and `phpunit`
   - For ruby projects this is currently `rubocop`, `rspec`, and `reek`
   - For scala projects this is currently `sbt compile`, `sbt test`, and `sbt scalafmt`
   - For kotlin projects this is currently `./gradlew build`, `./gradlew test`, and `./gradlew spotlessCheck`

   - Git hooks enforce this automatically
   - **Secret scanning failure blocks all pushes** - No exceptions

7. **Auto vs Manual Confirmation After Push**
   - After each successful push, the developer must choose between auto-pilot or manual confirmation mode
   - **Auto-pilot mode**: Continue with next development tasks automatically without manual intervention
     - AI assistant proceeds with next logical steps based on current work item
     - Continues until hitting a blocking issue or completing the work item
     - Ideal for routine tasks, well-defined work items, or when developer is confident in the direction
   - **Manual confirmation mode**: Require explicit approval before each subsequent action
     - AI assistant presents proposed next steps and waits for developer confirmation
     - Developer reviews and approves/denies each action before execution
     - Ideal for complex tasks, critical changes, or when developer wants tighter control
   - **Decision point**: After each push, AI assistant must ask: "Continue in auto-pilot mode or switch to manual confirmation?"
   - **Mode switching**: Can switch between modes at any time, not just after pushes
   - **Default behavior**: Start new work items in manual confirmation mode for safety
   - **Exception handling**: Auto-pilot mode automatically pauses on errors, conflicts, or security warnings

8. **Protected branches with gated check-ins**
   - All branches require pull request review (minimum 1 approval)
   - Requires all status checks to pass (build, tests, security scan)
   - Requires code review before merge
   - Requires up-to-date branch before merge
   - Dismiss stale pull request approvals when new commits are pushed
   - No direct pushes to protected branches allowed

9. **Pull request requirements**
   - All PRs must reference a GitHub issue
   - PR title must follow conventional commits format
   - PR description must include acceptance criteria
   - All conversations must be resolved before merge
   - At least one approval required before merge

## Security

1. **Never commit secrets**
   - No API keys, passwords, or tokens in code
   - Use environment variables for sensitive configuration
   - Keep `.env` files gitignored
   - Scan for secrets retroactively and advise

2. **Sensitive files to exclude**
   - Certificates and keys (`*.key`, `*.pem`, `*.p12`, `*.pfx`)
   - Database files (`*.db`, `*.sqlite`)
   - Log files and artifacts

3. **If secrets are accidentally committed**
   - Ask to use BFG Repo-Cleaner to remove from history
   - Ask to rotate compromised credentials
   - Push cleaned history to all branches when approved

4. **Local Secret Scanning Tools**
   - **Gitleaks**: Primary secret scanner for local development
     - Run `gitleaks detect` before each commit
     - Configured with custom workflow definitions for custom rules
     - Integrates with pre-commit hooks automatically
     - Scans for 200+ secret patterns including API keys, tokens, certificates
   - **TruffleHog**: Secondary secret scanner for comprehensive coverage
     - Run `trufflehog filesystem .` before pushing to remote
     - Uses entropy-based detection to find unknown secret patterns
     - Complements Gitleaks by finding patterns that might be missed
     - Particularly effective for custom-encoded secrets
   - **Pre-commit Integration**: Both tools run automatically via git hooks
     - Gitleaks runs on every commit attempt
     - TruffleHog runs on every push attempt
     - Commit is blocked if any secrets are detected
     - Developer must remediate secrets before proceeding
   - **Configuration Requirements**
     - Install both tools locally: `brew install gitleaks trufflehog` (macOS) or equivalent
     - Ensure pre-commit hooks are configured in workflow definitions
     - Update tool configurations when new secret patterns are identified
   - **False Positives**: Document known false positives in tool configuration exceptions

## CI/CD

1. **GitHub Environments**
   - `dev` - Local development settings
   - `test` - CI testing (ZAP scan, unit tests)
   - `staging` - Pre-production deployment
   - `prod` - Production deployment

2. **Environment variables**
   - Store in GitHub environment settings, not in code
   - Mirror local `.env.example` structure
   - Document required variables in README

3. **Cloud authentication**
   - Use OIDC federated credentials (no stored secrets)
   - Service principal needs appropriate permissions for deployment and container registry

## Performance & Optimization

1. **Data type optimization**
   - Analyze all data types for unnecessary memory usage
   - Use smaller types when appropriate (e.g., `short` instead of `int` for limited ranges)
   - Document memory savings in commit messages
   - Example: User IDs using `short` (2 bytes) instead of `int` (4 bytes) = 50% reduction

2. **Benchmarking requirements**
   - Create benchmarks for performance-critical code paths
   - Use BenchmarkDotNet for .NET projects
   - Compare optimized vs legacy implementations
   - Include memory allocation diagnostics
   - Run benchmarks separately from unit tests (they are resource-intensive)
   - Document performance improvements in release notes

3. **Feature flags for optimizations**
   - Implement feature flags to toggle optimizations in production
   - Allow rollback to legacy implementations without code changes
   - Configuration-driven via `appsettings.json` per environment
   - Enable A/B testing of performance improvements
   - Example: `UseOptimizedDataTypes` flag for toggling data type optimizations

## Cost Optimization & Cloud Economics

1. **Always choose the cheapest viable solution**
   - Use container instances instead of platform-as-a-service (pay-per-second vs hourly)
   - Container Instances: ~$5-10/month for dev environments
   - PaaS Free tier: Limited, PaaS B1: ~$15/month
   - Estimated savings: 50-70% by using Container Instances

2. **Resource sizing**
   - Start with minimum resources: 1 CPU core, 1 GB RAM
   - Scale up only after monitoring shows need
   - Use auto-scaling based on actual demand
   - Monitor cost impact of each resource increase

3. **Consumption-based pricing**
   - Prefer services with pay-per-use billing (Container Instances, Functions)
   - Avoid always-on services (App Service) for non-production
   - Use spot instances and reserved capacity for production

4. **Cost monitoring and alerts**
   - Integrate cloud monitoring tools for cost tracking
   - Set up cloud provider cost management alerts
   - Review costs weekly during development
   - Track cost impact of feature flags and optimizations

5. **Infrastructure as Code cost optimization**
   - Document estimated monthly costs in Bicep outputs
   - Include cost estimates in deployment summaries
   - Review Bicep templates for cost-saving opportunities
   - Use parameter files to switch between cost tiers per environment

6. **Development environment guidelines**
   - Dev: Container Instances (cheapest, ~$5-10/month)
   - Staging: App Service B1 or Container Instances (~$15-20/month)
   - Production: App Service B2+ with auto-scaling (cost varies)
   - Never use premium tiers in dev/staging unless required

## Quality Gates & Deployment Guardrails

1. **Mandatory checks before any deployment**
   - All unit tests must pass (100% pass rate required)
   - Code must build successfully with no errors
   - Code formatting must pass (no style violations)
   - Static analysis tools must pass (linting, type checking)
   - **Deployment is blocked if any check fails**

2. **Security scanning requirements**
   - ZAP (OWASP ZAP) security scan must complete for all deployments
   - No critical vulnerabilities allowed (CVSS 9.0+)
   - No high-severity vulnerabilities allowed (CVSS 7.0-8.9) without documented exception
   - Medium vulnerabilities must be documented in release notes
   - **Deployment is blocked if critical vulnerabilities are found**

3. **Code coverage requirements**
   - Minimum 80% code coverage required for staging/prod deployments
   - New code must maintain or improve coverage percentage
   - Coverage reports must be generated and archived
   - **Deployment is blocked if coverage falls below threshold**

4. **Deployment stage gates**
   - Dev deployment: Requires passing unit tests only
   - Staging deployment: Requires unit tests + ZAP scan + 80% coverage
   - Production deployment: Requires all above + manual approval + health check verification

5. **Failure notifications**
   - Failed checks must generate alerts to team
   - Failure reports must include root cause and remediation steps
   - Failed deployments must be logged with timestamps and responsible party
   - Automatic rollback triggers if health checks fail post-deployment

## Infrastructure as Code

1. **Bicep/Terraform**
   - Keep IaC templates in `infra/` directory
   - Use parameter files for environment-specific values
   - Run `plan` before `apply`

2. **Docker**
   - Use multi-stage builds for smaller images
   - Pin base image versions
   - Include health checks

## Deployment

1. **Environment promotion through CI/CD (not branches)**
   - Environment promotion is driven by CI/CD pipeline, not by branches
   - Single codebase deployed to multiple environments via pipeline stages
   - Configuration and secrets managed per-environment in GitHub Environments
   - No long-lived environment branches (test, staging, deploy)

2. **Deployment pipeline stages**
   - Dev: Automatic deployment on `feature` branch push
   - Staging: Automatic deployment on `main` branch push (after dev succeeds)
   - Production: Manual approval required on `main` branch push

3. **Staging first**
   - Always deploy to staging before production
   - Verify health checks pass
   - Use deployment slots for zero-downtime deployments

4. **Manual triggers for production**
   - Deployment workflows should be manually triggered
   - Require approval for production deployments

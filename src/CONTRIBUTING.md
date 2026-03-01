# Contributing Guidelines

This repository enforces strict coding standards and project conventions. All contributions must comply with the rules and configuration in the project's `.editorconfig` file and the guidelines below.

## Guidelines

- Follow the rules in `.editorconfig` exactly (indentation, line endings, var usage, expression-bodied members, namespace and file layout, accessibility modifiers, etc.).
- Target framework: .NET 8.
- Keep generated or modified code copy-paste ready — do not leave placeholders.
- Use explicit types rather than `var` unless the type is obvious and permitted by `.editorconfig`.
- Add accessibility modifiers for all non-interface members.
- Keep method bodies clear and prefer small helper methods for readability.
- Keep public API surface minimal and documented.

## Formatting and Style

- Indent using 4 spaces.
- Do not insert a final newline at the end of files (per `.editorconfig`).
- Sort using directives according to repository rules.

## Testing

- Include unit tests for new or changed behavior where feasible.

## Pull Requests

- Rebase or squash commits into a clear, small set of changes per PR.
- Include a short description of the change and why it was made.
- Ensure the code builds and runs locally on .NET 8.

## Automation

- CI should run build and tests; fix issues raised by CI before merging.

## Contact

- For questions about style or architecture, open an issue or reach out on the repository's issue tracker.
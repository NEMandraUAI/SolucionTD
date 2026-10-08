# Project Rules

This project is a C# Windows Forms application.

## Architecture protection

This modernization task is strictly limited to the presentation layer.

NEVER modify:
- BE classes
- BLL classes
- DAL classes
- Mapper classes
- Security classes
- business logic
- database logic
- SQL queries
- repositories
- services
- application configuration
- existing event handler logic
- existing public methods
- existing private methods
- existing interfaces
- existing models
- existing entity classes

Do not rename existing classes, methods, controls, fields, properties or events.

Do not change application behavior.

## UI modernization rules

The goal is to modernize the visual appearance of the existing Windows Forms application.

Allowed:
- *.Designer.cs files
- visual properties serialized by the Windows Forms Designer
- layout properties
- sizes and positions
- fonts
- colors
- backgrounds
- borders
- padding
- margins
- alignment
- anchoring
- docking
- control styles
- DataGridView visual properties
- MenuStrip visual properties
- Form visual properties
- Panel visual properties
- GroupBox visual properties
- Button visual properties
- Label visual properties
- TextBox visual properties
- ComboBox visual properties
- CheckBox visual properties
- RadioButton visual properties
- PictureBox visual properties
- visual resources when strictly required for the interface

Do NOT implement visual features by adding runtime code to normal .cs files.

Do NOT create theme managers, helper classes, custom controls, utility classes or rendering frameworks.

Do NOT move UI logic into other files.

Do NOT add event handlers.

The application must continue to behave exactly as before.

## Windows Forms safety

Keep the Windows Forms Designer functional.

Prefer standard WinForms controls and properties that can be serialized safely by the Designer.

Do not introduce unsupported custom properties into Designer-generated code.

Do not remove controls or events.

Do not change control names.

## Validation

After modifications:
1. Build the solution.
2. Check for compiler errors.
3. Check that all protected architectural files remain unchanged.
4. Review the Git diff.
5. Report every modified file.
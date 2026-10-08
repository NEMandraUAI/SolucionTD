---
applyTo: "**/*.Designer.cs"
---

# WinForms Designer Rules

These files contain Windows Forms Designer-generated code.

For UI modernization:

- Only modify visual and layout properties.
- Preserve every existing control.
- Preserve every existing control name.
- Preserve every existing event subscription.
- Preserve every existing component.
- Preserve the existing control hierarchy unless a purely visual layout correction requires otherwise.
- Do not modify event handlers.
- Do not modify business logic.
- Do not create new application logic.
- Do not reference BE, BLL, DAL or other architectural layers.
- Do not add helper methods.
- Do not add custom rendering logic.
- Do not create custom controls.
- Keep the Designer functional.

Prefer standard serializable WinForms properties.

Examples of allowed modifications:
- BackColor
- ForeColor
- Font
- Size
- Location
- Dock
- Anchor
- Padding
- Margin
- TextAlign
- FlatStyle
- BorderStyle
- RowHeadersVisible
- ColumnHeadersDefaultCellStyle
- DefaultCellStyle
- SelectionBackColor
- SelectionForeColor
- FormBorderStyle
- StartPosition
- Icon
- BackgroundImage
- BackgroundImageLayout

The final result must preserve the exact application behavior.
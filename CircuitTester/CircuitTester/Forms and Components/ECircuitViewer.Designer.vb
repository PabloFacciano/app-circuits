<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ECircuitViewer
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        CircuitContextMenu = New ContextMenuStrip(components)
        BtnDuplicate = New ToolStripMenuItem()
        BtnDelete = New ToolStripMenuItem()
        Separator1 = New ToolStripSeparator()
        BtnSelectAll = New ToolStripMenuItem()
        BtnUnselectAll = New ToolStripMenuItem()
        CircuitContextMenu.SuspendLayout()
        SuspendLayout()
        ' 
        ' CircuitContextMenu
        ' 
        CircuitContextMenu.Items.AddRange(New ToolStripItem() {BtnDuplicate, BtnDelete, Separator1, BtnSelectAll, BtnUnselectAll})
        CircuitContextMenu.Name = "CircuitContextMenu"
        CircuitContextMenu.Size = New Size(137, 98)
        ' 
        ' BtnDuplicate
        ' 
        BtnDuplicate.Image = My.Resources.Resources.duplicate
        BtnDuplicate.Name = "BtnDuplicate"
        BtnDuplicate.Size = New Size(136, 22)
        BtnDuplicate.Text = "&Duplicate"
        ' 
        ' BtnDelete
        ' 
        BtnDelete.Image = My.Resources.Resources.delete
        BtnDelete.Name = "BtnDelete"
        BtnDelete.Size = New Size(136, 22)
        BtnDelete.Text = "D&elete"
        ' 
        ' Separator1
        ' 
        Separator1.Name = "Separator1"
        Separator1.Size = New Size(133, 6)
        ' 
        ' BtnSelectAll
        ' 
        BtnSelectAll.Name = "BtnSelectAll"
        BtnSelectAll.Size = New Size(136, 22)
        BtnSelectAll.Text = "&Select All"
        ' 
        ' BtnUnselectAll
        ' 
        BtnUnselectAll.Name = "BtnUnselectAll"
        BtnUnselectAll.Size = New Size(136, 22)
        BtnUnselectAll.Text = "&Unselect All"
        ' 
        ' ECircuitViewer
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ContextMenuStrip = CircuitContextMenu
        Name = "ECircuitViewer"
        Size = New Size(300, 150)
        CircuitContextMenu.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents CircuitContextMenu As ContextMenuStrip
    Friend WithEvents BtnDuplicate As ToolStripMenuItem
    Friend WithEvents BtnDelete As ToolStripMenuItem
    Friend WithEvents Separator1 As ToolStripSeparator
    Friend WithEvents BtnSelectAll As ToolStripMenuItem
    Friend WithEvents BtnUnselectAll As ToolStripMenuItem
End Class

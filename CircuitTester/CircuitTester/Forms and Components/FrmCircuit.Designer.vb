<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCircuit
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        PanelContent = New Panel()
        CircuitViewer = New ECircuitViewer()
        PanelTop = New FlowLayoutPanel()
        BtnZoom = New RadioButton()
        BtnEdit = New RadioButton()
        Label1 = New Label()
        BtnPlay = New Button()
        BtnPlayStep = New Button()
        BtnPause = New Button()
        BtnStop = New Button()
        BtnConfig = New Button()
        PanelTopLine = New Panel()
        PanelLeft = New Panel()
        Panel2 = New Panel()
        LeftSideList = New ListBox()
        LeftSideSelect = New ComboBox()
        Panel1 = New Panel()
        TimerGO = New Timer(components)
        PanelRight = New Panel()
        PropertyGrid = New PropertyGrid()
        Panel4 = New Panel()
        StatusStrip1 = New StatusStrip()
        RunningTimeLabel = New ToolStripStatusLabel()
        PanelContent.SuspendLayout()
        PanelTop.SuspendLayout()
        PanelLeft.SuspendLayout()
        Panel2.SuspendLayout()
        PanelRight.SuspendLayout()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PanelContent
        ' 
        PanelContent.Controls.Add(CircuitViewer)
        PanelContent.Dock = DockStyle.Fill
        PanelContent.Location = New Point(200, 52)
        PanelContent.Name = "PanelContent"
        PanelContent.Size = New Size(508, 485)
        PanelContent.TabIndex = 2
        ' 
        ' CircuitViewer
        ' 
        CircuitViewer.BackColor = SystemColors.Control
        CircuitViewer.Circuit = Nothing
        CircuitViewer.CircuitGridColor = SystemColors.ControlLight
        CircuitViewer.CircuitGridSize = 30F
        CircuitViewer.CircuitOrigin = New Point(0, 0)
        CircuitViewer.CircuitSnapToGrid = True
        CircuitViewer.CircuitTemporalWireColor = Color.FromArgb(CByte(128), CByte(0), CByte(0), CByte(0))
        CircuitViewer.CircuitViewMode = ECircuitViewer.ViewerMode.Zoom
        CircuitViewer.CircuitZoom = 1F
        CircuitViewer.Dock = DockStyle.Fill
        CircuitViewer.Location = New Point(0, 0)
        CircuitViewer.Name = "CircuitViewer"
        CircuitViewer.Size = New Size(508, 485)
        CircuitViewer.TabIndex = 2
        ' 
        ' PanelTop
        ' 
        PanelTop.AutoSize = True
        PanelTop.AutoSizeMode = AutoSizeMode.GrowAndShrink
        PanelTop.Controls.Add(BtnZoom)
        PanelTop.Controls.Add(BtnEdit)
        PanelTop.Controls.Add(Label1)
        PanelTop.Controls.Add(BtnPlay)
        PanelTop.Controls.Add(BtnPlayStep)
        PanelTop.Controls.Add(BtnPause)
        PanelTop.Controls.Add(BtnStop)
        PanelTop.Controls.Add(BtnConfig)
        PanelTop.Dock = DockStyle.Top
        PanelTop.Location = New Point(0, 0)
        PanelTop.Name = "PanelTop"
        PanelTop.Padding = New Padding(3, 0, 0, 0)
        PanelTop.Size = New Size(1008, 51)
        PanelTop.TabIndex = 5
        ' 
        ' BtnZoom
        ' 
        BtnZoom.Appearance = Appearance.Button
        BtnZoom.BackgroundImage = My.Resources.Resources.zoom
        BtnZoom.BackgroundImageLayout = ImageLayout.Zoom
        BtnZoom.Location = New Point(6, 3)
        BtnZoom.Name = "BtnZoom"
        BtnZoom.Size = New Size(45, 45)
        BtnZoom.TabIndex = 3
        BtnZoom.UseVisualStyleBackColor = True
        ' 
        ' BtnEdit
        ' 
        BtnEdit.Appearance = Appearance.Button
        BtnEdit.BackgroundImage = My.Resources.Resources.edit
        BtnEdit.BackgroundImageLayout = ImageLayout.Zoom
        BtnEdit.Checked = True
        BtnEdit.Location = New Point(54, 3)
        BtnEdit.Margin = New Padding(0, 3, 3, 3)
        BtnEdit.Name = "BtnEdit"
        BtnEdit.Size = New Size(45, 45)
        BtnEdit.TabIndex = 4
        BtnEdit.TabStop = True
        BtnEdit.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point)
        Label1.Location = New Point(102, 3)
        Label1.Margin = New Padding(0, 3, 3, 3)
        Label1.Name = "Label1"
        Label1.Size = New Size(150, 45)
        Label1.TabIndex = 12
        Label1.Text = "STOP"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' BtnPlay
        ' 
        BtnPlay.BackgroundImage = My.Resources.Resources.play
        BtnPlay.BackgroundImageLayout = ImageLayout.Zoom
        BtnPlay.Location = New Point(255, 3)
        BtnPlay.Margin = New Padding(0, 3, 3, 3)
        BtnPlay.Name = "BtnPlay"
        BtnPlay.Size = New Size(45, 45)
        BtnPlay.TabIndex = 6
        BtnPlay.UseVisualStyleBackColor = True
        ' 
        ' BtnPlayStep
        ' 
        BtnPlayStep.BackgroundImage = My.Resources.Resources.play_step
        BtnPlayStep.BackgroundImageLayout = ImageLayout.Zoom
        BtnPlayStep.Location = New Point(303, 3)
        BtnPlayStep.Margin = New Padding(0, 3, 3, 3)
        BtnPlayStep.Name = "BtnPlayStep"
        BtnPlayStep.Size = New Size(45, 45)
        BtnPlayStep.TabIndex = 11
        BtnPlayStep.UseVisualStyleBackColor = True
        ' 
        ' BtnPause
        ' 
        BtnPause.BackgroundImage = My.Resources.Resources.pause
        BtnPause.BackgroundImageLayout = ImageLayout.Zoom
        BtnPause.Enabled = False
        BtnPause.Location = New Point(351, 3)
        BtnPause.Margin = New Padding(0, 3, 3, 3)
        BtnPause.Name = "BtnPause"
        BtnPause.Size = New Size(45, 45)
        BtnPause.TabIndex = 9
        BtnPause.UseVisualStyleBackColor = True
        ' 
        ' BtnStop
        ' 
        BtnStop.BackgroundImage = My.Resources.Resources._stop
        BtnStop.BackgroundImageLayout = ImageLayout.Zoom
        BtnStop.Enabled = False
        BtnStop.Location = New Point(399, 3)
        BtnStop.Margin = New Padding(0, 3, 3, 3)
        BtnStop.Name = "BtnStop"
        BtnStop.Size = New Size(45, 45)
        BtnStop.TabIndex = 10
        BtnStop.UseVisualStyleBackColor = True
        ' 
        ' BtnConfig
        ' 
        BtnConfig.BackgroundImage = My.Resources.Resources.config
        BtnConfig.BackgroundImageLayout = ImageLayout.Zoom
        BtnConfig.Location = New Point(447, 3)
        BtnConfig.Margin = New Padding(0, 3, 3, 3)
        BtnConfig.Name = "BtnConfig"
        BtnConfig.Size = New Size(45, 45)
        BtnConfig.TabIndex = 8
        BtnConfig.UseVisualStyleBackColor = True
        ' 
        ' PanelTopLine
        ' 
        PanelTopLine.BackColor = Color.FromArgb(CByte(0), CByte(122), CByte(204))
        PanelTopLine.Dock = DockStyle.Top
        PanelTopLine.Location = New Point(0, 51)
        PanelTopLine.Name = "PanelTopLine"
        PanelTopLine.Size = New Size(1008, 1)
        PanelTopLine.TabIndex = 7
        ' 
        ' PanelLeft
        ' 
        PanelLeft.BackColor = SystemColors.ControlLight
        PanelLeft.Controls.Add(Panel2)
        PanelLeft.Controls.Add(LeftSideSelect)
        PanelLeft.Controls.Add(Panel1)
        PanelLeft.Dock = DockStyle.Left
        PanelLeft.Location = New Point(0, 52)
        PanelLeft.Name = "PanelLeft"
        PanelLeft.Size = New Size(200, 485)
        PanelLeft.TabIndex = 8
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(LeftSideList)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(0, 23)
        Panel2.Name = "Panel2"
        Panel2.Padding = New Padding(10)
        Panel2.Size = New Size(199, 462)
        Panel2.TabIndex = 9
        ' 
        ' LeftSideList
        ' 
        LeftSideList.BackColor = SystemColors.ControlLight
        LeftSideList.BorderStyle = BorderStyle.None
        LeftSideList.Dock = DockStyle.Fill
        LeftSideList.FormattingEnabled = True
        LeftSideList.ItemHeight = 15
        LeftSideList.Location = New Point(10, 10)
        LeftSideList.Name = "LeftSideList"
        LeftSideList.Size = New Size(179, 442)
        LeftSideList.TabIndex = 0
        ' 
        ' LeftSideSelect
        ' 
        LeftSideSelect.Dock = DockStyle.Top
        LeftSideSelect.DropDownStyle = ComboBoxStyle.DropDownList
        LeftSideSelect.FormattingEnabled = True
        LeftSideSelect.Items.AddRange(New Object() {"New Component", "All Components", "All Connections", "Circuit History"})
        LeftSideSelect.Location = New Point(0, 0)
        LeftSideSelect.Name = "LeftSideSelect"
        LeftSideSelect.Size = New Size(199, 23)
        LeftSideSelect.TabIndex = 1
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.FromArgb(CByte(0), CByte(122), CByte(204))
        Panel1.Dock = DockStyle.Right
        Panel1.Location = New Point(199, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1, 485)
        Panel1.TabIndex = 8
        ' 
        ' TimerGO
        ' 
        ' 
        ' PanelRight
        ' 
        PanelRight.Controls.Add(PropertyGrid)
        PanelRight.Controls.Add(Panel4)
        PanelRight.Dock = DockStyle.Right
        PanelRight.Location = New Point(708, 52)
        PanelRight.Name = "PanelRight"
        PanelRight.Size = New Size(300, 485)
        PanelRight.TabIndex = 9
        PanelRight.Visible = False
        ' 
        ' PropertyGrid
        ' 
        PropertyGrid.Dock = DockStyle.Fill
        PropertyGrid.Location = New Point(1, 0)
        PropertyGrid.Name = "PropertyGrid"
        PropertyGrid.Size = New Size(299, 485)
        PropertyGrid.TabIndex = 9
        ' 
        ' Panel4
        ' 
        Panel4.BackColor = Color.FromArgb(CByte(0), CByte(122), CByte(204))
        Panel4.Dock = DockStyle.Left
        Panel4.Location = New Point(0, 0)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(1, 485)
        Panel4.TabIndex = 8
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {RunningTimeLabel})
        StatusStrip1.Location = New Point(0, 537)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(1008, 24)
        StatusStrip1.TabIndex = 10
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' RunningTimeLabel
        ' 
        RunningTimeLabel.BorderSides = ToolStripStatusLabelBorderSides.Top
        RunningTimeLabel.Name = "RunningTimeLabel"
        RunningTimeLabel.Size = New Size(74, 19)
        RunningTimeLabel.Text = "00:00:00.000"
        ' 
        ' FrmCircuit
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1008, 561)
        Controls.Add(PanelContent)
        Controls.Add(PanelRight)
        Controls.Add(PanelLeft)
        Controls.Add(PanelTopLine)
        Controls.Add(PanelTop)
        Controls.Add(StatusStrip1)
        KeyPreview = True
        Name = "FrmCircuit"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Circuit Tester"
        PanelContent.ResumeLayout(False)
        PanelTop.ResumeLayout(False)
        PanelLeft.ResumeLayout(False)
        Panel2.ResumeLayout(False)
        PanelRight.ResumeLayout(False)
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents PanelContent As Panel
    Friend WithEvents CircuitViewer As ECircuitViewer
    Friend WithEvents PanelTop As FlowLayoutPanel
    Friend WithEvents BtnZoom As RadioButton
    Friend WithEvents BtnEdit As RadioButton
    Friend WithEvents PanelTopLine As Panel
    Friend WithEvents PanelLeft As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents LeftSideList As ListBox
    Friend WithEvents LeftSideSelect As ComboBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents BtnPlay As Button
    Friend WithEvents TimerGO As Timer
    Friend WithEvents PanelRight As Panel
    Friend WithEvents PropertyGrid As PropertyGrid
    Friend WithEvents Panel4 As Panel
    Friend WithEvents BtnConfig As Button
    Friend WithEvents BtnPause As Button
    Friend WithEvents BtnStop As Button
    Friend WithEvents BtnPlayStep As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents RunningTimeLabel As ToolStripStatusLabel
End Class

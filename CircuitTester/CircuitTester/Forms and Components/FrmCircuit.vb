Imports CircuitTester.ECircuits

Public Class FrmCircuit

    Private frmConfig As FrmCircuitConfig

    ' TOP BAR
    Private Sub BtnZoom_CheckedChanged(sender As Object, e As EventArgs) Handles BtnZoom.CheckedChanged
        If BtnZoom.Checked Then
            CircuitViewer.CircuitViewMode = ECircuitViewer.ViewerMode.Zoom
        End If
    End Sub
    Private Sub BtnEdit_CheckedChanged(sender As Object, e As EventArgs) Handles BtnEdit.CheckedChanged
        If BtnEdit.Checked Then
            CircuitViewer.CircuitViewMode = ECircuitViewer.ViewerMode.Edit
        End If
    End Sub
    Private Sub BtnConfig_Click(sender As Object, e As EventArgs) Handles BtnConfig.Click
        PropertyGrid.SelectedObject = frmConfig
        PanelRight.Visible = True
    End Sub

    Private RunningSteps As Integer = 0
    Private RunningCircuit As Boolean = False
    Private CircuitRunningSince As Date
    Private CircuitPreRun As ECircuit
    Private Sub BtnPlay_Click(sender As Object, e As EventArgs) Handles BtnPlay.Click
        If RunningCircuit Then
            ' segundo play (estaba en pausa)
        Else
            ' primer play
            RunningCircuit = True
            CircuitPreRun = CircuitViewer.Circuit.Clone()
            CircuitRunningSince = Now
        End If
        CircuitViewer.Invalidate()
        TimerGO.Start()

        BtnPlay.Enabled = False
        BtnPlayStep.Enabled = False
        BtnPause.Enabled = True
        BtnStop.Enabled = True
    End Sub
    Private Sub BtnPause_Click() Handles BtnPause.Click
        RunningCircuit = True
        TimerGO.Stop()
        CircuitViewer.Invalidate()

        BtnPlay.Enabled = True
        BtnPlayStep.Enabled = True
        BtnPause.Enabled = False
        BtnStop.Enabled = True
    End Sub
    Private Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        RunningCircuit = False
        RunningSteps = 0
        TimerGO.Stop()
        CircuitViewer.Circuit = Nothing
        CircuitViewer.Circuit = CircuitPreRun.Clone()
        CircuitViewer.Invalidate()
        CircuitPreRun = Nothing

        BtnPlay.Enabled = True
        BtnPlayStep.Enabled = True
        BtnPause.Enabled = False
        BtnStop.Enabled = False

        RunningTimeLabel.Text = "00:00:00.00"
    End Sub
    Private Sub BtnPlayStep_Click(sender As Object, e As EventArgs) Handles BtnPlayStep.Click
        If RunningCircuit Then
            ' segundo play (estaba en pausa)
        Else
            ' primer play
            RunningCircuit = True
            CircuitPreRun = CircuitViewer.Circuit.Clone()
            CircuitRunningSince = Now
            RunningSteps = 0
        End If
        DoCircuitStep()

        BtnPlay.Enabled = True
        BtnPlayStep.Enabled = True
        BtnPause.Enabled = False
        BtnStop.Enabled = True
    End Sub
    Private Sub TimerGO_Tick(sender As Object, e As EventArgs) Handles TimerGO.Tick
        DoCircuitStep()

        If CircuitViewer.Circuit.AllPowerSourceIsDead Then
            BtnPause_Click()
        End If
    End Sub
    Private Sub DoCircuitStep()
        If RunningSteps = 0 Then
            CircuitViewer.Circuit.Init()
        End If
        RunningSteps += 1

        Dim runningTime As TimeSpan = Now - CircuitRunningSince
        RunningTimeLabel.Text = runningTime.ToString("hh\:mm\:ss\.ff") + " (" & RunningSteps & " h)"

        PropertyGrid.SelectedObject = PropertyGrid.SelectedObject

        CircuitViewer.Circuit.RunStep()
        CircuitViewer.Invalidate()
    End Sub

    ' RIGHT PANEL
    Private Sub UpdatePropertiesPanel()
        PanelRight.Visible = CircuitViewer.IsAnySelected
        If PanelRight.Visible Then
            For Each c In CircuitViewer.Circuit.CircuitComponents
                If c.IsSelected Then
                    PropertyGrid.SelectedObject = c
                    Exit Sub
                End If
            Next
            For Each c In CircuitViewer.Circuit.CircuitConnections
                If c.IsSelected Then
                    PropertyGrid.SelectedObject = c
                    Exit Sub
                End If
            Next
        End If
    End Sub
    Private Sub PropertyGrid_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles PropertyGrid.PropertyValueChanged
        CircuitViewer.Invalidate()
    End Sub

    ' LEFT PANEL
    Private Sub LeftSideSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LeftSideSelect.SelectedIndexChanged
        PreloadItemsInLeftSideList()
    End Sub
    Private Sub PreloadItemsInLeftSideList()
        LeftSideList.Items.Clear()
        LeftSideList.ClearSelected()

        If LeftSideSelect.SelectedIndex = 0 Then
            ' New Component
            LeftSideList.SelectionMode = SelectionMode.One
            For Each comp In defaultComponents
                LeftSideList.Items.Add(comp.Name)
            Next

        ElseIf LeftSideSelect.SelectedIndex = 1 Then
            ' All Components
            LeftSideList.SelectionMode = SelectionMode.MultiExtended
            For Each comp In CircuitViewer.Circuit.CircuitComponents
                LeftSideList.Items.Add(comp.Name)
                LeftSideList.SetSelected((LeftSideList.Items.Count - 1), comp.IsSelected)
            Next

        ElseIf LeftSideSelect.SelectedIndex = 2 Then
            ' All Connections
            LeftSideList.SelectionMode = SelectionMode.MultiExtended
            Dim connectionTextMask As String = "{0} ({1}) > {2} ({3})"
            Dim connectionText As String
            For Each conn In CircuitViewer.Circuit.CircuitConnections
                Dim name1 As String = "?", name2 As String = "?"
                If conn.OriginComponentID >= 0 Then
                    name1 = CircuitViewer.Circuit.CircuitComponents(conn.OriginComponentID).Name
                End If
                If conn.DestinationComponentID >= 0 Then
                    name2 = CircuitViewer.Circuit.CircuitComponents(conn.DestinationComponentID).Name
                End If

                connectionText = String.Format(
                    connectionTextMask,
                    name1,
                    conn.OriginComponentPin,
                    name2,
                    conn.DestinationComponentPin
                )
                LeftSideList.Items.Add(connectionText)
                LeftSideList.SetSelected((LeftSideList.Items.Count - 1), conn.IsSelected)
            Next

        ElseIf LeftSideSelect.SelectedIndex = 3 Then
            ' History
            LeftSideList.SelectionMode = SelectionMode.One
            For Each H In CircuitHistory
                LeftSideList.Items.Add(H.LastModifiedDate.ToLongTimeString)
            Next

        End If
    End Sub
    Private Sub LeftSideList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LeftSideList.SelectedIndexChanged
        If LeftSideSelect.SelectedIndex = 1 Then
            ' All Components
            For x = 0 To LeftSideList.Items.Count - 1
                CircuitViewer.Circuit.CircuitComponents(x).IsSelected = LeftSideList.GetSelected(x)
            Next
            UpdatePropertiesPanel()

        ElseIf LeftSideSelect.SelectedIndex = 2 Then
            ' All Connections
            For x = 0 To LeftSideList.Items.Count - 1
                CircuitViewer.Circuit.CircuitConnections(x).IsSelected = LeftSideList.GetSelected(x)
            Next
            UpdatePropertiesPanel()

        ElseIf LeftSideSelect.SelectedIndex = 3 Then
            ' Circuit History
            If LeftSideList.SelectedIndex = -1 Then Exit Sub
            If CircuitViewer.Circuit.LastModifiedDate = CircuitHistory(LeftSideList.SelectedIndex).LastModifiedDate Then Exit Sub
            CircuitViewer.Circuit = CircuitHistory(LeftSideList.SelectedIndex).Clone()

        End If
        CircuitViewer.Invalidate()
    End Sub
    Private Sub LeftSideList_DoubleClick(sender As Object, e As EventArgs) Handles LeftSideList.DoubleClick
        If LeftSideSelect.SelectedIndex = 0 And LeftSideList.SelectedIndex >= 0 Then
            ' New Component
            Dim c As EComponent = DefaultComponents(LeftSideList.SelectedIndex).Copy
            c.Rect = New RectangleF(0, 0, 50, 50)
            CircuitViewer.Circuit.CircuitComponents.Add(c)
            SaveCircuitCheckpoint()
            Me.Invalidate()
        End If
    End Sub

    ' CircuitViewer EVENTS
    Private CircuitHistory As New List(Of ECircuit)
    Private MaxHistory As Integer = 30
    Private Sub SaveCircuitCheckpoint()
        ' Skip history if currently running
        If RunningSteps > 0 Then Exit Sub

        Dim checkpoint As ECircuit = CircuitViewer.Circuit.Clone()
        checkpoint.LastModifiedDate = Now
        CircuitHistory.Insert(0, checkpoint)

        If CircuitHistory.Count > MaxHistory Then
            CircuitHistory.RemoveAt(LeftSideList.Items.Count)
        End If


        If LeftSideSelect.SelectedIndex = 3 Then
            ' Circuit History
            PreloadItemsInLeftSideList()
            LeftSideList.SelectedIndex = 0
        End If

    End Sub
    Private Sub CircuitViewer_ComponentDeleted(sender As Object, e As EventArgs) Handles CircuitViewer.ComponentDeleted
        PreloadItemsInLeftSideList()
        SaveCircuitCheckpoint()
        UpdatePropertiesPanel()
    End Sub
    Private Sub CircuitViewer_SelectedComponentsChanged(sender As Object, e As EventArgs) Handles CircuitViewer.SelectedComponentsChanged

        PreloadItemsInLeftSideList()

        Dim itemIndex As Integer = 0
        If LeftSideSelect.SelectedIndex = 1 Then
            ' All Components
            For Each comp In CircuitViewer.Circuit.CircuitComponents
                LeftSideList.SetSelected(itemIndex, comp.IsSelected)
                itemIndex += 1
            Next

        ElseIf LeftSideSelect.SelectedIndex = 2 Then
            ' All Connections
            For Each conn In CircuitViewer.Circuit.CircuitConnections
                LeftSideList.SetSelected(itemIndex, conn.IsSelected)
                itemIndex += 1
            Next

        End If

        UpdatePropertiesPanel()
    End Sub
    Private Sub CircuitViewer_ComponentChangedVisually(sender As Object, e As EventArgs) Handles CircuitViewer.ComponentChangedVisually
        SaveCircuitCheckpoint()
        UpdatePropertiesPanel()
    End Sub

    ' Form EVENTS
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim C As New ECircuit
        ECircuit.InitSample(C)
        CircuitViewer.CircuitOrigin = New Point(CircuitViewer.Width / 2, CircuitViewer.Height / 2)
        CircuitViewer.CircuitViewMode = ECircuitViewer.ViewerMode.Edit
        CircuitViewer.CircuitZoom = 2
        CircuitViewer.Circuit = C

        LeftSideSelect.SelectedIndex = 0
        frmConfig = New FrmCircuitConfig(Me)
    End Sub
    Private Sub FrmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Z And e.Control Then
            LeftSideSelect.SelectedIndex = 3
            If CircuitHistory.Count <= 0 Or LeftSideList.SelectedIndex >= LeftSideList.Items.Count - 1 Then Exit Sub
            LeftSideList.SelectedIndex += 1
            e.Handled = True
        End If
        If e.KeyCode = Keys.Y And e.Control Then
            LeftSideSelect.SelectedIndex = 3
            If CircuitHistory.Count <= 0 Or LeftSideList.SelectedIndex <= 0 Then Exit Sub
            LeftSideList.SelectedIndex -= 1
            e.Handled = True
        End If
    End Sub

End Class

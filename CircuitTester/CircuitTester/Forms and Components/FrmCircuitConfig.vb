
Imports System.ComponentModel

<TypeConverterAttribute(GetType(System.ComponentModel.ExpandableObjectConverter))>
<EditorBrowsable(EditorBrowsableState.Always)>
Public Class FrmCircuitConfig

    Private Frm As FrmCircuit
    Private CircuitViewer As ECircuitViewer
    Public Sub New(ByRef myFrm As FrmCircuit)
        Frm = myFrm
        CircuitViewer = myFrm.CircuitViewer
        Circuit = myFrm.CircuitViewer.Circuit
    End Sub

    <Category("Circuit")>
    Public Property Circuit As ECircuits.ECircuit


    <Category("View")>
    Public Property CircuitViewMode As ECircuitViewer.ViewerMode
        Get
            Return Frm.CircuitViewer.CircuitViewMode
        End Get
        Set(value As ECircuitViewer.ViewerMode)
            Frm.CircuitViewer.CircuitViewMode = value
        End Set
    End Property

    <Category("Run")>
    Private _circuitStatus As String
    Public ReadOnly Property CircuitStatus As String
        Get
            Return _circuitStatus
        End Get
    End Property




End Class

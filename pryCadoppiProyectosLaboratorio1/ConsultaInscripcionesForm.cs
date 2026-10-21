namespace pryCadoppiProyectosLaboratorio1;

public sealed class ConsultaInscripcionesForm : Form
{
    private readonly DataGridView listado = new();
    private readonly Label detalle = new();
    private readonly Color fondo = Color.FromArgb(18, 24, 32);
    private readonly Color panel = Color.FromArgb(28, 37, 49);
    private readonly Color panelClaro = Color.FromArgb(38, 50, 65);
    private readonly Color acento = Color.FromArgb(109, 226, 166);
    private readonly Color texto = Color.FromArgb(239, 245, 242);
    private readonly Color textoSuave = Color.FromArgb(168, 185, 180);
    private List<Inscripcion> inscripciones = [];

    public ConsultaInscripcionesForm()
    {
        Text = "MODO FIT | Consulta de inscripciones";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(900, 560);
        Size = new Size(1050, 650);
        BackColor = fondo;
        ForeColor = texto;
        Font = new Font("Segoe UI", 10F);
        ConstruirInterfaz();
        CargarInscripciones();
    }

    private void ConstruirInterfaz()
    {
        var raiz = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(24), BackColor = fondo };
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        Controls.Add(raiz);

        var cabecera = new Panel { Dock = DockStyle.Fill };
        cabecera.Controls.Add(new Label { Text = "CONSULTA DE INSCRIPCIONES", AutoSize = true, ForeColor = acento, Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(0, 4) });
        var actualizar = new Button { Text = "Actualizar", Width = 105, Height = 32, BackColor = panelClaro, ForeColor = texto, FlatStyle = FlatStyle.Flat, Anchor = AnchorStyles.Top | AnchorStyles.Right };
        actualizar.FlatAppearance.BorderColor = Color.FromArgb(70, 88, 103);
        actualizar.Click += (_, _) => CargarInscripciones();
        cabecera.Controls.Add(actualizar);
        cabecera.Resize += (_, _) => actualizar.Left = cabecera.ClientSize.Width - actualizar.Width;
        raiz.Controls.Add(cabecera, 0, 0);

        var contenido = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, BackColor = fondo };
        contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63));
        contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37));
        raiz.Controls.Add(contenido, 0, 1);

        ConfigurarListado();
        contenido.Controls.Add(listado, 0, 0);

        var panelDetalle = new Panel { Dock = DockStyle.Fill, BackColor = panel, Padding = new Padding(18), Margin = new Padding(14, 0, 0, 0) };
        panelDetalle.Controls.Add(new Label { Text = "DETALLE", AutoSize = true, ForeColor = acento, Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top, Height = 28 });
        detalle.Dock = DockStyle.Fill;
        detalle.ForeColor = texto;
        detalle.Padding = new Padding(0, 12, 0, 0);
        detalle.Text = "Seleccioná una inscripción para ver sus datos.";
        panelDetalle.Controls.Add(detalle);
        contenido.Controls.Add(panelDetalle, 1, 0);

        var pie = new Panel { Dock = DockStyle.Fill };
        var cerrar = new Button { Text = "Cerrar", Width = 100, Height = 34, BackColor = panelClaro, ForeColor = texto, FlatStyle = FlatStyle.Flat, Anchor = AnchorStyles.Top | AnchorStyles.Right };
        cerrar.FlatAppearance.BorderColor = Color.FromArgb(70, 88, 103);
        cerrar.Click += (_, _) => Close();
        pie.Controls.Add(cerrar);
        pie.Resize += (_, _) => cerrar.Left = pie.ClientSize.Width - cerrar.Width;
        raiz.Controls.Add(pie, 0, 2);
    }

    private void ConfigurarListado()
    {
        listado.Dock = DockStyle.Fill;
        listado.BackgroundColor = panel;
        listado.BorderStyle = BorderStyle.None;
        listado.GridColor = Color.FromArgb(62, 78, 91);
        listado.ForeColor = Color.FromArgb(25, 32, 38);
        listado.AllowUserToAddRows = false;
        listado.AllowUserToDeleteRows = false;
        listado.ReadOnly = true;
        listado.MultiSelect = false;
        listado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        listado.AutoGenerateColumns = false;
        listado.RowHeadersVisible = false;
        listado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        listado.ColumnHeadersDefaultCellStyle.BackColor = panelClaro;
        listado.ColumnHeadersDefaultCellStyle.ForeColor = texto;
        listado.ColumnHeadersDefaultCellStyle.SelectionBackColor = panelClaro;
        listado.EnableHeadersVisualStyles = false;
        listado.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = nameof(Inscripcion.Fecha), FillWeight = 23, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
        listado.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nombre", DataPropertyName = nameof(Inscripcion.Nombre), FillWeight = 29 });
        listado.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Plan", DataPropertyName = nameof(Inscripcion.Plan), FillWeight = 23 });
        listado.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = nameof(Inscripcion.Total), FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Format = "C0" } });
        listado.SelectionChanged += (_, _) => MostrarSeleccionado();
    }

    private void CargarInscripciones()
    {
        inscripciones = RepositorioInscripciones.ObtenerTodas();
        inscripciones.Reverse();
        listado.DataSource = null;
        listado.DataSource = inscripciones;
        MostrarSeleccionado();
    }

    private void MostrarSeleccionado()
    {
        if (listado.CurrentRow?.DataBoundItem is not Inscripcion seleccionada)
        {
            detalle.Text = inscripciones.Count == 0 ? "Todavía no hay inscripciones guardadas." : "Seleccioná una inscripción para ver sus datos.";
            return;
        }

        detalle.Text = $"Fecha: {seleccionada.Fecha:dd/MM/yyyy HH:mm}\n\nNombre: {seleccionada.Nombre}\nEdad: {seleccionada.Edad} años ({seleccionada.Categoria})\n\nPlan: {seleccionada.Plan}\nHorario: {seleccionada.Horario}\nDuración: {seleccionada.Meses} mes(es)\nServicios: {Servicios(seleccionada)}\n\nForma de pago: {seleccionada.FormaPago}\nSubtotal: {seleccionada.Subtotal:C0}\nDescuento: {seleccionada.Descuento:C0}\nTOTAL: {seleccionada.Total:C0}\nValor de cuota: {seleccionada.ValorCuota:C0}";
    }

    private static string Servicios(Inscripcion inscripcion)
    {
        var servicios = new List<string>();
        if (inscripcion.ClasesGrupales) servicios.Add("Clases grupales");
        if (inscripcion.EvaluacionInicial) servicios.Add("Evaluación inicial");
        return servicios.Count == 0 ? "Ninguno" : string.Join(", ", servicios);
    }
}

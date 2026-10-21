namespace pryCadoppiProyectosLaboratorio1;

public sealed class Form1 : Form
{
    private const decimal PrecioMusculacion = 15000m;
    private const decimal PrecioFuncional = 18000m;
    private const decimal PrecioNatacion = 25000m;
    private const decimal PrecioClases = 3000m;
    private const decimal DescuentoMenor = 0.25m;
    private const decimal DescuentoAdulto = 0.15m;
    private const decimal DescuentoMayor = 0.30m;

    private readonly TextBox nombre = new();
    private readonly NumericUpDown edad = new();
    private readonly ComboBox plan = new();
    private readonly ComboBox horario = new();
    private readonly NumericUpDown meses = new();
    private readonly CheckBox clases = new() { Text = "Clases grupales" };
    private readonly CheckBox evaluacion = new() { Text = "Evaluación inicial" };
    private readonly RadioButton efectivo = new() { Text = "Efectivo" };
    private readonly RadioButton tarjeta = new() { Text = "Tarjeta" };
    private readonly NumericUpDown cuotas = new();
    private readonly Label subtotal = new();
    private readonly Label descuento = new();
    private readonly Label total = new();
    private readonly Label valorCuota = new();
    private readonly Button calcular = new() { Text = "CALCULAR INSCRIPCIÓN" };
    private readonly Button consultar = new() { Text = "CONSULTAR INSCRIPCIONES" };
    private readonly Color fondo = Color.FromArgb(18, 24, 32);
    private readonly Color panel = Color.FromArgb(28, 37, 49);
    private readonly Color panelClaro = Color.FromArgb(38, 50, 65);
    private readonly Color acento = Color.FromArgb(109, 226, 166);
    private readonly Color texto = Color.FromArgb(239, 245, 242);
    private readonly Color textoSuave = Color.FromArgb(168, 185, 180);

    private struct SOCIO
    {
        public string Nombre, Categoria, Plan, Horario, FormaPago;
        public bool ClasesGrupales, EvaluacionInicial;
        public int Edad, Meses;
        public decimal Total, ValorCuota;
    }

    public Form1()
    {
        Text = "MODO FIT | Gestión de socios";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(980, 680);
        Size = new Size(1180, 760);
        BackColor = fondo;
        ForeColor = texto;
        Font = new Font("Segoe UI", 10F);
        DoubleBuffered = true;
        ConstruirInterfaz();
        ConectarEventos();
        EstadoInicial();
    }

    private void ConstruirInterfaz()
    {
        var raiz = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(28), BackColor = fondo };
        raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        Controls.Add(raiz);

        var izquierda = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Margin = new Padding(0, 0, 18, 0) };
        izquierda.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        izquierda.RowStyles.Add(new RowStyle(SizeType.Absolute, 138));
        izquierda.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        izquierda.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        raiz.Controls.Add(izquierda, 0, 0);
        izquierda.Controls.Add(CrearCabecera(), 0, 0);
        izquierda.Controls.Add(CrearDatos(), 0, 1);
        izquierda.Controls.Add(CrearPlan(), 0, 2);
        izquierda.Controls.Add(CrearAcciones(), 0, 3);
        raiz.Controls.Add(CrearResumen(), 1, 0);
    }

    private Control CrearCabecera()
    {
        var p = new Panel { Dock = DockStyle.Fill };
        p.Controls.Add(new Label { Text = "MODO FIT", AutoSize = true, Font = new Font("Segoe UI", 25, FontStyle.Bold), ForeColor = acento, Location = new Point(0, 2) });
        p.Controls.Add(new Label { Text = "GESTIÓN DE SOCIOS  /  EXPERIENCIA FITNESS", AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = textoSuave, Location = new Point(4, 45) });
        return p;
    }

    private Control CrearDatos()
    {
        var tarjeta = CrearTarjeta("DATOS DEL SOCIO");
        var tabla = Tabla(2, 2);
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        Texto(nombre);
        Numero(edad, 0, 99, 14);
        nombre.KeyPress += Nombre_KeyPress;
        tabla.Controls.Add(Etiqueta("NOMBRE"), 0, 0);
        tabla.Controls.Add(nombre, 1, 0);
        tabla.Controls.Add(Etiqueta("EDAD"), 0, 1);
        tabla.Controls.Add(edad, 1, 1);
        tarjeta.Controls.Add(tabla);
        return tarjeta;
    }

    private Control CrearPlan()
    {
        var tarjeta = CrearTarjeta("PLAN Y PREFERENCIAS");
        var tabla = Tabla(2, 4);
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        Combo(plan, "Musculación", "Funcional", "Natación");
        Combo(horario, "Mañana", "Tarde", "Noche");
        Numero(meses, 0, 99, 1);
        Check(clases);
        Check(evaluacion);
        tabla.Controls.Add(Etiqueta("PLAN"), 0, 0);
        tabla.Controls.Add(plan, 1, 0);
        tabla.Controls.Add(Etiqueta("HORARIO"), 2, 0);
        tabla.Controls.Add(horario, 3, 0);
        tabla.Controls.Add(Etiqueta("MESES"), 0, 1);
        tabla.Controls.Add(meses, 1, 1);
        tabla.Controls.Add(Etiqueta("SERVICIOS"), 2, 1);
        var servicios = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, BackColor = Color.Transparent, Margin = new Padding(0), Padding = new Padding(0, 3, 0, 0) };
        servicios.Controls.Add(clases);
        servicios.Controls.Add(evaluacion);
        tabla.Controls.Add(servicios, 3, 1);
        tarjeta.Controls.Add(tabla);
        return tarjeta;
    }

    private Control CrearAcciones()
    {
        var p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 0, 0) };
        calcular.Width = 225;
        calcular.Height = 42;
        calcular.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        calcular.BackColor = acento;
        calcular.ForeColor = Color.FromArgb(15, 45, 34);
        calcular.FlatStyle = FlatStyle.Flat;
        calcular.FlatAppearance.BorderSize = 0;
        calcular.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        calcular.Click += Calcular_Click;
        var limpiar = new Button { Text = "Limpiar", Width = 100, Height = 42, BackColor = panelClaro, ForeColor = texto, FlatStyle = FlatStyle.Flat, Anchor = AnchorStyles.Top | AnchorStyles.Right };
        limpiar.FlatAppearance.BorderColor = Color.FromArgb(70, 88, 103);
        limpiar.Click += (_, _) => EstadoInicial();
        consultar.Width = 190;
        consultar.Height = 42;
        consultar.BackColor = panelClaro;
        consultar.ForeColor = texto;
        consultar.FlatStyle = FlatStyle.Flat;
        consultar.FlatAppearance.BorderColor = Color.FromArgb(70, 88, 103);
        consultar.Click += (_, _) => new ConsultaInscripcionesForm().ShowDialog(this);
        p.Controls.Add(limpiar);
        p.Controls.Add(consultar);
        p.Controls.Add(calcular);
        p.Resize += (_, _) =>
        {
            calcular.Left = p.ClientSize.Width - calcular.Width;
            limpiar.Left = calcular.Left - limpiar.Width - 10;
            consultar.Left = limpiar.Left - consultar.Width - 10;
        };
        return p;
    }

    private Control CrearResumen()
    {
        var tarjeta = CrearTarjeta("RESUMEN DE INSCRIPCIÓN");
        tarjeta.Padding = new Padding(24);
        var contenido = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7 };
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 88));
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        contenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        contenido.Controls.Add(Destacado("TOTAL A PAGAR", total), 0, 0);
        contenido.Controls.Add(Linea("Subtotal", subtotal), 0, 1);
        contenido.Controls.Add(Linea("Descuento aplicado", descuento), 0, 2);
        contenido.Controls.Add(FormaPago(), 0, 3);
        contenido.Controls.Add(Linea("Valor de cada cuota", valorCuota), 0, 4);
        contenido.Controls.Add(new Label { Text = "Completá los datos del socio para calcular una propuesta clara y personalizada.\n\nMODO FIT  ·  MÁS FUERTE CADA DÍA", Dock = DockStyle.Fill, ForeColor = textoSuave, Padding = new Padding(2, 20, 2, 0) }, 0, 5);
        tarjeta.Controls.Add(contenido);
        return tarjeta;
    }

    private Control FormaPago()
    {
        var p = new Panel { Dock = DockStyle.Fill, BackColor = panelClaro, Padding = new Padding(12, 8, 12, 6), Margin = new Padding(0, 8, 0, 8) };
        p.Controls.Add(new Label { Text = "FORMA DE PAGO", AutoSize = true, ForeColor = textoSuave, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(12, 8) });
        efectivo.Location = new Point(12, 32);
        tarjeta.Location = new Point(105, 32);
        efectivo.AutoSize = tarjeta.AutoSize = true;
        efectivo.ForeColor = tarjeta.ForeColor = texto;
        efectivo.BackColor = tarjeta.BackColor = Color.Transparent;
        cuotas.Width = 62;
        cuotas.Minimum = 1;
        cuotas.Maximum = 12;
        cuotas.Location = new Point(200, 29);
        cuotas.BackColor = fondo;
        cuotas.ForeColor = texto;
        p.Controls.Add(efectivo);
        p.Controls.Add(tarjeta);
        p.Controls.Add(cuotas);
        p.Controls.Add(new Label { Text = "cuotas", AutoSize = true, ForeColor = textoSuave, Location = new Point(266, 33) });
        return p;
    }

    private Control CrearTarjeta(string titulo)
    {
        var p = new Panel { Dock = DockStyle.Fill, BackColor = panel, Padding = new Padding(18), Margin = new Padding(0, 0, 0, 10) };
        p.Controls.Add(new Label { Text = titulo, AutoSize = true, ForeColor = acento, Font = new Font("Segoe UI", 9, FontStyle.Bold), Dock = DockStyle.Top, Height = 26 });
        return p;
    }

    private TableLayoutPanel Tabla(int filas, int columnas) => new() { Dock = DockStyle.Fill, RowCount = filas, ColumnCount = columnas, BackColor = Color.Transparent, Padding = new Padding(0, 2, 0, 0) };
    private Label Etiqueta(string texto) => new() { Text = texto, AutoSize = false, Dock = DockStyle.Fill, ForeColor = textoSuave, Font = new Font("Segoe UI", 8, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 0, 10, 0), Margin = new Padding(0) };

    private Control Destacado(string titulo, Label valor)
    {
        var p = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(34, 75, 63), Padding = new Padding(14, 8, 14, 4), Margin = new Padding(0, 0, 0, 10) };
        p.Controls.Add(new Label { Text = titulo, AutoSize = true, ForeColor = Color.FromArgb(177, 232, 205), Font = new Font("Segoe UI", 8, FontStyle.Bold), Dock = DockStyle.Top });
        valor.Text = "$ 0";
        valor.AutoSize = true;
        valor.ForeColor = acento;
        valor.Font = new Font("Segoe UI", 25, FontStyle.Bold);
        valor.Dock = DockStyle.Fill;
        p.Controls.Add(valor);
        return p;
    }

    private Control Linea(string titulo, Label valor)
    {
        var p = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(0, 8, 0, 4) };
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        p.Controls.Add(new Label { Text = titulo, AutoSize = true, ForeColor = textoSuave, Anchor = AnchorStyles.Left }, 0, 0);
        valor.Text = "$ 0";
        valor.AutoSize = true;
        valor.ForeColor = texto;
        valor.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        valor.Anchor = AnchorStyles.Right;
        p.Controls.Add(valor, 1, 0);
        return p;
    }

    private void ConectarEventos()
    {
        nombre.TextChanged += Nombre_TextChanged;
        edad.ValueChanged += (_, _) => Actualizar();
        plan.SelectedIndexChanged += (_, _) => Actualizar();
        horario.SelectedIndexChanged += (_, _) => Actualizar();
        meses.ValueChanged += (_, _) => Actualizar();
        clases.CheckedChanged += (_, _) => Actualizar();
        efectivo.CheckedChanged += (_, _) => ActualizarPago();
        tarjeta.CheckedChanged += (_, _) => ActualizarPago();
        cuotas.ValueChanged += (_, _) => ActualizarPreview();
    }

    private void Calcular_Click(object? sender, EventArgs e)
    {
        if (!Validar(out string mensaje))
        {
            MessageBox.Show(mensaje, "Revisá los datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var socio = ObtenerSocio();
        var rebaja = Subtotal() - socio.Total;
        try
        {
            RepositorioInscripciones.Guardar(new Inscripcion
            {
                Fecha = DateTime.Now,
                Nombre = socio.Nombre,
                Edad = socio.Edad,
                Categoria = socio.Categoria,
                Plan = socio.Plan,
                Horario = socio.Horario,
                Meses = socio.Meses,
                ClasesGrupales = socio.ClasesGrupales,
                EvaluacionInicial = socio.EvaluacionInicial,
                FormaPago = socio.FormaPago,
                Subtotal = Subtotal(),
                Descuento = rebaja,
                Total = socio.Total,
                ValorCuota = socio.ValorCuota
            });
        }
        catch (IOException error)
        {
            MessageBox.Show($"No se pudo guardar la inscripción.\n\n{error.Message}", "Error de guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        MessageBox.Show($"SOCIO REGISTRADO\n\nNombre: {socio.Nombre}\nCategoría: {socio.Categoria}\nPlan: {socio.Plan} · {socio.Horario}\nDuración: {socio.Meses} mes(es)\nPago: {socio.FormaPago}\n\nSubtotal: {Subtotal():C0}\nDescuento: {rebaja:C0}\nTOTAL: {socio.Total:C0}\nValor de cuota: {socio.ValorCuota:C0}", "MODO FIT · Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        EstadoInicial();
    }

    private bool Validar(out string mensaje)
    {
        if (string.IsNullOrWhiteSpace(nombre.Text)) { mensaje = "Ingresá el nombre del socio."; return false; }
        if (edad.Value < 14) { mensaje = "La edad mínima para asociarse es de 14 años."; return false; }
        if (plan.SelectedIndex < 0 || horario.SelectedIndex < 0) { mensaje = "Elegí un plan y un horario."; return false; }
        if (meses.Value is < 1 or > 12) { mensaje = "La cantidad de meses debe estar entre 1 y 12."; return false; }
        mensaje = string.Empty;
        return true;
    }

    private SOCIO ObtenerSocio()
    {
        int edadSocio = (int)edad.Value;
        decimal importe = Subtotal();
        decimal totalSocio = importe - importe * ObtenerDescuento(edadSocio);
        bool pagaConTarjeta = tarjeta.Checked;
        return new SOCIO
        {
            Nombre = nombre.Text.Trim().ToUpperInvariant(), Edad = edadSocio,
            Categoria = edadSocio < 18 ? "Menor" : edadSocio >= 65 ? "Mayor" : "Adulto",
            Plan = plan.Text, Horario = horario.Text, Meses = (int)meses.Value,
            ClasesGrupales = clases.Checked, EvaluacionInicial = evaluacion.Checked,
            FormaPago = pagaConTarjeta ? $"Tarjeta en {cuotas.Value:0} cuotas" : "Efectivo",
            Total = totalSocio, ValorCuota = pagaConTarjeta ? totalSocio / cuotas.Value : totalSocio
        };
    }

    private decimal Subtotal()
    {
        decimal mensual = plan.Text switch { "Musculación" => PrecioMusculacion, "Funcional" => PrecioFuncional, "Natación" => PrecioNatacion, _ => 0m };
        return (mensual + (clases.Checked ? PrecioClases : 0m)) * meses.Value;
    }

    private decimal ObtenerDescuento(int edadSocio) => edadSocio < 18 ? DescuentoMenor : edadSocio >= 65 ? DescuentoMayor : DescuentoAdulto;

    private void EstadoInicial()
    {
        nombre.Clear();
        edad.Value = 14;
        plan.SelectedIndex = 0;
        horario.SelectedIndex = 0;
        meses.Value = 1;
        clases.Checked = false;
        evaluacion.Checked = false;
        efectivo.Checked = true;
        cuotas.Value = 1;
        ActualizarPago();
        nombre.Focus();
    }

    private void Actualizar()
    {
        calcular.Enabled = !string.IsNullOrWhiteSpace(nombre.Text) && plan.SelectedIndex >= 0 && horario.SelectedIndex >= 0;
        ActualizarPreview();
    }

    private void Nombre_TextChanged(object? sender, EventArgs e)
    {
        int posicion = nombre.SelectionStart;
        string soloLetras = new(nombre.Text.Where(caracter => char.IsLetter(caracter) || char.IsWhiteSpace(caracter)).ToArray());
        string mayusculas = soloLetras.ToUpperInvariant();
        if (nombre.Text != mayusculas)
        {
            nombre.Text = mayusculas;
            nombre.SelectionStart = Math.Min(posicion, nombre.Text.Length);
        }

        Actualizar();
    }

    private void Nombre_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void ActualizarPago()
    {
        cuotas.Enabled = tarjeta.Checked;
        if (efectivo.Checked) cuotas.Value = 1;
        ActualizarPreview();
    }

    private void ActualizarPreview()
    {
        if (plan.SelectedIndex < 0) return;
        decimal importe = Subtotal();
        decimal rebaja = importe * ObtenerDescuento((int)edad.Value);
        decimal importeTotal = importe - rebaja;
        subtotal.Text = importe.ToString("C0");
        descuento.Text = rebaja.ToString("C0");
        total.Text = importeTotal.ToString("C0");
        valorCuota.Text = (tarjeta.Checked ? importeTotal / cuotas.Value : importeTotal).ToString("C0");
    }

    private void Texto(TextBox control)
    {
        control.Dock = DockStyle.Fill;
        control.BackColor = fondo;
        control.ForeColor = texto;
        control.BorderStyle = BorderStyle.FixedSingle;
        control.Margin = new Padding(0, 4, 8, 4);
    }

    private void Numero(NumericUpDown control, decimal minimo, decimal maximo, decimal valor)
    {
        control.Minimum = minimo;
        control.Maximum = maximo;
        control.Value = Math.Clamp(valor, minimo, maximo);
        control.Dock = DockStyle.Fill; control.BackColor = fondo; control.ForeColor = texto;
        control.BorderStyle = BorderStyle.FixedSingle; control.Margin = new Padding(0, 4, 8, 4);
        control.KeyPress += SoloDigitos_KeyPress;
    }

    private void SoloDigitos_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
    }

    private void Combo(ComboBox control, params string[] opciones)
    {
        control.Items.AddRange(opciones); control.DropDownStyle = ComboBoxStyle.DropDownList;
        control.Dock = DockStyle.Fill; control.BackColor = fondo; control.ForeColor = texto;
        control.FlatStyle = FlatStyle.Flat; control.Margin = new Padding(0, 4, 8, 4);
    }

    private void Check(CheckBox control)
    {
        control.AutoSize = true; control.ForeColor = texto; control.BackColor = Color.Transparent;
        control.Margin = new Padding(0, 3, 12, 3);
    }
}

using System.Globalization;

namespace pryCadoppiSP4;

public partial class Form1 : Form
{
    private static readonly string[] Mozos = ["Julio", "Esteban", "Javier", "Gonzalo", "Alberto"];
    private static readonly string[] Categorias = ["Comidas", "Bebidas sin alcohol", "Bebidas con alcohol", "Postres"];

    private readonly float[,] ventas = new float[5, 4];
    private readonly DataGridView grilla = new();
    private readonly Button validarDatos = new() { Text = "Validar Datos" };
    private readonly Button mozoDelDia = new() { Text = "Mozo del día" };
    private readonly Button totales = new() { Text = "Totales" };
    private readonly Label resultadoMozo = new();
    private readonly Label resultadoImporteMozo = new();
    private readonly Label resultadoTotales = new();

    public Form1()
    {
        InitializeComponent();
        ConstruirInterfaz();
    }

    private void ConstruirInterfaz()
    {
        Text = "Bar La Milanga | Ventas diarias";
        Icon = new Icon(Path.Combine(AppContext.BaseDirectory, "ICONO-MILANESA.ico"));
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);
        Size = new Size(1050, 680);

        var fondo = Color.FromArgb(248, 244, 238);
        var colorPrincipal = Color.FromArgb(122, 42, 28);
        var colorSecundario = Color.FromArgb(244, 228, 211);
        BackColor = fondo;
        Font = new Font("Segoe UI", 10F);

        var raiz = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(28),
            BackColor = fondo
        };
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        Controls.Add(raiz);

        var encabezado = new Panel { Dock = DockStyle.Fill };
        encabezado.Controls.Add(new Label
        {
            Text = "BAR LA MILANGA",
            AutoSize = true,
            ForeColor = colorPrincipal,
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            Location = new Point(0, 0)
        });
        encabezado.Controls.Add(new Label
        {
            Text = "CARGA DE VENTAS DIARIAS POR MOZO",
            AutoSize = true,
            ForeColor = Color.FromArgb(100, 91, 83),
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Location = new Point(4, 43)
        });
        raiz.Controls.Add(encabezado, 0, 0);

        raiz.Controls.Add(new Label
        {
            Text = "Ingrese los importes finales de cada mozo. Se admite el valor cero.",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(75, 66, 59),
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 1);

        ConfigurarGrilla(colorPrincipal, colorSecundario);
        raiz.Controls.Add(grilla, 0, 2);

        raiz.Controls.Add(CrearConsultas(colorPrincipal, colorSecundario), 0, 3);

        var acciones = new Panel { Dock = DockStyle.Fill };
        ConfigurarBoton(validarDatos, colorPrincipal, Color.White, 150);
        validarDatos.Click += ValidarDatos_Click;
        acciones.Controls.Add(validarDatos);
        validarDatos.Location = new Point(0, 5);

        var cerrar = new Button { Text = "Cerrar", Width = 120, Height = 38, BackColor = Color.FromArgb(92, 83, 76), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        cerrar.FlatAppearance.BorderSize = 0;
        cerrar.Click += (_, _) => Application.Exit();
        acciones.Controls.Add(cerrar);
        cerrar.Location = new Point(validarDatos.Right + 12, 5);
        raiz.Controls.Add(acciones, 0, 4);
    }

    private void ConfigurarGrilla(Color colorPrincipal, Color colorSecundario)
    {
        grilla.Dock = DockStyle.Fill;
        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.AllowUserToResizeRows = false;
        grilla.RowHeadersVisible = true;
        grilla.RowHeadersWidth = 115;
        grilla.ColumnHeadersHeight = 42;
        grilla.RowTemplate.Height = 46;
        grilla.BackgroundColor = Color.White;
        grilla.BorderStyle = BorderStyle.FixedSingle;
        grilla.GridColor = Color.FromArgb(215, 202, 190);
        grilla.SelectionMode = DataGridViewSelectionMode.CellSelect;
        grilla.EditMode = DataGridViewEditMode.EditOnEnter;
        grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grilla.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = colorPrincipal,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleCenter
        };
        grilla.RowHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = colorSecundario,
            ForeColor = colorPrincipal,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleLeft
        };
        grilla.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Color.FromArgb(45, 39, 34),
            SelectionBackColor = Color.FromArgb(255, 236, 207),
            SelectionForeColor = Color.FromArgb(45, 39, 34),
            Alignment = DataGridViewContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 11)
        };
        grilla.EnableHeadersVisualStyles = false;

        foreach (var categoria in Categorias)
        {
            grilla.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = categoria,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }

        foreach (var mozo in Mozos)
        {
            var fila = grilla.Rows.Add();
            grilla.Rows[fila].HeaderCell.Value = mozo;
        }
    }

    private Control CrearConsultas(Color colorPrincipal, Color colorSecundario)
    {
        var consultas = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            BackColor = colorSecundario,
            Padding = new Padding(12)
        };
        consultas.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
        consultas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        consultas.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        consultas.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        ConfigurarBoton(mozoDelDia, colorPrincipal, Color.White, 150);
        mozoDelDia.Enabled = false;
        mozoDelDia.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        mozoDelDia.Click += MozoDelDia_Click;
        consultas.Controls.Add(mozoDelDia, 0, 0);

        resultadoMozo.AutoSize = true;
        resultadoMozo.Text = "Nombre: -";
        resultadoMozo.ForeColor = colorPrincipal;
        resultadoMozo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        resultadoMozo.Anchor = AnchorStyles.Left;
        consultas.Controls.Add(resultadoMozo, 1, 0);

        ConfigurarBoton(totales, colorPrincipal, Color.White, 150);
        totales.Enabled = false;
        totales.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        totales.Click += Totales_Click;
        consultas.Controls.Add(totales, 0, 1);

        resultadoTotales.AutoSize = true;
        resultadoTotales.Text = "Total general: -    Comidas: -    Bebidas sin alcohol: -    Bebidas con alcohol: -    Postres: -";
        resultadoTotales.ForeColor = Color.FromArgb(65, 55, 48);
        resultadoTotales.Anchor = AnchorStyles.Left;
        consultas.Controls.Add(resultadoTotales, 1, 1);

        resultadoImporteMozo.Visible = false;
        return consultas;
    }

    private static void ConfigurarBoton(Button boton, Color fondo, Color texto, int ancho)
    {
        boton.Width = ancho;
        boton.Height = 38;
        boton.BackColor = fondo;
        boton.ForeColor = texto;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
    }

    private void ValidarDatos_Click(object? sender, EventArgs e)
    {
        bool datosValidos = true;
        string? celdaConError = null;

        for (int fila = 0; fila < Mozos.Length; fila++)
        {
            for (int columna = 0; columna < Categorias.Length; columna++)
            {
                var celda = grilla.Rows[fila].Cells[columna];
                string texto = Convert.ToString(celda.Value)?.Trim() ?? string.Empty;
                if (!TryParseImporte(texto, out float importe))
                {
                    celda.Style.BackColor = Color.FromArgb(255, 220, 220);
                    datosValidos = false;
                    celdaConError ??= $"{Mozos[fila]} - {Categorias[columna]}";
                }
                else
                {
                    ventas[fila, columna] = importe;
                    celda.Value = importe.ToString("N2");
                    celda.Style.BackColor = Color.White;
                }
            }
        }

        mozoDelDia.Enabled = datosValidos;
        totales.Enabled = datosValidos;

        if (!datosValidos)
        {
            resultadoMozo.Text = "Nombre: -";
            resultadoImporteMozo.Text = "Importe: -";
            resultadoTotales.Text = "Total general: -    Comidas: -    Bebidas sin alcohol: -    Bebidas con alcohol: -    Postres: -";
            MessageBox.Show($"La celda '{celdaConError}' no contiene un importe numérico válido.\nComplete toda la grilla, admitiendo 0 como valor válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show("Todos los datos fueron validados correctamente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static bool TryParseImporte(string texto, out float importe)
    {
        return float.TryParse(texto, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out importe)
            || float.TryParse(texto, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out importe);
    }

    private void MozoDelDia_Click(object? sender, EventArgs e)
    {
        int indiceMozo = 0;
        float mayorTotal = TotalMozo(0);
        for (int fila = 1; fila < Mozos.Length; fila++)
        {
            float totalMozo = TotalMozo(fila);
            if (totalMozo > mayorTotal)
            {
                mayorTotal = totalMozo;
                indiceMozo = fila;
            }
        }

        resultadoMozo.Text = $"Nombre: {Mozos[indiceMozo]}     Importe total: {mayorTotal:N2}";
    }

    private void Totales_Click(object? sender, EventArgs e)
    {
        float[] totalesPorCategoria = new float[Categorias.Length];
        float totalGeneral = 0;

        for (int fila = 0; fila < Mozos.Length; fila++)
        {
            for (int columna = 0; columna < Categorias.Length; columna++)
            {
                totalesPorCategoria[columna] += ventas[fila, columna];
                totalGeneral += ventas[fila, columna];
            }
        }

        resultadoTotales.Text = $"Total general: {totalGeneral:N2}    " +
            $"Comidas: {totalesPorCategoria[0]:N2}    " +
            $"Bebidas sin alcohol: {totalesPorCategoria[1]:N2}    " +
            $"Bebidas con alcohol: {totalesPorCategoria[2]:N2}    " +
            $"Postres: {totalesPorCategoria[3]:N2}";
    }

    private float TotalMozo(int fila)
    {
        float total = 0;
        for (int columna = 0; columna < Categorias.Length; columna++)
        {
            total += ventas[fila, columna];
        }

        return total;
    }
}

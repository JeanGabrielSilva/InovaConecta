using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InovaConecta.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace InovaConecta {
    public partial class Form1 : Form {

        private WinFormsTimer lblResetTimer;
        private WinFormsTimer saveTimer;
        private WinFormsTimer timerPanel;
        private ContextMenuStrip menuContexto;

        private readonly string configFile = Path.Combine(Application.StartupPath, "config.json");
        private readonly string bancosTitulosFile = Path.Combine(Application.StartupPath, "bancos_titulos.json");

        private string selectedDatabase = "";
        private bool carregandoGrid = false;
        private bool atualizandoUI = false;
        private bool observacaoEditada = false;
        private string tituloOriginal = string.Empty;
        private string observacaoOriginal = string.Empty;

        public Form1() {
            InitializeComponent();
            LoadConfig();
            AdicionarAoInicioComWindows();
            EstilizarTextBoxes();
            CarregarInstanciasSQLAsync();

            dataGridBancos.ReadOnly = true;
            dataGridBancos.SelectionChanged += dataGridBancos_SelectionChanged;
            dataGridBancos.CellClick += dataGridBancos_CellClick;
            dataGridBancos.DataBindingComplete += DataGridBancos_DataBindingComplete;

            label1.ForeColor = ColorTranslator.FromHtml("#555555");
            labelConectaAlterado.Text = "Conecta alterado";
            labelConectaAlterado.ForeColor = Color.Green;
            labelConectaAlterado.Visible = false;
            panelConectaAlterado.Visible = false;

            lblInstancia.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblUsuario.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSenha.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            txtUsuarioInstancia.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblAssunto.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblObservacao.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            lblInovaConecta.ForeColor = ColorTranslator.FromHtml("#00A9FE");
            lblInformeDados.ForeColor = ColorTranslator.FromHtml("#4682B4");
            lblDetalhesBanco.ForeColor = ColorTranslator.FromHtml("#4682B4");

            btnBuscarBanco.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnBuscarBanco.BackColor = ColorTranslator.FromHtml("#3A8A60");
            btnBuscarBanco.ForeColor = Color.White;
            btnBuscarBanco.FlatStyle = FlatStyle.Flat;
            btnBuscarBanco.FlatAppearance.BorderSize = 0;

            btnTrocarConecta.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnTrocarConecta.BackColor = ColorTranslator.FromHtml("#00A9FE");
            btnTrocarConecta.ForeColor = Color.White;
            btnTrocarConecta.FlatStyle = FlatStyle.Flat;
            btnTrocarConecta.FlatAppearance.BorderSize = 0;

            btnIniciarInovafarma.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnIniciarInovafarma.BackColor = ColorTranslator.FromHtml("#4682B4");
            btnIniciarInovafarma.ForeColor = Color.White;
            btnIniciarInovafarma.FlatStyle = FlatStyle.Flat;
            btnIniciarInovafarma.FlatAppearance.BorderSize = 0;

            AplicarPlaceholder(txtTitulo, "Digite o título");
            AplicarPlaceholder(txtObservacao, "Digite a observação");

            txtTitulo.Enter += txtTitulo_Enter;
            txtTitulo.Leave += txtTitulo_Leave;
            txtObservacao.Enter += txtObservacao_Enter;
            txtObservacao.Leave += txtObservacao_Leave;

            lblResetTimer = new WinFormsTimer();
            lblResetTimer.Interval = 3000;
            lblResetTimer.Tick += (s, args) => {
                lblObservacaoSalva.Text = "";
                lblResetTimer.Stop();
            };

            CarregarCredenciais();

            if (string.IsNullOrWhiteSpace(txtUsuarioInstancia.Text)) {
                txtUsuarioInstancia.Text = "Digite o usuário";
                txtUsuarioInstancia.ForeColor = Color.Gray;
            }
            txtUsuarioInstancia.Enter += txtUsuarioInstancia_Enter;
            txtUsuarioInstancia.Leave += txtUsuarioInstancia_Leave;

            if (string.IsNullOrWhiteSpace(txtSenhaInstancia.Text)) {
                txtSenhaInstancia.Text = "Digite a senha";
                txtSenhaInstancia.ForeColor = Color.Gray;
                txtSenhaInstancia.PasswordChar = '\0';
            }
            txtSenhaInstancia.Enter += txtSenhaInstancia_Enter;
            txtSenhaInstancia.Leave += txtSenhaInstancia_Leave;

            ConfigurarNotifyIcon();

            saveTimer = new WinFormsTimer();
            saveTimer.Interval = 500;
            saveTimer.Tick += SaveTimer_Tick;

            timerPanel = new WinFormsTimer();
            timerPanel.Interval = 500;
            timerPanel.Tick += TimerPanel_Tick;
        }

        private void TimerPanel_Tick(object sender, EventArgs e) {
            timerPanel.Stop();
            if (!IsDisposed && panelConectaAlterado != null)
                panelConectaAlterado.Visible = false;
        }

        private void SaveTimer_Tick(object sender, EventArgs e) {
            saveTimer.Stop();
            if (carregandoGrid) return;
            if (observacaoEditada) {
                SalvarObservacao();
                observacaoEditada = false;
            }
        }

        private void AdicionarAoInicioComWindows() {
            try {
                string nomeApp = "InovaConecta";
                string caminhoApp = Application.ExecutablePath;
                RegistryKey chave = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (chave.GetValue(nomeApp) == null) {
                    chave.SetValue(nomeApp, "\"" + caminhoApp + "\"");
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao configurar inicialização automática: " + ex.Message);
            }
        }

        private void btnBuscarBanco_Click(object sender, EventArgs e) {
            string instancia = cmbInstancia.SelectedItem?.ToString();
            string usuario = txtUsuarioInstancia.Text;
            string senha = txtSenhaInstancia.Text;

            if (string.IsNullOrWhiteSpace(instancia)) {
                MessageBox.Show("Nenhuma instância selecionada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnBuscarBanco.Enabled = false;

            try {
                DataTable dt = BuscarBancosDaInstancia(instancia, usuario, senha);
                AplicarResultadoBancosNaUI(dt, instancia, usuario, senha);
            } catch (Exception ex) {
                btnBuscarBanco.Enabled = true;
                var resultado = MessageBox.Show(
                    "Erro ao buscar bancos de dados:\n" + ex.Message + "\n\nDeseja tentar novamente?",
                    "Erro de Conexão",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Error);
                if (resultado != DialogResult.Retry)
                    btnBuscarBanco.Enabled = false;
            }

            btnBuscarBanco.Enabled = true;
        }

        private DataTable BuscarBancosDaInstancia(string instancia, string usuario, string senha) {
            string connString = $"Server={instancia};User Id={usuario};Password={senha};Connection Timeout=8;";

            using (SqlConnection conn = new SqlConnection(connString)) {
                conn.Open();

                string query = @"
                    SELECT database_id, name
                    FROM sys.databases
                    WHERE database_id > 4
                    AND state_desc = 'ONLINE'
                    AND name NOT IN (
                        'master', 'tempdb', 'model', 'msdb',
                        'INOVAFARMA_UPDATE', 'INOVAFARMA_API',
                        'INOVAFARMA_SERVICE', 'TEMP_BAK',
                        'inovafarma_service', 'Inovafarma_API', 'inovafarma_api'
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    cmd.CommandTimeout = 8;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dt.Columns["name"].ColumnName = "NomeBanco";
                    dt.Columns.Add("Titulo", typeof(string));
                    return dt;
                }
            }
        }

        private void AplicarResultadoBancosNaUI(DataTable dt, string instanciaSelecionada, string usuario, string senha) {
            List<BancoInfo> bancos = new List<BancoInfo>();
            if (File.Exists(bancosTitulosFile)) {
                try {
                    bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile)) ?? new List<BancoInfo>();
                } catch { }
            }

            foreach (DataRow row in dt.Rows) {
                if (int.TryParse(row["database_id"].ToString(), out int databaseId)) {
                    string nomeBanco = row["NomeBanco"].ToString();
                    var bancoInfo = bancos.Find(b =>
                        b.Instancia == instanciaSelecionada &&
                        b.DatabaseId == databaseId &&
                        b.NomeBanco == nomeBanco);
                    row["Titulo"] = bancoInfo?.Titulo ?? string.Empty;
                }
            }

            try {
                carregandoGrid = true;
                dataGridBancos.SelectionChanged -= dataGridBancos_SelectionChanged;

                dataGridBancos.DataSource = null;
                dataGridBancos.DataSource = dt;
                dataGridBancos.ClearSelection();

                if (dataGridBancos.Rows.Count > 0) {
                    var primeiraLinha = dataGridBancos.Rows[0];
                    primeiraLinha.Selected = true;
                    dataGridBancos.CurrentCell = primeiraLinha.Cells["NomeBanco"];
                    selectedDatabase = primeiraLinha.Cells["NomeBanco"].Value?.ToString() ?? "";
                    lblBancoSelecionado.Text = "Banco Selecionado: " + selectedDatabase;
                    CarregarObservacao();
                }

                if (!string.IsNullOrWhiteSpace(instanciaSelecionada) &&
                    !string.IsNullOrWhiteSpace(usuario) &&
                    !string.IsNullOrWhiteSpace(senha)) {
                    SaveConfig();
                }
            } finally {
                carregandoGrid = false;
                dataGridBancos.SelectionChanged += dataGridBancos_SelectionChanged;
            }
        }

        private void btnTrocarConecta_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(selectedDatabase)) {
                MessageBox.Show("Por favor, selecione um banco de dados!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CriarConectaLog(selectedDatabase);
        }

        private void CriarConectaLog(string bancoSelecionado) {
            try {
                string conectaPath = @"C:\inovafarma\conecta.log";
                string conectaXmlPath = @"C:\inovafarma\conecta.xml";

                FecharInovaFarma();

                if (File.Exists(conectaPath)) File.Delete(conectaPath);
                if (File.Exists(conectaXmlPath)) File.Delete(conectaXmlPath);

                string conectaContent = $"Data Source={cmbInstancia.SelectedItem?.ToString()}; Initial Catalog={bancoSelecionado}; User ID={txtUsuarioInstancia.Text}; pwd={txtSenhaInstancia.Text}\n";
                File.WriteAllText(conectaPath, conectaContent, Encoding.Default);

                int attempts = 0;
                while (!File.Exists(conectaPath) && attempts < 10) {
                    Thread.Sleep(200);
                    attempts++;
                }

                if (File.Exists(conectaPath)) {
                    labelConectaAlterado.Text = $"Conecta log criado para o banco:\n{bancoSelecionado}";
                    labelConectaAlterado.ForeColor = Color.Green;
                    panelConectaAlterado.Visible = true;
                    timerPanel.Start();
                } else {
                    MessageBox.Show("Erro: o arquivo conecta.log não foi criado corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao criar o conecta.log: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FecharInovaFarma() {
            try {
                foreach (var process in Process.GetProcessesByName("InovaFarma")) {
                    process.Kill();
                    process.WaitForExit();
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao fechar InovaFarma: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IniciarInovaFarma() {
            try {
                ProcessStartInfo startInfo = new ProcessStartInfo {
                    FileName = @"C:\InovaFarma\verificador.exe",
                    WorkingDirectory = @"C:\InovaFarma"
                };
                Process.Start(startInfo);
            } catch (Exception ex) {
                MessageBox.Show("Erro ao iniciar InovaFarma: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveConfig() {
            try {
                var config = new {
                    Usuario = txtUsuarioInstancia.Text,
                    Senha = txtSenhaInstancia.Text
                };

                File.WriteAllText(configFile, JsonConvert.SerializeObject(config, Formatting.Indented));
            } catch (Exception ex) {
                MessageBox.Show("Erro ao salvar configurações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConfig() {
            if (File.Exists(configFile)) {
                try {
                    var config = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(configFile));
                    txtUsuarioInstancia.Text = config?.Usuario ?? "";
                    txtSenhaInstancia.Text = config?.Senha ?? "";
                } catch (Exception ex) {
                    MessageBox.Show("Erro ao carregar configurações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridBancos_SelectionChanged(object sender, EventArgs e) {
            if (carregandoGrid || atualizandoUI) return;

            try {
                atualizandoUI = true;

                if (observacaoEditada) {
                    saveTimer.Stop();
                    SalvarObservacao();
                    observacaoEditada = false;
                }

                if (dataGridBancos.SelectedRows.Count > 0) {
                    var linha = dataGridBancos.SelectedRows[0];
                    if (linha.Cells["NomeBanco"].Value == null) return;

                    selectedDatabase = linha.Cells["NomeBanco"].Value.ToString();
                    lblBancoSelecionado.Text = "Banco Selecionado: " + selectedDatabase;
                    CarregarObservacao();
                }
            } finally {
                atualizandoUI = false;
            }
        }

        private void CarregarObservacao() {
            try {
                if (File.Exists(bancosTitulosFile)) {
                    var bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));

                    int databaseId = ObterDatabaseIdDoBancoSelecionado();
                    string instancia = cmbInstancia.SelectedItem?.ToString();
                    string nomeBanco = selectedDatabase;

                    var bancoSelecionado = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                    if (bancoSelecionado != null) {
                        tituloOriginal = bancoSelecionado.Titulo ?? string.Empty;
                        observacaoOriginal = bancoSelecionado.Observacao ?? string.Empty;

                        txtTitulo.Text = string.IsNullOrWhiteSpace(tituloOriginal) ? "Digite o título" : tituloOriginal;
                        txtTitulo.ForeColor = string.IsNullOrWhiteSpace(tituloOriginal) ? Color.Gray : Color.Black;

                        txtObservacao.Text = string.IsNullOrWhiteSpace(observacaoOriginal) ? "Digite a observação" : observacaoOriginal;
                        txtObservacao.ForeColor = string.IsNullOrWhiteSpace(observacaoOriginal) ? Color.Gray : Color.Black;
                    } else {
                        tituloOriginal = string.Empty;
                        observacaoOriginal = string.Empty;

                        AplicarPlaceholder(txtTitulo, "Digite o título");
                        AplicarPlaceholder(txtObservacao, "Digite a observação");
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao carregar observação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarPlaceholder(RichTextBox textBox, string placeholder) {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
        }

        private void dataGridBancos_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (carregandoGrid || e.RowIndex < 0) return;

            var celula = dataGridBancos.Rows[e.RowIndex].Cells["NomeBanco"];
            if (celula.Value == null) return;

            selectedDatabase = celula.Value.ToString();
            lblBancoSelecionado.Text = "Banco Selecionado: " + selectedDatabase;
            CarregarObservacao();
        }

        private void AjustarDataGridView() {
            dataGridBancos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridBancos.RowHeadersVisible = false;
            dataGridBancos.AllowUserToAddRows = false;
            dataGridBancos.AllowUserToDeleteRows = false;
            dataGridBancos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridBancos.ReadOnly = true;
            dataGridBancos.AllowUserToResizeRows = false;
            dataGridBancos.MultiSelect = false;

            if (dataGridBancos.Columns.Contains("database_id")) {
                dataGridBancos.Columns["database_id"].HeaderText = "ID";
                dataGridBancos.Columns["database_id"].Width = 50;
            }

            if (dataGridBancos.Columns.Contains("NomeBanco")) {
                dataGridBancos.Columns["NomeBanco"].HeaderText = "Nome do Banco";
            }

            if (dataGridBancos.Columns.Contains("Titulo")) {
                dataGridBancos.Columns["Titulo"].HeaderText = "Assunto";
                dataGridBancos.Columns["Titulo"].Width = 200;
            }

            dataGridBancos.DefaultCellStyle.BackColor = Color.White;
            dataGridBancos.DefaultCellStyle.ForeColor = Color.Black;
            dataGridBancos.BackColor = ColorTranslator.FromHtml("#4682B4");
            dataGridBancos.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridBancos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridBancos.DefaultCellStyle.Font = new Font("Arial", 9);

            dataGridBancos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBancos.GridColor = Color.Gray;
            dataGridBancos.RowTemplate.Height = 23;
        }

        private void btnIniciarInovafarma_Click(object sender, EventArgs e) {
            IniciarInovaFarma();
        }

        private void CarregarInstanciasSQLAsync() {
            cmbInstancia.Items.Clear();

            foreach (var inst in ObterInstanciasLocais())
                cmbInstancia.Items.Add(inst);

            if (cmbInstancia.Items.Count > 0)
                cmbInstancia.SelectedIndex = 0;

            BuscarInstanciasRedeEmSegundoPlano();
        }

        private void BuscarInstanciasRedeEmSegundoPlano() {
            Task.Run(() => {
                List<string> encontradas = null;
                try {
                    var buscaTask = Task.Run(() => {
                        try {
                            return SqlDataSourceEnumerator.Instance.GetDataSources();
                        } catch {
                            return null;
                        }
                    });

                    if (!buscaTask.Wait(TimeSpan.FromSeconds(5)))
                        return;

                    var dt = buscaTask.Result;
                    if (dt == null) return;

                    encontradas = new List<string>();
                    foreach (DataRow row in dt.Rows) {
                        string servidor = row["ServerName"]?.ToString() ?? "";
                        string instancia = row["InstanceName"]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(servidor)) continue;

                        string nome = string.IsNullOrWhiteSpace(instancia)
                            ? servidor
                            : servidor + "\\" + instancia;

                        encontradas.Add(nome);
                    }
                } catch {
                    return;
                }

                if (encontradas == null || encontradas.Count == 0) return;

                try {
                    if (IsDisposed || Disposing) return;
                    if (!IsHandleCreated) return;

                    BeginInvoke(new Action(() => {
                        try {
                            if (IsDisposed || cmbInstancia.IsDisposed) return;

                            foreach (var inst in encontradas) {
                                bool jaExiste = false;
                                foreach (var item in cmbInstancia.Items) {
                                    if (string.Equals(item?.ToString(), inst, StringComparison.OrdinalIgnoreCase)) {
                                        jaExiste = true;
                                        break;
                                    }
                                }
                                if (!jaExiste)
                                    cmbInstancia.Items.Add(inst);
                            }
                        } catch { }
                    }));
                } catch { }
            });
        }

        private List<string> ObterInstanciasLocais() {
            var lista = new List<string>();
            string maquina = Environment.MachineName;

            foreach (var view in new[] { RegistryView.Registry64, RegistryView.Registry32 }) {
                try {
                    using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                    using (var key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\InstalledInstances")) {
                        if (key?.GetValue("InstalledInstances") is string[] instancias) {
                            foreach (var inst in instancias) {
                                string entrada = inst.Equals("MSSQLSERVER", StringComparison.OrdinalIgnoreCase)
                                    ? maquina
                                    : maquina + "\\" + inst;
                                if (!lista.Contains(entrada))
                                    lista.Add(entrada);
                            }
                        }
                    }
                } catch { }
            }

            if (lista.Count == 0) {
                try {
                    foreach (var svc in ServiceController.GetServices()) {
                        string nome = svc.ServiceName;
                        if (nome.Equals("MSSQLSERVER", StringComparison.OrdinalIgnoreCase)) {
                            if (!lista.Contains(maquina)) lista.Add(maquina);
                        } else if (nome.StartsWith("MSSQL$", StringComparison.OrdinalIgnoreCase)) {
                            string entrada = maquina + "\\" + nome.Substring(6);
                            if (!lista.Contains(entrada)) lista.Add(entrada);
                        }
                    }
                } catch { }
            }

            return lista;
        }

        private void btnSalvarObservacaoTitulo_Click(object sender, EventArgs e) {
            SalvarObservacao();
        }

        private void SalvarObservacao() {
            try {
                List<BancoInfo> bancos = new List<BancoInfo>();
                if (File.Exists(bancosTitulosFile)) {
                    bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));
                }

                int databaseId = ObterDatabaseIdDoBancoSelecionado();
                string instancia = cmbInstancia.SelectedItem?.ToString();
                string nomeBanco = selectedDatabase;

                var bancoSelecionado = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                if (bancoSelecionado == null) {
                    bancoSelecionado = new BancoInfo {
                        Instancia = instancia,
                        DatabaseId = databaseId,
                        NomeBanco = nomeBanco
                    };
                    bancos.Add(bancoSelecionado);
                }

                bancoSelecionado.Titulo = txtTitulo.Text == "Digite o título" ? "" : txtTitulo.Text;
                bancoSelecionado.Observacao = txtObservacao.Text == "Digite a observação" ? "" : txtObservacao.Text;

                File.WriteAllText(bancosTitulosFile, JsonConvert.SerializeObject(bancos, Formatting.Indented));

                AtualizarDataGridView();

                lblObservacaoSalva.ForeColor = Color.Green;
                lblObservacaoSalva.Text = "Observação salva com sucesso!";
                lblResetTimer.Start();
            } catch (Exception ex) {
                lblObservacaoSalva.ForeColor = Color.Red;
                lblObservacaoSalva.Text = "Erro ao salvar observação: " + ex.Message;
                lblResetTimer.Start();
            }
        }

        private void CarregarCredenciais() {
            try {
                if (File.Exists(configFile)) {
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile));

                    txtUsuarioInstancia.Text = config.Usuario ?? string.Empty;
                    txtUsuarioInstancia.ForeColor = string.IsNullOrWhiteSpace(config.Usuario) ? Color.Gray : Color.Black;

                    txtSenhaInstancia.Text = config.Senha ?? string.Empty;
                    txtSenhaInstancia.ForeColor = string.IsNullOrWhiteSpace(config.Senha) ? Color.Gray : Color.Black;
                    txtSenhaInstancia.PasswordChar = string.IsNullOrWhiteSpace(config.Senha) ? '\0' : '*';
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao carregar credenciais: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTitulo_Enter(object sender, EventArgs e) {
            if (txtTitulo.Text == "Digite o título") {
                txtTitulo.Text = "";
                txtTitulo.ForeColor = Color.Black;
            }
        }

        private void txtTitulo_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text)) {
                AplicarPlaceholder(txtTitulo, "Digite o título");
            }
        }

        private void txtObservacao_Enter(object sender, EventArgs e) {
            if (txtObservacao.Text == "Digite a observação") {
                txtObservacao.Text = "";
                txtObservacao.ForeColor = Color.Black;
            }
        }

        private void txtObservacao_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtObservacao.Text)) {
                AplicarPlaceholder(txtObservacao, "Digite a observação");
            }
        }

        private int ObterDatabaseIdDoBancoSelecionado() {
            if (dataGridBancos.SelectedRows.Count > 0) {
                return Convert.ToInt32(dataGridBancos.SelectedRows[0].Cells["database_id"].Value);
            }
            throw new Exception("Nenhum banco selecionado.");
        }

        private void AtualizarDataGridView() {
            try {
                List<BancoInfo> bancos = new List<BancoInfo>();
                if (File.Exists(bancosTitulosFile)) {
                    bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));
                }

                DataTable dt = (DataTable)dataGridBancos.DataSource;

                foreach (DataRow row in dt.Rows) {
                    int databaseId = Convert.ToInt32(row["database_id"]);
                    string instancia = cmbInstancia.SelectedItem?.ToString();
                    string nomeBanco = row["NomeBanco"].ToString();

                    var bancoInfo = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                    row["Titulo"] = bancoInfo != null ? bancoInfo.Titulo : string.Empty;
                }

                dataGridBancos.DataSource = dt;
                dataGridBancos.Refresh();
            } catch (Exception ex) {
                MessageBox.Show("Erro ao atualizar o DataGridView: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridBancos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            AjustarDataGridView();
        }

        private void txtTitulo_TextChanged(object sender, EventArgs e) {
            txtTitulo.Multiline = false;
            if (txtTitulo.Text != tituloOriginal && txtTitulo.Text != "Digite o título") {
                observacaoEditada = true;
                saveTimer.Stop();
                saveTimer.Start();
            }
        }

        private void txtObservacao_TextChanged(object sender, EventArgs e) {
            if (txtObservacao.Text != observacaoOriginal && txtObservacao.Text != "Digite a observação") {
                observacaoEditada = true;
                saveTimer.Stop();
                saveTimer.Start();
            }
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e) {
            panel2.BackColor = ColorTranslator.FromHtml("#4682B4");
        }

        private void EstilizarTextBoxes() {
            var fnt = new Font("Segoe UI", 10F, FontStyle.Regular);

            foreach (var tb in new[] { txtUsuarioInstancia, txtSenhaInstancia }) {
                tb.BorderStyle = BorderStyle.None;
                tb.BackColor = ColorTranslator.FromHtml("#F1F1F1");
                tb.Font = fnt;
                tb.ForeColor = Color.DimGray;
            }

            foreach (var tb in new[] { txtObservacao, txtTitulo }) {
                tb.BorderStyle = BorderStyle.None;
                tb.BackColor = ColorTranslator.FromHtml("#F1F1F1");
                tb.Font = fnt;
                tb.ForeColor = Color.DimGray;
            }

            txtSenhaInstancia.ForeColor = Color.Black;
            txtUsuarioInstancia.ForeColor = Color.Black;

            txtUsuarioInstancia.Size = new Size(341, 27);
            txtUsuarioInstancia.Multiline = true;
            txtSenhaInstancia.Size = new Size(341, 27);
            txtSenhaInstancia.Multiline = true;

            cmbInstancia.DrawMode = DrawMode.Normal;
            cmbInstancia.Size = new Size(341, 27);
            cmbInstancia.BackColor = ColorTranslator.FromHtml("#F1F1F1");
            cmbInstancia.ForeColor = Color.Black;

            txtTitulo.Size = new Size(594, 27);
            txtTitulo.Multiline = true;
            txtObservacao.Size = new Size(594, 78);
            txtObservacao.Multiline = true;

            txtTitulo.ForeColor = Color.Black;
            txtObservacao.ForeColor = Color.Black;
        }

        private void cmbInstancia_DrawItem(object sender, DrawItemEventArgs e) {
            try {
                if (e.Index < 0 || e.Index >= cmbInstancia.Items.Count) return;
                if (e.Bounds.Width <= 0 || e.Bounds.Height <= 0) return;

                string text = cmbInstancia.Items[e.Index].ToString();
                e.DrawBackground();

                Font fonte = e.Font ?? cmbInstancia.Font;
                bool selecionado = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                Color cor = selecionado ? Color.White : Color.Black;

                using (Brush textBrush = new SolidBrush(cor)) {
                    e.Graphics?.DrawString(text, fonte, textBrush, e.Bounds.X + 2, e.Bounds.Y + 4);
                }

                e.DrawFocusRectangle();
            } catch { }
        }

        private void txtUsuarioInstancia_Enter(object sender, EventArgs e) {
            if (txtUsuarioInstancia.Text == "Digite o usuário") {
                txtUsuarioInstancia.Text = "";
                txtUsuarioInstancia.ForeColor = Color.Black;
            }
        }

        private void txtUsuarioInstancia_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtUsuarioInstancia.Text)) {
                txtUsuarioInstancia.Text = "Digite o usuário";
                txtUsuarioInstancia.ForeColor = Color.Gray;
            }
        }

        private void txtSenhaInstancia_Enter(object sender, EventArgs e) {
            if (txtSenhaInstancia.Text == "Digite a senha") {
                txtSenhaInstancia.Text = "";
                txtSenhaInstancia.ForeColor = Color.Black;
                txtSenhaInstancia.PasswordChar = '*';
            }
        }

        private void txtSenhaInstancia_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtSenhaInstancia.Text)) {
                txtSenhaInstancia.Text = "Digite a senha";
                txtSenhaInstancia.ForeColor = Color.Gray;
                txtSenhaInstancia.PasswordChar = '\0';
            }
        }

        private void ConfigurarNotifyIcon() {
            ConfigurarMenuContexto();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                this.Hide();
            } else {
                SaveConfig();
                base.OnFormClosing(e);
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void ConfigurarMenuContexto() {
            if (menuContexto == null) {
                menuContexto = new ContextMenuStrip();
                var itemFechar = new ToolStripMenuItem("Fechar");
                itemFechar.Click += FecharSistema_Click;
                menuContexto.Items.Add(itemFechar);
                NotifyIcon.ContextMenuStrip = menuContexto;
            }
        }

        private void FecharSistema_Click(object sender, EventArgs e) {
            NotifyIcon.Visible = false;
            Application.Exit();
        }

        private void panelConectaAlterado_Paint(object sender, PaintEventArgs e) {
            panelConectaAlterado.BackColor = ColorTranslator.FromHtml("#ebf8ff");

            Color borderColor = ColorTranslator.FromHtml("#00A9FE");
            int borderWidth = 1;

            ControlPaint.DrawBorder(
                e.Graphics,
                panelConectaAlterado.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) {
            Color borderColor = ColorTranslator.FromHtml("#00A9FE");
            int borderWidth = 1;
            label1.ForeColor = ColorTranslator.FromHtml("#555555");

            ControlPaint.DrawBorder(
                e.Graphics,
                tableLayoutPanel2.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }
    }
}

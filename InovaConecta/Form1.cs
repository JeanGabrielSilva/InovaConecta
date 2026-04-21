using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;
using System.Threading;
// System.Data.Sql removido — SqlDataSourceEnumerator causava AccessViolationException via COM callbacks
using WinFormsTimer = System.Windows.Forms.Timer;  
using TimersTimer = System.Timers.Timer;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using static System.Windows.Forms.LinkLabel;
using Microsoft.Win32;
using System.ServiceProcess;


namespace InovaConecta {
    public partial class Form1 : Form {

        private WinFormsTimer lblResetTimer;
        private string configFile = Path.Combine(Application.StartupPath, "config.json");
        private string selectedDatabase = "";
        private string bancosTitulosFile = Path.Combine(Application.StartupPath, "bancos_titulos.json");
        private bool carregandoGrid = false;
        private bool atualizandoUI = false;

        private void AdicionarAoInicioComWindows() {
            try {
                string nomeApp = "InovaConecta"; // Nome no registro
                string caminhoApp = Application.ExecutablePath;

                RegistryKey chave = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (chave.GetValue(nomeApp) == null) {
                    chave.SetValue(nomeApp, "\"" + caminhoApp + "\"");
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao configurar inicialização automática: " + ex.Message);
            }
        }


        private bool observacaoEditada = false;
        private WinFormsTimer timer;

        public Form1() {
            InitializeComponent();
            LoadConfig(); // Carrega configurações salvas
            AdicionarAoInicioComWindows();
            EstilizarTextBoxes();
            CarregarInstanciasSQLAsync();
            dataGridBancos.ReadOnly = true; // Impede edição no DataGridView
            dataGridBancos.SelectionChanged += dataGridBancos_SelectionChanged;



            this.btnSalvarObservacaoTitulo = new System.Windows.Forms.Button();

            label1.ForeColor = ColorTranslator.FromHtml("#555555");

            // Configurações do botão Salvar Observação
            this.btnSalvarObservacaoTitulo.Location = new System.Drawing.Point(20, 130);
            this.btnSalvarObservacaoTitulo.Name = "btnSalvarObservacaoTitulo";
            this.btnSalvarObservacaoTitulo.Size = new System.Drawing.Size(100, 23);
            this.btnSalvarObservacaoTitulo.TabIndex = 14;
            this.btnSalvarObservacaoTitulo.Text = "Salvar Observação";
            this.btnSalvarObservacaoTitulo.UseVisualStyleBackColor = true;
            this.btnSalvarObservacaoTitulo.Click += new System.EventHandler(this.btnSalvarObservacaoTitulo_Click);

            dataGridBancos.CellClick += dataGridBancos_CellClick;
            labelConectaAlterado.Text = "Conecta alterado";
            labelConectaAlterado.ForeColor = Color.Green;
            labelConectaAlterado.Visible = false; // Inicialmente invisível

            panelConectaAlterado.Visible = false; // Inicialmente invisível

            // Configuração do Timer
            timer = new WinFormsTimer();
            timer.Interval = 5000; // 5000ms = 5 segundos
            timer.Tick += Timer_Tick;

            dataGridBancos.DataBindingComplete += DataGridBancos_DataBindingComplete;


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
            btnBuscarBanco.ForeColor = Color.White; // deixa o texto branco pra contrastar
            btnBuscarBanco.FlatStyle = FlatStyle.Flat; // remove o 3D padrão
            btnBuscarBanco.FlatAppearance.BorderSize = 0; // sem borda

            btnTrocarConecta.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnTrocarConecta.BackColor = ColorTranslator.FromHtml("#00A9FE");
            btnTrocarConecta.ForeColor = Color.White; // deixa o texto branco pra contrastar
            btnTrocarConecta.FlatStyle = FlatStyle.Flat; // remove o 3D padrão
            btnTrocarConecta.FlatAppearance.BorderSize = 0; // sem borda


            btnIniciarInovafarma.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnIniciarInovafarma.BackColor = ColorTranslator.FromHtml("#4682B4");
            btnIniciarInovafarma.ForeColor = Color.White; // deixa o texto branco pra contrastar
            btnIniciarInovafarma.FlatStyle = FlatStyle.Flat; // remove o 3D padrão
            btnIniciarInovafarma.FlatAppearance.BorderSize = 0; // sem borda

            cmbInstancia.DrawMode = DrawMode.Normal;


            // Placeholder inicial
            txtTitulo.Text = "Digite o título";
            txtTitulo.ForeColor = Color.Gray;

            txtObservacao.Text = "Digite a observação";
            txtObservacao.ForeColor = Color.Gray;

            // Vincular os eventos
            txtTitulo.Enter += txtTitulo_Enter;
            txtTitulo.Leave += txtTitulo_Leave;

            txtObservacao.Enter += txtObservacao_Enter;
            txtObservacao.Leave += txtObservacao_Leave;

            // Adicionar eventos para salvar automaticamente
            txtTitulo.TextChanged += txtTitulo_TextChanged;
            txtObservacao.TextChanged += txtObservacao_TextChanged;

            // Configurar placeholders iniciais
            AplicarPlaceholder(txtTitulo, "Digite o título");
            AplicarPlaceholder(txtObservacao, "Digite a observação");

            lblResetTimer = new WinFormsTimer();
            lblResetTimer.Interval = 3000; // Tempo de 3 segundos
            lblResetTimer.Tick += (s, args) => {
                lblObservacaoSalva.Text = ""; // Limpa a mensagem
                lblResetTimer.Stop(); // Para o timer
            };


            // Carregar as credenciais salvas do JSON
            CarregarCredenciais();

            // Placeholder inicial para txtUsuarioInstancia
            if (string.IsNullOrWhiteSpace(txtUsuarioInstancia.Text)) {
                txtUsuarioInstancia.Text = "Digite o usuário";
                txtUsuarioInstancia.ForeColor = Color.Gray;
            }
            txtUsuarioInstancia.Enter += txtUsuarioInstancia_Enter;
            txtUsuarioInstancia.Leave += txtUsuarioInstancia_Leave;

            // Placeholder inicial para txtSenhaInstancia
            if (string.IsNullOrWhiteSpace(txtSenhaInstancia.Text)) {
                txtSenhaInstancia.Text = "Digite a senha";
                txtSenhaInstancia.ForeColor = Color.Gray;
                txtSenhaInstancia.PasswordChar = '\0'; // Desativa o modo senha
            }
            txtSenhaInstancia.Enter += txtSenhaInstancia_Enter;
            txtSenhaInstancia.Leave += txtSenhaInstancia_Leave;

            ConfigurarNotifyIcon();



            saveTimer = new System.Windows.Forms.Timer();
            saveTimer.Interval = 500; // 3 segundos
            saveTimer.Tick += SaveTimer_Tick;

            timerPanel = new System.Windows.Forms.Timer();
            timerPanel.Interval = 500; // 3000 ms = 3 segundos
            timerPanel.Tick += TimerPanel_Tick;
            panelConectaAlterado.Visible = false; // começa invisível



        }
        private System.Windows.Forms.Timer timerPanel;

        private void TimerPanel_Tick(object sender, EventArgs e) {
            timerPanel.Stop();
            if (!IsDisposed && panelConectaAlterado != null)
                panelConectaAlterado.Visible = false;
        }


        private System.Windows.Forms.Timer saveTimer;

        private void SaveTimer_Tick(object sender, EventArgs e) {
            saveTimer.Stop();
            if (carregandoGrid) return;
            if (observacaoEditada) {
                SalvarObservacao();
                observacaoEditada = false;
            }
        }


        private async void btnBuscarBanco_Click(object sender, EventArgs e) {
            string instancia = cmbInstancia.SelectedItem?.ToString();
            string usuario = txtUsuarioInstancia.Text;
            string senha = txtSenhaInstancia.Text;

            if (string.IsNullOrWhiteSpace(instancia)) {
                MessageBox.Show("Nenhuma instância selecionada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnBuscarBanco.Enabled = false;

            while (true) {
                try {
                    DataTable dt = await Task.Run(() => BuscarBancosDaInstancia(instancia, usuario, senha));
                    AplicarResultadoBancosNaUI(dt, instancia, usuario, senha);
                    break;
                } catch (Exception ex) {
                    btnBuscarBanco.Enabled = true;
                    var resultado = MessageBox.Show(
                        "Erro ao buscar bancos de dados:\n" + ex.Message + "\n\nDeseja tentar novamente?",
                        "Erro de Conexão",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error);
                    if (resultado != DialogResult.Retry) break;
                    btnBuscarBanco.Enabled = false;
                }
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

            CriarConectaLog(selectedDatabase); // Passa o banco selecionado para o método CriarConectaLog
            ShowMessage();
        }

        private void btnCriarConectaLog_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(selectedDatabase)) {
                MessageBox.Show("Por favor, selecione um banco de dados!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CriarConectaLog(selectedDatabase); // Cria o conecta.log
        }

        private void CriarConectaLog(string bancoSelecionado) {
            try {
                string conectaPath = @"C:\inovafarma\conecta.log";
                string conectaXmlPath = @"C:\inovafarma\conecta.xml";

                // Fechar qualquer processo em execução relacionado ao InovaFarma ou Verificador
                FecharInovaFarma();

                // Apagar os arquivos conecta.log e conecta.xml, se existirem
                if (File.Exists(conectaPath)) {
                    File.Delete(conectaPath);
                }

                if (File.Exists(conectaXmlPath)) {
                    File.Delete(conectaXmlPath);
                }

                // Criar o novo arquivo conecta.log
                string conectaContent = $"Data Source={cmbInstancia.SelectedItem?.ToString()}; Initial Catalog={bancoSelecionado}; User ID={txtUsuarioInstancia.Text}; pwd={txtSenhaInstancia.Text}\n";
                File.WriteAllText(conectaPath, conectaContent, Encoding.Default);

                // Verifica se o arquivo foi criado corretamente
                int attempts = 0;
                while (!File.Exists(conectaPath) && attempts < 10) {
                    // Aguardar 200ms antes de tentar novamente
                    System.Threading.Thread.Sleep(200);
                    attempts++;
                }

                if (File.Exists(conectaPath)) {
                    labelConectaAlterado.Text = $"Conecta log criado para o banco:\n{bancoSelecionado}";
                    labelConectaAlterado.ForeColor = Color.Green;

                    panelConectaAlterado.Visible = true;  // Mostrar o painel
                    

                    timerPanel.Start();  // Iniciar contagem para sumir depois de 3 segundos
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
                    WorkingDirectory = @"C:\InovaFarma"  // Define o diretório de trabalho
                };
                Process.Start(startInfo);
            } catch (Exception ex) {
                MessageBox.Show("Erro ao iniciar InovaFarma: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SaveConfig() {
            try {
                string instanciaAtual = cmbInstancia.SelectedItem?.ToString() ?? "";

                // Acumula instâncias conhecidas para não perder histórico
                var instancias = CarregarInstanciasSalvas();
                if (!string.IsNullOrEmpty(instanciaAtual) && !instancias.Contains(instanciaAtual))
                    instancias.Add(instanciaAtual);

                var config = new {
                    Instancia = instanciaAtual,
                    Instancias = instancias,
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
                    string instanciaSalva = config?.Instancia ?? "";

                    if (!string.IsNullOrEmpty(instanciaSalva) && cmbInstancia.Items.Contains(instanciaSalva)) {
                        cmbInstancia.SelectedItem = instanciaSalva;
                    }

                    txtUsuarioInstancia.Text = config?.Usuario ?? "";
                    txtSenhaInstancia.Text = config?.Senha ?? "";
                } catch (Exception ex) {
                    MessageBox.Show("Erro ao carregar configurações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtInstancia_Leave(object sender, EventArgs e) {
            SaveConfig();
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


        private string tituloOriginal = string.Empty;
        private string observacaoOriginal = string.Empty;

        private void CarregarObservacao() {
            try {
                // Carregar o conteúdo atual do arquivo JSON
                if (File.Exists(bancosTitulosFile)) {
                    var bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));

                    // Obter o database_id, instância e nome do banco selecionado
                    int databaseId = ObterDatabaseIdDoBancoSelecionado();
                    string instancia = cmbInstancia.SelectedItem?.ToString(); // Nome da instância selecionada
                    string nomeBanco = selectedDatabase; // Nome do banco selecionado

                    // Buscar o banco selecionado pela combinação de instância, database_id e nome do banco
                    var bancoSelecionado = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                    if (bancoSelecionado != null) {
                        // Carregar os dados e armazenar os valores originais
                        tituloOriginal = bancoSelecionado.Titulo ?? string.Empty;
                        observacaoOriginal = bancoSelecionado.Observacao ?? string.Empty;

                        // Exibir os valores nos campos
                        txtTitulo.Text = string.IsNullOrWhiteSpace(tituloOriginal) ? "Digite o título" : tituloOriginal;
                        txtTitulo.ForeColor = string.IsNullOrWhiteSpace(tituloOriginal) ? Color.Gray : Color.Black;

                        txtObservacao.Text = string.IsNullOrWhiteSpace(observacaoOriginal) ? "Digite a observação" : observacaoOriginal;
                        txtObservacao.ForeColor = string.IsNullOrWhiteSpace(observacaoOriginal) ? Color.Gray : Color.Black;
                    } else {
                        // Banco não encontrado, resetar campos
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

        // Classe para representar as informações do banco
        public class BancoInfo {
            public string Instancia { get; set; } // Nome da instância do SQL Server
            public int DatabaseId { get; set; }   // Identificador único do banco de dados
            public string NomeBanco { get; set; } // Nome do banco de dados
            public string Titulo { get; set; }    // Título do banco
            public string Observacao { get; set; } // Observação do banco
        }


        private void dataGridBancos_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (carregandoGrid || e.RowIndex < 0) return;

            var celula = dataGridBancos.Rows[e.RowIndex].Cells["NomeBanco"];
            if (celula.Value == null) return;

            selectedDatabase = celula.Value.ToString();
            lblBancoSelecionado.Text = "Banco Selecionado: " + selectedDatabase;
            CarregarObservacao();
        }


        private void SelecionarBancoPintado(int rowIndex) {
            if (rowIndex >= 0 && rowIndex < dataGridBancos.Rows.Count) {
                // Atualiza a variável com o banco da célula ativa
                selectedDatabase = dataGridBancos.Rows[rowIndex].Cells[0].Value.ToString();

                // Marca a célula pintada (sem colorir) como a célula selecionada
                // Atualiza o foco na célula ativa
                dataGridBancos.CurrentCell = dataGridBancos.Rows[rowIndex].Cells[0];

                // Opcional: Se quiser apenas marcar a célula pintada para mostrar que foi selecionada (sem mudar a cor), comente a linha abaixo
                //dataGridBancos.Rows[rowIndex].Cells[0].Selected = true;
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            SaveConfig(); // Salva configurações ao fechar o programa
        }


        private void AjustarDataGridView() {
            dataGridBancos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridBancos.RowHeadersVisible = false; // Ocultar cabeçalho de linha
            dataGridBancos.AllowUserToAddRows = false; // Impedir adição de linhas
            dataGridBancos.AllowUserToDeleteRows = false; // Impedir exclusão de linhas
            dataGridBancos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  // Seleção por linha inteira
            dataGridBancos.ReadOnly = true; // Impedir edição das células
            dataGridBancos.AllowUserToResizeRows = false; // Impedir redimensionamento das linhas
            dataGridBancos.MultiSelect = false;

            // Configuração do título das colunas
            if (dataGridBancos.Columns.Contains("database_id")) {
                dataGridBancos.Columns["database_id"].HeaderText = "ID"; // Alterar o título da coluna para "ID"
                dataGridBancos.Columns["database_id"].Width = 50; // Definir largura pequena para a coluna "ID"
            }

            if (dataGridBancos.Columns.Contains("NomeBanco")) {
                dataGridBancos.Columns["NomeBanco"].HeaderText = "Nome do Banco"; // Alterar o título da coluna para "Nome do Banco"
            }

            if (dataGridBancos.Columns.Contains("Titulo")) {
                dataGridBancos.Columns["Titulo"].HeaderText = "Assunto"; // Alterar o título da coluna para "Título"
                dataGridBancos.Columns["Titulo"].Width = 200; // Definir largura para a coluna "Título"
            }

            // Personalização visual das células
            dataGridBancos.DefaultCellStyle.BackColor = Color.White; // Cor de fundo das células (branco)
            dataGridBancos.DefaultCellStyle.ForeColor = Color.Black; // Cor do texto
            dataGridBancos.BackColor = ColorTranslator.FromHtml("#4682B4"); // Cor de fundo da célula selecionada (verde mais escuro)
            dataGridBancos.DefaultCellStyle.SelectionForeColor = Color.White; // Cor do texto da célula selecionada

            // Fontes
            dataGridBancos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Fonte do cabeçalho
            dataGridBancos.DefaultCellStyle.Font = new Font("Arial", 9); // Fonte das células

            // Borda da célula
            dataGridBancos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBancos.GridColor = Color.Gray; // Cor das bordas da célula
            dataGridBancos.RowTemplate.Height = 23; // Altura das linhas

            // Estilo da linha ao passar o mouse
            dataGridBancos.BackColor = ColorTranslator.FromHtml("#4682B4"); // Cor da linha ao passar o mouse
            dataGridBancos.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void btnIniciarInovafarma_Click(object sender, EventArgs e) {
            IniciarInovaFarma();
        }



        private void CarregarInstanciasSQLAsync() {
            cmbInstancia.Items.Clear();

            // Instâncias locais (registro/serviços) — instantâneo
            var todas = new List<string>(ObterInstanciasLocais());

            // Instâncias salvas de conexões anteriores bem-sucedidas
            foreach (var inst in CarregarInstanciasSalvas())
                if (!todas.Contains(inst))
                    todas.Add(inst);

            foreach (var inst in todas)
                cmbInstancia.Items.Add(inst);

            if (cmbInstancia.Items.Count > 0)
                cmbInstancia.SelectedIndex = 0;
        }

        private List<string> CarregarInstanciasSalvas() {
            try {
                if (File.Exists(configFile)) {
                    var json = File.ReadAllText(configFile);
                    var config = JsonConvert.DeserializeObject<dynamic>(json);

                    // Lista de instâncias salvas (formato novo)
                    if (config?.Instancias != null)
                        return JsonConvert.DeserializeObject<List<string>>(config.Instancias.ToString()) ?? new List<string>();

                    // Compatibilidade com formato antigo (instância única)
                    string instAntiga = config?.Instancia ?? "";
                    if (!string.IsNullOrEmpty(instAntiga))
                        return new List<string> { instAntiga };
                }
            } catch { }
            return new List<string>();
        }

        private List<string> ObterInstanciasLocais() {
            var lista = new List<string>();
            string maquina = Environment.MachineName;

            // Tenta os dois hives (64-bit e 32-bit) — necessário pois o app pode rodar como 32-bit
            // mas o SQL Server registra no hive de 64-bit
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

            // Fallback: detecta pelo nome dos serviços do Windows (MSSQLSERVER / MSSQL$INSTANCIA)
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


        private void ShowMessage() {
            // Exibe a mensagem
            labelConectaAlterado.Visible = false;
            timer.Start(); // Inicia o timer
        }

        private void Timer_Tick(object sender, EventArgs e) {
            // Esconde a mensagem depois de 5 segundos
            labelConectaAlterado.Visible = false;
            timer.Stop(); // Para o timer
        }



        private void btnSalvarObservacaoTitulo_Click(object sender, EventArgs e) {
            SalvarObservacao();
        }

        private void SalvarObservacao() {
            try {
                // Carregar o conteúdo atual do arquivo JSON
                List<BancoInfo> bancos = new List<BancoInfo>();
                if (File.Exists(bancosTitulosFile)) {
                    bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));
                }

                // Obter o database_id, instância e nome do banco selecionado
                int databaseId = ObterDatabaseIdDoBancoSelecionado();
                string instancia = cmbInstancia.SelectedItem?.ToString(); // Nome da instância selecionada
                string nomeBanco = selectedDatabase; // Nome do banco selecionado

                // Encontrar o banco selecionado pela combinação de instância, database_id e nome do banco
                var bancoSelecionado = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                if (bancoSelecionado == null) {
                    bancoSelecionado = new BancoInfo {
                        Instancia = instancia,
                        DatabaseId = databaseId,
                        NomeBanco = nomeBanco
                    };
                    bancos.Add(bancoSelecionado);
                }

                // Atualizar campos sem salvar o placeholder
                bancoSelecionado.Titulo = txtTitulo.Text == "Digite o título" ? "" : txtTitulo.Text;
                bancoSelecionado.Observacao = txtObservacao.Text == "Digite a observação" ? "" : txtObservacao.Text;

                // Salvar no JSON
                File.WriteAllText(bancosTitulosFile, JsonConvert.SerializeObject(bancos, Formatting.Indented));

                // Atualizar DataGridView
                AtualizarDataGridView();

                lblObservacaoSalva.ForeColor = Color.Green; // Cor de sucesso
                lblObservacaoSalva.Text = "Observação salva com sucesso!";
                lblResetTimer.Start();
            } catch (Exception ex) {
                // Atualizar a label para exibir erro
                lblObservacaoSalva.ForeColor = Color.Red; // Cor de erro
                lblObservacaoSalva.Text = "Erro ao salvar observação: " + ex.Message;
                lblResetTimer.Start();
            }
        }

        public class Config {
            public string Usuario { get; set; }
            public string Senha { get; set; }
        }


        private void CarregarCredenciais() {
            try {
                // Verifica se o arquivo de configuração existe
                if (File.Exists(configFile)) {
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile));

                    // Carregar o usuário e senha salvos
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
                txtTitulo.ForeColor = Color.Black; // Texto digitado em preto
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
                txtObservacao.ForeColor = Color.Black; // Texto digitado em preto
            }
        }

        private void txtObservacao_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtObservacao.Text)) {
                AplicarPlaceholder(txtObservacao, "Digite a observação");
            }
        }




        private int ObterDatabaseIdDoBancoSelecionado() {
            // Verificar se há uma linha selecionada no DataGridView
            if (dataGridBancos.SelectedRows.Count > 0) {
                // Obter o database_id da linha selecionada
                return Convert.ToInt32(dataGridBancos.SelectedRows[0].Cells["database_id"].Value);
            }
            throw new Exception("Nenhum banco selecionado.");
        }

        private void AtualizarDataGridView() {
            try {
                // Carregar os dados do JSON
                List<BancoInfo> bancos = new List<BancoInfo>();
                if (File.Exists(bancosTitulosFile)) {
                    bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));
                }

                // Obter o DataTable atual do DataGridView
                DataTable dt = (DataTable)dataGridBancos.DataSource;

                // Atualizar os títulos no DataTable com base no database_id, instância e nome do banco
                foreach (DataRow row in dt.Rows) {
                    int databaseId = Convert.ToInt32(row["database_id"]); // Obter o database_id
                    string instancia = cmbInstancia.SelectedItem?.ToString(); // Obter a instância selecionada
                    string nomeBanco = row["NomeBanco"].ToString(); // Obter o nome do banco

                    // Buscar o banco no JSON pela combinação de instância, database_id e nome do banco
                    var bancoInfo = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                    if (bancoInfo != null) {
                        row["Titulo"] = bancoInfo.Titulo; // Atualizar o título
                    } else {
                        row["Titulo"] = string.Empty; // Limpar o título se não houver correspondência
                    }
                }

                // Atualizar o DataGridView
                dataGridBancos.DataSource = dt;
                dataGridBancos.Refresh(); // Forçar a atualização do DataGridView
            } catch (Exception ex) {
                MessageBox.Show("Erro ao atualizar o DataGridView: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridBancos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            // Ajusta o DataGridView após a vinculação de dados
            AjustarDataGridView();
        }

        private void txtTitulo_TextChanged(object sender, EventArgs e) {
            txtTitulo.Multiline = false;
            if (txtTitulo.Text != tituloOriginal && txtTitulo.Text != "Digite o título") {
                observacaoEditada = true;
                saveTimer.Stop();   // Para o timer caso esteja rodando
                saveTimer.Start();  // Reinicia o timer para esperar 3 segundos
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
            // cor de fundo cinza claro
            var back = ColorTranslator.FromHtml("#E0E0E0");
            // fonte Segoe UI 10 regular
            var fnt = new Font("Segoe UI", 10F, FontStyle.Regular);

            foreach (var tb in new[] { txtUsuarioInstancia, txtSenhaInstancia }) {
                tb.BorderStyle = BorderStyle.None;
                tb.BackColor = ColorTranslator.FromHtml("#F1F1F1");
                tb.Font = fnt;
                tb.ForeColor = Color.DimGray;  // cor do texto
            }

            foreach (var tb in new[] { txtObservacao, txtTitulo }) {
                tb.BorderStyle = BorderStyle.None;
                tb.BackColor = ColorTranslator.FromHtml("#F1F1F1");
                tb.Font = fnt;
                tb.ForeColor = Color.DimGray;  // cor do texto
            }

            txtSenhaInstancia.ForeColor = Color.Black;
            txtUsuarioInstancia.ForeColor = Color.Black;

            txtUsuarioInstancia.Size = new Size(341, 27);
            txtUsuarioInstancia.Multiline = true;
            txtSenhaInstancia.Size = new Size(341, 27);
            txtSenhaInstancia.Multiline = true;

            cmbInstancia.DrawMode = DrawMode.Normal;
            cmbInstancia.Size = new Size(341, 27); // Largura e altura do controle

            cmbInstancia.BackColor = ColorTranslator.FromHtml("#F1F1F1"); // Cor cinza claro
            cmbInstancia.ForeColor = Color.Black; // Texto preto

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
            } catch { /* protege contra AccessViolationException em redraws concorrentes */ }
        }

        private void labelConectaAlterado_Click(object sender, EventArgs e) {

        }

        private void cmbInstancia_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void txtUsuarioInstancia_TextChanged(object sender, EventArgs e) {

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
                txtSenhaInstancia.PasswordChar = '*'; // Ativa o modo senha
            }
        }

        private void txtSenhaInstancia_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtSenhaInstancia.Text)) {
                txtSenhaInstancia.Text = "Digite a senha";
                txtSenhaInstancia.ForeColor = Color.Gray;
                txtSenhaInstancia.PasswordChar = '\0'; // Desativa o modo senha
            }
        }


        private NotifyIcon notifyIcon; // Variável de nível de classe para garantir uma única instância

        private void ConfigurarNotifyIcon() {
            string caminhoIcone = Path.Combine(Application.StartupPath, "Logo-InovaConecta.ico");

            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "InovaConecta";
            notifyIcon.Visible = true;

            ConfigurarMenuContexto();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true; // Cancela o fechamento do formulário
                this.Hide(); // Oculta o formulário
            } else {
                base.OnFormClosing(e);
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e) {
            this.Show(); // Mostra o formulário novamente
            this.WindowState = FormWindowState.Normal; // Restaura o estado original
        }

        private void OcultarFormulario() {
            this.ShowInTaskbar = false; // Remove da barra de tarefas
            this.Hide(); // Oculta o formulário
        }

        private void RestaurarFormulario() {
            this.ShowInTaskbar = true; // Exibe na barra de tarefas
            this.Show(); // Mostra o formulário
            this.WindowState = FormWindowState.Normal;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.Show(); // Mostra o formulário novamente
            this.WindowState = FormWindowState.Normal; // Restaura o estado original
        }


        private ContextMenuStrip menuContexto;

        private void ConfigurarMenuContexto() {
            if (menuContexto == null) { // Verifica se o menu já foi configurado
                menuContexto = new ContextMenuStrip();

                // Criar item "Fechar"
                var itemFechar = new ToolStripMenuItem("Fechar");
                itemFechar.Click += FecharSistema_Click; // Associa o evento de clique

                // Adiciona o item ao menu
                menuContexto.Items.Add(itemFechar);

                // Associa o menu ao NotifyIcon
                NotifyIcon.ContextMenuStrip = menuContexto;
            }
        }

        private void FecharSistema_Click(object sender, EventArgs e) {
            NotifyIcon.Visible = false; // Ocultar o ícone na bandeja
            Application.Exit(); // Encerrar o sistema
        }

        private void lblDetalhesBanco_Click(object sender, EventArgs e) {

        }

        private void panelConectaAlterado_Paint(object sender, PaintEventArgs e) {
            panelConectaAlterado.BackColor = System.Drawing.ColorTranslator.FromHtml("#ebf8ff");

            Color borderColor = ColorTranslator.FromHtml("#00A9FE");
            int borderWidth = 1;

            Rectangle rect = new Rectangle(0, 0, panelConectaAlterado.Width - 1, panelConectaAlterado.Height - 1);

            ControlPaint.DrawBorder(
                e.Graphics,
                panelConectaAlterado.ClientRectangle,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid);

        }

        private void txtObservacao_TextChanged_1(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {
            label1.ForeColor = ColorTranslator.FromHtml("#555555");
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) {
            Color borderColor = ColorTranslator.FromHtml("#00A9FE");
            int borderWidth = 1;
            label1.ForeColor = ColorTranslator.FromHtml("#555555");


            Rectangle rect = new Rectangle(0, 0, tableLayoutPanel2.Width - 1, tableLayoutPanel2.Height - 1);

            ControlPaint.DrawBorder(
                e.Graphics,
                tableLayoutPanel2.ClientRectangle,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid,
                borderColor,
                borderWidth, ButtonBorderStyle.Solid);
        }

        private void label2_Click(object sender, EventArgs e) {
            Rectangle rect = new Rectangle(0, 0, tableLayoutPanel2.Width - 1, tableLayoutPanel2.Height - 1);

        }

        private void panel8_Paint(object sender, PaintEventArgs e) {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e) {

        }

        private void panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void btnTestar100Buscas_Click_1(object sender, EventArgs e) {

        }
    }
}
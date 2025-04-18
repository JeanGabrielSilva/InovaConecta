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
using System.Data.Sql;
using WinFormsTimer = System.Windows.Forms.Timer;  
using TimersTimer = System.Timers.Timer;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InovaConecta {
    public partial class Form1 : Form {

        private string configFile = Path.Combine(Application.StartupPath, "config.json");
        private string selectedDatabase = "";
        private string bancosTitulosFile = Path.Combine(Application.StartupPath, "bancos_titulos.json");


        private WinFormsTimer timer;

        public Form1() {
            InitializeComponent();
            LoadConfig(); // Carrega configurações salvas
            CarregarInstanciasSQLAsync();
            dataGridBancos.ReadOnly = true; // Impede edição no DataGridView
            dataGridBancos.SelectionChanged += dataGridBancos_SelectionChanged;

            this.btnSalvarObservacaoTitulo = new System.Windows.Forms.Button();
            this.Controls.Add(this.btnSalvarObservacaoTitulo);

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

            // Configuração do Timer
            timer = new WinFormsTimer();
            timer.Interval = 5000; // 5000ms = 5 segundos
            timer.Tick += Timer_Tick;

            dataGridBancos.DataBindingComplete += DataGridBancos_DataBindingComplete;
        }

        private void btnBuscarBanco_Click(object sender, EventArgs e) {
            // Construção da string de conexão com as credenciais fornecidas pelo usuário
            string connString = $"Server={cmbInstancia.SelectedItem?.ToString()};User Id={txtUsuarioInstancia.Text};Password={txtSenhaInstancia.Text};";

            try {
                // Tentativa de conexão com o banco de dados
                using (SqlConnection conn = new SqlConnection(connString)) {
                    conn.Open();

                    // Query para obter os bancos de dados online, excluindo os sistemas internos
                    string query = @"
                SELECT database_id, name 
                FROM sys.databases 
                WHERE database_id > 4
                AND state_desc = 'ONLINE'
                AND name NOT IN ('master', 'tempdb', 'model', 'msdb', 'INOVAFARMA_UPDATE', 'INOVAFARMA_API', 'INOVAFARMA_SERVICE', 'TEMP_BAK', 'inovafarma_service', 'Inovafarma_API', 'inovafarma_api')";

                    // Preenchendo um DataTable com os resultados da query
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Renomear a coluna "name" para "NomeBanco"
                    dt.Columns["name"].ColumnName = "NomeBanco";

                    // Carregar os dados do JSON
                    List<BancoInfo> bancos = new List<BancoInfo>();
                    if (File.Exists(bancosTitulosFile)) {
                        bancos = JsonConvert.DeserializeObject<List<BancoInfo>>(File.ReadAllText(bancosTitulosFile));
                    }

                    // Adicionar a coluna de título ao DataTable
                    dt.Columns.Add("Titulo", typeof(string));

                    // Combinar os dados do JSON com os dados dos bancos
                    foreach (DataRow row in dt.Rows) {
                        int databaseId = Convert.ToInt32(row["database_id"]); // Obter o database_id
                        string instancia = cmbInstancia.SelectedItem?.ToString(); // Obter a instância selecionada
                        string nomeBanco = row["NomeBanco"].ToString(); // Obter o nome do banco

                        // Buscar o banco no JSON pela combinação de instância, database_id e nome do banco
                        var bancoInfo = bancos.Find(b => b.Instancia == instancia && b.DatabaseId == databaseId && b.NomeBanco == nomeBanco);
                        if (bancoInfo != null) {
                            row["Titulo"] = bancoInfo.Titulo;
                        } else {
                            row["Titulo"] = string.Empty;
                        }
                    }

                    // Exibindo os resultados no DataGrid
                    dataGridBancos.DataSource = dt;

                    AjustarDataGridView();

                    // Garantir que nenhum banco esteja selecionado ao iniciar
                    dataGridBancos.ClearSelection();

                    // Salva as credenciais no JSON após a busca do banco (se os campos não estiverem vazios)
                    if (!string.IsNullOrEmpty(cmbInstancia.SelectedItem?.ToString()) &&
                        !string.IsNullOrEmpty(txtUsuarioInstancia.Text) &&
                        !string.IsNullOrEmpty(txtSenhaInstancia.Text)) {
                        SaveConfig(); // Chama o método para salvar as credenciais
                    }

                    // Ajusta o DataGridView conforme necessário (por exemplo, largura das colunas, etc.)

                    dataGridBancos.Invalidate();
                }
            } catch (Exception ex) {
                // Exibe uma mensagem de erro caso a conexão falhe
                MessageBox.Show("Erro ao conectar ao SQL Server: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // Se o arquivo foi criado, atualize o label com a mensagem
                    labelConectaAlterado.Text = $"Conecta log criado para o banco:\n{bancoSelecionado}";
                    labelConectaAlterado.ForeColor = Color.Green;
                    labelConectaAlterado.Visible = true;  // Exibe o label
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
                var config = new {
                    Instancia = cmbInstancia.SelectedItem?.ToString(),
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
            // Verifica se há uma linha selecionada
            if (dataGridBancos.SelectedRows.Count > 0) {
                // Força a seleção da célula que contém o nome do banco
                dataGridBancos.CurrentCell = dataGridBancos.SelectedRows[0].Cells["NomeBanco"];

                // Atualiza o banco selecionado
                selectedDatabase = dataGridBancos.SelectedRows[0].Cells["NomeBanco"].Value.ToString();

                // Atualiza a label para mostrar o nome do banco selecionado
                lblBancoSelecionado.Text = "Banco Selecionado: " + selectedDatabase;

                // Carrega o título e a observação do banco selecionado
                CarregarObservacao();
            }
        }

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
                        txtTitulo.Text = bancoSelecionado.Titulo;
                        txtObservacao.Text = bancoSelecionado.Observacao;
                    } else {
                        txtTitulo.Clear();
                        txtObservacao.Clear();
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro ao carregar observação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            // Verifica se o clique foi em uma célula válida (não no cabeçalho)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) {
                // Força a seleção da célula que contém o nome do banco
                dataGridBancos.CurrentCell = dataGridBancos.Rows[e.RowIndex].Cells["NomeBanco"];

                // Atualiza o banco selecionado
                selectedDatabase = dataGridBancos.Rows[e.RowIndex].Cells["NomeBanco"].Value.ToString();

                // Atualiza a label para mostrar o nome do banco selecionado
                lblBancoSelecionado.Text = "Banco Selecionado: " + selectedDatabase;

                // Carrega o título e a observação do banco selecionado
                CarregarObservacao();
            }
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
            // Configurações de exibição para o DataGridView
            dataGridBancos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // Preencher o espaço
            dataGridBancos.AutoResizeColumns(); // Ajuste automático das colunas
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
                dataGridBancos.Columns["Titulo"].HeaderText = "Título"; // Alterar o título da coluna para "Título"
                dataGridBancos.Columns["Titulo"].Width = 200; // Definir largura para a coluna "Título"
            }

            // Personalização visual das células
            dataGridBancos.DefaultCellStyle.BackColor = Color.White; // Cor de fundo das células (branco)
            dataGridBancos.DefaultCellStyle.ForeColor = Color.Black; // Cor do texto
            dataGridBancos.DefaultCellStyle.SelectionBackColor = Color.ForestGreen; // Cor de fundo da célula selecionada (verde mais escuro)
            dataGridBancos.DefaultCellStyle.SelectionForeColor = Color.White; // Cor do texto da célula selecionada

            // Fontes
            dataGridBancos.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Fonte do cabeçalho
            dataGridBancos.DefaultCellStyle.Font = new Font("Arial", 9); // Fonte das células

            // Borda da célula
            dataGridBancos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBancos.GridColor = Color.Gray; // Cor das bordas da célula
            dataGridBancos.RowTemplate.Height = 23; // Altura das linhas

            // Estilo da linha ao passar o mouse
            dataGridBancos.DefaultCellStyle.SelectionBackColor = Color.DarkGreen; // Cor da linha ao passar o mouse
            dataGridBancos.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void btnIniciarInovafarma_Click(object sender, EventArgs e) {
            IniciarInovaFarma();
        }



        private async void CarregarInstanciasSQLAsync() {
            try {
                // Exibe um indicador de carregamento (opcional)
                cmbInstancia.Items.Clear();
                cmbInstancia.Items.Add("Carregando instâncias...");
                cmbInstancia.SelectedIndex = 0;

                // Executa a busca das instâncias SQL em uma thread separada
                var servidores = await Task.Run(() => SqlDataSourceEnumerator.Instance.GetDataSources());

                // Atualiza a interface do usuário após a conclusão da busca
                cmbInstancia.Items.Clear();
                foreach (DataRow row in servidores.Rows) {
                    string servidor = row["ServerName"].ToString();
                    string instancia = row["InstanceName"].ToString();

                    if (!string.IsNullOrEmpty(instancia))
                        servidor += "\\" + instancia; // Adiciona a instância ao nome do servidor

                    cmbInstancia.Items.Add(servidor);
                }

                if (cmbInstancia.Items.Count > 0)
                    cmbInstancia.SelectedIndex = 0; // Seleciona a primeira instância disponível por padrão
            } catch (Exception ex) {
                MessageBox.Show("Erro ao buscar instâncias do SQL Server: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowMessage() {
            // Exibe a mensagem
            labelConectaAlterado.Visible = true;
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
                    // Se o banco não existir, criar um novo
                    bancoSelecionado = new BancoInfo {
                        Instancia = instancia,
                        DatabaseId = databaseId,
                        NomeBanco = nomeBanco
                    };
                    bancos.Add(bancoSelecionado);
                }

                // Atualizar o título e a observação
                bancoSelecionado.Titulo = txtTitulo.Text;
                bancoSelecionado.Observacao = txtObservacao.Text;

                // Salvar as alterações no arquivo JSON
                File.WriteAllText(bancosTitulosFile, JsonConvert.SerializeObject(bancos, Formatting.Indented));

                // Atualizar o DataGridView para refletir as mudanças
                AtualizarDataGridView();

                MessageBox.Show("Observação salva com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show("Erro ao salvar observação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        private void label3_Click(object sender, EventArgs e) {

        }
    }
}
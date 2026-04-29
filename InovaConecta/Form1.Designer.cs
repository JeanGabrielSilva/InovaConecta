using System.Drawing;

namespace InovaConecta {
    partial class Form1 {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridBancos = new System.Windows.Forms.DataGridView();
            this.lblObservacao = new System.Windows.Forms.Label();
            this.btnSalvarObservacaoTitulo = new System.Windows.Forms.Button();
            this.txtObservacao = new System.Windows.Forms.RichTextBox();
            this.txtTitulo = new System.Windows.Forms.RichTextBox();
            this.lblAssunto = new System.Windows.Forms.Label();
            this.txtUsuarioInstancia = new System.Windows.Forms.TextBox();
            this.txtSenhaInstancia = new System.Windows.Forms.TextBox();
            this.lblInstancia = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblSenha = new System.Windows.Forms.Label();
            this.btnBuscarBanco = new System.Windows.Forms.Button();
            this.btnTrocarConecta = new System.Windows.Forms.Button();
            this.btnIniciarInovafarma = new System.Windows.Forms.Button();
            this.cmbInstancia = new System.Windows.Forms.ComboBox();
            this.labelConectaAlterado = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblInovaConecta = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblInformeDados = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblObservacaoSalva = new System.Windows.Forms.Label();
            this.lblBancoSelecionado = new System.Windows.Forms.Label();
            this.lblDetalhesBanco = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelConectaAlterado = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBancos)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelConectaAlterado.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridBancos
            // 
            this.dataGridBancos.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridBancos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridBancos.Location = new System.Drawing.Point(22, 37);
            this.dataGridBancos.Name = "dataGridBancos";
            this.dataGridBancos.Size = new System.Drawing.Size(594, 204);
            this.dataGridBancos.TabIndex = 0;
            // 
            // lblObservacao
            // 
            this.lblObservacao.AutoSize = true;
            this.lblObservacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacao.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblObservacao.Location = new System.Drawing.Point(19, 302);
            this.lblObservacao.Name = "lblObservacao";
            this.lblObservacao.Size = new System.Drawing.Size(152, 24);
            this.lblObservacao.TabIndex = 6;
            this.lblObservacao.Text = "OBSERVAÇÃO";
            // 
            // btnSalvarObservacaoTitulo
            // 
            this.btnSalvarObservacaoTitulo.Location = new System.Drawing.Point(392, 294);
            this.btnSalvarObservacaoTitulo.Name = "btnSalvarObservacaoTitulo";
            this.btnSalvarObservacaoTitulo.Size = new System.Drawing.Size(71, 24);
            this.btnSalvarObservacaoTitulo.TabIndex = 4;
            this.btnSalvarObservacaoTitulo.Text = "Salvar ";
            this.btnSalvarObservacaoTitulo.UseVisualStyleBackColor = true;
            this.btnSalvarObservacaoTitulo.Visible = false;
            this.btnSalvarObservacaoTitulo.Click += new System.EventHandler(this.btnSalvarObservacaoTitulo_Click);
            // 
            // txtObservacao
            // 
            this.txtObservacao.Location = new System.Drawing.Point(22, 324);
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(594, 73);
            this.txtObservacao.TabIndex = 3;
            this.txtObservacao.Text = "";
            this.txtObservacao.TextChanged += new System.EventHandler(this.txtObservacao_TextChanged);
            // 
            // txtTitulo
            // 
            this.txtTitulo.Location = new System.Drawing.Point(22, 273);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(594, 18);
            this.txtTitulo.TabIndex = 2;
            this.txtTitulo.Text = "";
            this.txtTitulo.TextChanged += new System.EventHandler(this.txtTitulo_TextChanged);
            // 
            // lblAssunto
            // 
            this.lblAssunto.AutoSize = true;
            this.lblAssunto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssunto.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblAssunto.Location = new System.Drawing.Point(19, 249);
            this.lblAssunto.Name = "lblAssunto";
            this.lblAssunto.Size = new System.Drawing.Size(108, 24);
            this.lblAssunto.TabIndex = 5;
            this.lblAssunto.Text = "ASSUNTO";
            // 
            // txtUsuarioInstancia
            // 
            this.txtUsuarioInstancia.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuarioInstancia.ForeColor = System.Drawing.Color.Black;
            this.txtUsuarioInstancia.Location = new System.Drawing.Point(15, 125);
            this.txtUsuarioInstancia.Multiline = true;
            this.txtUsuarioInstancia.Name = "txtUsuarioInstancia";
            this.txtUsuarioInstancia.Size = new System.Drawing.Size(341, 27);
            this.txtUsuarioInstancia.TabIndex = 2;
            this.txtUsuarioInstancia.Text = "Digite o usuário";
            // 
            // txtSenhaInstancia
            // 
            this.txtSenhaInstancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenhaInstancia.ForeColor = System.Drawing.Color.Black;
            this.txtSenhaInstancia.Location = new System.Drawing.Point(16, 194);
            this.txtSenhaInstancia.Multiline = true;
            this.txtSenhaInstancia.Name = "txtSenhaInstancia";
            this.txtSenhaInstancia.Size = new System.Drawing.Size(341, 27);
            this.txtSenhaInstancia.TabIndex = 3;
            this.txtSenhaInstancia.Text = "Digite a senha";
            // 
            // lblInstancia
            // 
            this.lblInstancia.AutoSize = true;
            this.lblInstancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstancia.Location = new System.Drawing.Point(12, 37);
            this.lblInstancia.Name = "lblInstancia";
            this.lblInstancia.Size = new System.Drawing.Size(105, 22);
            this.lblInstancia.TabIndex = 4;
            this.lblInstancia.Text = "INSTÂNCIA";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(12, 100);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(91, 22);
            this.lblUsuario.TabIndex = 5;
            this.lblUsuario.Text = "USUÁRIO";
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenha.Location = new System.Drawing.Point(12, 169);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(72, 22);
            this.lblSenha.TabIndex = 6;
            this.lblSenha.Text = "SENHA";
            // 
            // btnBuscarBanco
            // 
            this.btnBuscarBanco.Location = new System.Drawing.Point(16, 239);
            this.btnBuscarBanco.Name = "btnBuscarBanco";
            this.btnBuscarBanco.Size = new System.Drawing.Size(340, 34);
            this.btnBuscarBanco.TabIndex = 7;
            this.btnBuscarBanco.Text = "Buscar Bancos";
            this.btnBuscarBanco.UseVisualStyleBackColor = true;
            this.btnBuscarBanco.Click += new System.EventHandler(this.btnBuscarBanco_Click);
            // 
            // btnTrocarConecta
            // 
            this.btnTrocarConecta.Location = new System.Drawing.Point(16, 279);
            this.btnTrocarConecta.Name = "btnTrocarConecta";
            this.btnTrocarConecta.Size = new System.Drawing.Size(340, 34);
            this.btnTrocarConecta.TabIndex = 8;
            this.btnTrocarConecta.Text = "Trocar Conecta";
            this.btnTrocarConecta.UseVisualStyleBackColor = true;
            this.btnTrocarConecta.Click += new System.EventHandler(this.btnTrocarConecta_Click);
            // 
            // btnIniciarInovafarma
            // 
            this.btnIniciarInovafarma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIniciarInovafarma.Font = new System.Drawing.Font("Unispace", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciarInovafarma.Image = ((System.Drawing.Image)(resources.GetObject("btnIniciarInovafarma.Image")));
            this.btnIniciarInovafarma.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnIniciarInovafarma.Location = new System.Drawing.Point(413, 13);
            this.btnIniciarInovafarma.Name = "btnIniciarInovafarma";
            this.btnIniciarInovafarma.Size = new System.Drawing.Size(144, 65);
            this.btnIniciarInovafarma.TabIndex = 10;
            this.btnIniciarInovafarma.Text = "Iniciar Inovafarma";
            this.btnIniciarInovafarma.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnIniciarInovafarma.UseVisualStyleBackColor = true;
            this.btnIniciarInovafarma.Click += new System.EventHandler(this.btnIniciarInovafarma_Click);
            // 
            // cmbInstancia
            // 
            this.cmbInstancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbInstancia.FormattingEnabled = true;
            this.cmbInstancia.Location = new System.Drawing.Point(16, 63);
            this.cmbInstancia.Name = "cmbInstancia";
            this.cmbInstancia.Size = new System.Drawing.Size(203, 28);
            this.cmbInstancia.TabIndex = 11;
            // 
            // labelConectaAlterado
            // 
            this.labelConectaAlterado.AutoSize = true;
            this.labelConectaAlterado.Location = new System.Drawing.Point(191, 345);
            this.labelConectaAlterado.Name = "labelConectaAlterado";
            this.labelConectaAlterado.Size = new System.Drawing.Size(165, 26);
            this.labelConectaAlterado.TabIndex = 12;
            this.labelConectaAlterado.Text = "Conecta log criado para o banco:\r\n{bancoSelecionado}";
            this.labelConectaAlterado.Visible = false;
            // 
            // panel1
            // 
            this.panel1.ColumnCount = 3;
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.panel1.Controls.Add(this.panel2, 1, 0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 490);
            this.panel1.Name = "panel1";
            this.panel1.RowCount = 2;
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.panel1.Size = new System.Drawing.Size(1008, 111);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(19, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 91);
            this.panel2.TabIndex = 0;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint_1);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnIniciarInovafarma, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(970, 91);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 370F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel6, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel7, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1008, 490);
            this.tableLayoutPanel4.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(373, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(632, 39);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(364, 39);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lblInovaConecta);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(364, 39);
            this.panel5.TabIndex = 15;
            // 
            // lblInovaConecta
            // 
            this.lblInovaConecta.AutoSize = true;
            this.lblInovaConecta.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInovaConecta.Location = new System.Drawing.Point(12, 17);
            this.lblInovaConecta.Name = "lblInovaConecta";
            this.lblInovaConecta.Size = new System.Drawing.Size(125, 22);
            this.lblInovaConecta.TabIndex = 5;
            this.lblInovaConecta.Text = "Inova Conecta";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnBuscarBanco);
            this.panel6.Controls.Add(this.labelConectaAlterado);
            this.panel6.Controls.Add(this.lblInformeDados);
            this.panel6.Controls.Add(this.txtUsuarioInstancia);
            this.panel6.Controls.Add(this.lblUsuario);
            this.panel6.Controls.Add(this.btnTrocarConecta);
            this.panel6.Controls.Add(this.cmbInstancia);
            this.panel6.Controls.Add(this.txtSenhaInstancia);
            this.panel6.Controls.Add(this.lblSenha);
            this.panel6.Controls.Add(this.lblInstancia);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 48);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(364, 439);
            this.panel6.TabIndex = 2;
            // 
            // lblInformeDados
            // 
            this.lblInformeDados.AutoSize = true;
            this.lblInformeDados.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformeDados.Location = new System.Drawing.Point(11, 0);
            this.lblInformeDados.Name = "lblInformeDados";
            this.lblInformeDados.Size = new System.Drawing.Size(350, 22);
            this.lblInformeDados.TabIndex = 13;
            this.lblInformeDados.Text = "INFORME OS DADOS DA INSTÂNCIA";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnSalvarObservacaoTitulo);
            this.panel7.Controls.Add(this.lblObservacaoSalva);
            this.panel7.Controls.Add(this.lblObservacao);
            this.panel7.Controls.Add(this.lblBancoSelecionado);
            this.panel7.Controls.Add(this.txtObservacao);
            this.panel7.Controls.Add(this.dataGridBancos);
            this.panel7.Controls.Add(this.lblDetalhesBanco);
            this.panel7.Controls.Add(this.txtTitulo);
            this.panel7.Controls.Add(this.lblAssunto);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(373, 48);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(632, 439);
            this.panel7.TabIndex = 3;
            // 
            // lblObservacaoSalva
            // 
            this.lblObservacaoSalva.AutoSize = true;
            this.lblObservacaoSalva.Location = new System.Drawing.Point(212, 257);
            this.lblObservacaoSalva.Name = "lblObservacaoSalva";
            this.lblObservacaoSalva.Size = new System.Drawing.Size(0, 13);
            this.lblObservacaoSalva.TabIndex = 15;
            // 
            // lblBancoSelecionado
            // 
            this.lblBancoSelecionado.AutoSize = true;
            this.lblBancoSelecionado.Enabled = false;
            this.lblBancoSelecionado.Location = new System.Drawing.Point(20, 22);
            this.lblBancoSelecionado.Name = "lblBancoSelecionado";
            this.lblBancoSelecionado.Size = new System.Drawing.Size(98, 13);
            this.lblBancoSelecionado.TabIndex = 9;
            this.lblBancoSelecionado.Text = "Banco selecionado";
            this.lblBancoSelecionado.Visible = false;
            // 
            // lblDetalhesBanco
            // 
            this.lblDetalhesBanco.AutoSize = true;
            this.lblDetalhesBanco.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetalhesBanco.Location = new System.Drawing.Point(18, 0);
            this.lblDetalhesBanco.Name = "lblDetalhesBanco";
            this.lblDetalhesBanco.Size = new System.Drawing.Size(332, 22);
            this.lblDetalhesBanco.TabIndex = 14;
            this.lblDetalhesBanco.Text = "DETALHES DO BANCO DE DADOS";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Controls.Add(this.panelConectaAlterado, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 448);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1008, 42);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // panelConectaAlterado
            // 
            this.panelConectaAlterado.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelConectaAlterado.Controls.Add(this.tableLayoutPanel2);
            this.panelConectaAlterado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConectaAlterado.Location = new System.Drawing.Point(19, 3);
            this.panelConectaAlterado.Name = "panelConectaAlterado";
            this.panelConectaAlterado.Size = new System.Drawing.Size(970, 36);
            this.panelConectaAlterado.TabIndex = 0;
            this.panelConectaAlterado.Paint += new System.Windows.Forms.PaintEventHandler(this.panelConectaAlterado_Paint);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel8, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel9, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(970, 36);
            this.tableLayoutPanel2.TabIndex = 15;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(43, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(924, 30);
            this.panel8.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Conecta trocado com sucesso";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(34, 30);
            this.panel9.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Image = global::InovaConecta.Properties.Resources.Informe;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 30);
            this.label2.TabIndex = 1;
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "InovaConecta";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "InovaConecta";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBancos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelConectaAlterado.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridBancos;
        private System.Windows.Forms.TextBox txtUsuarioInstancia;
        private System.Windows.Forms.TextBox txtSenhaInstancia;
        private System.Windows.Forms.Label lblInstancia;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Button btnBuscarBanco;
        private System.Windows.Forms.Button btnTrocarConecta;
        private System.Windows.Forms.Button btnIniciarInovafarma;
        private System.Windows.Forms.RichTextBox txtObservacao;
        private System.Windows.Forms.RichTextBox txtTitulo;
        private System.Windows.Forms.ComboBox cmbInstancia;
        private System.Windows.Forms.Label labelConectaAlterado;
        private System.Windows.Forms.Button btnSalvarObservacaoTitulo;
        private System.Windows.Forms.Label lblAssunto;
        private System.Windows.Forms.Label lblObservacao;
        private System.Windows.Forms.TableLayoutPanel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblInovaConecta;
        private System.Windows.Forms.Label lblInformeDados;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lblDetalhesBanco;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelConectaAlterado;
        private System.Windows.Forms.Label lblObservacaoSalva;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.Label lblBancoSelecionado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel9;
    }



}


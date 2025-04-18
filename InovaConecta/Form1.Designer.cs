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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridBancos = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSalvarObservacaoTitulo = new System.Windows.Forms.Button();
            this.txtObservacao = new System.Windows.Forms.RichTextBox();
            this.txtTitulo = new System.Windows.Forms.RichTextBox();
            this.titulo = new System.Windows.Forms.Label();
            this.txtUsuarioInstancia = new System.Windows.Forms.TextBox();
            this.txtSenhaInstancia = new System.Windows.Forms.TextBox();
            this.dsa = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBuscarBanco = new System.Windows.Forms.Button();
            this.btnTrocarConecta = new System.Windows.Forms.Button();
            this.lblBancoSelecionado = new System.Windows.Forms.Label();
            this.btnIniciarInovafarma = new System.Windows.Forms.Button();
            this.cmbInstancia = new System.Windows.Forms.ComboBox();
            this.labelConectaAlterado = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBancos)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridBancos, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 124);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 326);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridBancos
            // 
            this.dataGridBancos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridBancos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridBancos.Location = new System.Drawing.Point(3, 3);
            this.dataGridBancos.Name = "dataGridBancos";
            this.dataGridBancos.Size = new System.Drawing.Size(514, 320);
            this.dataGridBancos.TabIndex = 0;
            this.dataGridBancos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridBancos_CellClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnSalvarObservacaoTitulo, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtObservacao, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtTitulo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.titulo, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(523, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(274, 320);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Observação";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btnSalvarObservacaoTitulo
            // 
            this.btnSalvarObservacaoTitulo.Location = new System.Drawing.Point(3, 293);
            this.btnSalvarObservacaoTitulo.Name = "btnSalvarObservacaoTitulo";
            this.btnSalvarObservacaoTitulo.Size = new System.Drawing.Size(268, 24);
            this.btnSalvarObservacaoTitulo.TabIndex = 4;
            this.btnSalvarObservacaoTitulo.Text = "Salvar";
            this.btnSalvarObservacaoTitulo.UseVisualStyleBackColor = true;
            this.btnSalvarObservacaoTitulo.Click += new System.EventHandler(this.btnSalvarObservacaoTitulo_Click);
            // 
            // txtObservacao
            // 
            this.txtObservacao.Location = new System.Drawing.Point(3, 57);
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(268, 228);
            this.txtObservacao.TabIndex = 3;
            this.txtObservacao.Text = "";
            // 
            // txtTitulo
            // 
            this.txtTitulo.Location = new System.Drawing.Point(3, 18);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(268, 18);
            this.txtTitulo.TabIndex = 2;
            this.txtTitulo.Text = "";
            this.txtTitulo.TextChanged += new System.EventHandler(this.txtTitulo_TextChanged);
            // 
            // titulo
            // 
            this.titulo.AutoSize = true;
            this.titulo.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.titulo.Location = new System.Drawing.Point(3, 0);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(35, 13);
            this.titulo.TabIndex = 5;
            this.titulo.Text = "Título";
            // 
            // txtUsuarioInstancia
            // 
            this.txtUsuarioInstancia.Location = new System.Drawing.Point(77, 48);
            this.txtUsuarioInstancia.Name = "txtUsuarioInstancia";
            this.txtUsuarioInstancia.Size = new System.Drawing.Size(203, 20);
            this.txtUsuarioInstancia.TabIndex = 2;
            // 
            // txtSenhaInstancia
            // 
            this.txtSenhaInstancia.Location = new System.Drawing.Point(77, 74);
            this.txtSenhaInstancia.Name = "txtSenhaInstancia";
            this.txtSenhaInstancia.Size = new System.Drawing.Size(203, 20);
            this.txtSenhaInstancia.TabIndex = 3;
            // 
            // dsa
            // 
            this.dsa.AutoSize = true;
            this.dsa.Location = new System.Drawing.Point(21, 25);
            this.dsa.Name = "dsa";
            this.dsa.Size = new System.Drawing.Size(50, 13);
            this.dsa.TabIndex = 4;
            this.dsa.Text = "Instância";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Usuário";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Senha";
            // 
            // btnBuscarBanco
            // 
            this.btnBuscarBanco.Location = new System.Drawing.Point(311, 40);
            this.btnBuscarBanco.Name = "btnBuscarBanco";
            this.btnBuscarBanco.Size = new System.Drawing.Size(167, 34);
            this.btnBuscarBanco.TabIndex = 7;
            this.btnBuscarBanco.Text = "Buscar Bancos";
            this.btnBuscarBanco.UseVisualStyleBackColor = true;
            this.btnBuscarBanco.Click += new System.EventHandler(this.btnBuscarBanco_Click);
            // 
            // btnTrocarConecta
            // 
            this.btnTrocarConecta.Location = new System.Drawing.Point(528, 8);
            this.btnTrocarConecta.Name = "btnTrocarConecta";
            this.btnTrocarConecta.Size = new System.Drawing.Size(181, 30);
            this.btnTrocarConecta.TabIndex = 8;
            this.btnTrocarConecta.Text = "Trocar Conecta";
            this.btnTrocarConecta.UseVisualStyleBackColor = true;
            this.btnTrocarConecta.Click += new System.EventHandler(this.btnTrocarConecta_Click);
            // 
            // lblBancoSelecionado
            // 
            this.lblBancoSelecionado.AutoSize = true;
            this.lblBancoSelecionado.Location = new System.Drawing.Point(12, 108);
            this.lblBancoSelecionado.Name = "lblBancoSelecionado";
            this.lblBancoSelecionado.Size = new System.Drawing.Size(98, 13);
            this.lblBancoSelecionado.TabIndex = 9;
            this.lblBancoSelecionado.Text = "Banco selecionado";
            // 
            // btnIniciarInovafarma
            // 
            this.btnIniciarInovafarma.Location = new System.Drawing.Point(528, 74);
            this.btnIniciarInovafarma.Name = "btnIniciarInovafarma";
            this.btnIniciarInovafarma.Size = new System.Drawing.Size(181, 30);
            this.btnIniciarInovafarma.TabIndex = 10;
            this.btnIniciarInovafarma.Text = "Abrir Inovafarma";
            this.btnIniciarInovafarma.UseVisualStyleBackColor = true;
            this.btnIniciarInovafarma.Click += new System.EventHandler(this.btnIniciarInovafarma_Click);
            // 
            // cmbInstancia
            // 
            this.cmbInstancia.FormattingEnabled = true;
            this.cmbInstancia.Location = new System.Drawing.Point(77, 21);
            this.cmbInstancia.Name = "cmbInstancia";
            this.cmbInstancia.Size = new System.Drawing.Size(203, 21);
            this.cmbInstancia.TabIndex = 11;
            // 
            // labelConectaAlterado
            // 
            this.labelConectaAlterado.AutoSize = true;
            this.labelConectaAlterado.Location = new System.Drawing.Point(525, 38);
            this.labelConectaAlterado.Name = "labelConectaAlterado";
            this.labelConectaAlterado.Size = new System.Drawing.Size(165, 26);
            this.labelConectaAlterado.TabIndex = 12;
            this.labelConectaAlterado.Text = "Conecta log criado para o banco:\r\n{bancoSelecionado}";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelConectaAlterado);
            this.Controls.Add(this.cmbInstancia);
            this.Controls.Add(this.btnIniciarInovafarma);
            this.Controls.Add(this.lblBancoSelecionado);
            this.Controls.Add(this.btnTrocarConecta);
            this.Controls.Add(this.btnBuscarBanco);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dsa);
            this.Controls.Add(this.txtSenhaInstancia);
            this.Controls.Add(this.txtUsuarioInstancia);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "InovaConecta";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBancos)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridBancos;
        private System.Windows.Forms.TextBox txtUsuarioInstancia;
        private System.Windows.Forms.TextBox txtSenhaInstancia;
        private System.Windows.Forms.Label dsa;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBuscarBanco;
        private System.Windows.Forms.Button btnTrocarConecta;
        private System.Windows.Forms.Label lblBancoSelecionado;
        private System.Windows.Forms.Button btnIniciarInovafarma;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RichTextBox txtObservacao;
        private System.Windows.Forms.RichTextBox txtTitulo;
        private System.Windows.Forms.ComboBox cmbInstancia;
        private System.Windows.Forms.Label labelConectaAlterado;
        private System.Windows.Forms.Button btnSalvarObservacaoTitulo;
        private System.Windows.Forms.Label titulo;
        private System.Windows.Forms.Label label3;
    }
}

